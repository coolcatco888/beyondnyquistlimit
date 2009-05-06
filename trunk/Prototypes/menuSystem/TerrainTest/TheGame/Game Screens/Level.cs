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
using TheGame.Components.Display;
#endregion

namespace TheGame
{
    class Level : GameScreen
    {
        #region Fields

        private Texture2D item;

        private Actor actor;

        protected KeyboardDevice keyboardDevice = (KeyboardDevice)GameEngine.Services.GetService(typeof(KeyboardDevice));
        protected GamepadDevice gamepadDevice = (GamepadDevice)GameEngine.Services.GetService(typeof(GamepadDevice));
        private GameWeaponMenuPanel2D weaponPanel;

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

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public Level(string name, string terrainFileName)
            : base(name)
        {
            new TestComponent(this);

            levelMap = new Terrain(this, terrainFileName);
            terrainHeightMap = levelMap.HeightMapInfo;

            item = GameEngine.Content.Load<Texture2D>("itemsm");

            actor = new Actor(this, GameEngine.Content.Load<Texture2D>("theifWalkRun"), 64, 64, 1);
            actor.Position = new Vector3(0.0f, 0.0f, -30.0f);
        }

        public override void Update()
        {
            base.Update();

            if (gamepadDevice.IsButtonDown(Buttons.RightShoulder))
            {
                if (weaponPanel == null)
                {
                    CreateWeaponPanel();
                    Components.Add(weaponPanel);
                }
                if (weaponPanel != null)
                {
                    string selected = weaponPanel.SelectNewWeapon(gamepadDevice.LeftStickPosition);
                }
            }
            else
            {
                if (weaponPanel != null)
                {
                    weaponPanel.KillMenu();
                    weaponPanel = null;
                }

            }
        }

        private void CreateWeaponPanel()
        {
            weaponPanel = new GameWeaponMenuPanel2D(this, GetActorScreenCoordinates());
            weaponPanel.AddWeapon("Sword1", new ImageComponent2D(this, Vector2.Zero, item));
            weaponPanel.AddWeapon("Sword2", new ImageComponent2D(this, Vector2.Zero, item));
            weaponPanel.AddWeapon("Sword3", new ImageComponent2D(this, Vector2.Zero, item));
            weaponPanel.AddWeapon("Sword4", new ImageComponent2D(this, Vector2.Zero, item));
            weaponPanel.UpdateItemPositions();
        }


        private Vector2 GetActorScreenCoordinates()
        {
            Camera camera = (Camera)GameEngine.Services.GetService(typeof(Camera));
            

            Vector3 actorPosition = new Vector3(actor.Position.X, actor.Position.Y, actor.Position.Z);
            Vector3.Transform(actorPosition, camera.View * camera.Projection);

            //TODO: Fix Scaling Issue
            Vector2 coords = new Vector2(actorPosition.X * 10 + 375, actorPosition.Y * 10 + 375);

            return coords;
        }
    }
}
