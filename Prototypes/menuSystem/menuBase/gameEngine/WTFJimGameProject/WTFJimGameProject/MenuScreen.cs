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

        /// <summary>
        /// Handles the input for switching the menu item only.
        /// Any overrides must first make a call to this base method.
        /// </summary>
        /// <param name="input"></param>
        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);

            // Move to the previous menu entry?
            if (input.IsMenuUp(ControllingPlayer))
            {
                menu.Next();
            }

            if (input.IsMenuDown(ControllingPlayer))
            {
                menu.Previous();
            }
        }



    }
}
