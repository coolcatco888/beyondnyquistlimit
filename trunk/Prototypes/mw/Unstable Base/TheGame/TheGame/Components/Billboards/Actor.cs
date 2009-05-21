﻿
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
        protected Dictionary<string, SpriteSequence> sequences = new Dictionary<string, SpriteSequence>();
        protected SpriteSequence currentSequence;
        protected Vector3 velocity = Vector3.Zero;
        protected Vector3 direction = Vector3.Forward;
        protected SpriteInfo spriteInfo;
        protected Dictionary<string, AttackInfo> attacks = new Dictionary<string, AttackInfo>();
        protected string currentAttack = "";

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
            : base(parent, spriteInfo.SpriteSheet)
        {
            this.spriteInfo = spriteInfo;

            //this.Initialize();
        }

#endregion  // Constructors

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Dispose()
        {
            spriteInfo.SpriteSheet.Dispose();

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
                currentAttack = currentSequence.Update(gameTime);
            }
            else
            {
                currentSequence.Reset();
                if (nextSequenceTitle == "AttackingSouth")
                {
                }
                playSequence(nextSequenceTitle);
            }

            UpdatePosition();

            UpdateVertices(currentSequence, spriteInfo);

 	        base.Update(gameTime);
        }

        private void UpdatePosition()
        {
            // Obtain height map and store position before moving.
            HeightMapInfo heightInfo = ((Level)Parent).TerrainHeightMap;
            Vector3 oldPosition;

            // Get number of times the sprite frame has been incremented.
            int updates = currentSequence.UpdateCount;

            while (updates-- > 0)
            {
                oldPosition = this.Position;
                position.X += velocity.X * currentSequence.Velocity;
                position.Z -= velocity.Z * currentSequence.Velocity;

                if (heightInfo.IsOnHeightMap(position) == false)
                {
                    position = oldPosition;
                    break;
                }

                switch (orientation)
                {
                    case Orientation.North:
                        if (heightInfo.GetHeight(position) != 0.0f)
                        {
                            position = oldPosition;
                        }
                        break;

                    case Orientation.Northeast:
                        if (heightInfo.GetHeight(position) != 0.0f)
                        {
                            position = oldPosition;
                        }
                        else
                        {
                            oldPosition = position;
                        }
                        if (heightInfo.GetHeight(position) != 0.0f)
                        {
                            position = oldPosition;
                        }
                        break;

                    case Orientation.East:
                        if (heightInfo.GetHeight(position) != 0.0f)
                        {
                            position = oldPosition;
                        }
                        break;

                    case Orientation.Southeast:
                        if (heightInfo.GetHeight(position) != 0.0f)
                        {
                            position = oldPosition;
                        }
                        else
                        {
                            oldPosition = position;
                        }
                        if (heightInfo.GetHeight(position) != 0.0f)
                        {
                            position = oldPosition;
                        }
                        break;

                    case Orientation.South:
                        if (heightInfo.GetHeight(position) != 0.0f)
                        {
                            position = oldPosition;
                        }
                        break;

                    case Orientation.Southwest:
                        if (heightInfo.GetHeight(position) != 0.0f)
                        {
                            position = oldPosition;
                        }
                        else
                        {
                            oldPosition = position;
                        }
                        if (heightInfo.GetHeight(position) != 0.0f)
                        {
                            position = oldPosition;
                        }
                        break;

                    case Orientation.West:
                        if (heightInfo.GetHeight(position) != 0.0f)
                        {
                            position = oldPosition;
                        }
                        break;

                    case Orientation.Northwest:
                        if (heightInfo.GetHeight(position) != 0.0f)
                        {
                            position = oldPosition;
                        }
                        else
                        {
                            oldPosition = position;
                        }
                        if (heightInfo.GetHeight(position) != 0.0f)
                        {
                            position = oldPosition;
                        }
                        break;
                }
            }
        }

#endregion  // Update

        public override void Draw(GameTime gameTime)
        {
            if (velocity != Vector3.Zero && state != ActorState.Attacking)
            {
                direction = Vector3.Normalize(velocity);
            }

            base.Draw(gameTime);

            if(attacks.ContainsKey(currentAttack))
            {
                Camera camera = (Camera)GameEngine.Services.GetService(typeof(Camera));

                AttackInfo thisAttack = attacks[currentAttack];
                Vector3 oldPosition = this.Position;

                this.Position += this.direction * thisAttack.Distance + Vector3.Down;

                UpdateVertices(thisAttack.TextureCoordinates, spriteInfo.SpriteUnit, new Vector2(1.0f, 1.0f));

                // Assign world, view, & projection matricies to basicEffect.
                // TODO: implement rotation
                basicEffect.World = Matrix.CreateScale(thisAttack.Scale.X, thisAttack.Scale.Y, 1.0f) * Matrix.CreateRotationX(thisAttack.Rotation.X) * Matrix.CreateRotationY(thisAttack.Rotation.Y) * Matrix.CreateTranslation(position);
                basicEffect.View = camera.View;
                basicEffect.Projection = camera.Projection;

                GameEngine.Graphics.RenderState.CullMode = CullMode.None;

                // Draw billboard.
                basicEffect.Begin();
                basicEffect.CurrentTechnique.Passes[0].Begin();

                GameEngine.Graphics.VertexDeclaration = vertexDeclaration;
                GameEngine.Graphics.DrawUserPrimitives(PrimitiveType.TriangleFan, vertices, 0, 2);

                basicEffect.CurrentTechnique.Passes[0].End();
                basicEffect.End();

                GameEngine.Graphics.RenderState.CullMode = CullMode.CullCounterClockwiseFace;

                this.Position = oldPosition;
            }
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

        protected void AddAttack(string title, AttackInfo attack)
        {
            attacks.Add(title, attack);
        }

        protected void RemoveAttack(string title)
        {
            attacks.Remove(title);
        }

#endregion  // Methods

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

        public struct AttackInfo
        {
            private float distance;
            public float Distance
            {
                get { return distance; }
                set { distance = value; }
            }

            private Vector2 rotation;
            public Vector2 Rotation
            {
                get { return rotation; }
                set { rotation = value; }
            }

            private Vector2 textureCoordinates;
            public Vector2 TextureCoordinates
            {
                get { return textureCoordinates; }
                set { textureCoordinates = value; }
            }

            private Vector2 scale;
            public Vector2 Scale
            {
                get { return scale; }
                set { scale = value; }
            }
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
