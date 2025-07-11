
using Mbrit.Vegas.Simulator;

namespace Mbrit.Vegas.Utility
{
    internal class AllFoo
    {
        internal void DoMagic()
        {
            var rand = new Random(VegasRuntime.GetToken().GetHashCode());
            var rounds = WinLoseDrawRoundsBucket.GetAllWinLosePermutations(WalkGameDefaults.HandsPerRound, rand);

            Console.WriteLine("Done.");
        }
    }
}