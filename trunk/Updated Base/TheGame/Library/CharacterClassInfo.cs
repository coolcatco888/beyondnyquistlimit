using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace Library
{
    /// <summary>
    /// An informational structure for a character class.  Holds class base information and 
    /// level up gains.
    /// </summary>
    public class CharacterClassInfo
    {
        #region Descriptive Data

        // Describes what class this is
        private string className;
        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        #endregion

        #region Base Attributes
        /*
        // Strength modifier affects amount of physical damage
        private int strength;
        public int Strength
        {
            get { return strength; }
            set { strength = value; }
        }

        // Agility modifier affects defense value and dodging reflex spell types
        private int agility;
        public int Agility
        {
            get { return agility; }
            set { agility = value; }
        }

        // Consitution modifier affects health value and stun/poison effects
        private int constitution;
        public int Constitution
        {
            get { return constitution; }
            set { constitution = value; }
        }

        // Intelligence modifier affects mana value and damage with magical staves
        private int intelligence;
        public int Intelligence
        {
            get { return intelligence; }
            set { intelligence = value; }
        }

        // Wisdom modifier affects mind effects
        private int wisdom;
        public int Wisdom
        {
            get { return wisdom; }
            set { wisdom = value; }
        }

        // Charisma modifier affects merchant goods and information gathering
        private int charisma;
        public int Charisma
        {
            get { return charisma; }
            set { charisma = value; }
        }
        */
        #endregion

        #region Base Data

        // The base health of the class before modifiers and levels
        private int baseHealth;
        public int BaseHealth
        {
            get { return baseHealth; }
            set { baseHealth = value; }
        }

        // The base mana or stamina depending on the class before modifiers and levels
        private int baseMana;
        public int BaseMana    
        {
            get { return baseMana; }
            set { baseMana = value; }
        }

        // The base damage of the class before modifiers and levels
        private int baseDamage;
        public int BaseDamage
        {
            get { return baseDamage; }
            set { baseDamage = value; }
        }

        // The base speed of the class before modifiers and levels
        private int baseDefense;
        public int BaseDefense
        {
            get { return baseDefense; }
            set { baseDefense = value; }
        }

        #endregion

        #region Level Up Info

        // The base amount of health gain at a level up
        private int gainHealth;
        public int GainHealth
        {
            get { return gainHealth; }
            set { gainHealth = value; }
        }

        // The base amount of mana gain at a level up
        private int gainMana;
        public int GainMana
        {
            get { return gainMana; }
            set { gainMana = value; }
        }

        // The base amount of damage gain at a level up
        private int gainDamage;
        public int GainDamage
        {
            get { return gainDamage; }
            set { gainDamage = value; }
        }

        // The base amount of speed gain at a level up
        private int gainDefense;
        public int GainDefense
        {
            get { return gainDefense; }
            set { gainDefense = value; }
        }

        #endregion

        #region Content Reader

        public class CharacterClassInfoReader : ContentTypeReader<CharacterClassInfo>
        {
            protected override CharacterClassInfo Read(ContentReader input, 
                CharacterClassInfo existingInstance)
            {
                CharacterClassInfo classInfo = existingInstance;
                if (classInfo == null)
                {
                    classInfo = new CharacterClassInfo();
                }

                // Read in Descriptive Data 
                classInfo.className = input.ReadString();
                classInfo.description = input.ReadString();

                // Read in Base Attributes
                /*
                classInfo.strength = input.ReadInt32();
                classInfo.agility = input.ReadInt32();
                classInfo.constitution = input.ReadInt32();
                classInfo.intelligence = input.ReadInt32();
                classInfo.wisdom = input.ReadInt32();
                classInfo.charisma = input.ReadInt32();
                */

                // Read in Base Data
                classInfo.baseHealth = input.ReadInt32();
                classInfo.baseMana = input.ReadInt32();
                classInfo.baseDamage = input.ReadInt32();
                classInfo.baseDefense = input.ReadInt32();

                // Read in Level Up Info
                classInfo.gainHealth = input.ReadInt32();
                classInfo.gainMana = input.ReadInt32();
                classInfo.gainDamage = input.ReadInt32();
                classInfo.gainDefense = input.ReadInt32();

                return classInfo;
            }
        }

        #endregion
    }
}
