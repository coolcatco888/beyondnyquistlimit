using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace Library
{
    public class MultipleSequenceInfo
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

        #region MultipleSequence Information

        private float duration;
        public float Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        private List<SpriteSequenceInfo> subsequences;
        public List<SpriteSequenceInfo> Subsequences
        {
            get { return subsequences; }
            set { subsequences = value; }
        }

        #endregion  // MultipleSequence Information

        #region Content Reader

        public class MultipleSequenceInfoReader : ContentTypeReader<MultipleSequenceInfo>
        {
            protected override MultipleSequenceInfo Read(ContentReader input, MultipleSequenceInfo existingInstance)
            {
                MultipleSequenceInfo info = new MultipleSequenceInfo();

                info.StateKey = input.ReadString();
                info.OrientationKey = input.ReadString();
                info.Duration = input.ReadSingle();

                info.Subsequences = input.ReadObject<List<SpriteSequenceInfo>>();

                return info;
            }
        }

        #endregion // Content Reader
    }
}
