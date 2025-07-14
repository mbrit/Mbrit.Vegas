using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Simulator
{
    public class WalkPointOutcome
    {
        public decimal Profit { get; }
        public decimal TotalWagered { get; }
        public decimal EvPer100Currency { get; }
        public int Investable { get; }
        public int Bankroll { get; }
        public int Banked { get; }
        public WalkGameOutcome Outcome { get; }
        public int Hand { get; }

        internal WalkPointOutcome(decimal profit, decimal totalWagered, decimal evPer100Currency, int investable, int bankroll, int banked, 
            WalkGameOutcome outcome, int hand)
        {
            this.Profit = profit;
            this.TotalWagered = totalWagered;
            this.EvPer100Currency = evPer100Currency;
            this.Investable = investable;
            this.Bankroll = bankroll;
            this.Banked = banked;
            this.Outcome = outcome;
            this.Hand = hand;
        }
    }
}
