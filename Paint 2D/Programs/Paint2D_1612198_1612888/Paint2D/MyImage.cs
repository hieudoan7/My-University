﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
namespace Paint2D
{
    class MyImage:MyRectangle
    {
            Bitmap bmp;
            public MyImage() : base() { }
            public override void Draw(System.Drawing.Graphics g)
            {

                    //phải xét lại để trong quá trình MouseMove
                    float width = bmp.Width;
                    float height = bmp.Height;
                    //gp = new GraphicsPath();
                    float spX = Math.Min(startPoint.X, endPoint.X);
                    float spY = Math.Min(startPoint.Y, endPoint.Y);
                    RectangleF rec = new RectangleF(spX, spY, width, height);
                    g.DrawImage(bmp, spX, spY);
                
                

                

                //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                //base.fillPath(g, gp);
                //g.DrawPath(p, gp);


                if (this.isSelect)
                {
                    this.getListControlPoint();
                    this.showControlPoint(g);
                }
            }
            public override void Scale(int Type, MouseEventArgs e)
            {
            }
            public override bool isContain(System.Drawing.Point location)
            {
                return mRec.IsVisible(location) || mRec.IsOutlineVisible(location, p);
            }
            
            public override void mouseDown(MouseEventArgs e)
            {
                if (this.isMove || this.isResize)
                {
                    this.anchor = e.Location;
                }

            }
            public override void mouseMove(MouseEventArgs e)
            {
                if (isMove && e.Button == MouseButtons.Left)
                {
                    float dX = e.X - anchor.X, dY = e.Y - anchor.Y;
                    Matrix x = new Matrix();
                    x.Translate(dX, dY);
                    mRec.Transform(x);
                    //gp.Transform(x);

                    startPoint.X += dX;
                    endPoint.X += dX;
                    startPoint.Y += dY;
                    endPoint.Y += dY;

                    anchor = e.Location;
                }
                //this.checkOrder();
            }
            public override void mouseUp(MouseEventArgs e, object sender)
            {
                this.release(); //isMove = false & isDraw = false & isReszie=false
            }
            public override void setBMP(Bitmap x)
            {
                bmp = x;
                startPoint = new PointF(0, 0);
                endPoint = new PointF(bmp.Width, bmp.Height);
                RectangleF rec = new RectangleF(startPoint.X, startPoint.Y, bmp.Width, bmp.Height);
            mRec = new GraphicsPath();
                mRec.AddRectangle(rec);
                isDraw = false;
            }
            public override Bitmap getBMP()
            {
                return bmp;
            }
            public override void getListControlPoint()
            {
                // counter-lock thu tu theo chieu kim dong ho
                listControlPoint.Clear();
                PointF[] diem = new PointF[9];
                if (mRec.PointCount == 0)
                {
                    for (int i = 0; i < 4; i++)
                        diem[2 * i] = new PointF(0, 0);
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
                for (int i = 0; i < 9; i++)
                {
                    this.listControlPoint.Add(new RectangleF(diem[i].X - 5, diem[i].Y - 5, 10, 10));

                }

            }

            public override void showControlPoint(Graphics g)
            {
                Pen p = new Pen(Color.Aqua, 1.0F);
                p.DashStyle = DashStyle.Dash;
                g.DrawPath(p, mRec);
            }
            public override int objectType()
            {
                return 13;
            }

        }
}
