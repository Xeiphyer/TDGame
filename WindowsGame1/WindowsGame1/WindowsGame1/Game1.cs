/*All based on this tutorial http://www.xnadevelopment.com/tutorials.shtml */

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

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Vector2 mPosition = new Vector2(100, 200);
        Texture2D mSpriteTexture;
        Tower mSprite;
        Tower mSprite2;
        sprite Back1;
        Pcrane enemy1;

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
            this.IsMouseVisible = true;

            mSprite = new Tower();
            mSprite.Scale = 0.5f;
            
            mSprite2 = new Tower();
            mSprite2.Scale = 0.5f;
            
            Back1 = new sprite();
            Back1.Scale = 2.0f;
            graphics.PreferredBackBufferWidth = 800;//size of window
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();

            enemy1 = new Pcrane();
            enemy1.Scale = 0.5f;

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

            mSprite.LoadContent(this.Content);
            mSprite.Position = new Vector2(125, 245);

            mSprite2.LoadContent(this.Content);
            mSprite2.Position.X = 0; //these work too
            mSprite2.Position.Y = 0;

            Back1.LoadContent(this.Content, "Back01");

            enemy1.LoadContent(this.Content);

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

            enemy1.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            Back1.Draw(this.spriteBatch);
            enemy1.Draw(this.spriteBatch);
            mSprite.Draw(this.spriteBatch);
            mSprite2.Draw(this.spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
