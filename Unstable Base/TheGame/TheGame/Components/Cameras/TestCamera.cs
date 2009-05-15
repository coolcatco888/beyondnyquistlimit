using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace TheGame
{
    class TestCamera : Camera
    {
        double theta;
        double phi;

        public TestCamera(GameScreen parent)
            : base(parent)
        {
        }

        public override void Initialize()
        {
            theta = 0.0f;
            phi = 0.0f;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardDevice kbd = (KeyboardDevice)GameEngine.Services.GetService(typeof(KeyboardDevice));

            if (kbd.IsKeyDown(Keys.Up))
            {
                theta += 0.001 * gameTime.ElapsedGameTime.Milliseconds;
            }
            if (kbd.IsKeyDown(Keys.Left))
            {
                phi += 0.001 * gameTime.ElapsedGameTime.Milliseconds;
            }
            if (kbd.IsKeyDown(Keys.Down))
            {
                theta -= 0.001 * gameTime.ElapsedGameTime.Milliseconds;
            }
            if (kbd.IsKeyDown(Keys.Right))
            {
                phi -= 0.001 * gameTime.ElapsedGameTime.Milliseconds;
            }

            Position = new Vector3((float)(Math.Cos(theta) * Math.Sin(phi) * 5), (float)(Math.Sin(theta) * Math.Sin(phi) * 5), (float)(Math.Cos(theta) * 5));

            base.Update(gameTime);
        }
    }
}
