
#region Using Statements
using System;
using System.Collections.Generic;
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
        /// Current orientation of this actor.
        /// </summary>
        private Orientation actorOrientation = Orientation.South;
        public Orientation ActorOrientation
        {
            get { return actorOrientation; }
            set { actorOrientation = value; }
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
        /// Collection of sprite sequences for this actor.
        /// </summary>
        Dictionary<string, SpriteSequence> animations = new Dictionary<string,SpriteSequence>();

        /// <summary>
        /// Currently playing sprite sequence.
        /// </summary>
        SpriteSequence currentSequence;

        /// <summary>
        /// Next sequence to be played if current sequence is not interruptable.
        /// </summary>
        SpriteSequence nextSequence;

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

        #endregion

        public Actor()
        {
            throw new System.NotImplementedException();
        }

        public Actor(Texture2D spriteSheet)
        {
            this.spriteSheet = spriteSheet;
        }

        public void PlaySequence(SpriteSequence sequence)
        {
            if (currentSequence != sequence)
            {
                if (currentSequence.IsInterruptable)
                {
                    currentSequence = sequence;
                    currentFrame = sequence.StartFrame;
                    sourceRectangle = new Rectangle(currentFrame * (spriteWidth + spritePadding),
                        currentSequence.SheetRow * (spriteHeight + spritePadding), spriteWidth, spriteHeight);
                }
                else
                {
                    nextSequence = sequence;
                }
            }
        }

        public void PlaySequence(string title)
        {
            if(String.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("title");
            }
            PlaySequence(animations[title]);
        }

        public void UpdateSequence(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (isBufferFrame)
            {
                string title = State.ToString() + ActorOrientation.ToString();
                if (currentSequence.Title + currentSequence.Orientation.ToString() != title)
                {
                    PlaySequence(animations[title]);
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

                    sourceRectangle = new Rectangle(currentFrame * (spriteWidth + spritePadding),
                        currentSequence.SheetRow * (spriteHeight + spritePadding), spriteWidth, spriteHeight);

                    switch (ActorOrientation)
                    {
                        case Orientation.North:
                            destinationRectangle.Y -= currentSequence.Speed;
                            break;

                        case Orientation.Northeast:
                            destinationRectangle.Y -= currentSequence.Speed;
                            destinationRectangle.X += currentSequence.Speed;
                            break;

                        case Orientation.East:
                            destinationRectangle.X += currentSequence.Speed;
                            break;

                        case Orientation.Southeast:
                            destinationRectangle.Y += currentSequence.Speed;
                            destinationRectangle.X += currentSequence.Speed;
                            break;

                        case Orientation.South:
                            destinationRectangle.Y += currentSequence.Speed;
                            break;

                        case Orientation.Southwest:
                            destinationRectangle.Y += currentSequence.Speed;
                            destinationRectangle.X -= currentSequence.Speed;
                            break;

                        case Orientation.West:
                            destinationRectangle.X -= currentSequence.Speed;
                            break;

                        case Orientation.Northwest:
                            destinationRectangle.Y -= currentSequence.Speed;
                            destinationRectangle.X -= currentSequence.Speed;
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
            return;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteSheet,
                destinationRectangle,
                sourceRectangle,
                Color.White);
        }

        public void AddBasicSequences()
        {
            // Idle
            animations.Add("IdleSouth", new SpriteSequence("Idle", Orientation.South, 0, 0, 0, 0, false, true));
            animations.Add("IdleWest", new SpriteSequence("Idle", Orientation.West, 0, 1, 0, 0, false, true));
            animations.Add("IdleEast", new SpriteSequence("Idle", Orientation.East, 0, 2, 0, 0, false, true));
            animations.Add("IdleNorth", new SpriteSequence("Idle", Orientation.North, 0, 3, 0, 0, false, true));

            // Walking
            animations.Add("WalkingSouth", new SpriteSequence("Walking", Orientation.South, 10, 0, 1, 6, true, true));
            animations.Add("WalkingWest", new SpriteSequence("Walking", Orientation.West, 10, 1, 1, 6, true, true));
            animations.Add("WalkingEast", new SpriteSequence("Walking", Orientation.East, 10, 2, 1, 6, true, true));
            animations.Add("WalkingNorth", new SpriteSequence("Walking", Orientation.North, 10, 3, 1, 6, true, true));

            // Running
            animations.Add("RunningSouth", new SpriteSequence("Running", Orientation.South, 20, 0, 7, 12, true, true));
            animations.Add("RunningWest", new SpriteSequence("Running", Orientation.West, 20, 1, 7, 12, true, true));
            animations.Add("RunningEast", new SpriteSequence("Running", Orientation.East, 20, 2, 7, 12, true, true));
            animations.Add("RunningNorth", new SpriteSequence("Running", Orientation.North, 20, 3, 7, 12, true, true));

            // Padding for missing 8-direction animations
            animations.Add("IdleNorthwest", new SpriteSequence("Idle", Orientation.Northwest, 0, 1, 0, 0, false, true));
            animations.Add("IdleNortheast", new SpriteSequence("Idle", Orientation.Northeast, 0, 2, 0, 0, false, true));
            animations.Add("IdleSouthwest", new SpriteSequence("Idle", Orientation.Southwest, 0, 1, 0, 0, false, true));
            animations.Add("IdleSoutheast", new SpriteSequence("Idle", Orientation.Southeast, 0, 2, 0, 0, false, true));
            animations.Add("WalkingNorthwest", new SpriteSequence("Walking", Orientation.Northwest, 10, 1, 1, 6, true, true));
            animations.Add("WalkingNortheast", new SpriteSequence("Walking", Orientation.Northeast, 10, 2, 1, 6, true, true));
            animations.Add("WalkingSouthwest", new SpriteSequence("Walking", Orientation.Southwest, 10, 1, 1, 6, true, true));
            animations.Add("WalkingSoutheast", new SpriteSequence("Walking", Orientation.Southeast, 10, 2, 1, 6, true, true));
            animations.Add("RunningNorthwest", new SpriteSequence("Running", Orientation.Northwest, 20, 1, 7, 12, true, true));
            animations.Add("RunningNortheast", new SpriteSequence("Running", Orientation.Northeast, 20, 2, 7, 12, true, true));
            animations.Add("RunningSouthwest", new SpriteSequence("Running", Orientation.Southwest, 20, 1, 7, 12, true, true));
            animations.Add("RunningSoutheast", new SpriteSequence("Running", Orientation.Southeast, 20, 2, 7, 12, true, true));

            currentSequence = animations["IdleSouth"];
            sourceRectangle = new Rectangle(currentFrame * (spriteWidth + spritePadding),
                currentSequence.SheetRow, spriteWidth, spriteHeight);
        }

        public void Idle()
        {
            state = ActorState.Idle;
            PlaySequence(state.ToString() + currentSequence.Orientation.ToString());
        }
    }
}
