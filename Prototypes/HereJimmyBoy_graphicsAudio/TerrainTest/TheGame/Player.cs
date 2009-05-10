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

        private GroundEffect shadow;
        private MagicCircleEffect circle;

        public Player(GameScreen parent, SpriteInfo spriteInfo)
            : base(parent, spriteInfo)
        {
            SpriteInfo spriteInfo1 = GameEngine.Content.Load<SpriteInfo>(@"MagicCircle");
            circle = new MagicCircleEffect(parent, spriteInfo1, 0.1f, 0.05f, 0, 0);
            //shadow = new GroundEffect(parent, new SpriteInfo(GameEngine.Content.Load<Texture2D>("Shadow"), 64, 32, 0));

            AddBasicSequences();
        }

        public override void Initialize(GameScreen parent)
        {
            base.Initialize(parent);
        }

        public override void Update(GameTime gameTime)
        {
            UpdateController();

            circle.Position = this.position;
            circle.Update(gameTime);

            base.Update(gameTime);
        }

        private void UpdateController()
        {
            GamepadDevice gamepadDevice = (GamepadDevice)GameEngine.Services.GetService(typeof(GamepadDevice));
            zDirection = gamepadDevice.LeftStickPosition.Y;
            xDirection = gamepadDevice.LeftStickPosition.X;

            Vector3 newVelocity = new Vector3(xDirection, 0.0f, zDirection);

            if (newVelocity != Vector3.Zero)
            {
                velocity = Vector3.Normalize(newVelocity);
            }
            else
            {
                velocity = newVelocity;
            }

            Orientation previousOrientation = orientation;
            
            if(gamepadDevice.IsButtonDown(Buttons.A))
            {
                state = ActorState.Running;
            }
            else
            {
                state = ActorState.Walking;
            }

            if (zDirection == 0 && xDirection == 0)
            {
                idle();
            }

            if (zDirection > 0)
            {
                if (xDirection == 0)
                    orientation = Orientation.North;
                if (xDirection > 0)
                    orientation = Orientation.Northeast;
                if (xDirection < 0)
                    orientation = Orientation.Northwest;
            }
            else if (zDirection < 0)
            {
                if (xDirection == 0)
                    orientation = Orientation.South;
                if (xDirection > 0)
                    orientation = Orientation.Southeast;
                if (xDirection < 0)
                    orientation = Orientation.Southwest;
            }
            else if (xDirection > 0)
            {
                orientation = Orientation.East;
            }
            else if (xDirection < 0)
            {
                orientation = Orientation.West;
            }

            switch (previousOrientation)
            {
                case Orientation.North:
                    if ((orientation == Orientation.Northeast || orientation == Orientation.Northwest) && (xDirection > -0.3f && xDirection < 0.3f))
                        orientation = previousOrientation;
                    break;
                case Orientation.Northeast:
                    if ((orientation == Orientation.East || orientation == Orientation.North) && (xDirection > 0.2f || zDirection > 0.2f))
                        orientation = previousOrientation;
                    break;
                case Orientation.East:
                    if ((orientation == Orientation.Southeast || orientation == Orientation.Northeast) && (zDirection > -0.3f && zDirection < 0.3f))
                        orientation = previousOrientation;
                    break;
                case Orientation.Southeast:
                    if ((orientation == Orientation.South || orientation == Orientation.East) && (xDirection < 0.8f || zDirection < -0.8f))
                        orientation = previousOrientation;
                    break;
                case Orientation.South:
                    if ((orientation == Orientation.Southeast || orientation == Orientation.Southwest) && (xDirection > -0.3f && xDirection < 0.3f))
                        orientation = previousOrientation;
                    break;
                case Orientation.Southwest:
                    if ((orientation == Orientation.West || orientation == Orientation.South) && (xDirection < -0.2f || zDirection > -0.8f))
                        orientation = previousOrientation;
                    break;
                case Orientation.West:
                    if ((orientation == Orientation.Southwest || orientation == Orientation.Northwest) && (zDirection > -0.3f && zDirection < 0.3f))
                        orientation = previousOrientation;
                    break;
                case Orientation.Northwest:
                    if ((orientation == Orientation.North || orientation == Orientation.West) && (xDirection > -0.2f || zDirection < 0.8f))
                        orientation = previousOrientation;
                    break;
            }

            /*
#if !XBOX
            KeyboardDevice keyboardDevice = (KeyboardDevice)GameEngine.Services.GetService(typeof(KeyboardDevice));

            if (keyboardDevice.IsKeyDown(Keys.LeftShift) || keyboardDevice.IsKeyDown(Keys.RightShift))
                state = Actor.ActorState.Running;
            else
                state = Actor.ActorState.Walking;


            if (keyboardDevice.IsKeyDown(Keys.Up))
            {
                if (keyboardDevice.IsKeyDown(Keys.Right))
                    orientation = Orientation.Northeast;
                else if (keyboardDevice.IsKeyDown(Keys.Left))
                    orientation = Orientation.Northwest;
                else
                    orientation = Orientation.North;
            }
            else if (keyboardDevice.IsKeyDown(Keys.Down))
            {
                if (keyboardDevice.IsKeyDown(Keys.Right))
                    orientation = Orientation.Southeast;
                else if (keyboardDevice.IsKeyDown(Keys.Left))
                    orientation = Orientation.Southwest;
                else
                    orientation = Orientation.South;
            }
            else if (keyboardDevice.IsKeyDown(Keys.Right))
            {
                orientation = Orientation.East;
            }
            else if (keyboardDevice.IsKeyDown(Keys.Left))
            {
                orientation = Orientation.West;
            }
            else
            {
                idle();
            }
#endif  // !XBOX
            */
        }

        public void AddBasicSequences()
        {
            SpriteSequence sequence;
            int x = 0;
            int y = 0;

            // Add idle sequences.
            foreach (Orientation orientation in Enum.GetValues(typeof(Orientation)))
            {
                sequence = new SpriteSequence("Idle", orientation, false, 0.0f, 0);
                sequence.AddFrame(0, y++);

                sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);
            }

            x = 0;
            y = 0;

            // Add walking sequences.
            foreach (Orientation orientation in Enum.GetValues(typeof(Orientation)))
            {
                sequence = new SpriteSequence("Walking", orientation, true, 0.05f, 2);
                sequence.AddRow(y++, 1, 8);

                sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);
            }

            x = 0;
            y = 0;

            // Add running sequences.
            foreach (Orientation orientation in Enum.GetValues(typeof(Orientation)))
            {
                sequence = new SpriteSequence("Running", orientation, true, 0.1f, 1);
                sequence.AddRow(y++, 1, 8);

                sequences.Add(sequence.Title + sequence.Orientation.ToString(), sequence);
            }

            currentSequence = sequences["IdleSouth"];
        }
    }
}
