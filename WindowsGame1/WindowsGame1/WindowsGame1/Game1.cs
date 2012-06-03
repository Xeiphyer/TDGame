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
        Tower Tbutton;
        sprite Back1;
        //Pcrane enemy1;
        sprite sidebar;
        sprite title;
        sprite map;
        bool titleScreen = true;
        bool mapScreen = false;
        bool gameScreen = false;
        SpriteFont font;
        waves wave1;
        waves wave2;
        Button lvl1;
        Button start;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            this.IsMouseVisible = true;

            Tbutton = new Tower("Button", 830, 220);
            Tbutton.Scale = 0.3f;
            
            Back1 = new sprite();
            Back1.Scale = 2.0f;
            graphics.PreferredBackBufferWidth = 1000;//size of window
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();

            //enemy1 = new Pcrane();
            //enemy1.Scale = 0.5f;

            sidebar = new sprite();

            title = new sprite();
            map = new sprite();

            wave1 = new waves();
            wave2 = new waves();

            lvl1 = new Button();
            start = new Button();

            base.Initialize();

        }

        private void levelStart()
        {
            Stats.setLives(20);
            Stats.setGold(200);
            Stats.setEnergy(100);
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

            Tbutton.LoadContent(this.Content);

            Back1.LoadContent(this.Content, "Back01");

            //enemy1.LoadContent(this.Content);

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

            lvl1.LoadContent(this.Content, "lvl");
            lvl1.setPosition(new Vector2(30, 500));

            start.LoadContent(this.Content, "start");
            start.setPosition(new Vector2(350, 475));
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
                start.Update(Mouse.GetState());
            }
            else if (mapScreen)
            {
                updateMap();
                lvl1.Update(Mouse.GetState());
            }
            else if (gameScreen)
            {
                //enemy1.Update(gameTime);
 
                //Tbutton.setTarget(enemy1);
                

                if (wave1.getDone() == false)
                {
                    wave1.Update(gameTime);
                    updateCollision(wave1, gameTime);
                    updateTarget(wave1);
                }
                if (wave2.getDone() == false && wave1.getDone() == true)
                {
                    wave2.Update(gameTime);
                    updateCollision(wave2,gameTime);
                    updateTarget(wave2);
                }
                if (Stats.getLives() <= 0 || wave2.getDone() == true && wave2.getDone() == true)
                {
                    mapScreen = true;
                    gameScreen = false;
                }

                Tbutton.Update(gameTime);
            }

            base.Update(gameTime);
        }

        private void updateTitle()
        {
            if (start.getClicked() == true || Keyboard.GetState().IsKeyDown(Keys.Space) == true)
            {
                titleScreen = false;
                mapScreen = true;
                return;

            }
        }

        private void updateMap()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) == true || lvl1.getClicked() == true)
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

        private void updateTarget(waves waves)//I think the problem is the positions of the rectangles, I wish I could see them to test...
        {
            Rectangle rect1;
            Rectangle rect2;
            List<Tower> towers = Tbutton.getTowers();
            List<Pcrane> flock = waves.getList();
            
            for (int i = 0; i < towers.Count; i++)
            {
                //rect1 = new Rectangle((int)towers[i].getPosition().X, (int)towers[i].getPosition().Y, 700, 700);
                rect1 = towers[i].getRange(); //think this is working now
                for (int j = 0; j < flock.Count; j++)
                {
                    rect2 = new Rectangle((int)flock[j].getPosition().X, (int)flock[j].getPosition().Y, 70, 70);
                    //rect2 = flock[j].getRec(); //this is wrong too
                    if (towers[i].getTarget() == null && rect1.Intersects(rect2))
                    {
                        Tbutton.setTarget(flock[j]);
                        //towers[i].setTarget(flock[j]); //I have no idea why this won't work
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
                start.Draw(this.spriteBatch);
            }

            else if (mapScreen)
            {
                map.Draw(this.spriteBatch);
                lvl1.Draw(this.spriteBatch);
            }
            else if(gameScreen)
            {
                Back1.Draw(this.spriteBatch);
                //enemy1.Draw(this.spriteBatch,Color.Blue);

                if (wave1.getDone() == false)
                {
                    wave1.Draw(this.spriteBatch);
                }
                if (wave2.getDone() == false && wave1.getDone() == true)
                {
                    wave2.Draw(this.spriteBatch);
                }

                sidebar.Draw(this.spriteBatch);

                if (Tbutton.changeMouse() == true)
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
            spriteBatch.DrawString(font, "Lives: "+Stats.getLives()+"   Gold: "+Stats.getGold()+"   Energy: "+Stats.getEnergy(), new Vector2(0, 0), Color.White);
        }
    }
}
