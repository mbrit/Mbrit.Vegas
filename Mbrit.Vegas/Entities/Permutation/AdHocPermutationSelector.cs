using Mbrit.Vegas.Simulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas
{
    public class AdHocPermutationSelector : IPermutationSelector
    {
        public WalkGameMode Mode { get; }
        public int Investables { get; }
        public int Hands { get; }
        public int UnitSize { get; }

        public AdHocPermutationSelector(WalkGameMode mode, int investables, int hands, int unitSize)
        {
            this.Mode = mode;
            this.Investables = investables;
            this.Hands = hands;
            this.UnitSize = unitSize;
        }

        public WalkHailMary HailMaryMode => WalkHailMary.None;
    }
}
