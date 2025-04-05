using BootFX.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    internal class GameResult : IEnumerable<GameResultItem>
    {
        private decimal StartingBank { get; }
        private IEnumerable<GameResultItem> Hands { get; } = new List<GameResultItem>();
        internal GameOutcome Outcome { get; set; } = GameOutcome.PlayedThrough;

        internal GameResult(decimal startingBank)
        {
            this.StartingBank = startingBank;
        }

        internal void AddHand(WinLoseDraw result, decimal bankAfter, int trackAfter) => 
            ((List<GameResultItem>)this.Hands).Add(new GameResultItem(result, bankAfter, trackAfter));

        public IEnumerator<GameResultItem> GetEnumerator() => Hands.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        internal decimal FinalBank
        {
            get
            {
                if (this.Any())
                    return this.Last().BankAfter;
                else
                    return 0;
            }
        }

        internal int FinalTrack
        {
            get
            {
                if (this.Any())
                    return this.Last().TrackAfter;
                else
                    return 0;
            }
        }

        private IEnumerable<GameResultItem> Wins => this.Hands.Where(v => v.Result == WinLoseDraw.Win).ToList();

        private IEnumerable<GameResultItem> WinOrBonuses => this.Hands.Where(v => v.Result == WinLoseDraw.Win || v.Result == WinLoseDraw.Bonus).ToList();

        private IEnumerable<GameResultItem> Bonuses => this.Hands.Where(v => v.Result == WinLoseDraw.Bonus).ToList();

        private IEnumerable<GameResultItem> Losses => this.Hands.Where(v => v.Result == WinLoseDraw.Lose).ToList();

        private IEnumerable<GameResultItem> Draws => this.Hands.Where(v => v.Result == WinLoseDraw.Draw).ToList();

        public int NumHands => this.Hands.Count();

        public int NumWins => this.Wins.Count();

        public int NumWinOrBonuses => this.WinOrBonuses.Count();

        public int NumBonuses => this.Bonuses.Count();

        public int NumLosses => this.Losses.Count();

        public int NumDraws => this.Draws.Count();

        public decimal PercentWins => this.GetPercentage(this.NumWins);

        public decimal PercentWinOrBonuses => this.GetPercentage(this.NumWinOrBonuses);

        public decimal PercentBonuses => this.GetPercentage(this.NumBonuses);

        public decimal PercentLosses => this.GetPercentage(this.NumLosses);

        public decimal PercentDraws => this.GetPercentage(this.NumDraws);

        private decimal GetPercentage(int numDraws)
        {
            if (this.Hands.Any())
                return (decimal)numDraws / (decimal)this.Hands.Count();
            else
                return 0;
        }

        public decimal FinalProfit => this.FinalBank - this.StartingBank;

        public int HighestTrack => this.Max(v => v.TrackAfter);

        public int LowestTrack => this.Min(v => v.TrackAfter);
    }
}
