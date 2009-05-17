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

        private BillboardList playerList;
        public BillboardList PlayerList
        {
            get { return playerList; }
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
            levelMap.Initialize();

            playerList = new BillboardList();
            Library.SpriteInfo playerSpriteInfo = GameEngine.Content.Load<Library.SpriteInfo>(@"Sprites\\ActorTest");
            playerList.Add(new Player(this, playerSpriteInfo, PlayerIndex.One));
            

            //Library.SpriteInfo spriteInfo = GameEngine.Content.Load<Library.SpriteInfo>(@"Sprites\\MagicCircle");

            //MagicCircleEffect magicCircleEffect = new MagicCircleEffect(this, spriteInfo, 0.01f, 0.01f, new Point(0, 0));
            //magicCircleEffect.Position = new Vector3(0.0f, 0.0f, -1.0f);

            //FireTornado ft = new FireTornado(this, Vector3.UnitZ, 15.0f);
            //ft.Initialize();

            ChainBeam cb = new ChainBeam(this, 50.0f);
            cb.Initialize();

        }

    }
}
