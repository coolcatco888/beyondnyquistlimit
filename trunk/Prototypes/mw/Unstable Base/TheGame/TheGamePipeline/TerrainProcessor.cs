#region File Description
/// <summary>
/// Processor used to turn a bitmap heightmap into a 3D terrain model
/// 
/// <author>Alex Fontaine</author>
/// </summary>
#endregion

#region Using Statements
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
#endregion

namespace TheGamePipeline
{
    /// <summary>
    /// Custom content processor for creating terrain meshes. Given an
    /// input heightfield texture, this processor uses the MeshBuilder
    /// class to programatically generate terrain geometry.
    /// </summary>
    [ContentProcessor]
    public class TerrainProcessor : ContentProcessor<Texture2DContent, ModelContent>
    {
        /// <summary>
        /// The scale of the terrain.  Determines the size of the terrain field.  Represents
        /// the distance from one vertex to another. E.g. a value of 10 means 10 units from one vertex
        /// to the next
        /// </summary>
        private const float terrainScale = 1.0f;

        /// <summary>
        /// The bumpiness factor. Represents how high the terrain will go.  Actuall pixel values will
        /// fall between (0 - black and 1 - white) where black will be the "ground" level.  To determine the height
        /// multiply by the bumpiness factor.  E.g. a value of 10 means the actual range is 0 - 10 for heights
        /// </summary>
        private const float terrainBumpiness = 10;

        /// <summary>
        /// The texture coordinate scale.  Used when drawing the material for the model. Determines the size of the
        /// texture to use.
        /// </summary>
        private const float texCoordScale = 0.05f;

        /// <summary>
        /// The default texture for the terrain.  To overwrite the texture may need to physically add it in
        /// the draw method of the model. More testing needed for this.
        /// </summary>
        private const string terrainTexture = "MountainGrass.png";


        /// <summary>
        /// Generates a terrain mesh from an input height map bitmap
        /// </summary>
        public override ModelContent Process(Texture2DContent input,
                                             ContentProcessorContext context)
        {
            MeshBuilder builder = MeshBuilder.StartMesh(input.Name);

            // Convert the input texture to float format, for ease of processing.
            input.ConvertBitmapType(typeof(PixelBitmapContent<float>));

            PixelBitmapContent<float> heightfield;
            heightfield = (PixelBitmapContent<float>)input.Mipmaps[0];

            // Create the terrain vertices.
            for (int y = 0; y < heightfield.Height; y++)
            {
                for (int x = 0; x < heightfield.Width; x++)
                {
                    Vector3 position;

                    position.X = (x - heightfield.Width / 2) * terrainScale;
                    position.Z = (y - heightfield.Height / 2) * terrainScale;

                    //if(heightfield.GetPixel(x, y) == 0)
                    //{
                    //    position.Y = 0.0f;
                    //}
                    //else
                    //{
                    position.Y = ((heightfield.GetPixel(x, y)) * terrainBumpiness);
                    //}
                    

                    builder.CreatePosition(position);
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
                    AddVertex(builder, texCoordId, heightfield.Width, x, y);
                    AddVertex(builder, texCoordId, heightfield.Width, x + 1, y);
                    AddVertex(builder, texCoordId, heightfield.Width, x + 1, y + 1);

                    AddVertex(builder, texCoordId, heightfield.Width, x, y);
                    AddVertex(builder, texCoordId, heightfield.Width, x + 1, y + 1);
                    AddVertex(builder, texCoordId, heightfield.Width, x, y + 1);
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

