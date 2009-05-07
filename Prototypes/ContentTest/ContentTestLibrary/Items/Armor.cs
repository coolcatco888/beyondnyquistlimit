using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace ContentTestLibrary
{
    public class Armor : Equipment
    {
        #region Armor Info
        public enum Slot
        {
            Head, Body, Arms, Legs, Boots, Hands
        };

        private Slot armorSlot;
        public Slot ArmorSlot
        {
            get { return armorSlot; }
            set { armorSlot = value; }
        }

        private string ability;
        public string Ability
        {
            get { return ability; }
            set { ability = value; }
        }

        private int defenseValue;
        public int DefenseValue
        {
            get { return defenseValue; }
            set { defenseValue = value; }
        }

        #endregion

        #region Content Reader
        public class ArmorContentReader : ContentTypeReader<Armor>
        {
            protected override Armor Read(ContentReader input, Armor existingInstance)
            {
                Armor armor = existingInstance;
                if (armor == null)
                {
                    armor = new Armor();
                }

                input.ReadRawObject<Equipment>(armor as Equipment);

                armor.ArmorSlot = (Slot)input.ReadInt32();
                armor.Ability = input.ReadString();
                armor.DefenseValue = input.ReadInt32();

                return armor;
            }
        }
        #endregion
    }
}
