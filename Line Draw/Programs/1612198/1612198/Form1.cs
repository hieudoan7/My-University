using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1612198
{
    public delegate void MyDelegate(ref Bitmap bitmap);
    public partial class Form1 : Form
    {
        MyDelegate funcPointer = null;
        Bitmap bitmap;
        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cboTT.SelectedIndex = 0;
            cboLoai.SelectedIndex = 0;
            tboSL.Text = "1";
        }

        //Draw random line
        private void DrawRandomLine(ref Line line)   //thag goi no private ma no public thi ko duoc
        {
            switch (cboTT.Text)
            {
                case "DDA":
                    funcPointer = new MyDelegate(line.DDA);
                    break;
                case "Bresenham":
                    funcPointer = new MyDelegate(line.Bresenham);
                    break;
                case "Midpoint":
                    funcPointer = new MyDelegate(line.Midpoint);
                    break;
                case "Xiaolin Wu":
                    funcPointer = new MyDelegate(line.XiaolinWu);
                    break;
            }
            int size = 0;
            Random rand = new Random();
            long start_time = 0, finish_time = 0;
            if (Int32.TryParse(tboSL.Text, out size))
            {
                start_time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                for (int i = 0; i < size; i++)
                {
                    int xa = rand.Next(0, bitmap.Width);
                    int ya = rand.Next(0, bitmap.Height);
                    int xb = rand.Next(0, bitmap.Width);
                    int yb = rand.Next(0, bitmap.Height);
                    line.setLine(new Point(xa, ya), new Point(xb, yb));
                    funcPointer(ref bitmap);
                    pictureBox1.Image = bitmap;
                }
                finish_time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            }
            tboTime.Text = (finish_time - start_time).ToString();
            
        }

        //Draw random circle
        private void DrawRandomCirle(ref Circle circle)
        {
            switch (cboTT.Text)
            {
                case "DDA":
                    funcPointer = new MyDelegate(circle.DDA);
                    break;                       
                case "Bresenham":                
                    funcPointer = new MyDelegate(circle.Bresenham);
                    break;                       
                case "Midpoint":                 
                    funcPointer = new MyDelegate(circle.Midpoint);
                    break;
            }
            int size = 0;
            Random rand = new Random();
            long start_time = 0, finish_time = 0;
            if (Int32.TryParse(tboSL.Text, out size))
            {
                start_time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                for (int i = 0; i < size; i++)
                {
                    int xc = rand.Next(0, bitmap.Width);
                    int yc = rand.Next(0, bitmap.Height);
                    int r = rand.Next(0, Math.Min(bitmap.Width, bitmap.Height) / 2);
                    circle.setCircle(new Point(xc, yc), r);
                    funcPointer(ref bitmap);
                    pictureBox1.Image = bitmap;
                }
                finish_time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            }
            tboTime.Text = (finish_time - start_time).ToString();
        }
        //DrawRandomEllipse
        private void DrawRandomEllipse(ref Ellipse ellipse)
        {
            switch (cboTT.Text)
            {
                case "DDA":
                    funcPointer = new MyDelegate(ellipse.DDA);
                    break;
                case "Bresenham":
                    funcPointer = new MyDelegate(ellipse.Bresenham);
                    break;
                case "Midpoint":
                    funcPointer = new MyDelegate(ellipse.Midpoint);
                    break;
            }
            int size = 0;
            Random rand = new Random();
            long start_time = 0, finish_time = 0;
            if (Int32.TryParse(tboSL.Text, out size))
            {
                start_time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                for (int i = 0; i < size; i++)
                {
                    int xc = rand.Next(0, bitmap.Width);
                    int yc = rand.Next(0, bitmap.Height);
                    int a = 1, b = 1;
                    while (a == b) {
                        a = rand.Next(0, Math.Min(bitmap.Width, bitmap.Height) / 3);
                        b = rand.Next(0, Math.Min(bitmap.Width, bitmap.Height) / 3);
                    }
                    
                    ellipse.setEllipse(new Point(xc, yc), a,b);
                    funcPointer(ref bitmap);
                    pictureBox1.Image = bitmap;
                }
                finish_time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            }
            tboTime.Text = (finish_time - start_time).ToString();
        }
        //Draw Random Parabola
        private void DrawRandomParabola(ref Parabola parabola)
        {
            switch (cboTT.Text)
            {
                case "DDA":
                    funcPointer = new MyDelegate(parabola.DDA);
                    break;
                case "Bresenham":
                    funcPointer = new MyDelegate(parabola.Bresenham);
                    break;
                case "Midpoint":
                    funcPointer = new MyDelegate(parabola.Midpoint);
                    break;
            }
            int size = 0;
            Random rand = new Random();
            long start_time = 0, finish_time = 0;
            if (Int32.TryParse(tboSL.Text, out size))
            {
                start_time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                for (int i = 0; i < size; i++)
                {
                    int xc = rand.Next(0, bitmap.Width);
                    int yc = rand.Next(0, bitmap.Height);
                    int a = 0, b = 0;
                    while (b == 0||a==0) {
                        a = rand.Next(-20, 20);
                        b = rand.Next(-200, 200);
                    }
                    parabola.setParabola(new Point(xc, yc), a, b);
                    funcPointer(ref bitmap);
                    pictureBox1.Image = bitmap;
                }
                finish_time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            }
            tboTime.Text = (finish_time - start_time).ToString();
        }
        //Draw Random Hyperbola
        private void DrawRandomHyperbola(ref Hyperbola hyperbola)
        {
            switch (cboTT.Text)
            {
                case "DDA":
                    funcPointer = new MyDelegate(hyperbola.DDA);
                    break;
                case "Bresenham":
                    funcPointer = new MyDelegate(hyperbola.Bresenham);
                    break;
                case "Midpoint":
                    funcPointer = new MyDelegate(hyperbola.Midpoint);
                    break;
            }
            int size = 0;
            Random rand = new Random();
            long start_time = 0, finish_time = 0;
            if (Int32.TryParse(tboSL.Text, out size))
            {
                start_time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                for (int i = 0; i < size; i++)
                {
                    int xc = rand.Next(0, bitmap.Width);
                    int yc = rand.Next(0, bitmap.Height);
                    int a = 1, b = 1;
                    while (a == b)
                    {
                        a = rand.Next(0, Math.Min(bitmap.Width, bitmap.Height) / 3);
                        b = rand.Next(0, Math.Min(bitmap.Width, bitmap.Height) / 3);
                    }

                    hyperbola.setHyperbola(new Point(xc, yc), a, b);
                    funcPointer(ref bitmap);
                    pictureBox1.Image = bitmap;
                }
                finish_time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            }
            tboTime.Text = (finish_time - start_time).ToString();
        }
        //BtnDraw_Click
        private void btnDraw_Click(object sender, EventArgs e)
        {
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
       
            switch (cboLoai.Text)
            {
                case "Line":
                    Line line = new Line();
                    this.DrawRandomLine(ref line);
                    break;
                case "Circle":
                    Circle circle = new Circle();
                    this.DrawRandomCirle(ref circle);
                    break;
                case "Ellipse":
                    Ellipse ellipse = new Ellipse();
                    this.DrawRandomEllipse(ref ellipse);
                    break;
                case "Parabola":
                    Parabola parabola = new Parabola();;
                    this.DrawRandomParabola(ref parabola);
                    break;
                case "Hyperbola":
                    Hyperbola hyperbola = new Hyperbola();
                    this.DrawRandomHyperbola(ref hyperbola);
                    break;
            }
           
            
        }
    }
}
