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
    class TestScreen : GameScreen
    {
        PointSpriteSystem pss;
        double time;
        double angle;

        Random rand = new Random();

        public TestScreen(String name)
            : base(name)
        {
            new TestComponent(this);

            BasicModel temp;

            temp = new BasicModel(this, GameEngine.Content.Load<Model>("ig_box"));
            temp.Scale = 0.3f;
            temp.Position = new Vector3(1.0f, 1.2f, 0.3f);

            temp = new BasicModel(this, GameEngine.Content.Load<Model>("ig_box"));
            temp.Scale = 1.0f;
            temp.Position = new Vector3(0.0f, -2.2f, -4.5f);

            pss = new PointSpriteSystem(this);
        }

        public override void Update(GameTime gameTime)
        {
            time += gameTime.ElapsedGameTime.TotalSeconds;

                angle = rand.Next() * MathHelper.Pi * 2;

                pss.AddParticle(new Vector3((float)Math.Cos(angle), 0.0f , (float)Math.Sin(angle)), Vector3.Up);
                time = 0;
            

            base.Update(gameTime);
        }
    }
}
