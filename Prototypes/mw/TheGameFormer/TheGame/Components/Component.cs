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

    public class Component
    {
        //These paramaters should be fine left alone
        #region Parameters

        public bool Initialized
        {
            get { return initialized; }
            set { initialized = value; }
        }
        bool initialized;

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }
        bool enabled;

        public GameScreen Parent
        {
            get { return parent; }
            set { parent = value; }
        }
        GameScreen parent;

        #endregion

        //Note there is no default constructor
        #region Constructor

        public Component(GameScreen parent)
        {
            this.parent = parent;
            this.enabled = true;
        }

        #endregion

        // Section has the 3 main methods that should be overridden
        #region Methods

        public virtual void Initialize()
        {
            parent.Components.Add(this);
            initialized = true;
        }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Dispose()
        {
            parent.Components.Remove(this);
        }

        #endregion
    }
}
