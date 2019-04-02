using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1612198
{
    class Ellipse:Shape
    {
        private Point center;
        private int a, b; //coefficent
        //default constructor
        public Ellipse()
        {
            a = 1;
            b = 1;
        }
        //full parameter constructor
        public Ellipse(int a, int b)
        {
            this.a = a;
            this.b = b;
        }
        //setter
        public void setEllipse(Point C, int a, int b)
        {
            this.center = C;
            this.a = a;
            this.b = b;
        }
        //Ellipse Plot Point
        void ellipsePlotPoints(ref Bitmap bitmap, int x, int y)
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
            int x = 0;
            int y = b;
            ellipsePlotPoints(ref bitmap, x, y);
            //region1
            while (b*b*x < a*a*y)
            {
                x++;
                y = (int)(Math.Sqrt((1 - 1.0 * x * x / (a * a)) * b * b)+0.5);
                ellipsePlotPoints(ref bitmap, x, y);
            }
            //region2
            while (y > 0)
            {
                y--;
                x = (int)(Math.Sqrt((1 - 1.0 * y * y / (b * b)) * a * a) + 0.5);
                ellipsePlotPoints(ref bitmap, x, y);
            }
        }
        //Bresenham Algorithm
        public void Bresenham(ref Bitmap bitmap)
        {
            int a2 = a * a;
            int b2 = b * b;
            int _2a2 = 2 * a * a;
            int _2b2 = 2 * b * b;
            int _4b2 = 4 * b2;
            int _4a2 = 4 * a2;
            int _8a2 = 8 * a2;
            int _8b2 = 8 * b2;
            int p;
            int x = 0;
            int y = b;
            int px = 0; //8*b*b*x
            int py = _4a2 * y;  //2*a*a*y
            //plot the first set of poinst
            ellipsePlotPoints(ref bitmap, x, y);
            //Region 1
            p = -2*a*a*b+a*a; //round
            while (px < py)
            {
                x++;
                px += _4b2;
                if (p < 0)
                    p += _2b2 + px;
                else
                {
                    y--;
                    py -= _4a2;
                    p += px - py - _2a2 - _2b2;
                }
                ellipsePlotPoints(ref bitmap, x, y);
            }
            //Region 2
            //p = b2 * (2*x + 1) * (2*x + 1) + 4*a2 * (y - 1) * (y - 1) - 4*a2 * b2;
            p = a2 * (2 * y * y - 2 * y + 1) - 2*(a2 * b2 - b2 * x * x);
            while (y > 0)
            {
                y--;
                py -= _4a2;
                if (p > 0)
                    p += (-py);
                else
                {
                    x++;
                    px += _4b2;
                    p += _2b2 - py + px;
                }
                ellipsePlotPoints(ref bitmap, x, y);
            }
        }
        //Midpoint  Algorithm
        public void Midpoint(ref Bitmap bitmap)
        {
            int a2 = a * a;
            int b2 = b * b;
            int _4b2 = 4 * b2;
            int _4a2 = 4 * a2;
            int _8a2 = 8 * a2;
            int _8b2 = 8 * b2;
            int p;
            int x = 0;
            int y = b;
            int px = 0; //8*b*b*x
            int py = _8a2 * y;  //2*a*a*y
            //plot the first set of poinst
            ellipsePlotPoints(ref bitmap, x, y);
            //Region 1
            p = 4 * b2 - 4 * (a2 * b) + a2; //round
            while (px < py)
            {
                x++;
                px += _8b2;
                if (p < 0)
                    p += _4b2 + px;
                else
                {
                    y--;
                    py -= _8a2;
                    p += _4b2 + px - py;
                }
                ellipsePlotPoints(ref bitmap, x, y);
            }
            //Region 2
            p = b2 * (2 * x + 1) * (2 * x + 1) + 4 * a2 * (y - 1) * (y - 1) - 4 * a2 * b2;
            while (y > 0)
            {
                y--;
                py -= _8a2;
                if (p > 0)
                    p += _4a2 - py;
                else
                {
                    x++;
                    px += _8b2;
                    p += _4a2 - py + px;
                }
                ellipsePlotPoints(ref bitmap, x, y);
            }
        }
    }
}
