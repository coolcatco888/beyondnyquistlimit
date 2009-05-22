using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGame.Components.Display;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheGame.Game_Screens
{
    class CharacterSelectScreen : GameScreen
    {
        private InputHub inputHub = (InputHub)GameEngine.Services.GetService(typeof(InputHub));

        private List<CharacterChoice> choices = new List<CharacterChoice>();

        private Texture2D wizardFace, knightFace, priestFace, rangerFace;

        private SpriteFont font;

        private float fontScale = 5.0f;

        public CharacterSelectScreen(string name)
            : base(name)
        {
            wizardFace = GameEngine.Content.Load<Texture2D>("GUI\\itemsm");
            knightFace = wizardFace;
            priestFace = wizardFace;
            rangerFace = wizardFace;

            font = GameEngine.Content.Load<SpriteFont>("GUI\\menufont");
            SetupCharacterChoices();
            SetupCharacterChoosers();
        }

        public void SetupCharacterChoices()
        {
            PanelComponent2D wizard = new PanelComponent2D(this, new Vector2(100, 100));
            PanelComponent2D knight = new PanelComponent2D(this, new Vector2(100, 300));
            PanelComponent2D priest = new PanelComponent2D(this, new Vector2(200, 100));
            PanelComponent2D ranger = new PanelComponent2D(this, new Vector2(200, 300));

            Vector2 position = new Vector2(125, 100);
            Vector2 fontPosition = new Vector2(125, 200);

            wizard.PanelItems.Add(new ImageComponent2D(this, position, wizardFace));
            wizard.PanelItems.Add(new TextComponent2D(this, fontPosition, "Wizard", Color.White, font)
                {
                    Scale = fontScale,
                });

            knight.PanelItems.Add(new ImageComponent2D(this, position, knightFace));
            knight.PanelItems.Add(new TextComponent2D(this, fontPosition, "Knight", Color.White, font)
            {
                Scale = fontScale,
            });

            priest.PanelItems.Add(new ImageComponent2D(this, position, priestFace));
            priest.PanelItems.Add(new TextComponent2D(this, fontPosition, "Priest", Color.White, font)
            {
                Scale = fontScale,
            });

            ranger.PanelItems.Add(new ImageComponent2D(this, position, rangerFace));
            ranger.PanelItems.Add(new TextComponent2D(this, fontPosition, "Ranger", Color.White, font)
            {
                Scale = fontScale,
            });


            CharacterChoice wizardChoice = new CharacterChoice()
            {
                panel = wizard,
                characterName = "Wizard",
            };

            CharacterChoice knightChoice = new CharacterChoice()
            {
                panel = knight,
                characterName = "Knight",
            };

            CharacterChoice pirestChoice = new CharacterChoice()
            {
                panel = priest,
                characterName = "Priest",
            };

            CharacterChoice rangerChoice = new CharacterChoice()
            {
                panel = ranger,
                characterName = "Ranger",
            };
        }

        public void SetupCharacterChoosers()
        {

        }



    }

    class CharacterChoice
    {
        /// <summary>
        /// Stores player image and name
        /// </summary>
        public PanelComponent2D panel;

        public string characterName;

        public CharacterChoice up;

        public CharacterChoice down;

        public CharacterChoice left;

        public CharacterChoice right;

        public bool selected = false;

    }

    class CharacterChooser
    {
        public GamepadDevice gamepad;

        public CharacterChoice currentCharacter;

        public bool selected = false;

        public void HandleInput()
        {
            if (gamepad != null && gamepad.Enabled && currentCharacter != null && !selected)
            {
                if (gamepad.WasButtonPressed(Buttons.LeftThumbstickUp) 
                    && currentCharacter.up != null)
                {
                    currentCharacter = currentCharacter.up;
                }
                else if (gamepad.WasButtonPressed(Buttons.LeftThumbstickDown) 
                    && currentCharacter.down != null)
                {
                    currentCharacter = currentCharacter.down;
                }
                else if (gamepad.WasButtonPressed(Buttons.LeftThumbstickLeft)
                    && currentCharacter.left != null)
                {
                    currentCharacter = currentCharacter.left;
                }
                else if (gamepad.WasButtonPressed(Buttons.LeftThumbstickRight) 
                    && currentCharacter.right != null)
                {
                    currentCharacter = currentCharacter.right;
                }
                else if (gamepad.WasButtonPressed(Buttons.A) 
                    && !currentCharacter.selected)
                {
                    currentCharacter.selected = true;
                    selected = true;
                }
                else if (gamepad.WasButtonPressed(Buttons.B) 
                    && currentCharacter.selected && selected)
                {
                    currentCharacter.selected = false;
                    selected = false;
                }
            }

        }

    }
}
