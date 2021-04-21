using System;
using System.Collections.Generic;
using System.Text;

namespace Spades.Simulation
{
    public class AITrainer
    {

        public void train()
        {
            AI ai1 = new AI(0);
            AI ai2 = new AI(0);
            BotFightSim fightSim = new BotFightSim();


            for(int i = 0; i < 1000; i++)
            {
                AI winnerAI = fightSim.getBestAI(ai1, ai2);
                ai1 = winnerAI.copy();
                ai2 = winnerAI.copy();
                ai2.getNeuralNet().mutateEdges();

            }

            fightSim.getAIFightScores(ai1, ai2);
        }

    }
}
