using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace ContentTestLibrary
{
    public class PlayerInfo : ActorInfo
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

        [ContentSerializerIgnore]
        private CharacterClassInfo currentClass;
        [ContentSerializerIgnore]
        public CharacterClassInfo CurrentClass
        {
            get { return currentClass; }
        }

        #endregion
    }
}
