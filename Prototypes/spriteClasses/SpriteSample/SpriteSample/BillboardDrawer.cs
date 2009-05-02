
#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion  // Using Statements

namespace SpriteSample
{
    class BillboardDrawer
    {
        #region Fields

        GraphicsDevice graphicsDevice;
        VertexDeclaration vertexDeclaration;
        BasicEffect basicEffect;
        VertexPositionTexture[] vertices;

        #endregion  // Fields

        /// <summary>
        /// Construct the Billboard Drawer.
        /// </summary>
        /// <param name="device">Graphics Device for drawing.</param>
        public BillboardDrawer(GraphicsDevice device)
        {
            graphicsDevice = device;

            vertexDeclaration = new VertexDeclaration(
                device, VertexPositionTexture.VertexElements);

            basicEffect = new BasicEffect(device, null);

            // Four vertices to represet billboard.
            vertices = new VertexPositionTexture[4];

            vertices[0].Position = new Vector3(-1, 1, 0);
            vertices[1].Position = new Vector3(1, 1, 0);
            vertices[2].Position = new Vector3(1, -1, 0);
            vertices[3].Position = new Vector3(-1, -1, 0);
        }

        public void DrawBillboard(Texture2D texture, float textureRepeatCount,
            Matrix world, Matrix view, Matrix projection)
        {
            // Assign texture of basicEffect.
            basicEffect.Texture = texture;
            basicEffect.TextureEnabled = true;

            // Assign world, view, & projection matricies to basicEffect.
            basicEffect.World = world;
            basicEffect.View = view;
            basicEffect.Projection = projection;
            
            // Assign texture coordinates to vertices based on number of texture repeats.
            vertices[0].TextureCoordinate = new Vector2(0, 0);
            vertices[1].TextureCoordinate = new Vector2(textureRepeatCount, 0);
            vertices[2].TextureCoordinate = new Vector2(textureRepeatCount, textureRepeatCount);
            vertices[3].TextureCoordinate = new Vector2(0, textureRepeatCount);

            // Draw billboard.
            basicEffect.Begin();
            basicEffect.CurrentTechnique.Passes[0].Begin();

            graphicsDevice.VertexDeclaration = vertexDeclaration;
            graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleFan, vertices, 0, 2);

            basicEffect.CurrentTechnique.Passes[0].End();
            basicEffect.End();
        }
    }
}
