using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using ContentTestLibrary;

namespace ContentTestPipeline
{
    /// <summary>
    /// Contains information about the height map.
    /// Is called when building a map model with the TerrainProcessor
    /// </summary>
    public class HeightMapInfoContent
    {
        #region Fields

        /// <summary>
        /// 2D array of map heights at some x, z position
        /// </summary>
        public float[,] Heights
        {
            get
            {
                return heights;
            }
        }
        float[,] heights;

        /// <summary>
        /// Distance between height entries.
        /// </summary>
        public float TerrainScale
        {
            get
            {
                return terrainscale;
            }
        }
        private float terrainscale;
        #endregion

        public HeightMapInfoContent(PixelBitmapContent<float> heightmap,
            float terrainscale, float terrainbumpiness)
        {
            this.terrainscale = terrainscale;

            heights = new float[heightmap.Width, heightmap.Height];
            for (int y = 0; y < heightmap.Height; y++)
            {
                for (int x = 0; x < heightmap.Width; x++)
                {
                    if (heightmap.GetPixel(x, y) == 0)
                    {
                        heights[x, y] = -1.0f;
                    }
                    else
                    {
                        heights[x, y] = ((heightmap.GetPixel(x, y)) * terrainbumpiness);
                    }
                }
            }
        }
    }

    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to write the specified data type into binary .xnb format.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    /// </summary>
    [ContentTypeWriter]
    public class HeightInfoWriter : ContentTypeWriter<HeightMapInfo>
    {
        protected override void Write(ContentWriter output, HeightMapInfo value)
        {
            output.Write(value.TerrainScale);

            output.Write(value.Heights.GetLength(0));
            output.Write(value.Heights.GetLength(1));
            foreach (float height in value.Heights)
            {
                output.Write(height);
            }
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(HeightMapInfo.HeightMapInfoReader).AssemblyQualifiedName;
        }
    }
}
