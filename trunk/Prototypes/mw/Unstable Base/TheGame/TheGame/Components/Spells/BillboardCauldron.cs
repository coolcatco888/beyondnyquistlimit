#region Using Statements

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace TheGame
{
    public class BillboardCauldron : Billboard, IMoveable
    {
        protected Vector3[] positionList;
        protected SpriteSequence[] spriteSequences;
        protected Library.SpriteInfo spriteInfo;

        private int numberOfBubbles;
        private float radius;
        private float timer = 0.0f;
        private bool isComplete = false;

        private Vector3 origin;

        public bool IsComplete
        {
            get { return isComplete; }
        }
        public Vector3 Origin
        {
            get { return origin; }
            set { origin.X = value.X;
            origin.Z = value.Z;
            }
        }

        public BillboardCauldron(GameScreen parent, Library.SpriteInfo spriteInfo, Vector3 origin, float radius, int numberOfBubbles, float scale, int row, int firstFrame, int lastFrame, int bufferFrames)
            : base(parent, spriteInfo.SpriteSheet)
        {
            visible = true;
            Enabled = true;

            this.Scale = new Vector2(scale, scale);

            this.numberOfBubbles = numberOfBubbles;
            this.position = new Vector3(origin.X, scale, origin.Z);
            this.origin = new Vector3(origin.X, scale, origin.Z);
            this.radius = radius;

            positionList = new Vector3[numberOfBubbles];
            spriteSequences = new SpriteSequence[numberOfBubbles];

            this.spriteInfo = spriteInfo;

            for (int i = 0; i < numberOfBubbles; i++)
            {
                spriteSequences[i] = new SpriteSequence(false, bufferFrames);
                spriteSequences[i].AddRow(row, firstFrame, lastFrame);
                spriteSequences[i].IsPaused = true;

                positionList[i] = randomVectorXZ();
                while(Vector3.Distance(this.position, positionList[i]) >= radius)
                {
                    positionList[i] = randomVectorXZ();
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            for(int i = 0; i < numberOfBubbles; i++)
            {
                if (spriteSequences[i].IsPaused == false)
                {
                    spriteSequences[i].Update(gameTime);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GameEngine.Graphics.RenderState.AlphaBlendEnable = true;
            GameEngine.Graphics.RenderState.BlendFunction = BlendFunction.Add;
            GameEngine.Graphics.RenderState.SourceBlend = Blend.SourceAlpha;
            GameEngine.Graphics.RenderState.DestinationBlend = Blend.InverseSourceAlpha;

            for (int i = 0; i < numberOfBubbles; i++)
            {
                if (spriteSequences[i].IsComplete)
                {
                    spriteSequences[i].IsPaused = true;
                    spriteSequences[i].Reset();
                }

                else if (spriteSequences[i].IsPaused)
                {
                    positionList[i] = randomVectorXZ();

                    if (GameEngine.Random.NextDouble() > 0.8)
                    {
                        spriteSequences[i].IsPaused = false;
                    }
                }

                if (spriteSequences[i].IsPaused == false)
                {
                    this.position = positionList[i];
                    this.UpdateVertices(spriteSequences[i], spriteInfo);

                    base.Draw(gameTime);
                }
            }

            GameEngine.Graphics.RenderState.AlphaBlendEnable = false;
        }

        public override void Dispose()
        {
            this.Parent.Components.Remove(this);
        }

        protected Vector3 randomVectorXZ()
        {
            return new Vector3(((float)GameEngine.Random.NextDouble() - 0.5f) * 2.0f * radius + origin.X, origin.Y, ((float)GameEngine.Random.NextDouble() - 0.5f) * 2.0f * radius + origin.Z);
        }
    }
}
