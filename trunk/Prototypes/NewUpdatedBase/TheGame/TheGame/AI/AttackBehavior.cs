using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TheGame
{
    public class AttackBehavior : Behavior
    {
        protected float attackRadius;
        protected Player target;

        public AttackBehavior(GameScreen parent, Monster monster, float attackRadius)
            : base(parent, monster)
        {
            this.type = BehaviorType.Attack;
            this.attackRadius = attackRadius;
            updateTimeInterval = 5.0f;
        }


        public override void React(GameTime gameTime)
        {
            target = (Player)((Level)Parent).PlayerList.GetNearest(monster.Position);
            float distance = Vector3.Distance(monster.Position, target.Position);
            if (distance <= attackRadius && !(target.ActorStats.CurrentHealth <= 0))
            {
                reacted = true;
                desireLevel = 120;
            }
        }

        public override void Update(GameTime gameTime)
        {
            monster.State = Actor.ActorState.Attacking;
            monster.Direction = Vector3.Normalize(target.Position - monster.Position);
            monster.Direction = new Vector3(monster.Direction.X, monster.Direction.Y, monster.Direction.Z);
            this.GetOrientationFromDirection();
            monster.Speed = 0.0f;
        }

        public override void ResetTimeInterval()
        {
            currentTimeInterval = updateTimeInterval;
        }


        private void GetOrientation(Vector3 refPosition)
        {
            Vector3 relDirection = Vector3.Normalize(monster.Position - refPosition);

            float dot = Vector3.Dot(monster.Direction, relDirection);
            if (dot <= -1 && dot > -0.85f)
            {
                monster.Orientation = Orientation.South;
            }
            else if (dot <= -0.85f && dot > -0.50f)
            {
                if (monster.Direction.X < 0 || monster.Direction.Z < 0)
                    monster.Orientation = Orientation.Southwest;
                else
                    monster.Orientation = Orientation.Southeast;
            }
            else if (dot >= -0.5f && dot < 0.5f)
            {
                if (monster.Direction.X < 0 || monster.Direction.Z < 0)
                    monster.Orientation = Orientation.West;
                else
                    monster.Orientation = Orientation.East;
            }
            else if (dot >= 0.5f && dot < 0.85f)
            {
                if (monster.Direction.X < 0 || monster.Direction.Z < 0)
                    monster.Orientation = Orientation.Northwest;
                else
                    monster.Orientation = Orientation.Northeast;
            }
            else
            {
                monster.Orientation = Orientation.North;
            }
        }
    }
}
