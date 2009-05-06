
#region Using Statements

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace TheGame
{
    class SpriteInfo
    {
        #region Fields

        private Texture2D spriteSheet;
        private int width;
        private int height;
        private int padSize;
        private Vector2 spriteUnit;

        #endregion  // Fields

        #region Accessors

        public Texture2D SpriteSheet
        {
            get { return spriteSheet; }
            set { spriteSheet = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public int PadSize
        {
            get { return padSize; }
            set { padSize = value; }
        }

        public Vector2 SpriteUnit
        {
            get { return spriteUnit; }
            set { spriteUnit = value; }
        }

        #endregion  // Accessors

        #region Constructors

        public SpriteInfo() { }

        public SpriteInfo(Texture2D spriteSheet, int spriteWidth, int spriteHeight, int padSize)
        {
            this.spriteSheet = spriteSheet;
            this.width = spriteWidth;
            this.height = spriteHeight;
            this.padSize = padSize;

            spriteUnit = new Vector2((float)(width + padSize) / (float)spriteSheet.Width,
                (float)(height + padSize) / (float)spriteSheet.Height);
        }

        #endregion
    }
}
