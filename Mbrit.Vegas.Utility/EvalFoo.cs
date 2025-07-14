
using Mbrit.Vegas.Simulator;
using System.ComponentModel;

namespace Mbrit.Vegas.Utility
{
    internal class EvalFoo
    {
        internal void DoMagic()
        {
            //var key = "llllwllllllw";
            var key = "lllwwlwlwwwllwwwlww";
            //var round = WinLoseDrawExtender.GetRoundFromKey(key);

            // what game?
            var houseEdge = WalkGameDefaults.HouseEdge;
            var unitSize = 100;

            // where round...
            var hand = key.Length;            
            var playbackKey = key.Substring(0, hand);
            var playbackRound = WinLoseDrawExtender.GetRoundFromKey(playbackKey);
            var playbackRounds = playbackRound.WrapSingleton(houseEdge);

            // which hand are we at?
            var investables = 0;
            var handsPerRound = 0;
            var runs = new WalkFoo().DoMagic(playbackRounds, 1, new ManualPlayer(playbackRound, WalkGameMode.ReachSpike0p5), (index, bucket) =>
            {
                var setup = WalkGameDefaults.GetSetup(WalkGameMode.Unrestricted, playbackRounds, unitSize, 0);
                investables = setup.MaxPutIns;
                handsPerRound = setup.HandsPerRound;
                return setup;
            });

            var result = runs.First().Results.First();

            var eval = new WalkGameEvaluator();
            var pointOutcome = result.EndState.GetPointOutcome(hand, result.Args);
            eval.Evaluate(hand, playbackRound, pointOutcome, new AdHocPermutationSelector(WalkGameMode.Unrestricted,
                investables, handsPerRound, unitSize, houseEdge));
        }
    }
}