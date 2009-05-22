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
    public class BillboardList : List<IBillboard>
    {
        public IBillboard GetNearest(Vector3 comparePosition)
        {
            this.Sort(new ProximityComparer(comparePosition));

            return this[0];
        }

        public List<IBillboard> GetNearest(int count, Vector3 comparePosition)
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

        public List<IBillboard> GetAll(Vector3 comparePosition)
        {
            this.Sort(new ProximityComparer(comparePosition));

            return this;
        }

        public List<IBillboard> GetAllWithinRange(Vector3 sourcePosition, float radius)
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

        public List<IBillboard> GetAllWithinCone(Vector3 sourcePosition, Vector3 direction, float distance, float maxAngle)
        {
            this.Sort(new ProximityComparer(sourcePosition));

            List<IBillboard> withinRange = this.GetAllWithinRange(sourcePosition, distance);
            
            foreach (Billboard billboard in withinRange)
            {
                if (Vector3.Dot(direction, billboard.Position) >= maxAngle)
                {
                    withinRange.Remove(billboard);
                }
            }

            return withinRange;
        }

        #region IComparer classes

        protected class ProximityComparer : IComparer<IBillboard>
        {
            private Vector3 comparePoint;

            public ProximityComparer(Vector3 comparePosition)
            {
                this.comparePoint = comparePosition;
            }

            #region IComparer<IBillboard> Members

            public int Compare(IBillboard x, IBillboard y)
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
