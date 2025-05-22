using BootFX.Common;
using System.Collections;

namespace Mbrit.Vegas.Utility
{
    internal class Deck : IEnumerable<Card>
    {
        private IEnumerable<Card> Cards { get; }

        internal Deck(Random rand = null)
        {
            rand ??= new Random();

            var cards = new List<Card>();
            foreach(var suit in Enum.GetValues<Suit>())
            {
                for(var index = 1; index <= 13; index++)
                    cards.Add(new Card(suit, index));
            }

            cards.Shuffle(rand);

            this.Cards = cards;
        }

        public IEnumerator<Card> GetEnumerator() => this.Cards.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}