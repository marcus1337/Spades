using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Spades
{
    public class Deal
    {
        private Player[] players;
        public bool breakingSpades;
        public int[] tricks;
        public Book book;

        public Deal(int playerFirst, Player[] players)
        {
            this.players = players;
            reset(playerFirst);
        }

        public void addTrickResult()
        {
            tricks[book.getTrickWinIndex()]++;
        }

        private void reset(int playerFirst)
        {
            book = new Book(playerFirst);
            breakingSpades = false;
            tricks = new int[4];
            assignCardsToPlayers();
        }

        public bool isDealOver()
        {
            foreach (Player player in players)
            {
                if (player.getCards().Count > 0)
                    return false;
            }
            return true;
        }

        public void startNextBook()
        {
            book = new Book(book.getTrickWinIndex());
        }

        private void assignCardsToPlayers()
        {
            Stack<Card> cardStack = makeShuffledCardStack();
            foreach (Player player in players)
            {
                player.getCards().Clear();
                for (int i = 0; i < 13; i++)
                    player.getCards().Add(cardStack.Pop());
                player.getHand().orderBySuit();
            }
        }

        private Stack<Card> makeShuffledCardStack()
        {
            Deck deck = new Deck();
            deck.shuffle();
            Stack<Card> cardStack = new Stack<Card>();
            foreach (Card card in deck.getCards())
                cardStack.Push(card);
            return cardStack;
        }

        //To break spades: no other playable suit
        //Once spades broken: may lead with spades
        //Lead suit wins unless someone played spades, then spades wins
        //Other players must always play lead suit if they can.

    }
}
