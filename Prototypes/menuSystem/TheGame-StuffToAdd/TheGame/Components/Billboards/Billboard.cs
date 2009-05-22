
#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Library;

#endregion  // Using Statements

namespace TheGame
{
    public class Billboard : Component, IDrawableComponent, IBillboard, ICollidable
    {
        #region Drawing Billboard Fields

        // Vertex Declaration for Billboard texture drawing
        protected VertexDeclaration vertexDeclaration = new VertexDeclaration(
                GameEngine.Graphics, VertexPositionTexture.VertexElements);

        // Vertices that hold a position and texture coordinates
        protected VertexPositionTexture[] vertices;

        // Used to draw the billboard texture/image
        protected BasicEffect basicEffect;

        // TESTING PURPOSES ONLY - will remove later
        // Used to draw the bounding shape used in collision detection
        protected PrimitiveBatch primitiveBatch;
        // End Testing

        protected bool isDisposed = false;

        #endregion  // Fields

        #region Object Type

        // What type this billboard is.  Used by AI
        protected ObjectType type;
        public ObjectType Type
        {
            get { return type; }
            set { type = value; }
        }

        #endregion // Object Type

        #region Accessors

        public VertexPositionTexture[] Vertices
        {
            get { return vertices; }
        }

        public bool IsDisposed
        {
            get { return isDisposed; }
        }

        #endregion  // Accessors

        #region Constructor

        public Billboard(GameScreen parent, Texture2D texture2D)
            : base(parent)
        {
            visible = true;

            this.texture2D = texture2D;

            // Default values for scale, rotation and position
            scale = new Vector2(1.0f, 1.0f);
            rotation = 0.0f;
            position = new Vector3(0.0f, 0.0f, 0.0f);

            // TESTING PURPOSE ONLY - will remove later
            // Draws the bounding shapes
            primitiveBatch = new PrimitiveBatch(GameEngine.Graphics);
            // End Testing

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

        #endregion

        #region Component Overriden Members

        public override void Dispose()
        {
            primitiveBatch.Dispose();
            texture2D.Dispose();
            basicEffect.Dispose();

            base.Dispose();

            isDisposed = true;
        }

        #endregion // Component Overriden Members

        #region Update Method - Update Vertices

        public void UpdateVertices(SpriteSequence spriteSequence, Library.SpriteInfo spriteInfo)
        {
            scale.X = spriteSequence.Scale.X;

            vertices[0].TextureCoordinate = new Vector2(
                spriteSequence.CurrentFrameColumn * spriteInfo.SpriteUnit.X,
                spriteSequence.CurrentFrameRow * spriteInfo.SpriteUnit.Y);

            vertices[1].TextureCoordinate = new Vector2(
                spriteSequence.CurrentFrameColumn * spriteInfo.SpriteUnit.X + spriteInfo.SpriteUnit.X * spriteSequence.Scale.X,
                spriteSequence.CurrentFrameRow * spriteInfo.SpriteUnit.Y);

            vertices[2].TextureCoordinate = new Vector2(
                spriteSequence.CurrentFrameColumn * spriteInfo.SpriteUnit.X + spriteInfo.SpriteUnit.X * spriteSequence.Scale.X,
                spriteSequence.CurrentFrameRow * spriteInfo.SpriteUnit.Y + spriteInfo.SpriteUnit.Y * spriteSequence.Scale.Y);

            vertices[3].TextureCoordinate = new Vector2(
                spriteSequence.CurrentFrameColumn * spriteInfo.SpriteUnit.X,
                spriteSequence.CurrentFrameRow * spriteInfo.SpriteUnit.Y + spriteInfo.SpriteUnit.Y * spriteSequence.Scale.Y);
        }

        #endregion Update Method

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

        /// <summary>
        /// Whether the billboard is collidable or not
        /// </summary>
        protected bool collidable;
        public bool Collidable
        {
            get { return collidable; }
            set { collidable = value; }
        }

        /// <summary>
        /// The billboards bounding shape used in collision detection
        /// </summary>
        protected PrimitiveShape primitiveShape;
        public virtual PrimitiveShape PrimitiveShape
        {
            get { return primitiveShape; }
        }

        /// <summary>
        /// Determines if there is a hit between objects using their bounding shapes
        /// </summary>
        /// <param name="otherShape">The other objects bounding shape</param>
        /// <returns>True if a hit, false otherwise</returns>
        public virtual bool IsHit(PrimitiveShape otherShape)
        {
            return PrimitiveShape.TestCollision(primitiveShape, otherShape);
        }

        #endregion // ICollidable Members
    }
}
