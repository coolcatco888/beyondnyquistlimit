
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
        /// Current orientation of this actor.
        /// </summary>
        private Orientation actorOrientation = Orientation.South;
        public Orientation ActorOrientation
        {
            get { return actorOrientation; }
            set { actorOrientation = value; }
        }

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
        /// Collection of sprite sequences for this actor.
        /// </summary>
        Dictionary<string, SpriteSequence> animations = new Dictionary<string, SpriteSequence>();

        /// <summary>
        /// Currently playing sprite sequence.
        /// </summary>
        SpriteSequence currentSequence;

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
        /// Unit area of sprite relative to sprite sheet; between 0 & 1.
        /// </summary>
        Vector2 spriteUnit;

        /// <summary>
        /// Double buffering for sequence frames.
        /// </summary>
        bool isBufferFrame = false;

        // OMG ASPLODE!!!
        SpriteEffect asplode;
        float timer2 = 0.0f;

        public Actor(GameScreen parent, Texture2D spriteSheet, int spriteWidth, int spriteHeight, int spritePadding)
            : base(parent, spriteSheet, spriteWidth, spriteHeight, spritePadding)
        {
            spriteUnit = new Vector2((float)(spriteWidth + spritePadding) / (float)spriteSheet.Width,
                (float)(spriteHeight + spritePadding) / (float)spriteSheet.Height);

            AddBasicSequences();
        }

        public override void Update(GameTime gameTime)
        {
            controller();

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            HeightMapInfo heightInfo = ((Level)Parent).TerrainHeightMap;
            Vector3 oldPosition = this.Position;

            if (isBufferFrame)
            {
                string title = State.ToString() + ActorOrientation.ToString();
                if (currentSequence.Title + currentSequence.Orientation.ToString() != title)
                {
                    playSequence(animations[title]);
                }
                else if (timer > interval)
                {
                    // Reset to first frame if sequence loops.
                    if (currentSequence.IsLoop && currentFrame == currentSequence.EndFrame)
                    {
                        currentFrame = currentSequence.StartFrame;
                    }

                    // Remain on last frame if sequence does not loop.
                    else if (!currentSequence.IsLoop && currentFrame == currentSequence.EndFrame)
                    {
                    }
                    else
                    {
                        currentFrame++;
                    }

                    vertices[0].TextureCoordinate = new Vector2(currentFrame * spriteUnit.X, currentSequence.SheetRow * spriteUnit.Y);
                    vertices[1].TextureCoordinate = new Vector2(currentFrame * spriteUnit.X + spriteUnit.X, currentSequence.SheetRow * spriteUnit.Y);
                    vertices[2].TextureCoordinate = new Vector2(currentFrame * spriteUnit.X + spriteUnit.X, currentSequence.SheetRow * spriteUnit.Y + spriteUnit.Y);
                    vertices[3].TextureCoordinate = new Vector2(currentFrame * spriteUnit.X, currentSequence.SheetRow * spriteUnit.Y + spriteUnit.Y);

                    // Update position.
                    switch (ActorOrientation)
                    {
                        case Orientation.North:
                            position.Z -= currentSequence.Speed;
                            if (heightInfo.GetHeight(position) != -1.0f)
                            {
                                position = oldPosition;
                            }
                            break;

                        case Orientation.Northeast:
                            position.Z -= currentSequence.Speed;
                            if (heightInfo.GetHeight(position) != -1.0f)
                            {
                                position = oldPosition;
                            }
                            else
                            {
                                oldPosition = position;
                            }
                            position.X += currentSequence.Speed;
                            if (heightInfo.GetHeight(position) != -1.0f)
                            {
                                position = oldPosition;
                            }
                            break;

                        case Orientation.East:
                            position.X += currentSequence.Speed;
                            if (heightInfo.GetHeight(position) != -1.0f)
                            {
                                position = oldPosition;
                            }
                            break;

                        case Orientation.Southeast:
                            position.Z += currentSequence.Speed;
                            if (heightInfo.GetHeight(position) != -1.0f)
                            {
                                position = oldPosition;
                            }
                            else
                            {
                                oldPosition = position;
                            }
                            position.X += currentSequence.Speed;
                            if (heightInfo.GetHeight(position) != -1.0f)
                            {
                                position = oldPosition;
                            }
                            break;

                        case Orientation.South:
                            position.Z += currentSequence.Speed;
                            if (heightInfo.GetHeight(position) != -1.0f)
                            {
                                position = oldPosition;
                            }
                            break;

                        case Orientation.Southwest:
                            position.Z += currentSequence.Speed;
                            if (heightInfo.GetHeight(position) != -1.0f)
                            {
                                position = oldPosition;
                            }
                            else
                            {
                                oldPosition = position;
                            }
                            position.X -= currentSequence.Speed;
                            if (heightInfo.GetHeight(position) != -1.0f)
                            {
                                position = oldPosition;
                            }
                            break;

                        case Orientation.West:
                            position.X -= currentSequence.Speed;
                            if (heightInfo.GetHeight(position) != -1.0f)
                            {
                                position = oldPosition;
                            }
                            break;

                        case Orientation.Northwest:
                            position.Z -= currentSequence.Speed;
                            if (heightInfo.GetHeight(position) != -1.0f)
                            {
                                position = oldPosition;
                            }
                            else
                            {
                                oldPosition = position;
                            }
                            position.X -= currentSequence.Speed;
                            if (heightInfo.GetHeight(position) != -1.0f)
                            {
                                position = oldPosition;
                            }
                            break;
                    }
                    
                    timer = 0.0f;
                    isBufferFrame = false;
                }
            }
            else
            {
                isBufferFrame = true;
            }

            // OMG ASPLODE SO MUCH!!!
            timer2 += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timer2 > 500.0f)
            {
                asplode = new SpriteEffect(base.Parent, GameEngine.Content.Load<Texture2D>("sprite_sheet"),
                    64, 64, 0, new SpriteSequence(0, 0, 15, false), new Vector3(position.X, position.Y, position.Z - 0.1f), 1);
                timer2 = 0.0f;
            }

            base.Update(gameTime);
        }

        private void playSequence(SpriteSequence sequence)
        {
            if (currentSequence != sequence)
            {
                currentSequence = sequence;
                currentFrame = sequence.StartFrame;
            }
        }

        public void playSequence(string title)
        {
            if (String.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("title");
            }
            playSequence(animations[title]);
        }

        private void controller()
        {
            KeyboardDevice keyboardDevice = (KeyboardDevice)GameEngine.Services.GetService(typeof(KeyboardDevice));

            if (keyboardDevice.IsKeyDown(Keys.LeftShift))
                state = Actor.ActorState.Running;
            else
                state = Actor.ActorState.Walking;


            if (keyboardDevice.IsKeyDown(Keys.Up))
            {
                if (keyboardDevice.IsKeyDown(Keys.Right))
                    actorOrientation = Orientation.Northeast;
                else if (keyboardDevice.IsKeyDown(Keys.Left))
                    actorOrientation = Orientation.Northwest;
                else
                    actorOrientation = Orientation.North;
            }
            else if (keyboardDevice.IsKeyDown(Keys.Down))
            {
                if (keyboardDevice.IsKeyDown(Keys.Right))
                    actorOrientation = Orientation.Southeast;
                else if (keyboardDevice.IsKeyDown(Keys.Left))
                    actorOrientation = Orientation.Southwest;
                else
                    actorOrientation = Orientation.South;
            }
            else if (keyboardDevice.IsKeyDown(Keys.Right))
            {
                actorOrientation = Orientation.East;
            }
            else if (keyboardDevice.IsKeyDown(Keys.Left))
            {
                actorOrientation = Orientation.West;
            }
            else
            {
                Idle();
            }
        }

        public void AddBasicSequences()
        {
            // Idle
            animations.Add("IdleSouth", new SpriteSequence("Idle", Orientation.South, 0, 0, 0, 0, false, true));
            animations.Add("IdleWest", new SpriteSequence("Idle", Orientation.West, 0, 1, 0, 0, false, true));
            animations.Add("IdleEast", new SpriteSequence("Idle", Orientation.East, 0, 2, 0, 0, false, true));
            animations.Add("IdleNorth", new SpriteSequence("Idle", Orientation.North, 0, 3, 0, 0, false, true));

            // Walking
            animations.Add("WalkingSouth", new SpriteSequence("Walking", Orientation.South, 0.8f, 0, 1, 6, true, true));
            animations.Add("WalkingWest", new SpriteSequence("Walking", Orientation.West, 0.4f, 1, 1, 6, true, true));
            animations.Add("WalkingEast", new SpriteSequence("Walking", Orientation.East, 0.4f, 2, 1, 6, true, true));
            animations.Add("WalkingNorth", new SpriteSequence("Walking", Orientation.North, 0.8f, 3, 1, 6, true, true));
            
            // Running
            animations.Add("RunningSouth", new SpriteSequence("Running", Orientation.South, 1.6f, 0, 7, 12, true, true));
            animations.Add("RunningWest", new SpriteSequence("Running", Orientation.West, 0.8f, 1, 7, 12, true, true));
            animations.Add("RunningEast", new SpriteSequence("Running", Orientation.East, 0.8f, 2, 7, 12, true, true));
            animations.Add("RunningNorth", new SpriteSequence("Running", Orientation.North, 1.6f, 3, 7, 12, true, true));

            // Padding for missing 8-direction animations
            animations.Add("IdleNorthwest", new SpriteSequence("Idle", Orientation.Northwest, 0, 1, 0, 0, false, true));
            animations.Add("IdleNortheast", new SpriteSequence("Idle", Orientation.Northeast, 0, 2, 0, 0, false, true));
            animations.Add("IdleSouthwest", new SpriteSequence("Idle", Orientation.Southwest, 0, 1, 0, 0, false, true));
            animations.Add("IdleSoutheast", new SpriteSequence("Idle", Orientation.Southeast, 0, 2, 0, 0, false, true));
            animations.Add("WalkingNorthwest", new SpriteSequence("Walking", Orientation.Northwest, 0.4f, 1, 1, 6, true, true));
            animations.Add("WalkingNortheast", new SpriteSequence("Walking", Orientation.Northeast, 0.4f, 2, 1, 6, true, true));
            animations.Add("WalkingSouthwest", new SpriteSequence("Walking", Orientation.Southwest, 0.4f, 1, 1, 6, true, true));
            animations.Add("WalkingSoutheast", new SpriteSequence("Walking", Orientation.Southeast, 0.4f, 2, 1, 6, true, true));
            animations.Add("RunningNorthwest", new SpriteSequence("Running", Orientation.Northwest, 0.8f, 1, 7, 12, true, true));
            animations.Add("RunningNortheast", new SpriteSequence("Running", Orientation.Northeast, 0.8f, 2, 7, 12, true, true));
            animations.Add("RunningSouthwest", new SpriteSequence("Running", Orientation.Southwest, 0.8f, 1, 7, 12, true, true));
            animations.Add("RunningSoutheast", new SpriteSequence("Running", Orientation.Southeast, 0.8f, 2, 7, 12, true, true));

            currentSequence = animations["IdleSouth"];
            currentFrame = currentSequence.StartFrame;
        }

        public void Idle()
        {
            state = ActorState.Idle;
            playSequence(state.ToString() + currentSequence.Orientation.ToString());
        }

        #endregion  // Fields
    }
}
