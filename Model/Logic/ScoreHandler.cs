using System;
using System.Collections.Generic;
using System.Text;

namespace Spades
{
    public class ScoreHandler
    {

        private int getNilScore(Player player, int numTricks)
        {
            int multiplier = player.isBlindBid() ? 1 : 2;
            if (player.getBid() == 0 && numTricks > 0)
                return -100 * multiplier;
            if (player.getBid() == 0 && numTricks == 0)
                return 100 * multiplier;
            return 0;
        }
        private Tuple<int, int> getTeamNonNilScoreAndBags(Player player1, Player player2, int numTricks1, int numTricks2)
        {
            Tuple<int, int> totalBidAndTricks = getNonNilBidsAndTricks(player1, player2, numTricks1, numTricks2);
            int totalBid = totalBidAndTricks.Item1;
            int totalTricks = totalBidAndTricks.Item2;
            if (totalTricks < totalBid)
                return new Tuple<int, int>(totalBid * 10 * -1, 0);
            return new Tuple<int, int>(totalBid * 10 + totalTricks - totalBid, totalTricks - totalBid);
        }

        private Tuple<int, int> getNonNilBidsAndTricks(Player player1, Player player2, int numTricks1, int numTricks2)
        {
            if (player1.getBid() != 0 && player2.getBid() != 0)
                return new Tuple<int, int>(player1.getBid() + player2.getBid(), numTricks1 + numTricks2);
            else if (player1.getBid() != 0)
                return new Tuple<int, int>(player1.getBid(), numTricks1);
            else if (player2.getBid() != 0)
                return new Tuple<int, int>(player2.getBid(), numTricks2);
            return new Tuple<int, int>(0, 0);
        }

        public void calculateAndStoreTeamResult(Team team, int numTricks1, int numTricks2)
        {
            int teamScore = 0;
            teamScore += getNilScore(team.getPlayer1(), numTricks1);
            teamScore += getNilScore(team.getPlayer2(), numTricks2);
            Tuple<int, int> scoreAndBags = getTeamNonNilScoreAndBags(team.getPlayer1(), team.getPlayer2(), numTricks1, numTricks2);
            teamScore = handleBags(team, teamScore, scoreAndBags);
            team.addScore(teamScore);
            team.addBid();
        }

        private int handleBags(Team team, int teamScore, Tuple<int, int> scoreAndBags)
        {
            teamScore += scoreAndBags.Item1;
            team.addTotalBag(scoreAndBags.Item2);
            if (team.getBagsTotal() > 10)
            {
                teamScore -= 100;
                team.setBagsTotal(team.getBagsTotal() - 10);
            }
            team.addBag(scoreAndBags.Item2);
            return teamScore;
        }
    }
}
