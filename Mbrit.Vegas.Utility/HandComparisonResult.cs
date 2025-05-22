using System;
using System.Collections.Generic;

namespace Mbrit.Vegas.Utility
{
    internal class HandComparisonResult
    {
        internal Actor Winner { get; }
        internal PokerHand PlayerHand { get; }
        internal PokerHand DealerHand { get; }

        internal HandComparisonResult(Actor winner, PokerHand playerHand, PokerHand dealerHand)
        {
            this.Winner = winner;
            this.PlayerHand = playerHand;
            this.DealerHand = dealerHand;
        }
    }
} 