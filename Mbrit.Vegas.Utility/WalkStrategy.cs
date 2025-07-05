using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    internal abstract class WalkStrategy
    {
        internal abstract WinLoseDrawType GetWin(Random rand);
    }
}
