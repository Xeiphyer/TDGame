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
        sprite Back1;
        sprite topbar;
        sprite sidebar;
        sprite title;
        sprite map;
        SpriteFont font;
        Button lvl1Button;
        Button start;
        TowerButton tower1;
        Level1 lvl1;
        State gameState;
        Button lvl2Button;
        Level2 lvl2;

        enum State//games states
        {
            title,
            map,
            level1,
            level2,
            level3
        }

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

            sidebar = new sprite();
            topbar = new sprite();

            title = new sprite();
            map = new sprite();

            lvl1Button = new Button();

            start = new Button();

            lvl1 = new Level1();

            lvl2Button = new Button();
            lvl2 = new Level2();

            base.Initialize();

        }

        private void levelStart()
        {
            Stats.setLives(20);
            Stats.setGold(200);
            Stats.setEnergy(100);
            tower1.reset();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            tower1.LoadContent(this.Content);

            Back1.LoadContent(this.Content, "Back01");

            sidebar.LoadContent(this.Content, "side");
            sidebar.setPosition(new Vector2(800,0));
            topbar.LoadContent(this.Content, "topbar");
            topbar.setPosition(new Vector2(0, 0));

            title.LoadContent(this.Content, "title");
            map.LoadContent(this.Content, "Map");
            gameState = State.title;

            font = Content.Load<SpriteFont>("SpriteFont1");

            lvl1Button.LoadContent(this.Content, "lvl");
            lvl1Button.setPosition(new Vector2(30, 500));

            start.LoadContent(this.Content, "start");
            start.setPosition(new Vector2(350, 475));

            lvl2Button.LoadContent(this.Content, "lvl");
            lvl2Button.setPosition(new Vector2(120, 320));

            lvl1.LoadContent(this.Content);

            lvl2.LoadContent(this.Content);
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

            if (gameState == State.title)
            {
                updateTitle();
                start.Update(Mouse.GetState());
            }
            else if (gameState == State.map)
            {
                updateMap();
                lvl1Button.Update(Mouse.GetState());
                lvl2Button.Update(Mouse.GetState());
            }
            else if (gameState == State.level1)
            {
                lvl1.Update(gameTime);
                updateCollision(gameTime, lvl1.getCranes());
                updateTarget(lvl1);

                if(Stats.getLives() <= 0 || lvl1.getDone() == true)
                {
                    gameState = State.map;
                }

                tower1.Update(gameTime);
            }
            else if (gameState == State.level2)
            {
                lvl2.Update(gameTime);
                updateCollision(gameTime, lvl2.getCranes());
                updateTarget(lvl2);

                if (Stats.getLives() <= 0 || lvl2.getDone() == true)
                {
                    gameState = State.map;
                }

                tower1.Update(gameTime);
            }

            base.Update(gameTime);
        }

        private void updateTitle()
        {
            if (start.getClicked() == true || Keyboard.GetState().IsKeyDown(Keys.Space) == true)
            {
                gameState = State.map;
                return;

            }
        }

        private void updateMap()//checks button states on the map screen
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) == true || lvl1Button.getClicked() == true)//level 1
            {
                gameState = State.level1;
                levelStart();
                lvl1.reset();
                lvl1Button.setClicked(false);//reset the button off clicked state
                return;

            }

            if (lvl2Button.getClicked() == true)//level 2
            {
                gameState = State.level2;
                levelStart();
                lvl2.reset();
                lvl2Button.setClicked(false);//reset the button off clicked state
                return;

            }
        }

        private void updateCollision(GameTime gametime, List<Pcrane> cranes)
        {
            Rectangle rect1;
            Rectangle rect2;
            //List<Pcrane> cranes = level.getCranes();//this will have to be changes based on level, returns the cranes currently onscreen
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

        private void updateTarget(Level1 level)//I think the problem is the positions of the rectangles, I wish I could see them to test...
        {
            Rectangle rect1;
            Rectangle rect2;
            List<Tower> towers = tower1.getTowers();
            List<Pcrane> flock = level.getCranes();//this will have to be changes based on level, returns the cranes currently onscreen
            
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

            if (gameState == State.title)
            {
                title.Draw(this.spriteBatch);
                start.Draw(this.spriteBatch);
            }

            else if (gameState == State.map)
            {
                map.Draw(this.spriteBatch);
                lvl1Button.Draw(this.spriteBatch);
                lvl2Button.Draw(this.spriteBatch);
            }
            else if(gameState == State.level1 || gameState == State.level2)
            {
                if (gameState == State.level1)
                {
                    Back1.Draw(this.spriteBatch);
                    lvl1.Draw(this.spriteBatch);
                }
                else if (gameState == State.level2)
                {
                    Back1.Draw(this.spriteBatch);
                    lvl2.Draw(this.spriteBatch);
                }

                sidebar.Draw(this.spriteBatch);
                topbar.Draw(this.spriteBatch);

                if (tower1.changeMouse() == true)
                {
                    this.IsMouseVisible = false;        //code for changing the mouse image
                    MouseState Mstate = Mouse.GetState();            
                    tower1.Scale = 0.5f;
                    tower1.setImage(this.Content,"clearTower2");
                    //Vector2 pos = new Vector2((int)Math.Floor((float)(Mstate.X / 34))*34, (int)Math.Floor((float)(Mstate.Y / 34))*34); //new mouse snap-to off by a little
                    Vector2 pos = new Vector2(Mstate.X - 30, Mstate.Y - 30);     // *X1* Center tower on mouse. Change to 1/2 texture size.   
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
