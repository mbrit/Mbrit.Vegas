using BootFX.Common;
using Mbrit.Vegas.Web.Api.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Simulator
{
    public class ManualPlayer : IWalkGamePlayer
    {
        private WalkGameMode Mode { get; }
        private IWinLoseDrawRound Rounds { get; }

        public EventHandler<BankedProfitEventArgs> BankedProfit;
        public EventHandler<EventArgs<int>> HandPlaying;
        public EventHandler<EventArgs<int>> HandPlayed;

        internal ManualPlayer(IWinLoseDrawRound round, WalkGameMode mode)
        {
            this.Rounds = round;
            this.Mode = mode;
        }

        public bool CanPlayHand(int hand) => hand < this.Rounds.Count;

        // the initial player can only ever put in...
        public IEnumerable<PlayerMove> GetNextMoves(int hand, IWinLoseDrawRound round, WalkState state)
        {
            var moves = new List<PlayerMove>();
            if (hand == 0)
                moves.Add(new PlayerPutInMove());
            else
            {
                if (state.BankrollUnits != 0)
                    moves.Add(new PlayerPlayMove(state.BankrollUnits));
                else
                    moves.Add(new PlayerPutInMove());

                if (((this.Mode == WalkGameMode.StretchToSpike1 || this.Mode == WalkGameMode.ReachSpike0p5) && state.Profit >= state.Args.Spike0p5Win) ||
                    state.Profit >= state.Args.Spike1Win)
                {
                    moves.Add(new PlayerWalkMove());
                }
            }

            return moves;
        }

        public void StartPlaying(IWinLoseDrawRound round, WalkArgs args, WalkState state)
        {
            // no-op...
        }

        public void StopPlaying(IWinLoseDrawRound round, WalkArgs args, WalkState state, WalkResult result)
        {
            // no-op...
        }

        void IWalkGamePlayer.BankedProfit(int hand, int currencyAmount, int units) => this.OnBankedProfit(new BankedProfitEventArgs(hand, currencyAmount, units));

        void IWalkGamePlayer.StartPlayingHand(int hand, IWinLoseDrawRound round, WalkArgs args, WalkState state) => 
            this.OnHandPlaying(new EventArgs<int>(hand));

        void IWalkGamePlayer.StopPlayingHand(int hand, IWinLoseDrawRound round, WalkArgs args, WalkState state) =>
            this.OnHandPlayed(new EventArgs<int>(hand));

        private void OnBankedProfit(BankedProfitEventArgs e)
        {
            if (this.BankedProfit != null)
                this.BankedProfit(this, e);
        }

        private void OnHandPlayed(EventArgs<int> e)
        {
            if (this.HandPlayed != null)
                this.HandPlayed(this, e);
        }

        private void OnHandPlaying(EventArgs<int> e)
        {
            if (this.HandPlaying != null)
                this.HandPlaying(this, e);
        }
    }
}
