using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGame.Components.Display;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace TheGame.Game_Screens
{
    class MainMenuScreen : MenuScreen
    {

        private ContentManager content;

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent()
        {
            if (content == null)
                content = GameEngine.Content;

            //Build the menu from XML
            XMLPanel2DBuilder componentBuilder = new XMLPanel2DBuilder(this, content, "MenuPanels\\mainpanel.xml");

            //Make it into functional menu
            menu = MenuPanel2D.CreateMenuPanel2D(componentBuilder.Panel, 1, 3);

            //Add to drawable components
            Components.Add(menu);
        }

        public MainMenuScreen(string name)
            : base(name)
        {
        }

        protected override void HandleInput()
        {
            base.HandleInput();

            if (keyboardDevice.WasKeyPressed(Keys.Enter))
            {
                switch (menu.GetCurrentText())
                {
                    case "Start Game":
                        GameEngine.GameScreens.Remove(this);
                        new Level("test", "Terrain\\terrain");
                        //new TestScreen("test");
                        break;
                    case "Exit Game":
                        GameEngine.Game.Exit();
                        break;
                }
            }
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            content.Unload();
        }
    }
}
