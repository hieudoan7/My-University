using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paint2D
{
    public partial class Paint2D : Form
    {
        //Attribute of Form
        Shape curObj;
        Bitmap primaryBMP;
        Bitmap loadBMP;
        List<Shape> listObj;
        int objType;//1: Line, 2: Rectangle
        int mode;  //0: draw | 1: select | 2: move | 3: resize
        bool type;//0: outline,//1: fill;
        Color curColor = Color.Black;
        PointF loadLocation;
        Shape copyShape;
        //Convert object to binary 
        BinaryFormatter formatter;
        public Paint2D()
        {
            InitializeComponent();
            curObj = new Shape();
            //primaryBMP = new Bitmap(picBox.Width, picBox.Height,picBox.CreateGraphics()); //gan Graphics sinh ra tu picBox vs priamryBMP
            listObj = new List<Shape>();
            mode = 1; //mode default
            //cbStyle.SelectedValue = 0;
            //picBox.Height = 30;
            //copyShape = new Shape();
            copyShape = null;
            loadLocation = new PointF(0, 0);
            formatter = new BinaryFormatter();
        }

        private void Paint2D_Load(object sender, EventArgs e)
        {
            cbBrush.SelectedIndex = 2;

            cbStyle.SelectedIndex = 0;
            primaryBMP = new Bitmap(picBox.Width, picBox.Height, picBox.CreateGraphics()); //gan Graphics sinh ra tu picBox vs priamryBMP
        }
        //Paint Event
        private void picBox_paint(object sender, PaintEventArgs e)
        {
            Graphics g = Graphics.FromImage(primaryBMP);
            g.Clear(Color.White);
            if (loadBMP != null)
                g.DrawImage(loadBMP,loadLocation);
            foreach (Shape o in listObj)
            {
                o.Draw(g);
            }
            e.Graphics.DrawImage(primaryBMP, 0, 0); //picBox call thi Draw len no thoi
            //picBox.Image = primaryBMP;
        }
        //-----------------

        //Mouse Event

        private void btnCirArc_Click(object sender, EventArgs e)
        {

        }
        public void initObject(int objType)
        {

            if (curObj.objectType() == 12)
            {
                curObj.delete();
                this.listObj.Remove(curObj);

            }
            //if (curObj.objectType() == 4)
            //{ curObj = new Shape(); }
            switch (objType)
            {
                case 1:
                    curObj = new MyLine();
                    break;
                case 2:
                    curObj = new MyRectangle();
                    break;
                case 3:
                    curObj = new MyParallelogram();
                    break;
                case 4:
                    curObj = new MyPolyline();
                    break;
                case 5:
                    curObj = new MyEllipse();
                    break;
                case 6:
                    curObj = new MyCircle();
                    break;
                case 7:
                    curObj = new MyArc();
                    break;
                case 8:
                    curObj = new MyParabola();
                    break;
                case 9:
                    curObj = new MyHyperbola();
                    break;
                case 10:
                    curObj = new MyBezier();
                    break;
                case 11:
                    curObj = new MyText();
                    break;
                case 12:
                    curObj = new MySelect();
                    break;
                default:
                    curObj = null;
                    break;
            }
        }
        private void onMouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                if (mode == 0 && curObj.isDone()) //mode draw
                {
                    //if (objType == 4)
                      //  MessageBox.Show("dasds");
                    initObject(objType);
                    if (listObj.Count > 0) listObj[listObj.Count - 1].unSelect(); //bo chon thag truoc
                    curObj.setSelect(); //đang vẽ là đang chọn
                    //urColor = curObj.getColor();
                    //SetObjColor();
                    SetDashStype();
                    SetBrush();
                    SetOutLineSize();
                    
                    setCurColor(curObj.getColor());
                    if (objType != 4) listObj.Add(curObj);
                    else
                    {
                        if (curObj.isEmptyListLine())
                        {
                            listObj.Add(curObj);

                        }
                    }
                    curObj.isDraw = true;
                    //curObj.mouseDown(e);

                }
                //mode default
                if (mode == 1 /*&& !inProcess*/)
                {
                    /*List<Shape> dsTrung = new List<Shape>();
                    foreach (Shape o in listObj)
                    {
                        //Unselect all Object
                        o.unSelect();
                        if (o.isContain(e.Location))
                        {
                            curObj = o;
                            dsTrung.Add(o);
                        
                    }
                    if (dsTrung.Count > 1)
                    {
                        foreach (Shape o in dsTrung)
                        {
                            if (Math.Abs((o.startPoint.X - e.Location.X) * (o.startPoint.X - e.Location.X)) +
                                Math.Abs((o.startPoint.Y - e.Location.Y) * (o.startPoint.Y - e.Location.Y)) <
                                Math.Abs((curObj.startPoint.X - e.Location.X) * (curObj.startPoint.X - e.Location.X)) +
                                Math.Abs((curObj.startPoint.Y - e.Location.Y) * (curObj.startPoint.Y - e.Location.Y)))
                            {
                                curObj = o;
                            }
                        }
                    }*/
                    bool x = false;
                    for (int i = listObj.Count - 1; i > -1; i--)
                    {
                         listObj[i].unSelect();
                        
                        if (!x)
                        {
                            if (listObj[i].isContain(e.Location))
                            {
                                curObj = listObj[i];
                                x = true;
                            }
                        }
                    }
                    if(listObj.Count>0)
                    if (listObj[listObj.Count-1].objectType() == 13&&listObj[listObj.Count-1]!=curObj)
                    {
                        loadBMP = listObj[listObj.Count-1].getBMP();
                            loadLocation = listObj[listObj.Count - 1].startPoint;
                        listObj.Remove(listObj[listObj.Count-1]);
                    }
                    curObj.setSelect();
                    setCurColor(curObj.getColor());//nho o form
                                                   //lam luc mouseDown 1 lan thoi
                    for (int i = 0; i < curObj.listControlPoint.Count; i++)
                    {
                        if (curObj.listControlPoint[i].Contains(e.Location))
                        {
                            if (i == 8)
                                curObj.isRotate = true;
                            else
                                curObj.isResize = true;
                            //inProcess = true;
                            //MessageBox.Show("hello");
                            curObj.currentCp = curObj.listControlPoint[i];
                            curObj.currentCpIndex = i;
                            //curObj.mouseDown(e);
                        }
                    }
                    if (curObj.isContain(e.Location) && !curObj.isResize)
                    {
                        curObj.isMove = true;
                        //curObj.mouseDown(e);
                        //inProcess = true;
                    }
                } //end TH mode =1

                curObj.mouseDown(e);
                picBox.Refresh();
            } //end if click left mouse
        }


        //Update currentCp trong lúc onMouseMove
        //public void updateCurrentCp(MouseEventArgs e)
        //{

        //    curObj.currentCp = curObj.listControlPoint[curObj.currentCpIndex];
        //}
        private void onMouseMove(Object sender, MouseEventArgs e)
        {
            label1.Text = e.Location.X.ToString() + ", " + e.Location.Y.ToString();
            //chinh chuot cho dep thoi
            //Cursor = Cursors.Default;

            if (mode == 1 && curObj.isSelect == true)
            {
                if (curObj.HitTest(e.Location) == 9)
                    Cursor = Cursors.NoMove2D;
                else if (curObj.HitTest(e.Location) > 0)
                    Cursor = Cursors.Cross;
                else if (curObj.HitTest(e.Location) == 0)
                    Cursor = Cursors.SizeAll;
                else
                    Cursor = Cursors.Default;
            }
            else { Cursor = Cursors.Default; }
            //
            if ((objType == 4 || objType == 10) && e.Button == MouseButtons.None)
            {
                curObj.mouseMove(e);
                picBox.Refresh();
            }
            else if (e.Button==MouseButtons.Left)
            {
                //curObj.currentCp = e.Location;
                //if (curObj.isResize)
                //{
                //    updateCurrentCp(e);
                //}
                curObj.mouseMove(e);
                //picBox.Refresh();
            }
            
            picBox.Refresh();

        }

        private void onMouseUp(Object sender, MouseEventArgs e)
        {
            curObj.mouseUp(e, sender);
            //inProcess = false;
            //curObj.isMove = false;
            //curObj.isDraw = false;
            picBox.Refresh();
        }

        private void btnLine_Click(object sender, EventArgs e)
        {
            objType = 1;
            mode = 0;
        }

        private void btnRec_Click(object sender, EventArgs e)
        {
            objType = 2;
            mode = 0;
        }

        private void btnPara_Click(object sender, EventArgs e)
        {
            objType = 3;
            mode = 0;
        }

        private void btnPoGon_Click(object sender, EventArgs e)
        {
            objType = 4;
            //curObj = new MyPolyline();
            mode = 0;
        }

        private void onMouseHover(object sender, EventArgs e)
        {

        }

        private void onMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (objType == 4)
            {
                curObj.mouseDoubleClick(mode, e);
                //mode = 2;
            }
            picBox.Refresh();
        }

        private void btnEllipse_Click(object sender, EventArgs e)
        {
            objType = 5;
            mode = 0;
        }

        private void btnCircle_Click(object sender, EventArgs e)
        {
            objType = 6;
            mode = 0;
        }

        private void btnEllArc_Click(object sender, EventArgs e)
        {
            objType = 7;
            mode = 0;
        }

        private void btnParabol_Click(object sender, EventArgs e)
        {
            objType = 8;
            mode = 0;
        }

        private void btnHype_Click(object sender, EventArgs e)
        {
            objType = 9;
            mode = 0;
        }

        private void btnCursor_Click(object sender, EventArgs e)
        {
            mode = 1;
        }

        private void btnBezier_Click(object sender, EventArgs e)
        {
            objType = 10;
            mode = 0;
        }
        private void setCurColor(Color c)
        {
            btnCurColor.ForeColor = c;
            btnCurColor.BackColor = c;
        }
        private void setObjColor(Color c)
        {
            if (primaryBMP != null)
            {
                setCurColor(c);
                curObj.setColor(c);
                picBox.Refresh();
            }
        }
        private void btnBlack_Click(object sender, EventArgs e)
        {
            setObjColor(Color.Black);
        }

        private void btnSilver_Click(object sender, EventArgs e)
        {
            setObjColor(Color.Silver);
        }

        private void btnWhite_Click(object sender, EventArgs e)
        {
            setObjColor(Color.White);
        }

        private void btnRed_Click(object sender, EventArgs e)
        {
            setObjColor(Color.Red);
        }

        private void btnMaroon_Click(object sender, EventArgs e)
        {
            setObjColor(Color.Maroon);
        }

        private void btnOrange_Click(object sender, EventArgs e)
        {
            setObjColor(Color.Orange);
        }

        private void btnYellow_Click(object sender, EventArgs e)
        {
            setObjColor(Color.Yellow);
        }

        private void btnGreen_Click(object sender, EventArgs e)
        {
            setObjColor(Color.Green);
        }

        private void btnCyan_Click(object sender, EventArgs e)
        {
            setObjColor(Color.Cyan);
        }

        private void btnBlue_Click(object sender, EventArgs e)
        {
            setObjColor(Color.Blue);
        }

        private void btnPurple_Click(object sender, EventArgs e)
        {
            setObjColor(Color.Purple);
        }

        private void btnPink_Click(object sender, EventArgs e)
        {
            setObjColor(Color.Pink);
        }
        private void SetDashStype()
        {
            int chose = cbStyle.SelectedIndex;
            System.Drawing.Drawing2D.DashStyle x = System.Drawing.Drawing2D.DashStyle.Solid;

            switch (chose)
            {
                case 0:
                    x = System.Drawing.Drawing2D.DashStyle.Solid;
                    break;
                case 1:
                    x = System.Drawing.Drawing2D.DashStyle.Dot;
                    break;
                case 2:
                    x = System.Drawing.Drawing2D.DashStyle.Dash;
                    break;
                case 3:
                    x = System.Drawing.Drawing2D.DashStyle.DashDot;
                    break;
                case 4:
                    x = System.Drawing.Drawing2D.DashStyle.DashDotDot;
                    break;
                case 5:
                    x = System.Drawing.Drawing2D.DashStyle.Solid;
                    break;
            }
            curObj.setDash(x);
        }
        private void setDashStype(object sender, EventArgs e)
        {
            if (primaryBMP != null)
            {
                SetDashStype();
                picBox.Refresh();
            }

        }
        private void SetOutLineSize()
        { curObj.setPenSize(tbSize.Value / 10.0F); }
        private void setOutLineSize(object sender, EventArgs e)
        {
            SetOutLineSize();
            picBox.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.type = false;
            curObj.setFill(false);
            picBox.Refresh();

        }

        private void btnFill_Click(object sender, EventArgs e)
        {
            type = true;
            curObj.setFill(true);
            picBox.Refresh();
        }

        private void SetBrush()
        {
            curObj.setBrushStype(cbBrush.SelectedIndex);
        }
        private void setBrush(object sender, EventArgs e)
        {
            if (primaryBMP != null)
            {
                SetBrush();
                picBox.Refresh();
            }
        }

        private void btnText_Click(object sender, EventArgs e)
        {
            mode = 0;
            objType = 11;

            //picBox.SendToBack();
        }


        private void btnColDia_Click(object sender, EventArgs e)
        {
            ColorDialog t = new ColorDialog();
            if (t.ShowDialog() == DialogResult.OK)
            {
                setObjColor(t.Color);
            }
            picBox.Refresh();
        }

        private void btnFont_Click(object sender, EventArgs e)
        {
            FontDialog t = new FontDialog();
            if (t.ShowDialog() == DialogResult.OK)
            {
                if (curObj.objectType() == 11)
                {
                    curObj.setFont(t.Font);
                }
            }
            picBox.Refresh();
        }

        private void Key_Down(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (curObj.objectType() == 12)
                {

                }
                else
                {
                    curObj.delete();
                    this.listObj.Remove(curObj);
                    curObj = new Shape();
                }
            }
            picBox.Refresh();
        }

        private void btnSendBack_Click(object sender, EventArgs e)
        {

            cMSendBack.Show(btnSendBack.Location.X, btnSendBack.Location.Y + 90);

        }

        private void btnBringFront_Click(object sender, EventArgs e)
        {
            cMBringFront.Show(btnBringFront.Location.X, btnBringFront.Location.Y + 90);
        }

        private void sendBackwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i;
            for (i = listObj.Count - 1; i > -1; i--)
            {
                if (curObj == listObj[i])
                    break;
            }
            if (i > 0)
            {
                listObj[i] = listObj[i - 1];
                listObj[i - 1] = curObj;

            }
            picBox.Refresh();
        }

        private void sendToBackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i;
            for (i = listObj.Count - 1; i > -1; i--)
            {
                if (curObj == listObj[i])
                    break;
            }
            for (; i > 0; i--)
            {
                listObj[i] = listObj[i - 1];

            }
            listObj[0] = curObj;
            picBox.Refresh();
        }

        private void bringForwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i;
            for (i = 0; i < listObj.Count; i++)
            {
                if (curObj == listObj[i])
                    break;
            }
            if (i < listObj.Count - 1)
            {
                listObj[i] = listObj[i + 1];
                listObj[i + 1] = curObj;

            }
            picBox.Refresh();
        }

        private void bringToFrontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i;
            for (i = 0; i < listObj.Count; i++)
            {
                if (curObj == listObj[i])
                    break;
            }
            for (; i < listObj.Count - 1; i++)
            {
                listObj[i] = listObj[i + 1];

            }
            listObj[listObj.Count - 1] = curObj;
            picBox.Refresh();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog t = new SaveFileDialog();
            t.Filter = "Binary File|*.dat|Bitmap Image|*.bmp|Gif Image|*.gif|JPeg Image|*.jpg|PNG Image|*.png|TIFF Image|*.tif";
            t.Title = "Save an Image File";
            if (t.ShowDialog() == DialogResult.OK)
            {
                if (t.FileName != "")
                {
                    if (t.FilterIndex == 1)
                        SaveBinary(ref t);
                    else
                    {
                        System.IO.FileStream fs = (System.IO.FileStream)t.OpenFile();
                        // Saves the Image in the appropriate ImageFormat based upon the  
                        // File type selected in the dialog box.  
                        // NOTE that the FilterIndex property is one-based.  
                        switch (t.FilterIndex)
                        {
                            //case 1:
                            // SaveBinary(ref t);

                            //  break;
                            case 2:
                                primaryBMP.Save(fs,
                                   System.Drawing.Imaging.ImageFormat.Bmp);
                                break;

                            case 3:
                                primaryBMP.Save(fs,
                                   System.Drawing.Imaging.ImageFormat.Gif);
                                break;
                            case 4:
                                primaryBMP.Save(fs,
                                   System.Drawing.Imaging.ImageFormat.Jpeg);
                                break;
                            case 5:
                                primaryBMP.Save(fs,
                                   System.Drawing.Imaging.ImageFormat.Png);
                                break;
                            case 6:
                                primaryBMP.Save(fs,
                                   System.Drawing.Imaging.ImageFormat.Tiff);
                                break;
                        }
                        fs.Close();
                    }
                }
            }

        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (curObj.objectType() == 12)
            {
                float width = Math.Abs(curObj.endPoint.X - curObj.startPoint.X);
                float height = Math.Abs(curObj.endPoint.Y - curObj.startPoint.Y);

                float spX = Math.Min(curObj.startPoint.X, curObj.endPoint.X);
                float spY = Math.Min(curObj.startPoint.Y, curObj.endPoint.Y);
                RectangleF rec = new RectangleF(spX + 1, spY + 1, width - 1, height - 1);
                Bitmap cropped = primaryBMP.Clone(rec, primaryBMP.PixelFormat);
                Clipboard.SetImage(cropped);
                curObj.delete();
                this.listObj.Remove(curObj);
                curObj = new Shape();

            }
            else
            {
                if (curObj.objectType() != 1 && curObj.objectType() != 10 && curObj.objectType() != 4)
                {
                    float width = Math.Abs(curObj.endPoint.X - curObj.startPoint.X);
                    float height = Math.Abs(curObj.endPoint.Y - curObj.startPoint.Y);

                    float spX = Math.Min(curObj.startPoint.X, curObj.endPoint.X);
                    float spY = Math.Min(curObj.startPoint.Y, curObj.endPoint.Y);
                    RectangleF rec = new RectangleF(spX + 1, spY + 1, width - 1, height - 1);
                    Bitmap cropped = primaryBMP.Clone(rec, primaryBMP.PixelFormat);
                    Clipboard.SetImage(cropped);
                }
                copyShape = curObj.Clone();
            }
        }

        private void btnCrop_Click(object sender, EventArgs e)
        {
            objType = 12;
            mode = 0;
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog t = new OpenFileDialog();

            t.Filter = "Binary File|*.dat|Bitmap Image|*.bmp|Gif Image|*.gif|JPeg Image|*.jpg|PNG Image|*.png|TIFF Image|*.tif";
            t.Title = "Open an Image File";
            if (t.ShowDialog() == DialogResult.OK)
            {
                if (t.FileName != "")
                {
                    if (t.FilterIndex == 1)
                        OpenBinary(ref t);
                    else
                        loadBMP = new Bitmap(t.FileName);
                }
                picBox.Refresh();
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (copyShape == null)
            {
                if (Clipboard.ContainsImage())
                {
                    MyImage t = new MyImage();
                    t.setBMP(new Bitmap(Clipboard.GetImage()));
                    listObj.Add(t);
                }

            }
            else
            {
                listObj.Add(copyShape);
                curObj.unSelect();
                curObj = copyShape;
                curObj.setSelect();
                copyShape = null;
            }
            picBox.Refresh();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("PAINT 2D PROJECT\n\nAuthor: Doan Minh Hieu - Phan Minh Son.\n\n@2018\nUniversity of Sciences, Ho Chi Minh.");
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (curObj.objectType() < 12)
            {
                copyToolStripMenuItem_Click(sender, e);
                curObj.delete();
                this.listObj.Remove(curObj);
                curObj = new Shape();
                picBox.Refresh();
            }
        }

        private void binarySaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog t = new SaveFileDialog();
            t.Title = "Save an Image File";
            t.ShowDialog();
            formatter = new BinaryFormatter();
            Stream stream = new FileStream(t.FileName, FileMode.Create, FileAccess.Write);
            foreach (Shape o in listObj)
            {
                formatter.Serialize(stream, o);
            }
            stream.Close();
        }

        private void binaryLoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog t = new OpenFileDialog();

            t.Filter = "Binary File|*.dat";
            t.Title = "Open an Image File";
            t.ShowDialog();
            Stream readStream = new FileStream(t.FileName, FileMode.Open, FileAccess.Read);
            listObj.Clear();
            while (readStream.Position < readStream.Length)
            {
                curObj = (Shape)formatter.Deserialize(readStream);
                //curObj.p = new Pen(Color.Black, 1.0F);
                //curObj.p = new Pen(curObj.pColor, curObj.pWidth);
                //curObj.setColor(curObj.pColor);
                //curObj.setPenSize(curObj.pWidth);
                //curObj.gp = new System.Drawing.Drawing2D.GraphicsPath();
                curObj.Load();
                //curObj.isDraw = true;
                listObj.Add(curObj);
            }

            picBox.Refresh();
            //normalize
            foreach (Shape o in listObj)
            {
                o.isDraw = false;
            }
            mode = 1;
            curObj = listObj[listObj.Count - 1];
            readStream.Close();
        }

        private void SaveBinary(ref SaveFileDialog t)
        {
            formatter = new BinaryFormatter();
            Stream stream = new FileStream(t.FileName, FileMode.Create, FileAccess.Write);
            foreach (Shape o in listObj)
            {
                formatter.Serialize(stream, o);
            }
            stream.Close();
        }

        private void OpenBinary(ref OpenFileDialog t)
        {
            Stream readStream = new FileStream(t.FileName, FileMode.Open, FileAccess.Read);
            listObj.Clear();
            while (readStream.Position < readStream.Length)
            {
                curObj = (Shape)formatter.Deserialize(readStream);
                //curObj.p = new Pen(Color.Black, 1.0F);
                //curObj.p = new Pen(curObj.pColor, curObj.pWidth);
                //curObj.setColor(curObj.pColor);
                //curObj.setPenSize(curObj.pWidth);
                //curObj.gp = new System.Drawing.Drawing2D.GraphicsPath();
                curObj.Load();
                //curObj.isDraw = true;
                listObj.Add(curObj);
            }

            picBox.Refresh();
            //normalize
            foreach (Shape o in listObj)
            {
                o.isDraw = false;
            }
            mode = 1;
            curObj = listObj[listObj.Count - 1];
            readStream.Close();
        }
    }
}
