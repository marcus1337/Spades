using System;
using System.Collections.Generic;
using System.Text;

namespace Spades
{
    public class Team
    {
        private List<Tuple<int, int>> bids;
        private List<int> scores;
        private List<int> bags;
        private Player player1, player2;
        private int bagsTotal;

        public int getBagsTotal()
        {
            return bagsTotal;
        }

        public void setBagsTotal(int bagsTotal)
        {
            this.bagsTotal = bagsTotal;
        }

        public void addBid()
        {
            bids.Add(new Tuple<int, int>(player1.getBid(), player2.getBid()));
        }

        public List<Tuple<int, int>> getBids()
        {
            return bids;
        }

        public Player getPlayer1()
        {
            return player1;
        }

        public Player getPlayer2()
        {
            return player2;
        }

        public List<int> getBags()
        {
            return bags;
        }

        public void addTotalBag(int bag)
        {
            bagsTotal += bag;
        }

        public void addBag(int bag)
        {
            bags.Add(bag);
        }

        public void addScore(int score)
        {
            scores.Add(score);
        }

        public Team(Player player1, Player player2)
        {
            this.player1 = player1;
            this.player2 = player2;
            bids = new List<Tuple<int, int>>();
            scores = new List<int>();
            bagsTotal = 0;
        }

        public List<int> getScores()
        {
            return scores;
        }

        public int getScore()
        {
            int sum = 0;
            foreach (int x in scores)
                sum += x;
            return sum;
        }


    }
}
