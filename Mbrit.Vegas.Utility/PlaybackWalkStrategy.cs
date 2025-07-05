using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    internal class PlaybackWalkStrategy : WalkStrategy
    {
        private List<WinLoseDrawType> Vectors { get; }
        private int Index { get; set; }

        internal PlaybackWalkStrategy(IEnumerable<WinLoseDrawType> vectors)
        {
            this.Vectors = new List<WinLoseDrawType>(vectors);
        }

        internal override WinLoseDrawType GetWin(Random rand)
        {
            var result = this.Vectors[this.Index];
            this.Index++;
            return result;
        }
    }
}
