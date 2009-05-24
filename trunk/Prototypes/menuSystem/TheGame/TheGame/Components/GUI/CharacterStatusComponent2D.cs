using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheGame.Components.GUI;

namespace TheGame.Components.Display
{
    /// <summary>
    /// Represents a very specific implementation of a HUDStatusComponent2D.
    /// This displays the character's health, mana, level and picture.
    /// </summary>
    class CharacterStatusComponent2D : HUDStatusComponent2D
    {
        /// <summary>
        /// Stores the image of the CharacterStatus Display
        /// </summary>
        private ImageComponent2D hud;

        /// <summary>
        /// Stores the textual value of the mana and health bar.  This text is positioned at the centre
        /// of the bars.
        /// </summary>
        private TextComponent2D healthValue, manaValue;

        /// <summary>
        /// Stores the image of the character's head
        /// </summary>
        private ImageComponent2D playerImage;


        private Vector2 relativeHealthBarPos, relativeManaBarPos;

        private int lastHealthValue, lastManaValue;

        private SpriteFont textFont;

        private BoundingBox Bounds;

        public override void Initialize()
        {
            ((DisplayComponent2D)attackGauge).Initialize();
            ((DisplayComponent2D)healthBar).Initialize();
            ((DisplayComponent2D)manaBar).Initialize();
            hud.Initialize();
            playerImage.Initialize();
            healthValue.Initialize();
            manaValue.Initialize();
            ((DisplayComponent2D)level).Initialize();
            base.Initialize();
        }

        public override Vector2 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                Vector2 oldPosition = base.position;
                ((ValueBarComponent2D)healthBar).Position = ((ValueBarComponent2D)healthBar).Position - oldPosition + value;
                ((ValueBarComponent2D)manaBar).Position = ((ValueBarComponent2D)manaBar).Position - oldPosition + value;
                hud.Position = hud.Position - oldPosition + value;
                playerImage.Position = playerImage.Position - oldPosition + value;
                ((DisplayComponent2D)level).Position = ((DisplayComponent2D)level).Position - oldPosition + value;
                healthValue.Position = healthValue.Position - oldPosition + value;
                manaValue.Position = manaValue.Position - oldPosition + value;
                ((DisplayComponent2D)attackGauge).Position = ((DisplayComponent2D)attackGauge).Position - oldPosition + value;
                ((CircularGauge)attackGauge).SetupGauge();

                base.Position = value;
            }
        }


        /// <summary>
        /// Constructs a new Character Display Component.
        /// </summary>
        /// <param name="parent">Screen this component is contained in</param>
        /// <param name="param">Attributes needed to construct this object see CharacterStatusDisplayParams</param>
        public CharacterStatusComponent2D(GameScreen parent, CharacterStatusDisplayParams param)
            : base(parent)
        {
            this.position = position;

            //Add Bars
            this.healthBar = new ValueBarComponent2D(parent, param.Position + param.HealthBarPos, param.BarImage, param.HealthBarColor, param.HealthBarMaxValue, param.DamageBarColor);
            this.manaBar = new ValueBarComponent2D(parent, param.Position + param.ManaBarPos, param.BarImage, param.ManaBarColor, param.ManaBarMaxValue, param.ManaBarColor);
            UpdateBounds((DisplayComponent2D) this.healthBar);
            UpdateBounds((DisplayComponent2D) this.manaBar);

            //Add Hud
            this.hud = new ImageComponent2D(parent, param.Position, param.HudImage);
            UpdateBounds((DisplayComponent2D)this.hud);

            //Add Player Face
            this.playerImage = new ImageComponent2D(parent, param.Position + param.PlayerImageCentrePos - new Vector2(param.PlayerImage.Width * 0.5f, param.PlayerImage.Height * 0.5f), param.PlayerImage);
            UpdateBounds((DisplayComponent2D)this.playerImage);

            //Add level number
            this.level = new LevelTextComponent2D(parent, param.Position + param.LevelPos, param.Level, param.FontColor, param.TextFont, param.FontScale);
            UpdateBounds((DisplayComponent2D)this.level);
            this.relativeHealthBarPos = param.HealthBarPos + new Vector2(param.BarImage.Width * 0.5f, 0);
            this.relativeManaBarPos = param.ManaBarPos + new Vector2(param.BarImage.Width * 0.5f, 0);

            //Add number Text
            this.textFont = param.TextFont;
            this.healthValue = new TextComponent2D(parent, 
                //Set position to the middle of the health bar
                param.Position + 
                this.relativeHealthBarPos
                - new Vector2(param.TextFont.MeasureString(this.healthBar.CurrentValue + "").X * 0.5f, 0),
                this.healthBar.CurrentValue + "", param.FontColor, this.textFont, param.FontScale);
            UpdateBounds((DisplayComponent2D)this.healthValue);

            this.manaValue = new TextComponent2D(parent,
                //Set position to the middle of the mana bar
                param.Position + 
                this.relativeManaBarPos
                - new Vector2(param.TextFont.MeasureString(this.manaBar.CurrentValue + "").X * 0.5f, 0),
                this.manaBar.CurrentValue + "", param.FontColor, this.textFont, param.FontScale);
            UpdateBounds((DisplayComponent2D)this.manaBar);

            this.attackGauge = new CircularGauge(parent, param.Position + param.attackPosition, param.attackGaugeImage, param.attackMaxValue, param.attackCurrentValue, 
                param.attackGaugeRadius, param.attackGaugeStartAngle, param.attackGaugeEndAngle, param.attackGaugeFullColor, param.attackGaugeEmptyColor);

            this.lastHealthValue = healthBar.CurrentValue;
            this.lastManaValue = manaBar.CurrentValue;

        }

        public override void Update(GameTime gameTime)
        {
            //Update Health value position to centre of the bar if different
            if (lastHealthValue != healthBar.CurrentValue)
            {
                healthValue.Position = position + relativeHealthBarPos - 
                    new Vector2(textFont.MeasureString(healthBar.CurrentValue + "").X * 0.5f, 0);
                healthValue.Text = healthBar.CurrentValue + "";
                lastHealthValue = healthBar.CurrentValue;
            }

            //Update Health value position to centre of the bar if different
            if (lastManaValue != manaBar.CurrentValue)
            {
                manaValue.Position = position + relativeManaBarPos -
                    new Vector2(textFont.MeasureString(manaBar.CurrentValue + "").X * 0.5f, 0);
                manaValue.Text = manaBar.CurrentValue + "";
                lastManaValue = manaBar.CurrentValue;
            }

            base.Update(gameTime);
        }

        public override Vector2 Center
        {
            get { return new Vector2(Left + 0.5f * Width, Top + 0.5f * Height); }
        }

        public override float Height
        {
            get { return Bounds.Max.Y - Bounds.Min.Y; }
        }

        public override float Width
        {
            get { return Bounds.Max.X - Bounds.Min.X; }
        }

        public override float Left
        {
            get { return Bounds.Min.X; }
        }

        public override float Right
        {
            get { return Left + Width; }
        }

        public override float Bottom
        {
            get { return Top + Height; }
        }

        public override float Top
        {
            get { return Bounds.Min.Y; }
        }

        private void UpdateBounds(DisplayComponent2D item)
        {
            if (item.Left < Left)
            {
                Bounds.Min.X = item.Position.X;
            }
            if (item.Top < Top)
            {
                Bounds.Min.Y = item.Position.Y;
            }

            if (item.Right > Right)
            {
                Bounds.Max.X = (Right + Width + (item.Right - Right));
            }

            if (item.Bottom > Bottom)
            {
                Bounds.Max.Y = (Top + Height + (item.Bottom - Bottom));
            }
        }
    }

    /// <summary>
    /// In order to display this hud correctly, the picture of the HUD must be a
    /// transparent gif with cut outs for the health and manabar and must contain a spot
    /// for the player's picture and the player's level.
    /// 
    /// Positions and images for all of these components must be spedified here.
    /// </summary>
    public class CharacterStatusDisplayParams
    {
        /// <summary>
        /// Position of top-left hand corner of this component
        /// </summary>
        public Vector2 Position = Vector2.Zero;

        /// <summary>
        /// Image of the CharacterStatus display,the picture of the HUD must be a
        /// transparent gif with cut outs for the health and manabar an
        /// </summary>
        public Texture2D HudImage = null;

        /// <summary>
        /// Image of the bar, the bar must be rectangular and must match
        /// the cutout size exactly
        /// </summary>
        public Texture2D BarImage = null;

        /// <summary>
        /// Image of say the player's character's head, must be a transparent gif
        /// </summary>
        public Texture2D PlayerImage = null;

        /// <summary>
        /// Position of the top left hand corner of the health bar relative to the 
        /// Character status display position.
        /// </summary>
        public Vector2 HealthBarPos = Vector2.Zero;

        /// <summary>
        /// Position of the top left hand corner of the mana bar relative to the 
        /// Character status display position.
        /// </summary>
        public Vector2 ManaBarPos = Vector2.Zero;

        /// <summary>
        /// Maximum value of the health bar
        /// </summary>
        public int HealthBarMaxValue = 1;

        /// <summary>
        /// Maximum value of the mana bar
        /// </summary>
        public int ManaBarMaxValue = 1;

        /// <summary>
        /// Centre Position of where the player image should be
        /// </summary>
        public Vector2 PlayerImageCentrePos = Vector2.Zero;

        /// <summary>
        /// Font of the text displayed
        /// </summary>
        public SpriteFont TextFont = null;

        /// <summary>
        /// Position of where the top left hand corner of where the level number should appear
        /// </summary>
        public Vector2 LevelPos = Vector2.Zero;

        /// <summary>
        /// Initial level number
        /// </summary>
        public int Level = 1;

        /// <summary>
        /// Color of the font displayed
        /// </summary>
        public Color FontColor = Color.White;

        /// <summary>
        /// Color of the health bar, bars should be grayscale fades or just white
        /// </summary>
        public Color HealthBarColor = Color.Green;

        /// <summary>
        /// Color of the mana bar, bars should be grayscale fades or just white
        /// </summary>
        public Color ManaBarColor = Color.Blue;

        /// <summary>
        /// Color of a damage flash
        /// </summary>
        public Color DamageBarColor = Color.Red;

        /// <summary>
        /// Size of font
        /// </summary>
        public float FontScale = 1.0f;

        /// <summary>
        /// Attack Gauge center position for circular attack gauge
        /// </summary>
        public Vector2 attackPosition = Vector2.Zero;

        /// <summary>
        /// Attack Gauge slot piece image 
        /// </summary>
        public Texture2D attackGaugeImage = null;

        /// <summary>
        /// Attack gauge maximum value
        /// </summary>
        public int attackMaxValue = 1;

        /// <summary>
        /// Current value our of the attack gauge
        /// </summary>
        public int attackCurrentValue = 1;

        /// <summary>
        /// Current radius of the circular attack gauge
        /// </summary>
        public float attackGaugeRadius = 30.0f;

        /// <summary>
        /// Starting angle of the circular attack gauge
        /// </summary>
        public float attackGaugeStartAngle = MathHelper.PiOver4;

        /// <summary>
        /// Ending angle of the attack gauge
        /// </summary>
        public float attackGaugeEndAngle = MathHelper.Pi * 2.0f - MathHelper.PiOver4;

        /// <summary>
        /// Color of gauge pieces when full
        /// </summary>
        public Color attackGaugeFullColor = Color.Red;

        /// <summary>
        /// Color of gauge pieces when empty
        /// </summary>
        public Color attackGaugeEmptyColor = Color.Black;

    }

    
}
