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
    class BasicModel : Component3D
    {

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Camera camera = (Camera)GameEngine.Services.GetService(typeof(Camera));

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;

                    effect.World = Matrix.CreateScale(scale) * Matrix.CreateFromQuaternion(rotation) * Matrix.CreateTranslation(position);
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                }

                mesh.Draw();
            }
        }

        Model model;

        public BasicModel(GameScreen parent, Model model)
            : base(parent)
        {

            this.Parent = parent;
            this.model = model;
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardDevice keyboardDevice = (KeyboardDevice)GameEngine.Services.GetService(typeof(KeyboardDevice));

            if (keyboardDevice != null && keyboardDevice.IsKeyDown(Keys.R))
            {
                Quaternion rot = Quaternion.CreateFromYawPitchRoll(0.001f * gameTime.ElapsedGameTime.Milliseconds, 0.0f, 0.0f);
                rotation *= rot;
            }

            base.Update(gameTime);
        }
    }
}
