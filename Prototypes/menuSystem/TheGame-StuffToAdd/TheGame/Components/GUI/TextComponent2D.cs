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

        protected float scale;

        #endregion

        #region Properties

        /// <summary>
        /// Displayed text
        /// </summary>
        public string Text 
        { 
            set { text = value; } 
            get { return text; } 
        }

        /// <summary>
        /// Color of text
        /// </summary>
        public Color Color
        {
            set { color = value; }
            get { return color; }
        }

        /// <summary>
        /// Font of text
        /// </summary>
        public SpriteFont Font
        {
            set { font = value; }
            get { return font; }
        }

        /// <summary>
        /// Scale of text
        /// </summary>
        public float Scale
        {
            set { scale = value; }
            get { return scale; }
        }

        #endregion

        /// <summary>
        /// Creates screen text to be displayed.
        /// </summary>
        /// <param name="parent">Game screen this object is displayed on</param>
        /// <param name="position">Position the text will display at</param>
        /// <param name="text">Text to be displayed</param>
        /// <param name="color">Color of hit text</param>
        /// <param name="font">Font of hit text</param>
        public TextComponent2D(GameScreen parent, Vector2 position, string text, Color color, SpriteFont font)
            : this(parent, position, text, color, font, 1.0f)
        {

        }

        /// <summary>
        /// Creates screen text to be displayed with specified scale.
        /// </summary>
        /// <param name="parent">Game screen this object is displayed on</param>
        /// <param name="position">Position the text will display at</param>
        /// <param name="text">Text to be displayed</param>
        /// <param name="color">Color of text</param>
        /// <param name="font">Font of text</param>
        /// <param name="scale">1.0f for a normal size</param>
        public TextComponent2D(GameScreen parent, Vector2 position, string text, Color color, SpriteFont font, float scale) : base(parent)
        {
            this.position = position;
            this.text = text;
            this.color = color;
            this.font = font;
            this.scale = scale;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, text, position, color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
            spriteBatch.End();
        }


        public override Vector2 Center
        {
            get { return new Vector2(Left + 0.5f * Width, Top + 0.5f * Height);  }
        }

        public override float Height
        {
            get { return  font.MeasureString(text).Y * scale; }
        }

        public override float Width
        {
            get { return font.MeasureString(text).X * scale; }
        }

        public override float Left
        {
            get { return position.X;  }
        }

        public override float Right
        {
            get { return Left + Width;  }
        }

        public override float Bottom
        {
            get { return Top + Height;  }
        }

        public override float Top
        {
            get { return position.Y; }
        }
    }
}
