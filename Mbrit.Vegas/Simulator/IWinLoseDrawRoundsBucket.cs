using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Simulator
{
    public interface IWinLoseDrawRoundsBucket
    {
        int Count { get; }
        decimal HouseEdge { get; }
        IWinLoseDrawRound this[int index] { get; }

        IEnumerable<IWinLoseDrawRound> ToEnumerable();
    }
}
