using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGame.Components.Display;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame.Game_Screens
{
    class HUDScreen : GameScreen
    {

        private List<HUDStatusComponent2D> playerHuds;

        public HUDScreen(string name, Level level, Dictionary<PlayerIndex, Texture2D> playersAndFaces)
            : base(name)
        {
            
        }

        private static HUDStatusComponent2D CreateCharacterStatusHUD(GameScreen parent, Vector2 position, int currentHealth, int maxHealth, int currentMana, int maxMana, int level, Texture2D playerFace, int currentAttack, int maxAttack)
        {
            HUDStatusComponent2D hud;
            CharacterStatusDisplayParams hudParams = new CharacterStatusDisplayParams();
            hudParams.BarImage = GameEngine.Content.Load<Texture2D>("GUI\\healthbar");
            hudParams.DamageBarColor = Color.White;
            hudParams.FontColor = Color.White;
            hudParams.FontScale = 0.4f;
            hudParams.HealthBarColor = Color.Red;
            hudParams.HealthBarMaxValue = maxHealth;
            hudParams.HealthBarPos = new Vector2(58, 14);
            hudParams.HudImage = GameEngine.Content.Load<Texture2D>("GUI\\hud");
            hudParams.Level = level;
            hudParams.LevelPos = new Vector2(80, 57);
            hudParams.ManaBarColor = Color.Blue;
            hudParams.ManaBarMaxValue = maxMana;
            hudParams.ManaBarPos = new Vector2(58, 36);
            hudParams.PlayerImage = playerFace;
            hudParams.PlayerImageCentrePos = new Vector2(34, 34);
            hudParams.Position = position;
            hudParams.TextFont = GameEngine.Content.Load<SpriteFont>("GUI\\menufont");
            hudParams.attackCurrentValue = 70;
            hudParams.attackGaugeEmptyColor = Color.Gray;
            hudParams.attackGaugeEndAngle = MathHelper.Pi * 2.0f - MathHelper.PiOver4;
            hudParams.attackGaugeFullColor = Color.Red;
            hudParams.attackGaugeImage = GameEngine.Content.Load<Texture2D>("GUI\\circGauge");
            hudParams.attackGaugeRadius = 32.0f;
            hudParams.attackGaugeStartAngle = MathHelper.PiOver4;
            hudParams.attackMaxValue = maxAttack;
            hudParams.attackPosition = new Vector2(34, 32);

            hud = new CharacterStatusComponent2D(parent, hudParams);
            hud.Initialize();
            return hud;
        }
    }
}
