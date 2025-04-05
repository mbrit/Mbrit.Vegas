using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    internal class Card
    {
        internal Suit Suit { get; }
        internal int Rank { get; }

        internal Card(Suit suit, int rank)
        {
            this.Suit = suit;
            this.Rank = rank;
        }

        private string RankName
        {
            get
            {
                switch(this.Rank)
                {
                    case 1:
                        return "A";

                    case 11:
                        return "J";

                    case 12:
                        return "Q";

                    case 13:
                        return "K";

                    default:
                        return this.Rank.ToString();
                }
            }
        }

        public override string ToString() => GetSuitSymbol(this.Suit) + this.RankName;

        internal static string GetSuitSymbol(Suit suit)
        {
            if (suit == Suit.Diamond)
                return "♦";
            else if (suit == Suit.Heart)
                return "♥";
            else if (suit == Suit.Spade)
                return "♠";
            else if (suit == Suit.Club)
                return "♣";
            else
                throw new NotImplementedException("This operation has not been implemented.");
        }
    }
}
