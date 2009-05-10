using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace TheGamePipeline
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to apply custom processing to content data, converting an object of
    /// type TInput to TOutput. The input and output types may be the same if
    /// the processor wishes to alter data without changing its type.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    ///
    /// TODO: change the ContentProcessor attribute to specify the correct
    /// display name for this processor.
    /// </summary>
    [ContentProcessor(DisplayName = "SkyGardenProcessor")]
    public class SkyGardenProcessor : ContentProcessor<Texture2DContent, ModelContent>
    {
        const float terrainScale = 1.0f;
        const float terrainBumpiness = 10;
        const float texCoordScale = 0.1f;
        const string terrainTexture = "rocks.bmp";

        public override ModelContent Process(Texture2DContent input, ContentProcessorContext context)
        {
            MeshBuilder builder = MeshBuilder.StartMesh(input.Name);

            // Convert the input texture to float format, for ease of processing.
            input.ConvertBitmapType(typeof(PixelBitmapContent<float>));

            PixelBitmapContent<float> heightfield;
            heightfield = (PixelBitmapContent<float>)input.Mipmaps[0];
            float[,] heights = new float[heightfield.Width, heightfield.Height];

            // Create the terrain vertices.
            for (int y = 0; y < heightfield.Height; y++)
            {
                for (int x = 0; x < heightfield.Width; x++)
                {
                    Vector3 position;

                    position.X = (x - heightfield.Width / 2) * terrainScale;
                    position.Z = (y - heightfield.Height / 2) * terrainScale;

                    if (heightfield.GetPixel(x, y) == 0)
                    {
                        position.Y = -1.0f;
                        heights[x, y] = -1.0f;
                        builder.CreatePosition(position);
                    }
                    else
                    {
                        position.Y = -2.0f;
                        heights[x, y] = 0.0f;
                        builder.CreatePosition(position);
                    }
                }
            }

            // Create a material, and point it at our terrain texture.
            BasicMaterialContent material = new BasicMaterialContent();

            string directory = Path.GetDirectoryName(input.Identity.SourceFilename);
            string texture = Path.Combine(directory, terrainTexture);

            material.Texture = new ExternalReference<TextureContent>(texture);

            builder.SetMaterial(material);

            // Create a vertex channel for holding texture coordinates.
            int texCoordId = builder.CreateVertexChannel<Vector2>(
                                            VertexChannelNames.TextureCoordinate(0));

            // Create the individual triangles that make up our terrain.
            for (int y = 0; y < heightfield.Height - 1; y++)
            {
                for (int x = 0; x < heightfield.Width - 1; x++)
                {
                    if (heights[x, y] == -1.0f)
                    {
                        AddVertex(builder, texCoordId, heightfield.Width, x, y);
                        AddVertex(builder, texCoordId, heightfield.Width, x + 1, y);
                        AddVertex(builder, texCoordId, heightfield.Width, x + 1, y + 1);

                        AddVertex(builder, texCoordId, heightfield.Width, x, y);
                        AddVertex(builder, texCoordId, heightfield.Width, x + 1, y + 1);
                        AddVertex(builder, texCoordId, heightfield.Width, x, y + 1);
                    }
                }
            }

            // Chain to the ModelProcessor so it can convert the mesh we just generated.
            MeshContent terrainMesh = builder.FinishMesh();

            ModelContent model = context.Convert<MeshContent, ModelContent>(terrainMesh,
                                                              "ModelProcessor");
            model.Tag = new HeightMapInfoContent(heightfield, terrainScale, terrainBumpiness);

            //return context.Convert<MeshContent, ModelContent>(terrainMesh, "ModelProcessor");

            return model;
        }

        /// <summary>
        /// Helper for adding a new triangle vertex to a MeshBuilder,
        /// along with an associated texture coordinate value.
        /// </summary>
        static void AddVertex(MeshBuilder builder, int texCoordId, int w, int x, int y)
        {
            builder.SetVertexChannelData(texCoordId, new Vector2(x, y) * texCoordScale);

            builder.AddTriangleVertex(x + y * w);
        }
    }
}