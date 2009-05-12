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

namespace TheGame
{
    public class GameScreenCollection : KeyedCollection<string, GameScreen>
    {
        // Allow us to get a screen by name like so:
        // Engine.GameScreens["ScreenName"]
        protected override string GetKeyForItem(GameScreen item)
        {
            return item.Name;
        }

        protected override void RemoveItem(int index)
        {
            // Get the screen to be removed
            GameScreen screen = Items[index];

            // If this screen is the current default screen, set the
            // default to the background screen
            //if (Engine.DefaultScreen == screen)
            //    Engine.DefaultScreen = Engine.BackgroundScreen;

            base.RemoveItem(index);
        }
    }
}
