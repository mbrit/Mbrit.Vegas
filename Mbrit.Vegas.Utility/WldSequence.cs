using BootFX.Common;
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

        internal WldSequence(decimal winIncludingBonusPercentage, decimal bonusWinPercentage, decimal losePercentage, decimal drawPercentage, int hands, IRng rand)
        {
            if (winIncludingBonusPercentage + losePercentage + drawPercentage != 100)
                throw new InvalidOperationException("The percentages must sum to 100.");

            var results = new List<WinLoseDraw>();
            while (results.Count < hands)
            {
                for (var index = 0; index < (int)(winIncludingBonusPercentage - bonusWinPercentage) * 10; index++)
                    results.Add(WinLoseDraw.Win);

                for (var index = 0; index < (int)bonusWinPercentage * 10; index++)
                    results.Add(WinLoseDraw.Bonus);

                for (var index = 0; index < (int)losePercentage * 10; index++)
                    results.Add(WinLoseDraw.Lose);
                for (var index = 0; index < (int)drawPercentage * 10; index++)
                    results.Add(WinLoseDraw.Draw);
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
