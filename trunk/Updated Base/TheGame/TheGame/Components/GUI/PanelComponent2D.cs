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

        public BoundingBox Bounds;

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
            Vector3 position3D = new Vector3(position.X, position.Y, 0.0f);
            this.Bounds = new BoundingBox(position3D, position3D);
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
            get { return Bounds.Max.Y - Bounds.Min.Y;  }
        }

        public override float Width
        {
            get { return Bounds.Max.X - Bounds.Min.X; }
        }

        public override float Left
        {
            get { return  Bounds.Min.X; }
        }

        public override float Right
        {
            get { return  Left + Width; }
        }

        public override float Bottom
        {
            get { return Top + Height; }
        }

        public override float Top
        {
            get { return Bounds.Min.Y; }
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
            UpdateBounds(item);

            base.Add(item);
        }

        private void UpdateBounds(DisplayComponent2D item)
        {
            if (item.Left < owner.Left)
            {
                owner.Bounds.Min.X = item.Position.X;
            }
            if (item.Top < owner.Top)
            {
                owner.Bounds.Min.Y = item.Position.Y;
            }

            if (item.Right > owner.Right)
            {
                owner.Bounds.Max.X = (owner.Right + owner.Width + (item.Right - owner.Right));
            }

            if (item.Bottom > owner.Bottom)
            {
                owner.Bounds.Max.Y = (owner.Top + owner.Height + (item.Bottom - owner.Bottom));
            }
        }
    }

    

}
