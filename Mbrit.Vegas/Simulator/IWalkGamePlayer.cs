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

        void StartPlayingHand(int hand, IWinLoseDrawRound round, WalkArgs args, WalkState state);
        void StopPlayingHand(int hand, IWinLoseDrawRound round, WalkArgs args, WalkState state);

        bool CanPlayHand(int hand);

        IEnumerable<PlayerMove> GetNextMoves(int hand, IWinLoseDrawRound round, WalkState state);

        void BankedProfit(int hand, int currencyAmount, int units);

        void StretchFinished(WalkStretchFinishReason reason);
    }
}
