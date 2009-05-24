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

namespace TheGame
{
    public abstract class DrawableComponent : Component, IDrawableComponent
    {
        #region Initialization

        public DrawableComponent(GameScreen parent)
            : base(parent)
        {
        }

        //Default is visible
        public override void Initialize()
        {
            visible = true;
            base.Initialize();
        }

        #endregion

        #region IDrawable Members

        //Overide to add own draw code, call base.draw and override SetSpecialRenderStates
        //to add own render states.                                                                                                                        
        public virtual void Draw(GameTime gameTime)
        {
            SetSpecialRenderStates();
        }

        //true means the component is drawn
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }
        protected bool visible;

        #endregion

        #region Helper Methods

        //Override this and use base.draw (at TOP of method) to set special render options
        protected virtual void SetSpecialRenderStates()
        {
        }

        #endregion
    }
}