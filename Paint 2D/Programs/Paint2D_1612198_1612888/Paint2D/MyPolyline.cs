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
    class MyPolyline : Shape
    {
        //attribute
        List<MyLine> listLine;
        MyLine curLine;
        bool finised = false;
        public override int objectType()
        {
            return 4;
        }

        public MyPolyline() : base()
        {
            listLine = new List<MyLine>();
            //curLine = new MyLine();
        }
        /*public MyPolyline(MyPolyline x) : base(x)
        {
            listLine = x.listLine;
            curLine = x.curLine;
        }*/
        public override Shape Clone()
        {
            MyPolyline x = new MyPolyline();

            x.finised = finised;
            x.firstTime = firstTime;
            x.listLine = new List<MyLine>();
            foreach(MyLine t in listLine)
            {
                x.listLine.Add((MyLine)t.Clone());
            }
            x.curLine = (MyLine)curLine.Clone();
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
        public override void setColor(Color c)
        {
            //fillColor = c;
            p.Color = c;
        }
        public override void Draw(Graphics g)
        {
            foreach (MyLine l in listLine)
            {
                l.reDraw(g,p);
            }
            if(isSelect)
            {
                getListControlPoint();
                showControlPoint(g);
            }
        }

        //Override setSelect()
        public override void setSelect()
        {
            base.setSelect();
            foreach (MyLine l in listLine)
            {
                l.setSelect();
            }
        }
        public override void unSelect()
        {
            base.unSelect();
            foreach (MyLine l in listLine)
            {
                l.unSelect();
            }

        }
        //
        bool firstTime = true;
        public override void mouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //ve
                if (this.isDraw)
                {
                    if (firstTime)
                    {
                        //first time
                        curLine = new MyLine();
                        curLine.mouseDown(e);
                        curLine.startPoint = e.Location;
                        listLine.Add(curLine);
                        firstTime = false;
                    }
                    curLine.endPoint = e.Location;
                    this.setSelect();
                }
                else if(isMove||isResize)
                        { anchor = e.Location; }


            }

        }

        public void Move(ref MyLine t,MouseEventArgs e)
        {
            float deltaX = e.Location.X - anchor.X;
            float deltaY = e.Location.Y - anchor.Y;
            t.startPoint.X += deltaX;
            t.startPoint.Y += deltaY;
            t.endPoint.X += deltaX;
            t.endPoint.Y += deltaY;
            //anchor = e.Location;
        }
        public override void mouseMove(MouseEventArgs e)
        {
            if (this.isDraw) curLine.mouseMove(e);
            if(isMove)
            {
                for(int i=0;i<listLine.Count;i++)
                {
                    MyLine t = listLine[i];
                    Move(ref t, e);
                    listLine[i] = t;
                }
            }
            if(isResize)
            {
                int i = 0;
                for (;i<listControlPoint.Count-1;i++)
                {
                    if (listControlPoint[i].Contains(anchor))
                        break;
                }
                if (i == 0)
                {
                    listLine[0].startPoint = e.Location;
                }
                else if (i < listControlPoint.Count - 1)
                {
                    listLine[i].startPoint = e.Location;
                    listLine[i - 1].endPoint = e.Location;
                }
                else
                    listLine[i-1].endPoint = e.Location;
            }

            anchor = e.Location;

        }
        public override void mouseUp(MouseEventArgs e, object sender)
        {
            if (this.isDraw)
            {
                curLine.endPoint = e.Location;
                curLine = new MyLine();
                curLine.isDraw = true;
                listLine.Add(curLine);
                curLine.startPoint = e.Location;
                curLine.endPoint = e.Location;
            }
            if(isMove)
            {
                isMove = false;
            }
            if(isResize)
            {
                isResize = false;
            }
           // this.release();

        }

        public override void mouseDoubleClick(int mode, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDraw = false;
                listLine.Remove(listLine[listLine.Count - 1]);
                finised = true;
            }
        }

        public override bool isContain(Point location)
        {
            //return base.isContain(location);
            foreach (MyLine l in listLine)
            {
                if (l.isContain(location))
                {
                    return true;
                }
            }
            return false;
        }
        public override bool isEmptyListLine()
        {
            return this.listLine.Count == 0;
        }

        public override void getListControlPoint()
        {
            listControlPoint.Clear();
            foreach(MyLine t in listLine)
            this.listControlPoint.Add(new RectangleF(t.startPoint.X - 5, t.startPoint.Y - 5, 10, 10));
            this.listControlPoint.Add(new RectangleF(listLine[listLine.Count-1].endPoint.X - 5, listLine[listLine.Count - 1].endPoint.Y - 5, 10, 10));
        }
        public override bool isDone()
        {
            return finised;
        }
        //serialize & deserialize
        protected MyPolyline(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
            //int cnt =(int)info.GetValue("numberOfLine",typeof(int));
            //for (int i = 0; i < cnt; i++)
            //{
            //    MyLine ml = (MyLine)info.GetValue("listLine" + i.ToString(), typeof(MyLine));
            //    this.listLine.Add(ml);
            //}
            this.listLine = (List<MyLine>)info.GetValue("listLine", typeof(List<MyLine>));
            this.curLine = (MyLine)info.GetValue("curLine", typeof(MyLine));
            foreach (MyLine l in this.listLine)
            {
                l.p = new Pen(Color.Black, 1.0F);
                //l.p = new Pen(this.pColor, this.pWidth);
                //l.setDash(this.pDashStyle);
            }
            //this.setPenSize(this.pWidth);
            //this.setDash(this.pDashStyle);
            //this.setColor(this.pColor);
        }
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            base.GetObjectData(info, ctxt);
            //cnt = this.listLine.Count;
            //info.AddValue("numberOfLine", cnt,typeof(int));
            //for (int i = 0; i < this.listLine.Count; i++)
            //{
            //    info.AddValue("listLine" + i.ToString(), this.listLine[i], typeof(MyLine));

            //}
            info.AddValue("listLine", this.listLine, typeof(List<MyLine>));
            info.AddValue("curLine", this.curLine, typeof(MyLine));
        }
    }
}
