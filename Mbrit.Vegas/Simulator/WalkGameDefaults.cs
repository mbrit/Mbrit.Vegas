using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Simulator
{
    public static class WalkGameDefaults
    {
        public const int Investables = 12;
        public const int HandsPerRound = 25;
        public const decimal HouseEdge = 0.015M;
        //public const decimal HouseEdge = 0.005M;

        public static WalkGameSetup GetSetup(WalkGameMode mode, IWinLoseDrawRoundsBucket rounds, int unitSize, int hailMaryCount)
        {
            if (mode == WalkGameMode.ReachSpike0p5)
            {
                return new WalkGameSetup(rounds, unitSize, "Reach Spike 0.5x", (setup) =>
                {
                    var args = setup.GetArgs();
                    args.StopAtWinMode = WalkGameMode.ReachSpike0p5;
                    args.SetHailMary(hailMaryCount);
                    return args;
                });
            }
            else if (mode == WalkGameMode.StretchToSpike1)
            {
                return new WalkGameSetup(rounds, unitSize, "Stretch to Spike 1x", (setup) =>
                {
                    var args = setup.GetArgs();
                    args.SetStretchToSpike1();
                    args.SetHailMary(hailMaryCount);
                    return args;
                });
            }
            else if (mode == WalkGameMode.ReachSpike1)
            {
                return new WalkGameSetup(rounds, unitSize, "Reach Spike 1x", (setup) =>
                {
                    var args = setup.GetArgs();
                    args.StopAtWinMode = WalkGameMode.ReachSpike1;
                    args.SetHailMary(hailMaryCount);
                    return args;
                });
            }
            else if (mode == WalkGameMode.Unrestricted)
            {
                return new WalkGameSetup(rounds, unitSize, "Unrestricted", (setup) =>
                {
                    var args = setup.GetArgs();
                    args.StopAtWinMode = WalkGameMode.Unrestricted;
                    args.SetHailMary(hailMaryCount);
                    return args;
                });
            }
            else
                throw new NotSupportedException($"Cannot handle '{mode}'.");
        }
    }
}
