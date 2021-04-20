using System;
using System.Collections.Generic;
using System.Text;

namespace Spades
{
    public class Book
    {
        private int playerTurn, playerFirst;
        private List<Card> playedCards;

        public List<Card> getPlayedCards()
        {
            return playedCards;
        }

        public Book(int playerFirst)
        {
            playedCards = new List<Card>();
            this.playerFirst = playerTurn = playerFirst;
        }

        public int getPlayerTurn()
        {
            return playerTurn;
        }

        public void setPlayerStarter(int playerTurn)
        {
            this.playerTurn = this.playerFirst = playerTurn;
        }

        public int getTrickWinIndex()
        {
            int[] whoPlayedCards = getWhoPlayedCards();
            return whoPlayedCards[getBookWinnerIndex()];
        }

        public void playCard(Card card)
        {
            playedCards.Add(card);
            playerTurn = (playerTurn + 1) % 4;
        }

        public bool isBookOver()
        {
            return playedCards.Count == 4;
        }

        public bool isOtherCardBetter(Card bestCard, Card otherCard)
        {
            if (bestCard.getSuit() != Suit.Spades && otherCard.getSuit() == Suit.Spades)
                return true;
            if (otherCard.getSuit() == bestCard.getSuit() && otherCard.getRank() > bestCard.getRank())
                return true;
            return false;
        }

        public Card getBestPlayedCard()
        {
            if (playedCards.Count == 0)
                return null;
            Card bestCard = playedCards[0];
            for (int i = 1; i < playedCards.Count; i++)
                if (isOtherCardBetter(bestCard, playedCards[i]))
                    bestCard = playedCards[i];
            return bestCard;
        }

        private int getBookWinnerIndex()
        {
            Card bestCard = playedCards[0];
            int bestWhoPlayedIndex = 0;
            for (int i = 1; i < 4; i++)
            {
                if (isOtherCardBetter(bestCard, playedCards[i]))
                {
                    bestWhoPlayedIndex = i;
                    bestCard = playedCards[i];
                }
            }
            return bestWhoPlayedIndex;
        }

        private int[] getWhoPlayedCards()
        {
            int[] whoPlayedCards = new int[4];
            for (int i = 0; i < 4; i++)
            {
                whoPlayedCards[i] = playerTurn;
                playerTurn = (playerTurn + 1) % 4;
            }
            return whoPlayedCards;
        }

    }
}
