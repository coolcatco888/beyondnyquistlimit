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
    public class Spell : Container3D
    {
        public float Age
        {
            get { return spellInfo.Duration - timeRemaining; }
        }
        public float Duration
        {
            get { return spellInfo.Duration; }
        }
        public float TimeRemaining
        {
            get { return timeRemaining; }
            set { timeRemaining = value; }
        }
        public SpellInfo SpellInfo
        {
            get { return this.spellInfo; }
        }
        public float ParticlesPerSecond
        {
            get { return particlesPerSecond; }
            set { this.particlesPerSecond = value; }
        }
        protected float particlesPerSecond;
        protected SpellInfo spellInfo;
        protected float timeRemaining;

        public Spell(GameScreen parent, float duration)
            : base(parent)
        {
            this.spellInfo = new SpellInfo();
            this.spellInfo.Duration = duration;         
        }

        public Spell(GameScreen parent, SpellInfo spellInfo)
            : base(parent)
        {
            this.spellInfo = spellInfo;
        }

        public override void Initialize()
        {
            this.timeRemaining = this.spellInfo.Duration;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (spellInfo.Duration > 0)
            {
                timeRemaining -= gameTime.ElapsedGameTime.Milliseconds * 0.001f;

                if (TimeRemaining <= 0)
                {
                    this.Dispose();
                }
            }
        }

    }
}
