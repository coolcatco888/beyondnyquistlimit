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
using Library;

namespace TheGame
{
    public class FireTornado : Spell
    {
        public FireTornado(GameScreen parent, Vector3 target, float duration)
            : base(parent, duration)
        {
            this.target = target;
        }

        Vector3 target;

        PointSpriteSystem vortex;

        float particlesPerSecond;
        int timeElpase; 

        public override void Initialize()
        {

            base.Initialize();

            PointSpriteSystemSettings settings = new PointSpriteSystemSettings();
            settings.Color = Color.OrangeRed;
            settings.BasePosition = target;
            settings.Texture = GameEngine.Content.Load<Texture2D>("ParticleA");
            settings.Technique = "Cylindrical";

            particlesPerSecond = 800.0f;

            vortex = new PointSpriteSystem(Parent, settings);
            vortex.Initialize();
            Add(vortex);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            KeyboardDevice kbd = (KeyboardDevice)GameEngine.Services.GetService(typeof(KeyboardDevice));

            if (kbd.IsKeyDown(Keys.Up))
            {
                Position += -Vector3.UnitZ * 0.01f * gameTime.ElapsedGameTime.Milliseconds;
            }
            if (kbd.IsKeyDown(Keys.Left))
            {
                Position += -Vector3.UnitX * 0.01f * gameTime.ElapsedGameTime.Milliseconds;
            }
            if (kbd.IsKeyDown(Keys.Down))
            {
                Position += Vector3.UnitZ * 0.01f * gameTime.ElapsedGameTime.Milliseconds;
            }
            if (kbd.IsKeyDown(Keys.Right))
            {
                Position += Vector3.UnitX * 0.01f * gameTime.ElapsedGameTime.Milliseconds;
            }

            timeElpase += gameTime.ElapsedGameTime.Milliseconds;

            if (timeElpase > 10)
            {
                timeElpase = 0;

                int particlesToMake = (int)(particlesPerSecond * 0.01f);

                float theta = (float)MathHelper.Pi * 2.0f;
                float r = 0.4f;

                float rand1, rand2, rand3;

                for (int i = 0; i < particlesToMake; i++)
                {
                    rand1 = (float)GameEngine.Random.NextDouble();
                    rand2 = (float)GameEngine.Random.NextDouble();
                    rand3 = (float)GameEngine.Random.NextDouble();

                    vortex.AddParticle(new Vector3(r * rand1, theta * rand2, 0.2f), new Vector3(0.6f, 3.3f, 2.1f), 0.1f * rand3 + 0.1f, rand1 + rand2, 0.0f, null);
                }
            }
        }
    }
}
