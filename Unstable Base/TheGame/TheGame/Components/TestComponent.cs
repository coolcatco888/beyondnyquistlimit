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
    class TestComponent : Component, IDrawableComponent 
    {
        public TestComponent(GameScreen parent)
            : base(parent)
        {
            visible = true;
        }

        #region IDrawableComponent Members

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            GameEngine.Graphics.Clear(Color.Black);
        }

        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
            }
        }
        bool visible;

        #endregion
    }
}
