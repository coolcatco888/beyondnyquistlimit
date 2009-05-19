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
    public class FlameTrail : Spell
    {
        #region Fields

        PointSpriteSystem[] pss_fire;
        PointSpriteSystemSettings[] settings;
        BillboardTrail bt_flame;
        Player player;

        float timer = 0.0f;

        #endregion  // Fields

        #region Constructors

        public FlameTrail(GameScreen parent, SpellInfo spellInfo, Player player)
            : base(parent, spellInfo)
        {
            this.player = player;
        }

        #endregion  // Constructors

        public override void Initialize()
        {
            base.Initialize();

            this.Position = new Vector3(player.Position.X, 0.0f, player.Position.Z);

            // Flame
            Library.SpriteInfo flameInfo = GameEngine.Content.Load<Library.SpriteInfo>(@"Sprites\\CloudInfo");

            SpriteSequence baseSequence1 = new SpriteSequence(false, 1);
            baseSequence1.AddRow(2, 0, 4);

            SpriteSequence baseSequence2 = new SpriteSequence(true, 1);
            baseSequence2.AddRow(2, 5, 8);

            SpriteSequence baseSequence3 = new SpriteSequence(false, 1);
            baseSequence3.AddRow(2, 4, 0);

            MultipleSequence flameSequence = new MultipleSequence(false, 1);

            flameSequence.AddSequence(baseSequence1);
            flameSequence.AddSequence(baseSequence2);
            flameSequence.AddSequence(baseSequence3);

            flameSequence.Initialize();

            bt_flame = new BillboardTrail(this.Parent, flameInfo, flameSequence, this.position, 12);
            bt_flame.Initialize();


            // PSS
            pss_fire = new PointSpriteSystem[bt_flame.MaxTrailCount];

            settings = new PointSpriteSystemSettings[pss_fire.Length];

            for (int i = 0; i < settings.Length; i++)
            {
                settings[i] = new PointSpriteSystemSettings();

                settings[i].Color = Color.OrangeRed;
                settings[i].MaxParticles = 500;
                settings[i].BaseRotation = Quaternion.Identity;
                settings[i].Scale = 1.0f;
                settings[i].Texture = GameEngine.Content.Load<Texture2D>("ParticleA");
                settings[i].Technique = "Cylindrical";
            }

            particlesPerSecond = 200.0f;


            this.Add(bt_flame);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.Position = new Vector3(player.Position.X, 0.0f, player.Position.Z);
            this.bt_flame.Origin = this.Position;

            bt_flame.Update(gameTime);

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            //if (timer > 10)
            //{
            //    timer = 0;

            //    int particlesToMake = (int)(particlesPerSecond * 0.01f);

            //    float theta = (float)MathHelper.Pi * 2.0f;
            //    float r = 0.2f;

            //    for (int count = 0; count < pss_fire.Length; count++)
            //    {
            //        if (bt_flame.SpriteSequences[count].IsPaused == false)
            //        {
            //            if (pss_fire[count] == null)
            //            {
            //                settings[count].BasePosition = bt_flame.PositonList[count];
            //                pss_fire[count] = new PointSpriteSystem(this.Parent, settings[count]);
            //                pss_fire[count].Initialize();
            //                Add(pss_fire[count]);
            //            }
            //            for (int i = 0; i < particlesToMake; i++)
            //            {
            //                pss_fire[count].AddParticle(new Vector3(r * (float)GameEngine.Random.NextDouble(), theta * (float)GameEngine.Random.NextDouble(), 0.0f), new Vector3(0.3f, 2.0f, 3.0f), 0.8f, 1.3f, Color.OrangeRed, null);
            //            }
            //        }
            //        else if (pss_fire[count] != null)
            //        {
            //            pss_fire[count].Dispose();
            //            pss_fire[count] = null;
            //        }
            //    }

                //for (int i = 0; i < particlesToMake; i++)
                //{
                //    for (int j = 0; j < bt_flame.MaxTrailCount; j++)
                //    {
                //        if (bt_flame.SpriteSequences[j].IsPaused == false)
                //        {
                //            Vector3 difference = player.Position - bt_flame.PositonList[j];//pss_fire.Setting.BasePosition - bt_flame.PositonList[j];
                //            pss_fire.Setting.BasePosition = player.Position - difference;

                //            pss_fire.AddParticle(new Vector3(r * (float)GameEngine.Random.NextDouble(), theta * (float)GameEngine.Random.NextDouble(), 0.0f), new Vector3(0.0f, 0.0f, 3.0f), 0.8f, 1.3f, Color.OrangeRed, null);
                //        }
                //    }
                //}
            //}

            if (this.TimeRemaining <= 0)
            {
                bt_flame.Dispose();
            }

            // TEMPORARY
            this.TimeRemaining = 5.0f;
        }
    }
}
