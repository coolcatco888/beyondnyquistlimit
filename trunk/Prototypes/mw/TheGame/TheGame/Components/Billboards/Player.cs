using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Library;

namespace TheGame
{
    public class Player : Actor
    {
        #region Fields

        protected PlayerIndex playerIndex;
        protected CastingAura castingAura;
        protected SpriteInfo castingSpriteInfo;

        // TEMPORARY
        SpriteInfo waveInfo = GameEngine.Content.Load<Library.SpriteInfo>(@"Sprites\\CloudInfo");
        BillboardWave wave;

        #endregion  // Fields

        public Player(GameScreen parent, SpriteInfo spriteInfo, PlayerIndex playerIndex)
            : base(parent, spriteInfo)
        {
            this.playerIndex = playerIndex;
            castingSpriteInfo = GameEngine.Content.Load<Library.SpriteInfo>(@"Sprites\\MagicCircle");

            AddBasicSequences();

            this.Initialize();
        }

        public override void Initialize()
        {
            scale.X = 1.0f;
            scale.Y = 2.0f;

            position.Y = 2.0f;
            position.Z = -1.0f;

            castingAura = new CastingAura(this.Parent, castingSpriteInfo, 0.4f, 0.01f, new Point(0, 0));
            castingAura.Visible = false;
            castingAura.InvertScaleIncrement();

            // TEMPORARY

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            castingAura.Position = new Vector3(position.X, 0.0f, position.Z);

            // TEMPORARY
            if (currentSequence.Title == "Attacking" && (currentSequence.CurrentFrame.X == 4 || currentSequence.CurrentFrame.X == 16) && (wave == null || wave.IsComplete))
            {
                wave = new BillboardWave(this.Parent, waveInfo, position, Vector3.Normalize(velocity) * 2, 200, 10, 0, 0, 7, 1);
                wave.Initialize();
            }

            UpdateController();

            base.Update(gameTime);
        }

        private void UpdateController()
        {
            GamepadDevice gamepadDevice = (GamepadDevice)GameEngine.Services.GetService(typeof(GamepadDevice));
            KeyboardDevice keyboardDevice = (KeyboardDevice)GameEngine.Services.GetService(typeof(KeyboardDevice));

            // Update 360 degree velocity
            Vector3 newVelocity = new Vector3(gamepadDevice.LeftStickPosition.X, 0.0f, gamepadDevice.LeftStickPosition.Y);
            if (newVelocity != Vector3.Zero && currentSequence.Title != "Attacking")
            {
                velocity = Vector3.Normalize(newVelocity);
            }
            else
            {
                velocity = newVelocity;
            }


            // Update state.
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
        }

        public void AddBasicSequences()
        {
            SpriteSequence sequence;
            int x = 0;
            int y = 0;

            // Add idle sequences.
            foreach (Orientation orientation in Enum.GetValues(typeof(Orientation)))
            {
                sequence = new SpriteSequence("Idle", orientation, true, 0.0f, 0);
                sequence.AddFrame(x, y++);

                sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);
                if (y == 3)
                {
                    y = 0;
                    x += 9;
                }
            }

            x = 1;
            y = 0;

            // Add walking sequences.
            foreach (Orientation orientation in Enum.GetValues(typeof(Orientation)))
            {
                sequence = new SpriteSequence("Walking", orientation, true, 0.05f, 2);
                sequence.AddRow(y++, x, x+7);

                sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);
                if (y == 3)
                {
                    y = 0;
                    x += 9;
                }
            }
            x = 1;
            y = 0;

            // Add running sequences.
            foreach (Orientation orientation in Enum.GetValues(typeof(Orientation)))
            {
                sequence = new SpriteSequence("Running", orientation, true, 2.0f, 1);
                sequence.AddRow(y++, x, x + 7);

                sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);
                if (y == 3)
                {
                    y = 0;
                    x += 9;
                }
            }

            // Battle idle
            sequence = new SpriteSequence("Attacking", Orientation.Southwest, false, 0.0f, 3, 2, 1);
            sequence.AddRow(7, 0, 10);
            sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);
            sequence = new SpriteSequence("Attacking", Orientation.South, false, 0.0f, 3, 2, 1);
            sequence.AddRow(7, 0, 10);
            sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);
            sequence = new SpriteSequence("Attacking", Orientation.West, false, 0.0f, 3, 2, 1);
            sequence.AddRow(7, 0, 10);
            sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);

            sequence = new SpriteSequence("Attacking", Orientation.Southeast, false, 0.0f, 3, 2, 1);
            sequence.AddRow(7, 12, 22);
            sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);
            sequence = new SpriteSequence("Attacking", Orientation.East, false, 0.0f, 3, 2, 1);
            sequence.AddRow(7, 12, 22);
            sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);

            sequence = new SpriteSequence("Attacking", Orientation.Northwest, false, 0.0f, 3, 2, 1);
            sequence.AddRow(8, 0, 10);
            sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);
            sequence = new SpriteSequence("Attacking", Orientation.North, false, 0.0f, 3, 2, 1);
            sequence.AddRow(8, 0, 10);
            sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);

            sequence = new SpriteSequence("Attacking", Orientation.Northeast, false, 0.0f, 3, 2, 1);
            sequence.AddRow(8, 12, 22);
            sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);
            //sequence = new SpriteSequence("Attacking", Orientation.Southwest, true, 0.0f, 3);
            //sequence.AddRow(3, 0, 5);
            //sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);
            //sequence = new SpriteSequence("Attacking", Orientation.South, true, 0.0f, 3);
            //sequence.AddRow(3, 0, 5);
            //sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);
            //sequence = new SpriteSequence("Attacking", Orientation.West, true, 0.0f, 3);
            //sequence.AddRow(3, 0, 5);
            //sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);

            //sequence = new SpriteSequence("Attacking", Orientation.Southeast, true, 0.0f, 3);
            //sequence.AddRow(3, 6, 11);
            //sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);
            //sequence = new SpriteSequence("Attacking", Orientation.East, true, 0.0f, 3);
            //sequence.AddRow(3, 6, 11);
            //sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);

            //sequence = new SpriteSequence("Attacking", Orientation.Northwest, true, 0.0f, 3);
            //sequence.AddRow(3, 12, 17);
            //sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);
            //sequence = new SpriteSequence("Attacking", Orientation.North, true, 0.0f, 3);
            //sequence.AddRow(3, 12, 17);
            //sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);

            //sequence = new SpriteSequence("Attacking", Orientation.Northeast, true, 0.0f, 3);
            //sequence.AddRow(3, 18, 23);
            //sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);

            // Spell
            sequence = new SpriteSequence("Casting", Orientation.Southwest, true, 0.0f, 3);
            sequence.AddRow(4, 0, 4);
            sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);
            sequence = new SpriteSequence("Casting", Orientation.South, true, 0.0f, 3);
            sequence.AddRow(4, 0, 4);
            sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);
            sequence = new SpriteSequence("Casting", Orientation.West, true, 0.0f, 3);
            sequence.AddRow(4, 0, 4);
            sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);

            sequence = new SpriteSequence("Casting", Orientation.Southeast, true, 0.0f, 3);
            sequence.AddRow(4, 5, 9);
            sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);
            sequence = new SpriteSequence("Casting", Orientation.East, true, 0.0f, 3);
            sequence.AddRow(4, 5, 9);
            sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);

            sequence = new SpriteSequence("Casting", Orientation.Northwest, true, 0.0f, 3);
            sequence.AddRow(4, 10, 14);
            sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);
            sequence = new SpriteSequence("Casting", Orientation.North, true, 0.0f, 3);
            sequence.AddRow(4, 10, 14);
            sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);

            sequence = new SpriteSequence("Casting", Orientation.Northeast, true, 0.0f, 3);
            sequence.AddRow(4, 15, 19);
            sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);

            currentSequence = sequences["IdleSouth"];
        }
    }
}
