using System;
using System.Collections.Generic;
using System.Text;

namespace Spades.Simulation
{
    public class BotFightSim
    {
        private AI aiA1, aiA2, aiB1, aiB2;
        private Spades spades;

        private AI getAITurn()
        {
            if (spades.getPlayerTurn() == 0)
                return aiA1;
            if (spades.getPlayerTurn() == 2)
                return aiA2;
            if (spades.getPlayerTurn() == 1)
                return aiB1;
            return aiB2;
        }

        public void getAIFightScores(AI ai1, AI ai2)
        {
            init(ai1, ai2);
            for (int i = 0; i < 40; i++)
                playADeal();

            Console.WriteLine("SCORES: ");
            Console.WriteLine(spades.team1.getScore());
            Console.WriteLine(spades.team2.getScore());
        }

        public AI getBestAI(AI ai1, AI ai2)
        {
            init(ai1, ai2);

            for(int i = 0; i < 50; i++)
                playADeal();

            if (spades.team1.getScore() > spades.team2.getScore())
                return ai1;
            return ai2;
        }

        private void init(AI ai1, AI ai2)
        {
            spades = new Spades();
            this.aiA1 = ai1.copy(); 
            this.aiA2 = ai1.copy();
            this.aiB1 = ai2.copy();
            this.aiB2 = ai2.copy();
            aiA1.setPlayerIndex(0);
            aiA2.setPlayerIndex(2);
            aiB1.setPlayerIndex(1);
            aiB2.setPlayerIndex(3);
        }

        private void playADeal()
        {
            spades.addBid(getAITurn().getBid(spades));
            spades.addBid(getAITurn().getBid(spades));
            spades.addBid(getAITurn().getBid(spades));
            spades.addBid(getAITurn().getBid(spades));

            for (int i = 0; i < 13; i++)
            {
                spades.playCard(getAITurn().drawCard(spades));
                spades.playCard(getAITurn().drawCard(spades));
                spades.playCard(getAITurn().drawCard(spades));
                spades.playCard(getAITurn().drawCard(spades));

                spades.startNextBook();
            }
            spades.calculateAndStoreResults();
            spades.startNextDeal();
        }
    }
}
