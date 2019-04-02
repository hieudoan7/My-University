using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Paint2D
{
    [Serializable]
    class MyArc:MyRectangle
    {
        public MyArc() : base() { }
        public override void Draw(Graphics g)
        {
            if (isDraw)
            {

                //phải xét lại để trong quá trình MouseMove
                float width = Math.Abs(endPoint.X - startPoint.X);
                float height = Math.Abs(endPoint.Y - startPoint.Y);
                gp = new GraphicsPath();
                mRec = new GraphicsPath();
                float spX = Math.Min(startPoint.X, endPoint.X);
                float spY = Math.Min(startPoint.Y, endPoint.Y);
                RectangleF rec = new RectangleF(spX, spY, Math.Max(1,width), Math.Max(1,height));

                mRec.AddRectangle(rec);
                gp.AddArc(rec,0F,-180.0F);
            }
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //base.fillPath(g, gp);
            g.DrawPath(p, gp);
            //gp.Dispose();
            //show control point
            if (this.isSelect)
            {
                this.getListControlPoint();
                this.showControlPoint(g);
            }
        }

        public override int objectType()
        {
            return 7;
        }
        public override Shape Clone()
        {
            MyArc x = new MyArc();

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
        //serialize & deserialize
        protected MyArc(SerializationInfo info, StreamingContext ctxt):base(info, ctxt)
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
            RectangleF rec = new RectangleF(spX, spY, Math.Max(1, width), Math.Max(1, height));

            mRec.AddRectangle(rec);
            gp.AddArc(rec, 0F, -180.0F);
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
