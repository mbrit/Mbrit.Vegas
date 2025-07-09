using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Simulator
{
    public class WalkGameSetup : IWalkGameSetup
    {
        public IWinLoseDrawRoundsBucket Rounds { get; }

        internal string Name { get; }

        public int UnitSize { get; set; }

        internal Func<IWalkGameSetup, WalkArgs> GetArgsCallback { get; }

        // we need to be able to change this for the probability space evaluations...
        public int HandsPerRound { get; set; } = WalkGameDefaults.HandsPerRound;

        // fixed...
        public decimal TakeProfitMultiplier => 4;
        public int MaxPutIns => 12;

        //private const int DefaultMaxHands = 25;

        // adhoc...
        internal Dictionary<Type, object> Tags { get; } = new Dictionary<Type, object>();

        public WalkGameSetup(IWinLoseDrawRoundsBucket rounds, int unitSize, string name = null, Func<IWalkGameSetup, WalkArgs> getArgs = null)
        {
            this.Rounds = rounds;
            this.UnitSize = unitSize;
            this.Name = name;
            this.GetArgsCallback = getArgs;
        }

        public WalkArgs GetArgs()
        {
            var wrapper = new SetupWrapper(this);

            if (this.GetArgsCallback != null)
            {
                var args = this.GetArgsCallback(wrapper);
                if (!(args.HasName))
                    args.Name = this.Name;
                return args;
            }
            else
                return new WalkArgs(wrapper, this.Name);
        }

        internal class SetupWrapper : IWalkGameSetup
        {
            private WalkGameSetup Owner { get; }

            internal SetupWrapper(WalkGameSetup owner)
            {
                this.Owner = owner;
            }

            public WalkArgs GetArgs() => new WalkArgs(this);

            public int UnitSize => this.Owner.UnitSize;

            public decimal TakeProfitMultiplier => this.Owner.TakeProfitMultiplier;

            public decimal HouseEdge => this.Owner.HouseEdge;

            public int MaxPutIns => this.Owner.MaxPutIns;

            public int HandsPerRound => this.Owner.HandsPerRound;

            public IWinLoseDrawRoundsBucket Rounds => this.Owner.Rounds;
        }

        public decimal HouseEdge => this.Rounds.HouseEdge;

        internal int InitialInvestable => this.UnitSize * this.MaxPutIns;

        internal T GetTag<T>() => (T)this.Tags[typeof(T)];

        internal void SetTag<T>(T value) => this.Tags[typeof(T)] = value;
    }
}
