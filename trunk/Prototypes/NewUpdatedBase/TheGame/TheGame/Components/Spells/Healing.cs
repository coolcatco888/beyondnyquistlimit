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
    class Healing : Spell
    {
        PointSpriteSystem ring;

        float ringParticlesPerSecond;
        float ringParticlesToMake;

        float dustParticlesPerSecond;
        float dustParticlesToMake;

        float healingTickTimer;

        public Healing(GameScreen parent, SpellInfo spellInfo, Player caster, List<Actor> targets)
            : base(parent, spellInfo, caster, targets)
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            ringParticlesPerSecond = 200.0f;
            ringParticlesToMake = 0.0f;

            healingTickTimer = 0.0f;

            dustParticlesPerSecond = 20;
            dustParticlesToMake = 0;

            PointSpriteSystemSettings ringSettings = new PointSpriteSystemSettings("BasicPoint", "Cylindrical", GameEngine.Content.Load<Texture2D>("ParticleA"));
            ringSettings.MaxParticles = 4000;
            ringSettings.Color = Color.White;
            ring = new PointSpriteSystem(Parent, ringSettings);
            ring.Initialize();

        }

        public override void Update(GameTime gameTime)
        {
            if (targets.Count == 0)
            {
                timeRemaining = 0.0f;
                return;
            }

            ringParticlesToMake += ringParticlesPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;
            dustParticlesToMake += dustParticlesPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;

            ring.Position = caster.Position - Vector3.UnitY * 1.5f;

            while (ringParticlesToMake >= 1.0f)
            {
                ring.AddParticle(new Vector3(1.6f, GameEngine.Frand * 2 * (float)Math.PI, 0.0f), Vector3.UnitY, 2.0f, new Vector2(0.2f + GameEngine.Frand * 0.1f, 0.2f), null, null);

                ringParticlesToMake--;
            }

            while (dustParticlesToMake >= 1.0f)
            {
                ring.AddParticle(new Vector3(1.6f * GameEngine.Frand, GameEngine.Frand * 2 * (float)Math.PI, 4.0f), -Vector3.UnitZ * (GameEngine.Frand * 0.7f + 0.6f), 4.5f, new Vector2(0.2f + GameEngine.Frand * 0.3f, 0.3f), null, null);

                dustParticlesToMake--;
            }

            healingTickTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (healingTickTimer >= spellInfo.TickFrequency)
            {
                targets[0].ApplyDamage(-spellInfo.Damage);
                healingTickTimer = 0.0f;
            }

            base.Update(gameTime);
        }
    }
}
