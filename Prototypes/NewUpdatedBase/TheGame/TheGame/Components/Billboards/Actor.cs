
#region Using Statements

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Library;
using TheGame.Components.Cameras;

#endregion

namespace TheGame
{
    public class Actor : Billboard
    {
        #region Actor State Enum

        public enum ActorState
        {
            Idle,
            Walking,
            Running,
            Attacking,
            Chanting,
            Casting,
            Hit,
            Dying,
            Dead,
            Stun,
            Override
        }

        #endregion // Actor State Enum

        #region Fields

        // The current state of the actor
        protected ActorState state = ActorState.Idle;

        // The state of the actor during the last update cycle
        protected ActorState previousState;

        // The current orienation of the actor
        protected Orientation orientation = Orientation.South;

        // The current sprite sequence the actor is playing
        protected SpriteSequence currentSequence;

        // The current velocity of the actor (speed and direction)
        protected Vector3 direction = Vector3.Zero;
        protected float speed = 0.0f;

        // Information about the sprite sheet the actor is using
        protected SpriteInfo spriteInfo;

        protected Monster monsterTarget;
        protected Player playerTarget;

        protected ActorInfo actorStats = new ActorInfo();

        protected float height;

        //protected Vector3 direction = Vector3.Forward;

        #endregion  // Fields

        #region Fields - Flags

        protected bool hasAttacked;
        public bool HasAttacked
        {
            get { return hasAttacked; }
            set { hasAttacked = value; }
        }

        public bool hasBeenHit = false;
        public bool isDying = false;

        #endregion

        #region Dictionaries

        // need to check out / change.  ActorState or string? need to implement two types of boundingboxes.
        // Dictionary of bounding shapes for collision detection for the actor to use at different states and orientations
        // Used by the actor to check against other shapes
        protected Dictionary<string, PrimitiveShape> boundingShapesSelf = new Dictionary<string, PrimitiveShape>();

        // Dictionary of sprite sequences for the actor to use at different states and orientations
        protected Dictionary<string, SpriteSequence> sequences = new Dictionary<string, SpriteSequence>();

        #endregion // Dictionaries

        #region Accessors

        /// <summary>
        /// Gets or sets the state of this actor.
        /// </summary>
        public virtual ActorState State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// Gets or sets the orientation of this actor.
        /// </summary>
        public Orientation Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public Vector3 Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public ActorInfo ActorStats
        {
            get { return actorStats; }
            set { actorStats = value; }
        }

        #endregion  // Accessors

        #region Initialization

        public Actor(GameScreen parent, SpriteInfo spriteInfo, Vector3 position, Vector3 rotation, Vector3 scale)
            : base(parent, spriteInfo.SpriteSheet, position, rotation, scale)
        {
            this.spriteInfo = spriteInfo;
        }

        public Actor(GameScreen parent, SpriteInfo spriteInfo, Vector3 position, Vector3 rotation)
            : this(parent, spriteInfo, position, rotation, Vector3.One)
        {
        }

        public Actor(GameScreen parent, SpriteInfo spriteInfo, Vector3 position)
            : this(parent, spriteInfo, position, Vector3.Zero, Vector3.One)
        {
        }

        public Actor(GameScreen parent, SpriteInfo spriteInfo)
            : this(parent, spriteInfo, Vector3.Zero, Vector3.Zero, Vector3.One)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }
        #endregion // Initialization

        #region Dispose

        public override void Dispose()
        {
            spriteInfo.SpriteSheet.Dispose();

            base.Dispose();
        }

        #endregion // Dispose

        #region Update

        public override void Update(GameTime gameTime)
        {
            // Obtain title from current state and orientation
            UpdateSpriteSequence(gameTime);

            // Updates the sprite sequence vertices so that it can grab the next sprite in the animation
            UpdateVertices(currentSequence, spriteInfo);
            
            // Update the bounding shapes
            UpdateBounding();

            base.Update(gameTime);
        }

        #endregion  // Update

        #region Update Methods

        protected void UpdatePosition(GameTime gameTime)
        {
            // Obtain height map and store position before moving.
            HeightMapInfo heightInfo = ((Level)Parent).TerrainHeightMap;
            Vector3 oldPosition;

            Camera camera = (Camera)GameEngine.Services.GetService(typeof(Camera));

            oldPosition = this.Position;
            if (this is Player)
            {
                Vector3 relCameraDirection = camera.Position - camera.LookAt;

                float angle = ((float)Math.Atan2(-relCameraDirection.X, relCameraDirection.Z));

                direction = Vector3.Transform(direction, Matrix.CreateRotationY(angle));
            }

            if (direction != Vector3.Zero && state != ActorState.Attacking)
            {
                direction = Vector3.Normalize(direction);
            }

            position.X += direction.X * speed * (float)gameTime.ElapsedGameTime.Milliseconds;
            if(this is Player)
                position.Z -= direction.Z * speed * (float)gameTime.ElapsedGameTime.Milliseconds;
            if(this is Monster)
                position.Z += direction.Z * speed * (float)gameTime.ElapsedGameTime.Milliseconds;

            if (heightInfo.IsOnHeightMap(position) == false)
            {
                position = oldPosition;
                //break;
            }

            if (orientation == Orientation.North || orientation == Orientation.South ||
                orientation == Orientation.East || orientation == Orientation.West)
            {
                CheckCardinalMovementCollision(heightInfo, oldPosition);
            }
            else
            {
                CheckCardinalDiagonalMovementCollision(heightInfo, oldPosition);
                CheckCardinalMovementCollision(heightInfo, oldPosition);
            }
        }

        private void UpdateSpriteSequence(GameTime gameTime)
        {
            string nextSequenceTitle = state.ToString() + orientation.ToString();
            string currentSequenceTitle = currentSequence.Title + currentSequence.Orientation.ToString();

            if (currentSequenceTitle.Equals(nextSequenceTitle))
            {
                currentSequence.Update(gameTime);
            }
            else
            {
                currentSequence.Reset();
                if (nextSequenceTitle == "AttackingSouth")
                {
                }
                playSequence(nextSequenceTitle);
            }
        }

        /// <summary>
        /// Updates the current bounding shape.
        /// </summary>
        private void UpdateBounding()
        {
            boundingShapesSelf[state.ToString() + orientation.ToString()].Update(position);
            primitiveShape.Update(position);
        }

        /// <summary>
        /// Used during UpdatePosition.  Checks this actor object against all other
        /// billboard bounding boxes to see if there is a collision.  Note: This does not
        /// check for collisions with Monster objects
        /// </summary>
        /// <param name="oldPosition">The position that the object was at previously</param>
        protected virtual void CheckBillboardBoundingBoxes(Vector3 oldPosition)
        {
            boundingShapesSelf[state.ToString() + orientation.ToString()].Update(position);

            // Check for collision against each players bounding shape
            foreach (Player p in ((Level)Parent).PlayerList)
            {
                if (this != p)
                {
                    if (IsHit(p.primitiveShape))
                    {
                        position = oldPosition;
                    }
                }
            }

            // Check for collision against each monsters bounding shape
            foreach (Monster m in ((Level)Parent).MonsterList)
            {
                if (this != m)
                {
                    if (IsHit(m.PrimitiveShape))
                    {
                        position = oldPosition;
                    }
                }
            }
        }

        /// <summary>
        /// Checks for collision while moving in the North, East, West and South cardinal directions.
        /// </summary>
        /// <param name="heightInfo">The height map information of the terrain</param>
        /// <param name="oldPosition">The position that the object was at previously</param>
        private void CheckCardinalMovementCollision(HeightMapInfo heightInfo, Vector3 oldPosition)
        {
            if (heightInfo.GetHeight(position) != 0.0f)
            {
                position = oldPosition;
            }
            else
            {
                CheckBillboardBoundingBoxes(oldPosition);
            }
        }

        /// <summary>
        /// Checks for collision while moving in the Northeast, Northwest, Southeast, Southwest directions
        /// </summary>
        /// <param name="heightInfo">The height map information of the terrain</param>
        /// <param name="oldPosition">The position that the object was at previously</param>
        private void CheckCardinalDiagonalMovementCollision(HeightMapInfo heightInfo, Vector3 oldPosition)
        {
            if (heightInfo.GetHeight(position) != 0.0f)
            {
                position = oldPosition;
            }
            else
            {
                CheckBillboardBoundingBoxes(oldPosition);
                oldPosition = position;
            }
        }

        #endregion

        #region Sprite Sequencing Methods

        /// <summary>
        /// Plays the selected sequence of sprites resulting in an animation
        /// </summary>
        /// <param name="sequence">The sprite sequence to play</param>
        private void playSequence(SpriteSequence sequence)
        {
            if (currentSequence != sequence)
            {
                currentSequence = sequence;
            }
        }

        /// <summary>
        /// Plays the selected sequence of sprites resulting in an animation
        /// </summary>
        /// <param name="title">The title of the sprite sequence to play</param>
        private void playSequence(string title)
        {
            if (String.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("title");
            }
            playSequence(sequences[title]);
        }

        /// <summary>
        /// The default sequence, idling.
        /// </summary>
        protected void idle()
        {
            state = ActorState.Idle;
            playSequence(state.ToString() + currentSequence.Orientation.ToString());
        }

        #endregion  // Sprite Sequencing Methods   // Complete, don't touch

        #region ICollidable Members

        /// <summary>
        /// Determines if there is a hit between objects using their bounding shapes
        /// </summary>
        /// <param name="otherShape">The other objects bounding shape</param>
        /// <returns>True if a hit, false otherwise</returns>
        public override bool IsHit(PrimitiveShape otherShape)
        {
            return PrimitiveShape.TestCollision(boundingShapesSelf[state.ToString() + orientation.ToString()], otherShape);
        }

        #endregion // ICollidable Members
    }
}
