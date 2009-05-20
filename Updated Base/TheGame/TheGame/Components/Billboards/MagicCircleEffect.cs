
#region Using Statements

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
using Library;

#endregion  // Using Statements


namespace TheGame
{
    public class MagicCircleEffect : GroundEffect
    {
        #region Fields

        private float rotationSpeed;
        private float scaleIncrement;
        private Point sheetIndex;
        private float timer = 0.0f;
        private Random rand = new Random();

        #endregion  // Fields

        #region Constructors

        public MagicCircleEffect(GameScreen parent, SpriteInfo spriteInfo, float scaleIncrement, float rotationSpeed, Point sheetIndex)
            : base(parent, spriteInfo)
        {
            this.scaleIncrement = scaleIncrement;
            this.scale.X = scaleIncrement;
            this.scale.Y = scaleIncrement;
            this.rotationSpeed = rotationSpeed;
            this.sheetIndex = sheetIndex;

            vertices[0].TextureCoordinate = new Vector2(
                sheetIndex.X * spriteInfo.SpriteUnit.X,
                sheetIndex.Y * spriteInfo.SpriteUnit.Y);

            vertices[1].TextureCoordinate = new Vector2(
                sheetIndex.X * spriteInfo.SpriteUnit.X + spriteInfo.SpriteUnit.X,
                sheetIndex.Y * spriteInfo.SpriteUnit.Y);

            vertices[2].TextureCoordinate = new Vector2(
                sheetIndex.X * spriteInfo.SpriteUnit.X + spriteInfo.SpriteUnit.X,
                sheetIndex.Y * spriteInfo.SpriteUnit.Y + spriteInfo.SpriteUnit.Y);

            vertices[3].TextureCoordinate = new Vector2(
                sheetIndex.X * spriteInfo.SpriteUnit.X,
                sheetIndex.Y * spriteInfo.SpriteUnit.Y + spriteInfo.SpriteUnit.Y);

        }

        public MagicCircleEffect(GameScreen parent, SpriteInfo spriteInfo, float scaleIncrement, float rotationSpeed, int sheetColumn, int sheetRow)
            : base(parent, spriteInfo)
        {
            this.scaleIncrement = scaleIncrement;
            this.scale.X = scaleIncrement;
            this.scale.Y = scaleIncrement;
            this.rotationSpeed = rotationSpeed;
            this.sheetIndex.X = sheetColumn;
            this.sheetIndex.Y = sheetRow;

            vertices[0].TextureCoordinate = new Vector2(
                sheetIndex.X * spriteInfo.SpriteUnit.X,
                sheetIndex.Y * spriteInfo.SpriteUnit.Y);

            vertices[1].TextureCoordinate = new Vector2(
                sheetIndex.X * spriteInfo.SpriteUnit.X + spriteInfo.SpriteUnit.X,
                sheetIndex.Y * spriteInfo.SpriteUnit.Y);

            vertices[2].TextureCoordinate = new Vector2(
                sheetIndex.X * spriteInfo.SpriteUnit.X + spriteInfo.SpriteUnit.X,
                sheetIndex.Y * spriteInfo.SpriteUnit.Y + spriteInfo.SpriteUnit.Y);

            vertices[3].TextureCoordinate = new Vector2(
                sheetIndex.X * spriteInfo.SpriteUnit.X,
                sheetIndex.Y * spriteInfo.SpriteUnit.Y + spriteInfo.SpriteUnit.Y);

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            while (timer > 0)
            {
                rotation += Quaternion.CreateFromYawPitchRoll(0.0f, 0.0f, rotationSpeed);

                if (scale.X > 0 && scale.X < 1)
                {
                    scale.X += scaleIncrement;
                    scale.Y += scaleIncrement;
                }
                else
                {
                    scaleIncrement = 5;
                }

                timer -= 40;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameEngine.Graphics.RenderState.AlphaBlendEnable = true;
            GameEngine.Graphics.RenderState.SourceBlend = Blend.One;
            GameEngine.Graphics.RenderState.DestinationBlend = Blend.One;
            GameEngine.Graphics.RenderState.BlendFunction = BlendFunction.Add;

            base.Draw(gameTime);

            GameEngine.Graphics.RenderState.AlphaBlendEnable = false;
        }

        #endregion  // Constructors
    }
}
