﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame.Components.Display
{
    /// <summary>
    /// Represents a health or mana bar or any bar that has a game value.
    /// This is a very specific implementation of a gauge.
    /// This object should be created with a grayscale rectangular image.
    /// </summary>
    class ValueBarComponent2D : ImageComponent2D, IGauge
    {
        
        private int maxValue, currentValue;

        private Color damageColor, originalColor;

        private bool damaged = false;

        private float currentFade = 0.0f;

        private const float maxFade = 1.0f;

        private const byte maxColorVal = 255;

        /// <summary>
        /// Keeps track of the max value of the gauge and displays it accordingly
        /// </summary>
        public int MaxValue
        {
            set { maxValue = value < 1? 1 : maxValue; }
            get { return maxValue; }
        }

        /// <summary>
        /// Keeps track of the current value of the gauge and displays it accordingly
        /// </summary>
        public int CurrentValue
        {
            set { currentValue = value < 0? 0 : value > maxValue? maxValue : value; }
            get { return currentValue; }
        }

        /// <summary>
        /// The bar would briefly change to the damage flash color when value decreases i.e. 
        /// IncreaseDecreaseValue() is called with a negative value.
        /// </summary>
        public Color DamageColor
        {
            set { damageColor = value; }
            get { return damageColor; }
        }

        /// <summary>
        /// Creates a value bar gauge
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        /// <param name="image"></param>
        /// <param name="tint"></param>
        /// <param name="maxValue"></param>
        /// <param name="damageColor"></param>
        public ValueBarComponent2D(GameScreen parent, Vector2 position, Texture2D image, Color tint, int maxValue, Color damageColor) 
            : base(parent, position, image, tint)
        {
            this.maxValue = maxValue < 1? 1 : maxValue;
            this.currentValue = this.maxValue;
            this.scale = new Vector2(1.0f, 1.0f);
            this.damageColor = damageColor;
            this.originalColor = tint;
        }

        public void IncreaseDecreaseValue(int value)
        {
            damaged = value < 0;
            currentValue += value;
            currentValue = currentValue > maxValue ? maxValue : currentValue < 0 ? 0 : currentValue;
            this.scale.X = (float) currentValue / (float) maxValue;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float originalColorRatio = originalColorRatio = maxFade - currentFade;
            if (damageColor != null && damaged && currentFade < 1.0f)
            {
                currentFade += 0.10000f;
                tint = CreateFadedColor(originalColorRatio);
            }
            else if (damageColor != null && !damaged
                && currentFade <= 1.001f && currentFade > 0.0f)
            {
                currentFade -= 0.10000f;
                tint = CreateFadedColor(originalColorRatio);
            }
            else
            {
                damaged = false;
            }
        }

        private Color CreateFadedColor(float originalColorRatio)
        {
            float r = (((float)originalColor.R / (float)maxColorVal) * originalColorRatio) +
                (((float)damageColor.R / (float)maxColorVal) * currentFade);
            float g = (((float)originalColor.G / (float)maxColorVal) * originalColorRatio) +
                (((float)damageColor.G / (float)maxColorVal) * currentFade);
            float b = (((float)originalColor.B / (float)maxColorVal) * originalColorRatio) +
                (((float)damageColor.B / (float)maxColorVal) * currentFade);
            return new Color(r, g, b, 1.0f);
        }
    }
}
