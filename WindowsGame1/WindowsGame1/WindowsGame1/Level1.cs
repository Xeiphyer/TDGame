/*contains all the waves that will be called in level1*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1
{
    class Level1
    {
        waves wave1;
        waves wave2;
        waves wave3;
        waves wave4;
        waves wave5;
        State currentState;//current state of the level

        enum State//possible level states
        {
            one,
            two,
            three,
            four,
            five,
            done
        }

        public Level1()//constructor
        {
            currentState = State.one;

            wave1 = new waves();
            wave2 = new waves();
            wave3 = new waves();
            wave4 = new waves();
            wave5 = new waves();
        }

        public void reset()//resets the level
        {
            wave1.reset();
            wave2.reset();
            wave3.reset();
            wave4.reset();
            wave5.reset();
        }

        public void LoadContent(ContentManager theContentManager)//load all waves
        {
            wave1.LoadContent(theContentManager);
            wave1.setColor(Color.White);
            wave1.setNumberOfUnits(5);

            wave2.LoadContent(theContentManager);
            wave2.setColor(Color.Tomato);//change color
            wave2.setNumberOfUnits(5);//change number of units that spawn
            wave2.setHp(15);//change the Hp of those units

            wave3.LoadContent(theContentManager);
            wave3.setColor(Color.Green);
            wave3.setNumberOfUnits(7);
            wave3.setHp(20);

            wave4.LoadContent(theContentManager);
            wave4.setColor(Color.Yellow);
            wave4.setNumberOfUnits(10);
            wave4.setHp(30);

            wave5.LoadContent(theContentManager);
            wave5.setColor(Color.Blue);
            wave5.setNumberOfUnits(20);
            wave5.setHp(40);
        }

        public void Update(GameTime theGameTime)//updates appropriate wave and changes level state
        {
            if (wave1.getDone() == true)//changes level state by checking if waves are done
            {
                currentState = State.two;
                if (wave2.getDone() == true)
                {
                    currentState = State.three;
                    if (wave3.getDone() == true)
                    {
                        currentState = State.four;
                        if (wave4.getDone() == true)
                        {
                            currentState = State.five;
                            if (wave5.getDone() == true)
                            {
                                currentState = State.done;
                            }
                        }
                    }
                }
            }

            switch (currentState)//checking wich wave to update
            {
                case State.one:
                    wave1.Update(theGameTime);
                    break;
                case State.two:
                    wave2.Update(theGameTime);
                    break;
                case State.three:
                    wave3.Update(theGameTime);
                    break;
                case State.four:
                    wave4.Update(theGameTime);
                    break;
                case State.five:
                    wave5.Update(theGameTime);
                    break;
                case State.done:
                    break;
            }
        }

        public bool getDone()//returns the state of the level so you can check if its done
        {
            if (currentState == State.done)
            {
                return true;
            }
            else
                return false;
        }

        public List<Pcrane> getCranes()//returns a list of cranes from the wave on screen
        {
            switch (currentState)//checking wich wave to get the list from
            {
                case State.one:
                    return wave1.getList();
                case State.two:
                    return wave2.getList();
                case State.three:
                    return wave3.getList();
                case State.four:
                    return wave4.getList();
                case State.five:
                    return wave5.getList();
            }
            List<Pcrane> empty = new List<Pcrane>();
            return empty;
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            switch (currentState)//checking wich wave to Draw
            {
                case State.one:
                    wave1.Draw(theSpriteBatch);
                    break;
                case State.two:
                    wave2.Draw(theSpriteBatch);
                    break;
                case State.three:
                    wave3.Draw(theSpriteBatch);
                    break;
                case State.four:
                    wave4.Draw(theSpriteBatch);
                    break;
                case State.five:
                    wave5.Draw(theSpriteBatch);
                    break;
                case State.done:
                    break;
            }
        }
    }
}
