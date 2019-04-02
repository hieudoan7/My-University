using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace _1612198
{
    class Parabola:Shape
    {
        //attribute
        private Point center;
        private int a, b;
        //default constructor
        public Parabola()
        {
            this.center = new Point(50, 50);
            this.a = 1;
            this.b = 1;
        }
        //full constructor
        public Parabola(Point C, int a, int b)
        {
            this.center = C;
            this.a = a;
            this.b = b;
        }
        //setter
        public void setParabola(Point C, int a, int b)
        {
            this.center = C;
            this.a = a;
            this.b = b;
        }
        //parabola plot point
        public void paraPlotPoint(ref Bitmap bitmap, int x, int y)
        {
            int xCenter = center.X, yCenter = center.Y;
            mySetPixel(ref bitmap, xCenter + x, yCenter - y);
            mySetPixel(ref bitmap, xCenter - x, yCenter - y);
        }

        //DDA Algorithm
        public void DDA(ref Bitmap bitmap)
        {
            //region 1
            int A = a, B = b;
            int step = a * b > 0 ? -1 : 1;
            int y = 0, x = 0;
            paraPlotPoint(ref bitmap, x, y);
            int p = 2 * A + B; //
            int chanTren = (int)Math.Abs((B / (2 * A) + 0.5));
            while (x < chanTren)
            {
                x++;
                y = (int)(-A * x * x * 1.0 / B + 0.5);
                paraPlotPoint(ref bitmap, x, y);
            }
            //region 2   
            paraPlotPoint(ref bitmap, x, y);
            while (y < 300 && y>-300) //gioi han de ko ra vo cuc
            {
                y+=step;
                x = (int)(Math.Sqrt(Math.Abs(B*y*1.0/A))+0.5);
                paraPlotPoint(ref bitmap, x, y);
            }
        }
        //Bresenham Algorithm
        public void BreAx2plusBy(ref Bitmap bitmap) //A>0, f(x,y)=Ax^2 + B, AB<0 (A,B trai dau)
        {
            //region 1
            int A = Math.Abs(a), B = -Math.Abs(b);
            int y = 0, x = 0;
            paraPlotPoint(ref bitmap, x, y);
            int p =2*A+B; //
            int chanTren = (int)Math.Abs((B / (2 * A) + 0.5)); //chac thoi
            while (x < chanTren)
            {
                x++;
                if (p > 0)
                {
                    y++;
                    p += 4 * A * x + 2 * A + 2 * B;
                }
                else
                {
                    p += 4 * A * x + 2 * A;
                }
                paraPlotPoint(ref bitmap, x, y);
            }
            //region 2   
            p = A * (x * x + (x + 1) * (x + 1)) + 2 * B * (y + 1);
            paraPlotPoint(ref bitmap, x, y);
            while (y < 200) //gioi han de ko ra vo cuc
            {
                y++;
                if (p > 0)
                {
                    p += 2*B;
                }
                else
                {
                    x++;
                    p += 4 * A * x + 4 * A + 2 * B;
                }
                paraPlotPoint(ref bitmap, x, y);
            }
        }
        //
        public void _BreAx2plusBy(ref Bitmap bitmap) //A>0, f(x,y)=Ax^2 + B, AB<0 (A,B trai dau)
        {
            //region 1
            int A = Math.Abs(a), B = -Math.Abs(b);
            int y = 0, x = 0;
            paraPlotPoint(ref bitmap, x, -y);
            int p = 2 * A + B; //
            int chanTren = (int)Math.Abs((B / (2 * A) + 0.5)); //chac thoi
            while (x < chanTren)
            {
                x++;
                if (p > 0)
                {
                    y++;
                    p += 4 * A * x + 2 * A + 2 * B;
                }
                else
                {
                    p += 4 * A * x + 2 * A;
                }
                paraPlotPoint(ref bitmap, x, -y);
            }
            //region 2   
            p = A * (x * x + (x + 1) * (x + 1)) + 2 * B * (y + 1);
            paraPlotPoint(ref bitmap, x, -y);
            while (y < 200) //gioi han de ko ra vo cuc
            {
                y++;
                if (p > 0)
                {
                    p += 2 * B;
                }
                else
                {
                    x++;
                    p += 4 * A * x + 4 * A + 2 * B;
                }
                paraPlotPoint(ref bitmap, x, -y);
            }
        }
        public void Bresenham(ref Bitmap bitmap)
        {
            if (this.a * this.b < 0)
            {
                BreAx2plusBy(ref bitmap);
            }
            else _BreAx2plusBy(ref bitmap);
        }
        //Midpoint Algorithm
        public void Midpoint(ref Bitmap bitmap)
        {
            if (this.a * this.b < 0)
            {
                Ax2plusBy(ref bitmap);
            }
            else _Ax2plusBy(ref bitmap);
        }
        //AB<0
        public void Ax2plusBy(ref Bitmap bitmap) //A>0, f(x,y)=Ax^2 + B, AB<0 (A,B trai dau)
        {
            //region 1
            int A = Math.Abs(a), B = -Math.Abs(b);
            int y = 0, x = 0;
            paraPlotPoint(ref bitmap, x, y);
            int p = 2 * A + B; //
            int chanTren = (int)Math.Abs((B / (2 * A) + 0.5)); //chac thoi
            while (x < chanTren)
            {
                x++;
                if (p > 0)
                {
                    y++;
                    //p += 2 * A * (2 * x + 1) + 2 * B;
                    p += 4 * A * x + 2 * A + 2 * B;
                }
                else
                {
                    p += 4 * A * x + 2 * A;
                }
                paraPlotPoint(ref bitmap, x, y);
            }
            ////region 2   
            //p = A * A + 6 * A * B;
            //paraPlotPoint(ref bitmap, x, y);
            //while (y < 200) //gioi han de ko ra vo cuc
            //{
            //    y++;
            //    if (p > 0)
            //    {
            //        p += 4 * A * B;
            //    }
            //    else
            //    {
            //        x++;
            //        p += 8 * A * x + 4 * B;
            //    }
            //    paraPlotPoint(ref bitmap, x, y);
            //}
            //region 2   
            p = A * (2 * x + 1) * (2 * x + 1) + 4 * B * (y + 1);
            paraPlotPoint(ref bitmap, x, y);
            while (y < 200) //gioi han de ko ra vo cuc
            {
                y++;
                if (p > 0)
                {
                    p += 4 * B;
                }
                else
                {
                    x++;
                    p += 8 * A * x + 4 * B;
                }
                paraPlotPoint(ref bitmap, x, y);
            }
        }
        //AB>0 (A,B cung dau) ~ y =-ax^2 (a>0)
        public void _Ax2plusBy(ref Bitmap bitmap) 
        {
            //region 1
            int A = Math.Abs(a), B = -Math.Abs(b);
            int y = 0, x = 0;
            paraPlotPoint(ref bitmap, x, -y);
            int p = 2 * A + B; //
            int chanTren = (int)Math.Abs((B / (2 * A) + 0.5));
            while (x < chanTren)
            {
                x++;
                if (p > 0)
                {
                    y++;
                    p += 4 * A * x + 2 * A + 2 * B;
                }
                else
                {
                    p += 4 * A * x + 2 * A;
                }
                paraPlotPoint(ref bitmap, x, -y);
            }
            //region 2   
            //p = A * A + 6 * A * B;
            //paraPlotPoint(ref bitmap, x, -y);
            //while (y < 200) //gioi han de ko ra vo cuc
            //{
            //    y++;
            //    if (p > 0)
            //    {
            //        p += 4 * A * B;
            //    }
            //    else
            //    {
            //        x++;
            //        p += 8 * A * x + 4 * B;
            //    }
            //    paraPlotPoint(ref bitmap, x, -y);
            //}
            p = A * (2 * x + 1) * (2 * x + 1) + 4 * B * (y + 1);
            paraPlotPoint(ref bitmap, x, -y);
            while (y < 200) //gioi han de ko ra vo cuc
            {
                y++;
                if (p > 0)
                {
                    p += 4 * B;
                }
                else
                {
                    x++;
                    p += 8 * A * x + 4 * B;
                }
                paraPlotPoint(ref bitmap, x, -y);
            }
        }
    }
}
