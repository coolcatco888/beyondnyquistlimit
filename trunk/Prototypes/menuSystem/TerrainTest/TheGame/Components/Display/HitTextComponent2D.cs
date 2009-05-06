using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame.Components.Display
{
    class HitTextComponent2D : TextComponent2D
    {
        const int maxAliveTime = 30;

        const int positionIncrement = -1;

        const byte alphaIncrements = 25;

        private Random random;

        private byte i = 0;

        public HitTextComponent2D(GameScreen parent, Vector2 position, int value, Color color, SpriteFont font) 
            : this(parent, position, value, 100, 200, color, font) 
        {
        }

        public HitTextComponent2D(GameScreen parent, Vector2 position, int value, int midLevel, int highLevel, Color color, SpriteFont font) 
            : this(parent, position, (value >= 0? "+" : "") + value + "", color, font)
        {
            base.scale = Math.Abs(value) >= highLevel ? 1.25f : Math.Abs(value) >= midLevel ? 1.0f : 0.75f;
        }

        public HitTextComponent2D(GameScreen parent, Vector2 position, string text, Color color, SpriteFont font)
            : base(parent, position, text, color, font)
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
                Dispose();
            }

            i++;
        }
    }
}
