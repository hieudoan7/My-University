using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Fill_Color
{
    class Circle
    {
        public Point center;
        private int radius;
        //default constructor
        public Circle()
        {
            this.center = new Point(500, 300);
            this.radius = 200;
        }
        
        //
        void circlePlotPoints(ref Bitmap bitmap, int x, int y)
        {
            int xCenter = center.X, yCenter = center.Y;
            bitmap.SetPixel(xCenter + x, yCenter + y,Color.Black);
            bitmap.SetPixel(xCenter - x, yCenter + y,Color.Black);
            bitmap.SetPixel(xCenter + x, yCenter - y,Color.Black);
            bitmap.SetPixel(xCenter - x, yCenter - y,Color.Black);
            bitmap.SetPixel(xCenter + y, yCenter + x,Color.Black);
            bitmap.SetPixel(xCenter - y, yCenter + x,Color.Black);
            bitmap.SetPixel(xCenter + y, yCenter - x,Color.Black);
            bitmap.SetPixel(xCenter - y, yCenter - x,Color.Black);
        }
        public void draw(ref Bitmap bitmap) //using midpoint algorithm
        {
            int x = 0;
            int y = radius;
            int p = 1 - radius;
            circlePlotPoints(ref bitmap, x, y);
            while (x < y)
            {
                x++;
                if (p < 0)
                    p += 2 * x + 1;
                else
                {
                    y--;
                    p += 2 * (x - y) + 1;
                }
                circlePlotPoints(ref bitmap, x, y);
            }
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
    }
}
