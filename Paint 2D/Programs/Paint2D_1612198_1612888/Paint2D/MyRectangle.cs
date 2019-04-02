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
    class MyRectangle : Shape
    {
        //Rectangle Attribute
        protected GraphicsPath mRec;
        //----------------------

        //Default Construtor
        public MyRectangle():base(){}
        //---------------
        

       
        public override void Draw(System.Drawing.Graphics g)
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
                RectangleF rec = new RectangleF(spX, spY, width, height);
                gp.AddRectangle(rec);
                mRec.AddRectangle(rec);
                
            }
            
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            base.fillPath(g,gp);
            g.DrawPath(p, gp);


            if (this.isSelect)
            {
                this.getListControlPoint();
                this.showControlPoint(g);
            }
        }

        public override Shape Clone()
        {
            MyRectangle x = new MyRectangle();

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
        //Override isContain
        public override bool isContain(System.Drawing.Point location)
        {
            return gp.IsVisible(location) || gp.IsOutlineVisible(location,p);
        }
        public override void mouseDown(MouseEventArgs e) {
            //ve
            if (this.isDraw && e.Button==MouseButtons.Left)
            {
                this.startPoint = e.Location;
                this.endPoint = e.Location;
                //this.endPoint.X += 1; //tránh TH tồn tại width=height=0
            }
            //move or resize
            if (this.isMove || this.isResize)
            {
                this.anchor = e.Location;
            }
            
        }
        public override void mouseMove(MouseEventArgs e) {
            if (isDraw)
            {
                this.endPoint = e.Location;
            }
            //move
            if (isMove && e.Button==MouseButtons.Left)
            {
                float dX = e.X - anchor.X, dY = e.Y - anchor.Y;
                Matrix x = new Matrix();
                x.Translate(dX,dY);
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
        public override void mouseUp(MouseEventArgs e,object sender) {
            if (isDraw)
            {
                this.endPoint = e.Location;
                //format2point();
                mRegion = new Region(mRec);
                //his.makeBound();
                //chuan bi sau nay thoi
            }
            //this.checkOrder();
            this.release(); //isMove = false & isDraw = false & isReszie=false
        }
        public override void getListControlPoint()
        {
            // counter-lock thu tu theo chieu kim dong ho
            listControlPoint.Clear();
                PointF[] diem = new PointF[9];
            if(mRec.PointCount==0)
            {
                for (int i = 0; i < 4; i++)
                    diem[2 * i] = new PointF(0,0);
            }
            else
                for (int i = 0; i < 4; i++)
                    diem[2 * i] = mRec.PathPoints[i];

                diem[1] = new PointF((diem[0].X + diem[2].X) / 2, (diem[0].Y + diem[2].Y) / 2);
                diem[3] = new PointF((diem[2].X + diem[4].X) / 2, (diem[2].Y + diem[4].Y) / 2);
                diem[5] = new PointF((diem[4].X + diem[6].X) / 2, (diem[4].Y + diem[6].Y) / 2);
                diem[7] = new PointF((diem[6].X + diem[0].X) / 2, (diem[6].Y + diem[0].Y) / 2);
            float width = Math.Abs(endPoint.X - startPoint.X);
            float height = Math.Abs(endPoint.Y - startPoint.Y);
            float spX = Math.Min(startPoint.X, endPoint.X);
            float spY = Math.Min(startPoint.Y, endPoint.Y);
            diem[8] = rotateP(new PointF(spX+width/2,spY-20),CenterPoint(startPoint,endPoint),angle);
                for (int i = 0; i < 9; i++)
                {
                    this.listControlPoint.Add(new RectangleF(diem[i].X - 5, diem[i].Y - 5, 10, 10));

                }
            
        }
        
        //fill rectangle
        

        
        public virtual void Scale(int  Type,MouseEventArgs e)
        {
            PointF t = e.Location;
            float dX, dY, d1, d2,dXx,dYy,t1,t2;
            int temp=Type;
            if (Type % 2 == 0)
            {
                Type /= 2;
            }
            else if (Type == 1 || Type == 7)
            {
                Type = 0;
            }
            else Type = 2;

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
            if (temp == 1 || temp == 5)
                t = new PointF(CurP.X, t.Y);
            else if(temp==3||temp==7)
                t = new PointF(t.X, CurP.Y);

                dX = t.X - ConstP.X;
                dY = t.Y - ConstP.Y;
            dXx = CurP.X - ConstP.X;
            dYy = CurP.Y - ConstP.Y;
            
            t1 = dX / dXx;
            t2 = dY / dYy;

                x = new Matrix();
            if ( Math.Abs(dX) < 1 )
            {
                t1 = -1F;
            }
            if ( Math.Abs(dY) < 1)
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
        

        //more
        public bool isLeft(PointF a, PointF S, PointF E)
        {
            return (a.Y - S.Y) * (E.X - S.X) < (a.X - S.X) * (E.Y - S.Y);
        }
        public void rotate(MouseEventArgs e)
        {
            PointF center = CenterPoint(mRec.PathPoints[0], mRec.PathPoints[2]);
            double x1, y1, x2, y2;
            double d1, d2;
            x1 = this.listControlPoint[8].X - center.X;
            y1 = this.listControlPoint[8].Y - center.Y;
            x2 = e.X - center.X;
            y2 = e.Y - center.Y;
            d1 = Math.Sqrt(x1 * x1 + y1 * y1);
            d2 = Math.Sqrt(x2 * x2 + y2 * y2);
            
            float a = (float)(Math.Acos((Math.Min(1, (x1 * x2 + y1 * y2) / (d1 * d2)) )) * 180 / Math.PI);
            if (isLeft(e.Location, center, listControlPoint[8].Location))
            {
                a = -a;
            }
            this.angle +=a;
            if (angle >= 360) angle -= 360;
            if (angle <= 360) angle += 360;
            //this.angle += a;
            //this.makeBound();
            if (a != 0)
            {
                Matrix temp = new Matrix();
                temp.RotateAt(a, center);
                mRec.Transform(temp);
                gp.Transform(temp);
                //angle = 0;
            }
            startPoint = rotateP(mRec.PathPoints[0], center, -angle);
            endPoint = rotateP(mRec.PathPoints[2], center, -angle);

        }
        public PointF rotateP(PointF A, PointF Center, float angle)
        {
            float a = (float)(angle * Math.PI / 180);
            float x = (float)((A.X - Center.X) * Math.Cos(a) - (A.Y - Center.Y) * Math.Sin(a)) + Center.X;
            float y = (float)((A.Y - Center.Y) * Math.Cos(a) + (A.X - Center.X) * Math.Sin(a)) + Center.Y;

            return new PointF(x, y);
        }
        public PointF CenterPoint(PointF startPoint, PointF endPoint)
        {
            return new PointF((startPoint.X + endPoint.X) / 2, (startPoint.Y + endPoint.Y) / 2);
        }

        public override void showControlPoint(Graphics g)
        {
            Pen p = new Pen(Color.Red, 1.0F);
            Brush b = new SolidBrush(Color.Purple);
            /*GraphicsPath c = new GraphicsPath();
            foreach(RectangleF i in listControlPoint)
            {
                c.AddRectangle(i);
            }
            g.DrawPath(p,c);*/
            g.DrawRectangles(p, listControlPoint.ToArray());
            p = new Pen(Color.Aqua, 1.0F);
            p.DashStyle = DashStyle.Dash;
            g.DrawPath(p, mRec);
        }
        //serialize & deserialize
        protected MyRectangle(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
            //this.mRec = (GraphicsPath)info.GetValue("mRec", typeof(GraphicsPath));
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {

            base.GetObjectData(info, ctxt);
            //info.AddValue("mRec", this.mRec, typeof(GraphicsPath));
        }
        public override void Load()
        {
            float width = Math.Abs(endPoint.X - startPoint.X);
            float height = Math.Abs(endPoint.Y - startPoint.Y);
            gp = new GraphicsPath();
            mRec = new GraphicsPath();
            float spX = Math.Min(startPoint.X, endPoint.X);
            float spY = Math.Min(startPoint.Y, endPoint.Y);
            RectangleF rec = new RectangleF(spX, spY, width, height);
            gp.AddRectangle(rec);
            mRec.AddRectangle(rec);
            if(angle!=0)
            {
                Matrix temp = new Matrix();
                temp.RotateAt(angle,CenterPoint(startPoint,endPoint));
                mRec.Transform(temp);
                gp.Transform(temp);
            }
        }
    }
}
