using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGame.Components.Display;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TheGame.Game_Screens
{
    abstract class MenuScreen : GameScreen
    {
        protected Keys prev = Keys.Up,
            next = Keys.Down;
        protected MenuPanel2D menu = null;
        protected KeyboardDevice keyboardDevice = (KeyboardDevice)GameEngine.Services.GetService(typeof(KeyboardDevice));
        protected GamepadDevice gamepadDevice = (GamepadDevice)GameEngine.Services.GetService(typeof(GamepadDevice));

        protected MenuScreen(string name)
            : base(name)
        {

        }

        public override void Update()
        {

            HandleInput();
            base.Update();
        }

        protected virtual void HandleInput()
        {
            if (keyboardDevice.WasKeyPressed(prev) || gamepadDevice.WasButtonPressed(Buttons.LeftThumbstickUp))
            {
                menu.Previous();
            }

            if (keyboardDevice.WasKeyPressed(next) || gamepadDevice.WasButtonPressed(Buttons.LeftThumbstickDown))
            {
                menu.Next();
            }
        }
    }
}
