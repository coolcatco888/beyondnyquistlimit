﻿using System;
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

namespace WTFJimGameProject
{

    public class Component
    {
        //These paramaters should be fine left alone
        #region Parameters

        private bool visible;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

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

        #endregion

        //This should generally not be touched
        #region attributes

        GameScreen parent;

        public GameScreen Parent
        {
            set { parent = value; }
            get { return parent; }
        }

        #endregion

        //Note there is no default constructor
        #region Constructor

        public Component(GameScreen parent)
        {
            this.parent = parent;
            this.enabled = true;
            this.visible = true;

            Initialize(parent);
        }

        #endregion

        // Section has the 3 main methods that should be overridden
        #region Methods

        public virtual void Initialize(GameScreen parent)
        {
            //TODO: add component to parent
            initialized = true;
        }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw() { }

        public virtual void Dispose()
        {
            parent.Components.Remove(this);
        }

        #endregion
    }
}
