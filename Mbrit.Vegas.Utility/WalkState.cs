using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    internal class WalkState : IWalkAdjuster
    {
        private WalkArgs Args { get; }
        internal int Unit { get; private set; }
        internal int Invested { get; private set; }
        private List<int> Investables { get; set; }
        internal bool IsAborted { get; private set; }

        internal WalkState(WalkArgs args)
        {
            this.Args = args;
            this.Unit = args.Unit;

            var investables = new List<int>();
            for (var index = 0; index < args.MaxPutIns; index++)
                investables.Add(this.Unit);
            this.Investables = investables;
        }

        internal bool HasInvestables => this.Investables.Any();

        internal int TakeProfit => (int)((decimal)this.Unit * this.Args.TakeProfitMultiplier);

        internal int PutIn()
        {
            if (!(this.HasInvestables))
                throw new InvalidOperationException("Nothing to invest.");

            var value = this.Investables.First();
            this.Investables.RemoveAt(0);

            this.Invested += value;

            return value;
        }

        public void PressUnit()
        {
            var adjustment = 2M;

            this.Unit = (int)Math.Floor(((decimal)this.Unit * adjustment));
            while (this.Unit % 5 != 0)
                this.Unit++;

            var newInvestables = new List<int>();
            foreach (var value in this.Investables)
                newInvestables.Add(this.Unit);
            this.Investables = newInvestables;
        }

        public void Abort() => this.IsAborted = true;
    }
}
