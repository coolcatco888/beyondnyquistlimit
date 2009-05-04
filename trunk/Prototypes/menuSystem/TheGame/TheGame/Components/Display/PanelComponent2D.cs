using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame.Components.Display
{
    /// <summary>
    /// Essentially a collection of Components to be drawn relative to the panel location.
    /// 0,0 is the panel's top left hand corner. This implementation
    /// is intended to be the base for menu windows, alert boxes, dialog boxes and HUDs.
    /// When implementing menus, have the menu image as the first item added.
    /// 
    /// See MainMenuScreen LoadContent() for how to setup
    /// </summary>
    class PanelComponent2D : DisplayComponent2D
    {
        protected PanelComponents panelItems = new PanelComponents();

        public PanelComponents PanelItems
        {
            set { panelItems = value; }
            get { return panelItems; }
        }

        public PanelComponent2D(GameScreen parent, Vector2 position) : base(parent)
        {
            this.position = position;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (DisplayComponent2D item in panelItems)
            {
                item.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (DisplayComponent2D item in panelItems)
            {
                Vector2 origninalPos = item.Position;
                item.Position = origninalPos + position;
                item.Draw(gameTime);
                item.Position = origninalPos;
            }
        }
    }

    class PanelComponents : List<DisplayComponent2D>
    {
        /// <summary>
        /// Adds the components into the component list and removes the parent screen's reference
        /// to the component.  This ensures that drawing and updating of subcomponents are handled by 
        /// the panel and not the screen as the panel modifies its children's attributes upon drawing.
        /// </summary>
        /// <param name="item"></param>
        public new void Add(DisplayComponent2D item)
        {
            item.Parent.Components.Remove(item);
            base.Add(item);
        }
    }

}
