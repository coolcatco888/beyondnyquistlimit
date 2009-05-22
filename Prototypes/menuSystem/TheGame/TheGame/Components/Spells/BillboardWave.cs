#region Using Statements

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace TheGame
{
    public class BillboardWave : Billboard
    {
        protected Vector3[] positionList;
        protected SpriteSequence[] spriteSequences;
        protected Library.SpriteInfo spriteInfo;

        private int numberOfWaves;
        private int interval;
        private float timer = 0.0f;
        private bool isComplete = false;

        private Vector3 velocity;

        public bool IsComplete
        {
            get { return isComplete; }
        }

        public BillboardWave(GameScreen parent, Library.SpriteInfo spriteInfo, Vector3 startPosition, Vector3 direction, int interval, int numberOfWaves, int row, int firstFrame, int lastFrame, int bufferFrames)
            : base(parent, spriteInfo.SpriteSheet)
        {
            visible = true;
            Enabled = true;

            positionList = new Vector3[numberOfWaves];
            spriteSequences = new SpriteSequence[numberOfWaves];

            this.spriteInfo = spriteInfo;

            velocity = Vector3.Normalize(direction) * 1.6f;
            velocity.Z = -velocity.Z;

            for (int i = 0; i < numberOfWaves; i++)
            {
                spriteSequences[i] = new SpriteSequence(false, bufferFrames);
                spriteSequences[i].AddRow(row, firstFrame, lastFrame);

                positionList[i] = startPosition + velocity * (i + 2) + new Vector3(0, -1.0f, 0);
            }

            this.interval = interval;
            this.numberOfWaves = numberOfWaves;
        }

        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            for(int i = 0; i < numberOfWaves; i++)
            {
                if (timer > interval * (i + 1))
                {
                    spriteSequences[i].Update(gameTime);
                }
            }
            if (spriteSequences[numberOfWaves - 1].IsComplete)
            {
                this.isComplete = true;
                this.Dispose();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GameEngine.Graphics.RenderState.AlphaBlendEnable = true;
            GameEngine.Graphics.RenderState.BlendFunction = BlendFunction.Add;
            GameEngine.Graphics.RenderState.SourceBlend = Blend.SourceAlpha;
            GameEngine.Graphics.RenderState.DestinationBlend = Blend.InverseSourceAlpha;

            for (int i = 0; i < numberOfWaves; i++)
            {
                if (timer > interval * (i + 1) || spriteSequences[i].IsComplete)
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
    }
}
