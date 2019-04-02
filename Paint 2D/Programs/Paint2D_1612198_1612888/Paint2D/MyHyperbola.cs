using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Security.Permissions;
namespace Paint2D
{
    [Serializable]
    class MyHyperbola : MyRectangle
    {
        float a = 0, b = 0;
        PointF center;
        List<PointF>[] l; //list[4] containt 4 goc phan tu

        public MyHyperbola() : base() { }
        public override int objectType()
        {
            return 9;
        }
        /*public MyHyperbola(MyHyperbola x) : base(x)
        {
            a = x.a;
            b = x.b;
            center = x.center;
            l = x.l;
        }*/
        public override Shape Clone()
        {
            MyHyperbola x = new MyHyperbola();

            x.a = a;
            x.b = b;
            x.center = center;
            x.l = l;
            x.mRec = (GraphicsPath)mRec.Clone();
            x.startPoint = startPoint;
            x.endPoint = endPoint;
            x.angle = angle;
            x.p = (Pen)p.Clone();
            x.br = (HatchBrush)br.Clone();
            x.fillColor = fillColor;
            x.brStype = brStype;
            x.listControlPoint = listControlPoint;
            x.objType = objType;
            x.currentCp = currentCp;
            x.currentCpIndex = currentCpIndex;
            x.mRegion = mRegion.Clone();
            x.gp = (GraphicsPath)gp.Clone();
            x.anchor = anchor;
            x.isFill = isFill;
            x.isSelect = isSelect;
            x.isDraw = isDraw;
            x.isMove = isMove;
            x.isResize = isResize;
            x.isRotate = isRotate;
            return x;
        }
        public override void Draw(System.Drawing.Graphics g)
        {
            if (isDraw)
            {
                //phải xét lại để trong quá trình MouseMove
                float width = Math.Abs(endPoint.X - startPoint.X);
                float height = Math.Abs(endPoint.Y - startPoint.Y);
                float spX = Math.Min(startPoint.X, endPoint.X);
                float spY = Math.Min(startPoint.Y, endPoint.Y);
                RectangleF rec = new RectangleF(spX, spY, width, height);
                mRec = new GraphicsPath();
                //gp = new GraphicsPath();
                mRec.AddRectangle(rec);
                center = CenterPoint(new PointF(spX, spY), new PointF(spX + width, spY + height));

                a = (center.X - spX) / 5;
                b = (center.Y - spY) / 5;

                if (a != 0 && b != 0)
                {
                    l = new List<PointF>[4];
                    for (int i = 0; i < 4; i++)
                    {
                        l[i] = new List<PointF>();
                    }
                    DDA(height, width);
                    gp = new GraphicsPath();
                    for (int i = 0; i < 4; i++)
                    {
                        gp.AddLines(l[i].ToArray());
                        gp.StartFigure();

                    }

                }
                
            }
            if (a != 0 && b != 0)
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                //base.fillPath(g,gp);
                g.DrawPath(p, gp);
            }
               // g.Dispose();
            
            if (this.isSelect)
            {
                this.getListControlPoint();
                this.showControlPoint(g);
            }


        }

        public void hyperbolaPlotPoints(float x, float y)
        {
            float xCenter = center.X, yCenter = center.Y;

            l[0].Add(new PointF(xCenter + x, yCenter + y));
            l[1].Add(new PointF(xCenter - x, yCenter + y));
            l[2].Add(new PointF(xCenter + x, yCenter - y));
            l[3].Add(new PointF(xCenter - x, yCenter - y));

        }

        //DDA Algorithm
        public void DDA(float height,float width)
        {
            float x = a;
            float y = 0;
            hyperbolaPlotPoints(x, y);
            //region1
            while (a * a * y < b * b * x && y < height / 2 && x < width / 2)
            {
                y++;
                x = (float)(Math.Sqrt((1 + 1.0 * y * y / (b * b)) * a * a) + 0.5);
                hyperbolaPlotPoints(x, y);
            }
            //Region 2
            while (x < width / 2 && y < height / 2)
            {
                x++;
                y = (float)(Math.Sqrt((1.0 * x * x / (a * a) - 1) * b * b) + 0.5);
                hyperbolaPlotPoints(x, y);
            }
        }
        public override void setColor(Color c)
        {
            //fillColor = c;
            p.Color = c;
        }
        //serialize & deserialize
        protected MyHyperbola(SerializationInfo info, StreamingContext ctxt):base(info, ctxt)
        {
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            base.GetObjectData(info, ctxt);
        }
        public override void Load()
        {
            float width = Math.Abs(endPoint.X - startPoint.X);
            float height = Math.Abs(endPoint.Y - startPoint.Y);
            float spX = Math.Min(startPoint.X, endPoint.X);
            float spY = Math.Min(startPoint.Y, endPoint.Y);
            RectangleF rec = new RectangleF(spX, spY, width, height);
            mRec = new GraphicsPath();
            //gp = new GraphicsPath();
            mRec.AddRectangle(rec);
            center = CenterPoint(new PointF(spX, spY), new PointF(spX + width, spY + height));

            a = (center.X - spX) / 5;
            b = (center.Y - spY) / 5;

            if (a != 0 && b != 0)
            {
                l = new List<PointF>[4];
                for (int i = 0; i < 4; i++)
                {
                    l[i] = new List<PointF>();
                }
                DDA(height, width);
                gp = new GraphicsPath();
                for (int i = 0; i < 4; i++)
                {
                    gp.AddLines(l[i].ToArray());
                    gp.StartFigure();

                }

            }
            if (angle != 0)
            {
                Matrix temp = new Matrix();
                temp.RotateAt(angle, CenterPoint(startPoint, endPoint));
                mRec.Transform(temp);
                gp.Transform(temp);
            }
        }

    }
}


