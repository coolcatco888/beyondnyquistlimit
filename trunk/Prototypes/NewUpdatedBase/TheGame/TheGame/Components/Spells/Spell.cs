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

        public Actor Caster
        {
            get { return caster; }
            set { caster = value; }
        }

        public List<Actor> Targets
        {
            get { return targets; }
            set { targets = value; }
        }

        protected SpellInfo spellInfo;
        protected float timeRemaining;
        protected Actor caster;
        protected List<Actor> targets;

        #endregion

        #region Initialization

        public Spell(GameScreen parent, SpellInfo spellInfo, Actor caster, List<Actor> targets)
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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (spellInfo.Duration > 0)
            {
                timeRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (TimeRemaining <= 0)
                {
                    caster.State = Actor.ActorState.Idle;
                    ((Player)caster).NotCasting = true;
                    ((Player)caster).spellName = "";
                    this.Dispose();
                }
            }
        }
    }
}
