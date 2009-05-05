using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TheGame.Components.Display
{
    class GameWeaponMenuPanel2D : PanelComponent2D
    {

        private const int minLength = 50;


        public GameWeaponMenuPanel2D(GameScreen parent, Vector2 position)
            : base(parent, position)
        {
        }

        public void ConvertPositionsToCircularPositions()
        {
            Vector2 itemDirection = new Vector2(0, 1) * panelItems.Count * 10 + new Vector2(0, minLength);

            float angle = (2 * (float)Math.PI) / panelItems.Count;
            Quaternion rotation =
                Quaternion.CreateFromYawPitchRoll(0, 0, angle);

            foreach (DisplayComponent2D component in panelItems)
            {
                itemDirection = Vector2.Transform(itemDirection, rotation);
                component.Position = this.position + itemDirection;
            }
        }
    }
}
