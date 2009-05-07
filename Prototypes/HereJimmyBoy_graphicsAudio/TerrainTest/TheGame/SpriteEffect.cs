
#region Using Statements

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

#endregion
/*
namespace TheGame
{
    class SpriteEffect : Billboard, IAudioEmitter
    {
        public enum SpriteEffectState
        {
            Idle,
            Active,
            Finished,
        }

        private SpriteEffectState state;
        public SpriteEffectState State
        {
            get { return state; }
            set { state = value; }
        }

        private SpriteSequence sequence;

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

        float duration = 1.0f;

        /// <summary>
        /// Unit area of sprite relative to sprite sheet; between 0 & 1.
        /// </summary>
        Vector2 spriteUnit;

        public SpriteEffect(GameScreen parent, SpriteInfo spriteInfo)
            : base(parent, spriteInfo)
        {
        }

        public SpriteEffect(GameScreen parent, SpriteInfo spriteInfo,
            SpriteSequence sequence, Vector3 position, float duration)
            : base(parent, spriteInfo)
        {
            this.sequence = sequence;
            this.position = position;
            this.duration = duration;

            vertices[0].TextureCoordinate = new Vector2(0, 0);
            vertices[1].TextureCoordinate = new Vector2(spriteUnit.X, 0);
            vertices[2].TextureCoordinate = new Vector2(spriteUnit.X, spriteUnit.Y);
            vertices[3].TextureCoordinate = new Vector2(0, spriteUnit.Y);
        }

        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (currentFrame == 0)
            {
                AudioManager audioManager = (AudioManager)GameEngine.Services.GetService(typeof(AudioManager));
                audioManager.Play3DCue("asplosion", this);
            }

            if (timer > interval)
            {
                // Reset to first frame if sequence loops.
                if (sequence.IsLoop && currentFrame == sequence.EndFrame)
                {
                    currentFrame = sequence.StartFrame;
                }

                // Set state to finish if sequence is complete & does not loop.
                else if (!sequence.IsLoop && currentFrame == sequence.EndFrame)
                {
                    state = SpriteEffectState.Finished;

                    // Dispose if duration has expired.
                    if(duration <= 0)
                        this.Dispose();
                }
                else
                {
                    currentFrame++;
                }

                // Select correct sprite from sprite sheet.
                vertices[0].TextureCoordinate = new Vector2(currentFrame * spriteUnit.X, sequence.SheetRow * spriteUnit.Y);
                vertices[1].TextureCoordinate = new Vector2(currentFrame * spriteUnit.X + spriteUnit.X, sequence.SheetRow * spriteUnit.Y);
                vertices[2].TextureCoordinate = new Vector2(currentFrame * spriteUnit.X + spriteUnit.X, sequence.SheetRow * spriteUnit.Y + spriteUnit.Y);
                vertices[3].TextureCoordinate = new Vector2(currentFrame * spriteUnit.X, sequence.SheetRow * spriteUnit.Y + spriteUnit.Y);
                
                timer = 0.0f;
            }

            duration -= timer;

            base.Update(gameTime);
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
            get { return Vector3.Zero; }
        }

        #endregion
    }
}
*/
