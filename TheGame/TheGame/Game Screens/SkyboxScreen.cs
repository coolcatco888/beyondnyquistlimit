#region Using Statements
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
#endregion

namespace TheGame
{
    class SkyboxScreen : GameScreen
    {
        public SkyboxScreen(string name)
            : base(name)
        {  
        }

        public override void Initialize()
        {
            Skybox sky;

            sky = new Skybox(this, "redsky");

            base.Initialize();
        }
    }
}
