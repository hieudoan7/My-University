using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace Fill_Color
{
    class edgeBucket
    {
        public int ymin;
        public int ymax;
        public double x;
        public int sign;
        public int dX;
        public int dY;
        public int sum;
    }
    class Polygon
    {
        List<Point> vertices;
        List<edgeBucket> edgeTable;
        List<edgeBucket> activeList;
        public Polygon()
        {
            vertices = new List<Point>();
            vertices.Add(new Point(400, 200));
            vertices.Add(new Point(500, 200));
            vertices.Add(new Point(550, 300));
            vertices.Add(new Point(700, 150));
            vertices.Add(new Point(800, 300));
            vertices.Add(new Point(650, 500));
            vertices.Add(new Point(400, 500));
            edgeTable = new List<edgeBucket>();
            activeList = new List<edgeBucket>();

        }
        //star polygon
        public Polygon(String str)
        {
            vertices = new List<Point>();
            vertices.Add(new Point(400, 200));
            vertices.Add(new Point(500, 100));
            vertices.Add(new Point(550, 300));
            vertices.Add(new Point(600, 100));
            vertices.Add(new Point(700, 200));
            vertices.Add(new Point(550, 400));
            edgeTable = new List<edgeBucket>();
            activeList = new List<edgeBucket>();

        }
        public void draw(Graphics g)
        {
            Pen p = new Pen(Color.Black);
            for (int i = 0; i < vertices.Count - 1; i++)
            {
                g.DrawLine(p, vertices[i], vertices[i + 1]);
            }
            int endInd = vertices.Count;
            g.DrawLine(p, vertices[endInd - 1], vertices[0]);
        }

        //Create edge list, each element is a edge bucket
        public void createEdges()
        {
            int oldSize = vertices.Count;
            vertices.Add(vertices[0]);
            for (int i = 0; i < oldSize; i++)
            {
                edgeBucket tmp = new edgeBucket(); ;
                Point p1 = vertices[i];
                Point p2 = vertices[i + 1];
                tmp.ymin = Math.Min(p1.Y, p2.Y);
                tmp.ymax = Math.Max(p1.Y, p2.Y);
                tmp.dX = Math.Abs(p2.X - p1.X);
                tmp.dY = Math.Abs(p2.Y - p1.Y);
                tmp.sign = ((p2.X - p1.X) * (p2.Y - p1.Y) < 0) ? -1 : 1;
                tmp.sum = 0;
                tmp.x = (tmp.ymin == p1.Y) ? p1.X : p2.X;
                //add nhung cai nonhorizontal thoi
                if (tmp.dY > 0) edgeTable.Add(tmp);
            }
        }
        public void sortET()
        {
            for (int i = 0; i < edgeTable.Count - 1; i++)
            {
                for (int j = i + 1; j < edgeTable.Count; j++)
                {
                    if (edgeTable[j].ymin < edgeTable[i].ymin)
                    {
                        edgeBucket tmp = edgeTable[i];
                        edgeTable[i] = edgeTable[j];
                        edgeTable[j] = tmp;
                    }
                }
            }
        }
        public void sortAL()
        {
            for (int i = 0; i < activeList.Count - 1; i++)
            {
                for (int j = i + 1; j < activeList.Count; j++)
                {
                    if ((activeList[j].x < activeList[i].x) || (activeList[j].x == activeList[i].x && activeList[j].sign < activeList[i].sign))
                    {
                        edgeBucket tmp = activeList[i];
                        activeList[i] = activeList[j];
                        activeList[j] = tmp;
                    }

                }
            }
        }
        public void processET(int yScanLine, ref Bitmap bmp)
        {
            while (edgeTable.Count > 0)
            {
                //remove edges from active list if y == ymax
                if (activeList.Count > 0)
                {
                    for (int i = 0; i < activeList.Count; i++)
                    {
                        edgeBucket curEdge = activeList[i];
                        if (curEdge.ymax == yScanLine)
                        {
                            activeList.RemoveAt(i);
                            edgeTable.RemoveAt(i);
                            i--;
                        }
                    }
                }
                //Add edge from edge table to active list if y==ymin
                for (int i = 0; i < edgeTable.Count; i++)
                {
                    if (edgeTable[i].ymin == yScanLine)
                    {
                        activeList.Add(edgeTable[i]);
                    }
                }
                //Sort active list by x position and slope
                sortAL();
                //Fill the polygon pixel
                for (int i = 0; i < activeList.Count - 1; i += 2)
                {
                    int xLeft = (int)Math.Min(activeList[i].x, activeList[i + 1].x);
                    int xRight = (int)Math.Max(activeList[i].x, activeList[i + 1].x);
                    
                    for (int x = xLeft; x <= xRight; x++)
                    {
                        bmp.SetPixel(x, yScanLine, Color.Red);
                    }
                }
                //Increament X variables of buckets based on the slope
                for (int i = 0; i < activeList.Count; i++)
                {
                    double d = (activeList[i].sign) * (activeList[i].dX * 1.0 / activeList[i].dY);
                    activeList[i].x += (activeList[i].sign) * (activeList[i].dX * 1.0 / activeList[i].dY);
                }
                //increase yScanline
                yScanLine++;
            }
        }
        public void scanLine(ref Bitmap bmp)
        {
            createEdges();
            sortET();
            int y = edgeTable[0].ymin;
            processET(y, ref bmp);
        }

        public void boundaryFill(ref Bitmap bmp, int x, int y, Color boundaryColor, Color fillColor)
        {
            Color currentColor = bmp.GetPixel(x, y);

            if (!currentColor.ToArgb().Equals(boundaryColor.ToArgb()) && !currentColor.ToArgb().Equals(fillColor.ToArgb()))
            {
                bmp.SetPixel(x, y, fillColor);
                boundaryFill(ref bmp, x - 1, y, boundaryColor, fillColor);
                boundaryFill(ref bmp, x, y + 1, boundaryColor, fillColor);
                boundaryFill(ref bmp, x + 1, y, boundaryColor, fillColor);
                boundaryFill(ref bmp, x, y - 1, boundaryColor, fillColor);
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
