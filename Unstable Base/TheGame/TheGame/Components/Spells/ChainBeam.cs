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

namespace TheGame
{
    class ChainBeam : Spell
    {
        Vector3[] controlPoints;
        CubicBezierSystem beam;

        float particlesPerSecond;
        int timeElpase; 

        public ChainBeam(GameScreen parent, float duration)
            : base(parent, duration)
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            particlesPerSecond = 1200.0f;

            controlPoints = new Vector3[4];
            controlPoints[0] = new Vector3(-3.0f, 0.2f, 0.0f);
            controlPoints[1] = new Vector3(-2.0f, -2.2f, 0.0f);
            controlPoints[2] = new Vector3(2.0f, 2.2f, 0.0f);
            controlPoints[3] = new Vector3(3.0f, 2.2f, 0.0f); 

            PointSpriteSystemSettings settings = new PointSpriteSystemSettings();
            settings.effectName = "Bezier";
            settings.MaxParticles = 4000;
            settings.Color = Color.OrangeRed;
            settings.BasePosition = -Vector3.UnitZ;
            settings.Texture = GameEngine.Content.Load<Texture2D>("ParticleA");
            settings.Technique = "Bezier";

            beam = new CubicBezierSystem(Parent, settings, controlPoints);
            beam.Initialize();

            this.Add(beam);
            
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            

            timeElpase += gameTime.ElapsedGameTime.Milliseconds;

            if (timeElpase > 10)
            {
                

                int particlesToMake = (int)(particlesPerSecond * 0.001f * timeElpase);

                if (particlesToMake > 0)
                {

                    float theta = (float)MathHelper.Pi * 2.0f;

                    float rand1, rand2, rand3;

                    for (int i = 0; i < particlesToMake; i++)
                    {
                        rand1 = (float)GameEngine.Random.NextDouble();
                        rand2 = (float)GameEngine.Random.NextDouble();
                        rand3 = (float)GameEngine.Random.NextDouble();

                        beam.AddParticle(new Vector3(0.5f * rand1, theta, 0.0f), new Vector3(0.0f, 20.0f, 0.0f), 0.05f + 0.1f * rand2, 1.1f, 0.0f + rand3 * 0.05f, 1.0f);

                    }

                    timeElpase = 0;
                }
            }
        }
    }
}
