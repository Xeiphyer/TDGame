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
            SButton();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void SButton()
        {
            Text = "some shit here";//top of the window
            Button b = new Button();

            b.Parent = this;
            b.Text = "Mutha' fuck'n tower 1";//label on the button

            Image img = new Bitmap("C:\\Users\\Phil\\TDGame\\WindowsFormsApplication2\\tower1.gif");//button image
            b.Image = new Bitmap(img);
            b.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            b.Size = new System.Drawing.Size(img.Width, img.Height);

            b.Location = new Point(500, 50);//x,y button location
            b.Click += new EventHandler(b_Click);
        }

        void b_Click(object sender, EventArgs e)
        {
            Graphics grfx = CreateGraphics();
            Point ptText = Point.Empty;
            string str = "Button clicked";

            grfx.DrawString(str, Font, new SolidBrush(ForeColor), ptText);
            System.Threading.Thread.Sleep(250);
            grfx.FillRectangle(new SolidBrush(BackColor), new RectangleF(ptText, grfx.MeasureString(str, Font)));

            grfx.Dispose();
        }

    }

}
