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

            PointSpriteSystemSettings psss = new PointSpriteSystemSettings();
            psss.Color = Color.Green;
            psss.MaxPointCount = 5000;
            psss.ParticleDuration = 1.0f;
            psss.PointSpriteSize = 0.2f;
            psss.Position = Vector3.Zero;
            psss.SpriteTexture = GameEngine.Content.Load<Texture2D>("ParticleA");
            psss.Technique = "Spherical";

            pss = new PointSpriteSystem(this, psss);
        }

        public override void Update(GameTime gameTime)
        {
            time += gameTime.ElapsedGameTime.TotalSeconds;

            //for (int i = 0; i < gameTime.ElapsedGameTime.Milliseconds; i++ )
            //{
                time = 0;
                double theta = rand.NextDouble() * MathHelper.Pi * 2;
                double phi = rand.NextDouble() * MathHelper.Pi * 2;
                pss.AddParticle(new Vector3(1.0f, (float)theta, (float)phi), new Vector3(-1.0f, 0.0f, 0.0f));
                
            //}
            

            base.Update(gameTime);
        }
    }
}
