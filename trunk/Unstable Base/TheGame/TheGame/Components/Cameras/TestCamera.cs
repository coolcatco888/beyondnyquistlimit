using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace TheGame
{
    class TestCamera : Camera
    {
        Vector2 mousePosition;
        bool held;

        public TestCamera(GameScreen parent)
            : base(parent)
        {
        }

        public override void Initialize()
        {
            mousePosition = Vector2.Zero;
            held = false;

            base.Initialize();

            Position = new Vector3(0.0f, 3.0f, 2.0f);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardDevice kbd = (KeyboardDevice)GameEngine.Services.GetService(typeof(KeyboardDevice));
            MouseState ms = Mouse.GetState();

            Vector3 movement = Vector3.Zero;

            if (kbd.IsKeyDown(Keys.W))
            {
                movement.Z -= 0.01f * gameTime.ElapsedGameTime.Milliseconds;
            }
            if (kbd.IsKeyDown(Keys.A))
            {
                movement.X -= 0.01f * gameTime.ElapsedGameTime.Milliseconds;
            }
            if (kbd.IsKeyDown(Keys.S))
            {
                movement.Z += 0.01f * gameTime.ElapsedGameTime.Milliseconds;
            }
            if (kbd.IsKeyDown(Keys.D))
            {
                movement.X += 0.01f * gameTime.ElapsedGameTime.Milliseconds;
            }

            Vector3 dir = -Vector3.UnitZ;

            if (ms.RightButton == ButtonState.Pressed && !held)
            {
                held = true;
                mousePosition = new Vector2(ms.X, ms.Y);
            }
            else if (ms.RightButton == ButtonState.Pressed && held)
            {
                Vector2 drag = mousePosition - new Vector2(ms.X, ms.Y);

                if (drag != Vector2.Zero)
                    drag.Normalize();

                drag *= 0.001f * gameTime.ElapsedGameTime.Milliseconds;

                Quaternion rot = Quaternion.CreateFromYawPitchRoll(drag.X, drag.Y, 0.0f);

                Rotation *= rot;
                
            }
            else if(ms.RightButton == ButtonState.Released)
            {
                held = false;
            }

            
            Matrix rottrans = Matrix.CreateFromQuaternion(Rotation);

            Vector3.TransformNormal(ref movement, ref rottrans, out movement);
            Vector3.TransformNormal(ref dir, ref rottrans, out dir);

            Position += movement;
            LookAt = Position + dir;

            base.Update(gameTime);
        }
    }
}
