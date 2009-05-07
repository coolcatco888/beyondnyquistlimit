using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame.Components.Display
{
    class ValueBarComponent2D : ImageComponent2D, IGauge
    {
        private int maxValue, currentValue;

        private Color damageColor;

        private Vector2 horizontalScale;

        public int MaxValue
        {
            set { maxValue = value; }
            get { return maxValue; }
        }

        public int CurrentValue
        {
            set { currentValue = value; }
            get { return currentValue; }
        }

        public Color DamageColor
        {
            set { damageColor = value; }
            get { return damageColor; }
        }

        public ValueBarComponent2D(GameScreen parent, Vector2 position, Texture2D image, Color tint, int maxValue, Color damageColor) 
            : base(parent, position, image, tint)
        {
            this.maxValue = maxValue;
            this.currentValue = this.maxValue;
            horizontalScale = new Vector2(1.0f, 1.0f);
            this.damageColor = damageColor;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(image, position, null, tint, 0.0f, Vector2.Zero, horizontalScale, SpriteEffects.None, 0.0f);
            spriteBatch.End();
        }

        public void IncreaseDecreaseValue(int value)
        {
            currentValue += value;
            currentValue = currentValue > maxValue ? maxValue : currentValue < 0 ? 0 : currentValue;
            horizontalScale = new Vector2(0.0f, currentValue / maxValue);
        }
        
    }
}
