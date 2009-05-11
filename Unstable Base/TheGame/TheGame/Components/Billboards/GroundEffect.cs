
#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Library;

#endregion  // Using Statements

namespace TheGame
{
    class GroundEffect : Billboard
    {
        #region Fields

        #endregion  // Fields

        #region Accessors
        #endregion  // Accessors

        #region Constructors

        public GroundEffect(GameScreen parent, SpriteInfo spriteInfo)
            : base(parent, spriteInfo.SpriteSheet)
        {
            this.Initialize();
        }

        #endregion  // Constructors

        public override void Draw(GameTime gameTime)
        {
            Camera camera = (Camera)GameEngine.Services.GetService(typeof(Camera));

            // Assign world, view, & projection matricies to basicEffect.
            basicEffect.World = Matrix.CreateScale(scale) * Matrix.CreateRotationX(MathHelper.PiOver2) * Matrix.CreateRotationY(rotation) * Matrix.CreateTranslation(position);
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

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
