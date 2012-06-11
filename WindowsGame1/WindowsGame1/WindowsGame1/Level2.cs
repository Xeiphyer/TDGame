using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{
    class Level2 : Level1//extends level 1 class
    {
        public override void LoadContent(ContentManager theContentManager)//load all waves this is all you need really
        {
            wave1.LoadContent(theContentManager);
            wave1.setColor(Color.Aqua);
            wave1.setNumberOfUnits(5);

            wave2.LoadContent(theContentManager);
            wave2.setColor(Color.Magenta);//change color
            wave2.setNumberOfUnits(5);//change number of units that spawn
            wave2.setHp(15);//change the Hp of those units

            wave3.LoadContent(theContentManager);
            wave3.setColor(Color.BlueViolet);
            wave3.setNumberOfUnits(7);
            wave3.setHp(20);

            wave4.LoadContent(theContentManager);
            wave4.setColor(Color.Maroon);
            wave4.setNumberOfUnits(10);
            wave4.setHp(30);

            wave5.LoadContent(theContentManager);
            wave5.setColor(Color.DarkBlue);
            wave5.setNumberOfUnits(1);
            wave5.setHp(160);
            wave5.setScale(0.7f);
        }
    }
}
