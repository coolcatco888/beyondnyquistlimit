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

namespace SpriteSample
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Actor theif;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            theif = new Actor(Content.Load<Texture2D>("theifWalkRun"));
            theif.AddBasicSequences();
            theif.PlaySequence("IdleSouth");


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState keyState = Keyboard.GetState();

#if !XBOX
            if (keyState.IsKeyDown(Keys.LeftShift))
                theif.State = Actor.ActorState.Running;
            else
                theif.State = Actor.ActorState.Walking;


            if (keyState.IsKeyDown(Keys.Up))
            {
                if (keyState.IsKeyDown(Keys.Right))
                    theif.ActorOrientation = Orientation.Northeast;
                else if (keyState.IsKeyDown(Keys.Left))
                    theif.ActorOrientation = Orientation.Northwest;
                else
                    theif.ActorOrientation = Orientation.North;
            }
            else if (keyState.IsKeyDown(Keys.Down))
            {
                if (keyState.IsKeyDown(Keys.Right))
                    theif.ActorOrientation = Orientation.Southeast;
                else if (keyState.IsKeyDown(Keys.Left))
                    theif.ActorOrientation = Orientation.Southwest;
                else
                    theif.ActorOrientation = Orientation.South;
            }
            else if (keyState.IsKeyDown(Keys.Right))
            {
                theif.ActorOrientation = Orientation.East;
            }
            else if (keyState.IsKeyDown(Keys.Left))
            {
                theif.ActorOrientation = Orientation.West;
            }
            else
            {
                theif.Idle();
            }
            /*
            if(keyState.IsKeyDown(Keys.LeftShift))
            {
                if (keyState.IsKeyDown(Keys.Up))
                    theif.PlaySequence("RunningNorth");
                else if (keyState.IsKeyDown(Keys.Down))
                    theif.PlaySequence("RunningSouth");
                else if (keyState.IsKeyDown(Keys.Right))
                    theif.PlaySequence("RunningEast");
                else if (keyState.IsKeyDown(Keys.Left))
                    theif.PlaySequence("RunningWest");
                else
                    theif.Idle();
            }
            else
            {
                if (keyState.IsKeyDown(Keys.Up))
                    theif.PlaySequence("WalkingNorth");
                else if (keyState.IsKeyDown(Keys.Down))
                    theif.PlaySequence("WalkingSouth");
                else if (keyState.IsKeyDown(Keys.Right))
                    theif.PlaySequence("WalkingEast");
                else if (keyState.IsKeyDown(Keys.Left))
                    theif.PlaySequence("WalkingWest");
                else
                    theif.Idle();
            }
             */
#endif

            theif.UpdateSequence(gameTime);
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            theif.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
