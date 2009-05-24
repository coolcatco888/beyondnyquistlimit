using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TheGame
{
    public class IdleBehavior : Behavior
    {
        public IdleBehavior(GameScreen parent, Monster monster)
            : base(parent, monster)
        {
            this.type = BehaviorType.Idle;
            updateTimeInterval = 3000.0f;
            currentTimeInterval = updateTimeInterval;
        }

        public override void React(GameTime gameTime)
        {
            reacted = true;
            desireLevel = 1;
        }

        public override void Update(GameTime gameTime)
        {
            monster.State = Actor.ActorState.Idle;
            monster.Speed = 0.000f;
        }

        public override void Reset()
        {
            base.Reset();
        }

        public override void  ResetTimeInterval()
        {
            currentTimeInterval = updateTimeInterval;
        }
    }
}
