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
using TheGame.Components.Cameras;
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

        protected BillboardList monsters = new BillboardList();
        public BillboardList Monsters
        {
            get { return monsters; }
        }

        protected BillboardList players = new BillboardList();
        public BillboardList Players
        {
            get { return players; }
        }

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
            ActionCamera camera = (ActionCamera)GameEngine.Services.GetService(typeof(Camera));
            camera.ActorsToFollow = players;

            levelMap = new Terrain(this, terrainFileName);
            terrainHeightMap = levelMap.HeightMapInfo;

            Library.SpriteInfo spriteInfo = GameEngine.Content.Load<Library.SpriteInfo>(@"Sprites\\MagicCircle");

            //CastingAura magicCircleEffect = new MagicCircleEffect(this, spriteInfo, 0.01f, 0.01f, new Point(0, 0));
            //magicCircleEffect.Position = new Vector3(0.0f, 0.0f, -1.0f);

            //Library.SpellInfo spellInfo = new Library.SpellInfo();
            //spellInfo.Duration = 10.0f;

            spriteInfo = GameEngine.Content.Load<Library.SpriteInfo>(@"CrateXml");
            Monster crate = new Monster(this, spriteInfo.SpriteSheet, 20);
            crate.Position = new Vector3(0.0f, 0.0f, -10.0f);
            monsters.Add(crate);
            //crate = new Monster(this, texture, 10);
            //crate.Position = new Vector3(10.0f, 0.0f, -5.0f);
            //monsters.Add(crate);
            //crate = new Monster(this, texture, 10);
            //crate.Position = new Vector3(-10.0f, 0.0f, 5.0f);
            //monsters.Add(crate);

            Library.SpriteInfo playerSpriteInfo = GameEngine.Content.Load<Library.SpriteInfo>(@"Sprites\\ActorTest");
            Player playerOne = new Player(this, playerSpriteInfo, PlayerIndex.One);
            players.Add(playerOne);
            players.Add(crate);

            //FireTornado fireTornado = new FireTornado(this, spellInfo, new Vector3(0, 0, -1.0f));
            //Chanting ms = new Chanting(this, spellInfo, new Vector3(0, 0, -1.0f)); 
        }

        public override void Draw()
        {
            ActionCamera camera = (ActionCamera)GameEngine.Services.GetService(typeof(Camera));
            camera.Update(GameEngine.GameTime);
            base.Draw();
        }

        //public override void Update(GameTime gameTime)
        //{
        //    base.Update(gameTime);
        //    ActionCamera camera = (ActionCamera) GameEngine.Services.GetService(typeof(Camera));
        //    //Camera camera = (Camera)GameEngine.Services.GetService(typeof(Camera));
        //    camera.UpdateCameraPosition(players);
            
        //}

    }
}
