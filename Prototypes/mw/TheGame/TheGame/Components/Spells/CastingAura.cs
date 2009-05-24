
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
    public class CastingAura : Spell
    {
        #region Fields

        private float rotationSpeed;
        private float scaleIncrement;
        private Point sheetIndex;
        private float timer = 0.0f;
        private Random rand = new Random();

        private GroundEffect groundEffect;
        private SpriteInfo groundEffectInfo;

        PointSpriteSystem circleEffect;

        float particlesPerSecond;

        #endregion  // Fields

        #region Accessors

        public float ScaleIncrement
        {
            get { return scaleIncrement; }
        }

        public GroundEffect AuraCircle
        {
            get { return groundEffect; }
        }

        public PointSpriteSystem CircleEffect
        {
            get { return circleEffect; }
        }

        new public bool Visible
        {
            get { return groundEffect.Visible; }
            set { this.groundEffect.Visible = value;
            this.circleEffect.Visible = value; }
        }

        #endregion  // Accessors

        #region Constructors

        public CastingAura(GameScreen parent, Library.SpellInfo spellInfo, Component3D caster, ActorList targets, Point sheetIndex)
            : base(parent, spellInfo, caster, targets)
        {
            this.sheetIndex = sheetIndex;
        }



        public CastingAura(GameScreen parent, Library.SpellInfo spellInfo, Component3D caster, ActorList targets, int sheetColumn, int sheetRow)
            : base(parent, spellInfo, caster, targets)
        {
            this.sheetIndex.X = sheetColumn;
            this.sheetIndex.Y = sheetRow;
        }

        #endregion  // Constructors

        #region Initialize

        public override void Initialize()
        {
            base.Initialize();

            this.Position = new Vector3(Caster.Position.X, 0.0f, Caster.Position.Z);

            rotationSpeed = 0.008f;
            scaleIncrement = 0.15f;

            groundEffectInfo = GameEngine.Content.Load<Library.SpriteInfo>(@"Sprites\\MagicCircle");
            groundEffect = new GroundEffect(this.Parent, groundEffectInfo);

            groundEffect.Vertices[0].TextureCoordinate = new Vector2(
                sheetIndex.X * groundEffectInfo.SpriteUnit.X,
                sheetIndex.Y * groundEffectInfo.SpriteUnit.Y + groundEffectInfo.SpriteUnit.Y);

            groundEffect.Vertices[1].TextureCoordinate = new Vector2(
                sheetIndex.X * groundEffectInfo.SpriteUnit.X,
                sheetIndex.Y * groundEffectInfo.SpriteUnit.Y);

            groundEffect.Vertices[2].TextureCoordinate = new Vector2(
                sheetIndex.X * groundEffectInfo.SpriteUnit.X + groundEffectInfo.SpriteUnit.X,
                sheetIndex.Y * groundEffectInfo.SpriteUnit.Y + groundEffectInfo.SpriteUnit.Y);

            groundEffect.Vertices[3].TextureCoordinate = new Vector2(
                sheetIndex.X * groundEffectInfo.SpriteUnit.X + groundEffectInfo.SpriteUnit.X,
                sheetIndex.Y * groundEffectInfo.SpriteUnit.Y);

            spellInfo = new Library.SpellInfo();
            spellInfo.Duration = 0.0f;

            this.InvertScaleIncrement();

            // Particle System
            PointSpriteSystemSettings settings = new PointSpriteSystemSettings(GameEngine.Content.Load<Texture2D>("ParticleA"));
            settings.Color = Color.LightBlue;
            settings.MaxParticles = 5000;
            settings.Technique = "Cylindrical";
            settings.FinalSize = 0.0f;

            particlesPerSecond = 500.0f;

            circleEffect = new PointSpriteSystem(Parent, settings);

            circleEffect.Visible = false;
            groundEffect.Visible = false;

            groundEffect.Initialize();
            circleEffect.Initialize();

            this.Add(groundEffect);
            this.Add(circleEffect);
        }

        #endregion  // Initialize

        #region Update

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            GamepadDevice gamepadDevice = ((InputHub)GameEngine.Services.GetService(typeof(InputHub)))[((Player)caster).PlayerIndex];

            if (gamepadDevice.IsButtonDown(Buttons.RightTrigger))
            {
                if (!this.Visible || this.ScaleIncrement < 0)
                {
                    this.Visible = true;
                    this.InvertScaleIncrement();
                    this.Translate(Caster.Position.X - position.X, 0.0f, Caster.Position.Z - position.Z);
                }
            }
            else if (gamepadDevice.WasButtonReleased(Buttons.RightTrigger))
            {
                this.InvertScaleIncrement();
            }


            // Update Ground Effect
            if (groundEffect.Visible)
            {
                timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            while (timer > 0)
            {
                groundEffect.Rotate(rotationSpeed, 0.0f, 0.0f);

                if (groundEffect.Scale.X > 0 && groundEffect.Scale.X < 3)
                {
                    groundEffect.Scale = new Vector3(groundEffect.Scale.X + scaleIncrement, groundEffect.Scale.Y + scaleIncrement, 1.0f);
                }
                else if (groundEffect.Scale.X >= 3)
                {

                    int particlesToMake = (int)(particlesPerSecond * 0.01f);

                    for (int i = 0; i < particlesToMake; i++)
                    {
                        float rand1 = (float)GameEngine.Random.NextDouble();
                        float rand2 = (float)GameEngine.Random.NextDouble();
                        float rand3 = (float)GameEngine.Random.NextDouble();
                        float rand4 = (float)GameEngine.Random.NextDouble();
                        circleEffect.AddParticle(new Vector3(2.2f, (float)Math.PI * 2.0f * rand1, 0.2f), new Vector3(0.0f, 0.0f, 0.1f + 3.0f * rand2), 0.01f + rand3 * 2.0f, new Vector2(rand4 * 0.5f, 0.0f), null, null);
                    }
                }

                if (groundEffect.Scale.X <= 0)
                {
                    groundEffect.Visible = false;
                    return;
                }

                timer -= 40;
            }

            base.Update(gameTime);
        }

        #endregion  // Update

        #region Methods

        public void InvertScaleIncrement()
        {
            scaleIncrement = -scaleIncrement;
            groundEffect.Scale = new Vector3(groundEffect.Scale.X + scaleIncrement, groundEffect.Scale.Y + scaleIncrement, 1.0f);
        }

        #endregion  // Methods
    }
}
