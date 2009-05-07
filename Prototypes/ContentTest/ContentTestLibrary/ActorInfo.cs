using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace ContentTestLibrary
{
    public class ActorInfo
    {
        #region Current Statistics
        
        // Current Health
        private int health;
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        // Current mana/stamina
        private int mana;
        public int Mana
        {
            get { return mana; }
            set { mana = value; }
        }

        #endregion

        #region Calculated Statistics

        // Maximum health: calculated base on base value, modifier, and level
        [ContentSerializerIgnore]
        private int maxHealth;
        [ContentSerializerIgnore]
        public int MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

        // Maximum mana: calculated base on base value, modifier, and level
        [ContentSerializerIgnore]
        private int maxMana;
        [ContentSerializerIgnore]
        public int MaxMana
        {
            get { return maxMana; }
            set { maxMana = value; }
        }

        // Current damage: calculated based on base value, modifier, and level
        [ContentSerializerIgnore]
        private int damage;
        [ContentSerializerIgnore]
        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        // Current speed: calculated based on base value, modifier, and level
        [ContentSerializerIgnore]
        private int speed;
        [ContentSerializerIgnore]
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        #endregion

        #region Sprite Sheet(s)

        private string spriteXmlFile;
        public string SpriteXmlFile
        {
            get { return spriteXmlFile; }
            set { spriteXmlFile = value; }
        }

        /// <summary>
        /// SpriteInfo of a set of sprites on a sheet.
        /// </summary>
        [ContentSerializerIgnore]
        private SpriteInfo sprites;
        [ContentSerializerIgnore]
        public SpriteInfo Sprites
        {
            get { return sprites; }
        }

        #endregion

        #region Loading Methods

        /// <summary>
        /// Load the SpriteInfo from the xml file
        /// </summary>
        public void LoadSpriteInfo(ContentManager content)
        {
            sprites = content.Load<SpriteInfo>(@spriteXmlFile);
        }

        #endregion

        #region Content Reader

        public class ActorInfoReader : ContentTypeReader<ActorInfo>
        {
            protected override ActorInfo Read(ContentReader input, ActorInfo existingInstance)
            {
                ActorInfo actorInfo = existingInstance;
                if (actorInfo == null)
                {
                    actorInfo = new ActorInfo();
                }

                actorInfo.health = input.ReadInt32();
                actorInfo.mana = input.ReadInt32();
                actorInfo.spriteXmlFile = input.ReadString();

                actorInfo.LoadSpriteInfo(input.ContentManager);

                return actorInfo;
            }
        }

        #endregion

    }
}
