namespace Mbrit.Vegas
{
    using System;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Data;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    using BootFX.Common;
    using BootFX.Common.Dto;
    using BootFX.Common.Data;
    using BootFX.Common.Entities;
    using BootFX.Common.Entities.Attributes;
    using Mbrit.Vegas.Simulator;
    using static System.Collections.Specialized.BitVector32;


    /// <summary>
    /// Defines the entity type for 'GameRuns'.
    /// </summary>
    [Serializable()]
    [Entity(typeof(GameRunCollection), "GameRuns")]
    public class GameRun : GameRunBase
    {
        
        /// <summary>
        /// Constructor.
        /// </summary>
        public GameRun()
        {
        }
        
        /// <summary>
        /// Deserialization constructor.
        /// </summary>
        protected GameRun(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }

        public string Currency => "$";

        public static GameRun CreateGameRun(WalkGameMode mode, int unit, int hailMaryCount, int investables, int hands, decimal houseEdge)
        {
            var now = DateTime.UtcNow;

            var builder = new StringBuilder();
            builder.Append("Evening Walk at Flamingo, ");
            builder.Append(now.ToString("d MMMM yyyy"));

            var item = new GameRun()
            {
                Name = builder.ToString(),
                Mode = mode,
                Unit = unit,
                HailMaryCount = hailMaryCount,
                Investables = investables,
                Hands = hands,
                HouseEdge = houseEdge,
                Token = VegasRuntime.GetToken(),
                CreatedUtc = now,
            };
            item.SaveChanges();

            return item;
        }

        public void Abandon()
        {
            if (this.IsAbandoned)
                throw new InvalidOperationException("Run is already abandoned.");

            this.IsAbandoned = true;
            this.AbandonedUtc = DateTime.UtcNow;
            this.SaveChanges();
        }

        public string FormatUnits(int units)
        {
            if (units == 1)
                return "1 Unit";
            else
                return units + " Units";
        }

        public void SetAction(int hand, WalkGameAction action, int units)
        {
            using (TransactionState txn = Database.StartTransaction())
            {
                try
                {
                    this.AssertCanPlayHand(hand);

                    // set...
                    this.IsWaitingOnDecision = true;
                    this.Action = action;
                    this.ActionUnits = units;
                    this.ActionUtc = DateTime.UtcNow;
                    this.SaveChanges();

                    // ok...
                    txn.Commit();
                }
                catch (Exception ex)
                {
                    txn.Rollback(ex);
                    throw new InvalidOperationException("The operation failed", ex);
                }
            }
        }

        private void AssertCanPlayHand(int hand)
        {
            // make sure we're doing the correct hand...
            var hands = this.GetHandsOrderedByHand();

            if (hand != hands.Count())
                throw new InvalidOperationException($"Hand '{hand}' cannot be played.");
        }

        public IEnumerable<GameRunHand> GetHandsOrderedByHand()
        {
            var filter = GameRunHand.CreateFilter();
            filter.Constraints.Add(v => v.GameRunId, this.GameRunId);
            filter.ApplySort(v => v.Hand);
            return filter.ExecuteEnumerable();
        }

        public string UnitsToFormattedCurrency(int units) => FormatCurrency(UnitsToCurrency(units));

        private string FormatCurrency(int currencyValue) => this.Currency + currencyValue;

        private int UnitsToCurrency(int units) => units * this.Unit;

        public void SetOutcome(int hand, WinLoseDrawType outcome)
        {
            using (TransactionState txn = Database.StartTransaction())
            {
                try
                {
                    this.AssertCanPlayHand(hand);

                    var item = new GameRunHand()
                    {
                        GameRun = this,
                        Hand = hand,
                        Outcome = outcome,
                        DateTimeUtc = DateTime.UtcNow,
                    };
                    item.SaveChanges();

                    this.IsWaitingOnDecision = false;
                    this.Action = WalkGameAction.None;
                    this.ActionUnits = 0;
                    this.SetDBNull("ActionUtc");
                    this.SaveChanges();

                    // ok...
                    txn.Commit();
                }
                catch (Exception ex)
                {
                    txn.Rollback(ex);
                    throw new InvalidOperationException("The operation failed", ex);
                }
            }
        }

        public int CurrencyToUnits(int currencyAmount) => currencyAmount / this.Unit;

        // this needs more science...
        public int Spike0p5Win => this.Unit * 6;
        public int Spike1Win => this.Unit * 12;
    }
}
