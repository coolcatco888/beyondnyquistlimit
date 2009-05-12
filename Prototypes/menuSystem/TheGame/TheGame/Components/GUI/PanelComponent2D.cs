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

        public Rectangle Bounds;

        public override void Initialize()
        {
            foreach (DisplayComponent2D component in panelItems)
            {
                component.Initialize();
            }
            base.Initialize();
        }

        public PanelComponents PanelItems
        {
            set { panelItems = value; }
            get { return panelItems; }
        }

        public PanelComponent2D(GameScreen parent, Vector2 position) : base(parent)
        {
            this.position = position;
            this.Bounds = new Rectangle((int) this.position.X, (int) this.position.Y, 0, 0);
            this.panelItems = new PanelComponents(this);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (DisplayComponent2D item in panelItems)
            {
                item.Update(gameTime);
            }
        }

        public override void Dispose()
        {
            foreach (DisplayComponent2D component in panelItems)
            {
                component.Dispose();
            }
            base.Dispose();
        }

        public override Vector2 Center
        {
            get { return new Vector2(Left + 0.5f * Width, Top + 0.5f * Height); }
        }

        public override float Height
        {
            get { return (float) Bounds.Height; }
        }

        public override float Width
        {
            get { return (float) Bounds.Width; }
        }

        public override float Left
        {
            get { return (float) Bounds.X; }
        }

        public override float Right
        {
            get { return (float)Bounds.X + Width; }
        }

        public override float Bottom
        {
            get { return (float)Bounds.Y + Height; }
        }

        public override float Top
        {
            get { return (float) Bounds.Y; }
        }
    }

    class PanelComponents : List<DisplayComponent2D>
    {
        protected PanelComponent2D owner;

        public PanelComponents(PanelComponent2D owner)
        {
            this.owner = owner;
        }
        
        /// <summary>
        /// Adds the components into the component list and converts the component's position to the absolute
        /// screen position.  
        /// 
        /// NOTE-POTENTIAL BUG: This updates the panel's bounding box but it does not update when an item is removed.
        /// </summary>
        /// <param name="item"></param>
        public new void Add(DisplayComponent2D item)
        {
            item.Position = item.Position + owner.Position;
            if (item.Left < owner.Left)
            {
                owner.Bounds.X = (int) item.Position.X;
            }
            if (item.Top < owner.Top)
            {
                owner.Bounds.Y = (int)item.Position.Y;
            }

            if (item.Right > owner.Right)
            {
                owner.Bounds.Width = (int) (owner.Bounds.Width + (item.Right - owner.Right));
            }

            if (item.Bottom > owner.Bottom)
            {
                owner.Bounds.Height = (int)(owner.Bounds.Height + (item.Right - owner.Height));
            }

            base.Add(item);
        }
    }

    

}
