using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagement.GameWindows
{
    /// <summary>
    /// Represents graphical items in a panel, this could be either text or graphics
    /// </summary>
    abstract class ScreenItem
    {
        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
