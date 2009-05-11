using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGame.Components.Display;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TheGame.Game_Screens
{
    /// <summary>
    /// Represents a menu screen with one menu
    /// </summary>
    abstract class MenuScreen : GameScreen
    {
        /// <summary>
        /// You can change the keys for selecting next and previous menu items
        /// </summary>
        protected Keys prev = Keys.Up,
            next = Keys.Down;
        protected MenuPanel2D menu = null;
        protected KeyboardDevice keyboardDevice = (KeyboardDevice)GameEngine.Services.GetService(typeof(KeyboardDevice));
        protected GamepadDevice gamepadDevice = (GamepadDevice)GameEngine.Services.GetService(typeof(GamepadDevice));

        /// <summary>
        /// Creates a new MenuScreen
        /// </summary>
        /// <param name="name">Identifier to reference the screen</param>
        protected MenuScreen(string name)
            : base(name)
        {

        }

        /// <summary>
        /// Updates the menu items
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {

            HandleInput();
            base.Update(gameTime);
        }

        /// <summary>
        /// Handles the selection and interation through menu items only
        /// </summary>
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
