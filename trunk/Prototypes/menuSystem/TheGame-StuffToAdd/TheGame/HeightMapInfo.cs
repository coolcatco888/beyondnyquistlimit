#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
#endregion

namespace TheGame
{
    public class HeightMapInfo
    {
        #region Fields
        
        // Distance between 
        private float terrainScale;

        //private float terrainBumpiness;

        // 2D array of map heights
        private float[,] heights;

        // position of the bottom left most corner of map
        private Vector3 heightmapPosition;

        // width of the heightmap (including scale)
        private float heightmapWidth;

        // height of the heightmap (including scale)
        private float heightmapHeight;

        #endregion

        public HeightMapInfo(float[,] heights, float terrainScale)
        {
            if (heights == null)
            {
                throw new ArgumentNullException("heights fail");
            }

            this.terrainScale = terrainScale;
            this.heights = heights;

            // calculate map dimensions
            heightmapWidth = (heights.GetLength(0) - 1) * terrainScale;
            heightmapHeight = (heights.GetLength(1) - 1) * terrainScale;

            // calculate position of left bottom corner of map
            heightmapPosition.X = -(heights.GetLength(0) - 1) / 2 * terrainScale;
            heightmapPosition.Z = -(heights.GetLength(1) - 1) / 2 * terrainScale;
        }

        // Takes a position and determines if it is even on the map
        public bool IsOnHeightMap(Vector3 position)
        {
            Vector3 positionOnMap = position - heightmapPosition;

            return (positionOnMap.X > 0 &&
                positionOnMap.X < heightmapWidth &&
                positionOnMap.Z > 0 &&
                positionOnMap.Z < heightmapHeight);
        }

        // Gets the height of the map at the position
        public float GetHeight(Vector3 position)
        {
            // the position of the object on the bitmap
            Vector3 positionOnMap = position - heightmapPosition;

            // find the position of the 
            int left, top;
            left = (int)positionOnMap.X / (int)terrainScale;
            top = (int)positionOnMap.Z / (int)terrainScale;

            //float xNormalized = (positionOnMap.X % terrainScale) / terrainScale;
            //float zNormalized = (positionOnMap.Z % terrainScale) / terrainScale;

            //float topHeight = MathHelper.Lerp(
            //    heights[left, top],
            //    heights[left + 1, top],
            //    xNormalized);

            //float bottomHeight = MathHelper.Lerp(
            //    heights[left, top + 1],
            //    heights[left + 1, top + 1],
            //    xNormalized);

            //return MathHelper.Lerp(topHeight, bottomHeight, zNormalized);
            float height = heights[left, top];
            return heights[left, top];
        }
    }


    internal class HeightMapInfoReader : ContentTypeReader<HeightMapInfo>
    {
        protected override HeightMapInfo Read(ContentReader input, HeightMapInfo existingInstance)
        {
            float terrainScale = input.ReadSingle();
            int width = input.ReadInt32();
            int height = input.ReadInt32();
            float[,] heights = new float[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    heights[x, z] = input.ReadSingle();
                }
            }

            return new HeightMapInfo(heights, terrainScale);
        }
    }
}
