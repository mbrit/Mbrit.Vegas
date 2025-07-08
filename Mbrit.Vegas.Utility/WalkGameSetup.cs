using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    internal class WalkGameSetup
    {
        internal IWinLoseDrawRoundsBucket Rounds { get; }

        internal string Name { get; }

        internal int Unit { get; set; } = 100;

        internal Func<WalkArgs> GetArgsCallback { get; }

        // we need to be able to change this for the probability space evaluations...
        internal int HandsPerRound { get; set; } = DefaultMaxHands;

        // fixed...
        internal decimal TakeProfitMultiplier => 4;
        internal int MaxPutIns => 12;

        private const int DefaultMaxHands = 25;

        // adhoc...
        internal Dictionary<Type, object> Tags { get; } = new Dictionary<Type, object>();

        internal WalkGameSetup(IWinLoseDrawRoundsBucket rounds, string name = null, Func<WalkArgs> getArgs = null)
        {
            this.Rounds = rounds;
            this.Name = name;
            this.GetArgsCallback = getArgs;
        }

        internal WalkArgs GetArgs()
        {
            if (this.GetArgsCallback != null)
                return this.GetArgsCallback();
            else
                return new WalkArgs(this, this.Name);
        }

        internal decimal HouseEdge => this.Rounds.HouseEdge;

        internal int InitialInvestable => this.Unit * this.MaxPutIns;

        internal T GetTag<T>() => (T)this.Tags[typeof(T)];

        internal void SetTag<T>(T value) => this.Tags[typeof(T)] = value;
    }
}
