using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TheGame.Components.Cameras
{
    class ActionCamera : Camera
    {
        private float minDistance = 4.5f, minHeight = 10.0f, maxHeight = 12.0f;

        private BillboardList actorsToFollow;

        /// <summary>
        /// Closest Distance the camera can be to the actors
        /// </summary>
        public float MinDistance
        {
            get { return minDistance; }
            set { minDistance = value; }
        }

        /// <summary>
        /// Lowest the camera can be
        /// </summary>
        public float MinHeight
        {
            get { return minHeight; }
            set { minHeight = value; }
        }

        /// <summary>
        /// Highest the camera can be
        /// </summary>
        public float MaxHeight
        {
            get { return maxHeight; }
            set { maxHeight = value; }
        }


        /// <summary>
        /// A group of actors the camera needs to capture
        /// </summary>
        public BillboardList ActorsToFollow
        {
            get { return actorsToFollow; }
            set { actorsToFollow = value; }
        }

        /// <summary>
        /// Creates an action camera with standard settings
        /// </summary>
        /// <param name="parent">Screen the camera is contained in</param>
        public ActionCamera(GameScreen parent)
            : base(parent)
        {
        }

        /// <summary>
        /// Creates an action camera with variable minimum follow distance, minimum height, maximum height, and actors to follow
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="minDistance"></param>
        /// <param name="minHeight"></param>
        /// <param name="maxHeight"></param>
        /// <param name="actorsToFollow"></param>
        public ActionCamera(GameScreen parent, float minDistance, float minHeight, float maxHeight, BillboardList actorsToFollow)
            : base(parent)
        {
            this.minDistance = minDistance;
            this.minHeight = minHeight;
            this.maxHeight = maxHeight;
            this.actorsToFollow = actorsToFollow;
        }

        public override void Update(GameTime gameTime)
        {
            float sumX = 0, sumY = 0, sumZ = 0, count = 0;

            float distOfFurthestActorFromLookAt = minDistance;
            if (actorsToFollow != null)
            {
                //Sum all the positions
                foreach (Billboard actor in actorsToFollow)
                {
                    if (actor.IsDisposed)
                        continue;

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
                distOfFurthestActorFromLookAt = CalculateCameraZoomDistance(distOfFurthestActorFromLookAt);
                ChangeCameraPosition(distOfFurthestActorFromLookAt);

                base.Update(gameTime);
 
            }

        }

        private float CalculateCameraZoomDistance(float distOfFurthestActorFromLookAt)
        {
            foreach (Billboard actor in actorsToFollow)
            {
                if (actor.IsDisposed)
                    continue;

                float currentDist = (actor.Position - lookAt).Length() * 0.40f;
                if (currentDist > distOfFurthestActorFromLookAt)
                {
                    distOfFurthestActorFromLookAt = currentDist;
                }
            }
            return distOfFurthestActorFromLookAt;
        }

        private void ChangeCameraPosition(float distOfFurthestActorFromLookAt)
        {
            //Move the camera farther
            Vector3 newDirection = position - lookAt;
            newDirection.Normalize();

            //Scale direction
            newDirection = newDirection * distOfFurthestActorFromLookAt;
            newDirection = newDirection + (newDirection * minDistance);
            newDirection.Y = newDirection.Y > minHeight ? newDirection.Y < maxHeight ? newDirection.Y : maxHeight : minHeight;

            //Set new position
            position = lookAt + newDirection;

            //Push camera a bit to the side if the player is directly under the camera
            if (Math.Floor(position.X) == Math.Floor(lookAt.X) && Math.Floor(position.Z) == Math.Floor(lookAt.Z))
            {
                position.X = position.X + 1.0f;
            }
        }
    }
}
