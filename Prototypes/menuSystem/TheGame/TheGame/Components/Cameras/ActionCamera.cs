using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TheGame.Components.Cameras
{
    class ActionCamera : Camera
    {
        public ActionCamera(GameScreen parent)
            : base(parent)
        {
        }

        public void UpdateCameraPosition(List<Actor> actorsToFollow)
        {
            float sumX = 0, sumY = 0, sumZ = 0;
            float count = 0;

            foreach (Actor actor in actorsToFollow)
            {
                sumX += actor.Position.X;
                sumY += actor.Position.Y;
                sumZ += actor.Position.Z;
                count++;
            }

            //Change the lookAtPosition to be the average position of all of the actors
            if (count > 0)
            {
                lookAt = new Vector3(sumX / count, sumY / count, sumZ / count);
            }

            //Move the camera farther


        }
    }
}
