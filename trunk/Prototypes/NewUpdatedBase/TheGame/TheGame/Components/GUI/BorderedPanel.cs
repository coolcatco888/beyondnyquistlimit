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
            //Add Top Left corner
            //panelItems.Add(new ImageComponent2D(parent, position + new Vector2(0.0f, borderCorner.Height), borderCornerBG, bgColor)
            //    {
            //        Rotation = -MathHelper.PiOver2
            //    });
            //panelItems.Add(new ImageComponent2D(parent, position + new Vector2(0.0f, borderCorner.Height), borderCorner, Color.White)
            //    {
            //        Rotation = -MathHelper.PiOver2
            //    });

            ////Add Top Mid Border
            //panelItems.Add(new ImageComponent2D(parent, position + new Vector2(borderCorner.Width, 0.0f), borderBG, bgColor, new Vector2(size.X, 1.0f)));
            //panelItems.Add(new ImageComponent2D(parent, position + new Vector2(borderCorner.Width, 0.0f), border, Color.White, new Vector2(size.X, 1.0f)));

            ////Add Top Right corner
            //panelItems.Add(new ImageComponent2D(parent, position + new Vector2(borderCorner.Width, 0.0f) + new Vector2(size.X, 0.0f), borderCornerBG, bgColor));
            //panelItems.Add(new ImageComponent2D(parent, position + new Vector2(borderCorner.Width, 0.0f) + new Vector2(size.X, 0.0f), borderCorner, Color.White));

            //Add Left Mid Row Border
            //panelItems.Add(new ImageComponent2D(parent, position + new Vector2(borderCorner.Width, 0.0f), borderBG, Color.White, new Vector2(size.X, 1.0f)));
            //panelItems.Add(new ImageComponent2D(parent, position + new Vector2(borderCorner.Width, 0.0f), border, Color.White, new Vector2(size.X, 1.0f)));
        }
    }
}
