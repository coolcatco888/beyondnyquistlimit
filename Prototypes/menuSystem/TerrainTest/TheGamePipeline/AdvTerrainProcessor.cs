#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
#endregion

namespace TheGamePipeline
{
    /// <summary>
    /// This is a custom content processor for generating terrain meshes
    /// It takes a height map bitmap to generate the mesh.
    /// </summary>
    [ContentProcessor]
    class AdvTerrainProcessor : ContentProcessor<Texture2DContent, ModelContent>
    {
        #region Fields

        public float TerrainScale
        {
            get
            {
                return terrainscale;
            }
            set
            {
                terrainscale = value;
            }
        }
        private float terrainscale = 30f;

        public float TerrainBumpiness
        {
            get
            {
                return terrainbumpiness;
            }
            set
            {
                terrainbumpiness = value;
            }
        }
        private float terrainbumpiness = 500f;

        public float TextureCoordScale
        {
            get
            {
                return texturecoordscale;
            }
            set
            {
                texturecoordscale = value;
            }
        }
        private float texturecoordscale = 0.05f;

        public string TerrainTextureFile
        {
            get
            {
                return terraintexturefile;
            }
            set
            {
                terraintexturefile = value;
            }
        }
        private string terraintexturefile = "rocks.bmp";

        private float terrainSpecularValue = 0.8f;
        #endregion

        public override ModelContent Process(Texture2DContent input, ContentProcessorContext context)
        {
            MeshBuilder builder = MeshBuilder.StartMesh(input.Name);

            // Convert input texture to floats
            input.ConvertBitmapType(typeof(PixelBitmapContent<float>));

            PixelBitmapContent<float> heightmap;
            heightmap = (PixelBitmapContent<float>)input.Mipmaps[0];

            // Create the terrain vertices using the fields set above
            for (int y = 0; y < heightmap.Height; y++)
            {
                for (int x = 0; x < heightmap.Width; x++)
                {
                    Vector3 terrainVertexPosition;
                    // Generate scaled vertex position in xz plane and is centered at x = 0, z = 0;
                    terrainVertexPosition.X = terrainscale * (x - ((heightmap.Width - 1) / 2.0f));
                    terrainVertexPosition.Z = terrainscale * (y - ((heightmap.Height - 1) / 2.0f));

                    // Generate "height" of vertex and scaled by the bumpiness factor
                    terrainVertexPosition.Y = (heightmap.GetPixel(x, y) - 1) * terrainbumpiness;

                    builder.CreatePosition(terrainVertexPosition);
                }
            }

            // Create the material add it to the terrain texture
            BasicMaterialContent material = new BasicMaterialContent();
            material.SpecularColor = new Vector3(0.4f, 0.4f, 0.4f);

            string currentDirectory = Path.GetDirectoryName(input.Identity.SourceFilename);
            string textureFileName = Path.Combine(currentDirectory, terraintexturefile);

            material.Texture = new ExternalReference<TextureContent>(textureFileName);

            builder.SetMaterial(material);

            // Create vertex channel for texture coordinates
            int textureCoordId = builder.CreateVertexChannel<Vector2>(
                VertexChannelNames.TextureCoordinate(0));

            // Create terrain triangles
            for (int y = 0; y < heightmap.Height - 1; y++)
            {
                for (int x = 0; x < heightmap.Width - 1; x++)
                {
                    AddVertex(builder, textureCoordId, heightmap.Width, x, y);
                    AddVertex(builder, textureCoordId, heightmap.Width, x + 1, y);
                    AddVertex(builder, textureCoordId, heightmap.Width, x + 1, y + 1);

                    AddVertex(builder, textureCoordId, heightmap.Width, x, y);
                    AddVertex(builder, textureCoordId, heightmap.Width, x + 1, y + 1);
                    AddVertex(builder, textureCoordId, heightmap.Width, x, y + 1);
                }
            }

            MeshContent terrainMesh = builder.FinishMesh();

            ModelContent model = context.Convert<MeshContent, ModelContent>(terrainMesh, "ModelProcessor");

            //model.Tag = new HeightMapInfoContent(heightmap, terrainscale, terrainbumpiness);

            return model;
        }

        void AddVertex(MeshBuilder builder, int textureCoordId, int w, int x, int y)
        {
            builder.SetVertexChannelData(textureCoordId, new Vector2(x, y) * texturecoordscale);

            builder.AddTriangleVertex(x + y * w);
        }
    }
}
