using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WTFJimGameProject.GameWindows
{
    /// <summary>
    /// Essentially a collection of Components to be drawn relative to the panel location.
    /// 0,0 is the panel's top left hand corner. This implementation
    /// is intended to be the base for menu windows, alert boxes, dialog boxes and HUDs.
    /// When implementing menus, have the menu image as the first item added.
    /// 
    /// See MainMenuScreen LoadContent() for how to setup
    /// </summary>
    class PanelComponent2D : Component
    {
        protected Vector2 position;

        protected SpriteBatch spriteBatch;

        protected List<I2DComponent> panelItems = new List<I2DComponent>();

        public List<I2DComponent> PanelItems
        {
            set { panelItems = value; }
            get { return panelItems; }
        }

        public Vector2 Position
        {
            set { position = value; }
            get { return position; }
        }

        public SpriteBatch SpriteBatch
        {
            set { spriteBatch = value; }
            get { return spriteBatch; }
        }

        public PanelComponent2D(GameScreen parent, Vector2 position) : base(parent)
        {
            this.position = position;
            this.spriteBatch = parent.Owner.SpriteBatch;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (I2DComponent item in panelItems)
            {
                item.Update(gameTime);
            }
        }

        public override void Draw()
        {
            foreach (I2DComponent item in panelItems)
            {
                Vector2 origninalPos = item.Position;
                item.Position = origninalPos + position;
                item.Draw();
                item.Position = origninalPos;
            }
        }


    }
}
