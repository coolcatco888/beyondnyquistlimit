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
        const float incrementAngle = 2.0f * MathHelper.Pi / 16.0f,
            halfInc = incrementAngle * 0.5f;

        private List<ImageComponent2D> gaugeSquares = new List<ImageComponent2D>();

        private List<float> ranges = new List<float>();

        /// <summary>
        /// Circular gauges might not be a whole circle
        /// 
        /// It may be a pie that is in the shape of pacman
        /// </summary>
        private float startAngle, endAngle, radius;

        private Color fullColor, emptyColor;

        private int currentValue, maxValue;

        private Texture2D gaugeImage;


        public int MaxValue
        {
            set { maxValue = value < 1 ? 1 : maxValue; UpdateGaugeColors();  }
            get { return maxValue; }
        }

        public int CurrentValue
        {
            set { currentValue = value < 0 ? 0 : value > maxValue ? maxValue : value; UpdateGaugeColors(); }
            get { return currentValue; }
        }

        public override void Initialize()
        {
            foreach (DisplayComponent2D component in gaugeSquares)
            {
                component.Initialize();
            }
            base.Initialize();
        }

        public CircularGauge(GameScreen parent, Vector2 position, Texture2D gaugeImage, 
            int maxValue, int currentValue, float radius, float startAngle, float endAngle, Color fullColor, Color emptyColor)
            : base(parent)
        {
            this.position = position;
            this.maxValue = maxValue;
            this.currentValue = currentValue;
            this.radius = radius;
            this.startAngle = startAngle;
            this.endAngle = endAngle;
            this.fullColor = fullColor;
            this.emptyColor = emptyColor;
            this.gaugeImage = gaugeImage;
            SetupGauge();
        }

        private void SetupGauge()
        {
            //Add Items
            Vector2 itemDirection = new Vector2(radius, 0);//This direction will rotate clockwise
            Vector2 cornerToCenter = new Vector2(gaugeImage.Width * 0.5f, gaugeImage.Height * 0.5f);
            itemDirection = Vector2.Transform(itemDirection, Quaternion.CreateFromYawPitchRoll(0, 0, startAngle));
            Quaternion rotation = Quaternion.CreateFromYawPitchRoll(0, 0, incrementAngle);

            for (float angle = startAngle; angle < endAngle; angle += incrementAngle)
            {
                itemDirection = Vector2.Transform(itemDirection, rotation);//Change direction
                cornerToCenter = Vector2.Transform(cornerToCenter, rotation);
                Vector2 currentPos = this.position + itemDirection;//Change position
                ImageComponent2D currentSquare = new ImageComponent2D(Parent, currentPos, gaugeImage, fullColor);//Create bar item
                currentSquare.Rotation = angle + incrementAngle + MathHelper.PiOver2;// +MathHelper.Pi + MathHelper.PiOver4 * 0.25f;// -MathHelper.PiOver4;//Assuming the picture is upright, initially rotate to its side
                currentSquare.IsOriginCenter = true;
                gaugeSquares.Add(currentSquare);
                ranges.Add(angle + halfInc);
            }

            //Color Items
            UpdateGaugeColors();
            

        }

        private void UpdateGaugeColors()
        {
            float gaugePercent = (float) currentValue / (float) maxValue;

            float angle = gaugePercent * (endAngle - startAngle) + startAngle;

            int i = 0;
            foreach (float range in ranges)
            {
                if (angle > range)
                {
                    gaugeSquares.ElementAt(i).Tint = fullColor;
                }
                else
                {
                    gaugeSquares.ElementAt(i).Tint = emptyColor;
                }
                i++;
            }
        }


        public void IncreaseDecreaseValue(int value)
        {
            currentValue += value;
            currentValue = currentValue > maxValue ? maxValue : currentValue < 0 ? 0 : currentValue;
            UpdateGaugeColors();
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
