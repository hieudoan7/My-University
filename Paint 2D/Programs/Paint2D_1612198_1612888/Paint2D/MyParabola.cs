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
    class MyParabola : MyRectangle
    {
        public PointF O;
        public float f = 50;
        public MyParabola() : base() { }
        public override int objectType()
        {
            return 8;
        }
        /* public MyParabola(MyParabola x):base(x)
         {
             O = x.O;
             f = x.f;
         }*/
        public override Shape Clone()
        {
            MyParabola x = new MyParabola();
            x.O = O;
            x.f = f;
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
                //mRec = new Rectangle(spX, spY, Math.Min(width, height),Math.Max(width, height));
                RectangleF rec = new RectangleF(spX, spY, width, height);

                O = new PointF(width / 2 + spX, spY+height);
                gp = new GraphicsPath();
                mRec = new GraphicsPath();
                
                List<PointF> l = getPointParabola(true); //right
                if (l.Count > 0) gp.AddLines(l.ToArray());
                l.Clear();
                mRec.AddRectangle(rec);
                l = getPointParabola(false); //left
                gp.StartFigure();
                if (l.Count > 0) gp.AddLines(l.ToArray());
            }
            if (f != 0)
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                //base.fillPath(g,gp);
                g.DrawPath(p, gp);
            }
            
            //g.Dispose();
            //show control point
            if (this.isSelect)
            {
                this.getListControlPoint();
                this.showControlPoint(g);
            }
        }
        public List<PointF> getPointParabola(bool mode)
        {
            List<PointF> listPointPara = new List<PointF>();
            
                
                
            //format2point();
            float width = Math.Abs(endPoint.X - startPoint.X);
            float height = Math.Abs(endPoint.Y - startPoint.Y);
            float spX = Math.Min(startPoint.X, endPoint.X);
            float spY = Math.Min(startPoint.Y, endPoint.Y);
            f = (Math.Abs(spX - O.X) * Math.Abs(spX - O.X) / (2 * Math.Abs(spY - O.Y)));
            Double x = 0, y = 0;
                while (y <= height && x <= (width) / 2)
                {
                    y = (0.5/f) * x * x;
                    if (mode) listPointPara.Add(new PointF(O.X + (int)x, O.Y - (int)y));
                    if (!mode) listPointPara.Add(new PointF(O.X - (int)x, O.Y - (int)y));
                    x++;
                }
            
            return listPointPara;
        }

        public override void setColor(Color c)
        {
            //fillColor = c;
            p.Color = c;
        }
        //serialize & deserialize
        protected MyParabola(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
            this.O = (PointF)info.GetValue("O", typeof(PointF));
            this.f = (float)info.GetValue("f", typeof(float));
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            base.GetObjectData(info, ctxt);
            info.AddValue("O", this.O, typeof(PointF));
            info.AddValue("f", this.f, typeof(float));
        }
        public override void Load()
        {
            float width = Math.Abs(endPoint.X - startPoint.X);
            float height = Math.Abs(endPoint.Y - startPoint.Y);
            float spX = Math.Min(startPoint.X, endPoint.X);
            float spY = Math.Min(startPoint.Y, endPoint.Y);
            //mRec = new Rectangle(spX, spY, Math.Min(width, height),Math.Max(width, height));
            RectangleF rec = new RectangleF(spX, spY, width, height);

            O = new PointF(width / 2 + spX, spY + height);
            gp = new GraphicsPath();
            mRec = new GraphicsPath();

            List<PointF> l = getPointParabola(true); //right
            if (l.Count > 0) gp.AddLines(l.ToArray());
            l.Clear();
            mRec.AddRectangle(rec);
            l = getPointParabola(false); //left
            gp.StartFigure();
            if (l.Count > 0) gp.AddLines(l.ToArray());
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
