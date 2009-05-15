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
    public class Container3D : Component, I3DComponent
    {
        public Container3D(GameScreen parent)
            : base(parent)
        {
        }

        public List<I3DComponent> Component3DList
        {
            get { return component3DList; }
            set { component3DList = value; }
        }
        protected List<I3DComponent> component3DList;

        #region I3DComponent Members

        public Microsoft.Xna.Framework.Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                foreach(I3DComponent component3D in component3DList)
                {
                    component3D.Position += value - position;
                }
                position = value;
            }
        }
        protected Vector3 position;

        public Microsoft.Xna.Framework.Quaternion Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                foreach(I3DComponent component3D in component3DList)
                {
                    Matrix containerRot = Matrix.CreateFromQuaternion(value);
                    Matrix trans = Matrix.CreateTranslation(component3D.Position - position) * containerRot;

                    component3D.Position = Vector3.Transform(Vector3.Zero, trans);
                    component3D.Rotation = Quaternion.CreateFromRotationMatrix(trans);
                }

                rotation = value;
            }
        }
        protected Quaternion rotation;

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
        protected float scale;

        #endregion

        #region Component Members

        public override void Initialize()
        {
            component3DList = new List<I3DComponent>();
            position = Vector3.Zero;
            rotation = Quaternion.Identity;
            scale = 1.0f;

            base.Initialize();
        }

        public void Add(I3DComponent component3D)
        {
            component3D.Position += position;
            component3DList.Add(component3D);
        }

        #endregion


    }
}
