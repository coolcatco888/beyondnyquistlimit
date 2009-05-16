using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace ContentTestLibrary
{
    public class PlayerInfo
    {
        #region Current Statistics

        // Players current health value
        private int currentHealth;
        public int CurrentHealth
        {
            get { return currentHealth; }
            set { currentHealth = value; }
        }

        // Players current mana value
        private int currentMana;
        public int CurrentMana
        {
            get { return currentMana; }
            set { currentMana = value; }
        }

        // Players total damage: combination of class + level + weapon (+modifier)
        private int currentDamage;
        public int CurrentDamage
        {
            get { return currentDamage; }
            set { currentDamage = value; }
        }

        private int currentDefense;
        public int CurrentDefense
        {
            get { return currentDefense; }
            set { currentDefense = value; }
        }

        [ContentSerializerIgnore]
        private int attackGauge;
        [ContentSerializerIgnore]
        public int AttackGauge
        {
            get { return attackGauge; }
            set { attackGauge = value; }
        }
        #endregion

        #region Max Statistics

        private int maxHealth;
        public int MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

        private int maxMana;
        public int MaxMana
        {
            get { return maxMana; }
            set { maxMana = value; }
        }

        #endregion

        #region Level Related

        private int level;
        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        private int experience;
        public int Experience
        {
            get { return experience; }
            set { experience = value; }
        }

        private int experienceToNextLevel;
        public int ExperienceToNextLevel
        {
            get { return experienceToNextLevel; }
            set { experienceToNextLevel = value; }
        }

        #endregion
    }
}
