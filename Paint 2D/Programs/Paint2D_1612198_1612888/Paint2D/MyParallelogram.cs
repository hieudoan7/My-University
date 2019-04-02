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
    class MyParallelogram : MyRectangle
    {
        //MyParallelogram Attribute Add 1 doan de scale
        protected float subtract; //pixel sau - pixel dau
        //protected Rectangle mRec;
        //
        protected float Sub;
        //Default Construtor
        public override int objectType()
        {
            return 3;
        }
        public MyParallelogram() : base()
        {

        }
        /*public MyParallelogram(MyParallelogram x):base(x)
        {
            subtract = x.subtract;
            Sub = x.Sub;
        }*/
        //----
        public override Shape Clone()
        {
            MyParallelogram x = new MyParallelogram();
            x.subtract = subtract;
            x.Sub = Sub;
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
               
                float height = Math.Abs(endPoint.Y - startPoint.Y);
                float width = Math.Abs(endPoint.X - startPoint.X);
                float spX = Math.Min(startPoint.X, endPoint.X);
                float spY = Math.Min(startPoint.Y, endPoint.Y);
                float epX = Math.Max(startPoint.X, endPoint.X);
                float epY = Math.Max(startPoint.Y, endPoint.Y);
                gp = new GraphicsPath();
                mRec = new GraphicsPath();
                if (isDraw)
                { subtract = width / 4; Sub = subtract; }
                PointF B = new PointF(epX - subtract, spY);
                PointF D = new PointF(spX + subtract, epY);
                RectangleF rec = new RectangleF(spX, spY, width, height);
                mRec.AddRectangle(rec);
                gp.AddLine(spX, spY, B.X, B.Y);
                gp.AddLine(B.X, B.Y, epX, epY);
                gp.AddLine(epX, epY, D.X, D.Y);
                gp.AddLine(D.X, D.Y, spX, spY);
            }
            
            mRegion = new Region(gp);
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
        public  void reDraw()
        {
                float height = Math.Abs(endPoint.Y - startPoint.Y);
                float width = Math.Abs(endPoint.X - startPoint.X);
                float spX = Math.Min(startPoint.X, endPoint.X);
                float spY = Math.Min(startPoint.Y, endPoint.Y);
                float epX = Math.Max(startPoint.X, endPoint.X);
                float epY = Math.Max(startPoint.Y, endPoint.Y);
                gp = new GraphicsPath();
                mRec = new GraphicsPath();
                
                PointF B = new PointF(epX - subtract, spY);
                PointF D = new PointF(spX + subtract, epY);
                RectangleF rec = new RectangleF(spX, spY, width, height);
                mRec.AddRectangle(rec);
                gp.AddLine(spX, spY, B.X, B.Y);
                gp.AddLine(B.X, B.Y, epX, epY);
                gp.AddLine(epX, epY, D.X, D.Y);
                gp.AddLine(D.X, D.Y, spX, spY);
            
            mRegion = new Region(gp);
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            //g.DrawPath(p, gp);
            //gp.Dispose();
            //show control point
            
        }
        //Do trên Draw() Dispose() rồi nên ko được dùng, phải mở Dispose() ra

        public override bool isContain(Point location)
        {
            return gp.IsVisible(location) || gp.IsOutlineVisible(location, p);
        }

        //serialize & deserialize
        protected MyParallelogram(SerializationInfo info, StreamingContext ctxt)
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
            float height = Math.Abs(endPoint.Y - startPoint.Y);
            float width = Math.Abs(endPoint.X - startPoint.X);
            float spX = Math.Min(startPoint.X, endPoint.X);
            float spY = Math.Min(startPoint.Y, endPoint.Y);
            float epX = Math.Max(startPoint.X, endPoint.X);
            float epY = Math.Max(startPoint.Y, endPoint.Y);
            gp = new GraphicsPath();
            mRec = new GraphicsPath();
            
            subtract = width / 4; Sub = subtract;
            PointF B = new PointF(epX - subtract, spY);
            PointF D = new PointF(spX + subtract, epY);
            RectangleF rec = new RectangleF(spX, spY, width, height);
            mRec.AddRectangle(rec);
            gp.AddLine(spX, spY, B.X, B.Y);
            gp.AddLine(B.X, B.Y, epX, epY);
            gp.AddLine(epX, epY, D.X, D.Y);
            gp.AddLine(D.X, D.Y, spX, spY);
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
