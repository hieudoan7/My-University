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
    class MyCircle : MyRectangle
    {
        public MyCircle() : base() { }
        public override int objectType()
        {
            return 6;
        }
        public override Shape Clone()
        {
            MyCircle x = new MyCircle();

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
                mRec = new GraphicsPath();
                gp = new GraphicsPath();
                float spX = Math.Min(startPoint.X, endPoint.X);
                float spY = Math.Min(startPoint.Y, endPoint.Y);
                RectangleF rec = new RectangleF(spX, spY, Math.Max(width, height), Math.Max(width, height));
                mRec.AddRectangle(rec);
                gp.AddEllipse(rec);
            }
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            base.fillPath(g,gp);
            g.DrawPath(p, gp);
            //gp.Dispose();
            //show control point
            if (this.isSelect)
            {
                this.getListControlPoint();
                this.showControlPoint(g);
            }
        }
        public override void mouseMove(MouseEventArgs e)
        {
            if (isDraw)
            {
                float temp;
                if (Math.Abs(e.X - startPoint.X) > Math.Abs(e.Y - startPoint.Y)) 
                temp = Math.Abs(e.X - startPoint.X);
                else temp = Math.Abs(e.Y - startPoint.Y);
                float t1, t2;
                if (e.X > startPoint.X)
                    t1 = temp;
                else t1 = -temp;
                if (e.Y > startPoint.Y)
                    t2 = temp;
                else t2 = -temp;
                this.endPoint = new PointF(t1+startPoint.X,t2+startPoint.Y);
            }
            //move
            if (isMove && e.Button == MouseButtons.Left)
            {
                float dX = e.X - anchor.X, dY = e.Y - anchor.Y;
                Matrix x = new Matrix();
                x.Translate(dX, dY);
                mRec.Transform(x);
                gp.Transform(x);

                startPoint.X += dX;
                endPoint.X += dX;
                startPoint.Y += dY;
                endPoint.Y += dY;

                anchor = e.Location;
            }
            if (isResize /*&& e.Button == MouseButtons.Left*/)
            {
                Scale(currentCpIndex, e);
                anchor = e.Location;

            }
            if (isRotate)
            {
                rotate(e);
            }
            //this.checkOrder();
        }
        public override void mouseUp(MouseEventArgs e,object sender)
        {
            if (isDraw)
            {
                float temp;
                if (Math.Abs(e.X - startPoint.X) > Math.Abs(e.Y - startPoint.Y))
                    temp = Math.Abs(e.X - startPoint.X);
                else temp = Math.Abs(e.Y - startPoint.Y);
                float t1, t2;
                if (e.X > startPoint.X)
                    t1 = temp;
                else t1 = -temp;
                if (e.Y > startPoint.Y)
                    t2 = temp;
                else t2 = -temp;
                this.endPoint = new PointF(t1 + startPoint.X, t2 + startPoint.Y);
                //format2point();
                mRegion = new Region(mRec);
                //his.makeBound();
                //chuan bi sau nay thoi
            }
            //this.checkOrder();
            this.release(); //isMove = false & isDraw = false & isReszie=false
        }
        public override void Scale(int Type, MouseEventArgs e)
        {
            PointF t = e.Location;
            float dX, dY, d1, d2, dXx, dYy, t1, t2;
            int temp = Type;
            Type = Type / 2;

            PointF ConstP = mRec.PathPoints[(Type + 2) % 4];
            PointF CurP = mRec.PathPoints[Type];


            d1 = -ConstP.X;
            d2 = -ConstP.Y;

            //if (ConstP.X == t.X || ConstP.Y == t.Y)
            //{ anchor = e.Location; return; }

            Matrix x = new Matrix();
            x.Translate(d1, d2);
            mRec.Transform(x);
            gp.Transform(x);
            t.X += d1;
            t.Y += d2;

            x = new Matrix();
            x.Rotate(-angle);
            mRec.Transform(x);
            gp.Transform(x);
            t = rotateP(t, new PointF(0, 0), -angle);
            ConstP = mRec.PathPoints[(Type + 2) % 4];
            CurP = mRec.PathPoints[Type];

            float xx = Math.Max(Math.Abs(t.X - ConstP.X), Math.Abs(t.Y - ConstP.Y));
            float x1, x2;
            if (ConstP.X < t.X)
                x1 = xx;
            else x1 = -xx;
            if (ConstP.Y < t.Y)
                x2 = xx;
            else x2 = -xx;
            t = new PointF(ConstP.X + x1, ConstP.Y + x2);
            dX = t.X - ConstP.X;
            dY = t.Y - ConstP.Y;
            dXx = CurP.X - ConstP.X;
            dYy = CurP.Y - ConstP.Y;

            t1 = dX / dXx;
            t2 = dY / dYy;

            x = new Matrix();
            if (Math.Abs(dX) < 1)
            {
                t1 = -1F;
            }
            if (Math.Abs(dY) < 1)
            {
                t2 = -1F;
            }
            x.Scale(t1, t2);



            mRec.Transform(x);
            gp.Transform(x);

            x = new Matrix();
            x.Rotate(angle);
            mRec.Transform(x);
            gp.Transform(x);

            x = new Matrix();
            x.Translate(-d1, -d2);
            mRec.Transform(x);
            gp.Transform(x);
            PointF Center = CenterPoint(mRec.PathPoints[0], mRec.PathPoints[2]);

            startPoint = rotateP(mRec.PathPoints[0], Center, -angle);
            endPoint = rotateP(mRec.PathPoints[2], Center, -angle);



        }
        //serialize & deserialize
        protected MyCircle(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
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
            gp = new GraphicsPath();
            mRec = new GraphicsPath();
            float spX = Math.Min(startPoint.X, endPoint.X);
            float spY = Math.Min(startPoint.Y, endPoint.Y);
            RectangleF rec = new RectangleF(spX, spY, Math.Max(width, height), Math.Max(width, height));
            gp.AddEllipse(rec);
            mRec.AddRectangle(rec);
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
