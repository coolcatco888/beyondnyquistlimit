using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace WTFJimGameProject.GameWindows
{
    /// <summary>
    /// Represents a 2D image to be drawn on a screen using screen coordinates. 
    /// </summary>
    class ImageComponent2D : Component, I2DComponent
    {
        private SpriteBatch spriteBatch;

        private BoundingRectangle bounds;

        private Vector2 position;

        private Texture2D image;

        private Color tint;

        public Vector2 Position
        {
            set { position = value; }
            get { return position; }
        }

        public BoundingRectangle Bounds
        {
            set { bounds = value; }
            get { return bounds; }
        }

        public SpriteBatch SpriteBatch
        {
            set { spriteBatch = value; }
            get { return spriteBatch; }
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
        public ImageComponent2D(GameScreen parent)
            : this(parent, new Vector2(), null)
        {
        }

        /// <summary>
        /// Creates a panel with a background image, keep in mind panel items can be placed
        /// outside of a menu but generally shouldn't.
        /// </summary>
        /// <param name="position">Relative position on the screen or on a panel</param>
        /// <param name="image">Content Resource name for image</param>
        public ImageComponent2D(GameScreen parent, Vector2 position, Texture2D image)
            : this(parent, position, image, Color.White)
        {
        }


        /// <summary>
        /// Creates a panel with a background image with a tint color, keep in mind panel items can be placed
        /// outside of a menu but generally shouldn't.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="image"></param>
        /// <param name="tint"></param>

        public ImageComponent2D(GameScreen parent, Vector2 position, Texture2D image, Color tint) : base(parent)
        {
            this.position = position;
            this.image = image;
            this.tint = tint;
            this.spriteBatch = parent.Owner.SpriteBatch;
        }

        public override void Draw()
        {
            spriteBatch.Begin();
            spriteBatch.Draw(image, position, tint);
            spriteBatch.End();
        }
    }
}

