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
    abstract class DisplayComponent2D : DrawableComponent, I2DComponent, IDrawableComponent
    {

        protected SpriteBatch spriteBatch;

        protected Vector2 position;

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
            spriteBatch = GameEngine.SpriteBatch;
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime)
        {
        }

        #region I2DComponent Members

        public abstract Vector2 Center
        {
            get;
        }

        public abstract float Height
        {
            get;
        }

        public abstract float Width
        {
            get;
        }

        public abstract float Left
        {
            get;
        }

        public abstract float Right
        {
            get;
        }

        public abstract float Bottom
        {
            get;
        }

        public abstract float Top
        {
            get;
        }

        #endregion
    }
}
