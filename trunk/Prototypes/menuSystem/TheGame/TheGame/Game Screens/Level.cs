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

        private ActorList playerList;
        public ActorList PlayerList
        {
            get { return playerList; }
        }

        private ActorList monsterList;
        public ActorList MonsterList
        {
            get { return monsterList; }
        }
        
        #endregion

        string terrainFileName;

        /// <summary>
        /// A level game screen. Holds the components needed for the game.
        /// The terrain, the height information, any players and monster collections, etc
        /// </summary>
        /// <param name="name">Name of the GameScreen</param>
        /// <param name="terrainFileName">The filename of the terrain bitmap</param>
        public Level(string name, string terrainFileName)
            : base(name)
        {
            this.terrainFileName = terrainFileName;
            playerList = new ActorList();
            monsterList = new ActorList();
        }

        public override void Initialize()
        {
            //Initialize Camera
            ActionCamera camera = (ActionCamera)GameEngine.Services.GetService(typeof(Camera));
            camera.Initialize();
            camera.Position = new Vector3(0.0f, 10.0f, 25.0f);
            camera.LookAt = new Vector3(0, 0, 0);

            levelMap = new Terrain(this, terrainFileName);
            levelMap.Initialize();
            terrainHeightMap = levelMap.HeightMapInfo;
            
            Library.SpriteInfo playerSpriteInfo = GameEngine.Content.Load<Library.SpriteInfo>(@"Sprites\\ActorTest");
            Player player = new Player(this, playerSpriteInfo, PlayerIndex.One, "Wizard", new Vector3(1.0f, 2.0f, 1.0f));
            player.Initialize();
            playerList.Add(player);

            Library.SpriteInfo poringSpriteInfo = GameEngine.Content.Load<Library.SpriteInfo>(@"PoringSpriteInfo");
            Monster poring = new Monster(this, poringSpriteInfo, new Vector3(4.0f, 0.0f, 0.0f), "Poring");
            poring.Initialize();
            monsterList.Add(poring);

            camera.ActorsToFollow = playerList;

            base.Initialize();
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
                PauseScreen pauseScreen = new PauseScreen("pause", this, inputHub.MasterInput);
                pauseScreen.Initialize();
            }
        }

    }
}
