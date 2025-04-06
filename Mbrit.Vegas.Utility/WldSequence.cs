using BootFX.Common;
using Mbrit.Vegas.Games;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    internal class WldSequence : IEnumerable<WinLoseDraw>
    {
        private IEnumerable<WinLoseDraw> Results { get; }

        //internal WldSequence(decimal winIncludingBonusPercentage, decimal bonusWinPercentage, decimal losePercentage, decimal drawPercentage, int hands, IRng rand)
        internal WldSequence(Game game, int hands, IRng rand)
        {
            if (game.WinPercentage + game.LosePercentage + game.DrawPercentage != 1)
                throw new InvalidOperationException("The percentages must sum to 1.");

            var overallWins = game.WinPercentage;

            var results = new List<WinLoseDraw>();
            while (results.Count < hands * 2)
            {
                foreach (var win in game.Wins)
                {
                    var numWins = (int)((win.Percentage * overallWins) * 100);
                    for (var index = 0; index < numWins; index++)
                        results.Add(new WinLoseDraw(WinLoseDrawType.Win, win));
                }

                var numLosses = (int)(game.LosePercentage * 100);
                for (var index = 0; index < numLosses; index++)
                    results.Add(new WinLoseDraw(WinLoseDrawType.Lose));

                var numDraws = (int)(game.DrawPercentage * 100);
                for (var index = 0; index < numDraws; index++)
                    results.Add(new WinLoseDraw(WinLoseDrawType.Draw));
            }

            if (results.Count < hands)
                throw new InvalidOperationException("Not enough results created.");

            var wash = rand.Next(20, 30);
            for (var index = 0; index < wash; index++)
                results.Shuffle();

            while (results.Count > hands)
            {
                var index = rand.Next(0, results.Count - 1);
                results.RemoveAt(index);
            }

            this.Results = results;
        }

        public IEnumerator<WinLoseDraw> GetEnumerator() => Results.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
