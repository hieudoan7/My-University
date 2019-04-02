using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fill_Color
{
    public partial class Form1 : Form
    {
        private Bitmap bitmap;
        Polygon myPolygon;
        Circle myCircle;
        Ellipse myEllipse;
        int type; //0: polygon 1: circle 2: ellipse
        public Form1()
        {
            InitializeComponent();
        }

        private void btnPolygon_Click(object sender, EventArgs e)
        {
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bitmap);
            myPolygon = new Polygon();
            myPolygon.draw(g);
            pictureBox1.Image = bitmap;
            type = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }

        private void btnScanline_Click(object sender, EventArgs e)
        {
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bitmap);
            if (type == 0)
            {
                myPolygon.draw(g);
                myPolygon.scanLine(ref bitmap);
            }
            else if (type == 1)
            {
                myCircle.draw(ref bitmap);
                myCircle.boundaryFill2(ref bitmap, myCircle.center.X, myCircle.center.Y, Color.Black, Color.Red);
            }
            else if (type == 2)
            {
                myEllipse.draw(ref bitmap);
                myEllipse.boundaryFill2(ref bitmap, myEllipse.center.X, myEllipse.center.Y, Color.Black, Color.Red);

            }
            pictureBox1.Image = bitmap;
        } 

        private void btnBound_Click(object sender, EventArgs e)
        {
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bitmap);
            if (type == 0)
            {
                myPolygon.draw(g);
                myPolygon.boundaryFill2(ref bitmap, 700, 300, Color.Black, Color.Green);
            }
            else if (type == 1)
            {
                myCircle.draw(ref bitmap);
                myCircle.boundaryFill2(ref bitmap, myCircle.center.X, myCircle.center.Y, Color.Black, Color.Green);
            }
            else if (type == 2)
            {
                myEllipse.draw(ref bitmap);
                myEllipse.boundaryFill2(ref bitmap, myEllipse.center.X, myEllipse.center.Y, Color.Black, Color.Green);
            
            }
            pictureBox1.Image = bitmap;
        }

        private void btnCircle_Click(object sender, EventArgs e)
        {
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bitmap);
            myCircle = new Circle();
            myCircle.draw(ref bitmap);
            pictureBox1.Image = bitmap;
            type = 1;
        }

        private void btnEllipse_Click(object sender, EventArgs e)
        {
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bitmap);
            myEllipse = new Ellipse();
            myEllipse.draw(ref bitmap);
            pictureBox1.Image = bitmap;
            type = 2;
        }
    }
}
