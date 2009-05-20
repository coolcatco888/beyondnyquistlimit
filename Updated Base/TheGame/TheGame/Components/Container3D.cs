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
    public class Container3D : Component3D
    {
        public Container3D(GameScreen parent)
            : base(parent)
        {
        }

        public List<Component3D> ComponentList
        {
            get { return componentList; }
            set { componentList = value; }
        }
        protected List<Component3D> componentList;

        #region Component Members

        public override void Initialize()
        {
            componentList = new List<Component3D>();
            base.Initialize();
        }

        public override void Translate(Vector3 translation)
        {
            foreach (Component3D component in componentList)
                component.Translate(translation);

            base.Translate(translation);
        }

        public void Add(Component3D component)
        {
            component.Translate(position);
            componentList.Add(component);
        }

        #endregion


    }
}
