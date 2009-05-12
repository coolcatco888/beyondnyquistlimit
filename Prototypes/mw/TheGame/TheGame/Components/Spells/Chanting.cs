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
    public class Chanting : Spell
    {
        public Chanting(GameScreen parent, SpellInfo spellInfo, Vector3 position)
            : base(parent, spellInfo)
        {
            this.Position = position;
        }

        private static Random random = new Random();

        PointSpriteSystem circleEffect;

        float particlesPerSecond;
        int timeElpase; 

        public override void Initialize()
        {

            base.Initialize();

            PointSpriteSystemSettings settings = new PointSpriteSystemSettings();
            settings.Color = Color.LightBlue;
            settings.MaxPointCount = 5000;
            settings.ParticleDuration = 1.0f;
            settings.PointSpriteSize = 0.1f;
            settings.Position = Vector3.Zero;
            settings.Rotation = Quaternion.Identity;
            settings.Scale = 1.0f;
            settings.SpriteTexture = GameEngine.Content.Load<Texture2D>("ParticleA");
            settings.Technique = "Cylindrical";

            particlesPerSecond = 500.0f;

            circleEffect = new PointSpriteSystem(Parent, settings);
            Add(circleEffect);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            timeElpase += gameTime.ElapsedGameTime.Milliseconds;

            if (timeElpase > 10)
            {
                timeElpase = 0;

                int particlesToMake = (int)(particlesPerSecond * 0.01f);

                float theta = (float)MathHelper.Pi * 2.0f;

                for (int i = 0; i < particlesToMake; i++)
                {
                    circleEffect.AddParticle(new Vector3(1.0f * scale, theta * (float)random.NextDouble(), 0.0f), new Vector3(0.0f, 0.0f, 2.0f + (float)random.NextDouble()));
                }
            }
        }
    }
}
