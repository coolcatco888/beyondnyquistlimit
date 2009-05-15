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
    public class Terrain : Component, IDrawableComponent, I3DComponent
    {
        #region Fields

        // Terrain model built from a bitmap
        Model terrain;

        // The filename of the terrain bitmap
        String fileName;

        /// <summary>
        /// The height information associated with this terrain map
        /// </summary>
        private HeightMapInfo heightMapInfo;
        public HeightMapInfo HeightMapInfo
        {
            get { return heightMapInfo; }
        }
        
        #endregion

        #region IDrawableComponent Members

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Camera camera = (Camera)GameEngine.Services.GetService(typeof(Camera));

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

        // Whether this screen is visible or not
        private bool visible;
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }
        
        #endregion

        #region I3DComponent Members

        private Vector3 terrainPosition;
        public Vector3 Position
        {
            get { return terrainPosition; }
            set { terrainPosition = value; }
        }

        private Quaternion terrainRotation;
        public Quaternion Rotation
        {
            get { return terrainRotation; }
            set { terrainRotation = value; }
        }

        private float terrainScale;
        public float Scale
        {
            get { return terrainScale; }
            set { terrainScale = value; }
        }
        
        #endregion

        public Terrain(GameScreen parent, String fileName)
            : base(parent)
        {
            this.fileName = fileName;
        }

        public override void Initialize()
        {
            visible = true;

            terrain = GameEngine.Content.Load<Model>(fileName);
            heightMapInfo = terrain.Tag as HeightMapInfo;
            if (heightMapInfo == null)
            {
                string message = "The terrain model did not have a HeightMapInfo " +
                    "object attached. Are you sure you are using the " +
                    "TerrainProcessor?";
                throw new InvalidOperationException(message);
            }

            base.Initialize();
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
