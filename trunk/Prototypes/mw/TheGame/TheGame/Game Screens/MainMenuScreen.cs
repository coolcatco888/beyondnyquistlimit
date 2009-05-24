using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGame.Components.Display;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheGame.Components.GUI;
using TheGame.Components.Cameras;

namespace TheGame.Game_Screens
{
    /// <summary>
    /// Specific impmentation of a menu screen. First Screen Loaded whe the game is run.
    /// </summary>
    class MainMenuScreen : MenuScreen
    {
        

        /// <summary>
        /// Loads the Texture and Font for the menu
        /// </summary>
        public override void LoadContent()
        {
            Texture2D gradient = GameEngine.Content.Load<Texture2D>("GUI\\healthbar");
            Vector2 scale = new Vector2((float)GameEngine.Graphics.Viewport.Width / (float)gradient.Width,
                (float)GameEngine.Graphics.Viewport.Height / (float)gradient.Height);
            (new ImageComponent2D(this, Vector2.Zero, gradient, Color.CornflowerBlue, scale)).Initialize();

            //Build the menu from XML
            XMLPanel2DBuilder componentBuilder = new XMLPanel2DBuilder(this, GameEngine.Content, "Components\\GUI\\MenuPanels\\mainpanel.xml");
            Texture2D gauge = GameEngine.Content.Load<Texture2D>("GUI\\circGauge");
            
            //Make it into functional menu
            menu = MenuPanel2D.CreateMenuPanel2D(componentBuilder.Panel, 1, 3);
            menu.Initialize();

            //CreateCharacterStatusHUD();//Uncomment this to see the HUD
            base.LoadContent();
        }

        /// <summary>
        /// This is an example of how to create a hud using the sample images provided.
        /// </summary>
        private void CreateCharacterStatusHUD()
        {
            HUDStatusComponent2D hud;
            CharacterStatusDisplayParams hudParams = new CharacterStatusDisplayParams();
            hudParams.BarImage = GameEngine.Content.Load<Texture2D>("GUI\\healthbar");
            hudParams.DamageBarColor = Color.White;
            hudParams.FontColor = Color.White;
            hudParams.FontScale = 0.4f;
            hudParams.HealthBarColor = Color.Red;
            hudParams.HealthBarMaxValue = 1000;
            hudParams.HealthBarPos = new Vector2(58, 14);
            hudParams.HudImage = GameEngine.Content.Load<Texture2D>("GUI\\hud");
            hudParams.Level = 1;
            hudParams.LevelPos = new Vector2(80, 57);
            hudParams.ManaBarColor = Color.Blue;
            hudParams.ManaBarMaxValue = 500;
            hudParams.ManaBarPos = new Vector2(58, 36);
            hudParams.PlayerImage = GameEngine.Content.Load<Texture2D>("GUI\\itemsm");
            hudParams.PlayerImageCentrePos = new Vector2(34, 34);
            hudParams.Position = Vector2.Zero + new Vector2(20, 20);
            hudParams.TextFont = GameEngine.Content.Load<SpriteFont>("GUI\\menufont");
            hudParams.attackCurrentValue = 70;
            hudParams.attackGaugeEmptyColor = Color.Gray;
            hudParams.attackGaugeEndAngle = MathHelper.Pi * 2.0f - MathHelper.PiOver4;
            hudParams.attackGaugeFullColor = Color.Red;
            hudParams.attackGaugeImage = GameEngine.Content.Load<Texture2D>("GUI\\circGauge");
            hudParams.attackGaugeRadius = 32.0f;
            hudParams.attackGaugeStartAngle = MathHelper.PiOver4;
            hudParams.attackMaxValue = 100;
            hudParams.attackPosition = new Vector2(34, 32);

            hud = new CharacterStatusComponent2D(this, hudParams);
            hud.Initialize();
        }

        /// <summary>
        /// Creates a new MainMenuScreen
        /// </summary>
        /// <param name="name">Identifier to reference the screen</param>
        public MainMenuScreen(string name)
            : base(name)
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            AudioManager audioManager = (AudioManager)GameEngine.Services.GetService(typeof(AudioManager));
            audioManager.Play3DCue("music_menu", this);
        }

        /// <summary>
        /// All handles keyboard and controller input
        /// </summary>
        protected override void HandleInput()
        {
            base.HandleInput();
            HandleMenuSelection();
            
        }

        /// <summary>
        /// Example of how to handle menu selections
        /// </summary>
        private void HandleMenuSelection()
        {
            foreach (GamepadDevice pad in allGamePads)
            {
                if (keyboardDevice != null && keyboardDevice.WasKeyPressed(Keys.Enter) || pad != null && pad.WasButtonPressed(Buttons.A))
                {
                    switch (menu.GetCurrentText())
                    {
                        case "Start Game":
                            Dispose();
                            GameScreen characterSelectScreen = new CharacterSelectScreen("charselect");
                            characterSelectScreen.Initialize();

                            break;
                        case "Exit Game":
                            GameEngine.EndGame();
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Unloads textures and fonts
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();
            GameEngine.Content.Unload();
        }

        /// <summary>
        /// Handles cleanup when destroyed
        /// </summary>
        public override void Dispose()
        {
            UnloadContent();
            base.Dispose();
        }
    }
}
