
#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Library;

#endregion  // Using Statements

namespace TheGame
{
    public class Billboard : CollidableComponent, IBillboard
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

        #region Initilization

        public Billboard(GameScreen parent, Texture2D texture2D, Vector3 position, Vector3 rotation, Vector3 scale)
            : base(parent, position, rotation, scale)
        {
            this.texture2D = texture2D;

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

        public Billboard(GameScreen parent, Texture2D texture2D, Vector3 position, Vector3 rotation)
            : this(parent, texture2D, position, rotation, Vector3.One)
        {
        }

        public Billboard(GameScreen parent, Texture2D texture2D, Vector3 position)
            : this(parent, texture2D, position, Vector3.Zero, Vector3.One)
        {
        }

        public Billboard(GameScreen parent, Texture2D texture2D)
            : this(parent, texture2D, Vector3.Zero, Vector3.Zero, Vector3.One)
        {
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
            basicEffect.World = Matrix.CreateScale(scale.X, scale.Y, 1.0f) * Matrix.CreateBillboard(position, camera.Position, Vector3.Up, camera.Direction);
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

        #endregion

        #region IBillboard Members

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

        #endregion
    }
}
