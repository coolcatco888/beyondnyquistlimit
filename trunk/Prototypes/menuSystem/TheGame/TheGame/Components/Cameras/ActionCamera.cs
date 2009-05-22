using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TheGame.Components.Cameras
{
    class ActionCamera : Camera
    {
        private float minDistance, minHeight, maxHeight;

        private ActorList actorsToFollow;

        private float distancePerUpdate, zoomConstant;

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
        public ActorList ActorsToFollow
        {
            get { return actorsToFollow; }
            set { actorsToFollow = value; }
        }

        public ActionCamera(GameScreen parent, float minDistance, float minHeight, float maxHeight, float distancePerUpdate, float zoomConstant, ActorList actorsToFollow)
            : base(parent)
        {
            this.minDistance = minDistance;
            this.minHeight = minHeight;
            this.maxHeight = maxHeight;
            this.distancePerUpdate = distancePerUpdate;
            this.zoomConstant = zoomConstant;
            this.actorsToFollow = actorsToFollow;
        }

        public ActionCamera(GameScreen parent, float minDistance, float minHeight, float maxHeight, ActorList actorsToFollow)
            : this(parent, minDistance, minHeight, maxHeight, 0.0025f, 0.4f, actorsToFollow)
        {
        }

        public ActionCamera(GameScreen parent, ActorList actorsToFollow)
            : this(parent, 4.5f, 10.0f, 12.0f, 0.0025f, 0.4f, actorsToFollow)
        {
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
                    Vector3 newLookAt = new Vector3(sumX / count, sumY / count, sumZ / count);
                    newLookAt -= lookAt;
                    float length = newLookAt.Length();
                    float velocity = length * distancePerUpdate;
                    newLookAt.Normalize();
                    newLookAt *= velocity * gameTime.ElapsedGameTime.Milliseconds;
                    lookAt += newLookAt;
                }
                distOfFurthestActorFromLookAt = CalculateCameraZoomDistance(distOfFurthestActorFromLookAt);
                ChangeCameraPosition(distOfFurthestActorFromLookAt, gameTime);

                base.Update(gameTime);

            }


        }

        private float CalculateCameraZoomDistance(float distOfFurthestActorFromLookAt)
        {
            foreach (Billboard actor in actorsToFollow)
            {
                if (actor.IsDisposed)
                    continue;

                float currentDist = (actor.Position - lookAt).Length() * zoomConstant;
                if (currentDist > distOfFurthestActorFromLookAt)
                {
                    distOfFurthestActorFromLookAt = currentDist;
                }
            }
            return distOfFurthestActorFromLookAt;
        }

        private void ChangeCameraPosition(float distOfFurthestActorFromLookAt, GameTime gameTime)
        {
            //Move the camera farther
            Vector3 newDirection = position - lookAt;
            newDirection.Normalize();

            //Scale direction
            newDirection = newDirection * distOfFurthestActorFromLookAt;
            newDirection = newDirection + (newDirection * minDistance);
            newDirection.Y = newDirection.Y > minHeight ? newDirection.Y < maxHeight ? newDirection.Y : maxHeight : minHeight;

            //Set new position
            Vector3 newPosition = lookAt + newDirection;

            newPosition -= position;
            float length = newPosition.Length();
            float velocity = length * distancePerUpdate;
            newPosition.Normalize();
            newPosition *= velocity * gameTime.ElapsedGameTime.Milliseconds;
            position += newPosition;


            //Push camera a bit to the side if the player is directly under the camera
            if (Math.Floor(position.X) == Math.Floor(lookAt.X) && Math.Floor(position.Z) == Math.Floor(lookAt.Z))
            {
                position.X = position.X + 1.0f;
            }
        }
    }
}
