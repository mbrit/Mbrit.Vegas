using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Simulator
{
    internal class RandomWalkStrategy : WalkStrategy
    {
        private decimal HouseEdge { get; set; }

        internal RandomWalkStrategy(decimal houseEdge = 0.015M)
        {
            this.HouseEdge = houseEdge;
        }

        internal override WinLoseDrawType GetWin(Random rand)
        {
            //var result = rand.Next(1000) <= 485;

            /*
            var win = 500 - (int)(this.HouseEdge * 1000);
            var result = rand.Next(1000) <= win;
            if (result)
                return WinLoseDrawType.Win;
            else
                return WinLoseDrawType.Lose;
            */

            var win = SimulateHouseEdgeOutcome((double)this.HouseEdge, rand);
            //var win = PlayerWins(rand, (double)this.HouseEdge);
            if(win > 0)
                return WinLoseDrawType.Win;
            else
                return WinLoseDrawType.Lose;
        }

        public static bool PlayerWins(Random rng, double houseEdgePercent)
        {
            const int Resolution = 10000; // Granularity (1 = 0.01%)

            // Calculate win probability for a 1:1 payout under this edge
            double winProbability = (1.0 - houseEdgePercent / 100.0) / 2.0;

            // Convert to discrete threshold
            int winThreshold = (int)(winProbability * Resolution);

            // Roll between 1 and Resolution
            int roll = rng.Next(1, Resolution + 1);

            return roll <= winThreshold;
        }

        public static int SimulateHouseEdgeOutcome(double houseEdgePercent, Random rng)
        {
            // Define total possible values (1000 for fine-grain control)
            int totalRange = 1000;

            // Calculate how many values to remove due to house edge
            int edgeLosses = (int)(totalRange * (houseEdgePercent / 100.0));

            // Remaining range is fair play territory
            int fairRange = totalRange - edgeLosses;

            // Split fairly into win/loss
            int winThreshold = fairRange / 2; // e.g. 492 if fairRange is 985

            // Roll a random number
            int roll = rng.Next(1, totalRange + 1); // [1, 1000]

            // Evaluate outcome
            if (roll <= winThreshold)
                return +1; // Win
            else
                return -1; // Loss
        }
    }
}
