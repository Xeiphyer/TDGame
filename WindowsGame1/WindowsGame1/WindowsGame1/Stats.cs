using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    static class Stats
    {
        private static int lives;
        private static int gold;
        private static int energy;

        public static int getLives()
        {
            return lives;
        }
        public static void setLives(int livesIn)
        {
            lives = livesIn;
        }

        public static int getGold()
        {
            return gold;
        }
        public static void setGold(int goldIn)
        {
            gold = goldIn;
        }

        public static int getEnergy()
        {
            return energy;
        }
        public static void setEnergy(int energyIn)
        {
            energy = energyIn;
        }
    }
}
