using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Spades
{

    public class Deck
    {
        private Card[] cards;

        public Deck()
        {
            cards = new Card[52];
            for(int i = 0; i < 52; i++)
            {
                cards[i] = new Card((Rank)((i % 13) + 2), (Suit)(i / 13));
            }
        }

        public void shuffle()
        {
            cards = cards.OrderBy(x => Utils.Instance.rnd.Next()).ToArray();
        }

        public Card[] getCards()
        {
            return cards;
        }


    }
}
