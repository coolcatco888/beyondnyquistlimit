#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
#endregion

namespace TheGame
{
    class Terrain : Component, IDrawableComponent, I3DComponent
    {
        #region Fields

        // Terrain model built from a bitmap
        Model terrain;
        //HeightInfo heightInfo;
        String fileName;

        public HeightMapInfo HeightMapInfo
        {
            get
            {
                return heightMapInfo;
            }
        }
        HeightMapInfo heightMapInfo;

        #endregion

        #region IDrawableComponent Members

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Camera camera = (Camera)GameEngine.Services.GetService(typeof(Camera));

            GameEngine.Graphics.RenderState.DepthBufferEnable = true;
            GameEngine.Graphics.RenderState.AlphaBlendEnable = false;
            GameEngine.Graphics.RenderState.AlphaTestEnable = false;
            GameEngine.Graphics.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
            GameEngine.Graphics.SamplerStates[0].AddressV = TextureAddressMode.Wrap;

            foreach (ModelMesh mesh in terrain.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;

                    effect.EnableDefaultLighting();

                    effect.SpecularColor = new Vector3(0.6f, 0.4f, 0.2f);
                    effect.SpecularPower = 8;

                    effect.FogEnabled = true;
                    effect.FogColor = new Vector3(0.15f);
                    effect.FogStart = 100;
                    effect.FogEnd = 320;
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
                return terrainPosition;
            }
            set
            {
                terrainPosition = value;
            }
        }
        Vector3 terrainPosition;

        public Microsoft.Xna.Framework.Quaternion Rotation
        {
            get
            {
                return terrainRotation;
            }
            set
            {
                terrainRotation = value;
            }
        }
        Quaternion terrainRotation;

        public float Scale
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        float terrainScale;

        #endregion

        public Terrain(GameScreen parent, String fileName)
            : base(parent)
        {
            visible = true;
            this.fileName = fileName;
            
            terrain = GameEngine.Content.Load<Model>(fileName);
            heightMapInfo = terrain.Tag as HeightMapInfo;
            if (heightMapInfo == null)
            {
                string message = "The terrain model did not have a HeightMapInfo " +
                    "object attached. Are you sure you are using the " +
                    "TerrainProcessor?";
                throw new InvalidOperationException(message);
            }
        }

        public override void Initialize(GameScreen parent)
        {
            base.Initialize(parent);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardDevice keyboardDevice = (KeyboardDevice)GameEngine.Services.GetService(typeof(KeyboardDevice));

            if (keyboardDevice != null && keyboardDevice.IsKeyDown(Keys.R))
            {
                Quaternion rot = Quaternion.CreateFromYawPitchRoll(0.001f * gameTime.ElapsedGameTime.Milliseconds, 0.0f, 0.0f);
                terrainRotation *= rot;
            }

            base.Update(gameTime);
        }
    }
}
