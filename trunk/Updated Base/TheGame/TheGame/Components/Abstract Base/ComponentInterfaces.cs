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

        Vector3 Scale
        {
            get;
            set;
        }

        void Translate(Vector3 translation);
        void Translate(float x, float y, float z);

        void Rotate(Vector3 rotation);
        void Rotate(float yaw, float pitch, float roll);

        void ApplyScale(Vector3 scale);
        void ApplyScale(float x, float y, float z);
    }

    public interface I2DComponent
    {
        
        //Vector2 Scale
        //{
        //    get;
        //    set;
        //}

        //This is the top left corner;
        Vector2 Position
        {
            get;
            set;
        }

        Vector2 Center
        {
            get;
        }

        float Height
        {
            get;
        }

        float Width
        {
            get;
        }

        float Left
        {
            get;
        }
        
        float Right
        {
            get;
        }
        
        float Bottom
        {
            get;
        }
        
        float Top
        {
            get;
        }
    }

    public interface IBillboard
    {
        Texture2D Texture2D
        {
            get;
            set;
        }
    }

    public interface IPointSpriteSystem
    {
        PointSpriteSystemSettings Settings
        {
            get;
        }
    }

    public interface ICollidable
    {
        PrimitiveShape PrimitiveShape
        {
            get;
        }

        bool IsHit(PrimitiveShape otherShape);

        bool Collidable
        {
            get;
            set;
        }
    }
}
