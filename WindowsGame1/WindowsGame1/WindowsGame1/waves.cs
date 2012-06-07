using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace WindowsGame1
{
    class waves : sprite
    {
        List<Pcrane> cranes = new List<Pcrane>();
        State state;

        const int START_POSITION_X = 675;
        const int START_POSITION_Y = 550;
        int X;
        int Y;
        string ASSETNAME = "next1";
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;
        ContentManager mContentManager;
        int cooldown = 25;
        int unitCounter;
        int numberOfUnits;//number of units in the wave
        Color color;//color of this wave
        int Hp;//hp of all the units in this wave

        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;
        Vector2 mStartingPosition = Vector2.Zero;
        MouseState mouseState;
        MouseState lastMouseState;

        bool done = false;

        enum State
        {
            scroll,
            spawn,
            spawned
        }

        public waves()//constructor
        {
            Hp = 10;
            color = Color.White;
            numberOfUnits = 10;
        }

        public void setImage(ContentManager theContentManager, String str)
        {
            base.LoadContent(theContentManager, str);
        }

        public void setColor(Color clr)//changes the color of all the units in the wave
        {
            color = clr;
        }

        public void setHp(int newHp)//sets the Hp of all units in the wave
        {
            Hp = newHp;
        }

        public void setNumberOfUnits(int newNumberOfUnits)//changes the number of units that spawn
        {
            numberOfUnits = newNumberOfUnits;
        }

        public void reset()
        {
            X = START_POSITION_X;
            Y = START_POSITION_Y;
            cranes.Clear();
            unitCounter = numberOfUnits;
            state = State.scroll;
            setPosition(new Vector2(START_POSITION_X, START_POSITION_Y));
            done = false;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            mContentManager = theContentManager;

            foreach (Pcrane aCrane in cranes)
            {
                aCrane.LoadContent(theContentManager);
            }

            base.LoadContent(theContentManager, ASSETNAME);
        }

        public void Update(GameTime theGameTime)
        {
            UpdateCranes(theGameTime);
            UpdateMovement();
            base.Update(theGameTime, mSpeed, mDirection);
        }

        private void UpdateMovement()
        {
            if (state == State.scroll)
            {
                mSpeed = Vector2.Zero;
                mDirection = Vector2.Zero;

                if (X > 270)
                {
                    mSpeed.X = 100;
                    mDirection.X = MOVE_LEFT;
                    X = X + MOVE_LEFT;
                }
                if (X == 270)
                {
                    state = State.spawn;
                    --X;
                    mSpeed = Vector2.Zero;
                    mDirection = Vector2.Zero;
                }

            }
        }

        private void UpdateCranes(GameTime theGameTime)
        {
            if (cranes.Count == 0 && state == State.spawned)
            {
                done = true;
            }

            for (int i = 0; i < cranes.Count; i++)
            {
                cranes[i].Update(theGameTime);
                if (cranes[i].dead() == true)
                {
                    cranes.Remove(cranes[i]);
                }
            }

            --cooldown;

            if (cooldown <= 0 && state == State.spawn)
            {
                Pcrane aCrane = new Pcrane();
                aCrane.LoadContent(mContentManager);
                cranes.Add(aCrane);
                aCrane.Scale = 0.5f;
                aCrane.setHp(Hp);
                cooldown = 25;
                --unitCounter;
                if (unitCounter == 0)
                {
                    state = State.spawned;
                }
            }

        }

        public List<Pcrane> getList()
        {
            return cranes;
        }

        public bool getDone()
        {
            return done;
        }

        public void setDone(bool input)
        {
            done = input;
            return;
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            if (done == false)
            {
                foreach (Pcrane aCrane in cranes)
                {
                    aCrane.Draw(theSpriteBatch, color);
                }
                base.Draw(theSpriteBatch, color);
            }
        }

    }
}

