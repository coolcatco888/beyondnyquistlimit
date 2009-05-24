
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

        protected string currentAttack = "";
        protected Vector3 attackDirection;

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

        // Dictionary of slash attacks.
        protected Dictionary<string, AttackInfo> attacks = new Dictionary<string, AttackInfo>();

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

            // TEMPORARY ADD ATTACKS
            AttackInfo infoSW = new AttackInfo();
            infoSW.Distance = 1.0f;
            infoSW.Rotation = new Vector2(0.0f, 0.0f);
            infoSW.Scale = new Vector2(2.0f, 2.0f);
            infoSW.UnitScale = new Vector2(2.0f, 1.0f);
            infoSW.TextureCoordinates = new Vector2(16, 5);

            attacks.Add("SouthSlash", infoSW);
            attacks.Add("SouthwestSlash", infoSW);
            attacks.Add("WestSlash", infoSW);

            AttackInfo infoSE = new AttackInfo();
            infoSE.Distance = 1.0f;
            infoSE.Rotation = new Vector2(0.0f, 0.0f);
            infoSE.Scale = new Vector2(2.0f, 2.0f);
            infoSE.UnitScale = new Vector2(2.0f, 1.0f);
            infoSE.TextureCoordinates = new Vector2(18, 5);

            attacks.Add("EastSlash", infoSE);
            attacks.Add("SoutheastSlash", infoSE);

            AttackInfo infoNW = new AttackInfo();
            infoNW.Distance = 1.0f;
            infoNW.Rotation = new Vector2(0.0f, 0.0f);
            infoNW.Scale = new Vector2(2.0f, 2.0f);
            infoNW.UnitScale = new Vector2(2.0f, 1.0f);
            infoNW.TextureCoordinates = new Vector2(16, 6);

            attacks.Add("NorthSlash", infoNW);
            attacks.Add("NorthwestSlash", infoNW);

            AttackInfo infoNE = new AttackInfo();
            infoNE.Distance = 1.0f;
            infoNE.Rotation = new Vector2(0.0f, 0.0f);
            infoNE.Scale = new Vector2(2.0f, 2.0f);
            infoNE.UnitScale = new Vector2(2.0f, 1.0f);
            infoNE.TextureCoordinates = new Vector2(18, 6);

            attacks.Add("NortheastSlash", infoNE);


            // TEMPORARY ADD ATTACKS
            AttackInfo infoHSW = new AttackInfo();
            infoHSW.Distance = 1.0f;
            infoHSW.Rotation = new Vector2(0.0f, 0.0f);
            infoHSW.Scale = new Vector2(2.0f, 2.0f);
            infoHSW.UnitScale = new Vector2(2.0f, 1.0f);
            infoHSW.TextureCoordinates = new Vector2(24, 7);

            attacks.Add("SouthHeavySlash", infoHSW);
            attacks.Add("SouthwestHeavySlash", infoHSW);
            attacks.Add("WestHeavySlash", infoHSW);

            AttackInfo infoHSE = new AttackInfo();
            infoHSE.Distance = 1.0f;
            infoHSE.Rotation = new Vector2(0.0f, 0.0f);
            infoHSE.Scale = new Vector2(2.0f, 2.0f);
            infoHSE.UnitScale = new Vector2(2.0f, 1.0f);
            infoHSE.TextureCoordinates = new Vector2(26, 7);

            attacks.Add("EastHeavySlash", infoHSE);
            attacks.Add("SoutheastHeavySlash", infoHSE);

            AttackInfo infoHNW = new AttackInfo();
            infoHNW.Distance = 1.0f;
            infoHNW.Rotation = new Vector2(0.0f, 0.0f);
            infoHNW.Scale = new Vector2(2.0f, 2.0f);
            infoHNW.UnitScale = new Vector2(2.0f, 1.0f);
            infoHNW.TextureCoordinates = new Vector2(24, 8);

            attacks.Add("NorthHeavySlash", infoHNW);
            attacks.Add("NorthwestHeavySlash", infoHNW);

            AttackInfo infoHNE = new AttackInfo();
            infoHNE.Distance = 1.0f;
            infoHNE.Rotation = new Vector2(0.0f, 0.0f);
            infoHNE.Scale = new Vector2(2.0f, 2.0f);
            infoHNE.UnitScale = new Vector2(2.0f, 1.0f);
            infoHNE.TextureCoordinates = new Vector2(26, 8);

            attacks.Add("NortheastHeavySlash", infoHNE);
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
            string nextSequenceTitle;
            string currentSequenceTitle;
            if (currentSequence.Title == "AttackingHeavy" && state == ActorState.Attacking)
            {
                nextSequenceTitle = "AttackingHeavy" + orientation.ToString();
            }
            else
            {
                nextSequenceTitle = state.ToString() + orientation.ToString();
            }
            currentSequenceTitle = currentSequence.Title + currentSequence.Orientation.ToString();

            if (currentSequenceTitle.Equals(nextSequenceTitle))
            {
                currentAttack = currentSequence.Update(gameTime);
            }
            else
            {
                if (state.ToString() == "Attacking" && this is Player)
                {
                    GamepadDevice gamepadDevice = ((InputHub)GameEngine.Services.GetService(typeof(InputHub)))[((Player)this).PlayerIndex];

                    if (gamepadDevice.WasButtonPressed(Buttons.X))
                    {
                        nextSequenceTitle = "AttackingHeavy" + orientation.ToString();
                    }
                }

                currentSequence.Reset();
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

        #region Draw

        public override void Draw(GameTime gameTime)
        {
            if (direction != Vector3.Zero && state != ActorState.Attacking)
            {
                attackDirection = direction;
                attackDirection = new Vector3(attackDirection.X, attackDirection.Y, -attackDirection.Z);
            }

            base.Draw(gameTime);

            if (attacks.ContainsKey(currentAttack))
            {
                Camera camera = (Camera)GameEngine.Services.GetService(typeof(Camera));

                AttackInfo thisAttack = attacks[currentAttack];
                Vector3 oldPosition = this.Position;

                this.Position += attackDirection * thisAttack.Distance;// +Vector3.Down;

                UpdateVertices(thisAttack.TextureCoordinates, spriteInfo.SpriteUnit, thisAttack.UnitScale);

                // Assign world, view, & projection matricies to basicEffect.
                // TODO: implement rotation
                effect.Parameters["Color"].SetValue(Color.ToVector4());
                effect.Parameters["World"].SetValue(Matrix.CreateScale(-thisAttack.Scale.X, thisAttack.Scale.Y, 1.0f) * Matrix.CreateBillboard(position, camera.Position, Vector3.Up, camera.Direction));
                effect.Parameters["View"].SetValue(camera.View);
                effect.Parameters["Projection"].SetValue(camera.Projection);

                // TODO: Kickass alpha blend.
                GameEngine.Graphics.RenderState.AlphaBlendEnable = true;
                GameEngine.Graphics.RenderState.AlphaSourceBlend = Blend.One;
                GameEngine.Graphics.RenderState.AlphaDestinationBlend = Blend.One;

                // Draw billboard.
                effect.Begin();
                effect.CurrentTechnique.Passes[0].Begin();

                GameEngine.Graphics.VertexDeclaration = vertexDeclaration;
                GameEngine.Graphics.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices, 0, 2);

                effect.CurrentTechnique.Passes[0].End();
                effect.End();

                GameEngine.Graphics.RenderState.AlphaBlendEnable = false;

                this.Position = oldPosition;

                // Play attack sound.
                if (this is Player)
                {
                    //if (((Player)this).ClassInfo.ClassName == "Wizard")
                    //{
                    //    audioManager.Play3DCue("rodSlash", this);
                    //}
                    //else if (((Player)this).ClassInfo.ClassName == "Priest")
                    //{
                    //    audioManager.Play3DCue("maceSlash", this);
                    //}
                    //else
                    //{
                    //    audioManager.Play3DCue("slash", this);
                    //}

                    audioManager.SoundBank.PlayCue("slash");
                }
            }

            // Quality audio coding.
            if (this is Player)
            {
                if ((currentSequence.Title == "Walking" || currentSequence.Title == "Running") &&
                    (currentSequence.CurrentFrame.X == 4 || currentSequence.CurrentFrame.X == 8 ||
                    currentSequence.CurrentFrame.X == 13 || currentSequence.CurrentFrame.X == 17 ||
                    currentSequence.CurrentFrame.X == 22 || currentSequence.CurrentFrame.X == 26))
                {
                    if (((Player)this).ClassInfo.ClassName == "Knight")
                    {
                        audioManager.SoundBank.PlayCue("knight_step");
                        //audioManager.Play3DCue("knight_step", this);
                    }
                    else
                    {
                        audioManager.SoundBank.PlayCue("soft_step");
                        //audioManager.Play3DCue("soft_step", this);
                    }
                }
            }
        }

        #endregion  // Draw

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

        protected void AddAttack(string title, AttackInfo attack)
        {
            attacks.Add(title, attack);
        }

        protected void RemoveAttack(string title)
        {
            attacks.Remove(title);
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

        public virtual void ApplyDamage(int damage) { }

        #region IAudioEmitter Members

        protected AudioManager audioManager = (AudioManager)GameEngine.Services.GetService(typeof(AudioManager));

        public Vector3 Forward
        {
            get { return Vector3.Forward; }// direction; }
        }

        public Vector3 Up
        {
            get { return Vector3.Up; }
        }

        public Vector3 Velocity
        {
            get { return Vector3.Zero; }// direction* speed; }
        }

        #endregion
    }
}
