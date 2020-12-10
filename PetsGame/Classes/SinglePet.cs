using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetsGame
{
    public class SinglePet
    {
        public string petName;

        /// <summary>
        /// The number of days in the game. Cannot be negative.
        /// </summary>
        public int totalDays = 20;

        public int mHappiness = 15;
        public int happyBonus;
        public int hunger;
        public int hungerModifier = 2;
        public string petType;
        public string descriptionPet;
        public string age;
        public string descriptionAge;
    }
}
