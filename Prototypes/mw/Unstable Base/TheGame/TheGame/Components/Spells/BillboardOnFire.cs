#region Using Statements

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

#endregion  // Using Statements

namespace TheGame
{
    public class BillboardOnFire : Billboard
    {
        PointSpriteSystem pss_smoke;
        float timer = 0.0f;
        float particlesPerSecond;

        public BillboardOnFire(GameScreen parent, Texture2D texture2D)
            : base(parent, texture2D)
        { }

        public override void Initialize()
        {
            base.Initialize();

            PointSpriteSystemSettings settings = new PointSpriteSystemSettings();
            settings.Color = Color.OrangeRed;
            settings.MaxParticles = 500;
            settings.BasePosition = this.Position;
            settings.BaseRotation = Quaternion.Identity;
            settings.Scale = 1.0f;
            settings.Texture = GameEngine.Content.Load<Texture2D>("ParticleA");
            settings.Technique = "Cylindrical";

            particlesPerSecond = 200.0f;

            pss_smoke = new PointSpriteSystem(this.Parent, settings);
            pss_smoke.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            pss_smoke.Setting.BasePosition = this.Position;

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > 10 && this.Enabled)
            {
                timer = 0;

                int particlesToMake = (int)(particlesPerSecond * 0.01f);

                float theta = (float)MathHelper.Pi * 2.0f;
                float r = 0.2f;

                pss_smoke.AddParticle(
                    new Vector3(r * (float)GameEngine.Random.NextDouble(), theta * (float)GameEngine.Random.NextDouble(), 0.0f),
                    new Vector3(0.3f, 2.0f, 3.0f), 0.8f, 1.3f, Color.OrangeRed, null);
            }
        }
    }
}
