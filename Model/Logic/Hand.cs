using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Spades
{
    public class Hand
    {
        private List<Card> cards;

        public Hand()
        {
            cards = new List<Card>();
        }

        public List<Card> getCards()
        {
            return cards;
        }

        public void setCards(List<Card> cards)
        {
            this.cards = cards;
        }

        public void removeCard(Card card)
        {
            cards.Remove(card);
        }

        public void orderBySuit()
        {
            cards = cards.OrderBy(o => o.getSuit()).ThenBy(o => o.getRank()).ToList();
        }

        public bool[] getPlayableCardsUnbrokenSpades(List<Card> playedCards)
        {
            bool[] playable = new bool[cards.Count];
            if (playedCards.Count == 0)
                getStartPlayableCardsUnbrokenSpades(cards, playable);
            else
            {
                if (handHasSuit(cards, playedCards[0].getSuit()))
                    for (int i = 0; i < cards.Count; i++)
                        playable[i] = cards[i].getSuit() == playedCards[0].getSuit();
                else
                    for (int i = 0; i < cards.Count; i++)
                        playable[i] = true;
            }
            return playable;
        }

        public bool[] getPlayableCardsBrokenSpades(List<Card> playedCards)
        {
            bool[] playable = new bool[cards.Count];
            if (playedCards.Count == 0)
                for (int i = 0; i < cards.Count; i++)
                    playable[i] = true;
            else
            {
                if (handHasSuit(cards, playedCards[0].getSuit()))
                    for (int i = 0; i < cards.Count; i++)
                        playable[i] = cards[i].getSuit() == playedCards[0].getSuit();
                else
                    for (int i = 0; i < cards.Count; i++)
                        playable[i] = true;
            }
            return playable;
        }

        private bool handHasSuit(List<Card> hand, Suit suit)
        {
            foreach (Card card in hand)
                if (card.getSuit() == suit)
                    return true;
            return false;
        }

        private void getStartPlayableCardsUnbrokenSpades(List<Card> hand, bool[] playable)
        {
            bool anyNonSpades = false;
            foreach (Card card in hand)
                if (card.getSuit() != Suit.Spades)
                    anyNonSpades = true;

            if (anyNonSpades)
            {
                for (int i = 0; i < hand.Count; i++)
                    if (hand[i].getSuit() != Suit.Spades)
                        playable[i] = true;
            }
            else
            {
                for (int i = 0; i < hand.Count; i++)
                    playable[i] = true;
            }
        }

    }
}
