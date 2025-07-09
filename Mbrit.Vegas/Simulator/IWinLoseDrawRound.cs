using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Simulator
{
    public interface IWinLoseDrawRound
    {
        int Index { get; }
        WinLoseDrawType GetResult(int hand);
    }
}
