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
    /// 
    /// Here is an example of a panel with an image.
    ///<panel>
    ///  <position>
    ///    <xpos>250</xpos>
    ///    <ypos>200</ypos>
    /// </position>
    ///    <image2d>
    ///        <xpos>0</xpos>
    ///       <ypos>0</ypos>
    ///        <name>menubg</name>
    ///        <rcolor>255</rcolor>
    ///        <gcolor>255</gcolor>
    ///       <bcolor>255</bcolor>
    ///    </image2d>
    ///</panel>
    /// </summary>
    class ImageComponent2D : Component2D
    {
        private Texture2D image;

        private Color tint;

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
        public ImageComponent2D() : this (new Vector2(), null)
        {
        }

        /// <summary>
        /// Creates a panel with a background image, keep in mind panel items can be placed
        /// outside of a menu but generally shouldn't.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="image"></param>
        public ImageComponent2D(Vector2 position, Texture2D image) :  this (position, image, Color.White)
        {
        }


        /// <summary>
        /// Creates a panel with a background image with a tint color, keep in mind panel items can be placed
        /// outside of a menu but generally shouldn't.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="image"></param>
        /// <param name="tint"></param>

        public ImageComponent2D(Vector2 position, Texture2D image, Color tint)
        {
            this.position = position;
            this.image = image;
            this.tint = tint;
        }
        
        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(image, position, tint);
            spriteBatch.End();
        }
    }
}

