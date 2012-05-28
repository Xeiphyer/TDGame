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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Vector2 mPosition = new Vector2(100, 200);
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
        waves wave1;
        waves wave2;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

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

            wave1 = new waves();
            wave2 = new waves();

            base.Initialize();
        }

        private void levelStart()
        {
            lives = 20;
            gold = 200;
            energy = 100;
            titleScreen = false;
            mapScreen = false;
            gameScreen = true;
            Tbutton.reset();
            wave1.reset();
            wave2.reset();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

          //  mSprite.LoadContent(this.Content);

            Tbutton.LoadContent(this.Content);

            Back1.LoadContent(this.Content, "Back01");

            enemy1.LoadContent(this.Content);

            sidebar.LoadContent(this.Content, "side");
            sidebar.setPosition(new Vector2(800,0));

            title.LoadContent(this.Content, "title");
            map.LoadContent(this.Content, "Map");
            titleScreen = true;
            mapScreen = false;
            gameScreen = false;

            font = Content.Load<SpriteFont>("SpriteFont1");

            wave1.LoadContent(this.Content);

            wave2.LoadContent(this.Content);
            wave2.setColor(Color.Tomato);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

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

                Tbutton.setTarget(enemy1.getPosition());
                
                lives = lives - enemy1.leaks();
                if (wave1.getDone() == false)
                {
                    wave1.Update(gameTime);
                    updateCollision(wave1, gameTime);
                }
                if (wave2.getDone() == false && wave1.getDone() == true)
                {
                    wave2.Update(gameTime);
                    updateCollision(wave2,gameTime);
                }
                if (lives == 0)
                {
                    mapScreen = true;
                    gameScreen = false;
                }
                gold = gold - Tbutton.Update(gameTime);
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

        private void updateCollision(waves wave, GameTime gametime)
        {
            Rectangle rect1;
            Rectangle rect2;
            List<Pcrane> cranes = wave.getList();
            List<Fireball> mFireballs = Tbutton.getList();

            //*****************************fireball enemy collision***********************************//
            for(int i = 0; i < cranes.Count; i++)
            {
                Vector2 temPos1 = cranes[i].getPosition();
                rect1 = new Rectangle((int)temPos1.X,(int)temPos1.Y,70,70);//70s should be width then height
                lives = lives - cranes[i].leaks();
                gold = gold + cranes[i].bounty();

                for(int j =0; j < mFireballs.Count; j++)
                {
                    if (mFireballs[j].Visible == true && cranes[i].Visible == true)
                    {
                        Vector2 temPos2 = mFireballs[j].getPosition();
                        rect2 = new Rectangle((int)temPos2.X, (int)temPos2.Y, 20, 20);
                        if (rect1.Intersects(rect2))
                        {
                            cranes[i].hit(1);
                            mFireballs[j].Visible = false;
                        }
                    }
                }
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
                enemy1.Draw(this.spriteBatch,Color.Blue);

                if (wave1.getDone() == false)
                {
                    wave1.Draw(this.spriteBatch);
                }
                if (wave2.getDone() == false && wave1.getDone() == true)
                {
                    wave2.Draw(this.spriteBatch);
                }

                sidebar.Draw(this.spriteBatch);
         //   mSprite.Draw(this.spriteBatch);
            if (Tbutton.changeMouse(gold))
            {
                this.IsMouseVisible = false;        //code for changing the mouse image
                MouseState Mstate = Mouse.GetState();            
                Tbutton.Scale = 0.5f;
                Tbutton.setImage(this.Content,"clearTower");
                Vector2 pos = new Vector2(Mstate.X - 35, Mstate.Y - 35);     // *X1* Center tower on mouse. Change to 1/2 texture size.   
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
