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
using ContentTestLibrary;

namespace ContentTest
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont font;
        string firstXmlContent;
        Sprite sprite;
        List<Sprite> sprites;

        Armor leatherGloves;
        Equipment[] inventory;

        SpriteInfo knightspritemove;

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
            inventory = new Equipment[1];
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
            font = Content.Load<SpriteFont>(@"Arial");
            leatherGloves = Content.Load<Armor>(@"Items\\LeatherGloves");
            inventory[0] = leatherGloves;
            sprites = Content.Load<List<Sprite>>(@"SpriteList");
            knightspritemove = Content.Load<SpriteInfo>(@"KnightSprite");
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
            foreach (Armor a in inventory)
            {
                spriteBatch.DrawString(font, "Title: " + a.Title, new Vector2(10, 10), Color.Crimson);
                spriteBatch.DrawString(font, "Desc: " + a.Description, new Vector2(10, 30), Color.Crimson);
                spriteBatch.DrawString(font, "Gold: " + a.GoldValue, new Vector2(10, 50), Color.Crimson);
                spriteBatch.DrawString(font, "MinLevel: " + a.MinimumLevel, new Vector2(10, 70), Color.Crimson);
                List<string> list = a.RestrictedClasses;
                int count = 0;
                spriteBatch.DrawString(font, "Restricted Classes: ", new Vector2(10, 90), Color.Crimson);
                foreach (string s in list)
                {
                    spriteBatch.DrawString(font, s, new Vector2(10, 110 + (count*20)), Color.Red);
                    count++;
                }
                spriteBatch.DrawString(font, "TextureFile: " + a.TextureFileName, new Vector2(10, 150), Color.Crimson);
                spriteBatch.DrawString(font, "ArmorSlot: " + a.ArmorSlot, new Vector2(10, 170), Color.Crimson);
                spriteBatch.DrawString(font, "Ability: " + a.Ability, new Vector2(10, 190), Color.Crimson);
                spriteBatch.DrawString(font, "Defense: " + a.DefenseValue, new Vector2(10, 210), Color.Crimson);
            }
            foreach (Sprite s in sprites)
                s.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
