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
    class Camera : Component, I3DComponent
    {

        #region I3DComponent Members

        public Vector3 Position
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

        public Quaternion Rotation
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

        #region Properties

        public Vector3 LookAt
        {
            get { return lookAt; }
            set { lookAt = value; }
        }
        Vector3 lookAt;

        public Matrix View
        {
            get { return view; }
            set { view = value; }
        }
        Matrix view;

        public Matrix Projection
        {
            get { return projection; }
            set { projection = value; }
        }
        Matrix projection;

        #endregion

        #region Constructor

        public Camera(GameScreen parent)
            : base(parent)
        {
            Initialize(parent);
        }

        #endregion

        #region Component Members

        public override void Initialize(GameScreen parent)
        {
            position = new Vector3(0.0f, 10.0f, 10.0f);
            rotation = Quaternion.Identity;
            lookAt = new Vector3(0.0f, 0.0f, -1.0f);

            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GameEngine.Graphics.Viewport.AspectRatio, 1.0f, 10000.0f);
            
            base.Initialize(parent);
        }

        public override void Update(GameTime gameTime)
        {
            view = Matrix.CreateLookAt(position, position + lookAt, Vector3.Up);
 
            base.Update(gameTime);
        }

        public Vector3 Direction
        {
            get
            {
                Vector3 r = -Vector3.UnitZ;
                Matrix rotationMatrix = Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z);

                Vector3.TransformNormal(ref r, ref rotationMatrix, out r);

                return r;
            }
        }

        #endregion
    }
}
