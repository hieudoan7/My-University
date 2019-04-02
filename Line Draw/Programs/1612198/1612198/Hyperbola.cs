using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace _1612198
{
    class Hyperbola:Shape
    {
        private Point center;
        private int a, b; //coefficent
        //default constructor
        public Hyperbola()
        {
            a = 1;
            b = 1;
        }
        //full parameter constructor
        public Hyperbola(int a, int b)
        {
            this.a = a;
            this.b = b;
        }
        //setter
        public void setHyperbola(Point C, int a, int b)
        {
            this.center = C;
            this.a = a;
            this.b = b;
        }
        //Ellipse Plot Point
        public void hyperbolaPlotPoints(ref Bitmap bitmap, int x, int y)
        {
            int xCenter = center.X, yCenter = center.Y;
            mySetPixel(ref bitmap, xCenter + x, yCenter + y);
            mySetPixel(ref bitmap, xCenter - x, yCenter + y);
            mySetPixel(ref bitmap, xCenter + x, yCenter - y);
            mySetPixel(ref bitmap, xCenter - x, yCenter - y);
        }

        //DDA Algorithm
        public void DDA(ref Bitmap bitmap)
        {
            int xCenter = center.X, yCenter = center.Y;
            int x = a;
            int y = 0;
            hyperbolaPlotPoints(ref bitmap, x, y);
            //region1
            while (a * a * y < b * b * x)
            {
                y++;
                x = (int)(Math.Sqrt((1 + 1.0 * y * y / (b * b)) * a * a) + 0.5);
                hyperbolaPlotPoints(ref bitmap, x, y);
            }
            //Region 2
            while (x < 200)
            {
                x++;
                y = (int)(Math.Sqrt(( 1.0 * x * x / (a * a)-1) * b * b) + 0.5);
                hyperbolaPlotPoints(ref bitmap, x, y);
            }
        }
        //Bresenham Algorithms
        public void Bresenham(ref Bitmap bitmap)
        {
            Midpoint(ref bitmap);
        }
        //Midpoint Algorithms
        public void Midpoint(ref Bitmap bitmap)
        {
            int p;
            int x = a;
            int y = 0;
            //plot the first set of poinst
            hyperbolaPlotPoints(ref bitmap, x, y);
            //Region 1
            p = -4 * a * a + 4 * a * b * b + b * b; //round
            while (a * a * y < b * b * x)
            {
                y++;
                if (p < 0)
                {
                    x++;
                    p += 4 * b * b * 2 * x - 4 * a * a * (2 * y + 1);
                }
                else
                {
                    p -= 4 * a * a * (2 * y + 1);
                }
                hyperbolaPlotPoints(ref bitmap, x, y);
            }
            //Region 2
            p = (int)(b * b * (x + 1) * (x + 1) - a * a * (y + 0.5) * (y + 0.5) - a * a * b * b + 0.5);
            while (x < 200)
            {
                x++;
                if (p > 0)
                {
                    y++;
                    p += b * b * (2 * x + 1) - a * a * 2 * y;
                }
                else
                {
                    p += b * b * (2 * x + 1);
                }
                hyperbolaPlotPoints(ref bitmap, x, y);
            }
        }
    }
}
