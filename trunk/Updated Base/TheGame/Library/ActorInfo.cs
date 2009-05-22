using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    /// <summary>
    /// Information that an Actor (i.e. Player and Monster) will have
    /// </summary>
    public class ActorInfo
    {
        /// <summary>
        /// Max health of the actor. For a player max health represents
        /// the total health they have calculated from level the base gain and base amount.
        /// Monsters just have a fixed max health
        /// </summary>
        private int maxHealth;
        public int MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

        /// <summary>
        /// Similar to max health, this represents the max amount of mana that an actor has
        /// </summary>
        private int maxMana;
        public int MaxMana
        {
            get { return maxMana; }
            set { maxMana = value; }
        }

        /// <summary>
        /// The current amount of health. If actor has not been damage,
        /// current health will equal max health. For players, this value
        /// along with max health will be represented by a health bar on the HUD
        /// </summary>
        private int currentHealth;
        public int CurrentHealth
        {
            get { return currentHealth; }
            set { currentHealth = value; }
        }

        /// <summary>
        /// Similar to current health, this represents the actor's current
        /// mana value. Again, for players will be represented by a mana bar on
        /// the HUD
        /// </summary>
        private int currentMana;
        public int CurrentMana
        {
            get { return currentMana; }
            set { currentMana = value; }
        }

        /// <summary>
        /// For a player, represents the calculated total damage, for a monster,
        /// this is a fixed value.
        /// </summary>
        private int currentDamage;
        public int CurrentDamage
        {
            get { return currentDamage; }
            set { currentDamage = value; }
        }

        /// <summary>
        /// Similar to damage, but represents defense
        /// </summary>
        private int currentDefense;
        public int CurrentDefense
        {
            get { return currentDefense; }
            set { currentDefense = value; }
        }

        public void PopulateFields(PlayerInfo playerStats, CharacterClassInfo charStats)
        {
            maxHealth = charStats.BaseHealth + (playerStats.ClassLevel * charStats.GainHealth);
            currentHealth = maxHealth;

            maxMana = charStats.BaseMana + (playerStats.ClassLevel * charStats.GainMana);
            currentMana = maxMana;

            currentDamage = charStats.BaseDamage + (playerStats.ClassLevel * charStats.BaseDamage);
            currentDefense = charStats.BaseDefense + (playerStats.ClassLevel * charStats.GainDefense);
        }

        public void PopulateFields(MonsterInfo monsterStats)
        {
            maxHealth = monsterStats.Health;
            currentHealth = maxHealth;

            maxMana = monsterStats.Mana;
            currentMana = maxMana;

            currentDamage = monsterStats.Damage;
            currentDefense = monsterStats.Defense;
        }
    }
}
