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

        internal WalkPointOutcome(decimal profit, decimal totalWagered, decimal evPer100Currency)
        {
            this.Profit = profit;
            this.TotalWagered = totalWagered;
            this.EvPer100Currency = evPer100Currency;
        }
    }
}
