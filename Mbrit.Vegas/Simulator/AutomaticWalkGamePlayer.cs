using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Simulator
{
    public class AutomaticWalkGamePlayer : IWalkGamePlayer
    {
        public bool CanPlayHand(int hand) => true;      // never stops outside of game rules...

        public object GetDecision(int hand, IWinLoseDrawRound round, WalkArgs args, WalkState state)
        {
            return null;
        }

        public void StartPlaying(IWinLoseDrawRound round, WalkArgs args, WalkState state)
        {
            // no-op...
        }

        public void StopPlaying(IWinLoseDrawRound round, WalkArgs args, WalkState state, WalkResult result)
        {
            // no-op...
        }
    }
}
