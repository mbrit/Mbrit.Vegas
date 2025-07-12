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


    /// <summary>
    /// Defines the entity type for 'Permutations'.
    /// </summary>
    [Serializable()]
    [Entity(typeof(PermutationCollection), "Permutations")]
    public class Permutation : PermutationBase
    {
        
        /// <summary>
        /// Constructor.
        /// </summary>
        public Permutation()
        {
        }
        
        /// <summary>
        /// Deserialization constructor.
        /// </summary>
        protected Permutation(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }

        public static void AddPermutation(WalkResult result, IPermutationSelector selector)
        {
            if (selector == null)
                throw new ArgumentNullException("selector");

            if(selector.HailMaryMode != WalkHailMary.None)
                throw new NotSupportedException($"Cannot handle '{selector.HailMaryMode}'.");

            var key = result.Key;

            var item = new Permutation()
            {
                CreatedUtc = DateTime.UtcNow,
                Key = key,
                Mode = selector.Mode,
                Hands = selector.Hands,
                Investables = selector.Investables,
                UnitSize = selector.UnitSize,
                Profit = result.Bankroll,
                Invested = result.Invested,
                Wagered = result.TotalWagered,
                Ev = result.ExpectedValuePerHundredCurrency,
                Outcome = result.Outcome
            };

            item.ApplyPointOutcome(PermutationOutcomeType.MajorBust, result.PointOutcomeMajorBust);
            item.ApplyPointOutcome(PermutationOutcomeType.MinorBust, result.PointOutcomeMinorBust);
            item.ApplyPointOutcome(PermutationOutcomeType.Spike0p5, result.PointOutcomeSpike0p5);
            item.ApplyPointOutcome(PermutationOutcomeType.Spike1, result.PointOutcomeSpike1);

            item.SaveChanges();
        }

        private void ApplyPointOutcome(PermutationOutcomeType type, WalkPointOutcome pointOutcome)
        {
            var fields = GetPointOutcomeFields(type);

            if(pointOutcome != null)
            {
                this[fields.Seen] = true;
                this[fields.Hand] = pointOutcome.Hand;
                this[fields.Profit] = pointOutcome.Profit;
                this[fields.Wagered] = pointOutcome.TotalWagered;
                this[fields.Ev] = pointOutcome.EvPer100Currency;
            }
            else
            {
                this[fields.Seen] = false;
                this[fields.Hand] = -1;
                this[fields.Profit] = 0M;
                this[fields.Wagered] = 0M;
                this[fields.Ev] = 0M;
            }
        }

        private PermutationPointOutcomeFields GetPointOutcomeFields(PermutationOutcomeType type)
        {
            switch(type)
            {
                case PermutationOutcomeType.MajorBust:
                    return new PermutationPointOutcomeFields(
                        v => v.MajorBustSeen,
                        v => v.MajorBustHand,
                        v => v.MajorBustProfit,
                        v => v.MajorBustWagered,
                        v => v.MajorBustEv);

                case PermutationOutcomeType.MinorBust:
                    return new PermutationPointOutcomeFields(
                        v => v.MinorBustSeen,
                        v => v.MinorBustHand,
                        v => v.MinorBustProfit,
                        v => v.MinorBustWagered,
                        v => v.MinorBustEv);

                case PermutationOutcomeType.Spike0p5:
                    return new PermutationPointOutcomeFields(
                        v => v.Spike0p5Seen,
                        v => v.Spike0p5Hand,
                        v => v.Spike0p5Profit,
                        v => v.Spike0p5Wagered,
                        v => v.Spike0p5Ev);


                case PermutationOutcomeType.Spike1:
                    return new PermutationPointOutcomeFields(
                        v => v.Spike1Seen,
                        v => v.Spike1Hand,
                        v => v.Spike1Profit,
                        v => v.Spike1Wagered,
                        v => v.Spike1Ev);

                default:
                    throw new NotSupportedException($"Cannot handle '{type}'.");

            }
        }

        public static IEnumerable<string> GetKeys(IPermutationSelector selector)
        {
            var builder = new SqlBuilder<Permutation>();
            builder.Append("select ");
            builder.AppendField(v => v.Key);
            builder.Append(" from ");
            builder.AppendTable();
            builder.Append(" where ");
            builder.AppendFieldAndValue(v => v.Mode, selector.Mode);
            builder.Append(" and ");
            builder.AppendFieldAndValue(v => v.Hands, selector.Hands);
            builder.Append(" and ");
            builder.AppendFieldAndValue(v => v.UnitSize, selector.UnitSize);
            builder.Append(" and ");
            builder.AppendFieldAndValue(v => v.Investables, selector.Investables);

            if(selector.HailMaryMode == WalkHailMary.None)
            {
                builder.Append(" and ");
                builder.AppendField(v => v.HailMaryMap);
                builder.Append(" is null");
            }
            else
                throw new NotSupportedException($"Cannot handle '{selector.HailMaryMode}'.");

            return Database.ExecuteValuesVertical<string>(builder);
        }
    }
}
