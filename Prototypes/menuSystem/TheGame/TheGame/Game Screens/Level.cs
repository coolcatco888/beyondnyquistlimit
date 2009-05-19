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
using TheGame.Components.GUI;
using TheGame.Game_Screens;
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

        private BillboardList monsterList;
        public BillboardList MonsterList
        {
            get { return monsterList; }
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
            playerList.Add(new Player(this, playerSpriteInfo, PlayerIndex.One, "Wizard"));

            monsterList = new BillboardList();
            Library.SpriteInfo monsterSpriteInfo = GameEngine.Content.Load<Library.SpriteInfo>(@"PoringXml");
            Monster poring = new Monster(this, monsterSpriteInfo);
            poring.Position = new Vector3(-4.0f, 0.0f, -10.0f);
            monsterList.Add(poring);
            monsterList.Add(new Monster(this, monsterSpriteInfo)
            {
                Position = new Vector3(-4.0f, 0.0f, -12.0f)
            });
            monsterList.Add(new Monster(this, monsterSpriteInfo)
            {
                Position = new Vector3(-6.0f, 0.0f, -12.0f)
            });
            monsterList.Add(new Monster(this, monsterSpriteInfo)
            {
                Position = new Vector3(-5.0f, 0.0f, -5.0f)
            });
            monsterList.Add(new Monster(this, monsterSpriteInfo)
            {
                Position = new Vector3(-2.0f, 0.0f, -6.0f)
            });
            monsterList.Add(new Monster(this, monsterSpriteInfo)
            {
                Position = new Vector3(-1.0f, 0.0f, -1.0f)
            });
            //Library.SpriteInfo spriteInfo = GameEngine.Content.Load<Library.SpriteInfo>(@"Sprites\\MagicCircle");

            //MagicCircleEffect magicCircleEffect = new MagicCircleEffect(this, spriteInfo, 0.01f, 0.01f, new Point(0, 0));
            //magicCircleEffect.Position = new Vector3(0.0f, 0.0f, -1.0f);

            //FireTornado ft = new FireTornado(this, Vector3.UnitZ, 15.0f);
            //ft.Initialize();

            ChainBeam cb = new ChainBeam(this, 50.0f);
            cb.Initialize();

            //After Adding the players set the playerlist onto the camera
            ActionCamera camera = (ActionCamera)GameEngine.Services.GetService(typeof(Camera));
            camera.ActorsToFollow = new BillboardList();
            camera.ActorsToFollow.AddRange(playerList);
            //camera.ActorsToFollow.AddRange(monsterList);
            camera.Initialize();

            (new FrameRateCounterText2D(this, Vector2.Zero, Color.White, GameEngine.Content.Load<SpriteFont>("GUI\\menufont"), 1.0f)).Initialize(); 

        }

        public override void Update(GameTime gameTime)
        {
            HandleInput(gameTime);
            base.Update(gameTime);

        }

        private void HandleInput(GameTime gameTime)
        {
            KeyboardDevice keyboardDevice = (KeyboardDevice)GameEngine.Services.GetService(typeof(KeyboardDevice));
            InputHub inputHub = (InputHub)GameEngine.Services.GetService(typeof(InputHub));
            GamepadDevice gamepadDevice = inputHub[inputHub.MasterInput];

            if (keyboardDevice.WasKeyPressed(Keys.Escape) || gamepadDevice.WasButtonPressed(Buttons.Start))
            {
                new PauseScreen("pause", this, inputHub.MasterInput);
            }
        }


    }
}
