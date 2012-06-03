using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace WindowsGame1
{
    class Button : sprite
    {
        MouseState lastMouseState;
        Vector2 position;
        bool clicked;
        bool hover;
        int i;

        public Button()//construtor
        {
            clicked = false;
            hover = false;
        }

        /*public override void LoadContent(ContentManager contmgr, String str)
        {
            base.LoadContent(contmgr, str);
        }*/

        public void Update(MouseState mouseState)
        {
            position = getPosition();

            if (lastMouseState != null)
            {
                if (mouseState.X > position.X
                    && mouseState.X < (position.X + (int)(mSpriteTexture.Width * Scale))
                    && mouseState.Y > position.Y
                    && mouseState.Y < (position.Y + (int)(mSpriteTexture.Height * Scale)))
                {
                    hover = true;

                    if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                    {
                        clicked = true;
                    }
                }
            }

            if (mouseState.X < position.X 
                || mouseState.X > (position.X + (int)(mSpriteTexture.Width * Scale)) 
                || mouseState.Y < position.Y
                || mouseState.Y > (position.Y + (int)(mSpriteTexture.Height * Scale)))
            {
                hover = false;
            }

            lastMouseState = mouseState;
        }

        public bool getClicked()
        {
            return clicked;
        }

        public void setClicked(bool set)
        {
            clicked = set;
            return;
        }

        public override void Draw(SpriteBatch theSpriteBatch)//Draw the sprite to the screen
        {
            if (clicked == false)
            {
                Draw(theSpriteBatch, Color.Red);
                i = 0;
            }
            if (hover == true)
            {
                Draw(theSpriteBatch, Color.Yellow);
            }
            if (clicked == true)
            {
                Draw(theSpriteBatch, Color.Blue);
                i++;
                if (i == 10)
                {
                    clicked = false;
                }
            }
        }
    }
}
