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
    class CubicBezierSystem : PointSpriteSystem
    {
        public CubicBezierSystem(GameScreen parent, PointSpriteSystemSettings settings, Vector3[] controlPoints)
            : base(parent, settings)
        {
            if (controlPoints.Length != 4)
            {
                throw new Exception("Invalid control point count of " + controlPoints.Length + ". Array must contain 4 control points");  
            }

            this.controlPoints = controlPoints;
        }

        Vector3[] controlPoints;

        protected override void SetEffect()
        {
            effect.Parameters["ControlPoint0"].SetValue(controlPoints[0]);
            effect.Parameters["ControlPoint1"].SetValue(controlPoints[1]);
            effect.Parameters["ControlPoint2"].SetValue(controlPoints[2]);
            effect.Parameters["ControlPoint3"].SetValue(controlPoints[3]);
        }
    }
}
