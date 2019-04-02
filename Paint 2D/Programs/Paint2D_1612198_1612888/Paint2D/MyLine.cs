using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Security.Permissions;


namespace Paint2D
{
    [Serializable]
    class MyLine : Shape
    {
        public override int objectType()
        {
            return 1;
        }
        public MyLine() : base() { }
        public MyLine(Point sp, Point ep) : base()
        {
            startPoint = sp;
            endPoint = ep;
        }
        public override Shape Clone()
        {
            MyLine x = new MyLine();
            
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
            //x.gp = (GraphicsPath)gp.Clone();
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
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.DrawLine(this.p, this.startPoint, this.endPoint);
            //p.Dispose(); init o ngoai nen ko Dispose() duoc
            //show control point
            if (this.isSelect)
            {
                this.getListControlPoint();
                this.showControlPoint(g);
            }
        }
        public  void reDraw(System.Drawing.Graphics g,Pen pen)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.DrawLine(pen, this.startPoint, this.endPoint);
            //p.Dispose(); init o ngoai nen ko Dispose() duoc
            //show control point
            
        }
        public override void mouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //ve
                if (isDraw)
                {
                    this.startPoint = e.Location;
                    this.endPoint = e.Location;
                    isDraw = true;
                }
                //move
                if (isMove || this.isResize)
                {
                    //only 1 time
                    this.anchor = e.Location;
                }

            }

        }
        public override void mouseMove(MouseEventArgs e)
        {
            if (isDraw)
            {
                this.endPoint = e.Location;
            }
            //move

            if (isMove)
            {
                //MessageBox.Show("move");
                float deltaX = e.Location.X - anchor.X;
                float deltaY = e.Location.Y - anchor.Y;
                this.startPoint.X += deltaX;
                this.startPoint.Y += deltaY;
                this.endPoint.X += deltaX;
                this.endPoint.Y += deltaY;
                anchor = e.Location;
            }
            if (isResize)
            {
                moveFollowControlPoint(e);
            }
        }
        public void moveFollowControlPoint(MouseEventArgs e)
        {
            float deltaX = e.Location.X - anchor.X;
            float deltaY = e.Location.Y - anchor.Y;
            if (currentCpIndex == 0)
            {
                startPoint.X += deltaX;
                startPoint.Y += deltaY;
            }
            if (currentCpIndex == 1)
            {
                endPoint.X += deltaX;
                endPoint.Y += deltaY;
            }
            anchor = e.Location;

        }
        public override void mouseUp(MouseEventArgs e,object sender)
        {
            if (isDraw)
            {
                this.endPoint = e.Location;
                this.isDraw = false;
                //format2point();
            }
            //getListControlPoint();
            this.release();
        }
        public override void getListControlPoint()
        {
            listControlPoint.Clear();
            this.listControlPoint.Add(new RectangleF(startPoint.X - 5, startPoint.Y - 5, 10, 10));
            this.listControlPoint.Add(new RectangleF(endPoint.X - 5, endPoint.Y - 5, 10, 10));
        }
        //Override isContain
        public override bool isContain(Point loc)
        {
            if (loc.X < Math.Min(startPoint.X, endPoint.X) ||
                loc.X > Math.Max(startPoint.X, endPoint.X) ||
                loc.Y < Math.Min(startPoint.Y, endPoint.Y) ||
                loc.Y > Math.Max(startPoint.Y, endPoint.Y))
                return false;
            Double m = 1.0 * (endPoint.Y - startPoint.Y) / (endPoint.X - startPoint.X);
            Double b = 1.0 * startPoint.Y - m * startPoint.X;
            if (Math.Abs(loc.Y - (loc.X * m + b)) <= 1)
            {
                return true;
            }
            else return false;
        }
        //---
        public void setLineRegion()
        {
            GraphicsPath gpTmp = new GraphicsPath();
            gpTmp.AddLine(startPoint.X, startPoint.Y - 3, endPoint.X, endPoint.Y - 3);
            gpTmp.AddLine(endPoint.X, endPoint.Y - 3, endPoint.X, endPoint.Y + 3);
            gpTmp.AddLine(endPoint.X, endPoint.Y + 3, startPoint.X, startPoint.Y + 3);
            gpTmp.AddLine(startPoint.X, startPoint.Y + 3, startPoint.X, startPoint.Y - 3);
            mRegion = new Region(gpTmp);
        }
        public override void setColor(Color c)
        {
            //fillColor = c;
            p.Color = c;
        }
        //serialize & deserialize
        protected MyLine(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            base.GetObjectData(info, ctxt);
        }
    }
}
