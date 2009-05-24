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
using Library;

namespace TheGame
{
    public class ActorList : List<Actor>
    {
        public Actor GetNearest(Vector3 comparePosition)
        {
            this.Sort(new ProximityComparer(comparePosition));

            return this[0];
        }

        public List<Actor> GetNearest(int count, Vector3 comparePosition)
        {
            this.Sort(new ProximityComparer(comparePosition));

            if (count < this.Count)
            {
                return this.GetRange(0, count);
            }
            else
            {
                return this;
            }
        }

        public List<Actor> GetAll(Vector3 comparePosition)
        {
            this.Sort(new ProximityComparer(comparePosition));

            return this;
        }

        public List<Actor> GetAllWithinRange(Vector3 sourcePosition, float radius)
        {
            this.Sort(new ProximityComparer(sourcePosition));

            int index = 0;

            while (index < this.Count)
            {
                if (Vector3.Distance(sourcePosition, this[index].Position) <= radius)
                {
                    index++;
                }
                else
                {
                    break;
                }
            }

            return this.GetRange(0, index);
        }

        public List<Actor> GetAllWithinCone(Vector3 sourcePosition, Vector3 direction, float distance, float maxAngle)
        {
            this.Sort(new ProximityComparer(sourcePosition));

            List<Actor> withinRange = this.GetAllWithinRange(sourcePosition, distance);

            foreach (Actor actor in withinRange)
            {
                if (Vector3.Dot(direction, actor.Position) >= maxAngle)
                {
                    withinRange.Remove(actor);
                }
            }

            return withinRange;
        }

        #region IComparer classes

        protected class ProximityComparer : IComparer<Actor>
        {
            private Vector3 comparePoint;

            public ProximityComparer(Vector3 comparePosition)
            {
                this.comparePoint = comparePosition;
            }

            #region IComparer<Actor> Members

            public int Compare(Actor x, Actor y)
            {
                float xDistance = Vector3.Distance(comparePoint, x.Position);
                float yDistance = Vector3.Distance(comparePoint, y.Position);

                return xDistance.CompareTo(yDistance);
            }

            #endregion
        }

        #endregion
    }
}
