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
    class Shape : ISerializable
    {
        //position of shape
        
        public PointF startPoint;
        public PointF endPoint;

        //angle
        public float angle;
        //Pen and Brush
        public Pen p;
        //
        public HatchBrush br;
        public Color fillColor=Color.Blue;
        public int brStype=2;
        //Control Point
        public List<RectangleF> listControlPoint;
        public int objType;

        public RectangleF currentCp;
        public int currentCpIndex;
        //GraphicsPath and Region
        public Region mRegion;
        public GraphicsPath gp;

        //Anchor & Check variable
        public PointF anchor;
        public bool isFill;
        public bool isSelect;
        public bool isDraw;
        public bool isMove;
        public bool isResize;
        public bool isRotate;


        //default constructor
        public Shape()
        {
            startPoint = new Point(0, 0);
            endPoint = new Point(0, 0);

            angle = 0;

            p = new Pen(Color.Black, 1.0F);

            //br = new SolidBrush(Color.Blue);
             br= new HatchBrush(HatchStyle.BackwardDiagonal, Color.Blue, Color.Blue);
            
            mRegion = new Region();
            
            listControlPoint = new List<RectangleF>();
            

            isFill = false;
            isSelect = false;
            isDraw = false;
            isMove = false;
            isResize = false;
            isRotate = false;
        }
        public virtual Shape Clone()
        {
            Shape x = new Shape();
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
        public virtual void setColor(Color c )
        {
            if (isFill)
                fillColor = c;
            else
                p.Color = c;
        }
        public Color getColor()
        {
            if (isFill)
                return fillColor;
            return p.Color;
        }
        public void setDash(DashStyle t)
        {
            p.DashStyle = t;
        }
        public void setPenSize(float x)
        {
            p.Width = x;
        }
        public virtual void setFill(bool Type)
        {
            isFill = Type;
        }
        public void setBrushStype(int i)
        {
            brStype = i;
        }
        public virtual void setFont(Font t) { }
        public virtual void setBMP(Bitmap x)
        {

        }
        public virtual Bitmap getBMP()
        {
            return null;
        }
        public virtual int objectType()
        {
            return -1;
        }

        public virtual void setBrush()
        {
            switch(brStype)
            {
                case 0:
                    br = new HatchBrush(HatchStyle.Cross, fillColor);
                    break;
                case 1:
                    br = new HatchBrush(HatchStyle.Divot, fillColor);
                    break;
                case 2:
                    br = new HatchBrush(HatchStyle.Cross, fillColor,fillColor);
                    break;
                case 3:
                    br = new HatchBrush(HatchStyle.SolidDiamond, fillColor);
                    break;
                case 4:
                    br = new HatchBrush(HatchStyle.Wave, fillColor);
                    break;
                case 5:
                    br = new HatchBrush(HatchStyle.ZigZag, fillColor);
                    break;
            }
        }
        public virtual bool isDone()
        {
            return true;
        }
        //check variable
        public virtual void unSelect()
        {
            this.isSelect = false;
        }
        public virtual void setSelect(){
            this.isSelect=true;
        }
        //Method
        public virtual void Draw(Graphics g)
        {
        }
        public virtual void rotate(Graphics g) { }
        public virtual void mouseDown(MouseEventArgs e) { }
        public virtual void mouseMove(MouseEventArgs e) { }
        public virtual void mouseUp(MouseEventArgs e,object sender) { }
        public virtual bool isContain(Point location) {
            //return mRegion.IsVisible(location);
            return false;
        }
        public virtual void fillPath(Graphics g,GraphicsPath gp)
        {
            if (isFill)
            {
                setBrush();
                g.FillPath(br, gp);
                
            }
        }
        //control point
        public virtual void showControlPoint(Graphics g)
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
            
        }
        public virtual void getListControlPoint() { }
        //--------
        public virtual void format2point()
        {
            float spX = Math.Min(startPoint.X, endPoint.X);
            float spY = Math.Min(startPoint.Y, endPoint.Y);
            float epX = Math.Max(startPoint.X, endPoint.X);
            float epY = Math.Max(startPoint.Y, endPoint.Y);
            //reformat 2 point
            startPoint = new PointF(spX, spY);
            endPoint = new PointF(epX, epY);
        }
        public virtual void mouseDoubleClick(int mode, MouseEventArgs e) { }
        // Kiểm tra vị trí tương đối của 1 điểm và 1 đối tượng
        // - 1 : Nằm ngoài đối tượng
        // 0   : Trong đối tượng
        // > 1 : Điểm điều khiển 
        public virtual int HitTest(Point point)
        {
            for (int i = 0; i < this.listControlPoint.Count; i++)
            {
                if (listControlPoint[i].Contains(point))
                    return i+1;
            }
            if (this.isContain(point))
                return 0;
            return -1;
        }
        public virtual void release()
        {
            this.isDraw = false;
            this.isMove = false;
            //this.isSelect = false;
            //this.isFill = false;
            this.isResize = false;
            this.isRotate = false;
        }
        
        public virtual void delete()
        {

        }
        public virtual bool isEmptyListLine()
        {
            return true; //check first create
        }
        //serialize & deserialize
        protected Shape(SerializationInfo info, StreamingContext ctxt)
        {
            this.startPoint = (PointF)info.GetValue("startPoint", typeof(PointF));
            this.endPoint = (PointF)info.GetValue("endPoint", typeof(PointF));
            this.angle = (float)info.GetValue("angle", typeof(float));
            //this.p = (Pen)info.GetValue("p", typeof(Pen));
            this.p = new Pen(Color.Black);
            this.p.Color = (Color)info.GetValue("pColor", typeof(Color));
            this.p.DashStyle = (DashStyle)info.GetValue("pDashStyle", typeof(DashStyle));
            this.p.Width = (float)info.GetValue("pWidth", typeof(float));
            //this.br = (HatchBrush)info.GetValue("br", typeof(HatchBrush));
            this.fillColor = (Color)info.GetValue("fillColor", typeof(Color));
            this.brStype = info.GetInt32("brStype");
            this.listControlPoint = (List<RectangleF>)info.GetValue("listControlPoint", typeof(List<RectangleF>));
            this.objType = info.GetInt32("objType");
            this.currentCp = (RectangleF)info.GetValue("currentCp", typeof(RectangleF));
            this.currentCpIndex = info.GetInt32("currentCpIndex");
            //this.mRegion = (Region)info.GetValue("mRegion", typeof(Region));
            //this.gp = (GraphicsPath)info.GetValue("gp", typeof(GraphicsPath));
            this.anchor = (PointF)info.GetValue("anchor", typeof(PointF));
            this.isFill = info.GetBoolean("isFill");
            this.isSelect = info.GetBoolean("isSelect");
            this.isDraw = info.GetBoolean("isDraw");
            this.isMove = info.GetBoolean("isMove");
            this.isResize = info.GetBoolean("isResize");
            this.isRotate = info.GetBoolean("isRotate");
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("startPoint", this.startPoint, typeof(PointF));
            info.AddValue("endPoint", this.endPoint, typeof(PointF));
            info.AddValue("angle", this.angle, typeof(float));
            //info.AddValue("p", this.p, typeof(Pen));
            info.AddValue("pColor", this.p.Color, typeof(Color));
            info.AddValue("pDashStyle", this.p.DashStyle, typeof(DashStyle));
            info.AddValue("pWidth", this.p.Width, typeof(float));
            //info.AddValue("br", this.br, typeof(HatchBrush));
            info.AddValue("fillColor", this.fillColor, typeof(Color));
            info.AddValue("brStype", this.brStype);
            info.AddValue("listControlPoint", this.listControlPoint, typeof(List<RectangleF>));
            info.AddValue("objType", this.objType);
            info.AddValue("currentCp", this.currentCp, typeof(RectangleF));
            info.AddValue("currentCpIndex", this.currentCpIndex);
            //info.AddValue("mRegion", this.mRegion, typeof(Region));
            //info.AddValue("gp", this.gp, typeof(GraphicsPath));
            info.AddValue("anchor", this.anchor, typeof(PointF));
            info.AddValue("isFill", this.isFill);
            info.AddValue("isSelect", this.isSelect);
            info.AddValue("isDraw", this.isDraw);
            info.AddValue("isMove", this.isMove);
            info.AddValue("isResize", this.isResize);
            info.AddValue("isRotate", this.isRotate);
        }

        public virtual void Load()
        {

        }
    }
}
