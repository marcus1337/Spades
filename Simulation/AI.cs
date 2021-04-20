using System;
using System.Collections.Generic;
using System.Text;

namespace Spades
{
    [Serializable]
    public class AI
    {
        private int playerIndex;

        private float[] hiddenNodes, outputNodes;
        private float[] edgesAToB, edgesBToC;

        public void setPlayerIndex(int playerIndex)
        {
            this.playerIndex = playerIndex;
        }

        private void resetBuckets()
        {
            hiddenNodes = new float[4];
            outputNodes = new float[2];
            edgesAToB = new float[16];
            edgesBToC = new float[8];
        }
        private int propagateNetworkAndGetAction(float[] inputNodes)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    hiddenNodes[j] += inputNodes[i] * edgesAToB[i * 4 + j];
                }
            }
            for (int i = 0; i < 4; i++)
            {
                hiddenNodes[i] = Math.Max(0, hiddenNodes[i]);
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    outputNodes[j] += hiddenNodes[i] * edgesBToC[i * 2 + j];
                }
            }
            int maxIndex = 0;
            float maxOutputValue = outputNodes[0];
            for (int i = 1; i < 2; i++)
            {
                if (outputNodes[i] > maxOutputValue)
                {
                    maxIndex = i;
                    maxOutputValue = outputNodes[i];
                }
            }

            return maxIndex;
        }

        public AI(int playerIndex)
        {
            this.playerIndex = playerIndex;
            resetBuckets();
        }

        private bool canSabotageEnemyNilBid(Spades spades)
        {
            return spades.players[(playerIndex+1)%4].getBid() == 0 || spades.players[(playerIndex + 3) % 4].getBid() == 0;
        }

        private bool canSabotageEnemyFromReachingTotalBid(Spades spades)
        {
            return spades.getEnemyTeamGoalTricks(playerIndex) > spades.getEnemyTeamTakenTricks(playerIndex);
        }

        private bool needMoreTricks(Spades spades)
        {
            return spades.players[playerIndex].getBid() != 0 && spades.getTeamGoalTricks(playerIndex) > spades.getTeamTakenTricks(playerIndex);
        }

        private bool partnerNeedMoreTricks(Spades spades)
        {
            Player teamPlayer = spades.getTeamPlayer(playerIndex);
            if (teamPlayer.getBid() != 0 && spades.getTeamGoalTricks(playerIndex) > spades.getTeamTakenTricks(playerIndex))
                return true;
            return false;
        }

        private List<Card> getWinnableCards(Spades spades)
        {
            Card leadCard = spades.getBestPlayedCard();
            if (leadCard == null)
                return spades.getPlayableCards(playerIndex);

            List<Card> winCards = new List<Card>();
            List<Card> playableCards = spades.getPlayableCards(playerIndex);
            foreach (Card card in playableCards)
            {
                if (spades.isOtherCardBetter(leadCard, card))
                    winCards.Add(card);
            }
            return winCards;
        }

        private List<Card> getLosingCards(Spades spades)
        {
            Card leadCard = spades.getBestPlayedCard();
            if (leadCard == null)
                return new List<Card>();

            List<Card> loseCards = new List<Card>();
            List<Card> playableCards = spades.getPlayableCards(playerIndex);
            foreach (Card card in playableCards)
                if (!spades.isOtherCardBetter(leadCard, card))
                    loseCards.Add(card);

            return loseCards;
        }

        private bool canWinTrick(Spades spades)
        {
            Card leadCard = spades.getBestPlayedCard();
            if (leadCard == null)
                return true;
            List<Card> playableCards = spades.getPlayableCards(playerIndex);
            foreach(Card card in playableCards)
            {
                if (spades.isOtherCardBetter(leadCard, card))
                    return true;
            }

            return false;
        }

        private Card getHighestNonWinPlayableCard(Spades spades)
        {
            List<Card> loseCards = getLosingCards(spades);
            Card bestCard = loseCards[0];
            foreach(Card card in loseCards)
            {
                if (card.getRank() > bestCard.getRank())
                    bestCard = card;
            }
            return bestCard;
        }

        private Card getHighestPlayableCard(Spades spades)
        {
            List<Card> playableCards = spades.getPlayableCards(playerIndex);
            Card highest = playableCards[0];
            foreach(Card card in playableCards)
            {
                if (spades.isOtherCardBetter(highest, card))
                    highest = card;
            }
            return highest;
        }

        private Card getLowestPlayableCard(Spades spades)
        {
            List<Card> playableCards = spades.getPlayableCards(playerIndex);
            Card lowest = playableCards[0];
            foreach (Card card in playableCards)
            {
                if (!spades.isOtherCardBetter(lowest, card))
                    lowest = card;
            }
            return lowest;
        }

        Card tryWinAction(Spades spades)
        {
            if (canWinTrick(spades))
                return getHighestPlayableCard(spades);
            return getLowestPlayableCard(spades);
        }

        private bool canLoseTrick(Spades spades)
        {
            return getLosingCards(spades).Count > 0;
        }

        Card tryLoseAction(Spades spades)
        {
            if (canLoseTrick(spades))
                return getHighestNonWinPlayableCard(spades);
            if (spades.getPlayedCards().Count < 3)
                return getLowestPlayableCard(spades);
            return getHighestPlayableCard(spades);
        }

        public Card drawCard(Spades spades)
        {
            float canSabotage1 = canSabotageEnemyNilBid(spades) ? 1.0f : 0.0f;
            float canSabotage2 = canSabotageEnemyFromReachingTotalBid(spades) ? 1.0f : 0.0f;
            float needTricks1 = needMoreTricks(spades) ? 1.0f : 0.0f;
            float needTricks2 = partnerNeedMoreTricks(spades) ? 1.0f : 0.0f;
            int chosenAction = propagateNetworkAndGetAction(new float[] { canSabotage1, canSabotage2, needTricks1, needTricks2 });
            if (chosenAction == 1)
                return tryWinAction(spades);    
            return tryLoseAction(spades);
        }

        public int getBid(Spades spades)
        {
            List<Card> myCards = spades.players[playerIndex].getCards();
            int numAces = 0;
            int numKings = 0;
            foreach (Card card in myCards)
            {
                if (card.getRank() == Rank.Ace)
                    numAces++;
                if (card.getRank() == Rank.King)
                    numKings++;
            }
            int numBids = numAces;
            for (int i = 0; i < numKings; i++)
            {
                if (Utils.Instance.rnd.Next(0, 2) == 1)
                {
                    numBids++;
                }
            }

            if (numBids == 0 && Utils.Instance.rnd.Next(0, 2) == 1)
                numBids = 1;

            return numBids;
        }

    }
}
