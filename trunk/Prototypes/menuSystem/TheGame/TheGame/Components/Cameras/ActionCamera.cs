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

        private BillboardList actorsToFollow;

        public BillboardList ActorsToFollow
        {
            get { return actorsToFollow; }
            set { actorsToFollow = value; }
        }

        //TODO: Maybe this should contain reference to Level, actorsToFollow should be stored internally?
        public ActionCamera(GameScreen parent)
            : base(parent)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float sumX = 0, sumY = 0, sumZ = 0;
            float count = 0;

            float distOfFurthestActorFromLookAt = minDistance;
            if (actorsToFollow != null)
            {
                
                foreach (Billboard actor in actorsToFollow)
                {
                    if (actor.IsDisposed)
                    {
                        continue;
                    }
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
                    if (actor.IsDisposed)
                    {
                        continue;
                    }
                    float currentDist = (actor.Position - lookAt).Length();
                    if (currentDist > distOfFurthestActorFromLookAt)
                    {
                        distOfFurthestActorFromLookAt = currentDist;
                    }

                    count++;
                }

                //Move the camera farther
                Vector3 newDirection = position - lookAt;
                newDirection.Normalize();

                //Scale direction
                newDirection = newDirection * distOfFurthestActorFromLookAt;
                newDirection = newDirection + (newDirection * minDistance);


                //Set new position
                position = lookAt + newDirection;
 
            }

        }
    }
}
