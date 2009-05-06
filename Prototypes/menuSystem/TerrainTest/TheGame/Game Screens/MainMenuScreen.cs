using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGame.Components.Display;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame.Game_Screens
{
    class MainMenuScreen : MenuScreen
    {

        private ContentManager content;

        private GameWeaponMenuPanel2D weaponPanel;

        private Texture2D item;

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

            item = content.Load<Texture2D>("itemsm");
            //CreateWeaponPanel();


            //Make it into functional menu
            menu = MenuPanel2D.CreateMenuPanel2D(componentBuilder.Panel, 1, 3);

            //Add to drawable components
            Components.Add(menu);
            //Components.Add(weaponPanel);
        }

        private void CreateWeaponPanel()
        {
            weaponPanel = new GameWeaponMenuPanel2D(this, new Vector2(500, 450));
            weaponPanel.AddWeapon("Sword1", new ImageComponent2D(this, Vector2.Zero, item));
            weaponPanel.AddWeapon("Sword2", new ImageComponent2D(this, Vector2.Zero, item));
            weaponPanel.AddWeapon("Sword3", new ImageComponent2D(this, Vector2.Zero, item));
            weaponPanel.AddWeapon("Sword4", new ImageComponent2D(this, Vector2.Zero, item));
            weaponPanel.UpdateItemPositions();
        }

        public MainMenuScreen(string name)
            : base(name)
        {
        }

        protected override void HandleInput()
        {
            base.HandleInput();

            if (keyboardDevice.WasKeyPressed(Keys.Enter) || gamepadDevice.IsButtonDown(Buttons.A))
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

        public override void UnloadContent()
        {
            base.UnloadContent();
            content.Unload();
        }
    }
}
