using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace Library
{
    public class SpriteSequenceInfo
    {
        #region Dictionary Key Information

        // Actor state as a string. In the dictionary is one half of the key
        private string stateKey;
        public string StateKey
        {
            get { return stateKey; }
            set { stateKey = value; }
        }

        // Orientation as a string.  In the dictionary is one half of the key
        private string orientationKey;
        public string OrientationKey
        {
            get { return orientationKey; }
            set { orientationKey = value; }
        }

        #endregion // Dictionary Key Information

        #region Sequence Information

        private bool isLoop;
        public bool IsLoop
        {
            get { return isLoop; }
            set { isLoop = value; }
        }

        private float sequenceVelocity;
        public float SequenceVelocity
        {
            get { return sequenceVelocity; }
            set { sequenceVelocity = value; }
        }

        private int numBuffers;
        public int NumBuffers
        {
            get { return numBuffers; }
            set { numBuffers = value; }
        }

        private int scaleX;
        public int ScaleX
        {
            get { return scaleX; }
            set { scaleX = value; }
        }

        private int scaleY;
        public int ScaleY
        {
            get { return scaleY; }
            set { scaleY = value; }
        }

        #endregion

        #region Flags

        private bool changeScale;
        public bool ChangeScale
        {
            get { return changeScale; }
            set { changeScale = value; }
        }

        private bool isARowOrColumn;
        public bool IsARowOrColumn
        {
            get { return isARowOrColumn; }
            set { isARowOrColumn = value; }
        }

        #endregion // Flags

        #region Frame Indexes

        private int indexX;
        public int IndexX
        {
            get { return indexX; }
            set { indexX = value; }
        }

        private int indexY;
        public int IndexY
        {
            get { return indexY; }
            set { indexY = value; }
        }

        private int rowOrColumn;
        public int RowOrColumn
        {
            get { return rowOrColumn; }
            set { rowOrColumn = value; }
        }

        #endregion

        #region Attack Slashes

        private int attackKey;
        public int AttackKey
        {
            get { return attackKey; }
            set { attackKey = value; }
        }

        private string attackValue;
        public string AttackValue
        {
            get { return attackValue; }
            set { attackValue = value; }
        }

        #endregion  // Attack Slashes

        #region Content Reader

        public class SpriteSequenceInfoReader : ContentTypeReader<SpriteSequenceInfo>
        {

            protected override SpriteSequenceInfo Read(ContentReader input, SpriteSequenceInfo existingInstance)
            {
                SpriteSequenceInfo spriteSequenceInfo = new SpriteSequenceInfo();

                spriteSequenceInfo.stateKey = input.ReadString();
                spriteSequenceInfo.orientationKey = input.ReadString();

                spriteSequenceInfo.isLoop = input.ReadBoolean();

                spriteSequenceInfo.sequenceVelocity = input.ReadSingle();

                spriteSequenceInfo.numBuffers = input.ReadInt32();
                spriteSequenceInfo.scaleX = input.ReadInt32();
                spriteSequenceInfo.scaleY = input.ReadInt32();

                spriteSequenceInfo.changeScale = input.ReadBoolean();
                spriteSequenceInfo.isARowOrColumn = input.ReadBoolean();

                spriteSequenceInfo.indexX = input.ReadInt32();
                spriteSequenceInfo.indexY = input.ReadInt32();
                spriteSequenceInfo.RowOrColumn = input.ReadInt32();

                spriteSequenceInfo.attackKey = input.ReadInt32();
                spriteSequenceInfo.attackValue = input.ReadString();

                return spriteSequenceInfo;
            }
        }

        #endregion
    }
}
