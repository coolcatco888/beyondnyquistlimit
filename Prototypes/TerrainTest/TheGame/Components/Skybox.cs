#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
#endregion


namespace TheGame
{
    class Skybox : Component, IDrawableComponent
    {
        #region Fields

        string fileName;
        Texture2D[] textures;
        Effect effect;

        VertexBuffer vertices;
        IndexBuffer indices;
        VertexDeclaration vertexDecl;

        #endregion

        #region IDrawableComponent Members

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (vertices == null)
                return;

            Camera camera = (Camera)GameEngine.Services.GetService(typeof(Camera));

            effect.Begin();
            effect.Parameters["worldViewProjection"].SetValue(Matrix.Identity * camera.View * camera.Projection);

            for (int x = 0; x < 5; x++)
            {
                float f = 0;
                switch (x)
                {
                    case 0: //back
                        f = Vector3.Dot(camera.Direction, Vector3.Forward);
                        break;
                    case 1: //front
                        f = Vector3.Dot(camera.Direction, Vector3.Backward);
                        break;
                    case 2: //top
                        f = Vector3.Dot(camera.Direction, Vector3.Down);
                        break;
                    case 3: //left
                        f = Vector3.Dot(camera.Direction, Vector3.Right);
                        break;
                    case 4: //right
                        f = Vector3.Dot(camera.Direction, Vector3.Left);
                        break;

                }

                //if (f >= 0)
                //{
                IGraphicsDeviceService graphicsService = (IGraphicsDeviceService)
                    GameEngine.Services.GetService(typeof(IGraphicsDeviceService));

                GraphicsDevice device = graphicsService.GraphicsDevice;
                device.VertexDeclaration = vertexDecl;
                device.Vertices[0].SetSource(vertices, 0,
                    vertexDecl.GetVertexStrideSize(0));

                device.Indices = indices;

                effect.Parameters["baseTexture"].SetValue(textures[x]);
                effect.Techniques[0].Passes[0].Begin();

                device.DrawIndexedPrimitives(PrimitiveType.TriangleList,
                    0, x * 4, 4, x * 6, 2);
                effect.Techniques[0].Passes[0].End();

                //}
            }

            effect.End();
        }

        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
            }
        }
        public bool visible;

        #endregion

        #region Component Members

        public override void Initialize(GameScreen parent)
        {
            base.Initialize(parent);
        }

        #endregion

        public Skybox(GameScreen parent, string fileName)
            : base(parent)
        {
            visible = true;
            this.fileName = fileName;

            textures = new Texture2D[5];

            // next portion will go into Initialize once it is fixed...
            textures[0] = GameEngine.Content.Load<Texture2D>("Skybox\\" + fileName + "\\" + fileName + "back");
            textures[1] = GameEngine.Content.Load<Texture2D>("Skybox\\" + fileName + "\\" + fileName + "front");
            textures[2] = GameEngine.Content.Load<Texture2D>("Skybox\\" + fileName + "\\" + fileName + "top");
            textures[3] = GameEngine.Content.Load<Texture2D>("Skybox\\" + fileName + "\\" + fileName + "left");
            textures[4] = GameEngine.Content.Load<Texture2D>("Skybox\\" + fileName + "\\" + fileName + "right");

            effect = GameEngine.Content.Load<Effect>("Skybox\\skybox");

            IGraphicsDeviceService graphicsService =
                (IGraphicsDeviceService)GameEngine.Services.GetService(typeof(IGraphicsDeviceService));

            vertexDecl = new VertexDeclaration(graphicsService.GraphicsDevice,
                new VertexElement[] {
                    new VertexElement(0,0,VertexElementFormat.Vector3,
                           VertexElementMethod.Default,
                            VertexElementUsage.Position,0),
                    new VertexElement(0,sizeof(float)*3,VertexElementFormat.Vector2,
                           VertexElementMethod.Default,
                            VertexElementUsage.TextureCoordinate,0)});

            vertices = new VertexBuffer(graphicsService.GraphicsDevice,
                                typeof(VertexPositionTexture),
                                4 * 5,
                                BufferUsage.WriteOnly);

            VertexPositionTexture[] data = new VertexPositionTexture[4 * 5];

            Vector3 vExtents = new Vector3(200, 200, 200);
            //back
            data[0].Position = new Vector3(vExtents.X, -vExtents.Y, -vExtents.Z);
            data[0].TextureCoordinate.X = 1.0f; data[0].TextureCoordinate.Y = 1.0f;
            data[1].Position = new Vector3(vExtents.X, vExtents.Y, -vExtents.Z);
            data[1].TextureCoordinate.X = 1.0f; data[1].TextureCoordinate.Y = 0.0f;
            data[2].Position = new Vector3(-vExtents.X, vExtents.Y, -vExtents.Z);
            data[2].TextureCoordinate.X = 0.0f; data[2].TextureCoordinate.Y = 0.0f;
            data[3].Position = new Vector3(-vExtents.X, -vExtents.Y, -vExtents.Z);
            data[3].TextureCoordinate.X = 0.0f; data[3].TextureCoordinate.Y = 1.0f;

            //front
            data[4].Position = new Vector3(-vExtents.X, -vExtents.Y, vExtents.Z);
            data[4].TextureCoordinate.X = 1.0f; data[4].TextureCoordinate.Y = 1.0f;
            data[5].Position = new Vector3(-vExtents.X, vExtents.Y, vExtents.Z);
            data[5].TextureCoordinate.X = 1.0f; data[5].TextureCoordinate.Y = 0.0f;
            data[6].Position = new Vector3(vExtents.X, vExtents.Y, vExtents.Z);
            data[6].TextureCoordinate.X = 0.0f; data[6].TextureCoordinate.Y = 0.0f;
            data[7].Position = new Vector3(vExtents.X, -vExtents.Y, vExtents.Z);
            data[7].TextureCoordinate.X = 0.0f; data[7].TextureCoordinate.Y = 1.0f;

            //top
            data[8].Position = new Vector3(vExtents.X, vExtents.Y, -vExtents.Z);
            data[8].TextureCoordinate.X = 0.0f; data[8].TextureCoordinate.Y = 0.0f;
            data[9].Position = new Vector3(vExtents.X, vExtents.Y, vExtents.Z);
            data[9].TextureCoordinate.X = 0.0f; data[9].TextureCoordinate.Y = 1.0f;
            data[10].Position = new Vector3(-vExtents.X, vExtents.Y, vExtents.Z);
            data[10].TextureCoordinate.X = 1.0f; data[10].TextureCoordinate.Y = 1.0f;
            data[11].Position = new Vector3(-vExtents.X, vExtents.Y, -vExtents.Z);
            data[11].TextureCoordinate.X = 1.0f; data[11].TextureCoordinate.Y = 0.0f;


            //left
            data[12].Position = new Vector3(-vExtents.X, vExtents.Y, -vExtents.Z);
            data[12].TextureCoordinate.X = 1.0f; data[12].TextureCoordinate.Y = 0.0f;
            data[13].Position = new Vector3(-vExtents.X, vExtents.Y, vExtents.Z);
            data[13].TextureCoordinate.X = 0.0f; data[13].TextureCoordinate.Y = 0.0f;
            data[14].Position = new Vector3(-vExtents.X, -vExtents.Y, vExtents.Z);
            data[14].TextureCoordinate.X = 0.0f; data[14].TextureCoordinate.Y = 1.0f;
            data[15].Position = new Vector3(-vExtents.X, -vExtents.Y, -vExtents.Z);
            data[15].TextureCoordinate.X = 1.0f; data[15].TextureCoordinate.Y = 1.0f;

            //right
            data[16].Position = new Vector3(vExtents.X, -vExtents.Y, -vExtents.Z);
            data[16].TextureCoordinate.X = 0.0f; data[16].TextureCoordinate.Y = 1.0f;
            data[17].Position = new Vector3(vExtents.X, -vExtents.Y, vExtents.Z);
            data[17].TextureCoordinate.X = 1.0f; data[17].TextureCoordinate.Y = 1.0f;
            data[18].Position = new Vector3(vExtents.X, vExtents.Y, vExtents.Z);
            data[18].TextureCoordinate.X = 1.0f; data[18].TextureCoordinate.Y = 0.0f;
            data[19].Position = new Vector3(vExtents.X, vExtents.Y, -vExtents.Z);
            data[19].TextureCoordinate.X = 0.0f; data[19].TextureCoordinate.Y = 0.0f;

            vertices.SetData<VertexPositionTexture>(data);

            indices = new IndexBuffer(graphicsService.GraphicsDevice,
                                typeof(short), 6 * 5,
                                BufferUsage.WriteOnly);

            short[] ib = new short[6 * 5];

            for (int x = 0; x < 5; x++)
            {
                ib[x * 6 + 0] = (short)(x * 4 + 0);
                ib[x * 6 + 2] = (short)(x * 4 + 1);
                ib[x * 6 + 1] = (short)(x * 4 + 2);

                ib[x * 6 + 3] = (short)(x * 4 + 2);
                ib[x * 6 + 5] = (short)(x * 4 + 3);
                ib[x * 6 + 4] = (short)(x * 4 + 0);
            }

            indices.SetData<short>(ib);
        }

        public override void Dispose()
        {
            foreach (Texture2D texture in textures)
            {
                texture.Dispose();
            }
            effect.Dispose();
            vertices.Dispose();
            indices.Dispose();
            vertexDecl.Dispose();
            base.Dispose();
        }

    }
}
