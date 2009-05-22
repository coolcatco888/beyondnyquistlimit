using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame.Components.Display
{
    /// <summary>
    /// This represents a text component that when created 
    /// will appear and then disapppear after a short moment in time.
    /// </summary>
    class HitTextComponent2D : TextComponent2D
    {
        const int maxAliveTime = 30;

        const int positionIncrement = -1;

        const byte alphaIncrements = 25;

        private Random random;

        private byte i = 0;

        /// <summary>
        /// Creates the default Hit text component with a specified value.  The size of the text
        /// will be determined by how large the number value is, values greater or equal to 100
        /// will be displayed at the normal size,  values greater or equal to 200 will be displayed
        /// 25% larger otherwise smaller values will appear 25% smaller.
        /// </summary>
        /// <param name="parent">Game screen this object is displayed on</param>
        /// <param name="position">Position the text will display at</param>
        /// <param name="value">Value of hit</param>
        /// <param name="color">Color of hit text</param>
        /// <param name="font">Font of hit text</param>
        public HitTextComponent2D(GameScreen parent, Vector2 position, int value, Color color, SpriteFont font) 
            : this(parent, position, value, 100, 200, color, font) 
        {
        }

        /// <summary>
        /// Creates the Hit text component with a specified value. The size of the text
        /// will be determined by how large the number value is, values greater or equal to midLevel
        /// will be displayed at the normal size,  values greater or equal to highLevel will be displayed
        /// 25% larger otherwise smaller values will appear 25% smaller.
        /// </summary>
        /// <param name="parent">Game screen this object is displayed on</param>
        /// <param name="position">Position the text will display at</param>
        /// <param name="value">Value of hit</param>
        /// <param name="midLevel">Values greater or equal to this will be displayed 25% smaller</param>
        /// <param name="highLevel">Values greater or equl to this will be displayed 25% larger</param>
        /// <param name="color">Color of hit text</param>
        /// <param name="font">Font of hit text</param>
        public HitTextComponent2D(GameScreen parent, Vector2 position, int value, int midLevel, int highLevel, Color color, SpriteFont font) 
            : this(parent, position, (value >= 0? "+" : "") + value + "", color, font)
        {
            base.scale = Math.Abs(value) >= highLevel ? 1.25f : Math.Abs(value) >= midLevel ? 1.0f : 0.75f;
        }

        /// <summary>
        /// Creates a hit text with a specified text.
        /// </summary>
        /// <param name="parent">Game screen this object is displayed on</param>
        /// <param name="position">Position the text will display at</param>
        /// <param name="text">Text to be displayed</param>
        /// <param name="color">Color of hit text</param>
        /// <param name="font">Font of hit text</param>
        public HitTextComponent2D(GameScreen parent, Vector2 position, string text, Color color, SpriteFont font)
            : this(parent, position, text, color, font, 1.0f)
        {
        }

        /// <summary>
        /// Creates a hit text with a specified text and scale.
        /// </summary>
        /// <param name="parent">Game screen this object is displayed on</param>
        /// <param name="position">Position the text will display at</param>
        /// <param name="text">Text to be displayed</param>
        /// <param name="color">Color of hit text</param>
        /// <param name="font">Font of hit text</param>
        /// <param name="scale">1.0f for a normal size</param>
        public HitTextComponent2D(GameScreen parent, Vector2 position, string text, Color color, SpriteFont font, float scale)
            : base(parent, position, text, color, font, scale)
        {
            random = new Random();
            base.position.X = position.X + random.Next(10);
            base.position.Y = position.Y + random.Next(10);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
   
            if (i < maxAliveTime)
            {
                //FadeIn
                if (i < 10)
                {
                    base.color.A = (byte) ((i + 1) * alphaIncrements);
                }

                //Fadeout
                if (i >= 20)
                {
                    base.color.A = (byte) ((10 - (i - 19)) * alphaIncrements);
                }

                //Move position
                position.Y = position.Y + positionIncrement;
            }
            else if(Parent != null)
            {
                //Kill this component
                Dispose();
            }

            i++;
        }
    }
}
