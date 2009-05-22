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

    public abstract class Component
    {
        //Public Fields shared by all components
        #region Fields

        //should be set once the component is usable and not altered after
        public bool Initialized
        {
            get { return initialized; }
            set { initialized = value; }
        }
        protected bool initialized;

        //If true the object is updated during update
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }
        protected bool enabled;

        //Parent screen holding component, affects update and draw order
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
        }

        #endregion

        // Section has the 3 main methods that should be overridden
        #region Methods

        public virtual void Initialize()
        {
            this.enabled = true;
            initialized = true;
            parent.Components.Add(this);
        }

        //Update component game logic
        public virtual void Update(GameTime gameTime) { }

        public virtual void Dispose()
        {
            parent.Components.Remove(this);
        }

        #endregion
    }
}
