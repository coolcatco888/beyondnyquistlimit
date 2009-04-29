
#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace SpriteSample
{
    class Actor
    {
        #region Fields

        /// <summary>
        /// Possible states of the actor.
        /// </summary>
        public enum ActorState
        {
            Idle,
            Walking,
            Running,
            Attacking,
            Defending,
            Hit,
            Dying,
            Dead,
        }

        /// <summary>
        /// Current state of this actor.
        /// </summary>
        private ActorState state = ActorState.Idle;
        public ActorState State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// Current position of this actor on the world map.
        /// </summary>
        private Vector3 mapPosition;
        public Vector3 MapPosition
        {
            get { return mapPosition; }
            set { mapPosition = value; }
        }

        /// <summary>
        /// Possible orientations of the actor.
        /// </summary>
        public enum Orientation
        {
            North,
            Northeast,
            East,
            Southeast,
            South,
            Southwest,
            West,
            Northwest,
        }

        /// <summary>
        /// Current orientation of this actor.
        /// </summary>
        private Orientation orientation;
        public Orientation Orientation
        {
            get { return orientation; }
            set { orientation = value; }
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

        #endregion

        public Actor()
        {
            throw new System.NotImplementedException();
        }

        void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle)
        {
            spriteBatch.Draw(spriteSheet,
                destinationRectangle,
                sourceRectangle,
                Color.White);
        }
    }
}
