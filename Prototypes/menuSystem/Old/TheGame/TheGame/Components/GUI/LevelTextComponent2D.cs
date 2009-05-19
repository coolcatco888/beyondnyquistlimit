using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame.Components.Display
{
    /// <summary>
    /// A text component specifically for storing a character's level
    /// </summary>
    class LevelTextComponent2D : TextComponent2D, INumeric
    {
        /// <summary>
        /// Stores the level of the character
        /// </summary>
        private int level;

        /// <summary>
        /// "Lv." is appended to the numerical value of the text string
        /// </summary>
        private const string lv = "Lv. ";

        /// <summary>
        /// Level of the character
        /// </summary>
        public int Value
        {
            set
            {
                base.Text = lv + value;
                level = value;
            }
            get { return level; }

        }
        /// <summary>
        /// Creates a level Text component
        /// </summary>
        /// <param name="parent">Game screen this object is displayed on</param>
        /// <param name="position">Position the text will display at</param>
        /// <param name="level">Level of the character</param>
        /// <param name="color">Color of text</param>
        /// <param name="font">Font of text</param>
        /// <param name="scale">1.0f for a normal size</param>
        public LevelTextComponent2D(GameScreen parent, Vector2 position, int level, Color color, SpriteFont font, float scale)
            : base(parent, position, lv + level, color, font, scale)
        {
            this.level = level;
        }
    }
}
