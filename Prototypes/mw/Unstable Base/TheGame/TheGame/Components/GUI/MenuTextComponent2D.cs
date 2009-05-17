using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame.Components.Display
{

    /// <summary>
    /// Represents a rich text component that when selected, highlights itself with a specified highlight color.
    /// </summary>
    class MenuTextComponent2D : TextComponent2D
    {

        private Color selectedColor;

        private bool selected;

        public Color SelectedColor
        {
            set { selectedColor = value; }
            get { return selectedColor; }
        }

        public bool Selected
        {
            set { selected = value; }
            get { return selected; }
        }


        /// <summary>
        /// Constructs the menu text component.
        /// </summary>
        /// <param name="parent">Game screen this object is displayed on</param>
        /// <param name="position">Position the text will display at</param>
        /// <param name="text">Text to be displayed</param>
        /// <param name="color">Color of text</param>
        /// <param name="font">Font of text</param>
        /// <param name="selectedColor">Color when selected</param>
        /// <param name="selected">True to display with selected color, false to display with normal color</param>
        public MenuTextComponent2D(GameScreen parent, Vector2 position, string text, Color color, SpriteFont font, Color selectedColor, bool selected)
            : base(parent, position, text, color, font)
        {
            this.selectedColor = selectedColor;
            this.selected = selected;
        }

        public override void Draw(GameTime gameTime)
        {
            Color drawColor = selected ? selectedColor : color;
            spriteBatch.Begin();
            spriteBatch.DrawString(font, text, position, drawColor);
            spriteBatch.End();
        }

        public static MenuTextComponent2D CreateMenuTextComponent2D(TextComponent2D text, Color selectedColor, bool selected)
        {
            MenuTextComponent2D newText = new MenuTextComponent2D(text.Parent, 
                text.Position, text.Text, text.Color, text.Font, selectedColor, selected);
            return newText;
        }
    }
}
