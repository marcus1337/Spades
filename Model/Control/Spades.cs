using System;
using System.Collections.Generic;
using System.Text;

namespace Spades
{
    /*Spades API
     1. New game made via constructor.
            -Bid via addBid until !isBidPhase
            -Play via playCard until isBookFinished
                if(!isRoundFinished)
                    -startNextBook
                else
                    -calculateAndStoreResults
                    -startNextDeal
         */

    public class Spades
    {
        public Team team1, team2;
        public Player[] players;
        private Bidding bidding;
        private Deal deal;
        private ScoreHandler scoreHandler;
        private int startingPlayer;

        public bool isRoundFinished()
        {
            return !isBidPhase() && deal.isDealOver();
        }

        public int getPlayerTurn()
        {
            if (bidding.isBidPhase())
                return bidding.getPlayerTurn();
            return deal.book.getPlayerTurn();
        }

        public bool isBidPhase()
        {
            return bidding.isBidPhase();
        }
        public bool hasBid(int index)
        {
            return bidding.hasBid(index);
        }
        public void addBid(int bid)
        {
            bidding.addBid(bid);
        }

        public void calculateAndStoreResults()
        {
            scoreHandler.calculateAndStoreTeamResult(team1, deal.tricks[0], deal.tricks[2]);
            scoreHandler.calculateAndStoreTeamResult(team2, deal.tricks[1], deal.tricks[3]);
        }

        public bool isSpadesBroken()
        {
            return deal.breakingSpades;
        }

        public void playCard(Card card)
        {
            players[getPlayerTurn()].getHand().removeCard(card);
            deal.book.playCard(card);
            if (card.getSuit() == Suit.Spades)
                deal.breakingSpades = true;
            if (deal.book.isBookOver())
            {
                deal.addTrickResult();
            }
        }

        public List<Card> getPlayedCards()
        {
            return deal.book.getPlayedCards();
        }

        public List<Card> getPlayableCards(int playerIndex)
        {
            List<Card> result = new List<Card>();
            bool[] playableCardIndices = null;
            if (isSpadesBroken())
                playableCardIndices = players[playerIndex].getHand().getPlayableCardsBrokenSpades(deal.book.getPlayedCards());
            else
                playableCardIndices = players[playerIndex].getHand().getPlayableCardsUnbrokenSpades(deal.book.getPlayedCards());
            for(int i = 0; i < playableCardIndices.Length; i++)
                if (playableCardIndices[i])
                    result.Add(players[playerIndex].getHand().getCards()[i]);
            return result;
        }

        public bool isBookFinished()
        {
            return deal.book.isBookOver();
        }

        public void startNextBook()
        {
            deal.startNextBook();
        }

        public void startNextDeal()
        {
            startingPlayer = (startingPlayer + 1) % 4;
            bidding = new Bidding(startingPlayer, players);
            deal = new Deal(startingPlayer, players);
        }

        public Spades()
        {
            team1 = new Team(new Player(), new Player());
            team2 = new Team(new Player(), new Player());
            scoreHandler = new ScoreHandler();
            players = new Player[4];
            players[0] = team1.getPlayer1();
            players[1] = team2.getPlayer1();
            players[2] = team1.getPlayer2();
            players[3] = team2.getPlayer2();

            startingPlayer = Utils.Instance.rnd.Next(4);
            bidding = new Bidding(startingPlayer, players);
            deal = new Deal(startingPlayer, players);
        }


    }
}
