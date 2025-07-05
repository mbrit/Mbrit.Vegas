using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    internal class WalkResult
    {
        internal int Bankroll { get; }
        internal int TotalWagered { get; }
        internal IEnumerable<Chain> WinChains { get; }
        internal IEnumerable<Chain> LossChains { get; }
        internal IEnumerable<WinLoseDrawType> Vectors { get; }
        internal WalkOutcome Outcome { get; }

        internal WalkResult(int bankroll, int totalWagered, IEnumerable<Chain> winChains, IEnumerable<Chain> lossChains, IEnumerable<WinLoseDrawType> vectors, WalkOutcome outcome)
        {
            this.Bankroll = bankroll;
            this.TotalWagered = totalWagered;
            this.WinChains = winChains.ToList();
            this.LossChains = lossChains.ToList();
            this.Vectors = vectors.ToList();
            this.Outcome = outcome;
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
    }
}
