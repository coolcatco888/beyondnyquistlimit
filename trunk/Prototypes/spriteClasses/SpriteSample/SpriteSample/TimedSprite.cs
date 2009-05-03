
#region Using Statements

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

#endregion  // Using Statements

namespace SpriteSample
{
    class TimedSprite
    {
        #region Fields

        /// <summary>
        /// Current position of this sprite on the world map.
        /// </summary>
        private Vector3 mapPosition;
        public Vector3 MapPosition
        {
            get { return mapPosition; }
            set { mapPosition = value; }
        }

        /// <summary>
        /// Sprite sheet for this actor.
        /// </summary>
        Texture2D spriteSheet;

        /// <summary>
        /// Source rectangle for the current sprite.
        /// </summary>
        Rectangle sourceRectangle;

        /// <summary>
        /// Destination rectangle for the current sprite.
        /// </summary>
        Rectangle destinationRectangle = new Rectangle(0, 0, 64, 64);

        /// <summary>
        /// Width of a sprite.
        /// </summary>
        int spriteWidth = 64;

        /// <summary>
        /// Height of a sprite.
        /// </summary>
        int spriteHeight = 64;

        /// <summary>
        /// Number of pixels padding between sprites on sprite sheet.
        /// </summary>
        int spritePadding = 1;

        /// <summary>
        /// Current frame in current sprite sequence.
        /// </summary>
        int currentFrame;

        /// <summary>
        /// Elapsed time since last frame update.
        /// </summary>
        float timer = 0.0f;

        /// <summary>
        /// Time to elapse before frame is incremented.
        /// </summary>
        float interval = 1000.0f / 15.0f;

        /// <summary>
        /// Double buffering for sequence frames.
        /// </summary>
        bool isBufferFrame = false;

        #endregion  // Fields
    }
}
