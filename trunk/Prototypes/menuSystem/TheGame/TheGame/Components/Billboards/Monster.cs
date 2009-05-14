using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TheGame
{
    public class Monster : Billboard
    {
        protected int maxHealth;
        public int MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

        protected int currentHealth;
        public int CurrentHealth
        {
            get { return currentHealth; }
            set { currentHealth = value; }
        }

        Vector3 min;
        Vector3 max;

        public Monster(GameScreen parent, Texture2D texture, int health)
            : base(parent, texture)
        {
            maxHealth = health;
            currentHealth = maxHealth;
            this.Initialize();
        }

        public override void Initialize()
        {
            min = new Vector3(position.X - 1, position.Y, position.Z - 1);
            max = new Vector3(position.X + 1, position.Y + 1, position.Z + 1);

            boundingBox = new BoundingBox(min, max);
            collidable = true;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            min = new Vector3(position.X - 0.5f, position.Y - 2, position.Z - 1);
            max = new Vector3(position.X + 0.5f, position.Y + 2, position.Z + 3);

            boundingBox = new BoundingBox(min, max);
            if (currentHealth <= 0)
            {
                BillboardList ms = ((Level)Parent).Monsters;
                ms.Remove(this);
                this.Dispose();
            }
            base.Update(gameTime);
        }

        public void ApplyDamage(int value)
        {
            currentHealth -= value;
        }
    }
}
