using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Simulator
{
    public interface IWalkGamePlayer
    {
        void StartPlaying(IWinLoseDrawRound round, WalkArgs args, WalkState state);
        void StopPlaying(IWinLoseDrawRound round, WalkArgs args, WalkState state, WalkResult result);

        object GetDecision(int hand, IWinLoseDrawRound round, WalkArgs args, WalkState state);

        bool CanPlayHand(int hand);
    }
}
