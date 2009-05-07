
#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion  // Using Statements

namespace TheGame
{
    class GroundEffect : Billboard
    {
        #region Fields

        private float rotationAngle;

        #endregion  // Fields

        #region Accessors
        #endregion  // Accessors

        #region Constructors

        public GroundEffect(GameScreen parent, SpriteInfo spriteInfo)
            : base(parent, spriteInfo)
        {
            rotationAngle = 0.0f;

            vertices[0].Position = new Vector3(1, -1, 1);
            vertices[1].Position = new Vector3(-1, -1, 1);
            vertices[2].Position = new Vector3(-1, -1, -1);
            vertices[3].Position = new Vector3(1, -1, -1);
        }

        #endregion  // Constructors

        public override void Draw(GameTime gameTime)
        {
            Camera camera = (Camera)GameEngine.Services.GetService(typeof(Camera));

            GameEngine.Graphics.RenderState.AlphaTestEnable = true;
            GameEngine.Graphics.RenderState.AlphaFunction = CompareFunction.GreaterEqual;
            GameEngine.Graphics.RenderState.ReferenceAlpha = 200;

            //GameEngine.Graphics.RenderState.SourceBlend = Blend.SourceColor;


            // Assign world, view, & projection matricies to basicEffect.
            basicEffect.World = Matrix.CreateRotationY(rotationAngle) * Matrix.CreateTranslation(position);
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
    }
}
