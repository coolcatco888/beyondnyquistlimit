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
    class BasicModel : Component, IDrawableComponent, I3DComponent
    {

        #region IDrawableComponent Members

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
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

        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
            }
        }
        bool visible;

        #endregion

        #region I3DComponent Members

        public Microsoft.Xna.Framework.Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
        Vector3 position;

        public Microsoft.Xna.Framework.Quaternion Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
            }
        }
        Quaternion rotation;

        public float Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
            }
        }
        float scale;

        #endregion

        Model model;

        public BasicModel(GameScreen parent, Model model)
            : base(parent)
        {
            visible = true;
            position = Vector3.Zero;
            rotation = Quaternion.Identity;
            scale = 1.0f;

            this.Parent = parent;
            this.model = model;

            Initialize();
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
