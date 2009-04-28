using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagement.GameWindows
{
    /// <summary>
    /// Represents graphical items in a panel, this could be either text or graphics or panels
    /// 
    /// TODO - Extend from Jim's Component class and refactor method parameters to conform to standards
    /// </summary>
    abstract class Component2D
    {
        protected Vector2 position;

        public Vector2 Position
        {
            set { position = value; }
            get { return position; }
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

    }
}
