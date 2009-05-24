using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TheGame
{
    public class SeekBehavior : Behavior
    {
        protected float sightRadius;
        protected Player target;
       
        public SeekBehavior(GameScreen parent, Monster monster, float sightRadius)
            : base(parent, monster)
        {
            this.type = BehaviorType.Seek;
            this.sightRadius = sightRadius;
            updateTimeInterval = 50.0f;
            currentTimeInterval = updateTimeInterval;
        }

        public override void React(GameTime gameTime)
        {
            target = (Player)((Level)Parent).PlayerList.GetNearest(monster.Position);
            float distance = Vector3.Distance(monster.Position, target.Position);
            if (distance < sightRadius)
            {
                reacted = true;
                desireLevel = 20;
            }
        }

        public override void Update(GameTime gameTime)
        {
            monster.State = Actor.ActorState.Walking;
            monster.Direction = Vector3.Normalize(target.Position - monster.Position);
            monster.Direction = new Vector3(monster.Direction.X, monster.Direction.Y, monster.Direction.Z);
            this.GetOrientationFromDirection();
            monster.Speed = 0.01f;
        }

        public override void Reset()
        {
            base.Reset();
        }

        public override void ResetTimeInterval()
        {
            currentTimeInterval = updateTimeInterval;
        }
    }
}
