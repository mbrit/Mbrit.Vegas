using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Simulator
{
    public class WalkResult
    {
        internal int Bankroll { get; }
        internal int TotalWagered { get; }
        internal decimal ExpectedValuePerHundredCurrency { get; }
        internal IWinLoseDrawRound Round { get; }
        //internal IEnumerable<Chain> WinChains { get; }
        //internal IEnumerable<Chain> LossChains { get; }
        internal IEnumerable<WinLoseDrawType> Vectors { get; }
        internal WalkOutcome Outcome { get; }
        internal decimal WinChainScore { get; }
        internal decimal LossChainScore { get; }
        internal decimal ChainScore { get; }
        internal WalkStopReason StopReason { get; }
        internal WalkSpikeType SpikeType { get; }

        internal WalkResult(WalkState state, IWinLoseDrawRound round, IEnumerable<WinLoseDrawType> vectors, WalkOutcome outcome, WalkSpikeType spikeType, 
            bool didSeeMajorMust, bool didSeeMinorBust, bool didSeeSpike0p5, bool didSeeSpike1)
        {
            this.Bankroll = state.Profit;
            this.TotalWagered = state.TotalWagered;
            this.StopReason = state.StopReason;

            this.ExpectedValuePerHundredCurrency = (this.Bankroll / (decimal)state.TotalWagered) * 100M;

            this.Round = round;

            this.Vectors = vectors.ToList();
            this.Outcome = outcome;

            this.WinChainScore = state.WinChainScore;
            this.LossChainScore = state.LossChainScore;
            this.ChainScore = state.ChainScore;

            this.SpikeType = spikeType;
        }

        internal WinLoseDrawType WinLose
        {
            get
            {
                if (this.Bankroll <= 0)
                    return WinLoseDrawType.Lose;
                else
                    return WinLoseDrawType.Win;
            }
        }

        public bool DidBust => this.Outcome == WalkOutcome.MajorBust || this.Outcome == WalkOutcome.MinorBust;

        public bool IsSpike0p5 => this.Outcome == WalkOutcome.Spike0p5;

        public bool IsSpike1 => this.CheckSpike1OrBetterType(WalkSpikeType.One);

        public bool IsSpike1p5 => this.CheckSpike1OrBetterType(WalkSpikeType.OnePointFive);

        public bool IsSpike2 => this.CheckSpike1OrBetterType(WalkSpikeType.Two);

        public bool IsSpike3Plus => this.CheckSpike1OrBetterType(WalkSpikeType.ThreePlus);

        public int Index => this.Round.Index;

        private bool CheckSpike1OrBetterType(WalkSpikeType type) => this.Outcome == WalkOutcome.Spike1OrBetter && this.SpikeType == type;
    }
}
