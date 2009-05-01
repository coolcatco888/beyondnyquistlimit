using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WTFJimGameProject.GameWindows
{
    class MenuPanel2D : PanelComponent2D
    {

        private PanelComponent2D panel;

        private int startIndex, endIndex, currentIndex;

        public int StartIndex
        {
            set { startIndex = value; }
            get { return startIndex; }
        }

        public int EndIndex
        {
            set { endIndex = value; }
            get { return endIndex; }
        }

        public int CurrentIndex
        {
            set { currentIndex = value; }
            get { return currentIndex; }
        }

        private MenuPanel2D(GameScreen parent, Vector2 position)
            : base(parent, position)
        {
        }

        public static MenuPanel2D CreateMenuPanel2D(PanelComponent2D panel, int startIndex, int endIndex)
        {
            MenuPanel2D newPanel = new MenuPanel2D(panel.Parent, panel.Position);


            return newPanel;
        }


    }
}
