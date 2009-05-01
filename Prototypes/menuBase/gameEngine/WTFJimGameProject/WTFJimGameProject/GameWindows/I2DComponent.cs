using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WTFJimGameProject.GameWindows
{
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

        void Update(GameTime gameTime);
        void Draw();
    }
}
