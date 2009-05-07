using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace ContentTestLibrary
{
    public class Equipment
    {
        #region Descriptions

        /// <summary>
        /// The title of the item
        /// </summary>
        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// Long description of the item
        /// </summary>
        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        #endregion

        #region Data with Values

        /// <summary>
        /// The base gold value of the item
        /// </summary>
        private int goldValue;
        public int GoldValue
        {
            get { return goldValue; }
            set { goldValue = value; }
        }

        #endregion

        #region Class Restrictions

        /// <summary>
        /// List of classes that cannot use the item
        /// </summary>
        private List<string> restrictedClasses = new List<string>();
        public List<string> RestrictedClasses
        {
            get { return restrictedClasses; }
        }

        /// <summary>
        /// Minimum level required to use the item
        /// </summary>
        private int minLevel;
        public int MinimumLevel
        {
            get { return minLevel; }
            set { minLevel = value; }
        }

        #endregion

        #region Texture Info

        /// <summary>
        /// File name for the texture icon
        /// </summary>
        private string textureFileName;
        public string TextureFileName
        {
            get { return textureFileName; }
            set { textureFileName = value; }
        }

        #endregion

        // Will add more fields

        #region Content Reader
        public class EquipmentContentReader : ContentTypeReader<Equipment>
        {
            protected override Equipment Read(ContentReader input, Equipment existingInstance)
            {
                Equipment equipment = existingInstance;
                if (equipment == null)
                {
                    equipment = new Equipment();
                }

                equipment.Title = input.ReadString();
                equipment.Description = input.ReadString();
                equipment.GoldValue = input.ReadInt32();
                equipment.RestrictedClasses.AddRange(input.ReadObject<List<string>>());
                equipment.MinimumLevel = input.ReadInt32();
                equipment.TextureFileName = input.ReadString();

                return equipment;
            }
        }

        #endregion
    } 
}
