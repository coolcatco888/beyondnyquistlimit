using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGame.Components.Display;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame.Components.GUI
{
    class CircularGauge : DisplayComponent2D, IGauge
    {
        private List<ImageComponent2D> gaugeSquares = new List<ImageComponent2D>();

        /// <summary>
        /// Circular gauges might not be a whole circle
        /// </summary>
        private float startAngle, endAngle;

        private Color fullColor, emptyColor;

        private int currentValue, maxValue;


        public int MaxValue
        {
            set { maxValue = value < 1 ? 1 : maxValue; }
            get { return maxValue; }
        }

        public int CurrentValue
        {
            set { currentValue = value < 0 ? 0 : value > maxValue ? maxValue : value; }
            get { return currentValue; }
        }

        public CircularGauge(GameScreen parent, Vector2 position, Texture2D gaugeImage, 
            int maxValue, int currentValue, float startAngle, float endAngle, Color fullColor, Color emptyColor)
            : base(parent)
        {
            this.position = position;
            this.maxValue = maxValue;
            this.currentValue = currentValue;
            this.startAngle = startAngle;
            this.endAngle = endAngle;
            this.fullColor = fullColor;
            this.emptyColor = emptyColor;
            SetupGauge();
        }

        private void SetupGauge()
        {

        }


        public void IncreaseDecreaseValue(int value)
        {
            currentValue += value;
            currentValue = currentValue > maxValue ? maxValue : currentValue < 0 ? 0 : currentValue;
        }


        public override Vector2 Center
        {
            get { throw new NotImplementedException(); }
        }

        public override float Height
        {
            get { throw new NotImplementedException(); }
        }

        public override float Width
        {
            get { throw new NotImplementedException(); }
        }

        public override float Left
        {
            get { throw new NotImplementedException(); }
        }

        public override float Right
        {
            get { throw new NotImplementedException(); }
        }

        public override float Bottom
        {
            get { throw new NotImplementedException(); }
        }

        public override float Top
        {
            get { throw new NotImplementedException(); }
        }
    }
}
