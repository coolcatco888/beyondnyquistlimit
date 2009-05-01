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

namespace WTFJimGameProject
{
    class TestSquare : Component
    {
        public TestSquare(GameScreen parent)
            : base(parent)
        {

        }

        public override void Draw()
        {
            Parent.Owner.GraphicsDevice.Clear(Color.DarkOliveGreen);
            base.Draw();
        }
    }
}
