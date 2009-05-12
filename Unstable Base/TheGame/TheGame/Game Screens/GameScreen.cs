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
    public class GameScreen
    {
        #region Properties

        bool initialized;

        public bool Initialized
        {
            get { return initialized; }
            set { initialized = value; }
        }

        bool visible;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        bool enabled;

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        bool blocksUpdate;

        public bool BlocksUpdate
        {
            get { return blocksUpdate; }
            set { blocksUpdate = value; }
        }

        bool blocksDraw;

        public bool BlocksDraw
        {
            get { return blocksDraw; }
            set { blocksDraw = value; }
        }

        bool alwaysUpdate;

        public bool AlwaysUpdate
        {
            get { return alwaysUpdate; }
            set { alwaysUpdate = value; }
        }

        bool alwaysDraw;

        public bool AlwaysDraw
        {
            get { return alwaysDraw; }
            set { alwaysDraw = value; }
        }

        string name;

        public string Name
        {
            get { return name; }
        }

        

        #endregion

        //ScreenManager owner;
        ComponentCollection components;

        public ComponentCollection Components
        {
            get { return components; }
            set { components = value; }
        }

        public GameScreen(string name)
        {
            this.name = name;
            this.visible = true;
            this.enabled = true;

            components = new ComponentCollection(this);

            if(GameEngine.Initialized)
                GameEngine.GameScreens.Add(this);

            if (!initialized)
                Initialize();
        }

        public virtual void Initialize()
        {
            initialized = true;
        }

        /// <summary>
        /// Load graphics content for the screen.
        /// </summary>
        public virtual void LoadContent() { }


        /// <summary>
        /// Unload content for the screen.
        /// </summary>
        public virtual void UnloadContent() { }


        public virtual void Update(GameTime gameTime)
        {
            // Create a temporary list so we don't crash if
            // a component is added to the collection while
            // updating
            List<Component> updating = new List<Component>();

            // Populate the temporary list
            foreach (Component c in Components)
                updating.Add(c);

            // Update all components that have been initialized
            foreach (Component component in updating)
                if (component.Initialized && component.Enabled)
                    component.Update(gameTime);
        }

        public virtual void Draw()
        {
            // Temporary list
            List<Component> drawing = new List<Component>();
            List<IDrawableComponent> drawList3D = new List<IDrawableComponent>();
            List<IDrawableComponent> drawListBillboard = new List<IDrawableComponent>();
            List<IDrawableComponent> drawListEffects = new List<IDrawableComponent>();
            List<IDrawableComponent> drawList2D = new List<IDrawableComponent>();


            foreach (Component component in Components)
            {
                if (component is IDrawableComponent)
                {
                    drawing.Add(component);
                }
            }

            Component drawList;

            while (drawing.Count > 0)
            {
                drawList = drawing.First();
                drawing.Remove(drawList);

                if (drawList is I2DComponent)
                {
                    drawList2D.Add((IDrawableComponent)drawList);
                }
                else if (drawList is IPointSpriteSystem)
                {
                    drawListEffects.Add((IDrawableComponent)drawList);
                }
                else if (drawList is IBillboard)
                {
                    drawListBillboard.Add((IDrawableComponent)drawList);
                }
                else if (drawList is I3DComponent)
                {
                    drawList3D.Add((IDrawableComponent)drawList);
                }
                else
                {
                    drawList3D.Add((IDrawableComponent)drawList);
                }
            }

            //Seting up common 3D render states
            GameEngine.Graphics.RenderState.DepthBufferEnable = true;
            GameEngine.Graphics.RenderState.DepthBufferWriteEnable = true;
            GameEngine.Graphics.RenderState.AlphaBlendEnable = false;
            GameEngine.Graphics.RenderState.AlphaTestEnable = false;
            GameEngine.Graphics.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
            GameEngine.Graphics.SamplerStates[0].AddressV = TextureAddressMode.Wrap;

            foreach (IDrawableComponent drawable in drawList3D)
            {
                if(drawable.Visible)
                    drawable.Draw(GameEngine.GameTime);
            }

            // TEMPORARY
            GameEngine.Graphics.Clear(ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            //setting common render states for billboards
            GameEngine.Graphics.RenderState.AlphaTestEnable = true;
            GameEngine.Graphics.RenderState.AlphaFunction = CompareFunction.GreaterEqual;
            GameEngine.Graphics.RenderState.ReferenceAlpha = 200;

            foreach (IDrawableComponent drawable in drawListBillboard)
            {
                if (drawable.Visible)
                    drawable.Draw(GameEngine.GameTime);
            }


            //setting for sprite batch effects
            GameEngine.Graphics.RenderState.PointSpriteEnable = true;
            GameEngine.Graphics.RenderState.PointSizeMax = 256;

            GameEngine.Graphics.RenderState.AlphaBlendEnable = true;
            GameEngine.Graphics.RenderState.AlphaBlendOperation = BlendFunction.Add;
            GameEngine.Graphics.RenderState.SourceBlend = Blend.SourceAlpha;
            GameEngine.Graphics.RenderState.DestinationBlend = Blend.One;

            GameEngine.Graphics.RenderState.DepthBufferEnable = true;
            GameEngine.Graphics.RenderState.DepthBufferWriteEnable = false;

            foreach (IDrawableComponent drawable in drawListEffects)
            {
                if (drawable.Visible)
                    drawable.Draw(GameEngine.GameTime);
            }

            GameEngine.Graphics.RenderState.PointSpriteEnable = false;
            GameEngine.Graphics.RenderState.AlphaBlendEnable = false;
            GameEngine.Graphics.RenderState.DepthBufferWriteEnable = true;

            //SpriteBatch handles the device render states

            foreach (IDrawableComponent drawable in drawList2D)
            {
                if (drawable.Visible)
                    drawable.Draw(GameEngine.GameTime);
            }

        }

        public virtual void Dispose()
        {
            components.Clear();
            GameEngine.GameScreens.Remove(this);
            //owner.GameScreens.Remove(this);
        }
    }
}
