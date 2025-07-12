
using BootFX.Common.Management;
using System.Security.Principal;

namespace Mbrit.Vegas.Simulator
{
    internal class PermutationStrategy : Loggable
    {
        private int HandsPerRound { get; }
        internal int Permutations { get; }

        internal PermutationStrategy(int handsPerRound)
        {
            this.HandsPerRound = handsPerRound;
            this.Permutations = (int)Math.Pow(2, handsPerRound);
        }

        internal IEnumerable<Round<WinLoseDrawType>> GetVectors(Random rand)
        {
            var results = new List<Round<WinLoseDrawType>>();

            var logAt = DateTime.UtcNow.AddSeconds(2);
            var didLog = false;

            for(var index = 0; index < this.Permutations; index++)
            {
                var asBinary = index.ToString("b0");

                if (asBinary.Length < this.HandsPerRound)
                {
                    var prefix = new string('0', this.HandsPerRound - asBinary.Length);
                    asBinary = prefix + asBinary;
                }

                var vectors = new List<WinLoseDrawType>();
                foreach(var c in asBinary)
                {
                    if (c == '1')
                        vectors.Add(WinLoseDrawType.Win);
                    else if(c == '0')
                        vectors.Add(WinLoseDrawType.Lose);
                    else
                        throw new NotSupportedException($"Cannot handle '{c}'.");
                }

                // we need the results in reverse orders as the binary sequence is little endian and
                // we need big endian keys...
                vectors.Reverse();
                results.Add(new Round<WinLoseDrawType>(index, vectors));
            
                if(DateTime.UtcNow >= logAt)
                {
                    this.LogInfo(() => $"Created '{index + 1:n0}' permutations of '{this.Permutations:n0}'...");
                    logAt = DateTime.UtcNow.AddSeconds(2);
                    didLog= true;
                }
            }

            if(didLog)
                this.LogInfo(() => "Finished generating permutaions.");

            return results;
        }
    }
}