
#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion  // Using Statements

namespace TheGame
{
    class Billboard : Component, IDrawableComponent
    {
        #region Fields

        VertexDeclaration vertexDeclaration = new VertexDeclaration(
                GameEngine.Graphics, VertexPositionTexture.VertexElements);

        Texture2D texture2D;

        /// <summary>
        /// Current position of this billboard on the world map.
        /// </summary>
        protected Vector3 position;
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Scale of the billboard.
        /// </summary>
        private float scale;
        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        protected VertexPositionTexture[] vertices;

        BasicEffect basicEffect;

        #endregion  // Fields

        public Billboard(GameScreen parent, Texture2D texture2D) : base(parent)
        {
            visible = true;

            // Assign texture.
            this.texture2D = texture2D;

            scale = 1.0f;
            position = new Vector3(0.0f, 0.0f, 0.0f);

            // Setup basic effect.
            basicEffect = new BasicEffect(GameEngine.Graphics, null);
            basicEffect.Texture = texture2D;
            basicEffect.TextureEnabled = true;

            // Four vertices to represet billboard.
            vertices = new VertexPositionTexture[4];

            // Assign position.
            vertices[0].Position = new Vector3(1, 2, 0);
            vertices[1].Position = new Vector3(-1, 2, 0);
            vertices[2].Position = new Vector3(-1, 0, 0);
            vertices[3].Position = new Vector3(1, 0, 0);

            // Assign texture coordinates to vertices.
            vertices[0].TextureCoordinate = new Vector2(0, 0);
            vertices[1].TextureCoordinate = new Vector2(1, 0);
            vertices[2].TextureCoordinate = new Vector2(1, 1);
            vertices[3].TextureCoordinate = new Vector2(0, 1);
        }

        #region IDrawableComponent Members

        public void Draw(GameTime gameTime)
        {
            Camera camera = (Camera)GameEngine.Services.GetService(typeof(Camera));

            GameEngine.Graphics.RenderState.AlphaTestEnable = true;
            GameEngine.Graphics.RenderState.AlphaFunction = CompareFunction.GreaterEqual;
            GameEngine.Graphics.RenderState.ReferenceAlpha = 200;

            //GameEngine.Graphics.RenderState.SourceBlend = Blend.SourceColor;


            // Assign world, view, & projection matricies to basicEffect.
            basicEffect.World = Matrix.CreateWorld(position, -camera.LookAt, Vector3.Up);
            basicEffect.View = camera.View;
            basicEffect.Projection = camera.Projection;

            // Draw billboard.
            basicEffect.Begin();
            basicEffect.CurrentTechnique.Passes[0].Begin();

            GameEngine.Graphics.VertexDeclaration = vertexDeclaration;
            GameEngine.Graphics.DrawUserPrimitives(PrimitiveType.TriangleFan, vertices, 0, 2);

            basicEffect.CurrentTechnique.Passes[0].End();
            basicEffect.End();

            GameEngine.Graphics.RenderState.AlphaTestEnable = false;
        }

        private bool visible;
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        #endregion
    }
}
