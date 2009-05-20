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
    public abstract class CollidableComponent : Component3D, ICollidable
    {
        #region Initialization

        public CollidableComponent(GameScreen parent)
            : base(parent)
        {
        }

        public CollidableComponent(GameScreen parent, Vector3 position)
            : base(parent, position)
        {
        }

        public CollidableComponent(GameScreen parent, Vector3 position, Vector3 rotation)
            : base(parent, position, rotation)
        {
        }

        public CollidableComponent(GameScreen parent, Vector3 position, Vector3 rotation, Vector3 scale)
            : base(parent, position, rotation, scale)
        {
        }

        #endregion

        #region ICollidable Members

        /// <summary>
        /// Whether the billboard is collidable or not
        /// </summary>
        protected bool collidable;
        public bool Collidable
        {
            get { return collidable; }
            set { collidable = value; }
        }

        /// <summary>
        /// The billboards bounding shape used in collision detection
        /// </summary>
        protected PrimitiveShape primitiveShape;
        public virtual PrimitiveShape PrimitiveShape
        {
            get { return primitiveShape; }
        }

        /// <summary>
        /// Determines if there is a hit between objects using their bounding shapes
        /// </summary>
        /// <param name="otherShape">The other objects bounding shape</param>
        /// <returns>True if a hit, false otherwise</returns>
        public virtual bool IsHit(PrimitiveShape otherShape)
        {
            return PrimitiveShape.TestCollision(primitiveShape, otherShape);
        }

        #endregion // ICollidable Members
    }
}
