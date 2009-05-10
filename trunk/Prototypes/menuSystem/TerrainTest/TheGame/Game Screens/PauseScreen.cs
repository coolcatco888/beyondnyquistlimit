using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TheGame.Components.Display;


namespace TheGame.Game_Screens
{
    class PauseScreen : MenuScreen
    {
        private PlayerIndex sender;

        private ImageComponent2D fadeScreen;

        private GameScreen previousScreen;

        private void CreateMenu()
        {
            SpriteFont font = GameEngine.Content.Load<SpriteFont>("menufont");
            Texture2D menuBg = GameEngine.Content.Load<Texture2D>("menubg");

            menu = new MenuPanel2D(this, new Vector2(250, 200));
            menu.StartIndex = 1;
            menu.EndIndex = 2;
            menu.CurrentIndex = 1;
            menu.PanelItems.Add(new ImageComponent2D(this, Vector2.Zero, menuBg));
            menu.PanelItems.Add(new MenuTextComponent2D(this, new Vector2(10, 2), "Continue Game", Color.White, font, Color.Yellow, true));
            menu.PanelItems.Add(new MenuTextComponent2D(this, new Vector2(10, 42), "Exit Game", Color.White, font, Color.Yellow, false));
            Components.Add(menu);
        }

        public PauseScreen(string name, GameScreen previousScreen, PlayerIndex sender)
            : base(name)
        {
            this.sender = sender;
            this.fadeScreen = new ImageComponent2D(this, Vector2.Zero, new Texture2D(GameEngine.Graphics, GameEngine.Graphics.Viewport.Width, GameEngine.Graphics.Viewport.Height), new Color(Color.Black, 127));
            this.Components.Add(fadeScreen);
            GameEngine.BaseScreen.AlwaysUpdate = true;
            this.BlocksUpdate = true;
            this.previousScreen = previousScreen;
            CreateMenu();

        }

        protected override void HandleInput()
        {
            if (keyboardDevice.WasKeyPressed(Keys.Enter) || gamepadDevice.IsButtonDown(Buttons.A))
            {
                switch (menu.GetCurrentText())
                {
                    case "Continue Game":
                        Dispose();
                        break;
                    case "Exit Game":
                        previousScreen.Dispose();
                        Dispose();
                        (new MainMenuScreen("main_screen_fix_me_why_does_it_fail?")).LoadContent();
                        break;
                }
            }

            base.HandleInput();
        }




    }
}
