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
    public class Container3D : Component, IMoveable
    {
        public Container3D(GameScreen parent)
            : base(parent)
        {
        }

        public List<IMoveable> MoveableList
        {
            get { return moveableList; }
            set { moveableList = value; }
        }
        protected List<IMoveable> moveableList;

        #region IMoveable Members

        public Microsoft.Xna.Framework.Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                foreach(IMoveable moveable in moveableList)
                {
                    moveable.Position += value - position;
                }
                position = value;
            }
        }
        protected Vector3 position;

        #endregion

        #region Component Members

        public override void Initialize()
        {
            moveableList = new List<IMoveable>();
            position = Vector3.Zero;

            base.Initialize();
        }

        public void Add(IMoveable moveable)
        {
            moveable.Position += position;
            moveableList.Add(moveable);
        }

        #endregion


    }
}
