using System;
using System.Collections.Generic;
using System.Text;

namespace Spades
{
    public class Bidding
    {
        private int playerTurn, playerFirst;
        private Player[] players;
        private bool[] madeBids;

        public int getFirstBidderIndex()
        {
            return playerFirst;
        }

        public int getPlayerTurn()
        {
            return playerTurn;
        }

        public bool hasBid(int index)
        {
            return madeBids[index];
        }

        public Bidding(int playerFirst, Player[] players)
        {
            madeBids = new bool[4];
            this.players = players;
            this.playerFirst = playerFirst;
            bidPhase = true;
        }

        public void addBid(int bid)
        {
            players[playerTurn].setBid(bid);
            madeBids[playerTurn] = true;
            playerTurn = (playerTurn + 1) % 4;
            if (playerTurn == playerFirst)
            {
                bidPhase = false;
                madeBids = new bool[4];
            }
        }

        public bool isBidPhase()
        {
            return bidPhase;
        }

        private bool bidPhase;

    }
}
