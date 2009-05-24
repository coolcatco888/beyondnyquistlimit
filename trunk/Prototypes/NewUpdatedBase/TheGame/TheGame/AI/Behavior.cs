using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TheGame
{
    public enum BehaviorType { None, Seek, Attack, Wander, Idle }

    public abstract class Behavior : Component
    {
        protected Monster monster;

        protected int desireLevel;
        public int DesireLevel
        {
            get { return desireLevel; }
        }

        protected BehaviorType type;

        protected float updateTimeInterval;
        public float UpdateTimeInterval
        {
            get { return updateTimeInterval; }
            set { updateTimeInterval = value; }
        }

        protected float currentTimeInterval;
        public float CurrentTimeInterval
        {
            get { return currentTimeInterval; }
            set { currentTimeInterval = value; }
        }

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

            reacted = false;
        }

        public abstract void React(GameTime gameTime);
        public abstract void ResetTimeInterval();

        public virtual void Reset()
        {
            reacted = false;
        }

        public void GetOrientationFromDirection()
        {
            Camera camera = (Camera)GameEngine.Services.GetService(typeof(Camera));

            Vector3 relCameraDirection = camera.Position - camera.LookAt;

            float angle = ((float)Math.Atan2(-relCameraDirection.X, relCameraDirection.Z));

            Vector3 orientation = Vector3.Transform(monster.Direction, Matrix.CreateRotationY(angle));

            UpdateOrientation(orientation);
        }

        public void UpdateOrientation(Vector3 direction) // Do not touch ... complete
        {
            Orientation previousOrientation = monster.Orientation;
            if (direction.Z < 0.05f)
            {
                if (direction.X > 0)
                    monster.Orientation = Orientation.Northeast;
                else if (direction.X < 0)
                    monster.Orientation = Orientation.Northwest;
                else
                    monster.Orientation = Orientation.North;
            }
            else if (direction.Z > 0)
            {
                if (direction.X > 0)
                    monster.Orientation = Orientation.Southeast;
                else if (direction.X < 0)
                    monster.Orientation = Orientation.Southwest;
                else
                    monster.Orientation = Orientation.South;
            }
            else if (direction.X < 0)
            {
                monster.Orientation = Orientation.West;
            }
            else if (direction.X > 0)
            {
                monster.Orientation = Orientation.East;
            }
        }
    }
}
