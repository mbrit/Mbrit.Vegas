using BootFX.Common.Management;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Simulator
{
    public class RoundsBucket<T> : Loggable
    {
        public decimal HouseEdge { get; }

        protected List<Round<T>> Rounds { get; set; }

        protected RoundsBucket(int numRounds, int handsPerRound, decimal houseEdge, Func<int, T> callback, Random rand)
        {
            this.HouseEdge = houseEdge;

            // this method intentionally returns way more values than needed, because this is
            // how it's "burning" cards and plays...

            this.LogInfo(() => $"Generating '{numRounds}' rounds...");

            var allVectors = new List<Round<T>>();

            var roundTarget = rand.Next(numRounds * 4, numRounds * 8);
            for (var round = 0; round < roundTarget; round++)
            {
                var vectors = new List<T>();

                var handTarget = rand.Next(handsPerRound * 4, handsPerRound * 8);
                for (var hand = 0; hand < handTarget; hand++)
                {
                    var result = callback(hand);
                    vectors.Add(result);
                }

                allVectors.Add(new Round<T>(round, vectors));
            }

            allVectors.Wash(rand);

            var top = allVectors.Take(numRounds);
            this.Initialize(top);

            this.LogInfo(() => "Rounds generated.");
        }

        public int Count => this.Rounds.Count;

        protected RoundsBucket(int numRounds, int handsPerRound, IEnumerable<Round<T>> vectors)
        {
            this.Initialize(vectors);
        }

        protected virtual void Initialize(IEnumerable<Round<T>> vectors)
        {
            this.Rounds = vectors.ToList();
        }
    }
}
