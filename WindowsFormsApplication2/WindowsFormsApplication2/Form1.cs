using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Text = "some shit here";//top of the window

            Image image1 = new Bitmap("C:\\Users\\Phil\\TDGame\\WindowsFormsApplication2\\tower1.gif");//button image

            SButton(450, 50, image1, "tower 1");
            SButton(600, 50, image1, "tower 2");

            Graphics grfx = CreateGraphics();
            image(100, 100, image1, grfx);//not working

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void SButton(int x, int y, Image img, String label)
        {
            Button b = new Button();

            b.Parent = this;
            b.Text = label;//label on the button
            b.TextAlign = ContentAlignment.BottomCenter;

            b.Image = new Bitmap(img);
            b.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            b.Size = new System.Drawing.Size(img.Width, img.Height);

            b.Location = new Point(x, y);//x,y button location
            b.Click += new EventHandler(b_Click);
        }

        void b_Click(object sender, EventArgs e)
        {
            Graphics grfx = CreateGraphics();
            Point ptText = Point.Empty;
            string str = "Button clicked";

            grfx.DrawString(str, Font, new SolidBrush(ForeColor), ptText);
            System.Threading.Thread.Sleep(500);
            grfx.FillRectangle(new SolidBrush(BackColor), new RectangleF(ptText, grfx.MeasureString(str, Font)));

            grfx.Dispose();
        }

        public static void image(int x, int y, Image img, Graphics grfx)//not working
        {
            grfx.DrawImage(img, x, y);
            
        }

    }

}
