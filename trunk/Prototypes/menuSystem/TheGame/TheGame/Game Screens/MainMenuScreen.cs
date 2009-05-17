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
            //Build the menu from XML
            XMLPanel2DBuilder componentBuilder = new XMLPanel2DBuilder(this, GameEngine.Content, "Components\\GUI\\MenuPanels\\mainpanel.xml");
            Texture2D gauge = GameEngine.Content.Load<Texture2D>("GUI\\circGauge");
            
            //Make it into functional menu
            menu = MenuPanel2D.CreateMenuPanel2D(componentBuilder.Panel, 1, 3);
            menu.Initialize();
            (new CircularGauge(this, new Vector2(200, 200), gauge, 100, 55, 30.0f, MathHelper.PiOver4, MathHelper.Pi * 2.0f - MathHelper.PiOver4, Color.Red, Color.Black)).Initialize();

        }

        /// <summary>
        /// Creates a new MainMenuScreen
        /// </summary>
        /// <param name="name">Identifier to reference the screen</param>
        public MainMenuScreen(string name)
            : base(name)
        {

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
            if (keyboardDevice.WasKeyPressed(Keys.Enter) || gamepadDevice.WasButtonPressed(Buttons.A))
            {
                switch (menu.GetCurrentText())
                {
                    case "Start Game":
                        Dispose();
                        new SkyboxScreen("sky");
                        new Level("level", "Terrain\\terrain");
                        break;
                    case "Exit Game":
                        break;
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
