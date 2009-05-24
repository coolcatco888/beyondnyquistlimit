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
        protected GamepadDevice masterInput = null;
        protected List<GamepadDevice> allGamePads = new List<GamepadDevice>();

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
            for (int i = 0; i < 4; i++)
            {
                allGamePads.Add(inputHub[(PlayerIndex)i]);
            }
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
            foreach (GamepadDevice pad in allGamePads)
            {
                GamepadDevice currentPad = masterInput == null ? pad : masterInput;
                if (keyboardDevice != null && keyboardDevice.WasKeyPressed(prev) || currentPad != null && currentPad.WasButtonPressed(Buttons.LeftThumbstickUp))
                {
                    menu.Previous();
                    break;
                }

                if (keyboardDevice != null && keyboardDevice.WasKeyPressed(next) || currentPad != null && currentPad.WasButtonPressed(Buttons.LeftThumbstickDown))
                {
                    menu.Next();
                    break;
                }
            }
        }
    }
}
