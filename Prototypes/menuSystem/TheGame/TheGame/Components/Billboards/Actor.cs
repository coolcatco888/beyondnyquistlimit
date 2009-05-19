
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
    public class Actor : Billboard, IAudioEmitter
    {
        #region Actor State Enum

        public enum ActorState
        {
            Idle,
            Walking,
            Running,
            Attacking,
            Casting,
            Defending,
            Hit,
            Dying,
            Dead,
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
        protected Vector3 velocity = Vector3.Zero;

        // Information about the sprite sheet the actor is using
        protected SpriteInfo spriteInfo;

        protected Monster target;

        #endregion  // Fields

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
        public ActorState State
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

        #endregion  // Accessors

        #region Constructors

        public Actor(GameScreen parent, SpriteInfo spriteInfo)
            : base(parent, spriteInfo.SpriteSheet)
        {
            this.spriteInfo = spriteInfo;

            //this.Initialize();
        }

        #endregion  // Constructors

        #region Initialization

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
            // Update position of the sprite. Checks the terrain height and any Monsters/Billboard bounding boxes
            // To determine if it can go to that position.
            UpdatePosition();

            // Update the bounding if this object is not dying or dead
            if (state != ActorState.Dying && state != ActorState.Dead)
            {
                UpdateBounding();
            }

            // Updates the sprite sequence vertices so that it can grab the next sprite in the animation
            UpdateVertices(currentSequence, spriteInfo);

            if (state == ActorState.Dead)
            {
                if (this.Type == ObjectType.Monster)
                {
                    BillboardList monsters = ((Level)Parent).MonsterList;
                    monsters.Remove((Monster)this);
                    this.Dispose();
                }
                else
                {
                    BillboardList players = ((Level)Parent).PlayerList;
                    players.Remove((Player)this);
                    this.Dispose();
                }
            }

            base.Update(gameTime);
        }

        private void UpdatePosition()
        {
            // Obtain height map and store position before moving.
            HeightMapInfo heightInfo = ((Level)Parent).TerrainHeightMap;
            Vector3 oldPosition;

            Camera camera = (Camera)GameEngine.Services.GetService(typeof(Camera));

            // Get number of times the sprite frame has been incremented.
            int updates = currentSequence.UpdateCount;

            while (updates-- > 0)
            {
                oldPosition = this.Position;

                Vector3 relCameraDirection = camera.Position - camera.LookAt;

                float angle = ((float)Math.Atan2(-relCameraDirection.X, relCameraDirection.Z));

                velocity = Vector3.Transform(velocity, Matrix.CreateRotationY(angle));

                position.X += velocity.X * currentSequence.Velocity;
                position.Z -= velocity.Z * currentSequence.Velocity;

                if (heightInfo.IsOnHeightMap(position) == false)
                {
                    position = oldPosition;
                    break;
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
        }

        #endregion  // Update

        #region Update Methods

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
        private void CheckBillboardBoundingBoxes(Vector3 oldPosition)
        {
            boundingShapesSelf[state.ToString() + orientation.ToString()].Update(position);

            if (this.Type == ObjectType.Monster)
            {
                foreach (Player p in ((Level)Parent).PlayerList)
                {
                    if (IsHit(p.primitiveShape))
                    {
                        position = oldPosition;
                    }
                }
            }

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

        #region IAudioEmitter Members

        public Vector3 Forward
        {
            get { return Vector3.Forward; }
        }

        public Vector3 Up
        {
            get { return Vector3.Up; }
        }

        public Vector3 Velocity
        {
            get { return velocity; }
        }

        #endregion
    }
}
