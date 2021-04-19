using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spades.Testing
{
    public class Tests
    {

        Spades spades;
        Team team1, team2;

        private void initVariables()
        {
            spades = new Spades();
            team1 = spades.team1;
            team2 = spades.team2;
        }

        public bool testCardAssigning()
        {
            initVariables();
            List<Card> allCards = new List<Card>();
            allCards.AddRange(team1.getPlayer1().getCards());
            allCards.AddRange(team1.getPlayer2().getCards());
            allCards.AddRange(team2.getPlayer1().getCards());
            allCards.AddRange(team2.getPlayer2().getCards());

            bool isUnique = allCards.Distinct().Count() == allCards.Count;

            return isUnique;
        }

        public bool testBidding()
        {

            initVariables();

            for (int i = 0; i < 4; i++)
                if (spades.hasBid(i))
                    return false;

            int turnStart = spades.getPlayerTurn();
            List<int> bids = new List<int>();
            for(int i = 1; i <= 4; i++)
            {
                int theBid = Utils.Instance.rnd.Next(10, 1532);
                bids.Add(theBid);
                spades.addBid(theBid);
            }

            if (spades.getPlayerTurn() != turnStart)
                return false;
            if (spades.isBidPhase())
                return false;

            for (int i = 0; i < 4; i++)
                if (spades.players[(turnStart + i) % 4].getBid() != bids[i])
                    return false;

            return true;
        }

        public bool testBook()
        {
            //Check if valid cards are valid and unvalid cards unvalid.
            //Check if breakingSpades works as intended.
            for(int i = 0; i < 4; i++)
                foreach(Card card in spades.getPlayableCards(i))
                    if (card.getSuit() == Suit.Spades)
                        return false;

            for(int i = 0; i < 13; i++)
            {
                spades.playCard(spades.getPlayableCards(spades.getPlayerTurn())[0]);
                spades.playCard(spades.getPlayableCards(spades.getPlayerTurn())[0]);
                spades.playCard(spades.getPlayableCards(spades.getPlayerTurn())[0]);
                spades.playCard(spades.getPlayableCards(spades.getPlayerTurn())[0]);

                if (!spades.isBookFinished())
                    return false;

                List<Card> cards = spades.getPlayedCards();
                foreach (Card card in cards)
                {
                    Console.WriteLine(card.ToString());
                }

                spades.startNextBook();
                Console.WriteLine("-------\t" + (i + 1).ToString());
            }


            if (!spades.isRoundFinished())
                return false;


            return true;
        }

        public void testAll()
        {
            if (!testCardAssigning())
                throw new Exception("UNIQUE CARDS TEST");

            if (!testBidding())
                throw new Exception("BIDDING TEST");

            if(!testBook())
                throw new Exception("BOOKING TEST");

            Console.WriteLine("TESTS OK!");
        }

    }
}
