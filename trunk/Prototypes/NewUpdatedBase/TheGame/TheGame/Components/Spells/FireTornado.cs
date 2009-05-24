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
    class FireTornado : Spell
    {
        PointSpriteSystem tornado;

        float tornadoParticlesPerSecond;
        float tornadoParticlesToMake;
        private float damageTicker;

        public FireTornado(GameScreen parent, SpellInfo spellInfo, Player caster, List<Actor> targets)
            : base(parent, spellInfo, caster, targets)
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            tornadoParticlesPerSecond = 750.0f;
            tornadoParticlesToMake = 0.0f;
            damageTicker = 0.0f;

            PointSpriteSystemSettings tornadoSettings = new PointSpriteSystemSettings("BasicPoint", "Cylindrical", GameEngine.Content.Load<Texture2D>("Sprites\\explosion"));
            tornadoSettings.MaxParticles = 4000;
            tornadoSettings.Color = Color.White;
            tornado = new PointSpriteSystem(Parent, tornadoSettings);

            if (targets.Count != 0)
            {
                ((Monster)targets[0]).isStunned = true;
                ((Monster)targets[0]).stunDuration = this.Duration;
                ((Monster)targets[0]).Collidable = false;
            }

            tornado.Initialize();

        }

        public override void Update(GameTime gameTime)
        {
            if (targets.Count == 0)
            {
                timeRemaining = 0.0f;
                ((Monster)targets[0]).Collidable = true;
                return;
            }

            tornado.Position = new Vector3(targets[0].Position.X, 0.0f, targets[0].Position.Z);

            if (TimeRemaining > 0.5f)
            {
                if (targets[0].Position.Y < 6.0f)
                    targets[0].Translate(Vector3.UnitY * 3 * (float)gameTime.ElapsedGameTime.TotalSeconds);

                tornadoParticlesToMake += tornadoParticlesPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;

                while (tornadoParticlesToMake >= 1.0f)
                {
                    tornado.AddParticle(
                        new Vector3(GameEngine.Frand, GameEngine.Frand * 2 * (float)Math.PI, 0.0f),
                        new Vector3(GameEngine.Frand * 2, 5.0f + GameEngine.Frand * 5, 7.0f + GameEngine.Frand * 6),
                        GameEngine.Frand + 2.0f,
                        new Vector2(1.9f, 1.1f),
                        null,
                        1.0f);

                    tornadoParticlesToMake--;
                }

                damageTicker += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (damageTicker > spellInfo.TickFrequency)
                {
                    damageTicker = 0.0f;
                    ((Monster)targets[0]).ApplyDamage(spellInfo.Damage);
                }

            }
            else
            {
                targets[0].Position = new Vector3(targets[0].Position.X, Math.Max(timeRemaining * 12, 1.0f), targets[0].Position.Z);
            }


            if (timeRemaining <= 0.0f)
            {
                ((Monster)targets[0]).Collidable = true;
            }

            base.Update(gameTime);
        }
    }
}