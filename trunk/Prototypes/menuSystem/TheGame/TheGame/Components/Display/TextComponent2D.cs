using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame.Components.Display
{
    /// <summary>
    /// Represents a text item to be drawn on the screen using screen coordinates
    /// </summary>
    class TextComponent2D : DisplayComponent2D
    {
        #region Fields

        protected string text;

        protected Color color;

        protected SpriteFont font;

        #endregion

        #region Properties


        public string Text 
        { 
            set { text = value; } 
            get { return text; } 
        }

        public Color Color
        {
            set { color = value; }
            get { return color; }
        }

        public SpriteFont Font
        {
            set { font = value; }
            get { return font; }
        }

        #endregion

        public TextComponent2D(GameScreen parent, Vector2 position, string text, Color color, SpriteFont font) : base(parent)
        {
            this.position = position;
            this.text = text;
            this.color = color;
            this.font = font;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, text, position, color);
            spriteBatch.End();
        }

    }
}
