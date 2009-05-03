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
    //TODO: Draw oder MAY be added to the interface
    public interface IDrawableComponent
    {
        void Draw(GameTime gameTime);

        bool Visible
        {
            get;
            set;
        }
    }

    public interface I3DComponent
    {
        Vector3 Position
        {
            get;
            set;
        }

        Quaternion Rotation
        {
            get;
            set;
        }

        float Scale
        {
            get;
            set;
        }
    }
}
