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

namespace TheGame.Components
{
    class BillboardList : List<IBillboard>
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
                float yDistance = Vector3.Distance(comparePoint, x.Position);
                
                return xDistance.CompareTo(yDistance);
            }

            #endregion
        }

        #endregion
    }
}
