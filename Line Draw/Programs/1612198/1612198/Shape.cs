using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace _1612198
{
    public abstract class Shape
    {
        //public abstract void DDA(ref Bitmap bitmap);
        //public abstract void Bresenham(ref Bitmap bitmap);
        //public abstract void Midpoint(ref Bitmap bitmap);
        public void mySetPixel(ref Bitmap bitmap, int x, int y)
        {
            int w = bitmap.Width, h = bitmap.Height;
            if (x >= 0 && x < w && y >= 0 && y < h)
            {
                bitmap.SetPixel(x, y, Color.Black);
            }
        }
    }
}
