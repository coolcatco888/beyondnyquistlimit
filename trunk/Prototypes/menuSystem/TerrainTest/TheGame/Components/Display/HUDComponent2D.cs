using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame.Components.Display
{
    class HUDComponent2D : DisplayComponent2D
    {

        private ImageComponent2D hud;

        private ValueBarComponent2D healthBar, manaBar;

        private TextComponent2D value1, value2;

        private ImageComponent2D playerImage;

        public HUDComponent2D(GameScreen parent, HUDParameters param)
            : base(parent)
        {
            this.position = position;

            //Add Bars
            this.healthBar = new ValueBarComponent2D(parent, param.Position + param.HealthBarPos, param.BarImage, Color.Green, param.HealthBarMaxValue, Color.Red);
            this.manaBar = new ValueBarComponent2D(parent, param.Position + param.ManaBarPos, param.BarImage, Color.Blue, param.ManaBarMaxValue, Color.Red);

            //Add Hud
            this.hud = new ImageComponent2D(parent, param.Position, param.HudImage);

            //Add Player Face
            this.playerImage = new ImageComponent2D(parent, param.Position - new Vector2(param.PlayerImage.Width * 0.5f, param.PlayerImage.Height * 0.5f), param.PlayerImage);

            //TODO: Add level number


        }

        public class HUDParameters
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
        }


    }

    
}
