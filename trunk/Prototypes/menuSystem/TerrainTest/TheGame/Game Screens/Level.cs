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

        private SpriteFont font;

        private Actor actor;

        protected KeyboardDevice keyboardDevice = (KeyboardDevice)GameEngine.Services.GetService(typeof(KeyboardDevice));
        protected GamepadDevice gamepadDevice = (GamepadDevice)GameEngine.Services.GetService(typeof(GamepadDevice));
        private GameWeaponMenuPanel2D weaponPanel;

        private Random random;

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
            font = GameEngine.Content.Load<SpriteFont>("menufont");

            random = new Random();

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
                    Vector2 stickPosition = gamepadDevice.RightStickPosition;
                    if (stickPosition == Vector2.Zero)
                    {
                        stickPosition.X = stickPosition.X + -0.01f;
                    }
                    string selected = weaponPanel.SelectNewWeapon(stickPosition);
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
            

            if (gamepadDevice.WasButtonPressed(Buttons.X) || keyboardDevice.WasKeyPressed(Keys.Space))
            {
                new HitTextComponent2D(this, GetActorScreenCoordinates(), -random.Next(300), Color.Red, font);
            }

            if (gamepadDevice.WasButtonPressed(Buttons.Y) || keyboardDevice.WasKeyPressed(Keys.LeftControl))
            {
                new HitTextComponent2D(this, GetActorScreenCoordinates(), random.Next(300), Color.Green, font);
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
            Vector2 coords = TransformPositionToScreenCoordinates(actor.Position);

            return coords;
        }

        public Vector2 TransformPositionToScreenCoordinates(Vector3 oPosition)
        {
            int offset = 17;//TODO: Change this once centre of billboard found!
            Camera camera = (Camera)GameEngine.Services.GetService(typeof(Camera));
            Vector4 oTransformedPosition = Vector4.Transform(oPosition, camera.View * camera.Projection);
            if (oTransformedPosition.W != 0)
            {
                oTransformedPosition.X = oTransformedPosition.X / oTransformedPosition.W;
                oTransformedPosition.Y = oTransformedPosition.Y / oTransformedPosition.W;
                oTransformedPosition.Z = oTransformedPosition.Z / oTransformedPosition.W;
            }

            Vector2 oPosition2D = new Vector2(
              oTransformedPosition.X * GameEngine.Graphics.PresentationParameters.BackBufferWidth / 2 + GameEngine.Graphics.PresentationParameters.BackBufferWidth / 2,
              -oTransformedPosition.Y * GameEngine.Graphics.PresentationParameters.BackBufferHeight / 2 + GameEngine.Graphics.PresentationParameters.BackBufferHeight / 2);

            return new Vector2(oPosition2D.X - offset, oPosition2D.Y - offset);
        }
    }
}
