using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    internal class GameResultItem 
    {
        internal WinLoseDraw Result { get; }
        internal decimal BankAfter { get; }
        internal int TrackAfter { get; }

        internal GameResultItem(WinLoseDraw result, decimal bankAfter, int trackAfter)
        {
            this.Result = result;
            this.BankAfter = bankAfter;
            this.TrackAfter = trackAfter;
        }
    }
}
