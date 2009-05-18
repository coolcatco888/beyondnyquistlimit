
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
    public class CircularVoid : Spell
    {
        #region Fields

        PointSpriteSystemInverted pss_void;
        BillboardCauldron billboardCauldron;
        Player player;

        float timer = 0.0f;

        #endregion  //Fields

        public CircularVoid(GameScreen parent, SpellInfo spellInfo, Player player)
            : base(parent, spellInfo.Duration)
        {
            this.player = player;
        }

        public override void Initialize()
        {
            base.Initialize();

            this.Position = new Vector3(player.Position.X, 0.0f, player.Position.Z);

            // Particle System
            PointSpriteSystemSettings settings = new PointSpriteSystemSettings();

            settings.Color = Color.LightGray;
            settings.MaxParticles = 5000;
            settings.BasePosition = Vector3.Zero;
            settings.BaseRotation = Quaternion.Identity;
            settings.Scale = 1.0f;
            settings.Texture = GameEngine.Content.Load<Texture2D>("ParticleA");
            settings.Technique = "Cylindrical";

            particlesPerSecond = 500.0f;

            pss_void = new PointSpriteSystemInverted(Parent, settings);
            pss_void.Initialize();

            particlesPerSecond = 10000;


            // Billboard Cauldron
            Library.SpriteInfo cauldronInfo = GameEngine.Content.Load<Library.SpriteInfo>(@"Sprites\\CloudInfo");
            billboardCauldron = new BillboardCauldron(this.Parent, cauldronInfo, this.position,
                1.3f, 50, 0.3f, 1, 0, 8, 2);
            billboardCauldron.Initialize();

            this.Add(pss_void);
            this.Add(billboardCauldron);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.Position = new Vector3(player.Position.X, 0.0f, player.Position.Z);
            this.billboardCauldron.Origin = this.Position;
            
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            while (timer > 0)
            {
                int particlesToMake = (int)(particlesPerSecond * 0.01f);

                //float theta = (float)MathHelper.Pi * 2.0f;

                for (int i = 0; i < particlesToMake; i++)
                {
                    float randomColor = (float)GameEngine.Random.NextDouble() * 0.95f;

                    pss_void.AddParticle(
                        new Vector3((float)GameEngine.Random.NextDouble() * 2.0f, (float)Math.PI * 2.0f * (float)GameEngine.Random.NextDouble(), -0.2f),
                        new Vector3(0.0f, (float)GameEngine.Random.NextDouble(), (float)GameEngine.Random.NextDouble() * 0.5f),
                        (float)GameEngine.Random.NextDouble() * 0.6f, 1.0f,
                        new Color(0.08f, 0.08f, 0.08f, 1.0f), //new Color(randomColor, 0.8f, randomColor, 0.8f),
                        null);
                }

                timer -= 40;
            }

            if (this.TimeRemaining <= 0)
            {
                billboardCauldron.Dispose();
            }

            // TEMPORARY
            this.TimeRemaining = 5.0f;
        }
    }
}
