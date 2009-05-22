using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TheGame
{
    public enum BehaviorType { None, Cohesion, Alignment, Separation, Seek, Flee, Wander }

    public abstract class Behavior : Component
    {
        protected Monster monster;

        protected BehaviorType type;

        public Vector3 Reaction
        {
            get
            {
                return reaction;
            }
        }
        protected Vector3 reaction;

        public bool IsReacted
        {
            get
            {
                return reacted;
            }
        }
        protected bool reacted;

        protected Behavior(GameScreen parent, Monster monster)
            : base(parent)
        {
            this.monster = monster;

            type = BehaviorType.None;

            reaction = Vector3.Zero;

            reacted = false;
        }

        public abstract void React(Billboard otherObject, GameTime gameTime);

        public virtual void Reset()
        {
            reacted = false;
            reaction = Vector3.Zero;
        }
    }
}
