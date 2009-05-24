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
        public WanderBehavior(GameScreen parent, Monster monster)
            : base(parent, monster)
        {
            this.type = BehaviorType.Wander;
            updateTimeInterval = 2000.0f;
            currentTimeInterval = updateTimeInterval;
        }

        public override void  React(GameTime gameTime)
        {
            reacted = true;
            desireLevel = 3;
        }

        public override void Update(GameTime gameTime)
        {
            monster.State = Actor.ActorState.Walking;
            UpdateOrientation();
            monster.Speed = 0.005f;
        }

        public override void Reset()
        {
            base.Reset();
        }

        public void UpdateOrientation()
        {
            int negx = GameEngine.Random.Next(2);
            int negz = GameEngine.Random.Next(2);
            float x = (float)GameEngine.Random.NextDouble();
            float z = (float)GameEngine.Random.NextDouble();

            if (negx == 0)
            {
                x *= -1;
            }
            if (negz == 0)
            {
                z *= -1;
            }

            monster.Direction = new Vector3(x, 0.0f, z);
            if(monster.Direction != Vector3.Zero)
                monster.Direction = Vector3.Normalize(monster.Direction);
            GetOrientationFromDirection();
        }

        public override void ResetTimeInterval()
        {
            currentTimeInterval = updateTimeInterval;
        }

        
    }
}
