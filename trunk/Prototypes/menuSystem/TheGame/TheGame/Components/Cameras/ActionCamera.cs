using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TheGame.Components.Cameras
{
    class ActionCamera : Camera
    {
        private float minDistance = 5;

        //TODO: Maybe this should contain reference to Level, actorsToFollow should be stored internally?
        public ActionCamera(GameScreen parent)
            : base(parent)
        {
        }


        //TODO: This maybe should be in update?
        public void UpdateCameraPosition(List<Billboard> actorsToFollow)
        {
            float sumX = 0, sumY = 0, sumZ = 0;
            float count = 0;

            float maxDist = 0;

            foreach (Billboard actor in actorsToFollow)
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

            foreach (Billboard actor in actorsToFollow)
            {
                float currentDist = (actor.Position - lookAt).Length();
                if (currentDist > maxDist)
                {
                    maxDist = currentDist;
                }

                count++;
            }

            //Move the camera farther
            Vector3 newDirection = new Vector3(-Direction.X, -Direction.Y, -Direction.Z);
            newDirection.Normalize();
            newDirection = newDirection * maxDist;
            newDirection = new Vector3(newDirection.X + minDistance, newDirection.Y + minDistance, newDirection.Z + minDistance);

            position = (lookAt + (newDirection * 2.0f)) * 1.02f;


        }
    }
}
