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
       // Tower mSprite;
        Tower Tbutton;
        sprite Back1;
        Pcrane enemy1;
        sprite sidebar;
        sprite title;
        sprite map;
        bool titleScreen = true;
        bool mapScreen = false;
        bool gameScreen = false;
        SpriteFont font;
        int lives;
        int gold;
        int energy;


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

            //mSprite = new Tower("Tower", 125, 200);
           // mSprite.Scale = 0.5f;
            
            Tbutton = new Tower("Button", 830, 220);
            Tbutton.Scale = 0.3f;
            
            Back1 = new sprite();
            Back1.Scale = 2.0f;
            graphics.PreferredBackBufferWidth = 1000;//size of window
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();

            enemy1 = new Pcrane();
            enemy1.Scale = 0.5f;

            sidebar = new sprite();

            title = new sprite();
            map = new sprite();

            base.Initialize();
        }

        private void levelStart()
        {
            lives = 1;
            gold = 100;
            energy = 100;
            titleScreen = false;
            mapScreen = false;
            gameScreen = true;
            Tbutton.reset();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

          //  mSprite.LoadContent(this.Content);

            Tbutton.LoadContent(this.Content);

            Back1.LoadContent(this.Content, "Back01");

            enemy1.LoadContent(this.Content);

            sidebar.LoadContent(this.Content, "side");
            sidebar.Position.X = 800;

            title.LoadContent(this.Content, "title");
            map.LoadContent(this.Content, "Map");
            titleScreen = true;
            mapScreen = false;
            gameScreen = false;

            font = Content.Load<SpriteFont>("SpriteFont1");
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

            if (titleScreen)
            {
                updateTitle();
            }
            else if (mapScreen)
            {
                updateMap();
            }
            else if (gameScreen)
            {
                enemy1.Update(gameTime);
              //  mSprite.Update(gameTime, enemy1.getV());
                Tbutton.Update(gameTime, enemy1.getV());
                lives = lives - enemy1.leaks();
                if (lives == 0)
                {
                    mapScreen = true;
                    gameScreen = false;
                }
            }

            base.Update(gameTime);
        }

        private void updateTitle()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space) == true)
            {
                titleScreen = false;
                mapScreen = true;
                return;

            }
        }

        private void updateMap()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) == true)
            {
                mapScreen = false;
                gameScreen = true;
                levelStart();
                return;

            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            if (titleScreen)
            {

                title.Draw(this.spriteBatch);

            }

            else if (mapScreen)
            {

                map.Draw(this.spriteBatch);

            }
            else if(gameScreen)
            {
            Back1.Draw(this.spriteBatch);
            enemy1.Draw(this.spriteBatch);
            sidebar.Draw(this.spriteBatch);
         //   mSprite.Draw(this.spriteBatch);
            if (Tbutton.changeMouse())
            {
                this.IsMouseVisible = false;        //code for changing the mouse image
                MouseState Mstate = Mouse.GetState();            
                Tbutton.Scale = 0.5f;
                Tbutton.setImage(this.Content,"clearTower");
                Vector2 pos = new Vector2(Mstate.X, Mstate.Y);
                Tbutton.Draw(this.spriteBatch, pos);
                Tbutton.setImage(this.Content,"tower1");
            }
            else
            {
                this.IsMouseVisible = true;
            }
            Tbutton.Scale = 0.3f;
            Tbutton.Draw(this.spriteBatch);
            DrawText();
            }

            spriteBatch.End();
            
            base.Draw(gameTime);
        }

        private void DrawText()
        {
            spriteBatch.DrawString(font, "Lives: "+lives+"   Gold: "+gold+"   Energy: "+energy, new Vector2(0, 0), Color.White);
        }
    }
}
