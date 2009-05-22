using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGame.Components.Display;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame.Components.GUI
{
    class BorderedPanel : PanelComponent2D
    {
        public BorderedPanel(GameScreen parent, Vector2 position, 
            Texture2D border, Texture2D borderCorner, Texture2D borderBG, Texture2D borderCornerBG, 
            Texture2D bg, Vector2 size, Color bgColor)
            : base(parent, position)
        {

        }
    }
}
