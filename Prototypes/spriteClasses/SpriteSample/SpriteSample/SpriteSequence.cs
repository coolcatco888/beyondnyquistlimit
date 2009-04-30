using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpriteSample
{
    class SpriteSequence
    {
        /// <summary>
        /// Title of this sequence.
        /// </summary>
        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// Row to reference on sprite sheet.
        /// </summary>
        private int sheetRow;
        public int SheetRow
        {
            get { return sheetRow; }
            set { sheetRow = value; }
        }

        /// <summary>
        /// Starting frame for this sequence.
        /// </summary>
        private int startFrame;
        public int StartFrame
        {
            get { return startFrame; }
            set { startFrame = value; }
        }

        /// <summary>
        /// Last frame for this sequence.
        /// </summary>
        private int endFrame;
        public int EndFrame
        {
            get { return endFrame; }
            set { endFrame = value; }
        }

        /// <summary>
        /// Whether or not this sequence should loop on completion.
        /// </summary>
        private Boolean isLoop;
        public Boolean IsLoop
        {
            get { return isLoop; }
            set { isLoop = value; }
        }

        /// <summary>
        /// Whether or not this sequence may be interrupted.
        /// </summary>
        private Boolean isInterruptable;
        public Boolean IsInterruptable
        {
            get { return isInterruptable; }
            set { isInterruptable = value; }
        }

        /// <summary>
        /// Orientation of sprite sequence.
        /// </summary>
        private Orientation orientation;
        public Orientation Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }

        /// <summary>
        /// Travel speed of sprite during this sequence.
        /// </summary>
        private int speed;
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public SpriteSequence() { }

        public SpriteSequence(string title, Orientation orientation, int speed, int sheetRow, int startFrame, int endFrame, Boolean loop, Boolean interruptable)
        {
            this.Title = title;
            this.Orientation = orientation;
            this.Speed = speed;
            this.SheetRow = sheetRow;
            this.StartFrame = startFrame;
            this.EndFrame = endFrame;
            this.IsLoop = loop;
            this.IsInterruptable = interruptable;
        }
    }
}
