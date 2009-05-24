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

        private AudioManager audioManager;

        private void CreateMenu()
        {
            SpriteFont font = GameEngine.Content.Load<SpriteFont>("GUI\\menufont");
            Texture2D menuBg = GameEngine.Content.Load<Texture2D>("GUI\\menubg");

            menu = new MenuPanel2D(this, new Vector2(250, 200));
            menu.StartIndex = 1;
            menu.EndIndex = 2;
            menu.CurrentIndex = 1;
            menu.PanelItems.Add(new ImageComponent2D(this, Vector2.Zero, menuBg));
            menu.PanelItems.Add(new MenuTextComponent2D(this, new Vector2(10, 2), "Continue Game", Color.White, font, Color.Yellow, true));
            menu.PanelItems.Add(new MenuTextComponent2D(this, new Vector2(10, 42), "Exit Game", Color.White, font, Color.Yellow, false));
            menu.Initialize();
            base.LoadContent();
        }

        public PauseScreen(string name, GameScreen previousScreen, PlayerIndex sender)
            : base(name)
        {
            this.sender = sender;
            masterInput = inputHub[sender];
            Texture2D blank = GameEngine.Content.Load<Texture2D>("GUI\\blank");

         
            this.fadeScreen = new ImageComponent2D(this, Vector2.Zero, blank, new Color(Color.Black, 100), new Vector2(GameEngine.Graphics.Viewport.Width, GameEngine.Graphics.Viewport.Height));
            fadeScreen.Initialize();
            this.Components.Add(fadeScreen);
            GameEngine.BaseScreen.AlwaysUpdate = true;
            this.BlocksUpdate = true;
            this.previousScreen = previousScreen;
            CreateMenu();
            

        }

        public override void Initialize()
        {
            base.Initialize();

            audioManager = (AudioManager)GameEngine.Services.GetService(typeof(AudioManager));
            audioManager.PauseAllCues();
        }

        protected override void HandleInput()
        {
            
            if (keyboardDevice.WasKeyPressed(Keys.Escape) || masterInput.WasButtonPressed(Buttons.Start))
            {
                audioManager.ResumeAllCues();
                Dispose();
            }

            else if (keyboardDevice.WasKeyPressed(Keys.Enter) || masterInput.WasButtonPressed(Buttons.A))
            {
                switch (menu.GetCurrentText())
                {
                    case "Continue Game":
                        audioManager.ResumeAllCues();
                        Dispose();
                        break;
                    case "Exit Game":
                        foreach (GameScreen item in GameEngine.GameScreens.ToList())
                        {
                            if (item != GameEngine.BaseScreen)
                            {
                                item.Dispose();
                            }
                        }
                        MainMenuScreen main = new MainMenuScreen("main");
                        main.LoadContent();
                        main.Initialize();
                        break;
                }
            }

            base.HandleInput();
        }

    }
}
