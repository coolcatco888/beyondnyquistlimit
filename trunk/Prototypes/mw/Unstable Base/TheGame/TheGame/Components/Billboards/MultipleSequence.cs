
#region Using Statements

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

#endregion


namespace TheGame
{
    public class MultipleSequence : SpriteSequence
    {
        #region Fields

        protected List<SpriteSequence> sequenceList = new List<SpriteSequence>();
        protected int sequenceIndex;
        protected float sequenceDuration;
        protected float sequenceTimer;
        protected bool allComplete;

        #endregion  // Fields

        #region Accessors

        public override bool IsComplete
        {
	        get { return allComplete; }
        }

        #endregion  // Accessors

        public MultipleSequence(SpriteSequence sequence)
            : base(sequence.Title, sequence.Orientation, sequence.IsLoop, sequence.Velocity, sequence.BufferFrames)
        {
            this.scale = sequence.Scale;
            this.isPaused = sequence.IsPaused;
            this.isComplete = sequence.IsComplete;
            this.frame = sequence.FrameList;
            this.currentFrame = sequence.CurrentFrame;
            this.frameTotal = sequence.FrameTotal;
            this.frameIndex = sequence.FrameIndex;
        }

        public MultipleSequence(bool isLoop, int bufferFrames)
            : base(isLoop, bufferFrames)
        {
        }

        public MultipleSequence(bool isLoop, int bufferFrames, int xScale, int yScale)
            : base(isLoop, bufferFrames, xScale, yScale)
        {
        }

        public MultipleSequence(String title, Orientation orientation, bool isLoop, float velocity, int bufferFrames)
            : base(title, orientation, isLoop, velocity, bufferFrames)
        {
        }

        public MultipleSequence(String title, Orientation orientation, bool isLoop, float velocity, int bufferFrames, int xScale, int yScale)
            : base(title, orientation, isLoop, velocity, bufferFrames, xScale, yScale)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            allComplete = false;
            sequenceIndex = 0;

            sequenceDuration = 1000.0f;
            sequenceTimer = 0.0f;

            this.SetToSequence(sequenceList[0]);
        }

        public override void Update(GameTime gameTime)
        {
            if (this.isLoop)
            {
                sequenceTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (sequenceTimer >= sequenceDuration)
                {
                    this.isComplete = true;
                    sequenceTimer = 0.0f;
                }
            }

            if (this.isComplete)
            {
                if (sequenceIndex < sequenceList.Count - 1)
                {
                    sequenceIndex++;
                    this.SetToSequence(sequenceList[sequenceIndex]);
                }
                else
                {
                    allComplete = true;
                }
            }

 	        base.Update(gameTime);
        }

        public void SetToSequence(SpriteSequence sequence)
        {
            this.orientation = sequence.Orientation;
            this.velocity = sequence.Velocity;
            this.isLoop = sequence.IsLoop;

            this.isPaused = sequence.IsPaused;
            this.isComplete = sequence.IsComplete;

            this.frame = sequence.FrameList;
            this.currentFrame = sequence.CurrentFrame;
            this.frameTotal = sequence.FrameTotal;
            this.frameIndex = sequence.FrameIndex;

            this.scale = sequence.Scale;
            this.bufferFrames = sequence.BufferFrames;
            this.bufferTotal = sequence.BufferTotal;
        }

        public override void Reset()
        {
            foreach (SpriteSequence sequence in sequenceList)
            {
                sequence.Reset();
            }

            this.sequenceIndex = 0;
            this.SetToSequence(sequenceList[0]);
            this.isPaused = true;
            this.allComplete = false;

            base.Reset();
        }

        #region Frame Methods

        public void AddSequence(SpriteSequence sequence)
        {
            sequenceList.Add(sequence);
        }

        public void RemoveSequence(SpriteSequence sequence)
        {
            sequenceList.Remove(sequence);
        }

        public void RemoveSequence(int index)
        {
            sequenceList.RemoveAt(index);
        }

        #endregion // Frame Methods


        #region ICloneable Members

        public new object Clone()
        {
            MultipleSequence clone = new MultipleSequence((SpriteSequence)base.Clone());

            clone.sequenceList = this.sequenceList;
            clone.sequenceIndex = this.sequenceIndex;
            clone.sequenceDuration = this.sequenceDuration;
            clone.sequenceTimer = this.sequenceTimer;
            clone.allComplete = this.allComplete;

            return clone;
        }

        #endregion
    }
}
