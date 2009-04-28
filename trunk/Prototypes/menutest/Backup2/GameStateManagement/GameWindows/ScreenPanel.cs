using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameStateManagement.GameWindows
{
    /// <summary>
    /// Represents a 2D panel to be drawn on a screen. This implementation
    /// is intended to be the base for menu windows, alert boxes, dialog boxes and HUDs.
    /// This is essentially a collection of panel items.
    /// </summary>
    class ScreenPanel
    {
        private Vector2 position;

        private Texture2D image;

        private Color tint;

        protected List<ScreenItem> panelItems = new List<ScreenItem>();

        public List<ScreenItem> PanelItems
        {
            get { return panelItems; }
        }

        public Vector2 Position
        {
            set { position = value; }
            get { return position; }
        }

        public Texture2D Image
        {
            set { image = value; }
            get { return image; }
        }

        public Color Tint
        {
            set { tint = value; }
            get { return tint; }
        }

        /// <summary>
        /// Creates a panel without an image, preferable for an HUD system
        /// </summary>
        public ScreenPanel() : this (new Vector2(), null)
        {
        }

        /// <summary>
        /// Creates a panel with a background image, keep in mind panel items can be placed
        /// outside of a menu but generally shouldn't.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="image"></param>
        public ScreenPanel(Vector2 position, Texture2D image) :  this (position, image, Color.White)
        {
        }


        /// <summary>
        /// Creates a panel with a background image with a tint color, keep in mind panel items can be placed
        /// outside of a menu but generally shouldn't.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="image"></param>
        /// <param name="tint"></param>

        public ScreenPanel(Vector2 position, Texture2D image, Color tint)
        {
            this.position = position;
            this.image = image;
            this.tint = tint;
        }
        
        public virtual void Update(GameTime gameTime)
        {
            foreach (ScreenItem item in panelItems)
            {
                item.Update(gameTime);
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (image != null)
            {
                spriteBatch.Draw(image, position, tint);
            }
            foreach (ScreenItem item in panelItems)
            {
                item.Draw(gameTime, spriteBatch);
            }
        }
    }
}

