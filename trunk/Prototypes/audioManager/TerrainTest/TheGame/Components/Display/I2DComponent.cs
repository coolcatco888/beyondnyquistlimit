using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame.Components.Display
{
    //TODO: May need to be included in Component.cs? Should this maybe extend IDrawableComponent?
    interface I2DComponent
    {
        Vector2 Position
        {
            set;
            get;
        }


        SpriteBatch SpriteBatch
        {
            set;
            get;
        }


        //TODO: Should be in IDrawableComponent?
        void Update(GameTime gameTime);
    }
}
