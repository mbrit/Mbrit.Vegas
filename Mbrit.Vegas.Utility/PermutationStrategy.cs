
namespace Mbrit.Vegas.Utility
{
    internal class PermutationStrategy
    {
        private int HandsPerRound { get; }
        internal int Permutations { get; }

        internal PermutationStrategy(int handsPerRound)
        {
            this.HandsPerRound = handsPerRound;
            this.Permutations = (int)Math.Pow(2, handsPerRound);
        }

        internal IEnumerable<Round<char>> GetVectors(int maxPermutationSampleSize, Random rand)
        {
            if (maxPermutationSampleSize > this.Permutations)
                maxPermutationSampleSize = 0;

            var per = 0;
            if (maxPermutationSampleSize > 0)
            {
                per = (int)((decimal)this.Permutations / (decimal)maxPermutationSampleSize);
                if (per == 0)
                    per = 1;
            }

            var results = new List<Round<char>>();
            for(var index = 0; index < this.Permutations; index++)
            {
                var toAdd = false;
                if(per > 0)
                {
                    var select = rand.Next(0, per - 1);
                    toAdd = select == 0;
                }
                else
                    toAdd = true;

                if (toAdd)
                {
                    var asBinary = index.ToString("b0");

                    if (asBinary.Length < this.HandsPerRound)
                    {
                        var prefix = new string('0', this.HandsPerRound - asBinary.Length);
                        asBinary = prefix + asBinary;
                    }

                    results.Add(new Round<char>(asBinary.ToCharArray()));
                }
            }

            if (maxPermutationSampleSize > 0)
            {
                // if this is a real problem, fix it then...
                //while(results.Count < maxPermutationSampleSize)
                //{
                //    throw new NotImplementedException("This operation has not been implemented.");
                //}

                while (results.Count > maxPermutationSampleSize)
                {
                    var index = rand.Next(0, results.Count - 1);
                    results.RemoveAt(index);
                }
            }

            return results;
        }
    }
}