
#region Using Statements

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace TheGame
{
    class SpriteSequence
    {
#region Fields

        private string title;
        private Orientation orientation;
        private float velocity;
        private bool isLoop;
        private int updateCount;

        private float timer = 0.0f;
        private float interval = 1000.0f / 25.0f;

        private List<Point> frame = new List<Point>();
        private Point currentFrame;
        private int frameTotal = 0;
        private int frameIndex = 0;

        private int bufferFrames;
        private int bufferTotal;

        #endregion  // Fields

#region Accessors

        public string Title
        {
            get { return title; }
        }

        public Orientation Orientation
        {
            get { return orientation; }
        }

        public bool IsLoop
        {
            get { return isLoop; }
        }

        public float Velocity
        {
            get { return velocity; }
        }

        public int UpdateCount
        {
            get { return updateCount; }
        }

        public int CurrentFrameColumn
        {
            get { return currentFrame.X; }
        }

        public int CurrentFrameRow
        {
            get { return currentFrame.Y; }
        }

#endregion  // Accessors

#region Constructors

        public SpriteSequence(string title, Orientation orientation, bool isLoop, float velocity, int bufferFrames)
        {
            this.title = title;
            this.orientation = orientation;
            this.isLoop = isLoop;
            this.velocity = velocity;

            this.bufferTotal = bufferFrames;
            this.bufferFrames = bufferFrames;
        }

#endregion  // Constructors

#region Update

        public void Update(GameTime gameTime)
        {
            updateCount = 0;
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            while (timer > 0)
            {
                if (bufferFrames == 0)
                {
                    // Reset to first frame if sequence loops.
                    if (isLoop && frameIndex == frameTotal - 1)
                    {
                        frameIndex = 0;
                    }
                    // Remain on last frame if sequence does not loop.
                    else if (!isLoop && frameIndex == frameTotal - 1)
                    {
                    }
                    else
                    {
                        frameIndex++;
                    }

                    // Reassign current frame.
                    currentFrame = frame[frameIndex];

                    bufferFrames = bufferTotal;
                }
                else
                {
                    bufferFrames--;
                }

                // Increment updates.
                updateCount++;

                // Decrement timer.
                timer -= interval;
            }
        }

#endregion  // Update

#region Frame Methods

        public void AddFrame(Point frameLocation)
        {
            frame.Add(frameLocation);
            frameTotal++;
        }

        public void AddFrame(int x, int y)
        {
            this.AddFrame(new Point(x, y));
        }

        public void AddRow(int row, int start, int end)
        {
            while (start <= end)
            {
                this.AddFrame(start++, row);
            }
        }

        public void AddColumn(int column, int start, int end)
        {
            while (start <= end)
            {
                this.AddFrame(column, start++);
            }
        }

        public void EmptyFrames()
        {
            while (frameTotal > 0)
            {
                frame.RemoveAt(--frameTotal);
            }
            frameIndex = 0;
        }

 #endregion // Add Frame Methods
    }
}