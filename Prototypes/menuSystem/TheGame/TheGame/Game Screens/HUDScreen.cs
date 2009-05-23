using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGame.Components.Display;
using TheGame.Components.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Library;

namespace TheGame.Game_Screens
{
    class HUDScreen : GameScreen
    {
        private Dictionary<Player, HUDStatusComponent2D> playerHuds = new Dictionary<Player, HUDStatusComponent2D>();

        private Dictionary<Monster, int> monsterCurrentHealth = new Dictionary<Monster, int>();

        public HUDScreen(string name, Level level)
            : base(name)
        {
            int screenWidth = GameEngine.Graphics.Viewport.Width;
            int screenHeight = GameEngine.Graphics.Viewport.Height;
            int screenWidthIncrement = level.PlayerList.Count > 0 ? screenWidth / level.PlayerList.Count : 0;
            int currentOffset = 0;
            foreach (Player player in level.PlayerList)
            {
                ActorInfo actorStats = player.ActorStats;
                CharacterClassInfo classInfo = player.ClassInfo;
                PlayerInfo playerInfo = player.PlayerInfo;
                string characterName = classInfo.ClassName;
                Texture2D playerFace = GameEngine.Content.Load<Texture2D>("GUI\\" + characterName + "face");
                //TODO: Add player stats once put in player
                HUDStatusComponent2D hud = CreateCharacterStatusHUD(this, Vector2.Zero, actorStats.CurrentHealth, actorStats.MaxHealth, actorStats.CurrentMana, actorStats.MaxMana, playerInfo.ClassLevel, playerFace, playerInfo.CurrentAttackGauge, playerInfo.MaxAttackGauge);
                Vector2 newPosition = new Vector2(10.0f + currentOffset, screenHeight - hud.Height);
                hud.Position = newPosition;
                hud.Initialize();
                currentOffset += screenWidthIncrement;
                playerHuds.Add(player, hud);
            }

            foreach (Monster monster in level.MonsterList)
            {
                monsterCurrentHealth.Add(monster, monster.ActorStats.CurrentHealth);
            }
        }

        public override void  Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Viewport viewport = GameEngine.Graphics.Viewport;
            Camera camera = (Camera) GameEngine.Services.GetService(typeof(Camera));
            //Update Player Stats
            foreach (KeyValuePair<Player, HUDStatusComponent2D> hud in playerHuds)
            {
                hud.Value.AttackGauge.CurrentValue = hud.Key.PlayerInfo.CurrentAttackGauge;
                hud.Value.Healthbar.CurrentValue = hud.Key.ActorStats.CurrentHealth;
                hud.Value.ManaBar.CurrentValue = hud.Key.ActorStats.CurrentMana;
            }

            foreach (KeyValuePair<Monster, int> monsterHealth in monsterCurrentHealth)
            {

                Vector3 monsterScreenPos = viewport.Project(monsterHealth.Key.Position, camera.Projection, camera.View,
                    Matrix.CreateScale(new Vector3(monsterHealth.Key.Scale.X, monsterHealth.Key.Scale.Y, 1.0f)) * Matrix.CreateWorld(Vector3.Zero, camera.Position - monsterHealth.Key.Position, Vector3.Up) * Matrix.CreateTranslation(monsterHealth.Key.Position));

            }

        }

        private static HUDStatusComponent2D CreateCharacterStatusHUD(GameScreen parent, Vector2 position, int currentHealth, int maxHealth, int currentMana, int maxMana, int level, Texture2D playerFace, int currentAttack, int maxAttack)
        {
            HUDStatusComponent2D hud;
            CharacterStatusDisplayParams hudParams = new CharacterStatusDisplayParams();
            hudParams.BarImage = GameEngine.Content.Load<Texture2D>("GUI\\healthbar");
            hudParams.DamageBarColor = Color.White;
            hudParams.FontColor = Color.White;
            hudParams.FontScale = 1.0f;
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
            hudParams.attackCurrentValue = currentAttack;
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
