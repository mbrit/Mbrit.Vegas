using Mbrit.Vegas.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mbrit.Vegas.Simulator;

namespace Mbrit.Vegas.Utility
{
    internal class WinLoseDraw
    {
        internal WinLoseDrawType Type { get; }
        internal GameWin Win { get; }

        internal WinLoseDraw(WinLoseDrawType type, GameWin win = null)
        {
            if (type == WinLoseDrawType.Win && win == null)
                throw new ArgumentException("Win must be specified.");
            else if (type != WinLoseDrawType.Win && win != null)
                throw new ArgumentException("Win must not be specified.");

            this.Type = type;
            this.Win = win;
        }
    }
}
