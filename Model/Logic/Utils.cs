using System;
using System.Collections.Generic;
using System.Text;

namespace Spades
{
    public class Utils
    {
        public System.Random rnd;
        private Utils()
        {
            rnd = new System.Random();
        }
        public int decreaseModuloStep(int value, int modulo)
        {
            value--;
            if (value < 0)
                value = modulo - 1;
            return value;
        }


        private static Utils instance = null;
        public static Utils Instance
        {
            get
            {
                if (instance == null)
                    instance = new Utils();
                return instance;
            }
        }


    }
}
