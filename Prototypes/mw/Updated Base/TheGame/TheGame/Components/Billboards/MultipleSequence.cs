﻿
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

        public MultipleSequence(SpriteSequence sequence, float duration)
            : base(sequence.Title, sequence.Orientation, sequence.IsLoop, sequence.Speed, sequence.BufferFrames)
        {
            this.sequenceDuration = duration;

            this.scale = sequence.Scale;
            this.IsPaused = sequence.IsPaused;
            this.isComplete = sequence.IsComplete;
            this.frame = sequence.FrameList;
            this.currentFrame = sequence.CurrentFrame;
            this.frameTotal = sequence.FrameTotal;
            this.frameIndex = sequence.FrameIndex;
        }

        public MultipleSequence(string title, Orientation orientation, bool isLoop, int bufferFrames, float duration)
            : base(isLoop, bufferFrames)
        {
            this.title = title;
            this.orientation = orientation;
            this.sequenceDuration = duration;
        }

        public void Initialize()
        {
            allComplete = false;
            sequenceIndex = 0;

            sequenceTimer = 0.0f;

            this.SetToSequence(sequenceList[0]);
        }

        public override string Update(GameTime gameTime)
        {
            if (this.isLoop)
            {
                sequenceTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (sequenceTimer >= sequenceDuration && sequenceDuration >= 0)
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
                else if (sequenceDuration >= 0)
                {
                    allComplete = true;
                }
            }

 	        return base.Update(gameTime);
        }

        public void SetToSequence(SpriteSequence sequence)
        {
            this.orientation = sequence.Orientation;
            this.speed = sequence.Speed;
            this.isLoop = sequence.IsLoop;

            this.IsPaused = sequence.IsPaused;
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
            this.allComplete = false;

            base.Reset();
        }

        public void StopLooping()
        {
            this.sequenceDuration = 0.0f;
        }

        public void NextSequence()
        {
            if (sequenceIndex < sequenceList.Count - 1)
            {
                sequenceIndex++;
                this.SetToSequence(sequenceList[sequenceIndex]);
            }
            else if (sequenceDuration >= 0)
            {
                allComplete = true;
            }
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
    }
}