﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WTFJimGameProject
{
    public abstract class GameScreen
    {
        #region Properties

        bool initialized;

        public bool Initialized
        {
            get { return initialized; }
            set { initialized = value; }
        }

        bool visible;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        bool enabled;

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        bool blocksUpdate;

        public bool BlocksUpdate
        {
            get { return blocksUpdate; }
            set { blocksUpdate = value; }
        }

        bool blocksDraw;

        public bool BlocksDraw
        {
            get { return blocksDraw; }
            set { blocksDraw = value; }
        }

        bool alwaysUpdate;

        public bool AlwaysUpdate
        {
            get { return alwaysUpdate; }
            set { alwaysUpdate = value; }
        }

        bool alwaysDraw;

        public bool AlwaysDraw
        {
            get { return alwaysDraw; }
            set { alwaysDraw = value; }
        }

        string name;

        public string Name
        {
            get { return name; }
        }

        ScreenManager owner;

        public ScreenManager Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        #endregion

        /// <summary>
        /// Gets the index of the player who is currently controlling this screen,
        /// or null if it is accepting input from any player. This is used to lock
        /// the game to a specific player profile. The main menu responds to input
        /// from any connected gamepad, but whichever player makes a selection from
        /// this menu is given control over all subsequent screens, so other gamepads
        /// are inactive until the controlling player returns to the main menu.
        /// </summary>
        public PlayerIndex? ControllingPlayer
        {
            get { return controllingPlayer; }
            internal set { controllingPlayer = value; }
        }

        PlayerIndex? controllingPlayer;

        ComponentCollection components;

        public ComponentCollection Components
        {
            get { return components; }
            set { components = value; }
        }

        public GameScreen(string name, ScreenManager owner)
        {
            this.owner = owner;
            this.name = name;
            this.visible = true;
            this.enabled = true;

            components = new ComponentCollection(this);

            owner.GameScreens.Add(this);

            if (!initialized)
                Initialize();
        }

        public virtual void Initialize()
        {
            initialized = true;
        }

        /// <summary>
        /// Load graphics content for the screen.
        /// </summary>
        public virtual void LoadContent() { }


        /// <summary>
        /// Unload content for the screen.
        /// </summary>
        public virtual void UnloadContent() { }

        /// <summary>
        /// Allows the screen to handle user input. Unlike Update, this method
        /// is only called when the screen is active, and not when some other
        /// screen has taken the focus.
        /// </summary>
        public virtual void HandleInput(InputState input) { }


        public virtual void Update(GameTime gameTime)
        {
            // Create a temporary list so we don't crash if
            // a component is added to the collection while
            // updating
            List<Component> updating = new List<Component>();

            // Populate the temporary list
            foreach (Component c in Components)
                updating.Add(c);

            // Update all components that have been initialized
            foreach (Component component in updating)
                if (component.Initialized && component.Enabled)
                    component.Update(gameTime);
        }

        public virtual void Draw()
        {
            // Temporary list
            List<Component> drawing = new List<Component>();

            //TODO: Change this so it's good
            foreach (Component component in Components)
            {
                //if (component is I2DComponent)
                //{
                    drawing.Add(component);
                //}
            }   

            foreach (Component component in drawing)
                component.Draw();
        }

        public virtual void Dispose()
        {
            components.Clear();
            owner.GameScreens.Remove(this);
        }
    }
}
