using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Simulator
{
    internal class EvaluationBucket
    {
        internal Dictionary<int, RoundEvaluation> Evaluations { get; } = new Dictionary<int, RoundEvaluation>();
        internal Dictionary<int, int> Stops { get; } = new Dictionary<int, int>();

        internal void AddEvaluation(IWinLoseDrawRound round, int hand, WalkState state, WalkOutcomesBucket bucket)
        {
            var eval = this.GetEvaluation(round);
            eval.SetOutcome(hand, bucket);
        }

        private RoundEvaluation GetEvaluation(IWinLoseDrawRound round)
        {
            if (!(this.Evaluations.ContainsKey(round.Index)))
                this.Evaluations[round.Index] = new RoundEvaluation();
            return this.Evaluations[round.Index];
        }

        internal void SetStop(IWinLoseDrawRound round, int hand)
        {
            var eval = this.GetEvaluation(round);
            eval.SetStop(hand);
        }
    }
}
