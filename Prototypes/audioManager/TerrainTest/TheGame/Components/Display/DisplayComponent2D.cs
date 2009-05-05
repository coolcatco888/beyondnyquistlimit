using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame.Components.Display
{
    /// <summary>
    /// Base class for all menu, HUD and other 2D screen displayable components.
    /// </summary>
    abstract class DisplayComponent2D : Component, I2DComponent, IDrawableComponent
    {
        protected bool visible;

        protected SpriteBatch spriteBatch;

        protected Vector2 position;

        public bool Visible
        {
            set { visible = value; }
            get { return visible; }
        }

        public Vector2 Position
        {
            set { position = value; }
            get { return position; }
        }


        public SpriteBatch SpriteBatch
        {
            set { spriteBatch = value; }
            get { return spriteBatch; }
        }
        /// <summary>
        /// Creates a 2D display component that is drawn onto the screen using screen coordinates.
        /// </summary>
        /// <param name="parent"></param>
        protected DisplayComponent2D(GameScreen parent)
            : base(parent)
        {
            visible = true;
            spriteBatch = GameEngine.SpriteBatch;
        }

        public override void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(GameTime gameTime)
        {
        }
    }
}
