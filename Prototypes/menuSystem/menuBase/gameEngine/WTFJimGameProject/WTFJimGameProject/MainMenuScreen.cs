using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using WTFJimGameProject.GameWindows;

namespace WTFJimGameProject
{
    class MainMenuScreen : MenuScreen
    {
        /// <summary>
        /// Handles the loading and unloading of game content.
        /// </summary>
        ContentManager content;

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(Owner.Game.Services, "Content");

            //Build the menu from XML
            XMLPanel2DBuilder componentBuilder = new XMLPanel2DBuilder(this, content, "MenuPanels\\mainpanel.xml");

            //Make it into functional menu
            menu = MenuPanel2D.CreateMenuPanel2D(componentBuilder.Panel, 1, 2);

            //Add to drawable components
            Components.Add(menu);
            base.LoadContent();
        }

        /// <summary>
        /// Handles the input specifically for this menu.
        /// </summary>
        /// <param name="input"></param>
        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);
            PlayerIndex playerIndex;
            if (input.IsMenuSelect(ControllingPlayer, out playerIndex))
            {
                string menuOption = menu.GetCurrentText();

                switch (menuOption)
                {
                    case "Exit Game":
                        Dispose();
                        Owner.Game.Exit();
                        break;

                    case "Start Game":
                        //TODO - Screen Manager must have an add and remove method
                        (new BlankScreen("Game", Owner)).LoadContent();
                        this.Visible = false;
                        break;
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            content.Dispose();
        }

        public MainMenuScreen(string name, ScreenManager owner)
            : base(name, owner)
        {
            Components.Add(new TestSquare(this));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
