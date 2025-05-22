using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    internal class BankrollResults
    {
        internal int Bankroll { get; }
        internal int MinBankroll { get; }
        internal int MaxBankroll { get; }
        internal int TotalWager { get; }
        internal int HandsPlayed { get; }

        internal BankrollResults(int bankroll, int minBankroll, int maxBankroll, int totalWager, int handsPlayed)
        {
            this.Bankroll = bankroll;
            this.MinBankroll = minBankroll;
            this.MaxBankroll = maxBankroll;
            this.TotalWager = totalWager;
            this.HandsPlayed = handsPlayed;
        }
    }
}
