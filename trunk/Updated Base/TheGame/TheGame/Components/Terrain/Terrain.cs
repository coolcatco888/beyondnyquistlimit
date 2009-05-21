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
    public class Terrain : Component3D
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

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
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
        
        public Terrain(GameScreen parent, String fileName)
            : base(parent)
        {
            this.fileName = fileName;
        }

        public override void Initialize()
        {
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
