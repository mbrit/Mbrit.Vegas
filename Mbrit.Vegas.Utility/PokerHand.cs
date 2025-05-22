using System;
using System.Collections.Generic;
using System.Text;

namespace Mbrit.Vegas.Utility
{
    internal class PokerHand
    {
        public PokerHandRank Rank { get; }
        public List<int> RankValues { get; }
        public List<int> Kickers { get; }

        public PokerHand(PokerHandRank rank, List<int> rankValues, List<int> kickers)
        {
            Rank = rank;
            RankValues = rankValues;
            Kickers = kickers;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(this.Rank);

            if(this.RankValues.Any())
            {
                builder.Append(", rank: ");
                builder.AppendJoin(", ", this.RankValues);
            }

            /*
            if (this.Kickers.Any())
            {
                builder.Append(", kickers: ");
                builder.AppendJoin(", ", this.Kickers);
            }
            */

            return builder.ToString();
        }
    }
} 