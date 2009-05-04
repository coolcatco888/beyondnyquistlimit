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
    class Level : GameScreen
    {
        #region Fields
        public HeightMapInfo TerrainHeightMap
        {
            get
            {
                return terrainHeightMap;
            }
            set
            {
                terrainHeightMap = value;
            }
        }
        private HeightMapInfo terrainHeightMap;

        public Terrain LevelMap
        {
            get
            {
                return levelMap;
            }
            set
            {
                levelMap = value;
            }
        }
        private Terrain levelMap;

        #endregion

        public Level(string name, string terrainFileName)
            : base(name)
        {
            new TestComponent(this);

            levelMap = new Terrain(this, terrainFileName);
            terrainHeightMap = levelMap.HeightMapInfo;

            Actor actor = new Actor(this, GameEngine.Content.Load<Texture2D>("theifWalkRun"), 64, 64, 1);
            actor.Position = new Vector3(0.0f, 0.0f, -30.0f);
        }

    }
}
