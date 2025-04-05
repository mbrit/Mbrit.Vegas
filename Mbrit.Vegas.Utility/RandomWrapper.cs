using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    internal class RandomWrapper : IRng
    {
        private Random Random { get; }

        internal RandomWrapper(Random rand = null)
        {
            rand ??= new Random();
            this.Random = rand;
        }

        public int Next(int minInclusive, int maxInclusive) => this.Random.Next(minInclusive, maxInclusive + 1);
    }
}
