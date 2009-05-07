
#region Using Statements

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace TheGame
{
    class Actor : Billboard
    {
#region Fields

        private ActorState state = ActorState.Idle;
        private Orientation orientation = Orientation.South;
        Dictionary<string, Texture2D> spriteSheets = new Dictionary<string, Texture2D>();
        Dictionary<string, SpriteSequence> sequences = new Dictionary<string, SpriteSequence>();
        SpriteSequence currentSequence;
        private GroundEffect shadow;

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
            AddBasicSequences();
            shadow = new GroundEffect(parent, new SpriteInfo(GameEngine.Content.Load<Texture2D>("Shadow"), 64, 32, 0));
        }

#endregion  // Constructors

#region Update

        public override void Update(GameTime gameTime)
        {
            // Obtain title from current state and orientation
            string nextSequenceTitle = state.ToString() + orientation.ToString();
            string currentSequenceTitle = currentSequence.Title + currentSequence.Orientation.ToString();

            if(currentSequenceTitle.Equals(nextSequenceTitle))
            {
                UpdateController();

                currentSequence.Update(gameTime);

                UpdatePosition();

                UpdateVertices();

                shadow.Position = this.position;
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

            // Update position.
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
        }

        private void UpdateController()
        {
            KeyboardDevice keyboardDevice = (KeyboardDevice)GameEngine.Services.GetService(typeof(KeyboardDevice));

            if (keyboardDevice.IsKeyDown(Keys.LeftShift) || keyboardDevice.IsKeyDown(Keys.RightShift))
                state = Actor.ActorState.Running;
            else
                state = Actor.ActorState.Walking;


            if (keyboardDevice.IsKeyDown(Keys.Up))
            {
                if (keyboardDevice.IsKeyDown(Keys.Right))
                    orientation = Orientation.Northeast;
                else if (keyboardDevice.IsKeyDown(Keys.Left))
                    orientation = Orientation.Northwest;
                else
                    orientation = Orientation.North;
            }
            else if (keyboardDevice.IsKeyDown(Keys.Down))
            {
                if (keyboardDevice.IsKeyDown(Keys.Right))
                    orientation = Orientation.Southeast;
                else if (keyboardDevice.IsKeyDown(Keys.Left))
                    orientation = Orientation.Southwest;
                else
                    orientation = Orientation.South;
            }
            else if (keyboardDevice.IsKeyDown(Keys.Right))
            {
                orientation = Orientation.East;
            }
            else if (keyboardDevice.IsKeyDown(Keys.Left))
            {
                orientation = Orientation.West;
            }
            else
            {
                idle();
            }
        }

#endregion  // Update

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

        private void idle()
        {
            state = ActorState.Idle;
            playSequence(state.ToString() + currentSequence.Orientation.ToString());
        }

        public void AddBasicSequences()
        {
            SpriteSequence sequence;
            int x = 0;
            int y = 0;

            // Add idle sequences.
            foreach(Orientation orientation in Enum.GetValues(typeof(Orientation)))
            {
                sequence = new SpriteSequence("Idle", orientation, false, 0.0f, 0);
                sequence.AddFrame(0, y++);

                sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);
            }

            x = 0;
            y = 0;

            // Add walking sequences.
            foreach (Orientation orientation in Enum.GetValues(typeof(Orientation)))
            {
                sequence = new SpriteSequence("Walking", orientation, true, 0.2f, 2);
                sequence.AddRow(y++, 1, 8);

                sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);
            }

            x = 0;
            y = 0;

            // Add running sequences.
            foreach (Orientation orientation in Enum.GetValues(typeof(Orientation)))
            {
                sequence = new SpriteSequence("Running", orientation, true, 0.4f, 1);
                sequence.AddRow(y++, 1, 8);

                sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);
            }

            currentSequence = sequences["IdleSouth"];

            /*
            float walkSpeed = 0.2f;
            int walkBuffers = 2;

            float runSpeed = 0.3f;
            int runBuffers = 1;

            // Idle
            sequences.Add("IdleSouth", new SpriteSequence(base.Parent, spriteInfo, "Idle", Orientation.South, 0, 0, 0, false, 0, 0));
            sequences.Add("IdleSouthwest", new SpriteSequence(base.Parent, spriteInfo, "Idle", Orientation.Southwest, 1, 0, 0, false, 0, 0));
            sequences.Add("IdleWest", new SpriteSequence(base.Parent, spriteInfo, "Idle", Orientation.West, 2, 0, 0, false, 0, 0));
            sequences.Add("IdleNorthwest", new SpriteSequence(base.Parent, spriteInfo, "Idle", Orientation.Northwest, 3, 0, 0, false, 0, 0));
            sequences.Add("IdleNorth", new SpriteSequence(base.Parent, spriteInfo, "Idle", Orientation.North, 4, 0, 0, false, 0, 0));
            sequences.Add("IdleNortheast", new SpriteSequence(base.Parent, spriteInfo, "Idle", Orientation.Northeast, 5, 0, 0, false, 0, 0));
            sequences.Add("IdleEast", new SpriteSequence(base.Parent, spriteInfo, "Idle", Orientation.East, 6, 0, 0, false, 0, 0));
            sequences.Add("IdleSoutheast", new SpriteSequence(base.Parent, spriteInfo, "Idle", Orientation.Southeast, 7, 0, 0, false, 0, 0));

            // Walking
            sequences.Add("WalkingSouth", new SpriteSequence(base.Parent, spriteInfo, "Walking", Orientation.South, 0, 1, 8, true, walkBuffers, walkSpeed));
            sequences.Add("WalkingSouthwest", new SpriteSequence(base.Parent, spriteInfo, "Walking", Orientation.Southwest, 1, 1, 8, true, walkBuffers, walkSpeed));
            sequences.Add("WalkingWest", new SpriteSequence(base.Parent, spriteInfo, "Walking", Orientation.West, 2, 1, 8, true, walkBuffers, walkSpeed));
            sequences.Add("WalkingNorthwest", new SpriteSequence(base.Parent, spriteInfo, "Walking", Orientation.Northwest, 3, 1, 8, true, walkBuffers, walkSpeed));
            sequences.Add("WalkingNorth", new SpriteSequence(base.Parent, spriteInfo, "Walking", Orientation.North, 4, 1, 8, true, walkBuffers, walkSpeed));
            sequences.Add("WalkingNortheast", new SpriteSequence(base.Parent, spriteInfo, "Walking", Orientation.Northeast, 5, 1, 8, true, walkBuffers, walkSpeed));
            sequences.Add("WalkingEast", new SpriteSequence(base.Parent, spriteInfo, "Walking", Orientation.East, 6, 1, 8, true, walkBuffers, walkSpeed));
            sequences.Add("WalkingSoutheast", new SpriteSequence(base.Parent, spriteInfo, "Walking", Orientation.Southeast, 7, 1, 8, true, walkBuffers, walkSpeed));

            // Running
            sequences.Add("RunningSouth", new SpriteSequence(base.Parent, spriteInfo, "Running", Orientation.South, 0, 1, 8, true, runBuffers, runSpeed));
            sequences.Add("RunningSouthwest", new SpriteSequence(base.Parent, spriteInfo, "Running", Orientation.Southwest, 1, 1, 8, true, runBuffers, runSpeed));
            sequences.Add("RunningWest", new SpriteSequence(base.Parent, spriteInfo, "Running", Orientation.West, 2, 1, 8, true, runBuffers, runSpeed));
            sequences.Add("RunningNorthwest", new SpriteSequence(base.Parent, spriteInfo, "Running", Orientation.Northwest, 3, 1, 8, true, runBuffers, runSpeed));
            sequences.Add("RunningNorth", new SpriteSequence(base.Parent, spriteInfo, "Running", Orientation.North, 4, 1, 8, true, runBuffers, runSpeed));
            sequences.Add("RunningNortheast", new SpriteSequence(base.Parent, spriteInfo, "Running", Orientation.Northeast, 5, 1, 8, true, runBuffers, runSpeed));
            sequences.Add("RunningEast", new SpriteSequence(base.Parent, spriteInfo, "Running", Orientation.East, 6, 1, 8, true, 1, runSpeed));
            sequences.Add("RunningSoutheast", new SpriteSequence(base.Parent, spriteInfo, "Running", Orientation.Southeast, 7, 1, 8, true, runBuffers, runSpeed));
            
            currentSequence = sequences["IdleSouth"];
             */
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
    }
}
