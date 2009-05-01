using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WTFJimGameProject.GameWindows;

namespace WTFJimGameProject
{
    abstract class MenuScreen : GameScreen
    {
        protected MenuPanel2D menu;
        
        public MenuScreen(string name, ScreenManager owner) : base(name, owner) 
        { 
        }
    }
}
