using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheGame.Components.Display
{
    /// <summary>
    /// Represents a character's status display.  All displays must contain
    /// a character's health, mana, and level.
    /// </summary>
    abstract class HUDStatusComponent2D : DisplayComponent2D
    {
        /// <summary>
        /// Stores the health bar component for the character
        /// </summary>
        protected IGauge healthBar;


        /// <summary>
        /// Stores the mana bar component for the character
        /// </summary>
        protected IGauge manaBar;


        /// <summary>
        /// Stores the level component for the character
        /// </summary>
        protected INumeric level;

        /// <summary>
        /// Stores the health bar data for the character
        /// </summary>
        public IGauge Healthbar
        {
            get { return healthBar; }
        }

        /// <summary>
        /// Stores the mana bar data for the character
        /// </summary>
        public IGauge ManaBar
        {
            get { return manaBar; }
        }

        /// <summary>
        /// Stores the level data for the character
        /// </summary>
        public INumeric Level
        {
            get { return level; }
        }

        protected HUDStatusComponent2D(GameScreen parent)
            : base(parent)
        {

        }

    }
}
