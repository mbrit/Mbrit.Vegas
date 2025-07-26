using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Simulator
{
    public class WalkResult
    {
        public WalkState EndState { get; }
        public WalkArgs Args { get; }

        internal int Bankroll { get; }
        internal int TotalWagered { get; }
        internal int Invested { get; }
        internal decimal ExpectedValuePerHundredCurrency { get; }
        internal IWinLoseDrawRound Round { get; }
        //internal IEnumerable<Chain> WinChains { get; }
        //internal IEnumerable<Chain> LossChains { get; }
        internal IEnumerable<WinLoseDrawType> Vectors { get; }
        public WalkGameOutcome Outcome { get; }
        internal decimal WinChainScore { get; }
        internal decimal LossChainScore { get; }
        internal decimal ChainScore { get; }
        internal WalkStopReason StopReason { get; }
        internal WalkSpikeType SpikeType { get; }

        internal WalkPointOutcome PointOutcomeMajorBust { get; }
        internal WalkPointOutcome PointOutcomeMinorBust { get; }
        internal WalkPointOutcome PointOutcomeSpike0p5 { get; }
        internal WalkPointOutcome PointOutcomeSpike1 { get; }

        public Dictionary<int, WalkPointOutcome> PointOutcomes { get; }

        internal WalkResult(WalkState state, WalkArgs args, IWinLoseDrawRound round, IEnumerable<WinLoseDrawType> vectors, WalkGameOutcome outcome, WalkSpikeType spikeType, 
            WalkPointOutcome pointOutcomeMajorBust, WalkPointOutcome pointOutcomeMinorBust, WalkPointOutcome pointOutcomeSpike0p5, 
            WalkPointOutcome pointOutcomeSpike1, Dictionary<int, WalkPointOutcome> pointOutcomes)
        {
            this.EndState = state;
            this.Args = args;

            this.Bankroll = state.Profit;
            this.TotalWagered = state.TotalWagered;
            this.StopReason = state.StopReason;
            this.Invested = state.Invested;

            if(state.TotalWagered != 0)
                this.ExpectedValuePerHundredCurrency = (this.Bankroll / (decimal)state.TotalWagered) * 100M;

            this.Round = round;

            this.Vectors = vectors.ToList();
            this.Outcome = outcome;

            this.WinChainScore = state.WinChainScore;
            this.LossChainScore = state.LossChainScore;
            this.ChainScore = state.ChainScore;

            this.SpikeType = spikeType;

            this.PointOutcomeMajorBust = pointOutcomeMajorBust;
            this.PointOutcomeMinorBust = pointOutcomeMinorBust;
            this.PointOutcomeSpike0p5 = pointOutcomeSpike0p5;
            this.PointOutcomeSpike1 = pointOutcomeSpike1;

            this.PointOutcomes = new Dictionary<int, WalkPointOutcome>(pointOutcomes);
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

        public bool DidBust => this.Outcome == WalkGameOutcome.MajorBust || this.Outcome == WalkGameOutcome.MinorBust;

        public bool IsSpike0p5 => this.Outcome == WalkGameOutcome.Spike0p5;

        public bool IsSpike1 => this.CheckSpike1OrBetterType(WalkSpikeType.One);

        public bool IsSpike1p5 => this.CheckSpike1OrBetterType(WalkSpikeType.OnePointFive);

        public bool IsSpike2 => this.CheckSpike1OrBetterType(WalkSpikeType.Two);

        public bool IsSpike3Plus => this.CheckSpike1OrBetterType(WalkSpikeType.ThreePlus);

        public int Index => this.Round.Index;

        public string Key => this.Vectors.GetKey();

        private bool CheckSpike1OrBetterType(WalkSpikeType type) => this.Outcome == WalkGameOutcome.Spike1OrBetter && this.SpikeType == type;
    }
}
