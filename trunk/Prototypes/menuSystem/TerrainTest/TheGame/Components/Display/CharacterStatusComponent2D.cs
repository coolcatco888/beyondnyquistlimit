using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame.Components.Display
{
    class CharacterStatusComponent2D : HUDItemComponent2D
    {

        private ImageComponent2D hud;

        private TextComponent2D healthValue, manaValue;

        private ImageComponent2D playerImage;

        private Vector2 relativeHealthBarPos, relativeManaBarPos;

        private int lastHealthValue, lastManaValue;

        private SpriteFont textFont;

        public CharacterStatusComponent2D(GameScreen parent, StatusDisplay param)
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

    public class StatusDisplay
    {
        public Vector2 Position = Vector2.Zero;
        public Texture2D HudImage = null;
        public Texture2D BarImage = null;
        public Texture2D PlayerImage = null;
        public Vector2 HealthBarPos = Vector2.Zero;
        public Vector2 ManaBarPos = Vector2.Zero;
        public int HealthBarMaxValue = -1;
        public int ManaBarMaxValue = -1;
        public Vector2 PlayerImageCentrePos = Vector2.Zero;
        public SpriteFont TextFont = null;
        public Vector2 LevelPos = Vector2.Zero;
        public int Level = 1;
        public Color FontColor = Color.White;
        public Color HealthBarColor = Color.Green;
        public Color ManaBarColor = Color.Blue;
        public Color DamageBarColor = Color.Red;
        public float FontScale = 1.0f;
    }

    
}
