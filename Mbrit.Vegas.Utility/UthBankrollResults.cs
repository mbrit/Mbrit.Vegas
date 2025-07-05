using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    internal class UthBankrollResults : BankrollResults
    {
        internal int Num4x { get; }
        internal int Num2x { get; }
        internal int Num1x { get; }

        internal UthBankrollResults(int bankroll, int minBankroll, int maxBankroll, int totalWager, int handsPlayed, int num4x, int num2x, int num1x)
            : base(bankroll, minBankroll, maxBankroll, totalWager, handsPlayed)
        {
            this.Num4x = num4x;
            this.Num2x = num2x;
            this.Num1x = num1x;
        }
    }
}
