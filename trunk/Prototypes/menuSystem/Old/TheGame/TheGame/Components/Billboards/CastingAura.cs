
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
    public class CastingAura : GroundEffect
    {
        #region Fields

        private float rotationSpeed;
        private float scaleIncrement;
        private Point sheetIndex;
        private float timer = 0.0f;
        private Random rand = new Random();
        private Chanting auraParticles;
        private Library.SpellInfo spellInfo;

        #endregion  // Fields

        #region Accessors

        public float ScaleIncrement
        {
            get { return scaleIncrement; }
        }

        public Chanting AuraParticles
        {
            get { return AuraParticles; }
        }

        #endregion  // Accessors

        #region Constructors

        public CastingAura(GameScreen parent, SpriteInfo spriteInfo, float scaleIncrement, float rotationSpeed, Point sheetIndex)
            : base(parent, spriteInfo)
        {
            this.scaleIncrement = scaleIncrement;
            this.scale = new Vector2(scaleIncrement, scaleIncrement);
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

            //this.Initialize();
        }

        public CastingAura(GameScreen parent, SpriteInfo spriteInfo, float scaleIncrement, float rotationSpeed, int sheetColumn, int sheetRow)
            : base(parent, spriteInfo)
        {
            this.scaleIncrement = scaleIncrement;
            this.scale = new Vector2(scaleIncrement, scaleIncrement);
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

            //this.Initialize();
        }

#endregion  // Constructors

        public override void Initialize()
        {
            spellInfo = new Library.SpellInfo();
            spellInfo.Duration = 0.0f;

            base.Initialize();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (visible)
            {
                timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            GamepadDevice gamepadDevice = (GamepadDevice)GameEngine.Services.GetService(typeof(GamepadDevice));

            while (timer > 0)
            {
                rotation += rotationSpeed;

                if (scale.X > 0 && scale.X < 3)
                {
                    if (auraParticles != null)
                    {
                        auraParticles.SpellInfo.Duration = 0.0f;
                    }
                    scale.X += scaleIncrement;
                    scale.Y += scaleIncrement;
                }
                else if (scale.X >= 3)
                {
                    if (auraParticles == null || spellInfo.Duration == 0.0f)
                    {
                        auraParticles = new Chanting(this.Parent, spellInfo, this.position);
                        auraParticles.Scale = 2.2f;
                    }
                    auraParticles.SpellInfo.Duration += 10.0f;
                }

                if (scale.X <= 0)
                {
                    visible = false;
                    return;
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

        public void InvertScaleIncrement()
        {
            scaleIncrement = -scaleIncrement;
            scale.X += scaleIncrement;
            scale.Y += scaleIncrement;
        }
    }
}
