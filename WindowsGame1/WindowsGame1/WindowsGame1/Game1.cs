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
        //Tower Tbutton;
        sprite Back1;
        //Pcrane enemy1;
        sprite topbar;
        sprite sidebar;
        sprite title;
        sprite map;
        bool titleScreen = true;
        bool mapScreen = false;
        bool gameScreen = false;
        SpriteFont font;
        //waves wave1;
        //waves wave2;
        Button lvl1Button;
        Button start;
        TowerButton tower1;
        Level1 lvl1;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            this.IsMouseVisible = true;

            tower1 = new TowerButton(830, 220);
            //tower1.scale = 0.5f;
            
            Back1 = new sprite();
            Back1.Scale = 2.0f;
            graphics.PreferredBackBufferWidth = 1000;//size of window
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();

            //enemy1 = new Pcrane();
            //enemy1.Scale = 0.5f;

            sidebar = new sprite();
            topbar = new sprite();

            title = new sprite();
            map = new sprite();

            //wave1 = new waves();
            //wave2 = new waves();

            lvl1Button = new Button();
            start = new Button();

            lvl1 = new Level1();

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
            tower1.reset();
            //wave1.reset();
            //wave2.reset();
            lvl1.reset();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            tower1.LoadContent(this.Content);

            Back1.LoadContent(this.Content, "Back01");

            //enemy1.LoadContent(this.Content);

            sidebar.LoadContent(this.Content, "side");
            sidebar.setPosition(new Vector2(800,0));
            topbar.LoadContent(this.Content, "topbar");
            topbar.setPosition(new Vector2(0, 0));

            title.LoadContent(this.Content, "title");
            map.LoadContent(this.Content, "Map");
            titleScreen = true;
            mapScreen = false;
            gameScreen = false;

            font = Content.Load<SpriteFont>("SpriteFont1");

            //wave1.LoadContent(this.Content);

            //wave2.LoadContent(this.Content);
            //wave2.setColor(Color.Tomato);

            lvl1Button.LoadContent(this.Content, "lvl");
            lvl1Button.setPosition(new Vector2(30, 500));

            start.LoadContent(this.Content, "start");
            start.setPosition(new Vector2(350, 475));

            lvl1.LoadContent(this.Content);
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
                lvl1Button.Update(Mouse.GetState());
            }
            else if (gameScreen)
            {
                //enemy1.Update(gameTime);
 
                //Tbutton.setTarget(enemy1);

                lvl1.Update(gameTime);
                updateCollision(gameTime);
                updateTarget();

                /*if (wave1.getDone() == false)
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
                }*/
                //if (Stats.getLives() <= 0 || wave2.getDone() == true && wave2.getDone() == true)
                if(Stats.getLives() <= 0 || lvl1.getDone() == true)
                {
                    mapScreen = true;
                    gameScreen = false;
                }

                tower1.Update(gameTime);
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
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) == true || lvl1Button.getClicked() == true)
            {
                mapScreen = false;
                gameScreen = true;
                levelStart();
                lvl1Button.setClicked(false);
                return;

            }
        }

        public void updateCollision(GameTime gametime)
        {
            Rectangle rect1;
            Rectangle rect2;
            List<Pcrane> cranes = lvl1.getCranes();//this will have to be changes based on level, returns the cranes currently onscreen
            List<Fireball> mFireballs = tower1.getFire();

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

        private void updateTarget()//I think the problem is the positions of the rectangles, I wish I could see them to test...
        {
            Rectangle rect1;
            Rectangle rect2;
            List<Tower> towers = tower1.getTowers();
            List<Pcrane> flock = lvl1.getCranes();//this will have to be changes based on level, returns the cranes currently onscreen
            
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
                        //Tbutton.setTarget(flock[j]);
                        towers[i].setTarget(flock[j]); //woooo it works!
                    }
                }
            }
        }

        protected override void Draw(GameTime gameTime)//calls all the draw functions in each class
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
                lvl1Button.Draw(this.spriteBatch);
            }
            else if(gameScreen)
            {
                Back1.Draw(this.spriteBatch);

                lvl1.Draw(this.spriteBatch);

                sidebar.Draw(this.spriteBatch);
                topbar.Draw(this.spriteBatch);

                if (tower1.changeMouse() == true)
            {
                this.IsMouseVisible = false;        //code for changing the mouse image
                MouseState Mstate = Mouse.GetState();            
                tower1.Scale = 0.5f;
                tower1.setImage(this.Content,"clearTower2");
                Vector2 pos = new Vector2((int)Math.Floor((float)(Mstate.X / 34))*34, (int)Math.Floor((float)(Mstate.Y / 34))*34); //new mouse snap-to off by a little
                //Vector2 pos = new Vector2(Mstate.X - 30, Mstate.Y - 30);     // *X1* Center tower on mouse. Change to 1/2 texture size.   
                tower1.Draw(this.spriteBatch, pos);
                tower1.setImage(this.Content,"tower2");
            }
            else
            {
                this.IsMouseVisible = true;
            }

            tower1.Scale = 0.7f; //this changes the button size on the sidebar
            tower1.Draw(this.spriteBatch);

            DrawText();
            }

            spriteBatch.End();
            
            base.Draw(gameTime);
        }

        private void DrawText()//draws the text at the top of the screen
        {
            spriteBatch.DrawString(font, "Lives: "+Stats.getLives()+"   Gold: "+Stats.getGold()+"   Energy: "+Stats.getEnergy(), new Vector2(10, 1), Color.Black);
        }
    }
}
