
#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Library;

#endregion  // Using Statements

namespace TheGame
{
    public class Billboard : Component, IDrawableComponent, IBillboard, ICollidable
    {
        #region Fields

        protected VertexDeclaration vertexDeclaration = new VertexDeclaration(
                GameEngine.Graphics, VertexPositionTexture.VertexElements);

        protected VertexPositionTexture[] vertices;

        protected BasicEffect basicEffect;

        #endregion  // Fields

        public Billboard(GameScreen parent, Texture2D texture2D)
            : base(parent)
        {
            visible = true;

            this.texture2D = texture2D;

            scale = new Vector2(1.0f, 1.0f);
            rotation = 0.0f;
            position = new Vector3(0.0f, 0.0f, 0.0f);

            // Setup basic effect.
            basicEffect = new BasicEffect(GameEngine.Graphics, null);
            basicEffect.Texture = this.texture2D;
            basicEffect.TextureEnabled = true;

            // Four vertices to represet billboard.
            vertices = new VertexPositionTexture[4];

            // Assign position.
            vertices[0].Position = new Vector3(1, 1, 0);
            vertices[1].Position = new Vector3(-1, 1, 0);
            vertices[2].Position = new Vector3(-1, -1, 0);
            vertices[3].Position = new Vector3(1, -1, 0);

            // Assign texture coordinates to vertices.
            vertices[0].TextureCoordinate = new Vector2(0, 0);
            vertices[1].TextureCoordinate = new Vector2(1, 0);
            vertices[2].TextureCoordinate = new Vector2(1, 1);
            vertices[3].TextureCoordinate = new Vector2(0, 1);
        }

        public override void Dispose()
        {
            texture2D.Dispose();
            basicEffect.Dispose();

            base.Dispose();
        }

        #region IDrawableComponent Members

        public virtual void Draw(GameTime gameTime)
        {
            Camera camera = (Camera)GameEngine.Services.GetService(typeof(Camera));

            // Assign world, view, & projection matricies to basicEffect.
            // TODO: implement rotation
            basicEffect.World = Matrix.CreateScale(scale.X, scale.Y, 1.0f) * Matrix.CreateWorld(position, -camera.Direction, Vector3.Up);
            basicEffect.View = camera.View;
            basicEffect.Projection = camera.Projection;

            // Draw billboard.
            basicEffect.Begin();
            basicEffect.CurrentTechnique.Passes[0].Begin();

            GameEngine.Graphics.VertexDeclaration = vertexDeclaration;
            GameEngine.Graphics.DrawUserPrimitives(PrimitiveType.TriangleFan, vertices, 0, 2);

            basicEffect.CurrentTechnique.Passes[0].End();
            basicEffect.End();
        }

        protected bool visible;
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        #endregion

        #region IBillboard Members

        /// <summary>
        /// Rotation of the billboard.
        /// </summary>
        protected float rotation;
        public float Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
            }
        }

        protected Texture2D texture2D;
        public Texture2D Texture2D
        {
            get
            {
                return texture2D;
            }
            set
            {
                texture2D = value;
            }
        }

        /// <summary>
        /// Current position of this billboard on the world map.
        /// </summary>
        protected Vector3 position;
        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        /// <summary>
        /// Scale of the billboard.
        /// </summary>
        protected Vector2 scale;
        public Vector2 Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
            }
        }

        #endregion

        #region ICollidable Members

        protected BoundingBox boundingBox;
        public virtual BoundingBox BoundingBox
        {
            get { return boundingBox; }
        }

        public virtual bool IsHit(BoundingBox otherBounds)
        {
            return boundingBox.Intersects(otherBounds);
        }

        protected bool collidable;
        public bool Collidable
        {
            get
            {
                return collidable;
            }
            set
            {
                collidable = value;
            }
        }

        #endregion
    }
}
