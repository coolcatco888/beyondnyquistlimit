using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ContentTestLibrary
{
    public class TerrainInfo
    {
        #region Fields

        private float terrainScale;
        public float TerrainScale
        {
            get { return terrainScale; }
            set { terrainScale = value; }
        }

        private float terrainBumpiness;
        public float TerrainBumpiness
        {
            get { return terrainBumpiness; }
            set { terrainBumpiness = value; }
        }

        private float texCoordScale;
        public float TexCoordScale
        {
            get { return texCoordScale; }
            set { texCoordScale = value; }
        }

        private string terrainFileName;
        public string TerrainFileName
        {
            get { return terrainFileName; }
            set { terrainFileName = value; }
        }

        private string terrainTexture;
        public string TerrainTexture
        {
            get { return terrainTexture; }
            set { terrainTexture = value; }
        }

        #endregion

        #region Content Reader

        public class TerrainInfoReader : ContentTypeReader<TerrainInfo>
        {
            protected override TerrainInfo Read(ContentReader input, TerrainInfo existingInstance)
            {
                TerrainInfo terrainInfo = existingInstance;
                if (terrainInfo == null)
                {
                    terrainInfo = new TerrainInfo();
                }

                terrainInfo.terrainScale = input.ReadSingle();
                terrainInfo.terrainBumpiness = input.ReadSingle();
                terrainInfo.texCoordScale = input.ReadSingle();
                terrainInfo.terrainFileName = input.ReadString();
                terrainInfo.terrainTexture = input.ReadString();

                return terrainInfo;
            }
        }

        #endregion
    }
}
