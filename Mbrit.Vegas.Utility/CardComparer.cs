using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    internal class CardComparer : Comparer<Card>
    {
        public override int Compare(Card x, Card y)
        {
            var a = this.GetRank(x.Rank);
            var b = this.GetRank(y.Rank);

            return b.CompareTo(a);  
        }

        private int GetRank(int rank)
        {
            if (rank == 1)
                rank = 14;
            return rank;
        }
    }
}
