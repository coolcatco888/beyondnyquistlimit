using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Library;

namespace Library
{
    public class AttackInfo
    {
        private float distance;
        public float Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        private Vector2 rotation;
        public Vector2 Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        private Vector2 textureCoordinates;
        public Vector2 TextureCoordinates
        {
            get { return textureCoordinates; }
            set { textureCoordinates = value; }
        }

        private Vector2 scale;
        public Vector2 Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        private Vector2 unitScale;
        public Vector2 UnitScale
        {
            get { return unitScale; }
            set { unitScale = value; }
        }
    }
}
