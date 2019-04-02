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
    class MyText : MyRectangle
    {
        string text;
        TextBox tx;
        //string text;
        
        
        public MyText() : base() {
            text = "";
            tx = new TextBox();
            tx.Validated += new EventHandler(tbValidate);
            tx.Multiline = true;
            tx.Font = new Font("Time New Roman", 30);
            
            isFill = true;
            fillColor = Color.Black;
            
            
            
        }
        /* public MyText(MyText x):base(x)
         {
             text = x.text;
             tx = x.tx;
         }*/
        public override Shape Clone()
        {
            MyText x = new MyText();
            x.text = tx.Text;
            x.tx.Font = (Font)tx.Font.Clone();
            x.tx.Text = tx.Text;
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
        public override void Draw(Graphics g)
        { //phải xét lại để trong quá trình MouseMove
                
                //gp.AddRectangle(rec);
                
                float width = Math.Abs(endPoint.X - startPoint.X);
                float height = Math.Abs(endPoint.Y - startPoint.Y);

                float spX = Math.Min(startPoint.X, endPoint.X);
                float spY = Math.Min(startPoint.Y, endPoint.Y);
                RectangleF rec = new RectangleF(spX, spY, width, height);
            if (isDraw)
            {
                gp = new GraphicsPath();
                mRec = new GraphicsPath();
                mRec.AddRectangle(rec);
                
            }

            if (!isSelect)
            {
                tx.Visible = false;
                //gp.Reset();
                //gp.AddString(text, new FontFamily(tx.Font.Name), 0, tx.Font.Size+19, rec, StringFormat.GenericTypographic);
                
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                this.setBrush();
                g.DrawString(text, tx.Font, br, rec);
                //base.fillPath(g, gp);
                //g.DrawPath(p, gp);
            }
            

                if (this.isSelect)
                {
                    this.getListControlPoint();
                    this.showControlPoint(g);
                }
        }
        protected void tbValidate(Object sender, EventArgs e)
        {
            TextComplete(sender, e);
        }
        protected void TextComplete(Object sender, EventArgs e)
        {
            text = tx.Text;
            if (text != null)
                tx.Visible = false;
        }
        public override bool isContain(System.Drawing.Point location)
        {
            return mRec.IsVisible(location) || mRec.IsOutlineVisible(location, p);
        }
        //public void reRotate()
        //{
        //    if(angle!=0)
        //    { PointF center = CenterPoint(mRec.PathPoints[0], mRec.PathPoints[2]);



        //        Matrix temp = new Matrix();
        //        temp.RotateAt(angle, center);
        //        gp.Transform(temp);
        //        //angle = 0;
        //    }
        //    //startPoint = rotateP(mRec.PathPoints[0], center, -angle);
        //    //endPoint = rotateP(mRec.PathPoints[2], center, -angle);
        //}
        //public override void keyPress(char s)
        //{
        //    if (s == 8)
        //    {
        //        if (text.Length > 0)
        //            text = text.Remove(text.Length - 1);
        //    }
        //    else if ((s >= 32 && s <= 126)||s==13)
        //        text += s;
        //}
         public override void mouseDown(MouseEventArgs e)
        {
            
            if (this.isDraw && e.Button == MouseButtons.Left)
            {
                this.startPoint = e.Location;
                this.endPoint = e.Location;
                //this.endPoint.X += 1; //tránh TH tồn tại width=height=0
            }
            //move or resize
            else
            {
                if (this.isMove || this.isResize)
                {
                    this.anchor = e.Location;
                }
            }

        }
        public override void mouseUp(MouseEventArgs e, object sender)
        {
            if (isDraw)
            {
                this.endPoint = e.Location;
                //format2point();
                mRegion = new Region(mRec);

                

            }
            if (isSelect)
            {
                PictureBox t = (PictureBox)sender;
                t.Parent.Controls.Add(tx);

                float width = Math.Abs(endPoint.X - startPoint.X);
                float height = Math.Abs(endPoint.Y - startPoint.Y);

                float spX = Math.Min(startPoint.X, endPoint.X);
                float spY = Math.Min(startPoint.Y, endPoint.Y);

                tx.Location = new Point((int)spX, (int)spY);
                tx.Height = (int)height;
                tx.Width = (int)width;
                tx.BringToFront();
                tx.Show();
                tx.Focus();
            }
            //tx.Focus();
            //this.checkOrder();
            this.release(); //isMove = false & isDraw = false & isReszie=false
        }
        public override void setBrush()
        {
            br = new HatchBrush(HatchStyle.Cross, fillColor, fillColor);
        }
        public override void setFill(bool Type)
        {
        }
        public override void setColor(Color c)
        {
                fillColor = c;
                p.Color = c;
                tx.ForeColor = c;
        }
        public override void setFont(Font t)
        {
            tx.Font = t;
        }
        public override int objectType()
        {
            return 11;
        }
        public override void delete()
        {
            tx.Dispose();
        }
        //serialize & deserialize
        protected MyText(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
            this.text = info.GetString("text");
            //this.tx = (TextBox)info.GetValue("tx", typeof(TextBox));
            tx = new TextBox();
            tx.Text = text;
            tx.Validated += new EventHandler(tbValidate);
            tx.Multiline = true;
            tx.Font = (Font)info.GetValue("font", typeof(Font));

            this.isSelect = false;
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            base.GetObjectData(info, ctxt);
            info.AddValue("text", this.tx.Text,typeof(string));
            info.AddValue("font", this.tx.Font, typeof(Font));
           
            //info.AddValue("tx", this.tx, typeof(TextBox));
        }
        public override void Load()
        {
            float width = Math.Abs(endPoint.X - startPoint.X);
            float height = Math.Abs(endPoint.Y - startPoint.Y);

            float spX = Math.Min(startPoint.X, endPoint.X);
            float spY = Math.Min(startPoint.Y, endPoint.Y);
            RectangleF rec = new RectangleF(spX, spY, width, height);
            gp = new GraphicsPath();
            mRec = new GraphicsPath();
            mRec.AddRectangle(rec);

        }
    }
}
