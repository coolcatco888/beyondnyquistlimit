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
    class TestScreen : GameScreen
    {
        /// <summary>
        /// Handles the loading and unloading of game content.
        /// </summary>
        ContentManager content;

        PanelComponent2D panel;

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(Owner.Game.Services, "Content");

            //NOTE: This just builds the menupanel but there is no functionality yet.
            XMLPanel2DBuilder componentBuilder = new XMLPanel2DBuilder(this, content, "MenuPanels\\mainpanel.xml");
            panel = componentBuilder.Panel;
        
            base.LoadContent();
        }

        public override void Dispose()
        {
            base.Dispose();
            content.Dispose();
        }

        public TestScreen(string name, ScreenManager owner)
            : base(name, owner)
        {
            Components.Add(new TestSquare(this));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            panel.Update(gameTime);
        }

        public override void Draw()
        {
            base.Draw();
            panel.Draw();
        }
    }
}
