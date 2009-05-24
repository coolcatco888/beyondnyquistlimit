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

        private Dictionary<PlayerIndex, ImageComponent2D> offScreenArrows = new Dictionary<PlayerIndex, ImageComponent2D>();

        private SpriteFont font;

        private readonly Color[] playerArrowColors = 
        {
            Color.Green,
            Color.Red,
            Color.Blue,
            Color.Yellow
        };

        private const float HIT_TEXT_SCALE = 3.0f;

        private const float PADDING = 20.0f;

        public HUDScreen(string name, Level level)
            : base(name)
        {
            int screenWidth = GameEngine.Graphics.Viewport.Width;
            int screenHeight = GameEngine.Graphics.Viewport.Height;
            font = GameEngine.Content.Load<SpriteFont>("GUI\\menufont");
            int screenWidthIncrement = level.PlayerList.Count > 0 ? screenWidth / level.PlayerList.Count : 0;
            int currentOffset = 0;
            int i = 0;
            foreach (Player player in level.PlayerList)
            {
                //Setup Player Hud
                ActorInfo actorStats = player.ActorStats;
                CharacterClassInfo classInfo = player.ClassInfo;
                PlayerInfo playerInfo = player.PlayerInfo;
                string characterName = classInfo.ClassName;
                Texture2D playerFace = GameEngine.Content.Load<Texture2D>("GUI\\" + characterName + "face");
                //TODO: Add player stats once put in player
                HUDStatusComponent2D hud = CreateCharacterStatusHUD(this, Vector2.Zero, actorStats.CurrentHealth, actorStats.MaxHealth, actorStats.CurrentMana, actorStats.MaxMana, playerInfo.ClassLevel, playerFace, playerInfo.CurrentAttackGauge, playerInfo.MaxAttackGauge);
                Vector2 newPosition = new Vector2(PADDING + currentOffset, screenHeight - hud.Height - PADDING);
                hud.Position = newPosition;
                hud.Initialize();
                playerHuds.Add(player, hud);

                //Setup offscreen arrows
                Texture2D arrowTex = GameEngine.Content.Load<Texture2D>("GUI\\offarrow");
                ImageComponent2D arrow = new ImageComponent2D(this, Vector2.Zero, arrowTex, playerArrowColors[(int)player.PlayerIndex]);
                arrow.Initialize();
                arrow.Visible = false;
                arrow.IsOriginCenter = true;
                offScreenArrows.Add(player.PlayerIndex, arrow);

                //Increment offsets and indicies
                currentOffset += screenWidthIncrement;
                i++;
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
                Player player = hud.Key;
                hud.Value.AttackGauge.CurrentValue = player.PlayerInfo.CurrentAttackGauge;
                int manaDecrease = player.ActorStats.CurrentMana - hud.Value.ManaBar.CurrentValue;
                hud.Value.ManaBar.IncreaseDecreaseValue(manaDecrease);

                //Popup damage text to Player
                int damage = player.ActorStats.CurrentHealth - hud.Value.Healthbar.CurrentValue;
                if (damage != 0)
                {
                    Vector3 playerScreenPos = viewport.Project(player.Position, camera.Projection, camera.View, Matrix.Identity);
                    Vector2 textLocation = new Vector2(playerScreenPos.X, playerScreenPos.Y);
                    DisplayHitText(damage, textLocation);
                    hud.Value.Healthbar.IncreaseDecreaseValue(damage);
                }
                DisplayOffScreenArrow(hud.Key, viewport, camera);
            }

            for (int i = 0; i < monsterCurrentHealth.Count; i++)
            {
                Monster monster = monsterCurrentHealth.Keys.ElementAt(i);
                if (monster.IsDisposed)
                    continue;

                //Popup damage text to Monster
                int damage = monster.ActorStats.CurrentHealth - monsterCurrentHealth[monster];
                if (damage != 0)
                {
                    Vector3 monsterScreenPos = viewport.Project(monster.Position, camera.Projection, camera.View, Matrix.Identity);
                    Vector2 textLocation = new Vector2(monsterScreenPos.X, monsterScreenPos.Y);
                    DisplayHitText(damage, textLocation);
                    monsterCurrentHealth[monster] = monster.ActorStats.CurrentHealth;
                }
            }

        }

        private void DisplayHitText(int damage, Vector2 textLocation)
        {
            HitTextComponent2D damageText = new HitTextComponent2D(this, textLocation, damage, damage < 0 ? Color.Red : Color.Green, font, HIT_TEXT_SCALE);
            damageText.Initialize();
        }

        private void DisplayOffScreenArrow(Player player, Viewport viewport, Camera camera)
        {

            ImageComponent2D arrow = offScreenArrows[player.PlayerIndex];
            arrow.Visible = false;
            arrow.Rotation = 0.0f;

            Vector3 screenPos = viewport.Project(player.Position, camera.Projection, camera.View, Matrix.Identity);
            if (screenPos.X < 0.0f && screenPos.Y > viewport.Height - arrow.Height - 90.0f)
            {
                arrow.Visible = true;
                arrow.Rotation = MathHelper.PiOver4;
                arrow.Position = new Vector2(arrow.Height, viewport.Height - arrow.Height - 90.0f);
            }
            else if (screenPos.X > viewport.Width && screenPos.Y > viewport.Height - arrow.Height - 90.0f)
            {
                arrow.Visible = true;
                arrow.Rotation = -MathHelper.PiOver4;
                arrow.Position = new Vector2(viewport.Width - arrow.Height, viewport.Height - arrow.Height - 90.0f);
            }
            else if (screenPos.X < 0.0f)
            {
                arrow.Visible = true;
                arrow.Rotation = MathHelper.PiOver2;
                arrow.Position = new Vector2(arrow.Height, screenPos.Y);
            }
            else if (screenPos.X > viewport.Width)
            {
                arrow.Visible = true;
                arrow.Rotation = -MathHelper.PiOver2;
                arrow.Position = new Vector2(viewport.Width - arrow.Height, screenPos.Y);
            }
            else if (screenPos.Y < 0.0f)
            {
                arrow.Visible = true;
                arrow.Rotation = MathHelper.Pi;
                arrow.Position = new Vector2(screenPos.X, arrow.Height);
            }
            else if (screenPos.Y > viewport.Height - arrow.Height - 90.0f)
            {
                arrow.Visible = true;
                arrow.Position = new Vector2(screenPos.X, viewport.Height - arrow.Height - 90.0f);
            }
        }

        private HUDStatusComponent2D CreateCharacterStatusHUD(GameScreen parent, Vector2 position, int currentHealth, int maxHealth, int currentMana, int maxMana, int level, Texture2D playerFace, int currentAttack, int maxAttack)
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
            hudParams.TextFont = font;
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
