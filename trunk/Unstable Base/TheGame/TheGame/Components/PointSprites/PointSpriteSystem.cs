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
    class PointSpriteSystem : Component, IDrawableComponent, IPointSpriteSystem , I3DComponent
    {
        public PointSpriteSystem(GameScreen parent, PointSpriteSystemSettings settings)
            : base(parent)
        {
            this.settings = settings;
        }

        

        #region Component Members

        public override void Initialize()
        {

            visible = true;

            particles = new ParticleVertex[settings.MaxParticles];

            SetEffect();

            effect.CurrentTechnique = effect.Techniques[settings.Technique];

            EffectParameterCollection parameters = effect.Parameters;

            // Look up shortcuts for parameters that change every frame.
            effectWorldParameter = parameters["World"];
            effectViewParameter = parameters["View"];
            effectProjectionParameter = parameters["Projection"];
            effectViewportHeightParameter = parameters["ViewportHeight"];
            effectTimeParameter = parameters["CurrentTime"];

            parameters["SpriteTexture"].SetValue(settings.Texture);

            vertexDeclaration = new VertexDeclaration(GameEngine.Graphics, ParticleVertex.VertexElements);

            dynamicVertexBuffer = new DynamicVertexBuffer(GameEngine.Graphics, ParticleVertex.SizeInBytes * settings.MaxParticles, BufferUsage.Points);

            base.Initialize();
        }

        protected virtual void SetEffect()
        {
            Effect e = GameEngine.Content.Load<Effect>(settings.effectName);

            // If we have several particle systems, the content manager will return
            // a single shared effect instance to them all. But we want to preconfigure
            // the effect with parameters that are specific to this particular
            // particle system. By cloning the effect, we prevent one particle system
            // from stomping over the parameter settings of another.

            effect = e.Clone(GameEngine.Graphics);
            
        }

        public override void Update(GameTime gameTime)
        {
            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            retireActiveParticles();
            freeRetiredParticles();

            if (firstActiveParticle == firstFreeParticle)
                currentTime = 0;

            if (firstRetiredParticle == firstActiveParticle)
                drawCounter = 0;

            base.Update(gameTime);
        }

        #endregion

        #region IDrawableComponent Members

        public void Draw(GameTime gameTime)
        {
            GraphicsDevice device = GameEngine.Graphics;

            // Restore the vertex buffer contents if the graphics device was lost.
            if (dynamicVertexBuffer.IsContentLost)
            {
                dynamicVertexBuffer.SetData(particles);
            }

            // If there are any particles waiting in the newly added queue,
            // we'd better upload them to the GPU ready for drawing.
            if (firstNewParticle != firstFreeParticle)
            {
                addNewParticlesToVertexBuffer();
            }

            // If there are any active particles, draw them now!
            if (firstActiveParticle != firstFreeParticle)
            {
                //SetParticleRenderStates(device.RenderState);
                //device.RenderState.PointSpriteEnable = true;
                //device.RenderState.PointSizeMax = 256;

                //device.RenderState.AlphaBlendEnable = true;
                //device.RenderState.AlphaBlendOperation = BlendFunction.Add;
                //device.RenderState.SourceBlend = Blend.SourceAlpha;
                //device.RenderState.DestinationBlend = Blend.One;

                //device.RenderState.DepthBufferEnable = true;
                //device.RenderState.DepthBufferWriteEnable = false;

                // Set an effect parameter describing the viewport size. This is needed
                // to convert particle sizes into screen space point sprite sizes.
                effectViewportHeightParameter.SetValue(device.Viewport.Height);
                
                Camera camera = (Camera)GameEngine.Services.GetService(typeof(Camera));

                effectWorldParameter.SetValue(Matrix.CreateFromQuaternion(Setting.BaseRotation) * Matrix.CreateTranslation(Setting.BasePosition));
                effectProjectionParameter.SetValue(camera.Projection);
                effectViewParameter.SetValue(camera.View);

                // Set an effect parameter describing the current time. All the vertex
                // shader particle animation is keyed off this value.
                effectTimeParameter.SetValue(currentTime);

                // Set the particle vertex buffer and vertex declaration.
                device.Vertices[0].SetSource(dynamicVertexBuffer, 0,
                                             ParticleVertex.SizeInBytes);

                device.VertexDeclaration = vertexDeclaration;

                // Activate the particle effect.
                effect.Begin();

                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Begin();

                    if (firstActiveParticle < firstFreeParticle)
                    {
                        // If the active particles are all in one consecutive range,
                        // we can draw them all in a single call.
                        device.DrawPrimitives(PrimitiveType.PointList,
                                              firstActiveParticle,
                                              firstFreeParticle - firstActiveParticle);
                    }
                    else
                    {
                        // If the active particle range wraps past the end of the queue
                        // back to the start, we must split them over two draw calls.
                        device.DrawPrimitives(PrimitiveType.PointList,
                                              firstActiveParticle,
                                              particles.Length - firstActiveParticle);

                        if (firstFreeParticle > 0)
                        {
                            device.DrawPrimitives(PrimitiveType.PointList,
                                                  0,
                                                  firstFreeParticle);
                        }
                    }

                    pass.End();
                }

                effect.End();

                // Reset a couple of the more unusual renderstates that we changed,
                // so as not to mess up any other subsequent drawing.
                //device.RenderState.PointSpriteEnable = false;
                //device.RenderState.AlphaBlendEnable = false;
                //device.RenderState.DepthBufferWriteEnable = true;
            }

            drawCounter++;
        }

        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
            }
        }
        bool visible;

        #endregion

        /// <summary>
        /// Adds a new particle to the system.
        /// </summary>
        public void AddParticle(Vector3 position, Vector3 velocity, float size, float duration, float spin, float? data)
        {
            // Figure out where in the circular queue to allocate the new particle.
            int nextFreeParticle = firstFreeParticle + 1;

            if (nextFreeParticle >= particles.Length)
                nextFreeParticle = 0;

            // If there are no free particles, we just have to give up.
            if (nextFreeParticle == firstRetiredParticle)
                return;

            // Fill in the particle vertex structure.
            particles[firstFreeParticle].Position = position * settings.Scale + settings.Position;
            particles[firstFreeParticle].Velocity = velocity * settings.Scale;
            particles[firstFreeParticle].Color = settings.Color;
            particles[firstFreeParticle].Size = size;
            particles[firstFreeParticle].Time.X = currentTime;
            particles[firstFreeParticle].Time.Y = duration;
            particles[firstFreeParticle].Rotation = spin;

            if(data.HasValue)
                particles[firstFreeParticle].Data = data.Value;
            
            firstFreeParticle = nextFreeParticle;
        }

        public PointSpriteSystemSettings Setting
        {
            get { return settings; }
            set { settings = value; }
        }
        protected PointSpriteSystemSettings settings;

        protected Effect effect;

        EffectParameter effectWorldParameter;
        EffectParameter effectViewParameter;
        EffectParameter effectProjectionParameter;
        EffectParameter effectViewportHeightParameter;
        EffectParameter effectTimeParameter;

        ParticleVertex[] particles;

        VertexDeclaration vertexDeclaration;
        DynamicVertexBuffer dynamicVertexBuffer;

        int firstActiveParticle;
        int firstNewParticle;
        int firstFreeParticle;
        int firstRetiredParticle;

        float currentTime;

        int drawCounter;

        #region Helper Methods

        /// <summary>
        /// Helper for checking when active particles have reached the end of
        /// their life. It moves old particles from the active area of the queue
        /// to the retired section.
        /// </summary>
        void retireActiveParticles()
        {

            while (firstActiveParticle != firstNewParticle)
            {
                // Is this particle old enough to retire?
                float particleAge = currentTime - particles[firstActiveParticle].Time.X;

                if (particleAge < particles[firstActiveParticle].Time.Y)
                    break;

                // Remember the time at which we retired this particle.
                particles[firstActiveParticle].Time.X = drawCounter;

                // Move the particle from the active to the retired queue.
                firstActiveParticle++;

                if (firstActiveParticle >= particles.Length)
                    firstActiveParticle = 0;
            }
        }

        /// <summary>
        /// Helper for checking when retired particles have been kept around long
        /// enough that we can be sure the GPU is no longer using them. It moves
        /// old particles from the retired area of the queue to the free section.
        /// </summary>
        void freeRetiredParticles()
        {
            while (firstRetiredParticle != firstActiveParticle)
            {
                // Has this particle been unused long enough that
                // the GPU is sure to be finished with it?
                int age = drawCounter - (int)particles[firstRetiredParticle].Time.X;

                // The GPU is never supposed to get more than 2 frames behind the CPU.
                // We add 1 to that, just to be safe in case of buggy drivers that
                // might bend the rules and let the GPU get further behind.
                if (age < 3)
                    break;

                // Move the particle from the retired to the free queue.
                firstRetiredParticle++;

                if (firstRetiredParticle >= particles.Length)
                    firstRetiredParticle = 0;
            }
        }

        /// <summary>
        /// Helper for uploading new particles from our managed
        /// array to the GPU vertex buffer.
        /// </summary>
        void addNewParticlesToVertexBuffer()
        {
            int stride = ParticleVertex.SizeInBytes;

            if (firstNewParticle < firstFreeParticle)
            {
                // If the new particles are all in one consecutive range,
                // we can upload them all in a single call.
                dynamicVertexBuffer.SetData(firstNewParticle * stride, particles,
                                     firstNewParticle,
                                     firstFreeParticle - firstNewParticle,
                                     stride, SetDataOptions.NoOverwrite);
            }
            else
            {
                // If the new particle range wraps past the end of the queue
                // back to the start, we must split them over two upload calls.
                dynamicVertexBuffer.SetData(firstNewParticle * stride, particles,
                                     firstNewParticle,
                                     particles.Length - firstNewParticle,
                                     stride, SetDataOptions.NoOverwrite);

                if (firstFreeParticle > 0)
                {
                    dynamicVertexBuffer.SetData(0, particles,
                                         0, firstFreeParticle,
                                         stride, SetDataOptions.NoOverwrite);
                }
            }

            // Move the particles we just uploaded from the new to the active queue.
            firstNewParticle = firstFreeParticle;
        }

        #endregion

        #region IPointSpriteSystem Members

        public int MaxParticleCount
        {
            get { return settings.MaxParticles; }
        }

        #endregion

        #region I3DComponent Members

        public Vector3 Position
        {
            get
            {
                return settings.BasePosition;
            }
            set
            {
                settings.BasePosition = value;
            }
        }

        public Quaternion Rotation
        {
            get
            {
                return settings.BaseRotation;
            }
            set
            {
                settings.BaseRotation = value;
            }
        }

        public float Scale
        {
            get
            {
                return settings.Scale;
            }
            set
            {
                settings.Scale = value;
            }
        }

        #endregion
    }
}
