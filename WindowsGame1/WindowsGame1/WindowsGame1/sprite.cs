using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;



namespace WindowsGame1
{
    class sprite
    {
        Vector2 Position = new Vector2(0, 0);//The current position of the Sprite
        protected Texture2D mSpriteTexture;//The texture object used when drawing the sprite
        public Rectangle Size;//The size of the Sprite bugged, usually off
        public float scale = 1.0f;//Used to size the Sprite up or down from the original image
        public string AssetName;
        public SpriteFont font;

        public void LoadContent(ContentManager theContentManager, string theAssetName)//Load the texture for the sprite using the Content Pipeline
        {
            mSpriteTexture = theContentManager.Load<Texture2D>(theAssetName);
            AssetName = theAssetName;
            Size = new Rectangle(0, 0, (int)(mSpriteTexture.Width * scale), (int)(mSpriteTexture.Height * scale));
            font = theContentManager.Load<SpriteFont>("SpriteFont1");
        }

        public virtual void Draw(SpriteBatch theSpriteBatch)//Draw the sprite to the screen
        {
            theSpriteBatch.Draw(mSpriteTexture, Position, new Rectangle(0, 0, mSpriteTexture.Width, mSpriteTexture.Height), Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }
        public virtual void Draw(SpriteBatch theSpriteBatch, Color clr)//Draw the sprite to the screen
        {
            theSpriteBatch.Draw(mSpriteTexture, Position, new Rectangle(0, 0, mSpriteTexture.Width, mSpriteTexture.Height), clr, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }

        public float Scale
        {
            get { return scale; }
            set
            {
                scale = value;
                //Recalculate the Size of the Sprite with the new scale
                try
                {
                    Rectangle temp = new Rectangle((int)Position.X, (int)Position.Y, (int)(mSpriteTexture.Width * Scale), (int)(mSpriteTexture.Height * Scale));
                    Size = temp;
                }
                catch(Exception e)
                {
                    Console.WriteLine("Something went wrong with your resize :("+e);
                }
            }
        }

        public void setPosition(Vector2 xy)
        {
            Position = xy;
            Size.X = (int)xy.X;
            Size.Y = (int)xy.Y;
        }

        public Vector2 getPosition()
        {
            return Position;
        }

        public int getHeight()
        {
            return mSpriteTexture.Height*(int)scale;
        }

        public int getWidth()
        {
            return mSpriteTexture.Width*(int)scale;
        }

        public void Update(GameTime theGameTime, Vector2 theSpeed, Vector2 theDirection)//Update the Sprite and change it's position based on the passed in speed, direction and elapsed time.
        {
            Position += theDirection * theSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
        }
        

    }
}
