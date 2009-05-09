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

        private Color damageColor, originalColor;

        private Vector2 horizontalScale;

        private bool damaged = false;

        private float currentFade = 0.1f;

        private const float maxFade = 2.0f;

        public int MaxValue
        {
            set { maxValue = value < 1? 1 : maxValue; }
            get { return maxValue; }
        }

        public int CurrentValue
        {
            set { currentValue = value < 0? 0 : value > maxValue? maxValue : value; }
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
            this.maxValue = maxValue < 1? 1 : maxValue;
            this.currentValue = this.maxValue;
            horizontalScale = new Vector2(1.0f, 1.0f);
            this.damageColor = damageColor;
            this.originalColor = tint;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(image, position, null, tint, 0.0f, Vector2.Zero, horizontalScale, SpriteEffects.None, 0.0f);
            spriteBatch.End();
        }

        public void IncreaseDecreaseValue(int value)
        {
            damaged = value < 0;
            currentValue += value;
            currentValue = currentValue > maxValue ? maxValue : currentValue < 0 ? 0 : currentValue;
            horizontalScale.X = (float) currentValue / (float) maxValue;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //TODO: Add Damage Color Flash
            if (damageColor != null && damaged)
            {
                if (currentFade < maxFade)
                {
                    float originalColorRatio;
                    if(currentFade < 10)
                    {
                        currentFade += 0.1f;
                        originalColorRatio = maxFade - currentFade;
                        //tint = new Color(                        
                    }
                    
                }
            }
        }
        
    }
}
