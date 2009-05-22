using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace TheGame.Components.Display
{
    /// <summary>
    /// Represents a 2D image to be drawn on a screen using screen coordinates. 
    /// </summary>
    class ImageComponent2D : DisplayComponent2D
    {
        protected Texture2D image;

        protected Color tint;

        protected Vector2 scale;

        protected float rotation = 0.0f;

        protected bool isOriginCenter = false;

        protected Vector2 relativeCenter;

        public bool IsOriginCenter
        {
            set { isOriginCenter = value; }
            get { return isOriginCenter; }
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

        public Vector2 Scale
        {
            set { scale = value; }
            get { return scale; }
        }

        public float Rotation
        {
            set { rotation = value; }
            get { return rotation; }
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

        public ImageComponent2D(GameScreen parent, Vector2 position, Texture2D image, Color tint)
            : this(parent, position, image, tint, Vector2.One)
        {
        }

        /// <summary>
        /// Creates a panel with a background image with a tint color, keep in mind panel items can be placed
        /// outside of a menu but generally shouldn't.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="image"></param>
        /// <param name="tint"></param>

        public ImageComponent2D(GameScreen parent, Vector2 position, Texture2D image, Color tint, Vector2 scale)
            : base(parent)
        {
            this.position = position;
            this.image = image;
            this.tint = tint;
            this.scale = scale;
            this.relativeCenter = new Vector2(image.Width * 0.5f, image.Height * 0.5f);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(image, position, null, tint, rotation, isOriginCenter? relativeCenter : Vector2.Zero, scale, SpriteEffects.None, 0.0f);
            spriteBatch.End();
        }

        public override void Dispose()
        {
            this.image.Dispose();
            base.Dispose();
        }

        public override Vector2 Center
        {
            get { return new Vector2(Left + 0.5f * Width, Top + 0.5f * Height); }
        }

        public override float Height
        {
            get { return (float) image.Height * scale.Y;  }
        }

        public override float Width
        {
            get { return (float)image.Width * scale.X;  }
        }

        public override float Left
        {
            get { return position.X; }
        }

        public override float Right
        {
            get { return Left + Width; }
        }

        public override float Bottom
        {
            get { return Top + Height; }
        }

        public override float Top
        {
            get { return position.Y; }
        }
    }
}

