using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagement.GameWindows
{
    /// <summary>
    /// Essentially a collection of Components to be drawn with an offset
    /// When implementing menus, have the menu image as the first item added.
    /// 
    /// See MainMenuScreen LoadContent() for how to setup
    /// </summary>
    class PanelComponent2D : Component2D
    {
        private List<Component2D> panelItems = new List<Component2D>();

        public List<Component2D> PanelItems
        {
            get { return panelItems; }
        }

        public PanelComponent2D(Vector2 position)
        {
            this.position = position;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Component2D item in panelItems)
            {
                item.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Component2D item in panelItems)
            {
                Vector2 origninalPos = item.Position;
                item.Position = origninalPos + position;
                item.Draw(gameTime, spriteBatch);
                item.Position = origninalPos;
            }
        }


    }
}
