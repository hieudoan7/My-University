using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1612198
{
    class Circle:Shape
    {
        private Point center;
        private int radius;
        //default constructor
        public Circle()
        {
            this.center = new Point(0, 0);
            this.radius = 0;
        }
        //full parameter constructor
        public Circle(Point center, int radius)
        {
            this.center = center;
            this.radius = radius;
        }
        //set all attribute of a circle
        public void setCircle(Point C, int r)
        {
            this.center = C;
            this.radius = r;
        }
        
        //
        void circlePlotPoints(ref Bitmap bitmap, int x, int y)
        {
            int xCenter = center.X, yCenter = center.Y;
            mySetPixel(ref bitmap, xCenter + x, yCenter + y);
            mySetPixel(ref bitmap, xCenter - x, yCenter + y);
            mySetPixel(ref bitmap, xCenter + x, yCenter - y);
            mySetPixel(ref bitmap, xCenter - x, yCenter - y);
            mySetPixel(ref bitmap, xCenter + y, yCenter + x);
            mySetPixel(ref bitmap, xCenter - y, yCenter + x);
            mySetPixel(ref bitmap, xCenter + y, yCenter - x);
            mySetPixel(ref bitmap, xCenter - y, yCenter - x);
        }
        //DDA Algorithm
        public void DDA(ref Bitmap bitmap)
        {
            int xCenter = center.X, yCenter = center.Y;
            int x = 0;
            int y = radius;
            circlePlotPoints(ref bitmap, x, y);
            while (x < y)
            {
                x++;
                y = (int)(Math.Sqrt(radius * radius - x * x)+0.5);
                circlePlotPoints(ref bitmap, x, y);
            }
        }
        
        //Bresenham Algorithm
        public void Bresenham(ref Bitmap bitmap)
        {
            int xCenter = center.X, yCenter = center.Y;
            int x = 0;
            int y = radius;
            int p = 3 - 2 * radius; //p_i=d1-d2
            circlePlotPoints(ref bitmap, x, y);
            while (x < y)
            {
                x++;
                if (p < 0)
                    p += 4 * x + 2;  //4x_(i+1)+2 = 4x_i+6 do tăng trước rồi
                else
                {
                    y--;
                    p += 4 * (x - y) + 2;  //4(x_(i+1)-y_(i+1))+2 = 4(x-y)+10 
                }
                circlePlotPoints(ref bitmap, x, y);
            }
        }
        //Midpoint Algorithm
        public void Midpoint(ref Bitmap bitmap)
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

    }
}
