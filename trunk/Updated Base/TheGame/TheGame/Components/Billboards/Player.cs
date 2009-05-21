using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
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

        #endregion  // Fields

        #region Fields - Miscellaneous

        // Index used by input to determine what player number/controller is used for this player
        protected PlayerIndex playerIndex;

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

        #endregion

        #region TEMPORARY - Testing Fields

        SpriteInfo waveInfo = GameEngine.Content.Load<Library.SpriteInfo>(@"Sprites\\CloudInfo");
        BillboardWave wave;

        #endregion

        #region Constructor

        public Player(GameScreen parent, SpriteInfo spriteInfo, PlayerIndex playerIndex, string className)
            : base(parent, spriteInfo, new Vector3(0.0f, 2.0f, 0.0f), Vector3.Zero, new Vector3(1.0f, 2.0f, 1.0f))
        {
            this.playerIndex = playerIndex;
            string classInfoFile = className + "ClassInfo";

            hasAttacked = false;
            classInfo = GameEngine.Content.Load<Library.CharacterClassInfo>(@classInfoFile);
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
            previousState = state;

            // update controller input
            HandleInput(gameTime);
            DealDamage();

            // TEMPORARY
            if (currentSequence.Title == "Attacking" && (currentSequence.CurrentFrame.X == 4 || currentSequence.CurrentFrame.X == 16) && (wave == null || wave.IsComplete))
            {
                wave = new BillboardWave(this.Parent, waveInfo, position, Vector3.Normalize(velocity) / 10, 50, 12, 0, 0, 7, 1);
                wave.Initialize();
            }

            if (castingAura.Visible == true)
            {
                castingAura.Position = new Vector3(position.X, 0.0f, position.Z);
                castingAura.Update(gameTime);
            }

            base.Update(gameTime);

            HandleStates(gameTime);
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

        private void DealDamage()
        {
            if (currentSequence.Title == "Attacking" && ((currentSequence.CurrentFrame.X == 4
                 || currentSequence.CurrentFrame.X == 12)) && target != null && !hasAttacked)
            {
                target.GetHit(this.classInfo.BaseDamage, orientation, 0.5f);
                this.hasAttacked = true;
            }

            if (currentSequence.IsComplete)
                hasAttacked = false;
        }

        // TESTING PURPOSES ONLY - need to verify/change/fix
        // Checks for an attack
        private void CheckForAttack(GameTime gameTime)
        {
            Monster t = null;

            foreach (Monster m in ((Level)Parent).MonsterList)
            {
                if (IsHit(m.PrimitiveShape))
                {
                    t = m;
                    break;
                }
            }

            target = t;
        }
        // End Testing

        /// <summary>
        /// Update the controller.  By that we mean update the 
        /// player's orientation based on controller input
        /// </summary>
        /// <param name="gameTime">The game time</param>
        private void HandleInput(GameTime gameTime)
        {
            GamepadDevice gamepadDevice = ((InputHub)GameEngine.Services.GetService(typeof(InputHub)))[playerIndex];
            KeyboardDevice keyboardDevice = (KeyboardDevice)GameEngine.Services.GetService(typeof(KeyboardDevice));

            // Update 360 degree velocity
            Vector3 newVelocity = new Vector3(gamepadDevice.LeftStickPosition.X, 0.0f, gamepadDevice.LeftStickPosition.Y);
            if (newVelocity != Vector3.Zero)
            {
                velocity = Vector3.Normalize(newVelocity);
            }
            else
            {
                velocity = newVelocity;
            }

            // Update state of player
            if (gamepadDevice.IsButtonDown(Buttons.RightTrigger) && state != ActorState.Attacking)
            {
                state = ActorState.Casting;

                if (!castingAura.Visible || castingAura.ScaleIncrement < 0)
                {
                    castingAura.Visible = true;
                    castingAura.InvertScaleIncrement();
                    castingAura.Position = new Vector3(position.X, 0.0f, position.Z - 1.0f);
                }
            }
            // Attacks and starts the damage timer
            else if (gamepadDevice.WasButtonPressed(Buttons.B) || keyboardDevice.WasKeyPressed(Keys.Space))
            {
                state = ActorState.Attacking;
            }

            if (gamepadDevice.WasButtonReleased(Buttons.RightTrigger))
            {
                castingAura.InvertScaleIncrement();
                state = ActorState.Idle;
            }

            // If not attacking or finished attacking.
            if ((state != ActorState.Attacking && state != ActorState.Casting) || (state == ActorState.Attacking && currentSequence.IsComplete))
            {
                if (velocity.Z == 0 && velocity.X == 0)
                {
                    state = ActorState.Idle;
                }
                else if (gamepadDevice.IsButtonDown(Buttons.A))
                {
                    state = ActorState.Running;
                }
                else
                {
                    state = ActorState.Walking;
                }
                // Update orientation.
                UpdateOrientation();
            }
        }

        private void HandleStates(GameTime gameTime)
        {
            if (state != previousState)
            {
                switch (state)
                {
                    case ActorState.Attacking:
                        CheckForAttack(gameTime);
                        break;
                }
            }
        }

        public void UpdateOrientation()
        {
            Orientation previousOrientation = orientation;
            if (velocity.Z > 0)
            {
                if (velocity.X == 0)
                    orientation = Orientation.North;
                if (velocity.X > 0)
                    orientation = Orientation.Northeast;
                if (velocity.X < 0)
                    orientation = Orientation.Northwest;
            }
            else if (velocity.Z < 0)
            {
                if (velocity.X == 0)
                    orientation = Orientation.South;
                if (velocity.X > 0)
                    orientation = Orientation.Southeast;
                if (velocity.X < 0)
                    orientation = Orientation.Southwest;
            }
            else if (velocity.X > 0)
            {
                orientation = Orientation.East;
            }
            else if (velocity.X < 0)
            {
                orientation = Orientation.West;
            }
            // Buffer orientation transitions.
            switch (previousOrientation)
            {
                case Orientation.North:
                    if ((orientation == Orientation.Northeast && velocity.X < 0.1f) ||
                        (orientation == Orientation.Northwest && velocity.X > -0.1f))
                        orientation = previousOrientation;
                    break;
                case Orientation.Northeast:
                    if ((orientation == Orientation.East && velocity.X < 0.9f) ||
                        (orientation == Orientation.North && velocity.Z < 0.9f))
                        orientation = previousOrientation;
                    break;
                case Orientation.East:
                    if ((orientation == Orientation.Southeast && velocity.Z > -0.1f) ||
                        (orientation == Orientation.Northeast && velocity.Z < 0.1f))
                        orientation = previousOrientation;
                    break;
                case Orientation.Southeast:
                    if ((orientation == Orientation.South && velocity.Z > -0.9f) ||
                        (orientation == Orientation.East && velocity.X < 0.9f))
                        orientation = previousOrientation;
                    break;
                case Orientation.South:
                    if ((orientation == Orientation.Southeast && velocity.X < 0.1f) ||
                        (orientation == Orientation.Southwest && velocity.X > -0.1f))
                        orientation = previousOrientation;
                    break;
                case Orientation.Southwest:
                    if ((orientation == Orientation.West && velocity.X > -0.9f) ||
                        (orientation == Orientation.South && velocity.Z > -0.9f))
                        orientation = previousOrientation;
                    break;
                case Orientation.West:
                    if ((orientation == Orientation.Southwest && velocity.Z > -0.1f) ||
                        (orientation == Orientation.Northwest && velocity.Z < 0.1f))
                        orientation = previousOrientation;
                    break;
                case Orientation.Northwest:
                    if ((orientation == Orientation.North && velocity.Z < 0.9f) ||
                        (orientation == Orientation.West && velocity.X > -0.9f))
                        orientation = previousOrientation;
                    break;
            }
        }

        #endregion // Update Methods

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
    }
}
