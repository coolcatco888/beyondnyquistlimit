#region Using Statements

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace TheGame
{
    class BillboardTrail : BillboardOnFire, IMoveable
    {
        protected Vector3[] positionList;
        protected MultipleSequence[] spriteSequences;
        protected Library.SpriteInfo spriteInfo;

        private int maxTrailCount;
        private bool isComplete = false;
        private float spawnTimer = 0.0f;
        private float updateTimer = 0.0f;

        private Vector3 origin;

        public bool IsComplete
        {
            get { return isComplete; }
        }
        public Vector3 Origin
        {
            get { return origin; }
            set
            {
                origin.X = value.X;
                origin.Z = value.Z;
            }
        }
        public Vector3[] PositonList
        {
            get { return positionList; }
        }
        public MultipleSequence[] SpriteSequences
        {
            get { return spriteSequences; }
        }
        public int MaxTrailCount
        {
            get { return maxTrailCount; }
        }

        public BillboardTrail(GameScreen parent, Library.SpriteInfo spriteInfo, MultipleSequence sequence, Vector3 position, int maxTrailCount)
            : base(parent, spriteInfo.SpriteSheet)
        {
            this.spriteInfo = spriteInfo;
            this.origin = new Vector3(position.X, 1.0f, position.Z);
            this.maxTrailCount = maxTrailCount;

            this.spriteSequences = new MultipleSequence[maxTrailCount];
            this.positionList = new Vector3[maxTrailCount];

            for (int i = 0; i < maxTrailCount; i++)
            {
                spriteSequences[i] = (MultipleSequence)sequence.Clone();
                spriteSequences[i].IsPaused = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            spawnTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            updateTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            while (spawnTimer > 0)
            {
                for (int i = 0; i < maxTrailCount; i++)
                {
                    if (spriteSequences[i].IsPaused)
                    {
                        positionList[i] = this.origin;
                        spriteSequences[i].IsPaused = false;
                        break;
                    }
                }

                spawnTimer -= 500;
            }

            while (updateTimer > 0)
            {
                for (int i = 0; i < maxTrailCount; i++)
                {
                    if (spriteSequences[i].IsPaused == false)
                    {
                        spriteSequences[i].Update(gameTime);
                    }
                    if (spriteSequences[i].IsComplete)
                    {
                        spriteSequences[i].IsPaused = true;
                        spriteSequences[i].Reset();
                    }
                }

                updateTimer -= 50;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameEngine.Graphics.RenderState.AlphaBlendEnable = true;
            GameEngine.Graphics.RenderState.BlendFunction = BlendFunction.Add;
            GameEngine.Graphics.RenderState.SourceBlend = Blend.SourceAlpha;
            GameEngine.Graphics.RenderState.DestinationBlend = Blend.InverseSourceAlpha;

            for (int i = 0; i < maxTrailCount; i++)
            {
                if (spriteSequences[i].IsPaused == false)
                {
                    this.position = positionList[i];
                    this.UpdateVertices(spriteSequences[i], spriteInfo);

                    base.Draw(gameTime);
                }
            }

            GameEngine.Graphics.RenderState.AlphaBlendEnable = false;
        }
    }
}
