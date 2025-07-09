using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Simulator
{
    internal delegate void WalkStepDelegate(int stepIndex, WinLoseDrawType win, IEnumerable<WinLoseDrawType> vectorsBefore, IWalkAdjuster adjuster);
    internal delegate void WalkFinishedDelegate(WalkState state, decimal final);
    internal delegate WalkState WalkCreateStateDelegate(WalkArgs args);

    public class WalkRunArgs
    {
        internal bool DoTrace { get; set; }

        internal WalkStepDelegate Step { get; set; }
        internal WalkFinishedDelegate Finished { get; set; }
        internal WalkCreateStateDelegate CreateState { get; set; }
    }
}
