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
        public Spell(GameScreen parent, SpellInfo spellInfo)
            : base(parent)
        {
            this.spellInfo = spellInfo;
        }

        public SpellInfo SpellInfo
        {
            get { return spellInfo; }
            set { spellInfo = value; }
        }
        SpellInfo spellInfo;

        protected float timeSpent;

        public override void Initialize()
        {
            timeSpent = 0.0f;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            timeSpent += gameTime.ElapsedGameTime.Milliseconds * 0.001f;
            
            if (timeSpent > spellInfo.Duration)
            {
                this.Dispose();
            }
        }

        protected float particlesPerSecond;
        public float ParticlesPerSecond
        {
            get { return particlesPerSecond; }
            set { particlesPerSecond = value; }
        }
    }
}
