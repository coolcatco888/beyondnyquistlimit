using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Library
{
    public class BoundingShapeInfo
    {
        private string stateKey;
        public string StateKey
        {
            get { return stateKey; }
            set { stateKey = value; }
        }

        private string orientationKey;
        public string OrientationKey
        {
            get { return orientationKey; }
            set { orientationKey = value; }
        }

        private List<Vector3> vertices;
        public List<Vector3> Vertices
        {
            get { return vertices; }
            set { vertices = value; }
        }

        [ContentSerializerIgnore]
        private Vector3[] verts;
        [ContentSerializerIgnore]
        public Vector3[] Verts
        {
            get { return verts; }
        }

        public void FillVectexArray()
        {
            verts = new Vector3[vertices.Count];
            for (int i = 0; i < vertices.Count; i++)
            {
                verts[i] = vertices[i];
            }
        }

        #region Content Reader

        public class BoundingShapeInfoReader : ContentTypeReader<BoundingShapeInfo>
        {
            protected override BoundingShapeInfo Read(ContentReader input, BoundingShapeInfo existingInstance)
            {
                BoundingShapeInfo shapeInfo = new BoundingShapeInfo();

                shapeInfo.stateKey = input.ReadString();
                shapeInfo.orientationKey = input.ReadString();

                shapeInfo.vertices = input.ReadRawObject<List<Vector3>>();

                shapeInfo.FillVectexArray();

                return shapeInfo;
            }
        }

        #endregion
    }
}
