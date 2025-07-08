using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    internal interface IWinLoseDrawRoundsBucket
    {
        int Count { get; }
        decimal HouseEdge { get; }
        IWinLoseDrawRound this[int index] { get; } 
    }
}
