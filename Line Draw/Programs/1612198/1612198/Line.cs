using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1612198
{
    class Line:Shape
    {
        //attribute
        private Point A, B;
        //default constructor
        public Line()
        {
            this.A = new Point(0, 0);
            this.B = new Point(0, 0);
        }
        //full parameters constructor
        public Line(Point A, Point B)
        {
            this.A = A;
            this.B = B;
        }
        //setter
        public void setLine(Point A, Point B)
        {
            this.A = A;
            this.B = B;
        }
       
        //DDA Algorithm
        public void DDA(ref Bitmap bitmap){
            int xa = A.X, ya = A.Y;
            int xb = B.X, yb = B.Y;
            int dx = xb - xa, dy = yb - ya, steps, k;
            float xIncrement, yIncrement, x = xa, y = ya;
            if (Math.Abs(dx) > Math.Abs(dy)) steps = Math.Abs(dx);
            else steps = Math.Abs(dy);
            xIncrement = dx / (float)steps;
            yIncrement = dy / (float)steps;
            mySetPixel(ref bitmap, (int)x, (int)y);
            for (k = 0; k < steps; k++)
            {
                x += xIncrement;
                y += yIncrement;
                int roundX = (int)(x + 0.5); //2.3 -> int(2.8) = 2, 2.6 -> int(3.1)=3
                int roundY = (int)(y + 0.5);
                mySetPixel(ref bitmap, roundX, roundY);
            }
        }
        
        //Bresenham Algorithm
        public void BresenhamX(ref Bitmap bitmap)
        {
            int xa = A.X, ya = A.Y, xb = B.X, yb = B.Y;
            int dx = Math.Abs(xa - xb), dy = Math.Abs(ya - yb);
            int p = 2 * dy - dx;
            //start
            int x = xa, y = ya;
            int xEnd = xb;
            int step = 1;
            if (ya > yb) step = -1;
            if (xb < xa)
            {
                x = xb;
                y = yb;
                xEnd = xa;
                if (ya < yb) step = -1;
            }
            //bitmap.SetPixel(x, y, Color.Red);
            mySetPixel(ref bitmap, x, y);
            while (x < xEnd)
            {
                x++;
                if (p < 0)
                {
                    p += 2 * dy;
                }
                else
                {
                    y+=step;
                    p += 2 * dy - 2 * dx;
                }
                //bitmap.SetPixel(x, y, Color.Red);
                mySetPixel(ref bitmap, x, y);
            }
        }
        public void BresenhamY(ref  Bitmap bitmap)
        {
            int xa = A.X, ya = A.Y, xb = B.X, yb = B.Y;
            int dx = Math.Abs(xa - xb), dy = Math.Abs(ya - yb);
            int p = 2 * dx - dy;
            //start
            int x = xa, y = ya;
            int yEnd = yb;
            int step = 1;
            if (xa > xb) step = -1;
            if (yb < ya)
            {
                x = xb;
                y = yb;
                yEnd = ya;
                if (xb > xa) step = -1;
            }
            //bitmap.SetPixel(x, y, Color.Red);
            mySetPixel(ref bitmap, x, y);

            while (y < yEnd)
            {
                y++;
                if (p < 0)
                {
                    p += 2 * dx;
                }
                else
                {
                    x+=step;
                    p += 2 * dx - 2 * dy;
                }
                //bitmap.SetPixel(x, y, Color.Red);
                mySetPixel(ref bitmap, x, y);

            }
        }
        public void Bresenham(ref Bitmap bitmap)
        {
            if (Math.Abs(A.X - B.X) >= Math.Abs(A.Y - B.Y))
                BresenhamX(ref bitmap);
            else BresenhamY(ref bitmap);
        }

        //Midpoint Algorithm
        public void Midpoint(ref Bitmap bitmap) //ichan lineBresenham
        {
            if (Math.Abs(A.X - B.X) >= Math.Abs(A.Y - B.Y))
                BresenhamX(ref bitmap);
            else BresenhamY(ref bitmap);
        }
        //swap 2 integer
        public void swap(ref int a,ref int b){
            int tmp = a;
            a = b;
            b = tmp;
        }
        //Fraction of x
        public float fPart(float x)
        {
            if (x > 0) return x - (int)x;
            else return -x +(int)x;
        }
        //draw a pixel on screen of given brightness
        // 0 <=brightness<=1.
        public void  plotWithBrightness(ref Bitmap bitmap, int x, int y,float bright)
        {
            int c = (int)(bright * 255);
            int w = bitmap.Width, h = bitmap.Height;
            if (x >= 0 && x < w && y >= 0 && y < h)
            {
                bitmap.SetPixel(x, y, Color.FromArgb(255, c, c, c));
            }
        }
        //XiaolinWu Algorithms
        public void XiaolinWu(ref Bitmap bitmap)
        {
            int xa=A.X,ya=A.Y,xb=B.X,yb=B.Y;
            bool steep = Math.Abs(yb - ya) > Math.Abs(xb - xa);
            //doi xung qua y = x de dua ve x chay. Sau do trong luc setPixel thi ve doi xung qua y=x la xong
            if (steep == true)
            {
                swap(ref xa,ref ya);
                swap(ref xb,ref yb);
            }
            //swap A,B to first Point is left. B->A = A->B
            if (xa > xb)
            {
                swap(ref xa,ref xb);
                swap(ref ya,ref yb);
            }
            //luon cho x chay va ve theo dx
            float dx = xb - xa;
            float dy = yb - ya;
            float gradient = dy / dx;  //can negative
            if (dx == 0.0)
            {
                gradient = 1F;
            }
            int Xstart = xa;
            int Xend = xb;
            float y=ya;
            //main loop;
            if (steep)
            {
                for (int x = Xstart; x <= Xend; x++)
                {
                    int tren = (int)y;
                    int duoi = (int)y + 1;
                    float brightTren = fPart(y);
                    float brightDuoi = 1 - brightTren;
                    plotWithBrightness(ref bitmap, duoi, x, brightDuoi);
                    plotWithBrightness(ref bitmap, tren, x, brightTren);
                    y += gradient;
                }
            }
            else
            {
                for (int x = Xstart; x <= Xend; x++)
                {
                    int tren = (int)y;
                    int duoi = (int)y + 1;
                    float brightTren = fPart(y);
                    float brightDuoi = 1 - brightTren;
                    plotWithBrightness(ref bitmap, x, tren, brightTren);
                    plotWithBrightness(ref bitmap, x, duoi, brightDuoi);
                    y+=gradient;
                }
            }
        }
    }
}
