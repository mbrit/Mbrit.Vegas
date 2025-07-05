using BootFX.Common.Management;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    internal class RoundsBucket<T> : Loggable, IEnumerable<Round<T>>
    {
        private IEnumerable<Round<T>> Rounds { get; }

        protected RoundsBucket(int numRounds, int handsPerRound, Func<T> callback, Random rand)
        {
            // this method intentionally returns way more values than needed, because this is
            // how it's "burning" cards and plays...

            this.LogInfo(() => "Generating rounds...");

            var allVectors = new List<Round<T>>();

            var roundTarget = rand.Next(numRounds * 4, numRounds * 8);
            for (var round = 0; round < roundTarget; round++)
            {
                var vectors = new List<T>();

                var handTarget = rand.Next(handsPerRound * 4, handsPerRound * 8);
                for (var hand = 0; hand < handTarget; hand++)
                {
                    var result = callback();
                    vectors.Add(result);
                }

                allVectors.Add(new Round<T>(vectors));
            }

            allVectors.Wash(rand);

            this.Rounds = allVectors;

            this.LogInfo(() => "Rounds generated.");
        }

        public IEnumerator<Round<T>> GetEnumerator() => this.Rounds.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
