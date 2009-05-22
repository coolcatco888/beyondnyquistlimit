using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    /// <summary>
    /// Structure class of player specific information.  Can be loaded from XML.
    /// </summary>
    public class PlayerInfo : ActorInfo
    {
        private string className;
        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }

        private int classLevel;
        public int ClassLevel
        {
            get { return classLevel; }
            set { classLevel = value; }
        }

        private int experience;
        public int Experience
        {
            get { return experience; }
            set { experience = value; }
        }

        private int experienceToNextLevel;
        public int ExperienceToNextLevel
        {
            get { return experienceToNextLevel; }
            set { experienceToNextLevel = value; }
        }

        private int currentAttackGauge;
        public int CurrentAttackGauge
        {
            get { return currentAttackGauge; }
            set { currentAttackGauge = value; }
        }

        private int maxAttackGauge;
        public int MaxAttackGauge
        {
            get { return maxAttackGauge; }
            set { maxAttackGauge = value; }
        }
    }
}
