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
        public FireTornado(GameScreen parent, SpellInfo spellInfo, Vector3 target)
            : base(parent, spellInfo)
        {
            this.target = target;
        }

        private static Random random = new Random();

        Vector3 target;

        PointSpriteSystem vortex;

        float particlesPerSecond;
        int timeElpase; 

        public override void Initialize()
        {

            base.Initialize();

            PointSpriteSystemSettings settings = new PointSpriteSystemSettings();
            settings.Color = Color.OrangeRed;
            settings.MaxPointCount = 5000;
            settings.ParticleDuration = 1.3f;
            settings.PointSpriteSize = 0.2f;
            settings.Position = target;
            settings.Rotation = Quaternion.Identity;
            settings.Scale = 1.0f;
            settings.SpriteTexture = GameEngine.Content.Load<Texture2D>("ParticleA");
            settings.Technique = "Cylindrical";

            particlesPerSecond = 1200.0f;

            vortex = new PointSpriteSystem(Parent, settings);
            Add(vortex);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            

            Position = target;

            timeElpase += gameTime.ElapsedGameTime.Milliseconds;

            if (timeElpase > 10)
            {
                timeElpase = 0;

                int particlesToMake = (int)(particlesPerSecond * 0.01f);

                float theta = (float)MathHelper.Pi * 2.0f;
                float r = 0.2f;

                for (int i = 0; i < particlesToMake; i++)
                {
                    vortex.AddParticle(new Vector3(r * (float)random.NextDouble(), theta * (float)random.NextDouble(), 0.0f), new Vector3(0.6f, 4.0f, 3.0f));
                }
            }
        }
    }
}
