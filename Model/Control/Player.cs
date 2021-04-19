using System;
using System.Collections.Generic;
using System.Text;

namespace Spades
{
    public class Player
    {
        private Hand hand;
        private int bid;

        private string name;

        public string getName()
        {
            return name;
        }

        public void setName(string name)
        {
            this.name = name;
        }

        public int getBid()
        {
            return bid;
        }

        public void setBid(int bid)
        {
            this.bid = bid;
        }

        public List<Card> getCards()
        {
            return hand.getCards();
        }

        public void setCards(List<Card> cards)
        {
            hand.setCards(cards);
        }

        public Hand getHand()
        {
            return hand;
        }

        public Player()
        {
            bid = -1;
            hand = new Hand();
        }

    }
}
