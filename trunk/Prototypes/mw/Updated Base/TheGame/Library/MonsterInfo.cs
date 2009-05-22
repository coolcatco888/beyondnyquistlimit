using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    public class MonsterInfo
    {
        private int health;
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        private int mana;
        public int Mana
        {
            get { return mana; }
            set { mana = value; }
        }

        private int damage;
        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        private int defense;
        public int Defense
        {
            get { return defense; }
            set { defense = value; }
        }

        private int experience;
        public int Experience
        {
            get { return experience; }
            set { experience = value; }
        }
    }
}
