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
        public string EffectName;
        public string Technique;
        public int MaxParticles;
        public Texture2D Texture;
        public Color Color;
        public float Intensity;
        public float InitialSize;
        public float FinalSize;

        public PointSpriteSystemSettings(string effectName, string technique, int maxParticles, Texture2D texture, Color color, float intensity, float initialSize, float finalSize)
        {
            EffectName = effectName;
            Technique = technique;
            MaxParticles = maxParticles;
            Texture = texture;
            Color = color;
            Intensity = intensity;
            InitialSize = initialSize;
            FinalSize = finalSize;
        }

        public PointSpriteSystemSettings(string effectName, string technique, Texture2D texture)
            : this(effectName, technique, 2000, texture, Color.White, 2.0f, 0.2f, 0.2f)
        {
        }

        public PointSpriteSystemSettings(Texture2D texture)
            : this("BasicPoint", "Cartesian", 2000, texture, Color.White, 2.0f, 0.2f, 0.2f)
        {
        }
        
    }
}
