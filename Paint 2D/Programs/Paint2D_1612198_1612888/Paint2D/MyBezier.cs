using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Security.Permissions;
using System.Runtime.Serialization;
namespace Paint2D
{
    [Serializable]
    class MyBezier : Shape
    {
        PointF[] boundPoint=new PointF[4];
        public int cur = 3;
        public MyBezier() : base()
        {

        }
        public override int objectType()
        {
            return 10;
        }
        public override Shape Clone()
        {
            MyBezier x = new MyBezier();

            x.boundPoint = (PointF[])boundPoint.Clone();
            x.cur = cur;
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
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gp = new GraphicsPath();
            gp.AddBezier(boundPoint[0], boundPoint[1], boundPoint[2], boundPoint[3]);

            g.DrawPath(p, gp);

            //gp.Dispose();
            //ve xong roi reformat
            //mRegion = new Region(mRec);
            //show control point
            if (this.isSelect)
            {
                this.getListControlPoint();
                this.showControlPoint(g);
            }
        }
        //Override isContain
        public override bool isContain(System.Drawing.Point location)
        {
            return gp.IsVisible(location) || gp.IsOutlineVisible(location, p);
        }
        public override bool isDone()
        {
            return cur == -1;
        }
        public override void mouseDown(MouseEventArgs e)
        {
            //ve
            if (this.isDraw)
            {
                if (cur == 3)
                {
                    boundPoint[0] = e.Location;
                    boundPoint[1] = e.Location;
                    boundPoint[2] = e.Location;
                    boundPoint[3] = e.Location;
                }

            }

            //move or resize
            if (this.isMove || this.isResize)
            {

                this.anchor = e.Location;
            }

        }
        public override void mouseMove(MouseEventArgs e)
        {
            if (isDraw)
            {
                boundPoint[cur] = e.Location;
                if (cur == 1) boundPoint[cur + 1] = e.Location;
            }
            //move
            if (isMove && e.Button == MouseButtons.Left)
            {
                PointF t;
                float deltaX = e.Location.X - anchor.X;
                float deltaY = e.Location.Y - anchor.Y;
                for (int i = 0; i < 4; i++)
                {
                    t = boundPoint[i];
                    t.X += deltaX;
                    t.Y += deltaY;
                    boundPoint[i] = t;
                }
                anchor = e.Location;
            }
            if (isResize /*&& e.Button == MouseButtons.Left*/)
            {
                moveFollowControlPoint(e);
                //Update CurrentCp: Vi no chi duoc khoi tao luc Down thoi, nay la Move (mấy tiếng đồng hồ của bố) :'(

            }
        }

        public override void mouseUp(MouseEventArgs e,object sender)
        {
            if (isDraw)
            {
                boundPoint[cur] = e.Location;
                if (cur == 3) cur = 1;
                else if (cur == 1) cur = 2;
                else cur = -1;
                //format2point();
                //mRegion = new Region(mRec); //chuan bi sau nay thoi
            }
            if (cur == -1)
                release();
            //isMove = false & isDraw = false & isReszie=false
        }

        public override void getListControlPoint()
        {
            PointF t;
            listControlPoint.Clear();
            for (int i = 0; i < 4; i++)
            {
                t = boundPoint[i];
                this.listControlPoint.Add(new RectangleF(t.X - 5, t.Y - 5, 10, 10));
            }
        }

        //fill rectangle
        
        public void moveFollowControlPoint(MouseEventArgs e)
        {
            float deltaX = e.X - anchor.X;
            float deltaY = e.Y - anchor.Y;
            //
            //endPoint.X += deltaX;

            //
            PointF temp = boundPoint[currentCpIndex];
            temp.X += deltaX;
            temp.Y += deltaY;
            boundPoint[currentCpIndex] = temp;
            anchor = e.Location;

            //getListControlPoint();

        }
        public override void setColor(Color c)
        {
            //fillColor = c;
            p.Color = c;
        }
        //serialize & deserialize
        protected MyBezier(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
            this.boundPoint = (PointF[])info.GetValue("boundPoint", typeof(PointF[]));
            this.cur = info.GetInt32("cur");
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            base.GetObjectData(info, ctxt);
            info.AddValue("boundPoint", this.boundPoint, typeof(PointF[]));
            info.AddValue("cur", this.cur);
        }
    }
}
