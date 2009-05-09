using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
            this.manaBar = new ValueBarComponent2D(parent, param.Position + param.ManaBarPos, param.BarImage, param.ManaBarColor, param.ManaBarMaxValue, param.DamageBarColor);

            //Add Hud
            this.hud = new ImageComponent2D(parent, param.Position, param.HudImage);

            //Add Player Face
            this.playerImage = new ImageComponent2D(parent, param.PlayerImageCentrePos - new Vector2(param.PlayerImage.Width * 0.5f, param.PlayerImage.Height * 0.5f), param.PlayerImage);

            //Add level number
            this.level = new LevelTextComponent2D(parent, param.LevelPos, param.Level, param.FontColor, param.TextFont, param.FontScale);

            this.relativeHealthBarPos = param.HealthBarPos + new Vector2(param.BarImage.Width * 0.5f, 0);
            this.relativeManaBarPos = param.ManaBarPos + new Vector2(param.BarImage.Width * 0.5f, 0);

            //Add number Text
            this.textFont = param.TextFont;
            this.healthValue = new TextComponent2D(parent, 
                //Set position to the middle of the health bar
                this.relativeHealthBarPos
                - new Vector2(param.TextFont.MeasureString(this.healthBar.CurrentValue + "").X * 0.5f, 0),
                this.healthBar.CurrentValue + "", param.FontColor, this.textFont, param.FontScale);

            this.manaValue = new TextComponent2D(parent,
                //Set position to the middle of the mana bar
                this.relativeManaBarPos
                - new Vector2(param.TextFont.MeasureString(this.manaBar.CurrentValue + "").X * 0.5f, 0),
                this.manaBar.CurrentValue + "", param.FontColor, this.textFont, param.FontScale);

            this.lastHealthValue = healthBar.CurrentValue;
            this.lastManaValue = manaBar.CurrentValue;

        }

        public override void Update(GameTime gameTime)
        {
            //Update Health value position to centre of the bar if different
            if (lastHealthValue != healthBar.CurrentValue)
            {
                healthValue.Position = relativeHealthBarPos - 
                    new Vector2(textFont.MeasureString(healthBar.CurrentValue + "").X * 0.5f, 0);
                healthValue.Text = healthBar.CurrentValue + "";
                lastHealthValue = healthBar.CurrentValue;
            }

            //Update Health value position to centre of the bar if different
            if (lastManaValue != manaBar.CurrentValue)
            {
                manaValue.Position = relativeManaBarPos -
                    new Vector2(textFont.MeasureString(manaBar.CurrentValue + "").X * 0.5f, 0);
                manaValue.Text = manaBar.CurrentValue + "";
                lastManaValue = manaBar.CurrentValue;
            }

            base.Update(gameTime);
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
    }

    
}
