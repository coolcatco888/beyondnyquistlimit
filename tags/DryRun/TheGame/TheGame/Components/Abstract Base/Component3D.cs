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
    public abstract class Component3D : DrawableComponent, I3DComponent
    {
        #region Initiazation

        public Component3D(GameScreen parent, Vector3 position, Vector3 rotation, Vector3 scale)
            : base(parent)
        {
            this.Parent = parent;
            this.position = position;
            this.rotation = Quaternion.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z);
            this.scale = scale;
        }

        public Component3D(GameScreen parent, Vector3 position, Vector3 rotation)
            : this(parent, position, rotation, Vector3.One)
        {
        }

        public Component3D(GameScreen parent, Vector3 position)
            : this(parent, position, Vector3.Zero)
        {
        }

        public Component3D(GameScreen parent)
            : this(parent, Vector3.Zero)
        {
        }

        #endregion

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
        protected Quaternion rotation;

        public Vector3 Scale
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
        protected Vector3 scale;

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
    }
}
