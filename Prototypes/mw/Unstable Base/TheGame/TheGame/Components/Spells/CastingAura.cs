
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

        public bool Visible
        {
            get { return groundEffect.Visible; }
            set { this.groundEffect.Visible = value;
            this.circleEffect.Visible = value; }
        }

        #endregion  // Accessors

        #region Constructors

        public CastingAura(GameScreen parent, SpellInfo spellInfo, Point sheetIndex)
            : base(parent, spellInfo)
        {
            this.sheetIndex = sheetIndex;
        }

        public CastingAura(GameScreen parent, SpellInfo spellInfo, int sheetColumn, int sheetRow)
            : base(parent, spellInfo)
        {
            this.sheetIndex.X = sheetColumn;
            this.sheetIndex.Y = sheetRow;
        }

#endregion  // Constructors

        public override void Initialize()
        {
            base.Initialize();

            rotationSpeed = 0.008f;
            scaleIncrement = 0.15f;

            groundEffectInfo = GameEngine.Content.Load<Library.SpriteInfo>(@"Sprites\\MagicCircle");
            groundEffect = new GroundEffect(this.Parent, groundEffectInfo);

            groundEffect.Vertices[0].TextureCoordinate = new Vector2(
                sheetIndex.X * groundEffectInfo.SpriteUnit.X,
                sheetIndex.Y * groundEffectInfo.SpriteUnit.Y);

            groundEffect.Vertices[1].TextureCoordinate = new Vector2(
                sheetIndex.X * groundEffectInfo.SpriteUnit.X + groundEffectInfo.SpriteUnit.X,
                sheetIndex.Y * groundEffectInfo.SpriteUnit.Y);

            groundEffect.Vertices[2].TextureCoordinate = new Vector2(
                sheetIndex.X * groundEffectInfo.SpriteUnit.X + groundEffectInfo.SpriteUnit.X,
                sheetIndex.Y * groundEffectInfo.SpriteUnit.Y + groundEffectInfo.SpriteUnit.Y);

            groundEffect.Vertices[3].TextureCoordinate = new Vector2(
                sheetIndex.X * groundEffectInfo.SpriteUnit.X,
                sheetIndex.Y * groundEffectInfo.SpriteUnit.Y + groundEffectInfo.SpriteUnit.Y);

            spellInfo = new Library.SpellInfo();
            spellInfo.Duration = 0.0f;

            this.InvertScaleIncrement();

            // Particle System
            PointSpriteSystemSettings settings = new PointSpriteSystemSettings();
            settings.Color = Color.LightBlue;
            settings.MaxParticles = 5000;
            settings.BasePosition = Vector3.Zero;
            settings.BaseRotation = Quaternion.Identity;
            settings.Scale = 1.0f;
            settings.Texture = GameEngine.Content.Load<Texture2D>("ParticleA");
            settings.Technique = "Cylindrical";

            particlesPerSecond = 500.0f;

            circleEffect = new PointSpriteSystem(Parent, settings);

            circleEffect.Visible = false;
            groundEffect.Visible = false;

            groundEffect.Initialize();
            circleEffect.Initialize();

            this.Add((IMoveable)groundEffect);
            this.Add((IMoveable)circleEffect);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            GamepadDevice gamepadDevice = ((InputHub)GameEngine.Services.GetService(typeof(InputHub)))[spellInfo.Caster];

            if (gamepadDevice.IsButtonDown(Buttons.RightTrigger))
            {
                if (!this.Visible || this.ScaleIncrement < 0)
                {
                    this.Visible = true;
                    //castingAura.CircleEffect.
                    this.InvertScaleIncrement();
                    this.Position = new Vector3(this.position.X, 0.0f, this.position.Z - 1.0f);
                }
            }
            else if (gamepadDevice.WasButtonReleased(Buttons.RightTrigger))
            {
                this.InvertScaleIncrement();
            }

            if (groundEffect.Scale.X >= 0)
            {
                this.Enabled = false;
            }


            // Update Ground Effect

            if (groundEffect.Visible)
            {
                timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            while (timer > 0)
            {
                groundEffect.Rotation += rotationSpeed;

                if (groundEffect.Scale.X > 0 && groundEffect.Scale.X < 3)
                {
                    groundEffect.Scale = new Vector2(groundEffect.Scale.X + scaleIncrement, groundEffect.Scale.Y + scaleIncrement);
                }
                else if (groundEffect.Scale.X >= 3)
                {

                    int particlesToMake = (int)(particlesPerSecond * 0.01f);

                    for (int i = 0; i < particlesToMake; i++)
                    {
                        float rand1 = (float)GameEngine.Random.NextDouble();
                        float rand2 = (float)GameEngine.Random.NextDouble();
                        float rand3 = (float)GameEngine.Random.NextDouble();
                        circleEffect.AddParticle(new Vector3(2.2f, (float)Math.PI * 2.0f * rand1, 0.0f), new Vector3(0.0f, 0.0f, 0.1f + 3.0f * rand2), 0.01f + rand3 * 0.2f, 1.0f, null, null);
                    }
                    /*
                    if (currentSpell == null)
                    {
                        // Begin spell.
                        if (gamepadDevice.IsButtonDown(Buttons.B) && spellInfo.Duration == 0.0f)
                        {
                            currentSpell = new FireTornado(this.Parent, spellInfo, this.position);
                            currentSpell.SpellInfo.Duration = 3.0f;
                            nextTornadoInput = Buttons.RightThumbstickUp;
                        }
                        else if (gamepadDevice.IsButtonDown(Buttons.A) && spellInfo.Duration == 0.0f)
                        {
                            currentSpell = new LeafWhirlwind(this.Parent, spellInfo, this.position);
                            currentSpell.SpellInfo.Duration = 3.0f;
                            nextTornadoInput = Buttons.RightThumbstickUp;
                        }
                        else if (gamepadDevice.IsButtonDown(Buttons.X) && spellInfo.Duration == 0.0f)
                        {
                            currentSpell = new IceVortex(this.Parent, spellInfo, this.position);
                            currentSpell.SpellInfo.Duration = 3.0f;
                            nextTornadoInput = Buttons.RightThumbstickUp;
                        }
                        else if (gamepadDevice.IsButtonDown(Buttons.Y) && spellInfo.Duration == 0.0f)
                        {
                            currentSpell = new Hurricane(this.Parent, spellInfo, this.position);
                            currentSpell.SpellInfo.Duration = 3.0f;
                            nextTornadoInput = Buttons.RightThumbstickUp;
                        }
                    }
                    else if (currentSpell != null && gamepadDevice.IsButtonDown(nextTornadoInput))
                    {
                        currentSpell.Scale += 1.0f;
                        currentSpell.ParticlesPerSecond += currentSpell.ParticlesPerSecond * 0.1f;
                        currentSpell.SpellInfo.Duration += 0.1f;

                        switch (nextTornadoInput)
                        {
                            case Buttons.RightThumbstickUp:
                                nextTornadoInput = Buttons.RightThumbstickRight;
                                break;
                            case Buttons.RightThumbstickRight:
                                nextTornadoInput = Buttons.RightThumbstickDown;
                                break;
                            case Buttons.RightThumbstickDown:
                                nextTornadoInput = Buttons.RightThumbstickLeft;
                                break;
                            case Buttons.RightThumbstickLeft:
                                nextTornadoInput = Buttons.RightThumbstickUp;
                                break;
                        }
                    }
                    else if (currentSpell != null)
                    {
                        if (currentSpell.Scale > 0.6f)
                        {
                            currentSpell.Scale -= 0.5f;
                            currentSpell.ParticlesPerSecond -= currentSpell.ParticlesPerSecond * 0.05f;
                        }
                        
                        if (currentSpell.SpellInfo.Duration <= 0.0f)
                        {
                            currentSpell = null;
                        }
                    }*/
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

        public void InvertScaleIncrement()
        {
            scaleIncrement = -scaleIncrement;
            groundEffect.Scale = new Vector2(groundEffect.Scale.X + scaleIncrement, groundEffect.Scale.Y + scaleIncrement);
        }
    }
}
