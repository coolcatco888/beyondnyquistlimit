using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGame.Components.Display;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame.Components.GUI
{
    class FrameRateCounterText2D : TextComponent2D
    {

        private int frameRate = 0;
        private int frameCounter = 0;
        private TimeSpan elapsedTime = TimeSpan.Zero;

        public int FramesPerSecond
        {
            get { return frameRate; }
        }

        public FrameRateCounterText2D(GameScreen parent, Vector2 position, Color color, SpriteFont font, float scale)
            : base(parent, position, "", color, font)
        {

        }

        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            frameCounter++;
            this.Text = "FPS: " + frameRate;
            base.Draw(gameTime);
        }


    }
}
