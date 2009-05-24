﻿
#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Library;

#endregion  // Using Statements

namespace TheGame
{
    public class GroundEffect : Billboard
    {
        #region Fields

        #endregion  // Fields

        #region Accessors
        #endregion  // Accessors

        #region Constructors

        public GroundEffect(GameScreen parent, SpriteInfo spriteInfo)
            : base(parent, spriteInfo.SpriteSheet)
        {
        }

        #endregion  // Constructors

        public override void Draw(GameTime gameTime)
        {
            Camera camera = (Camera)GameEngine.Services.GetService(typeof(Camera));

            // Assign world, view, & projection matricies to basicEffect.
            effect.Parameters["Color"].SetValue(Color.ToVector4());
            effect.Parameters["World"].SetValue(Matrix.CreateScale(scale.X, scale.Y, 1.0f) * Matrix.CreateRotationX(-MathHelper.PiOver2) * Matrix.CreateFromQuaternion(rotation) * Matrix.CreateTranslation(position));
            effect.Parameters["View"].SetValue(camera.View);
            effect.Parameters["Projection"].SetValue(camera.Projection);

            GameEngine.Graphics.RenderState.AlphaBlendEnable = true;
            GameEngine.Graphics.RenderState.SourceBlend = Blend.One;
            GameEngine.Graphics.RenderState.DestinationBlend = Blend.One;
            GameEngine.Graphics.RenderState.BlendFunction = BlendFunction.Add;

            // Draw billboard.
            effect.Begin();
            effect.CurrentTechnique.Passes[0].Begin();

            GameEngine.Graphics.VertexDeclaration = vertexDeclaration;
            GameEngine.Graphics.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices, 0, 2);

            effect.CurrentTechnique.Passes[0].End();
            effect.End();

            GameEngine.Graphics.RenderState.AlphaBlendEnable = false;
            GameEngine.Graphics.RenderState.AlphaTestEnable = false;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
