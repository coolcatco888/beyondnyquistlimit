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

namespace WTFJimGameProject
{

    //TODO: This may have to be deprecated use Rectangle class from C# library?
    public class BoundingRectangle
    {
        Vector2 topLeft;
        Vector2 widthHeight;

        public BoundingRectangle(float x, float y, float width, float height)
        {
            topLeft = new Vector2(x, y);
            widthHeight = new Vector2(width, height);
        }

        public float Left
        {
            get { return topLeft.X; }
        }

        public float Right
        {
            get { return topLeft.X + widthHeight.X; }
        }

        public float Top
        {
            get { return topLeft.Y; }
        }

        public float Bottom
        {
            get { return topLeft.Y + widthHeight.Y; }
        }
    }

    public interface I2DComponent
    {
        //TODO - Decide whether this is needed
        BoundingRectangle Bounds { get; set;}
        Vector2 Position { get; set; }
        SpriteBatch SpriteBatch { get; set; }

        void Update();

        void Draw();
    }

    public interface ITextComponent
    {
        //TODO: Add more parameters
        String Text { get; set; }
    }
    //TODO - Should this be abstract?  (I don't see the reason why a Component should be instantiated.
    //An abstract class does not have to have abstract methods)
    public class Component
    {
        #region Parameters

        public GameScreen Parent
        {
            get { return parent; }
            set { parent = value; }
        }
        GameScreen parent;

        public bool Initialized
        {
            get { return initialized; }
            set { initialized = value; }
        }
        bool initialized;

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }
        bool enabled;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }
        bool visible;

        #endregion

        public Component(GameScreen parent)
        {
            this.parent = parent;
            this.enabled = true;
            this.visible = true;

            Initialize(parent);
        }

        public virtual void Initialize(GameScreen parent)
        {
            //TODO: add component to parent
            initialized = true;
        }

        public virtual void Update() { }

        public virtual void Draw() { }

        public virtual void Dispose()
        {
            parent.Components.Remove(this);
        }
    }
}
