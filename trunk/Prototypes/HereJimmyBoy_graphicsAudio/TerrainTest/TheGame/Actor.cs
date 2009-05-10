
#region Using Statements

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Library;

#endregion

namespace TheGame
{
    public class Actor : Billboard, IAudioEmitter
    {
#region Fields

        protected ActorState state = ActorState.Idle;
        protected Orientation orientation = Orientation.South;
        protected Dictionary<string, Texture2D> spriteSheets = new Dictionary<string, Texture2D>();
        protected Dictionary<string, SpriteSequence> sequences = new Dictionary<string, SpriteSequence>();
        protected SpriteSequence currentSequence;
        protected Vector3 velocity = Vector3.Zero;
        protected float xDirection = 0.0f;
        protected float zDirection = 0.0f;

#endregion  // Fields

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
            : base(parent, spriteInfo)
        {
        }

#endregion  // Constructors

        public override void Dispose()
        {
            basicEffect.Dispose();

            base.Dispose();
        }

#region Update

        public override void Update(GameTime gameTime)
        {
            // Obtain title from current state and orientation
            string nextSequenceTitle = state.ToString() + orientation.ToString();
            string currentSequenceTitle = currentSequence.Title + currentSequence.Orientation.ToString();

            if(currentSequenceTitle.Equals(nextSequenceTitle))
            {
                currentSequence.Update(gameTime);

                UpdatePosition();

                UpdateVertices();
            }
            else
            {
                playSequence(nextSequenceTitle);
            }

 	        base.Update(gameTime);
        }

        private void UpdateVertices()
        {
            vertices[0].TextureCoordinate = new Vector2(
                currentSequence.CurrentFrameColumn * spriteInfo.SpriteUnit.X,
                currentSequence.CurrentFrameRow * spriteInfo.SpriteUnit.Y);

            vertices[1].TextureCoordinate = new Vector2(
                currentSequence.CurrentFrameColumn * spriteInfo.SpriteUnit.X + spriteInfo.SpriteUnit.X,
                currentSequence.CurrentFrameRow * spriteInfo.SpriteUnit.Y);

            vertices[2].TextureCoordinate = new Vector2(
                currentSequence.CurrentFrameColumn * spriteInfo.SpriteUnit.X + spriteInfo.SpriteUnit.X,
                currentSequence.CurrentFrameRow * spriteInfo.SpriteUnit.Y + spriteInfo.SpriteUnit.Y);

            vertices[3].TextureCoordinate = new Vector2(
                currentSequence.CurrentFrameColumn * spriteInfo.SpriteUnit.X,
                currentSequence.CurrentFrameRow * spriteInfo.SpriteUnit.Y + spriteInfo.SpriteUnit.Y);
        }

        private void UpdatePosition()
        {
            // Obtain height map and store position before moving.
            HeightMapInfo heightInfo = ((Level)Parent).TerrainHeightMap;
            Vector3 oldPosition = this.Position;

            // Get number of times the sprite frame has been incremented.
            int updates = currentSequence.UpdateCount;
            
            position.X += velocity.X * currentSequence.Velocity;
            position.Z -= velocity.Z * currentSequence.Velocity;
            // Update position.
            //switch(state)
            //{
            //    case ActorState.Walking:
            //        position.X += xMovement / 8;
            //        position.Z -= zMovement / 8;
            //        break;
            //    case ActorState.Running:
            //        position.X += xMovement / 4;
            //        position.Z -= zMovement / 4;
            //        break;
            //}

            /*
            while (updates-- > 0)
            {
                switch (orientation)
                {
                    case Orientation.North:
                        position.Z -= currentSequence.Velocity;
                        if (heightInfo.GetHeight(position) != -1.0f)
                        {
                            position = oldPosition;
                        }
                        break;

                    case Orientation.Northeast:
                        position.Z -= currentSequence.Velocity;
                        if (heightInfo.GetHeight(position) != -1.0f)
                        {
                            position = oldPosition;
                        }
                        else
                        {
                            oldPosition = position;
                        }
                        position.X += currentSequence.Velocity;
                        if (heightInfo.GetHeight(position) != -1.0f)
                        {
                            position = oldPosition;
                        }
                        break;

                    case Orientation.East:
                        position.X += currentSequence.Velocity;
                        if (heightInfo.GetHeight(position) != -1.0f)
                        {
                            position = oldPosition;
                        }
                        break;

                    case Orientation.Southeast:
                        position.Z += currentSequence.Velocity;
                        if (heightInfo.GetHeight(position) != -1.0f)
                        {
                            position = oldPosition;
                        }
                        else
                        {
                            oldPosition = position;
                        }
                        position.X += currentSequence.Velocity;
                        if (heightInfo.GetHeight(position) != -1.0f)
                        {
                            position = oldPosition;
                        }
                        break;

                    case Orientation.South:
                        position.Z += currentSequence.Velocity;
                        if (heightInfo.GetHeight(position) != -1.0f)
                        {
                            position = oldPosition;
                        }
                        break;

                    case Orientation.Southwest:
                        position.Z += currentSequence.Velocity;
                        if (heightInfo.GetHeight(position) != -1.0f)
                        {
                            position = oldPosition;
                        }
                        else
                        {
                            oldPosition = position;
                        }
                        position.X -= currentSequence.Velocity;
                        if (heightInfo.GetHeight(position) != -1.0f)
                        {
                            position = oldPosition;
                        }
                        break;

                    case Orientation.West:
                        position.X -= currentSequence.Velocity;
                        if (heightInfo.GetHeight(position) != -1.0f)
                        {
                            position = oldPosition;
                        }
                        break;

                    case Orientation.Northwest:
                        position.Z -= currentSequence.Velocity;
                        if (heightInfo.GetHeight(position) != -1.0f)
                        {
                            position = oldPosition;
                        }
                        else
                        {
                            oldPosition = position;
                        }
                        position.X -= currentSequence.Velocity;
                        if (heightInfo.GetHeight(position) != -1.0f)
                        {
                            position = oldPosition;
                        }
                        break;
                }
            }
             */
        }

#endregion  // Update

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

#region Methods

        private void playSequence(SpriteSequence sequence)
        {
            if (currentSequence != sequence)
            {
                currentSequence = sequence;
            }
        }

        private void playSequence(string title)
        {
            if (String.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("title");
            }
            playSequence(sequences[title]);
        }

        protected void idle()
        {
            state = ActorState.Idle;
            playSequence(state.ToString() + currentSequence.Orientation.ToString());
        }

#endregion  // Methods

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
