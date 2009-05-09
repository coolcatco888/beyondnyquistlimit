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

        private GameWeaponMenuPanel2D weaponPanel;

        private Random random;

        private CharacterStatusComponent2D hud;

        protected KeyboardDevice keyboardDevice = (KeyboardDevice)GameEngine.Services.GetService(typeof(KeyboardDevice));
        protected GamepadDevice gamepadDevice = (GamepadDevice)GameEngine.Services.GetService(typeof(GamepadDevice));

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
            CreateHUD();
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
            else if (weaponPanel != null)
            {
                weaponPanel.KillMenu();
                weaponPanel = null;
            }

            if (gamepadDevice.WasButtonPressed(Buttons.X) || keyboardDevice.WasKeyPressed(Keys.Space))
            {
                int value = random.Next(300);
                new HitTextComponent2D(this, TransformPositionToScreenCoordinates(actor.Position), -value, Color.Red, font);
                hud.Healthbar.IncreaseDecreaseValue(-value);
            }

            if (gamepadDevice.WasButtonPressed(Buttons.Y) || keyboardDevice.WasKeyPressed(Keys.LeftControl))
            {
                int value = random.Next(300);
                new HitTextComponent2D(this, TransformPositionToScreenCoordinates(actor.Position), value, Color.Green, font);
                hud.Healthbar.IncreaseDecreaseValue(value);
            }
        }

        private void CreateWeaponPanel()
        {
            weaponPanel = new GameWeaponMenuPanel2D(this, TransformPositionToScreenCoordinates(actor.Position));
            weaponPanel.AddWeapon("Sword1", new ImageComponent2D(this, Vector2.Zero, item));
            weaponPanel.AddWeapon("Sword2", new ImageComponent2D(this, Vector2.Zero, item));
            weaponPanel.AddWeapon("Sword3", new ImageComponent2D(this, Vector2.Zero, item));
            weaponPanel.AddWeapon("Sword4", new ImageComponent2D(this, Vector2.Zero, item));
            weaponPanel.UpdateItemPositions();
        }

        private void CreateHUD()
        {
            StatusDisplay hudParams = new StatusDisplay();
            hudParams.BarImage = GameEngine.Content.Load<Texture2D>("healthbar");
            hudParams.DamageBarColor = Color.Orange;
            hudParams.FontColor = Color.White;
            hudParams.FontScale = 0.4f;
            hudParams.HealthBarColor = Color.Red;
            hudParams.HealthBarMaxValue = 1000;
            hudParams.HealthBarPos = new Vector2(58, 14);
            hudParams.HudImage = GameEngine.Content.Load<Texture2D>("hud");
            hudParams.Level = 1;
            hudParams.LevelPos = new Vector2(80, 57);
            hudParams.ManaBarColor = Color.Blue;
            hudParams.ManaBarMaxValue = 500;
            hudParams.ManaBarPos = new Vector2(58, 36);
            hudParams.PlayerImage = GameEngine.Content.Load<Texture2D>("itemsm"); 
            hudParams.PlayerImageCentrePos = new Vector2(34, 34);
            hudParams.Position = Vector2.Zero;
            hudParams.TextFont = GameEngine.Content.Load<SpriteFont>("menufont");

            hud = new CharacterStatusComponent2D(this, hudParams);

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
