#region File Description
/// <summary>
/// The Level game screen
/// 
/// <author>Alex Fontaine</summary>
/// </summary>
#endregion

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

        /// <summary>
        /// The height information of the terrain model. Used for height map collision.
        /// </summary>
        private HeightMapInfo terrainHeightMap;
        public HeightMapInfo TerrainHeightMap
        {
            get { return terrainHeightMap; }
            set { terrainHeightMap = value; }
        }

        /// <summary>
        /// The current terrain of the level
        /// </summary>
        private Terrain levelMap;
        public Terrain LevelMap
        {
            get { return levelMap; }
            set { levelMap = value; }
        }
        
        #endregion

        /// <summary>
        /// A level game screen. Holds the components needed for the game.
        /// The terrain, the height information, any players and monster collections, etc
        /// </summary>
        /// <param name="name">Name of the GameScreen</param>
        /// <param name="terrainFileName">The filename of the terrain bitmap</param>
        public Level(string name, string terrainFileName)
            : base(name)
        {
            //new TestComponent(this);

            levelMap = new Terrain(this, terrainFileName);
            terrainHeightMap = levelMap.HeightMapInfo;

            Actor actor = new Actor(this, GameEngine.Content.Load<Texture2D>("theifWalkRun"), 64, 64, 1);
        }

    }
}
