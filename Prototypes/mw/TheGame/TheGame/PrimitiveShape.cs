
#region Using Statements

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion // Using Statements

namespace TheGame
{
    public class PrimitiveShape
    {
        #region Fields
        // Color of the shape when drawing
        public Color ShapeColor = Color.White;

        // Vertices in 2D screen coordinates of the bounding shape
        Vector2[] vertices;

        // Vertex coordinates in 3D used for the bounding shape
        Vector3[] vertices3D;

        // Transformed 2D vertices
        Vector2[] transformedVertices;

        // Custom rectangle bounding class calculated from the bounds and vertices
        BoundingRectangle bounds;

        // Position in 2D screen coordinates
        Vector2 position = Vector2.Zero;

        // Position of the owning object in 3D space (i.e. the objects Zero position)
        Vector3 position3D = Vector3.Zero;

        // Scale of the object's texture coordinates
        Vector2 scale = Vector2.Zero;

        // Rotation of the shape
        float rotation = 0f;

        #endregion // Fields

        #region Accessors

        // Returns the position in screen coordinates of the bounding shape
        public Vector2 Position
        {
            get { return position; }
            set
            {
                if (!position.Equals(value))
                {
                    position = value;
                    CalculatePointsAndBounds();
                }
            }
        }

        // Returns how much the shape has been rotated by
        public float Rotation
        {
            get { return rotation; }
            set
            {
                if (rotation != value)
                {
                    rotation = value;
                    CalculatePointsAndBounds();
                }
            }
        }

        // Returns the BoundingRectangle calculated from the vertices.
        // Used in simple collision tests
        public BoundingRectangle Bounds
        {
            get { return bounds; }
        }

        #endregion // Accessors

        #region Constructors

        /// <summary>
        /// The "default" constructor. Takes an array of 2D screen vertices to draw the shape. It then
        /// calculates the bounds and points to test in collision detection
        /// </summary>
        /// <param name="vertices">Array of 2D screen vertices</param>
        public PrimitiveShape(params Vector2[] vertices)
        {
            this.vertices = (Vector2[])vertices.Clone();
            this.transformedVertices = new Vector2[vertices.Length];
            CalculatePointsAndBounds();
        }

        /// <summary>
        /// Constructs the bounding shape from an array of 3D vertices. We pass in 
        /// the objects position and scale too, so that all calculations can remain in the
        /// shape
        /// </summary>
        /// <param name="position">The position of the object in 3D space</param>
        /// <param name="scale">The scale of the objects texture coordinates</param>
        /// <param name="vertices3">The array of 3D world vertices</param>
        public PrimitiveShape(Vector3 position, Vector2 scale, params Vector3[] vertices3)
        {
            this.vertices3D = (Vector3[])vertices3.Clone();
            this.position3D = position;
            this.scale = scale;
            this.vertices = new Vector2[vertices3.Length];
            InitializeVertices2D();

        }

        #endregion // Constructors

        #region Initialization

        /// <summary>
        /// Initializes the 2D vertices if constructing the shape from 3D vertex points
        /// </summary>
        public void InitializeVertices2D()
        {
            Viewport viewport = GameEngine.Graphics.Viewport;
            Camera camera = (Camera)GameEngine.Services.GetService(typeof(Camera));

            for (int i = 0; i < vertices3D.Length; i++)
            {
                Vector3 temp = viewport.Project(vertices3D[i], camera.Projection, camera.View, Matrix.CreateScale(new Vector3(scale.X, scale.Y, 1.0f)) *
                    Matrix.CreateWorld(Vector3.Zero, camera.Position - position3D, Vector3.Up) * Matrix.CreateTranslation(position3D));
                vertices[i] = new Vector2(temp.X, temp.Y);
            }
            this.transformedVertices = new Vector2[vertices.Length];
            if (!this.IsOffScreen())
                CalculatePointsAndBounds();
        }

        #endregion // Initialization

        #region Update

        /// <summary>
        /// Updates the vertices of the bounding shape when the position of the object is changed.
        /// Can also be used for "future" checking (i.e. checking the bounding box in an advanced position
        /// from the object to detect collisions before hand.
        /// </summary>
        /// <param name="newPosition"></param>
        public void Update(Vector3 newPosition)
        {
            this.position3D = newPosition;

            // This is really "reinitializing" the 2D vertices.  We have to recalculate them
            // due to the position changing in the 3D world.  This will reflect in the bounding boxes
            // growing/shrinking due to perspective.
            InitializeVertices2D();
        }

        #endregion // Update

        public bool IsOffScreen()
        {
            bool isOffscreen = false;
            foreach (Vector2 v in vertices)
            {
                if (v.X < 0 || v.Y < 0)
                    isOffscreen = true;
            }
            return isOffscreen;
        }

        #region Draw

        /// <summary>
        /// Draws the bounding shape. Used to test collision detection, and to see
        /// what the shape actually looks like
        /// </summary>
        /// <param name="batch">Similar to SpriteBatch, helps in drawing the shape</param>
        public void Draw(PrimitiveBatch batch)
        {
            batch.Begin(PrimitiveType.LineList);
            for (int i = 0; i < transformedVertices.Length; i++)
            {
                batch.AddVertex(transformedVertices[i], ShapeColor);
                batch.AddVertex(transformedVertices[(i + 1) % transformedVertices.Length], ShapeColor);
            }
            batch.End();
        }

        #endregion // Draw

        #region Primitive Shape Methods

        /// <summary>
        /// Calculates the bounding rectangle and transforms the vertices with a 2D rotation and position if any
        /// </summary>
        private void CalculatePointsAndBounds()
        {
            for (int i = 0; i < vertices.Length; i++)
                transformedVertices[i] = Vector2.Transform(vertices[i], Matrix.CreateRotationZ(rotation)) + position;

            bounds = new BoundingRectangle(transformedVertices);
        }

        /* using the algorithm written by Darel Rex Finley at
         * http://alienryderflex.com/polygon/
         * 
         * we take a given point and draw a horizontal line, counting the
         * intersections with sides of the polygon. if the number of intersections
         * is odd, the point is inside the polygon.
         */
        public bool ContainsPoint(Vector2 point)
        {
            if (!bounds.Contains(point))
                return false;

            bool oddNodes = false;

            int j = vertices.Length - 1;
            float x = point.X;
            float y = point.Y;

            for (int i = 0; i < transformedVertices.Length; i++)
            {
                Vector2 tpi = transformedVertices[i];
                Vector2 tpj = transformedVertices[j];

                if (tpi.Y < y && tpj.Y >= y || tpj.Y < y && tpi.Y >= y)
                    if (tpi.X + (y - tpi.Y) / (tpj.Y - tpi.Y) * (tpj.X - tpi.X) < x)
                        oddNodes = !oddNodes;

                j = i;
            }

            return oddNodes;
        }

        public static bool TestCollision(PrimitiveShape shape1, PrimitiveShape shape2)
        {
            if (shape1.Bounds.Intersects(shape2.Bounds))
            {
                //simple check if the first polygon contains any points from the second
                for (int i = 0; i < shape2.transformedVertices.Length; i++)
                    if (shape1.ContainsPoint(shape2.transformedVertices[i]))
                        return true;

                //switch around and test the other way
                for (int i = 0; i < shape1.transformedVertices.Length; i++)
                    if (shape2.ContainsPoint(shape1.transformedVertices[i]))
                        return true;

                //now we have to check for line segment intersections
                for (int i = 0; i < shape1.transformedVertices.Length; i++)
                {
                    //get the two points from a segment on shape 1
                    Vector2 a = shape1.transformedVertices[i];
                    Vector2 b = shape1.transformedVertices[(i + 1) % shape1.transformedVertices.Length];

                    for (int j = 0; j < shape2.transformedVertices.Length; j++)
                    {
                        //get two points from a segment on shape 2
                        Vector2 c = shape2.transformedVertices[j];
                        Vector2 d = shape2.transformedVertices[(j + 1) % shape2.transformedVertices.Length];

                        //figure out of we have an intersection
                        if (segmentsIntersect(a, b, c, d))
                            return true;
                    }
                }
            }

            return false;
        }

        //thanks to Joseph Duchesne for this method
        //http://www.idevgames.com/forum/showthread.php?t=7458
        private static bool segmentsIntersect(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            float[,] tarray = new float[4, 2];  //<===== Find the inner bounding of rect ABCD

            float ax = a.X;
            float ay = a.Y;
            float bx = b.X;
            float by = b.Y;
            float cx = c.X;
            float cy = c.Y;
            float dx = d.X;
            float dy = d.Y;

            if (ax < bx)
            {
                tarray[0, 0] = ax;
                tarray[1, 0] = bx;
            }
            else
            {
                tarray[0, 0] = bx;
                tarray[1, 0] = ax;
            }
            if (ay < by)
            {
                tarray[0, 1] = ay;
                tarray[1, 1] = by;
            }
            else
            {
                tarray[0, 1] = by;
                tarray[1, 1] = ay;
            }
            if (cx < dx)
            {
                tarray[2, 0] = cx;
                tarray[3, 0] = dx;
            }
            else
            {
                tarray[2, 0] = dx;
                tarray[3, 0] = cx;
            }
            if (cy < dy)
            {
                tarray[2, 1] = cy;
                tarray[3, 1] = dy;
            }
            else
            {
                tarray[2, 1] = dy;
                tarray[3, 1] = cy;
            }

            float[,] tarray2 = new float[2, 2];

            if (tarray[0, 0] < tarray[2, 0])
                tarray2[0, 0] = tarray[2, 0];
            else
                tarray2[0, 0] = tarray[0, 0];
            if (tarray[0, 1] < tarray[2, 1])
                tarray2[0, 1] = tarray[2, 1];
            else
                tarray2[0, 1] = tarray[0, 1];
            if (tarray[1, 0] < tarray[3, 0])
                tarray2[1, 0] = tarray[1, 0];
            else
                tarray2[1, 0] = tarray[3, 0];
            if (tarray[1, 1] < tarray[3, 1])
                tarray2[1, 1] = tarray[1, 1];
            else
                tarray2[1, 1] = tarray[3, 1];

            float mab = (ay - by) / (ax - bx); //<===== Find Slopes of Lines
            float mcd = (cy - dy) / (cx - dx);

            if (mab == mcd)
                return false;  //the lines are parallel

            float yiab = ((ax - bx) * ay - ax * (ay - by)) / (ax - bx);
            float yicd = ((cx - dx) * cy - cx * (cy - dy)) / (cx - dx);
            float x = (yicd - yiab) / (mab - mcd);
            float y = mab * x + yiab;

            return (
                x > tarray2[0, 0] &&
                x < tarray2[1, 0] &&
                y > tarray2[0, 1] &&
                y < tarray2[1, 1]);
        }

        #endregion // Primitive Shape Methods
    }
}
