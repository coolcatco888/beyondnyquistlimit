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
        
        protected Vector3 position;

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

        Vector3 Scale
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
        Vector3 scale;

        public virtual void Translate(Vector3 translation)
        {
            position += translation;
        }

        public void Translate(float x, float y, float z)
        {
            Translate(new Vector3(x, y, z));
        }

        public void Rotate(Vector3 rotation)
        {
            Rotate(rotation.X, rotation.Y, rotation.Z);
        }

        public virtual void Rotate(float yaw, float pitch, float roll)
        {
            rotation *= Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll);
        }

        public virtual void ApplyScale(Vector3 scale)
        {
            scale = Vector3.Multiply(this.scale, scale);
        }

        public void ApplyScale(float x, float y, float z)
        {
            ApplyScale(new Vector3(x, y, x));
        }

        #endregion

        #region Properties

        public Vector3 LookAt
        {
            get { return lookAt; }
            set { lookAt = value; }
        }

        protected Vector3 lookAt;

        public Vector3 Direction
        {
            get { return Vector3.Normalize(LookAt - position); }
        }

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
        }

        #endregion

        #region Component Members

        public override void Initialize()
        {
            position = Vector3.Zero;
            rotation = Quaternion.Identity;
            lookAt = -Vector3.UnitZ;

            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GameEngine.Graphics.Viewport.AspectRatio, 1.0f, 10000.0f);
            
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            view = Matrix.CreateLookAt(position, lookAt, Vector3.Up);
            base.Update(gameTime);
        }

        #endregion
    }
}
