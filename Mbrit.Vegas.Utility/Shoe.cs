using BootFX.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    internal class Shoe
    {
        private List<Card> Cards { get; set; }
        private int Position { get; set; } = -1;

        private int NumberOfDecks { get; }
        private Random Random { get; }

        internal Shoe(int numberOfDecks, Random rand = null)
        {
            rand ??= new Random();

            this.NumberOfDecks = numberOfDecks;
            this.Random = rand;

            this.Rebuild();
        }

        private void Rebuild()
        { 
            var cards = new List<Card>();
            for(var index = 0; index < this.NumberOfDecks; index++)
            {
                var deck = new Deck(this.Random);
                cards.AddRange(deck);
            }

            var wash = this.Random.Next(100, 200);
            for (var index = 0; index < wash; index++)
                cards.Shuffle(this.Random);

            this.Cards = cards;
            this.Position = -1;
        }

        internal Card Deal()
        {
            if (this.Position == this.Cards.Count - 1)
                this.Rebuild();

            this.Position++;
            return this.Cards[this.Position];
        }
    }
}
