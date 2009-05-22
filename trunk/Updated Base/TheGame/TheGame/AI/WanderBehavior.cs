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

        public override void React(Billboard otherObject, GameTime gameTime)
        {
            // do nothing for wander
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
                {
                    monster.State = Actor.ActorState.Idle;
                    monster.Speed = 0.0f;
                }
                else
                {
                    monster.State = Actor.ActorState.Walking;
                    UpdateOrientation();
                    monster.Speed = 0.005f;
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

        public void UpdateOrientation()
        {
            int num = GameEngine.Random.Next(8);

            switch (num)
            {
                case 0:
                    monster.Orientation = Orientation.South;
                    monster.Direction = new Vector3(0.0f, 0.0f, -1.0f);
                    break;
                case 1:
                    monster.Orientation = Orientation.Southwest;
                    monster.Direction = Vector3.Normalize(new Vector3(-1.0f, 0.0f, -1.0f));
                    break;
                case 2:
                    monster.Orientation = Orientation.West;
                    monster.Direction = new Vector3(-1.0f, 0.0f, 0.0f);
                    break;
                case 3:
                    monster.Orientation = Orientation.Northwest;
                    monster.Direction = Vector3.Normalize(new Vector3(-1.0f, 0.0f, 1.0f));
                    break;
                case 4:
                    monster.Orientation = Orientation.North;
                    monster.Direction = new Vector3(0.0f, 0.0f, 1.0f);
                    break;
                case 5:
                    monster.Orientation = Orientation.Northeast;
                    monster.Direction = Vector3.Normalize(new Vector3(1.0f, 0.0f, 1.0f));
                    break;
                case 6:
                    monster.Orientation = Orientation.East;
                    monster.Direction = new Vector3(1.0f, 0.0f, 0.0f);
                    break;
                case 7:
                    monster.Orientation = Orientation.Southeast;
                    monster.Direction = Vector3.Normalize(new Vector3(1.0f, 0.0f, -1.0f));
                    break;
            }
        }

        private Vector3 steerVector;
    }
}
