using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagement.GameWindows
{
    /// <summary>
    /// Represents a text item in a panel
    /// </summary>
    class TextComponent2D : Component2D
    {
        #region Fields

        private string text;

        private Color color;

        private SpriteFont font;

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

        public TextComponent2D(Vector2 position, string text, Color color, SpriteFont font)
        {
            this.position = position;
            this.text = text;
            this.color = color;
            this.font = font;
            
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, text, position, color);
            spriteBatch.End();
        }

    }
}
