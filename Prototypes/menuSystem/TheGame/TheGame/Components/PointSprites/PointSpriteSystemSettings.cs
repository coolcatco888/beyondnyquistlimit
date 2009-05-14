using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace TheGame
{
    public class PointSpriteSystemSettings
    {
        public string Technique = "Cartesian";
        public int MaxPointCount = 2000;
        public Texture2D SpriteTexture = null;

        public Vector3 Position = Vector3.Zero;
        public Quaternion Rotation = Quaternion.Identity;
        public float Scale = 1.0f;

        public Color Color = Color.White;
        public float PointSpriteSize = 0.5f;
        public float ParticleDuration = 1.0f;
    }
}
