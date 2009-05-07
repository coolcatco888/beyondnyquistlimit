using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame.Components.Display
{
    class GameWeaponMenuPanel2D : DisplayComponent2D
    {

        private Dictionary<string, ImageComponent2D> weapons = new Dictionary<string,ImageComponent2D>();

        private const int minLength = 40, maxScale = 10;

        private const byte unSelectedTint = 125, selectedTint = 255;

        private int curScale = 0;

        private List<Vector2> ranges = new List<Vector2>();

        private int selected = 0;

        private bool killMe = false;
        
        /// <summary>
        /// Creates a circular weapon/item menu selection.  Each item is displayed in a circular fashion.
        /// This menu grows and shrinks to accommodate for the increase and decrease of items.
        /// </summary>
        /// <param name="parent">Screen that contains this object</param>
        /// <param name="position">position of the weapon panel, should take in screen coodinates of actor.</param>
        public GameWeaponMenuPanel2D(GameScreen parent, Vector2 position)
            : base(parent)
        {
            this.position = position;
        }

        public void UpdateItemPositions()
        {
            UpdateItemPositons(maxScale);
        }

        /// <summary>
        /// Places weapon icons in weapon panel in a circular fashion.  
        /// Use scale = 10 to put them into their default positions.
        /// </summary>
        /// <param name="scale">Scales distance from centre. Range 0 - 10</param>
        private void UpdateItemPositons(int scale)
        {
            ranges.Clear();
            Vector2 itemDirection = new Vector2(0, 1) * weapons.Count * scale + new Vector2(0, minLength);

            float angle = scale * (MathHelper.TwoPi / weapons.Count) / maxScale;
            Quaternion rotation =
                Quaternion.CreateFromYawPitchRoll(0, 0, angle);
            float range1 = 0, range2 = angle;

            foreach (KeyValuePair<string, ImageComponent2D> item in weapons)
            {
                ImageComponent2D component = item.Value;
                ranges.Add(new Vector2(range1, range2));
                itemDirection = Vector2.Transform(itemDirection, rotation);
                component.Position = this.position + itemDirection;
                if (ranges.Count - 1 != selected)
                {
                    component.Tint = new Color(component.Tint, unSelectedTint);
                }
                else
                {
                    component.Tint = new Color(component.Tint, selectedTint);
                }
                range1 += angle;
                range2 += angle;
            }
        }

        /// <summary>
        /// Adds a new weapon to the weapon menu. All weapon names must be unique.
        /// </summary>
        /// <param name="name">Name of the Weapon</param>
        /// <param name="weapon">Image of the weapon</param>
        public void AddWeapon(string name, ImageComponent2D weapon)
        {
            weapons.Add(name, weapon);
            UpdateItemPositions();
        }

        /// <summary>
        /// Given the name of the weapon, remove from the weapon list.
        /// </summary>
        /// <param name="name">Name of the weapon.</param>
        public void RemoveWeapon(string name)
        {
            weapons.Remove(name);
            UpdateItemPositions();
        }

        /// <summary>
        /// Returnes the image of the weapon given its name.
        /// </summary>
        /// <param name="name">Name of the weapon</param>
        /// <returns></returns>
        public ImageComponent2D GetWeaponImage(string name)
        {
            ImageComponent2D weapon = null;
            weapons.TryGetValue(name, out weapon);
            return weapon;
        }

        /// <summary>
        /// Since the weapons are displayed in a circular fasion, pass in a normalized vector
        /// of the joystic position using a standard Cartesian Grid, with X and Y values ranging from -1 to 1.
        /// Also makes the selected weapon display fully opaque and the unselected ones semi transparent.
        /// </summary>
        /// <param name="joystickPosition">Joystic position using a standard Cartesian Grid, with X and Y values ranging from -1 to 1</param>
        /// <returns>Name of the weapon.</returns>
        public string SelectNewWeapon(Vector2 joystickPosition)
        {
            float angle = (float)Math.Atan2(joystickPosition.Y, -1 * joystickPosition.X);
            if (angle < 0)
            {
                angle += MathHelper.TwoPi;
            }
            string name = "";
            ImageComponent2D weapon = weapons.Values.ElementAt(selected);
            weapon.Tint = new Color(weapon.Tint, unSelectedTint);             
            int i = 0;
            foreach (Vector2 range in ranges)
            {
                if (angle >= range.X && angle < range.Y)
                {
                    name = weapons.Keys.ElementAt(i);
                    selected = i;
                    break;
                }
                i++;
            }
            weapon = weapons.Values.ElementAt(selected);
            weapon.Tint = new Color(weapon.Tint, selectedTint);     
            return name;
        }

        /// <summary>
        /// Gets the name of selected weapon
        /// </summary>
        /// <returns>Name of selected weapon.</returns>
        public string GetNameOfSelectedWeapon()
        {
            return weapons.Keys.ElementAt(selected);
        }

        /// <summary>
        /// Handles grow and shrink animations of the weapon menu panel.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            //Animations
            //TODO: Make it sync with game time.
            if (curScale < 10 && !killMe)
            {
                curScale++;
                UpdateItemPositons(curScale);
            }
            
            if (killMe && curScale > 0)
            {
                curScale--;
                UpdateItemPositons(curScale);
            }

            if (killMe && curScale == 0 && Parent != null)
            {
                this.Dispose();
            }
        }

        /// <summary>
        /// Shrinks and disposes menu.
        /// </summary>
        public void KillMenu()
        {
            killMe = true;
        }

        public override void Dispose()
        {
            
            foreach (KeyValuePair<string, ImageComponent2D> item in weapons)
            {
                item.Value.Dispose();
            }
            weapons.Clear();
            base.Dispose();
            
        }
    }
}
