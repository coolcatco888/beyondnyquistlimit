using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame.Components.Display
{
    class LevelTextComponent2D : TextComponent2D, INumeric
    {
        private int level;

        private const string lv = "Lv. ";

        public int Value
        {
            set
            {
                base.Text = lv + value;
                level = value;
            }
            get { return level; }

        }

        public LevelTextComponent2D(GameScreen parent, Vector2 position, int level, Color color, SpriteFont font, float scale)
            : base(parent, position, lv + level, color, font, scale)
        {
            this.level = level;
        }
    }
}
