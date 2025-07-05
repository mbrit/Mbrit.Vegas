using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    internal delegate void WalkStepDelegate(int stepIndex, WinLoseDrawType win, IEnumerable<WinLoseDrawType> vectorsBefore, IWalkAdjuster adjuster);

    internal class WalkRunArgs
    {
        internal bool DoTrace { get; set; }
        internal WalkStrategy Strategy { get; set; } = new RandomWalkStrategy();
        internal WalkStepDelegate Step { get; set; }
    }
}
