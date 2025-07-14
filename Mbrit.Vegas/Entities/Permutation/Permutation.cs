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
    using BootFX.Common.Management;

    public record PermutationIdAndKey(int Id, string Key);

    /// <summary>
    /// Defines the entity type for 'Permutations'.
    /// </summary>
    [Serializable()]
    [Entity(typeof(PermutationCollection), "Permutations")]
    public class Permutation : PermutationBase, IPermutation
    {
        // by convention, database decimals are always decimal(18,5) -- this preserves precision, without
        // anyone having to remember that that one field out of thousands of decimals field across
        // hundreds of projects has differnet precisions...
        internal const decimal ScoreAdjust = 10000M;

        private const int EncodedDecimalDp = 1000;
        private const int EncodedDecimalLength = 9;

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
                HouseEdge = selector.HouseEdge,
                Score = WalkOutcomesBucket.GetRoundScore(result.Round, selector.HouseEdge) * ScoreAdjust,       // deal with 18,5 decimals...
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

            var builder = new StringBuilder();
            var hand = 0;
            while(true)
            {
                if (!(result.PointOutcomes.ContainsKey(hand)))
                    break;

                if (builder.Length > 0)
                    builder.Append("$");

                var outcome = result.PointOutcomes[hand];

                if (outcome.Outcome == WalkGameOutcome.MajorBust)
                    builder.Append("MB");
                else if (outcome.Outcome == WalkGameOutcome.MinorBust)
                    builder.Append("NB");
                else if (outcome.Outcome == WalkGameOutcome.Evens)
                    builder.Append("EV");
                else if (outcome.Outcome == WalkGameOutcome.Spike0p5)
                    builder.Append("S0");
                else if (outcome.Outcome == WalkGameOutcome.Spike1OrBetter)
                    builder.Append("S1");
                else
                    throw new NotSupportedException($"Cannot handle '{outcome.Outcome}'.");

                Action<decimal> append = (value) =>
                {
                    builder.Append("|");
                    var asString = Math.Round(value, 3);
                    builder.Append(asString);
                };

                append(outcome.Profit);
                append(outcome.EvPer100Currency);
                append(outcome.Investable);
                append(outcome.Bankroll);
                append(outcome.Banked);

                hand++;
            }

            item.Pattern = builder.ToString();

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

        private static SqlFilter<Permutation> GetPermutationFilter(IPermutationSelector selector)
        {
            var filter = new SqlFilter<Permutation>();
            filter.Constraints.Add(v => v.Mode, selector.Mode);
            filter.Constraints.Add(v => v.Hands, selector.Hands);
            filter.Constraints.Add(v => v.UnitSize, selector.UnitSize);
            filter.Constraints.Add(v => v.Investables, selector.Investables);
            filter.Constraints.Add(v => v.HouseEdge, selector.HouseEdge);

            if (selector.HailMaryMode == WalkHailMary.None)
                filter.Constraints.AddIsNullConstraint(v => v.HailMaryMap);
            else
                throw new NotSupportedException($"Cannot handle '{selector.HailMaryMode}'.");

            return filter;
        }

        public static IEnumerable<PermutationIdAndKey> GetKeysAndIdPairs(IPermutationSelector selector)
        {
            var builder = new SqlBuilder<Permutation>();
            builder.Append("select ");
            builder.AppendField(v => v.PermutationId);
            builder.Append(", ");
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
            builder.Append(" and ");
            builder.AppendFieldAndValue(v => v.HouseEdge, selector.HouseEdge);

            if (selector.HailMaryMode == WalkHailMary.None)
            {
                builder.Append(" and ");
                builder.AppendField(v => v.HailMaryMap);
                builder.Append(" is null");
            }
            else
                throw new NotSupportedException($"Cannot handle '{selector.HailMaryMode}'.");

            var table = Database.ExecuteDataTable(builder);

            var results = new List<PermutationIdAndKey>();
            foreach (DataRow row in table.Rows)
                results.Add(new PermutationIdAndKey((int)row["permutationid"], (string)row["key"]));

            return results;
        }

        internal static IEnumerable<Permutation> GetByPrefix(string key, IPermutationSelector selector)
        {
            var filter = GetPermutationFilter(selector);
            filter.Constraints.Add(v => v.Key, SqlOperator.StartsWith, key + "%");
            return filter.ExecuteEnumerable();
        }

        public decimal ResolvedScore => this.Score / ScoreAdjust;
    }
}
