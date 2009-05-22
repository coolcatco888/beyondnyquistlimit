﻿using System;
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

        private float fontScale = 2.0f;

        private CharacterChoice wizardChoice, knightChoice, priestChoice, rangerChoice;

        private List<CharacterChooser> playerChoosers = new List<CharacterChooser>();

        private List<ImageComponent2D> characterSelectors = new List<ImageComponent2D>();

        public CharacterSelectScreen(string name)
            : base(name)
        {

            Texture2D gradient = GameEngine.Content.Load<Texture2D>("GUI\\healthbar");
            Vector2 scale = new Vector2((float)GameEngine.Graphics.Viewport.Width / (float)gradient.Width, 
                (float)GameEngine.Graphics.Viewport.Height / (float)gradient.Height);

            (new ImageComponent2D(this, Vector2.Zero, gradient, Color.CornflowerBlue, scale)).Initialize();

            wizardFace = GameEngine.Content.Load<Texture2D>("GUI\\itemsm");
            knightFace = wizardFace;
            priestFace = wizardFace;
            rangerFace = wizardFace;

            font = GameEngine.Content.Load<SpriteFont>("GUI\\menufont");
            (new TextComponent2D(this, new Vector2(200, 30), "Choose Your Warrior", Color.White, font)
                {
                    Scale = fontScale + 1.0f
                }).Initialize();
            (new TextComponent2D(this, new Vector2(250, 500), "Push Start to Play", Color.White, font)
            {
                Scale = fontScale
            }).Initialize();
            LoadCharacterSelectorImages();
            SetupCharacterChoices();
            SetupCharacterChoosers();
        }

        public void LoadCharacterSelectorImages()
        {
            for (int i = 1; i < 5; i++)
            {
                ImageComponent2D selector = new ImageComponent2D(this, Vector2.Zero,
                    GameEngine.Content.Load<Texture2D>("GUI\\charsel" + (5 - i)));
                characterSelectors.Add(selector);
            }
        }

        public void SetupCharacterChoices()
        {
            PanelComponent2D wizard = new PanelComponent2D(this, new Vector2(150, 100));
            PanelComponent2D knight = new PanelComponent2D(this, new Vector2(450, 100));
            PanelComponent2D priest = new PanelComponent2D(this, new Vector2(150, 300));
            PanelComponent2D ranger = new PanelComponent2D(this, new Vector2(450, 300));

            Vector2 position = new Vector2(75, 50);
            Vector2 fontPosition = new Vector2(50, 100);

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

            wizard.Initialize();
            knight.Initialize();
            priest.Initialize();
            ranger.Initialize();

            wizardChoice = new CharacterChoice()
            {
                panel = wizard,
                characterName = "Wizard",
            };

            knightChoice = new CharacterChoice()
            {
                panel = knight,
                characterName = "Knight",
            };

            priestChoice = new CharacterChoice()
            {
                panel = priest,
                characterName = "Priest",
            };

            rangerChoice = new CharacterChoice()
            {
                panel = ranger,
                characterName = "Ranger",
            };

            wizardChoice.right = knightChoice;
            wizardChoice.down = priestChoice;

            knightChoice.left = wizardChoice;
            knightChoice.down = rangerChoice;

            priestChoice.up = wizardChoice;
            priestChoice.right = rangerChoice;

            rangerChoice.left = priestChoice;
            rangerChoice.up = knightChoice;

            choices.Add(wizardChoice);
            choices.Add(knightChoice);
            choices.Add(priestChoice);
            choices.Add(rangerChoice);

        }

        public void SetupCharacterChoosers()
        {
           
            for(int i = 0; i < 4; i++)
            {
                if (inputHub[(PlayerIndex)i].IsConnected)
                {
                    CharacterChooser chooser = new CharacterChooser();
                    chooser.gamepad = inputHub[(PlayerIndex)i];
                    chooser.currentCharacter = choices.ElementAt(i);
                    ImageComponent2D selector = characterSelectors.ElementAt(characterSelectors.Count - 1 - i);
                    selector.Position = chooser.currentCharacter.panel.Position;
                    selector.Initialize();
                    chooser.selector = selector;
                    playerChoosers.Add(chooser);
                }
                
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (CharacterChooser chooser in playerChoosers)
            {
                chooser.HandleInput();
            }
            base.Update(gameTime);
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

        public ImageComponent2D selector;

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
                    TintOrUnTintItems(currentCharacter.panel.PanelItems, true);
                    currentCharacter.selected = true;
                    selected = true;
                }
                selector.Position = currentCharacter.panel.Position;
            }
            else if (gamepad != null && gamepad.Enabled && currentCharacter != null && gamepad.WasButtonPressed(Buttons.B)
                    && currentCharacter.selected && selected)
            {
                TintOrUnTintItems(currentCharacter.panel.PanelItems, false);
                currentCharacter.selected = false;
                selected = false;
            }

        }

        private void TintOrUnTintItems(PanelComponents items, bool tint)
        {
            float alpha = tint ? 0.5f : 1.0f;
            foreach (DisplayComponent2D component in items)
            {
                if (component is ImageComponent2D)
                {
                    ((ImageComponent2D)component).Tint = new Color(((ImageComponent2D)component).Tint, alpha);
                }
                else if (component is TextComponent2D)
                {
                    ((TextComponent2D)component).Color = new Color(((TextComponent2D)component).Color, alpha);
                }
            }
        }

    }
}
