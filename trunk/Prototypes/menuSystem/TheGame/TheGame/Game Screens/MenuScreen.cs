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
        protected InputHub inputHub = (InputHub)GameEngine.Services.GetService(typeof(InputHub));
        protected GamepadDevice gamepadDevice;

        public override void  LoadContent()
        {
            foreach (DisplayComponent2D component in menu.PanelItems)
            {
                if (component is MenuTextComponent2D)
                {
                    ((MenuTextComponent2D)component).Scale = 2.5f;
                }
            }
            base.LoadContent();
        }

        /// <summary>
        /// Creates a new MenuScreen
        /// </summary>
        /// <param name="name">Identifier to reference the screen</param>
        protected MenuScreen(string name)
            : base(name)
        {
            gamepadDevice = inputHub[inputHub.MasterInput];
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
            if (keyboardDevice != null && keyboardDevice.WasKeyPressed(prev) || gamepadDevice != null && gamepadDevice.WasButtonPressed(Buttons.LeftThumbstickUp))
            {
                menu.Previous();
            }

            if (keyboardDevice != null && keyboardDevice.WasKeyPressed(next) || gamepadDevice != null && gamepadDevice.WasButtonPressed(Buttons.LeftThumbstickDown))
            {
                menu.Next();
            }
        }
    }
}
