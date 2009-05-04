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
    class TestScreen : GameScreen
    {
        public TestScreen(String name)
            : base(name)
        {
            new TestComponent(this);

            Terrain temp;

            //BasicModel temp;

            Skybox sky;

            //sky = new Skybox(this, "redsky");

            temp = new Terrain(this, "Terrain\\terrain");
            temp.Position = new Vector3(0.0f, 0.0f, 0.0f);

            Actor actor = new Actor(this, GameEngine.Content.Load<Texture2D>("theifWalkRun"), 64, 64, 1);


            //temp = new BasicModel(this, GameEngine.Content.Load<Model>("ig_box"));

            //temp = new BasicModel(this, GameEngine.Content.Load<Model>("ig_box"));
            //temp.Scale = 0.3f;
            //temp.Position = new Vector3(1.0f, 1.2f, 0.3f);

            //temp = new BasicModel(this, GameEngine.Content.Load<Model>("ig_box"));
            //temp.Scale = 1.0f;
            //temp.Position = new Vector3(0.0f, -2.2f, -4.5f);
        }
    }
}
