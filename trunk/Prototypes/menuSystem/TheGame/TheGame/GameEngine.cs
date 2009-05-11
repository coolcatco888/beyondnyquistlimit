using System;
using System.Collections.Generic;
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

namespace TheGame
{
    public static class GameEngine
    {
        static bool initialized;

        public static bool Initialized
        {
            get { return initialized; }
            set { initialized = value; }
        }

        static GameTime gameTime;

        public static GameTime GameTime
        {
            get { return gameTime; }
            set { gameTime = value; }
        }

        static SpriteBatch spriteBatch;

        /// <summary>
        /// A default SpriteBatch shared by all the screens. This saves
        /// each screen having to bother creating their own local instance.
        /// </summary>
        public static SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        public static GraphicsDevice Graphics
        {
            get { return game.GraphicsDevice; }
        }

        public static ContentManager Content
        {
            get { return game.Content; }
        }

        public static GameServiceContainer Services
        {
            get { return game.Services; }
        }

        public static GameScreenCollection GameScreens
        {
            get { return screenManager.GameScreens; }
            set { screenManager.GameScreens = value; }
        }

        static Game game;
        static ScreenManager screenManager;


        public static GameScreen BaseScreen
        {
            get { return GameEngine.baseScreen; }
            set { GameEngine.baseScreen = value; }
        }
        static GameScreen baseScreen;

        

        public static void Initialize(Game g)
        {
            game = g;

            screenManager = new ScreenManager(g);
            g.Components.Add(screenManager);

            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            initialized = true;
        }


    }
}
