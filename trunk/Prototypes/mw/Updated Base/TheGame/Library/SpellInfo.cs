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

namespace Library
{
    public enum Element { None, Fire, Water, Wind, Earth, Shadow, Holy, Heart };
    public enum HitType { None, Self, Targets, Radius, Cone }

    public class SpellInfo
    {
        public Element Element;
        public HitType HitType;

        public float Duration;

        public float Radius;
        public float Theta;
        public float Phi;

        public int Damage;
        public float TickFrequency;
    }
}
