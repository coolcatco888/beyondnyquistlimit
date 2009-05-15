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
            get { return duration - timeRemaining; }
        }
        public float Duration
        {
            get { return duration; }
        }
        public float TimeRemaining
        {
            get { return timeRemaining; }
            set { timeRemaining = value; }
        }
        protected float duration;
        protected float timeRemaining;

        public Spell(GameScreen parent, float duration)
            : base(parent)
        {
            this.duration = duration;         
        }

        public override void Initialize()
        {
            this.timeRemaining = duration;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            timeRemaining -= gameTime.ElapsedGameTime.Milliseconds * 0.001f;
            
            if (TimeRemaining <= 0)
            {
                this.Dispose();
            }
        }

    }
}
