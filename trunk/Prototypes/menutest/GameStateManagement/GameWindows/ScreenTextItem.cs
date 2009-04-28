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
    class ScreenTextItem : ScreenItem
    {
        #region Fields

        private string text;

        private Color color;

        private SpriteFont font;

        private Vector2 position;

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

        public Vector2 Position
        {
            set { position = value; }
            get { return position; }
        }

        #endregion

        public ScreenTextItem(string text, Color color, SpriteFont font, Vector2 position)
        {
            this.text = text;
            this.color = color;
            this.font = font;
            this.position = position;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, text, position, color);
        }
    }
}
