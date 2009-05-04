#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework;
#endregion

namespace TheGamePipeline
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
    /// A typewriter for the height info.  Needs to have a matching
    /// reader.
    /// </summary>
    [ContentTypeWriter]
    public class HeightMapInfoWriter : ContentTypeWriter<HeightMapInfoContent>
    {
        protected override void Write(ContentWriter output, HeightMapInfoContent value)
        {
            output.Write(value.TerrainScale);

            output.Write(value.Heights.GetLength(0));
            output.Write(value.Heights.GetLength(1));
            foreach (float height in value.Heights)
            {
                output.Write(height);
            }
        }

        /// <summary>
        /// Tells pipeline what CLR type the data will be loaded
        /// into at runtime
        /// </summary>
        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return "TheGame.HeightMapInfo, " +
                "TheGame, Version=1.0.0.0, Culture=neutral";
        }

        /// <summary>
        /// Tells the pipeline what type of reader to use
        /// </summary>
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "TheGame.HeightMapInfoReader, " +
                "TheGame, Version=1.0.0.0, Culture=neutral";
        }
    }
}
