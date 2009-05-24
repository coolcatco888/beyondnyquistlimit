using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using TheGame.Components.Display;
using Library;

namespace TheGame
{
    public class Player : Actor
    {
        #region Fields - Informational Class Specific Fields

        // Casting aura for this specific player/class
        protected CastingAura castingAura;

        // Class info for the player
        protected CharacterClassInfo classInfo;
        protected float gaugeTimer = 0.0f;

        public CharacterClassInfo ClassInfo
        {
            get { return classInfo; }
        }

        #endregion  // Fields

        #region Fields - Miscellaneous

        // Index used by input to determine what player number/controller is used for this player
        protected PlayerIndex playerIndex;

        public PlayerIndex PlayerIndex
        {
            get { return playerIndex; }
        }

        // Flag for the attack damage timer
        protected bool damageTimerActive;

        // Timer used for a delay in applying damage, to match the frame of the attack swing
        protected float damageTimer;
        protected float damageInterval;
        protected SpellInfo spellInfo;

        #endregion  // Fields

        #region Fields - Flags

        private bool hasAttacked;
        public bool HasAttacked
        {
            get { return hasAttacked; }
            set { hasAttacked = value; }
        }

        protected PlayerInfo playerInfo;

        public PlayerInfo PlayerInfo
        {
            get { return playerInfo; }
        }


        #endregion

        #region TEMPORARY - Testing Fields

        SpriteInfo waveInfo = GameEngine.Content.Load<Library.SpriteInfo>(@"Sprites\\CloudInfo");
        BillboardWave wave;
        Spell currentSpell;
        string spellName = "";

        #endregion

        #region SpellSequenceFields
        // This is the master list of moves in logical order. This array is kept
        // around in order to draw the move list on the screen in this order.
        Move[] spellComboLibrary;
        // The move list used for move detection at runtime.
        MoveList moveList;

        // The move list is used to match against an input manager for each player.
        InputManager inputManager;
        // Stores each players' most recent move and when they pressed it.
        Move currentPlayerMove;
        TimeSpan currentPlayerMoveTime;
        Move previousMove;

        // Time until the currently "active" move dissapears from the screen.
        readonly TimeSpan MoveTimeOut = TimeSpan.FromSeconds(1.0);

        SpriteFont font;
        #endregion

        #region Constructor

        public Player(GameScreen parent, SpriteInfo spriteInfo, PlayerIndex playerIndex, string className, Vector3 scale)
            : base(parent, spriteInfo, new Vector3(0.0f, 2.0f, 0.0f), Vector3.Zero, scale)
        {
            font = GameEngine.Content.Load<SpriteFont>("GUI\\menufont");
            

            this.playerIndex = playerIndex;
            string classInfoFile = className + "ClassInfo";
            SetupComboLibraryAndInputManager();

            hasAttacked = false;
            classInfo = GameEngine.Content.Load<Library.CharacterClassInfo>(@classInfoFile);
            actorStats = new ActorInfo();
            playerInfo = new PlayerInfo(className);//TODO: new PlayerInfo() should be loaded from xml
            actorStats.PopulateFields(playerInfo, classInfo);
        }

        #endregion // Constructor

        #region Initialize

        public override void Initialize()
        {
            type = ObjectType.Player;

            spellInfo = new SpellInfo();
            spellInfo.Caster = this.playerIndex;
            spellInfo.Duration = -1.0f;

            castingAura = new CastingAura(this.Parent, spellInfo, new Point(0, 0));
            castingAura.Initialize();
            castingAura.Enabled = false;

            // Initialize the movement seqences
            InitializeSpriteSequences();

            // Initialize the bounding shapes
            InitializeBoundingShapes();

            base.Initialize();
        }

        #endregion // Initialize

        #region Update

        public override void Update(GameTime gameTime)
        {
            gaugeTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (gaugeTimer >= 200.0f && playerInfo.CurrentAttackGauge < 100)
            {
                playerInfo.CurrentAttackGauge += 10;
                gaugeTimer = 0;
            }
            previousState = state;

            UpdatePosition(gameTime);

            HandleInput(gameTime);

            base.Update(gameTime);
        }

        #endregion

        #region Draw

        // TESTING PURPOSES ONLY - used only to test the positions of the shapes
        // Draws the bounding shape
        public override void Draw(GameTime gameTime)
        {
            boundingShapesSelf[state.ToString() + orientation.ToString()].Draw(primitiveBatch);
            primitiveShape.Draw(primitiveBatch);
            base.Draw(gameTime);
        }
        // End Testing

        #endregion // Draw

        #region Update Methods

        public void UpdateOrientation() // Do not touch ... complete
        {
            Orientation previousOrientation = orientation;
            if (direction.Z > 0)
            {
                if (direction.X == 0)
                    orientation = Orientation.North;
                if (direction.X > 0)
                    orientation = Orientation.Northeast;
                if (direction.X < 0)
                    orientation = Orientation.Northwest;
            }
            else if (direction.Z < 0)
            {
                if (direction.X == 0)
                    orientation = Orientation.South;
                if (direction.X > 0)
                    orientation = Orientation.Southeast;
                if (direction.X < 0)
                    orientation = Orientation.Southwest;
            }
            else if (direction.X > 0)
            {
                orientation = Orientation.East;
            }
            else if (direction.X < 0)
            {
                orientation = Orientation.West;
            }
            // Buffer orientation transitions.
            switch (previousOrientation)
            {
                case Orientation.North:
                    if ((orientation == Orientation.Northeast && direction.X < 0.1f) ||
                        (orientation == Orientation.Northwest && direction.X > -0.1f))
                        orientation = previousOrientation;
                    break;
                case Orientation.Northeast:
                    if ((orientation == Orientation.East && direction.X < 0.9f) ||
                        (orientation == Orientation.North && direction.Z < 0.9f))
                        orientation = previousOrientation;
                    break;
                case Orientation.East:
                    if ((orientation == Orientation.Southeast && direction.Z > -0.1f) ||
                        (orientation == Orientation.Northeast && direction.Z < 0.1f))
                        orientation = previousOrientation;
                    break;
                case Orientation.Southeast:
                    if ((orientation == Orientation.South && direction.Z > -0.9f) ||
                        (orientation == Orientation.East && direction.X < 0.9f))
                        orientation = previousOrientation;
                    break;
                case Orientation.South:
                    if ((orientation == Orientation.Southeast && direction.X < 0.1f) ||
                        (orientation == Orientation.Southwest && direction.X > -0.1f))
                        orientation = previousOrientation;
                    break;
                case Orientation.Southwest:
                    if ((orientation == Orientation.West && direction.X > -0.9f) ||
                        (orientation == Orientation.South && direction.Z > -0.9f))
                        orientation = previousOrientation;
                    break;
                case Orientation.West:
                    if ((orientation == Orientation.Southwest && direction.Z > -0.1f) ||
                        (orientation == Orientation.Northwest && direction.Z < 0.1f))
                        orientation = previousOrientation;
                    break;
                case Orientation.Northwest:
                    if ((orientation == Orientation.North && direction.Z < 0.9f) ||
                        (orientation == Orientation.West && direction.X > -0.9f))
                        orientation = previousOrientation;
                    break;
            }
        }

        #endregion // Update Methods

        #region Input Methods
        /// <summary>
        /// Update the controller.  By that we mean update the 
        /// player's orientation based on controller input
        /// </summary>
        /// <param name="gameTime">The game time</param>
        private void HandleInput(GameTime gameTime)
        {
            GamepadDevice gamepadDevice = ((InputHub)GameEngine.Services.GetService(typeof(InputHub)))[playerIndex];
            KeyboardDevice keyboardDevice = (KeyboardDevice)GameEngine.Services.GetService(typeof(KeyboardDevice));
            
            switch (previousState)
            {
                case ActorState.Idle:
                    IdleStateInput(gamepadDevice);
                    break;
                case ActorState.Walking:
                    WalkingStateInput(gamepadDevice);
                    break;
                case ActorState.Running:
                    RunningStateInput(gamepadDevice);
                    break;
                case ActorState.Attacking:
                    AttackingStateInput(gamepadDevice);
                    break;
                case ActorState.Chanting:
                    ChantingStateInput(gamepadDevice);
                    break;
                case ActorState.Casting:
                    CastingStateInput(gamepadDevice);
                    break;
                case ActorState.Dead:
                    ActorList players = ((Level)Parent).PlayerList;
                    players.Remove((Player)this);
                    this.Dispose();
                    break;
            }
        }

        private void CastingStateInput(GamepadDevice gamepadDevice)
        {
            if (!spellName.Equals(""))
            {
                CreateSpell(spellName);
            }
            if (currentSequence.IsComplete)
            {
                state = ActorState.Idle;
                spellName = "";
            }
        }

        /// <summary>
        /// Input state changes while in the chanting actor state
        /// </summary>
        private void ChantingStateInput(GamepadDevice gamepadDevice)
        {
            //DO INPUT SEQUENCE STUFF HERE
            HandleComboMove();

            speed = 0.0f;
            if (gamepadDevice.WasButtonReleased(Buttons.RightTrigger))
            {
                state = ActorState.Casting;
            }
        }

        /// <summary>
        /// Input state changes while in the attacking actor state
        /// </summary>
        private void AttackingStateInput(GamepadDevice gamepadDevice)
        {
            speed = 0.0f;

            foreach (Monster m in ((Level)Parent).MonsterList)
            {
                if (IsHit(m.PrimitiveShape))
                {
                    target = m;
                }
            }

            if (currentSequence.Title == "Attacking" && ((currentSequence.CurrentFrame.X == 4
                 || currentSequence.CurrentFrame.X == 12)) && !hasAttacked)
            {
                if(target != null)
                {
                    target.GetHit((int)((float) actorStats.CurrentDamage * (float)playerInfo.CurrentAttackGauge / (float)playerInfo.MaxAttackGauge), 
                        orientation, 0.5f);
                    this.hasAttacked = true;
                }
                playerInfo.CurrentAttackGauge = 0;
            }

            if (currentSequence.IsComplete)
            {
                hasAttacked = false;
                target = null;
                state = ActorState.Idle;
            }
        }

        /// <summary>
        /// Input state changes while in the running actor state
        /// </summary>
        private void RunningStateInput(GamepadDevice gamepadDevice)
        {
            direction = new Vector3(gamepadDevice.LeftStickPosition.X, 0.0f, gamepadDevice.LeftStickPosition.Y);
            UpdateOrientation();
            speed = 0.02f;

            if (gamepadDevice.LeftStickPosition == Vector2.Zero)
            {
                state = ActorState.Idle;
            }
            else if (gamepadDevice.WasButtonPressed(Buttons.B))
            {
                state = ActorState.Attacking;
            }
            else if (gamepadDevice.IsButtonDown(Buttons.RightTrigger))
            {
                state = ActorState.Chanting;
            }
            else if (gamepadDevice.IsButtonUp(Buttons.A))
            {
                state = ActorState.Walking;
            }
        }

        /// <summary>
        /// Input state changes while in the walking actor state
        /// </summary>
        private void WalkingStateInput(GamepadDevice gamepadDevice)
        {
            direction = new Vector3(gamepadDevice.LeftStickPosition.X, 0.0f, gamepadDevice.LeftStickPosition.Y);
            UpdateOrientation();
            speed = 0.005f;

            if (gamepadDevice.LeftStickPosition == Vector2.Zero)
            {
                state = ActorState.Idle;
            }
            else if (gamepadDevice.WasButtonPressed(Buttons.B))
            {
                state = ActorState.Attacking;
            }
            else if (gamepadDevice.IsButtonDown(Buttons.RightTrigger))
            {
                state = ActorState.Chanting;
            }
            else if (gamepadDevice.IsButtonDown(Buttons.A))
            {
                state = ActorState.Running;
            }
        }

        /// <summary>
        /// Input state changes while in the Idle actor state
        /// </summary>
        private void IdleStateInput(GamepadDevice gamepadDevice)
        {
            speed = 0.0f;
            if (gamepadDevice.LeftStickPosition != Vector2.Zero)
            {
                if (gamepadDevice.IsButtonDown(Buttons.A))

                    state = ActorState.Running;
                else
                    state = ActorState.Walking;
            }
            else if (gamepadDevice.WasButtonPressed(Buttons.B))
            {
                state = ActorState.Attacking;
            }
            else if (gamepadDevice.IsButtonDown(Buttons.RightTrigger))
            {
                state = ActorState.Chanting;
            }
        }

        #endregion // Input Methods

        #region Initialization Methods

        // Initialize the sprite sequences
        // Reads in from a XML file.
        protected void InitializeSpriteSequences()
        {
            List<SpriteSequenceInfo> sequenceInfo = GameEngine.Content.Load<List<SpriteSequenceInfo>>(@"PlayerSpriteSequences");
            SpriteSequence sequence;

            foreach (SpriteSequenceInfo info in sequenceInfo)
            {
                Orientation o = Utility.GetOrientationFromString(info.OrientationKey);

                if (!info.ChangeScale)
                {
                    sequence = new SpriteSequence(info.StateKey, o, info.IsLoop, info.SequenceVelocity,
                        info.NumBuffers);
                }
                else
                {
                    sequence = new SpriteSequence(info.StateKey, o, info.IsLoop, info.SequenceVelocity,
                        info.NumBuffers, info.ScaleX, info.ScaleY);
                }

                if (!info.IsARowOrColumn)
                {
                    sequence.AddFrame(info.IndexX, info.IndexY);
                }
                else
                {
                    sequence.AddRow(info.RowOrColumn, info.IndexX, info.IndexY);
                }
                sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);
            }

            currentSequence = sequences["IdleSouth"];
        }

        // TESTING PURPOSES ONLY - need to fix/move etc... you know the drill
        // Initializes the damage timer information
        public void InitializeDamageTimer()
        {
            damageTimerActive = false;
            damageTimer = 0.0f;
            damageInterval = 5000.0f;
        }
        // End Testing

        private void InitializeBoundingShapes()
        {
            List<BoundingShapeInfo> shapesInfo = GameEngine.Content.Load<List<BoundingShapeInfo>>(@"PlayerBoundingShapes");

            foreach (BoundingShapeInfo info in shapesInfo)
            {
                if (info.StateKey == "Idle")
                {
                    boundingShapesSelf[info.StateKey + info.OrientationKey] = new PrimitiveShape(position, new Vector2(scale.X, scale.Y), info.Verts);
                    boundingShapesSelf[info.StateKey + info.OrientationKey].ShapeColor = Color.Aqua;

                    boundingShapesSelf["Walking" + info.OrientationKey] = new PrimitiveShape(position, new Vector2(scale.X, scale.Y), info.Verts);
                    boundingShapesSelf["Walking" + info.OrientationKey].ShapeColor = Color.Magenta;

                    boundingShapesSelf["Running" + info.OrientationKey] = new PrimitiveShape(position, new Vector2(scale.X, scale.Y), info.Verts);
                    boundingShapesSelf["Running" + info.OrientationKey].ShapeColor = Color.Ivory;

                    boundingShapesSelf["Chanting" + info.OrientationKey] = new PrimitiveShape(position, new Vector2(scale.X, scale.Y), info.Verts);
                    boundingShapesSelf["Chanting" + info.OrientationKey].ShapeColor = Color.Gold;
                    boundingShapesSelf["Casting" + info.OrientationKey] = new PrimitiveShape(position, new Vector2(scale.X, scale.Y), info.Verts);
                    boundingShapesSelf["Casting" + info.OrientationKey].ShapeColor = Color.Gold;
                }
                else if (info.StateKey == "Others")
                {
                    primitiveShape = new PrimitiveShape(position, new Vector2(scale.X, scale.Y), info.Verts);
                }
                else
                {
                    boundingShapesSelf[info.StateKey + info.OrientationKey] = new PrimitiveShape(position, new Vector2(scale.X, scale.Y), info.Verts);
                    boundingShapesSelf[info.StateKey + info.OrientationKey].ShapeColor = Color.Black;
                }
            }
        }

        #endregion // Initialization Methods

        #region Spell Input Sequence
        /// <summary>
        /// Example of how to setup the Combo Sequence Functionality
        /// </summary>
        private void SetupComboLibraryAndInputManager()
        {
            // Construct the master list of moves.
            spellComboLibrary = new Move[]
                {
                    new Move("Chain Beam", Buttons.DPadUp, Buttons.A),
                    new Move("Activate Spell",  Buttons.A,  Buttons.X,  Buttons.Y,  Buttons.B),
                };

            // Construct a move list which will store its own copy of the moves array.
            moveList = new MoveList(spellComboLibrary);

            // Create an InputManager for each player with a sufficiently large buffer.
            inputManager = new InputManager(playerIndex, moveList.LongestMoveLength);
            //for (int i = 0; i < inputManagers.Length; ++i)
            //{
            //    inputManagers[i] =
            //        new InputManager((PlayerIndex)i, moveList.LongestMoveLength);
            //}

            // Give each player a location to store their most recent move.
            //playerMoves = new Move[inputManagers.Length];
            //prevMoves = new Move[inputManagers.Length];
            //playerMoveTimes = new TimeSpan[inputManagers.Length];
        }

        /// <summary>
        /// Example of keeping track of Combo Moves
        /// </summary>
        private void HandleComboMove()
        {

            // Expire old moves.
            if (GameEngine.GameTime.TotalRealTime - currentPlayerMoveTime > MoveTimeOut)
            {
                currentPlayerMove = null;
            }

            // Get the updated input manager.
            inputManager.Update(GameEngine.GameTime);

            // Detection and record the current player's most recent move.
            Move newMove = moveList.DetectMove(inputManager);
            if (newMove != null)
            {
                if (newMove != previousMove)
                {
                    //HitTextComponent2D text = new HitTextComponent2D(Parent, new Vector2(400, 400), newMove.Name, Color.Red, font, 2.0f);
                    //text.Initialize();
                    spellName = newMove.Name;
                    previousMove = newMove;
                }
                else
                {
                    previousMove = newMove;
                }

                currentPlayerMove = newMove;
                currentPlayerMoveTime = GameEngine.GameTime.TotalRealTime;
            }
            
        }

        private void CreateSpell(string spellName)
        {
            switch (spellName)
            {
                case "Chain Beam":
                    currentSpell = new ChainBeam(this.Parent, 500.0f);
                    break;
                case "Fire Tornado":

                    break;
                case "Fire Line":

                    break;
                case "Healing":

                    break;
            }
        }

        #endregion
    }
}
