using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Fill_Color
{
    class Ellipse
    {
        public Point center;
        private int a, b; //coefficent
        //default constructor
        public Ellipse()
        {
            center = new Point(500, 300);
            a = 150;
            b = 100;
        }
        
        //Ellipse Plot Point
        void ellipsePlotPoints(ref Bitmap bitmap, int x, int y)
        {
            int xCenter = center.X, yCenter = center.Y;
            bitmap.SetPixel(xCenter + x, yCenter + y,Color.Black);
            bitmap.SetPixel(xCenter - x, yCenter + y,Color.Black);
            bitmap.SetPixel(xCenter + x, yCenter - y,Color.Black);
            bitmap.SetPixel(xCenter - x, yCenter - y,Color.Black);
        }
        public void boundaryFill2(ref Bitmap bmp, int x, int y, Color boundaryColor, Color fillColor)
        {
            Stack<Point> points = new Stack<Point>();
            points.Push(new Point(x, y));

            while (points.Count > 0)
            {
                Point currentPoint = points.Pop();
                int xx = currentPoint.X;
                int yy = currentPoint.Y;

                Color currentColor = bmp.GetPixel(xx, yy);
                if (!currentColor.ToArgb().Equals(boundaryColor.ToArgb()) && !currentColor.ToArgb().Equals(fillColor.ToArgb()))
                {
                    bmp.SetPixel(xx, yy, fillColor);
                    points.Push(new Point(xx + 1, yy));
                    points.Push(new Point(xx - 1, yy));
                    points.Push(new Point(xx, yy + 1));
                    points.Push(new Point(xx, yy - 1));
                }
            }
        }                 
        
        //Midpoint  Algorithm
        public void draw(ref Bitmap bitmap) //Midpoint
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
