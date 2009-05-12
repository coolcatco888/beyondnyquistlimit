using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace ContentTestLibrary
{
    public class LevelStruct
    {
        private int maxLevel;
        public int MaxLevel
        {
            get { return maxLevel; }
            set { maxLevel = value; }
        }

        private List<int> experienceLevels = new List<Int32>();
        public List<int> ExperienceLevels
        {
            get { return experienceLevels; }
        }

        public int getExperienceNeededToLevel(int level)
        {
            if (level <= maxLevel && level >= 1)
                return experienceLevels[level - 1];
            else
                throw new ArgumentOutOfRangeException("level", "Level needs to be between 1 and " + maxLevel);
        }

        public class LevelStructReader : ContentTypeReader<LevelStruct>
        {
            protected override LevelStruct Read(ContentReader input, LevelStruct existingInstance)
            {
                LevelStruct levelStruct = new LevelStruct();

                levelStruct.maxLevel = input.ReadInt32();
                levelStruct.ExperienceLevels.AddRange(input.ReadObject<List<Int32>>());

                return levelStruct;
            }
        }
    }
}
