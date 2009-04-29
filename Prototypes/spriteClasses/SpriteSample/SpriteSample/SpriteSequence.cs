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
        private Boolean loop;
        public Boolean Loop
        {
            get { return loop; }
            set { loop = value; }
        }

        public SpriteSequence() { }

        public SpriteSequence(string title, int sheetRow, int startFrame, int endFrame, Boolean loop)
        {
            this.Title = title;
            this.SheetRow = sheetRow;
            this.StartFrame = startFrame;
            this.EndFrame = endFrame;
            this.Loop = loop;
        }
    }
}
