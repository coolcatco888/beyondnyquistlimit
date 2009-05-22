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
        #region Fields

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

        public Component3D Caster
        {
            get { return caster; }
            set { caster = value; }
        }

        public Component3DList Targets
        {
            get { return targets; }
            set { targets = value; }
        }

        protected SpellInfo spellInfo;
        protected float timeRemaining;
        protected Component3D caster;
        protected Component3DList targets;

        #endregion

        #region Initialization

        public Spell(GameScreen parent, SpellInfo spellInfo, Component3D caster, Component3DList targets)
            : base(parent)
        {
            this.spellInfo = spellInfo;
            this.caster = caster;
            this.targets = targets;
        }

        public override void Initialize()
        {
            this.timeRemaining = this.spellInfo.Duration;

            base.Initialize();
        }

        #endregion

        #region Update

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

        #endregion  // Update

    }
}
