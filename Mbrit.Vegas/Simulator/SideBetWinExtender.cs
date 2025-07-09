using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Simulator
{
    internal static class SideBetWinExtender
    {
        public static decimal GetPayoutMultiplier(this SideBetWin win) => win switch
        {
            SideBetWin.Jackpot => 30M,
            SideBetWin.Major => 15M,
            SideBetWin.Minor => 5M,
            SideBetWin.Bonus => 2M,
            _ => 0M
        };
    }
}
