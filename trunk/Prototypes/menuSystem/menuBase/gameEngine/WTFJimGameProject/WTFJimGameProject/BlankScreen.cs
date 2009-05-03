using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WTFJimGameProject.GameWindows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WTFJimGameProject
{
    class BlankScreen : GameScreen
    {

        ContentManager content;

        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);
            PlayerIndex playerIndex;
            if (input.IsMenuCancel(ControllingPlayer, out playerIndex))
            {
                content.Dispose();
                Owner.Game.Exit();
            }
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(Owner.Game.Services, "Content");
            Components.Add(new TextComponent2D(this, new Vector2(200, 200), "Blank Screen", Color.White, content.Load<SpriteFont>("menufont")));

        }
        public BlankScreen(string name, ScreenManager owner)
            : base(name, owner)
        {
            
        }
    }
}
