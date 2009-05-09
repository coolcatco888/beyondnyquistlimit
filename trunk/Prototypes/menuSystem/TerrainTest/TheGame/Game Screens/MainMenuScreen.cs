using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGame.Components.Display;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame.Game_Screens
{
    class MainMenuScreen : MenuScreen
    {

        private ContentManager content;

        private GameWeaponMenuPanel2D weaponPanel;

        private Texture2D item;

        private SpriteFont font;

        #region ComboSequenceFields
        // This is the master list of moves in logical order. This array is kept
        // around in order to draw the move list on the screen in this order.
        Move[] moves;
        // The move list used for move detection at runtime.
        MoveList moveList;

        // The move list is used to match against an input manager for each player.
        InputManager[] inputManagers;
        // Stores each players' most recent move and when they pressed it.
        Move[] playerMoves;
        TimeSpan[] playerMoveTimes;
        Move[] prevMoves;

        // Time until the currently "active" move dissapears from the screen.
        readonly TimeSpan MoveTimeOut = TimeSpan.FromSeconds(1.0);
        #endregion

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent()
        {
            if (content == null)
                content = GameEngine.Content;

            //Build the menu from XML
            XMLPanel2DBuilder componentBuilder = new XMLPanel2DBuilder(this, content, "MenuPanels\\mainpanel.xml");

            item = content.Load<Texture2D>("itemsm");
            font = content.Load<SpriteFont>("menufont");


            //Make it into functional menu
            menu = MenuPanel2D.CreateMenuPanel2D(componentBuilder.Panel, 1, 3);

            //Add to drawable components
            Components.Add(menu);
            new TextComponent2D(this, new Vector2(50, 400), "Combos:\nX to Punch\nA to Jump\nA, X, Y and B to Activate Spell", Color.White, font, 0.75f);
        }

        private void CreateWeaponPanel()
        {
            weaponPanel = new GameWeaponMenuPanel2D(this, new Vector2(500, 450));
            weaponPanel.AddWeapon("Sword1", new ImageComponent2D(this, Vector2.Zero, item));
            weaponPanel.AddWeapon("Sword2", new ImageComponent2D(this, Vector2.Zero, item));
            weaponPanel.AddWeapon("Sword3", new ImageComponent2D(this, Vector2.Zero, item));
            weaponPanel.AddWeapon("Sword4", new ImageComponent2D(this, Vector2.Zero, item));
            weaponPanel.UpdateItemPositions();
        }

        public MainMenuScreen(string name)
            : base(name)
        {
            //See Method Definition
            SetupComboLibraryAndInputManager();

        }

        /// <summary>
        /// Example of how to setup the Combo Sequence Functionality
        /// </summary>
        private void SetupComboLibraryAndInputManager()
        {
            // Construct the master list of moves.
            moves = new Move[]
                {
                    new Move("Jump",        Buttons.A) { IsSubMove = true },
                    new Move("Punch",       Buttons.X) { IsSubMove = true },
                    new Move("Activate Spell", Buttons.A, Buttons.X, Buttons.Y, Buttons.B),
                };

            // Construct a move list which will store its own copy of the moves array.
            moveList = new MoveList(moves);

            // Create an InputManager for each player with a sufficiently large buffer.
            inputManagers = new InputManager[2];
            for (int i = 0; i < inputManagers.Length; ++i)
            {
                inputManagers[i] =
                    new InputManager((PlayerIndex)i, moveList.LongestMoveLength);
            }

            // Give each player a location to store their most recent move.
            playerMoves = new Move[inputManagers.Length];
            prevMoves = new Move[inputManagers.Length];
            playerMoveTimes = new TimeSpan[inputManagers.Length];
        }

        protected override void HandleInput()
        {
            base.HandleInput();

            //See indidual method comments for examples of intended usage
            HandleMenuSelection();
            HandleWeaponMenuPanel();
            HandleComboMoves();
            
            
        }

        /// <summary>
        /// Example of how to handle displaying the weapon menu panel
        /// </summary>
        private void HandleWeaponMenuPanel()
        {
            if (gamepadDevice.IsButtonDown(Buttons.RightShoulder))
            {
                if (weaponPanel == null)
                {
                    CreateWeaponPanel();
                    Components.Add(weaponPanel);
                }
                if (weaponPanel != null)
                {
                    string selected = weaponPanel.SelectNewWeapon(gamepadDevice.LeftStickPosition);
                }
            }
            else if (weaponPanel != null)
            {
                weaponPanel.KillMenu();
                weaponPanel = null;
            }
        }

        /// <summary>
        /// Example of how to handle menu selections
        /// </summary>
        private void HandleMenuSelection()
        {
            if (keyboardDevice.WasKeyPressed(Keys.Enter) || gamepadDevice.IsButtonDown(Buttons.A))
            {
                switch (menu.GetCurrentText())
                {
                    case "Start Game":
                        GameEngine.GameScreens.Remove(this);
                        new Level("test", "Terrain\\terrain");
                        break;
                    case "Exit Game":
                        GameEngine.Game.Exit();
                        break;
                }
            }
        }

        /// <summary>
        /// Example of keeping track of Combo Moves
        /// </summary>
        private void HandleComboMoves()
        {
            for (int i = 0; i < inputManagers.Length; ++i)
            {
                // Expire old moves.
                if (GameEngine.GameTime.TotalRealTime - playerMoveTimes[i] > MoveTimeOut)
                {
                    playerMoves[i] = null;
                }

                // Get the updated input manager.
                InputManager inputManager = inputManagers[i];
                inputManager.Update(GameEngine.GameTime);

                // Detection and record the current player's most recent move.
                Move newMove = moveList.DetectMove(inputManager);
                if (newMove != null)
                {
                    if (newMove != prevMoves[i])
                    {
                        new HitTextComponent2D(this, new Vector2(400, 400), newMove.Name, Color.Red, font);
                        prevMoves[i] = newMove;
                    }
                    else
                    {
                        prevMoves[i] = newMove;
                    }

                    playerMoves[i] = newMove;
                    playerMoveTimes[i] = GameEngine.GameTime.TotalRealTime;

                }
            }
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            content.Unload();
        }
    }
}
