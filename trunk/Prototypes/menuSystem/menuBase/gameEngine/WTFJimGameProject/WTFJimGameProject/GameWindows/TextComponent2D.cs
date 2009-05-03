using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WTFJimGameProject.GameWindows
{
    /// <summary>
    /// Represents a text item to be drawn on the screen using screen coordinates
    /// </summary>
    class TextComponent2D : Component, I2DComponent
    {
        #region Fields

        private Vector2 position;

        private SpriteBatch spriteBatch;

        private string text;

        private Color color;

        private SpriteFont font;

        #endregion

        #region Properties

        public Vector2 Position
        {
            set { position = value; }
            get { return position; }
        }

        public SpriteBatch SpriteBatch
        {
            set { spriteBatch = value; }
            get { return spriteBatch; }
        }

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
            this.spriteBatch = parent.Owner.SpriteBatch;
            
        }

        public override void Draw()
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, text, position, color);
            spriteBatch.End();
        }

    }
}
