using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ContentTestLibrary
{
    /// <summary>
    /// Holds the information for one sprite sheet
    /// </summary>
    public class SpriteInfo
    {
        #region Primary Fields

        /// <summary>
        /// Width of one sprite
        /// </summary>
        private int width;
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        /// <summary>
        /// Height of one sprite
        /// </summary>
        private int height;
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        /// <summary>
        /// The padding size between sprites
        /// </summary>
        private int padSize;
        public int PadSize
        {
            get { return padSize; }
            set { padSize = value; }
        }
        
        #endregion

        #region Calculated Fields

        [ContentSerializerIgnore]
        private Vector2 spriteUnit;
        [ContentSerializerIgnore]
        public Vector2 SpriteUnit
        {
            get { return spriteUnit; }
        }

        [ContentSerializerIgnore]
        private float centerHeight;
        [ContentSerializerIgnore]
        public float CenterHeight
        {
            get { return centerHeight; }
        }

        #endregion

        #region Graphics Data

        /// <summary>
        /// Filename of the sprite sheet to load in
        /// </summary>
        private string spriteSheetFile;
        public string SpriteSheetFile
        {
            get { return spriteSheetFile; }
            set { spriteSheetFile = value; }
        }

        /// <summary>
        /// The entire sprite sheet!!!
        /// </summary>
        [ContentSerializerIgnore]
        private Texture2D spriteSheet;
        [ContentSerializerIgnore]
        public Texture2D SpriteSheet
        {
            get { return spriteSheet; }
        }

        #endregion

        #region Methods to Load Content

        /// <summary>
        /// Loads the sprite sheet from the specified file.
        /// Will not have the ContentManager parameter in real project.
        /// </summary>
        public void LoadSpriteSheet(ContentManager content)
        {
            spriteSheet = content.Load<Texture2D>(spriteSheetFile);
        }

        /// <summary>
        /// Calculates the centerHeight and the spriteUnit values.
        /// </summary>
        public void CalculateFields()
        {
            centerHeight = this.height / 2 - 1;

            spriteUnit = new Vector2((float)(width + padSize) / (float)spriteSheet.Width,
                (float)(height + padSize) / (float)spriteSheet.Height);
        }

        #endregion

        #region Content Reader

        public class SpriteInfoReader : ContentTypeReader<SpriteInfo>
        {
            // Read in a SpriteInfo from a .xnb file and calculate necessary fields and load texture
            protected override SpriteInfo Read(ContentReader input, SpriteInfo existingInstance)
            {
                SpriteInfo spriteInfo = existingInstance;
                if (spriteInfo == null)
                {
                    spriteInfo = new SpriteInfo();
                }

                // Read in Primary Fields
                spriteInfo.width = input.ReadInt32();
                spriteInfo.height = input.ReadInt32();
                spriteInfo.padSize = input.ReadInt32();
                spriteInfo.spriteSheetFile = input.ReadString();

                spriteInfo.LoadSpriteSheet(input.ContentManager);
                spriteInfo.CalculateFields();

                return spriteInfo;
            }
        }

        #endregion
    }
}
