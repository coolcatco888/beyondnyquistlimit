
#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion  // Using Statements


namespace TheGame
{
    class MagicCircleEffect : GroundEffect
    {
        #region Fields

        private float rotationSpeed;
        private float scaleIncrement;
        private float scale;
        private Point sheetIndex;
        private float timer = 0.0f;

        #endregion  // Fields

        #region Constructors

        public MagicCircleEffect(GameScreen parent, SpriteInfo spriteInfo, float scaleIncrement, float rotationSpeed, Point sheetIndex)
            : base(parent, spriteInfo)
        {
            this.scaleIncrement = scaleIncrement;
            this.scale = scaleIncrement;
            this.rotationSpeed = rotationSpeed;
            this.sheetIndex = sheetIndex;
        }

        public MagicCircleEffect(GameScreen parent, SpriteInfo spriteInfo, float scaleIncrement, float rotationSpeed, int sheetColumn, int sheetRow)
            : base(parent, spriteInfo)
        {
            this.scaleIncrement = scaleIncrement;
            this.scale = scaleIncrement;
            this.rotationSpeed = rotationSpeed;
            this.sheetIndex.X = sheetColumn;
            this.sheetIndex.Y = sheetRow;

            vertices[0].TextureCoordinate = new Vector2(
                sheetIndex.X * spriteInfo.SpriteUnit.X,
                sheetIndex.Y * spriteInfo.SpriteUnit.Y);

            vertices[1].TextureCoordinate = new Vector2(
                sheetIndex.X * spriteInfo.SpriteUnit.X + spriteInfo.SpriteUnit.X,
                sheetIndex.Y * spriteInfo.SpriteUnit.Y);

            vertices[2].TextureCoordinate = new Vector2(
                sheetIndex.X * spriteInfo.SpriteUnit.X + spriteInfo.SpriteUnit.X,
                sheetIndex.Y * spriteInfo.SpriteUnit.Y + spriteInfo.SpriteUnit.Y);

            vertices[3].TextureCoordinate = new Vector2(
                sheetIndex.X * spriteInfo.SpriteUnit.X,
                sheetIndex.Y * spriteInfo.SpriteUnit.Y + spriteInfo.SpriteUnit.Y);
        }

        public override void Initialize(GameScreen parent)
        {
            base.Initialize(parent);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            while (timer > 0)
            {
                vertices[0].Position.X = 1 * scale;
                vertices[0].Position.Y = -0.99f;
                vertices[0].Position.Z = 1 * scale;
                vertices[1].Position.X = -1 * scale;
                vertices[1].Position.Y = -0.99f;
                vertices[1].Position.Z = 1 * scale;
                vertices[2].Position.X = -1 * scale;
                vertices[2].Position.Y = -0.99f;
                vertices[2].Position.Z = -1 * scale;
                vertices[3].Position.X = 1 * scale;
                vertices[3].Position.Y = -0.99f;
                vertices[3].Position.Z = -1 * scale;

                rotationAngle += rotationSpeed;

                if (scale > 0 && scale < 2)
                {
                    scale += scaleIncrement;
                }
                else
                {
                    scaleIncrement = 2;
                }

                timer -= 40;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Camera camera = (Camera)GameEngine.Services.GetService(typeof(Camera));

            GameEngine.Graphics.RenderState.AlphaBlendEnable = true;
            GameEngine.Graphics.RenderState.SourceBlend = Blend.One;
            GameEngine.Graphics.RenderState.DestinationBlend = Blend.One;
            GameEngine.Graphics.RenderState.BlendFunction = BlendFunction.Add;

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

            GameEngine.Graphics.RenderState.AlphaBlendEnable = false;
        }

        #endregion  // Constructors
    }
}
