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
    public class WanderBehavior : Behavior
    {
        float interval;
        float timer;
        bool timerActive;

        public WanderBehavior(GameScreen parent, Monster monster)
            : base(parent, monster)
        {
            timer = 0.0f;
            interval = 2000.0f;
            this.type = BehaviorType.Wander;
            steerVector = Vector3.UnitZ;
        }

        public override void  React(Billboard otherObject, GameTime gameTime)
        {
            //do nothing
        }

        public override void Update(GameTime gameTime)
        {
            reacted = true;
            if (reacted == true)
            {
                if (!timerActive)
                    timerActive = true;
            }

            if (timerActive && timer < interval)
            {
                timer += gameTime.ElapsedGameTime.Milliseconds;
            }

            if (timer >= interval)
            {
                int num = GameEngine.Random.Next(4);
                if (num == 0)
                    monster.State = Actor.ActorState.Idle;
                else
                {
                    monster.State = Actor.ActorState.Walking;
                    monster.UpdateOrientation();
                }
                timer = 0.0f;
            }
        }

        public override void Reset()
        {
            base.Reset();
        }

        public void Release()
        {
            //buy vy
        }

        private Vector3 steerVector;
    }
}
