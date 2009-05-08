using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace ContentTestLibrary
{
    public class Weapon : Equipment
    {
        #region Weapon Info

        private int damageValue;
        public int DamageValue
        {
            get { return damageValue; }
            set { damageValue = value; }
        }
        
        #endregion

        #region Content Reader

        public class WeaponContentReader : ContentTypeReader<Weapon>
        {
            protected override Weapon Read(ContentReader input, Weapon existingInstance)
            {
                Weapon weapon = existingInstance;
                if (weapon == null)
                {
                    weapon = new Weapon();
                }

                input.ReadRawObject<Equipment>(weapon as Equipment);

                weapon.DamageValue = input.ReadInt32();

                return weapon;
            }
        }

        #endregion
    }
}
