using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame.Components.Display
{
    class StatusDisplayComponent2D : DisplayComponent2D
    {

        private ImageComponent2D hud;

        private ValueBarComponent2D healthBar, manaBar;

        private TextComponent2D healthValue, manaValue, levelNumber;

        private ImageComponent2D playerImage;

        private int level;

        private const string lv = "Lv. ";

        public ValueBarComponent2D HealthBar
        {
            get { return healthBar; }
        }

        public ValueBarComponent2D ManaBar
        {
            get { return manaBar; }
        }

        /// <summary>
        /// To set the level call ChangeLevel()
        /// </summary>
        public int Level
        {
            get { return level; }
        }



        public StatusDisplayComponent2D(GameScreen parent, StatusDisplay param)
            : base(parent)
        {
            this.position = position;

            //Add Bars
            this.healthBar = new ValueBarComponent2D(parent, param.Position + param.HealthBarPos, param.BarImage, Color.Green, param.HealthBarMaxValue, Color.Red);
            this.manaBar = new ValueBarComponent2D(parent, param.Position + param.ManaBarPos, param.BarImage, Color.Blue, param.ManaBarMaxValue, Color.Red);

            //Add Hud
            this.hud = new ImageComponent2D(parent, param.Position, param.HudImage);

            //Add Player Face
            this.playerImage = new ImageComponent2D(parent, param.PlayerImageCentrePos - new Vector2(param.PlayerImage.Width * 0.5f, param.PlayerImage.Height * 0.5f), param.PlayerImage);

            //Add level number
            this.level = param.Level;
            this.levelNumber = new TextComponent2D(parent, param.LevelPos, lv + level, param.FontColor, param.TextFont, param.FontScale);

            //Add number Text
            this.healthValue = new TextComponent2D(parent, 
                //Set position to the middle of the health bar
                param.HealthBarPos + new Vector2(this.healthBar.Image.Width * 0.5f, 0)
                - new Vector2(param.TextFont.MeasureString(this.healthBar.CurrentValue + "").X * 0.5f, 0), 
                this.healthBar.CurrentValue + "", param.FontColor, param.TextFont, param.FontScale);

            this.manaValue = new TextComponent2D(parent,
                //Set position to the middle of the health bar
                param.ManaBarPos + new Vector2(this.manaBar.Image.Width * 0.5f, 0)
                - new Vector2(param.TextFont.MeasureString(this.manaBar.CurrentValue + "").X * 0.5f, 0),
                this.manaBar.CurrentValue + "", param.FontColor, param.TextFont, param.FontScale);

        }


        public void IncreaseDecreaseHealth(int value)
        {
            healthBar.IncreaseDecreaseValue(value);
            healthValue.Position = new Vector2(this.healthBar.Image.Width * 0.5f, 0)
                - new Vector2(healthValue.Font.MeasureString(this.healthBar.CurrentValue + "").X * 0.5f * healthValue.Scale, 0);
        }


        public void IncreaseDecreaseMana(int value)
        {
            manaBar.IncreaseDecreaseValue(value);
            manaValue.Position = new Vector2(this.manaBar.Image.Width * 0.5f, 0)
                - new Vector2(manaValue.Font.MeasureString(this.manaBar.CurrentValue + "").X * 0.5f * healthValue.Scale, 0);
        }

        public void ChangeLevel(int level)
        {
            this.level = level;
            levelNumber.Text = lv + level;
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
        public float FontScale = 1.0f;
    }

    
}
