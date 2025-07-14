using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Simulator
{
    public class AutomaticWalkGamePlayer : IWalkGamePlayer
    {
        public void BankedProfit(int currencyAmount, int units)
        {
            // no-op...
        }

        public void BankedProfit(int hand, int currencyAmount, int units)
        {
            // no-op...
        }

        public bool CanPlayHand(int hand) => true;      // never stops outside of game rules...

        public IEnumerable<PlayerMove> GetNextMoves(int hand, IWinLoseDrawRound round, WalkState state) => new List<PlayerMove>();

        public void StartPlaying(IWinLoseDrawRound round, WalkArgs args, WalkState state)
        {
            // no-op...
        }

        public void StopPlaying(IWinLoseDrawRound round, WalkArgs args, WalkState state, WalkResult result)
        {
            // no-op...
        }

        public void StartPlayingHand(int hand, IWinLoseDrawRound round, WalkArgs args, WalkState state)
        {
            // no-op...
        }

        public void StopPlayingHand(int hand, IWinLoseDrawRound round, WalkArgs args, WalkState state)
        {
            // no-op...
        }

        public void StretchFinished(WalkStretchFinishReason reason)
        {
            // no-op...
        }
    }
}
