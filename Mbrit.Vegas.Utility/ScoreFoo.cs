
using BootFX.Common;
using Mbrit.Vegas.Simulator;

namespace Mbrit.Vegas.Utility
{
    internal class ScoreFoo
    {
        internal void DoMagic()
        {
            var rand = VegasRuntime.GetRng();
            var houseEdge = WalkGameDefaults.HouseEdge;
            Console.WriteLine("House edge --> " + houseEdge);
            var bucket = WinLoseDrawRoundsBucket.GetWinLoseBucket(10, 15, houseEdge, rand);

            var table = new ConsoleTable();
            foreach(var round in bucket.ToEnumerable())
            {
                var row = table.AddRow(round.GetKey());

                var score = WalkOutcomesBucket.GetRoundScore(round, houseEdge);
                row.AddCell(score);
            }

            table.Render();
        }
    }
}