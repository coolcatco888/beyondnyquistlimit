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
        protected PanelComponents panelItems;

        public PanelComponents PanelItems
        {
            set { panelItems = value; }
            get { return panelItems; }
        }

        public PanelComponent2D(GameScreen parent, Vector2 position) : base(parent)
        {
            this.position = position;
            this.panelItems = new PanelComponents(this);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (DisplayComponent2D item in panelItems)
            {
                item.Update(gameTime);
            }
        }
    }

    class PanelComponents : List<DisplayComponent2D>
    {
        PanelComponent2D owner;

        public PanelComponents(PanelComponent2D owner)
        {
            this.owner = owner;
        }
        
        /// <summary>
        /// Adds the components into the component list and converts the component's position to the absolute
        /// screen position.
        /// </summary>
        /// <param name="item"></param>
        public new void Add(DisplayComponent2D item)
        {
            item.Position = item.Position + owner.Position;
            base.Add(item);
        }
    }

}
