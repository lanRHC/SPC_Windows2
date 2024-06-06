using HalconDotNet;
using Newtonsoft.Json;
using NPOI.Util;
using SPC_Windows.SPCFile;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Vision2.vision
{
    /// <summary>
    /// 运算
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// 等于
        /// </summary>
        等于 = 0,
        /// <summary>
        /// 不等于
        /// </summary>
        不等于 = 1,
        /// <summary>
        /// 大于
        /// </summary>
        大于 = 2,
        /// <summary>
        /// 大于等于
        /// </summary>
        大于等于 = 3,
        /// <summary>
        /// 小于
        /// </summary>
        小于 = 4,

        /// <summary>
        /// 小于等于
        /// </summary>
        小于等于 = 5,
    }
    /// <summary>
    /// 通道
    /// </summary>
    public enum ImageTypeObj
    {
        Image3 = 0,
        Gray = 1,
        R = 2,
        G = 3,
        B = 4,
        H = 5,
        S = 6,
        V = 7,
    }

    /// <summary>
    /// 颜色
    /// </summary>
    public enum ColorResult
    {
        red = 1,
        yellow = 2,
        green = 0,
        blue = 3,
        black = 4,
        white = 5,
        gray = 6,
        cyan = 7,
        magenta = 8,
        coral = 9,
        pink = 10,
        goldenrod = 11,
        orange = 12,
        gold = 13,
        navy = 14,
        turquoise = 15,
        khaki = 16,
        violet = 17,
        firebrick = 18,
    }

    /// <summary>
    /// 位置
    /// </summary>
    public class XYZPoint3D
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

    }
    public class XYZPoint2D
    {
        public XYZPoint2D(double x,double y)
        {
            X = x;
            Y = y;
        }
        public double X { get; set; }
        public double Y { get; set; }

    }     /// <summary>
          /// 区域颜色
          /// </summary>
    public class ObjectColor
    {
        public ObjectColor()
        {
            HobjectColot = "red";
            _HObject = new HObject();
            HOperatorSet.GenEmptyObj(out _HObject);
        }

        public ObjectColor(HObject hObject, string color)
        {
            HobjectColot = color;
            _HObject = hObject;
        }
        public ObjectColor(HObject hObject, HTuple color = null)
        {
            _HObject = hObject;
            if (color == null)
            {
                HobjectColot = "green";
            }
            else
            {
                HobjectColot = color;
            }
        }

        /// <summary>
        /// 结果区域
        /// </summary>
        public HObject _HObject;

        /// <summary>
        /// 区域颜色
        /// </summary>
        public HTuple HobjectColot
        {
            get
            {
                if (Colot == null)
                {
                    Colot = "red";
                }
                if (Colot == "null")
                {
                    Colot = "red";
                }
                return Colot;
            }

            set { Colot = value; }
        }

        private HTuple Colot;

        public void Dispose()
        {
            HobjectColot = null;
            if (_HObject != null)
            {
                _HObject.Dispose();
            }
        }
    }
    /// <summary>
    /// 缺陷加文本字符显示
    /// </summary>
    public class ObjectTextColor : ObjectColor
    {
        public bool IsShow { get; set; } = true;
        public List<HObject> ListOBJs = new List<HObject>();
        public List<string> Listlabel_Name { get; set; } = new List<string>();
        public MassageText MassageText { get; set; } = new MassageText();
    }



    /// <summary>
    /// 键值的Object
    /// </summary>
    public class DicHObjectColot
    {
        public Dictionary<string, ObjectColor> DirectoryHObject = new Dictionary<string, ObjectColor>();
        public string Name { get; set; }

        public HObject this[string index]
        {
            get
            {
                if (DirectoryHObject.ContainsKey(index))
                {
                    return DirectoryHObject[index]._HObject;
                }
                else
                {
                    return new HObject();
                }
            }
            set
            {
                if (DirectoryHObject.ContainsKey(index))
                {
                    DirectoryHObject[index]._HObject = value;
                }
                else
                {
                    DirectoryHObject.Add(index, new ObjectColor() { _HObject = value });
                }
            }
        }

        public HObject ShowObj(int hWindowHalconID)
        {
            HObject hObjects = new HObject();
            hObjects.GenEmptyObj();
            foreach (var item in DirectoryHObject)
            {
                try
                {
                    HOperatorSet.SetColor(hWindowHalconID, item.Value.HobjectColot);
                }
                catch (Exception)
                {
                    HOperatorSet.SetColor(hWindowHalconID, "red");
                }
                try
                {
                    HOperatorSet.DispObj(item.Value._HObject, hWindowHalconID);
                }
                catch { }
                hObjects = hObjects.ConcatObj(item.Value._HObject);
            }
            return hObjects;
        }

        public HObject ShowObj(HWindow hWindowHalconID)
        {
            HObject hObjects = new HObject();
            hObjects.GenEmptyObj();
            foreach (var item in DirectoryHObject)
            {
                try
                {
                    HOperatorSet.SetColor(hWindowHalconID, item.Value.HobjectColot);
                }
                catch (Exception)
                {
                    HOperatorSet.SetColor(hWindowHalconID, "red");
                }
                try
                {
                    HOperatorSet.DispObj(item.Value._HObject, hWindowHalconID);
                }
                catch { }
                hObjects = hObjects.ConcatObj(item.Value._HObject);
            }
            return hObjects;
        }

        public HObject ShowObj(string naem, int hWindowHalconID)
        {
            try
            {
                if (DirectoryHObject.ContainsKey(naem))
                {
                    HOperatorSet.SetColor(hWindowHalconID, DirectoryHObject[naem].HobjectColot);
                    HOperatorSet.DispObj(DirectoryHObject[naem]._HObject, hWindowHalconID);
                    return DirectoryHObject[naem]._HObject;
                }
                else
                {
                    HWindID.Disp_message(hWindowHalconID, naem + "不存在!", 20, 2120, true);
                    return new HObject();
                }
            }
            catch { }
            return new HObject();
        }

        public HObject ShowObj(string naem, HWindow hWindowHalconID)
        {
            try
            {
                if (DirectoryHObject.ContainsKey(naem))
                {
                    HOperatorSet.SetColor(hWindowHalconID, DirectoryHObject[naem].HobjectColot);
                    HOperatorSet.DispObj(DirectoryHObject[naem]._HObject, hWindowHalconID);
                    return DirectoryHObject[naem]._HObject;
                }
                else
                {
                    HWindID. Disp_message(hWindowHalconID, naem + "不存在!", 20, 2120, true);
                    return new HObject();
                }
            }
            catch { }
            return new HObject();
        }

        public void Clear()
        {
            DirectoryHObject.Clear();
        }

        public void RemoeNull()
        {
            foreach (var item in DirectoryHObject)
            {
                if (item.Value._HObject == null)
                {
                    item.Value._HObject = new HObject();
                    item.Value._HObject.GenEmptyObj();
                }
                if (!item.Value._HObject.IsInitialized())
                {
                    item.Value._HObject.GenEmptyObj();
                }
            }
        }



        private void Remove(string name)
        {
            if (DirectoryHObject.ContainsKey(name))
            {
                DirectoryHObject[name]._HObject.Dispose();
                DirectoryHObject.Remove(name);
            }
        }

        public void MouseButtonsLeft(ContextMenuStrip contextMenuStrip, TreeView treeView)
        {
        }
    }



public class HWindID 
    {
        public HWindID()
        {
            //hObject = new HObject();
        }

        public static int LineWidth=1;
        public static string Font = "";
        public static int FontSize = 20;

        public static bool Bold { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="halcon3DMo"></param>
        /// <returns></returns>
        public  Halcon3DModel GetObjectModel3D(Halcon3DModel halcon3DMo=null)
        {
            if (halcon3DMo!=null)
            {
                halcon3DModel = halcon3DMo;
            }
            return halcon3DModel;
        }
        private Halcon3DModel halcon3DModel;
        public HSmartWindowControl GetHSmartWindowControl(HSmartWindowControl hSmartWindowControl = null, HWindID hWindID = null)
        {
            if (hSmartWindowControl != null)
            {
                hSmwindowControl = hSmartWindowControl;
            }
            return hSmwindowControl;
        }
        public HWindowControl GetHWindowControl(HWindowControl hWindowCo = null)
        {
            if (hWindowCo != null)
            {
                hwindowControl = hWindowCo;
            }
            return hwindowControl;
        }


        public static void DispImage(HTuple hv_WindowHandle, HObject iamge)
        {
            try
            {
                if (iamge.IsInitialized() || iamge.CountObj() >= 1)
                {
                    HOperatorSet.GetImageSize(iamge, out HTuple width, out HTuple height);
                    HWindID.SetImagePart(hv_WindowHandle, height, width);
                    //HOperatorSet.SetPart(hv_WindowHandle, 0, 0, height, width);
                    HOperatorSet.DispImage(iamge, hv_WindowHandle);
                }
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwindowControl"></param>
        /// <param name="imgHeight"></param>
        /// <param name="imgWidth"></param>
        public static void SetImagePart(HTuple hwindowControl, HTuple imgHeight, HTuple imgWidth)
        {
            try
            {
                if (imgWidth.Length==0)
                {
                    return;

                }
                HTuple HeigthImage = imgHeight;
                HTuple WidthImage = imgWidth;
                HOperatorSet.GetWindowExtents(hwindowControl, out HTuple row1, out HTuple row2, out HTuple width, out HTuple heiget);
                //           hwindowControl.GetWindowExtents(out int row1, out int row2, out int width, out int heiget);
                double scaleWidth = imgWidth[0].D / width;
                double scaleHeight = imgHeight[0].D / heiget;
                HTuple Row0_1;
                HTuple Row1_1;
                HTuple Col0_1;
                HTuple Col1_1;
                if (scaleWidth >= scaleHeight)
                {
                    Row0_1 = -(1.0) * ((heiget * scaleWidth) - imgHeight) / 2;
                    Col0_1 = 0;
                    Row1_1 = Row0_1 + heiget * scaleWidth;
                    Col1_1 = Col0_1 + width * scaleWidth;
                }
                else
                {
                    Row0_1 = 0;
                    Col0_1 = -(1.0) * ((width * scaleHeight) - imgWidth) / 2;
                    Row1_1 = Row0_1 + heiget * scaleHeight;
                    Col1_1 = Col0_1 + width * scaleHeight;
                }
                HOperatorSet.SetPart(hwindowControl, Row0_1, Col0_1, Row1_1, Col1_1);
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="imgHeight"></param>
        /// <param name="imgWidth"></param>
        public void SetImagePart(HTuple imgHeight, HTuple imgWidth)
        {
            if (imgHeight.Length==0)
            {
                return;
            }
            HeigthImage = imgHeight;
            WidthImage = imgWidth;
            if (hwindowControl.Width == 0 || hwindowControl.Height == 0)
            {
                return;
            }
            double scaleWidth = imgWidth[0].D / hwindowControl.Width;
            double scaleHeight = imgHeight[0].D / hwindowControl.Height;
            if (scaleWidth >= scaleHeight)
            {
                Row0_1 = -(1.0) * ((hwindowControl.Height * scaleWidth) - imgHeight) / 2;
                Col0_1 = 0;
                Row1_1 = Row0_1 + hwindowControl.Height * scaleWidth;
                Col1_1 = Col0_1 + hwindowControl.Width * scaleWidth;
            }
            else
            {
                Row0_1 = 0;
                Col0_1 = -(1.0) * ((hwindowControl.Width * scaleHeight) - imgWidth) / 2;
                Row1_1 = Row0_1 + hwindowControl.Height * scaleHeight;
                Col1_1 = Col0_1 + hwindowControl.Width * scaleHeight;
            }
            HOperatorSet.SetPart(hwindowControl.HalconWindow, Row0_1, Col0_1, Row1_1, Col1_1);
        }
        private HTuple Row0_1, Col0_1, Row1_1, Col1_1;

        private HTuple m_ImageRow1, m_ImageCol1;
        private double stratX;
        private double stratY;
        private HTuple H_Scale = 0.2; //缩放步长
        private HTuple MaxScale = 10000;//最大放大系数
        private HTuple ptX, ptY;
        private HTuple m_ImageRow0, m_ImageCol0;
        private HTuple hv_Button;

        private bool meuseBool;
        /// <summary>
        /// 放大移动图像标志，
        /// </summary>
        public bool WhidowAdd;

        public int Width { get; set; }

        public int Height { get; set; }
     
        /// <summary>
        /// 宽高比例
        /// </summary>
        public double H_W_Size = 1;

        //public WDDockStyleEnum wDDockStyleEnum;
        /// <summary>
        /// 
        /// </summary>
        public double WidthImage = 2000;
        /// <summary>
        /// 
        /// </summary>
        public double HeigthImage = 2000;



        public HWindow GetHWindow()
        {
            return hWindow;
        }

        //HWindowControl hWindowControl1;
        private HWindow hWindow;


        public void Initialize(HWindowControl hSmartWindowCont)
        {
            hWindow = hSmartWindowCont.HalconWindow;
            hwindowControl = hSmartWindowCont;
            try
            {
                hwindowControl.HalconWindow.SetWindowParam("graphics_stack_max_element_num", 50000);
                HOperatorSet.SetDraw(hWindow, "margin");
                //HOperatorSet.SetLineWidth(hWindow, Vision.Instance.LineWidth);
                //HOperatorSet.QueryFont(hWindow, out HTuple font);
                //Vision.SetFont(hWindow);
            }
            catch (Exception)
            { }
            hwindowControl.HMouseUp += hWindowControl1_HMouseUp;
            hwindowControl.HMouseDown += hWindowControl1_MouseDown;
            hwindowControl.HMouseWheel += hWindowControl2_HMouseWheel;
            hwindowControl.HMouseMove += hWindowControl4_HMouseMove;
            hwindowControl.KeyDown += HWindowControl1_KeyDown;
            hwindowControl.KeyUp += HWindowControl1_KeyUp;
            hwindowControl.Resize += HSmartWindowControl_Resize;
        }

        public void Initialize(HSmartWindowControl hSmartWindowCont)
        {
            hWindow = hSmartWindowCont.HalconWindow;
            hSmwindowControl = hSmartWindowCont;
            try
            {

                hSmwindowControl.HalconWindow.SetWindowParam("graphics_stack_max_element_num", 60000);
                hSmwindowControl.MouseWheel += my_MouseWheel;
                hSmwindowControl.HDoubleClickToFitContent = true;
                //ContextMenuStrip.Items.Add(new ToolStripMenuItem() { Name="保存图像",Text= "保存图像" });
                //hSmwindowControl.Resize += HSmartWindowControl_Resize;
                //
                //hSmwindowControl.HMouseUp += hWindowControl1_HMouseUp;
                hSmwindowControl.PreviewKeyDown += HSmwindowControl_PreviewKeyDown;
                hSmwindowControl.KeyUp += HSmwindowControl_KeyUp; ;
                hSmwindowControl.HMouseDown += hSWindowControl1_MouseDown;
                //hSmwindowControl.HMouseMove += hWindowControl4_HMouseMove;

                HOperatorSet.SetDraw(hWindow, "margin");
                HOperatorSet.SetLineWidth(hWindow, LineWidth);
                //HOperatorSet.QueryFont(hWindow, out HTuple font);
                //Vision.SetFont(hWindow);
            }
            catch (Exception)
            { }
        }
        ContextMenuStrip ContextMenuStrip = new ContextMenuStrip();

        private void HSmwindowControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                //WhidowAdd = false;
                hSmwindowControl.ContextMenuStrip = null;
                this.ShowObj();
            }
        }

        public static void AddMouseWheelZoon(HSmartWindowControl hSmartWindowCont)
        {
            hSmartWindowCont.MouseWheel += my_MouseWheel;
            void my_MouseWheel(object sender, MouseEventArgs e)
            {
                try
                {
                    //System.Drawing.Point pt = this.Location;
                    System.Drawing.Point pt = hSmartWindowCont.Location;
                    int leftBorder = hSmartWindowCont.Location.X;
                    int rightBorder = hSmartWindowCont.Location.X + hSmartWindowCont.Size.Width;
                    int topBorder = hSmartWindowCont.Location.Y;
                    int bottomBorder = hSmartWindowCont.Location.Y + hSmartWindowCont.Size.Height;
                    if (e.X > leftBorder && e.X < rightBorder && e.Y > topBorder && e.Y < bottomBorder)
                    {
                    }
                    MouseEventArgs newe = new MouseEventArgs(e.Button, e.Clicks, e.X - pt.X, e.Y - pt.Y, e.Delta);
                    hSmartWindowCont.HSmartWindowControl_MouseWheel(sender, newe);
                }
                catch (Exception)
                {
                }
            }
        }

        private void HSmwindowControl_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            try
            {
                if (e.Control)
                {

                    this.ShowImage();
                    hSmwindowControl.ContextMenuStrip = ContextMenuStrip;

                }


            }
            catch (Exception)
            {
            }
        }

        private void my_MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                //System.Drawing.Point pt = this.Location;
                System.Drawing.Point pt = hSmwindowControl.Location;
                int leftBorder = hSmwindowControl.Location.X;
                int rightBorder = hSmwindowControl.Location.X + hSmwindowControl.Size.Width;
                int topBorder = hSmwindowControl.Location.Y;
                int bottomBorder = hSmwindowControl.Location.Y + hSmwindowControl.Size.Height;
                if (e.X > leftBorder && e.X < rightBorder && e.Y > topBorder && e.Y < bottomBorder)
                {
                }
                MouseEventArgs newe = new MouseEventArgs(e.Button, e.Clicks, e.X - pt.X, e.Y - pt.Y, e.Delta);
                hSmwindowControl.HSmartWindowControl_MouseWheel(sender, newe);
            }
            catch (Exception)
            {
            }
        }
        private void HSmartWindowControl_Resize(object sender, EventArgs e)
        {
            try
            {
                Task.Run(() =>
                {
                    Thread.Sleep(10);
                    this.SetImagePart(this.HeigthImage, WidthImage);
                    ShowObj();
                });

            }
            catch (Exception)
            {
            }
        }
        private HSmartWindowControl hSmwindowControl;

        private HWindowControl hwindowControl;


        public void SetDraw(bool isMargin)
        {
            try
            {
                OneResIamge.SetDraw = isMargin;
                if (!isMargin)
                {
                    HOperatorSet.SetDraw(hWindow, "margin");
                }
                else
                {
                    HOperatorSet.SetDraw(hWindow, "fill");
                }

            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 设置临时显示大小
        /// </summary>
        /// <param name="hv_WindowHandle"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="size"></param>
        /// <param name="selt"></param>
        public static void SetPart(HTuple hv_WindowHandle, int row, int col, int size, double selt = 1)
        {
            try
            {
                if (selt == 0)
                {
                    selt = 1;
                }
                int row1 = row - size;
                int col1 = col - (int)(size * selt);
                int row2 = row + size;
                int col2 = col + (int)(size * selt);
                HOperatorSet.SetPart(hv_WindowHandle, row1, col1, row2, col2);
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 设置临时显示区域大小
        /// </summary>
        /// <param name="rowStrat"></param>
        /// <param name="colStrat"></param>
        /// <param name="rowEnd"></param>
        /// <param name="colEnd"></param>
        public void SetPart(int row, int col, int size)
        {
            try
            {
                if (hwindowControl != null)
                {
                    H_W_Size = (double)hwindowControl.WindowSize.Height / (double)hwindowControl.Width;
                }
                int row1 = row - size;
                int col1 = col - (int)(size / this.H_W_Size);
                int row2 = row + size;
                int col2 = col + (int)(size / this.H_W_Size);
                HOperatorSet.SetPart(hWindow, row1, col1, row2, col2);
                //HOperatorSet.SetPart(hWindow, rowStrat, colStrat, rowEnd, colEnd);
                ShowImage();
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 设置临时显示区域大小
        /// </summary>
        /// <param name="rowStrat"></param>
        /// <param name="colStrat"></param>
        /// <param name="rowEnd"></param>
        /// <param name="colEnd"></param>
        public void SetPart(double row, double col, double size)
        {
            try
            {
                SetPart((int)row, (int)col, (int)size);
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 设置临时显示区域大小
        /// </summary>
        /// <param name="rowStrat"></param>
        /// <param name="colStrat"></param>
        /// <param name="rowEnd"></param>
        /// <param name="colEnd"></param>
        public void SetPart(HTuple rowStrat, HTuple colStrat, HTuple rowEnd, HTuple colEnd)
        {
            try
            {
                HOperatorSet.SetPart(hWindow, rowStrat, colStrat, rowEnd, colEnd);
                ShowImage();
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 设置长期填充图像坐标
        /// </summary>
        /// <param name="rowStrat"></param>
        /// <param name="colStrat"></param>
        /// <param name="rowEnd"></param>
        /// <param name="colEnd"></param>
        public void SetPerpetualPart(HTuple rowStrat, HTuple colStrat, HTuple rowEnd, HTuple colEnd)
        {
            try
            {
                ImageRowStrat = rowStrat.TupleInt();
                ImageColStrat = colStrat.TupleInt();
                HeigthImage = rowEnd.TupleInt();
                WidthImage = colEnd.TupleInt();
                HOperatorSet.SetPart(hWindow, ImageRowStrat, ImageColStrat, HeigthImage, WidthImage);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 设置长期填充图像坐标
        /// </summary>
        /// <param name="row">中心</param>
        /// <param name="col">中心</param>
        /// <param name="size">大小</param>
        /// <param name="selt">比例</param>
        public void SetPointPart(int row, int col, int size, double selt = 1)
        {
            try
            {
                if (selt == 0)
                {
                    selt = 1;
                }
                int row1 = row - size;
                int col1 = col - (int)(size * selt);
                int row2 = row + size;
                int col2 = col + (int)(size * selt);

                ImageRowStrat = row1;
                ImageColStrat = col1;
                HeigthImage = row2;
                WidthImage = col2;
                HOperatorSet.SetPart(hWindow, ImageRowStrat, ImageColStrat, HeigthImage, WidthImage);
            }
            catch (Exception)
            {
            }
        }
        private void HWindowControl1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                WhidowAdd = false;
                this.OneResIamge.IsXLDOrImage = false;
                this.OneResIamge.IsMoveBool = false;
                ShowObj();
            }
            else if (e.KeyCode == Keys.ShiftKey)
            {
                this.OneResIamge.IsXLDOrImage = false;
            }
        }

        private void HWindowControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                WhidowAdd = true;
                this.OneResIamge.IsMoveBool = true;
            }
            else if (e.Shift)
            {
                this.OneResIamge.IsXLDOrImage = true;
            }
            else
            {

            }
            if (e.Control && e.KeyCode == Keys.Q)
            {
                try
                {
                    foreach (var item in OneResIamge.GetNameHobj())
                    {
                        HOperatorSet.AreaCenter(item.Value._HObject, out HTuple area, out HTuple row, out HTuple col);
                        HOperatorSet.DispText(this.hWindow,
                           item.Key, "image", row, col, "black", new HTuple(), new HTuple());
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void hWindowControl1_HMouseUp(object sender, HMouseEventArgs e)
        {
            meuseBool = false;

        }

        private void hSWindowControl1_MouseDown(object sender, HMouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (e.Clicks == 2)
                    {
                        if (Dict.Count == 0)
                        {
                            HOperatorSet.GetImageSize(Image(), out HTuple wi, out HTuple heit);
                            if (wi.Length != 0)
                            {
                                OneResIamge.Width = wi;
                                OneResIamge.Height = heit;
                                Width = wi;
                                Height = heit;
                                HeigthImage = heit;
                                WidthImage = wi;
                            }
                        }
                        if (hwindowControl != null)
                        {
                            double Thisscel = (double)hwindowControl.Height / (double)hwindowControl.Width;
                            double scel = (double)HeigthImage / (double)WidthImage;
                        }

                        foreach (var item in Dict)
                        {
                            this.AddObj(item.Value.HObject);
                        }
                        this.HDrawingObjectClear();
                        ShowImage(false);
                        ShowObj();
                    }
                }
                else if (e.Button == MouseButtons.Left)
                {
                    if (!this.Drawing)
                    {
                        if (this.DrawMode)
                        {
                            foreach (var item in OneResIamge.GetNameHobj())
                            {
                                HOperatorSet.GetRegionIndex(item.Value._HObject, (int)e.Y, (int)e.X, out HTuple index);
                                if (index > 0)
                                {
                                    //RunProgram.DragMoveOBJS(this, OneResIamge.GetNameHobj());
                                    this.ShowObj();
                                    break;
                                }
                                else
                                {
                                    //HOperatorSet.SetDraw(hSmartWindowControl.HalconWindow, "fill");
                                    ////HOperatorSet.SetColor(hSmartWindowControl.HalconWindow, "red");
                                    //HOperatorSet.SetColor(hSmartWindowControl.HalconWindow, "#ff000040");
                                    //HOperatorSet.DispObj(item.Value.Object, hSmartWindowControl.HalconWindow);
                                }
                            }
                        }

                    }
                }

                if (!this.Drawing)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        stratX = e.X;
                        stratY = e.Y;
                        meuseBool = true;
                        //OneResIamge.IsMoveBool = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void hWindowControl1_MouseDown(object sender, HMouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    //HOperatorSet.SetWindowAttr("background_color", "white");
                    if (e.Clicks == 2)
                    {
                        if (Dict.Count == 0)
                        {

                            HOperatorSet.GetImageSize(Image(), out HTuple wi, out HTuple heit);
                            if (wi.Length != 0)
                            {
                                OneResIamge.Width = wi;
                                OneResIamge.Height = heit;
                                Width = wi;
                                Height = heit;
                                HeigthImage = heit;
                                WidthImage = wi;
                            }
                            if (!Drawing)
                            {
                                this.SetImagePart(heit, wi);
                                //HOperatorSet.SetPart(hWindow, ImageRowStrat, ImageColStrat, HeigthImage - 1, WidthImage - 1);
                            }
                        }
                        if (hwindowControl != null)
                        {
                            double Thisscel = (double)hwindowControl.Height / (double)hwindowControl.Width;
                            double scel = (double)HeigthImage / (double)WidthImage;
                        }

                        foreach (var item in Dict)
                        {
                            this.AddObj(item.Value.HObject);
                        }
                        this.HDrawingObjectClear();
                        ShowImage(true);
                        ShowObj();
                    }
                }
                else if (e.Button == MouseButtons.Left)
                {
                    if (!this.Drawing)
                    {
                        if (this.DrawMode)
                        {
                            foreach (var item in OneResIamge.GetNameHobj())
                            {
                                HOperatorSet.GetRegionIndex(item.Value._HObject, (int)e.Y, (int)e.X, out HTuple index);
                                if (index > 0)
                                {
                                    //RunProgram.DragMoveOBJS(this, OneResIamge.GetNameHobj());
                                    this.ShowObj();
                                    break;
                                }
                                else
                                {
                                    //HOperatorSet.SetDraw(hSmartWindowControl.HalconWindow, "fill");
                                    ////HOperatorSet.SetColor(hSmartWindowControl.HalconWindow, "red");
                                    //HOperatorSet.SetColor(hSmartWindowControl.HalconWindow, "#ff000040");
                                    //HOperatorSet.DispObj(item.Value.Object, hSmartWindowControl.HalconWindow);
                                }
                            }
                        }

                    }
                }

                if (!this.Drawing)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        stratX = e.X;
                        stratY = e.Y;
                        meuseBool = true;
                        //OneResIamge.IsMoveBool = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void hWindowControl4_HMouseMove(object sender, HMouseEventArgs e)
        {
            try
            {


                if (!WhidowAdd)
                {
                    return;
                }
                if (meuseBool)
                {
                    double motionX, motionY;
                    motionX = ((e.X - stratX));
                    motionY = ((e.Y - stratY));
                    if (((int)motionX != 0) || ((int)motionY != 0))
                    {
                        if (m_ImageRow1 == null)
                        {
                            m_ImageRow1 = WidthImage;
                            m_ImageCol1 = HeigthImage;
                        }

                        System.Drawing.Rectangle rect2 = hwindowControl.ImagePart;
                        HTuple row = rect2.Y + -motionY;
                        HTuple colum = rect2.X + -motionX;
                        rect2.X = (int)Math.Round(colum.D);
                        rect2.Y = (int)Math.Round(row.D);
                        hwindowControl.ImagePart = rect2;
                        stratX = e.X - motionX;
                        stratY = e.Y - motionY;
                    }
                }
                else
                {
                    //ShowObj();
                }

                ShowImage();
                string data = "";
                //if (Vision.GetHalconRunName() != null)
                //{
                //    Vision.GetHalconRunName().GetCalib().GetPointRctoXY(e.X, e.Y, out HTuple ys, out HTuple xs);
                //    Vision.GetHalconRunName().GetCalib().GetZeroRctoXY(e.Y, e.X, out HTuple ysz, out HTuple xsz);
                //    //Vision.Disp_message(hWindow, "X:" + xs.TupleString("0.02f") +
                //    //    "Y:" + ys.TupleString("0.02f") + "zX" + xsz.TupleString("0.02f") + "zY" + ysz.TupleString("0.02f"), 5,
                //    //    hwindowControl.Width / 4, true, "red", "false");
                //}
                try
                {
                    if (e.Y < 0 || e.X < 0)
                    {
                        return;
                    }
                    //data = "";
                    if (e.X > OneResIamge.Width)
                    {
                        return;
                    }
                    if (e.Y > OneResIamge.Height)
                    {
                        return;
                    }
                    HOperatorSet.GetGrayval(OneResIamge.GetImageOBJ(OneResIamge.ShowImageType), e.Y, e.X, out HTuple Grey);
                    if (Grey.Length == 3)
                    {
                        data += string.Format( "R{0} G{1} B{2}", Grey.TupleSelect(0).D.ToString("000"), Grey.TupleSelect(1).D.ToString("000") , Grey.TupleSelect(2).D.ToString("000"));
                    }
                    else if (Grey.Length == 1)
                    {
                        data += "B" + Grey.D.ToString("000");
                    }
                     data += "C:" + e.X.ToString("0.0") + "R:" + e.Y.ToString("0.0");
                    if (halcon3DModel!=null)
                    {
                        if (halcon3DModel.Image3D!=null)
                        {
                            HOperatorSet.GetGrayval(halcon3DModel.Image3D, e.Y, e.X, out HTuple Grey3D);
                            data += "Z:" + Grey3D.D.ToString("0.000") + "ZP:" + Grey3D.TupleMult(1 / halcon3DModel.SZ);
                        }
                    }
                }
                catch (Exception)
                {
                }
                HWindID.Disp_message(hWindow, data, hwindowControl.Height - 25, 0, true, "red", "false");

            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 放大图像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hWindowControl2_HMouseWheel(object sender, HalconDotNet.HMouseEventArgs e)
        {
            try
            {
                if (!WhidowAdd)
                {
                    return;
                }
                hWindow.GetPart(out m_ImageRow0, out m_ImageCol0, out m_ImageRow1, out m_ImageCol1);
                HOperatorSet.GetMposition(hWindow, out ptY, out ptX, out hv_Button);
                if (m_ImageRow1 == null)
                {
                    m_ImageRow1 = WidthImage;
                    m_ImageCol1 = HeigthImage;
                }
                //向上滑动滚轮，图像缩小。以当前鼠标的坐标为支点进行缩小或放大
                if (e.Delta > 0)
                {//重新计算缩小后的图像区域
                    Row0_1 = ptY - 1 / (1 - H_Scale) * (ptY - m_ImageRow0);
                    Row1_1 = ptY - 1 / (1 - H_Scale) * (ptY - m_ImageRow1);
                    Col0_1 = ptX - 1 / (1 - H_Scale) * (ptX - m_ImageCol0);
                    Col1_1 = ptX - 1 / (1 - H_Scale) * (ptX - m_ImageCol1);     //限定缩小范围
                    if ((Col1_1 - Col0_1).TupleAbs() / WidthImage <= 100)  //设置在图形窗口中显示局部图像
                    {
                        m_ImageRow0 = Row0_1;
                        m_ImageCol0 = Col0_1;
                        m_ImageRow1 = Row1_1;
                        m_ImageCol1 = Col1_1;
                    }
                }
                else
                {          //重新计算放大后的图像区域
                    Row0_1 = ptY - 1 / (1 + H_Scale) * (ptY - m_ImageRow0);
                    Row1_1 = ptY - 1 / (1 + H_Scale) * (ptY - m_ImageRow1);
                    Col0_1 = ptX - 1 / (1 + H_Scale) * (ptX - m_ImageCol0);
                    Col1_1 = ptX - 1 / (1 + H_Scale) * (ptX - m_ImageCol1);
                    HTuple dw = (WidthImage / (Col1_1 - Col0_1).TupleAbs());
                    if ((WidthImage / (Col1_1 - Col0_1).TupleAbs()) <= MaxScale)                    //限定放大范围
                    {
                        //设置在图形窗口中显示局部图像
                        m_ImageRow0 = Row0_1;
                        m_ImageCol0 = Col0_1;
                        m_ImageRow1 = Row1_1;
                        m_ImageCol1 = Col1_1;
                    }
                }
                HOperatorSet.SetPart(hWindow, m_ImageRow0, m_ImageCol0, m_ImageRow1, m_ImageCol1);
                ShowObj();
            }
            catch (Exception es)
            {
            }
        }
        public System.Diagnostics.Stopwatch GetStopwatch()
        {
            return WatchOut;
        }
        public System.Diagnostics.Stopwatch WatchOut = new System.Diagnostics.Stopwatch();
        /// <summary>
        /// 
        /// </summary>
        public int ImageRowStrat = 0;
        /// <summary>
        /// 
        /// </summary>
        public int ImageColStrat = 0;

        public void SetImage(HObject imaget, bool isPart = true)
        {
            try
            {
                if (hWindow == null)
                {
                    return;
                }
                if (OneResIamge == null)
                {
                    OneResIamge = new OneResultOBj();
                }
                if (!HWindID.IsObjectValided(imaget)) return;
  
                OneResIamge.Image = imaget;
                if (imaget==null) return;

                HOperatorSet.GetImageSize(imaget, out HTuple wi, out HTuple heit);
                if (wi.Length==0) return;
                OneResIamge.Width = wi;
                OneResIamge.Height = heit;
                Width = wi;
                Height = heit;
                if (wi.Length != 0)
                {
                    H_W_Size = (double)heit / (double)wi;
                }
                if (isPart)
                {
                    ImageRowStrat = 0;
                    ImageColStrat = 0;
                    if (wi.Length != 0)
                    {
                        WidthImage = wi;
                        HeigthImage = heit;
                    }
                    if (hSmwindowControl == null)
                    {
                        this.SetImagePart(heit, wi);
                        //         hWindow.SetPart(ImageRowStrat, ImageColStrat, HeigthImage - 1, WidthImage - 1);
                    }
                }
                if (hSmwindowControl != null)
                {
                    hSmwindowControl.HalconWindow.ClearWindow();
                }
                hWindow.ClearWindow();
                hWindow.DispObj(GetImageOBJ(OneResIamge.ShowImageType));
                if (isPart)
                {
                    if (hSmwindowControl != null)
                    {
                        hSmwindowControl.SetFullImagePart(null);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


        public void RaedIamge(string paht)
        {
            try
            {
                HOperatorSet.ReadImage(out HObject hObject, paht);
                SetImage(hObject);
            }
            catch (Exception)
            {

            }
        }

        public OneResultOBj GetOneImageR(OneResultOBj oneResultOBj = null)
        {
            return OneResIamge;
        }
        public HObject GetImageOBJ(ImageTypeObj imageType)
        {
            himageHSVRGB.Image = Image();
            return himageHSVRGB.GetImageOBJ(imageType);
        }


        public HimageHSVRGB himageHSVRGB = new HimageHSVRGB();
        /// <summary>
        ///
        /// </summary>
        public OneResultOBj OneResIamge { 
        get;
        set; } 
       = new OneResultOBj();

        private HObject hObject = new HObject();

        public bool Drawing { get; set; }

        public bool DrawMode { get; set; }
        public int DrawType { get; set; }
        public bool DrawErasure { get; set; }
        public HTuple CaliConst { get; set; } = 1;

        public void ShowImage(bool fill = false)
        {
            try
            {
                if (hWindow != null)
                {
                    OneResIamge.ShowImage(hWindow, fill);
                    //  OneResIamge.ShowAll(hWindow);
                }
                if (fill)
                {
                    if (hSmwindowControl != null)
                    {
                        hSmwindowControl.SetFullImagePart(null);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 设置图片后显示的固定区域
        /// </summary>
        /// <param name="hObject"></param>
        public void SetImageObj(HObject hObject)
        {
            OneResIamge.SetHobject(hObject);
        }
        /// <summary>
        /// 显示全部
        /// </summary>
        public void ShowObj()
        {
            try
            {
                if (hSmwindowControl != null)
                {
                    hSmwindowControl.HalconWindow.ClearWindow();
                    if (HWindID.IsObjectValided(Image()))
                    {
                        //hSmwindowControl.SetFullImagePart(null);
                        hSmwindowControl.HalconWindow.DispObj(Image());

                    }

                    OneResIamge.ShowObjEX(hSmwindowControl.HalconWindow);
                }
                else
                {
                    OneResIamge.ShowAll(hWindow);
                }

                foreach (var item in Dict)
                {
                    HTuple TR = new HTuple();
                    HTuple TRC = new HTuple();
                    try
                    {
                        TR = item.Value.dobj.GetDrawingObjectParams("row");
                        TRC = item.Value.dobj.GetDrawingObjectParams("column");
                    }
                    catch (Exception)
                    {

                    }



                    HWindID.Disp_message(hWindow, item.Key, TR, TRC, false, ColorResult.yellow.ToString(), "false");
                }
            }
            catch (Exception)
            {
            }
        }

        public void HobjClear()
        {
            OneResIamge.ClearAllObj();
            this.Drawing = false;
        }

        public void Focus()
        {
            try
            {
                if (hwindowControl != null)
                {
                    hwindowControl.Focus();
                }
                if (hSmwindowControl != null)
                {
                    hSmwindowControl.Focus();
                    //hSmwindowControl.HMoveContent = false;
                }

            }
            catch (Exception)
            {
            }
        }

        public HWindow hWindowHalcon(HWindow hawid = null)
        {
            return this.hWindow;
        }

        public HObject Image(HObject hObject = null)
        {
            if (hObject != null)
            {
                OneResIamge.Image = hObject;
            }
            if (OneResIamge == null)
            {
                OneResIamge = new OneResultOBj();
            }
            return OneResIamge.Image;
        }

        public void AddImageMeassge(HTuple rows, HTuple cols, HTuple meassage, ColorResult colorResult = ColorResult.green)
        {
            OneResIamge.AddImageMassage(rows, cols, meassage, colorResult);
        }

        public void AddImageMeassge(MassageText massageText)
        {
            for (int i = 0; i < massageText.Rows.Count; i++)
            {
                OneResIamge.AddImageMassage(massageText.Rows[i], massageText.Columns[i], massageText.Massage[i], massageText.GetColorResult());
            }

        }
        public void AddMeassge(HTuple text)
        {
            OneResIamge.AddMeassge(text);
        }

        public void AddObj(HObject hObject, ColorResult colorResult = ColorResult.green)
        {

            OneResIamge.AddObj(hObject, colorResult);
        }

        //List<ContainerOBJ> containerOBJs;

        //public List<ContainerOBJ> GetContainerOBJs(List<ContainerOBJ> containers)
        //{
        //    if (containers != null)
        //    {
        //        containerOBJs = containers;
        //    }
        //    return containerOBJs;
        //}

        public void AddObj(HObject hObject, HTuple color)
        {
            OneResIamge.AddObj(hObject, color);
        }
        public void AddNameObj(string names, HObject hObject)
        {
            OneResIamge.AddNameOBJ(names, hObject);
        }
        public void AddNameObj(OneCompOBJs.OneComponent oneComponent)
        {
            if (oneComponent.aOK)
            {
                OneResIamge.AddNameOBJ(oneComponent.ComponentID, oneComponent.ROI());
            }
            else 
            {
                OneResIamge.AddNameOBJ(oneComponent.ComponentID, oneComponent.ROI(),ColorResult.red);
            }
           
        }
        public void AddNameObj(string names, HObject hObject, ColorResult colorResult)
        {
            OneResIamge.AddNameOBJ(names, hObject, colorResult);
        }

        public void SetNameObjColor(string Name, HTuple color)
        {
            try
            {


            }
            catch (Exception)
            {
            }
        }


        public HTuple GetCaliConstMM(HTuple values)
        {
            return values.TupleMult(CaliConst).TupleMult(1);
        }

        public HTuple GetCaliConstPx(HTuple values)
        {
            return values.TupleDiv(CaliConst);
        }

        #region 绘制句柄
        /// <summary>
        /// 删除绘制句柄
        /// </summary>
        public void HDrawingObjectClear()
        {
            this.Drawing = false;
            try
            {
                foreach (var item in Dict)
                {
                    try
                    {

                        hWindow.DetachDrawingObjectFromWindow(item.Value.dobj);
                        if (hwindowControl != null)
                        {
                            //HOperatorSet.DetachDrawingObjectFromWindow(hwindowControl.HalconWindow, item.Value.dobj);
                        }
                        //hWindow.DetachDrawingObjectFromWindow(item.Value.dobj);

                    }
                    catch (Exception ex)
                    {
                    }
                    item.Value.dobj.ClearDrawingObject();
                    item.Value.dobj.ClearHandle();
                    item.Value.dobj.Dispose();
              
                }
                //hWindow.DetachBackgroundFromWindow();
            }
            catch (Exception ex)
            {

            }

            Dict.Clear();
        }

        private HDrawingObject selected_drawing_object;
        /// <summary>
        /// 添加区域到绘制
        /// </summary>
        /// <param name="name">句柄名称</param>
        /// <param name="obj">绘制句柄</param>
        public void AttachDrawObj(string name, HdrawingObj obj)
        {
            if (!Dict.ContainsKey(name))
            {
                Dict.Add(name, obj);
            }
            else
            {
                try
                {
                    Dict[name].dobj.ClearHandle();
                }
                catch (Exception)
                {
                }

                Dict[name] = obj;
            }
            obj.OnEonve();
            hWindow.AttachDrawingObjectToWindow(obj.dobj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwidow"></param>
        /// <param name="hDrawingObject"></param>
        /// <param name="sobelFilterEvne"></param>
        public static void AttachDrawObjName(HWindow hwidow, HDrawingObject hDrawingObject, HDrawingObject.HDrawingObjectCallbackClass dragEnve,
            HDrawingObject.HDrawingObjectCallbackClass detachEvne = null, HDrawingObject.HDrawingObjectCallbackClass ResizeEvne = null,
            HDrawingObject.HDrawingObjectCallbackClass SelectEvne = null, HDrawingObject.HDrawingObjectCallbackClass AttachEvne = null)
        {
            try
            {
                hDrawingObject.OnDrag(dragEnve);//画
                if (detachEvne != null)
                {
                    hDrawingObject.OnDetach(detachEvne);//拆
                }
                if (ResizeEvne != null)
                {
                    hDrawingObject.OnResize(ResizeEvne);//移动大小
                }
                if (SelectEvne != null)
                {
                    hDrawingObject.OnSelect(SelectEvne);//首次选中
                }
                if (AttachEvne != null)
                {
                    hDrawingObject.OnAttach(AttachEvne);//连接
                }
                hwidow.AttachDrawingObjectToWindow(hDrawingObject);

            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 绘制触发事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="obj"></param>
        /// <param name="hDrawingObjectType"></param>
        /// <param name="sobelFilterEvne"></param>
        public void AddAttachDrawObjName(string name, HObject obj, HDrawingObject.HDrawingObjectType hDrawingObjectType, HdrawingObj. SobelFilterEvne sobelFilterEvne, ColorResult colorResult = ColorResult.green)
        {
            try
            {
                HTuple[] hv_part = new HTuple[] { };

                if (obj!=null)
                {
                   
                }
                HOperatorSet.SmallestRectangle2(obj, out HTuple hv_RowA, out HTuple hv_ColumnA, out HTuple hv_Phi, out HTuple hv_Length1, out HTuple hv_Length2);
                switch (hDrawingObjectType)
                {
                    case HDrawingObject.HDrawingObjectType.RECTANGLE1:
                        hv_part = new HTuple[4];
                        HOperatorSet.SmallestRectangle1(obj, out hv_RowA, out hv_ColumnA, out hv_Length1, out hv_Length2);
                        if (hv_RowA.Length == 0)
                        {
                            hv_RowA = this.ImageRowStrat + 0;
                            hv_ColumnA = this.ImageColStrat + 0;
                            hv_Length1 = this.ImageRowStrat + 2400;
                            hv_Length2 = ImageColStrat + 4000;

                        }
                        hv_part[0] = hv_RowA;
                        hv_part[1] = hv_ColumnA;
                        hv_part[2] = hv_Length1;
                        hv_part[3] = hv_Length2;
                        break;
                    case HDrawingObject.HDrawingObjectType.RECTANGLE2:
                        hv_part = new HTuple[5];
                        if (hv_RowA.Length == 0)
                        {
                            hv_RowA = 1400;
                            hv_ColumnA = 2000;
                            hv_Phi = 0;
                            hv_Length1 = 200;
                            hv_Length2 = 400;
                        }
                        hv_part[0] = hv_RowA;
                        hv_part[1] = hv_ColumnA;
                        hv_part[2] = hv_Phi;
                        hv_part[3] = hv_Length1;
                        hv_part[4] = hv_Length2;
                        break;
                    case HDrawingObject.HDrawingObjectType.CIRCLE:
                        HOperatorSet.SmallestCircle(obj, out hv_RowA, out hv_ColumnA, out hv_Length1);
                        hv_part = new HTuple[3];
                        hv_part[0] = hv_RowA;
                        hv_part[1] = hv_ColumnA;
                        hv_part[2] = hv_Length1;
                        break;
                    case HDrawingObject.HDrawingObjectType.ELLIPSE:
                        break;
                    case HDrawingObject.HDrawingObjectType.CIRCLE_SECTOR:
                        HOperatorSet.SmallestCircle(obj, out hv_RowA, out hv_ColumnA, out hv_Length1);
                        hv_part = new HTuple[5];
                        hv_part[0] = hv_RowA;
                        hv_part[1] = hv_ColumnA;
                        hv_part[2] = hv_Length1;
                        hv_part[3] = 0;
                        hv_part[4] = 1.3;
                        break;
                    case HDrawingObject.HDrawingObjectType.ELLIPSE_SECTOR:
                        break;
                    case HDrawingObject.HDrawingObjectType.LINE:
                        break;
                    case HDrawingObject.HDrawingObjectType.XLD_CONTOUR:
                        HOperatorSet.SmallestCircle(obj, out hv_RowA, out hv_ColumnA, out hv_Length1);
                        hv_part = new HTuple[8];
                        hv_part[0] = hv_RowA;
                        hv_part[1] = hv_ColumnA;
                        hv_part[2] = hv_Length1;
                        hv_part[3] = 0;
                        hv_part[4] = 1.3;
                        hv_part[5] = hv_RowA;
                        hv_part[6] = hv_ColumnA;
                        hv_part[7] = hv_Length1;

                        break;
                    case HDrawingObject.HDrawingObjectType.TEXT:
                        hv_part = new HTuple[3];
                        hv_part[0] = hv_RowA.TupleInt();
                        hv_part[1] = hv_ColumnA.TupleInt();
                        hv_part[2] = "123";
                        break;
                    default:
                        break;
                }

                HDrawingObject circle = HDrawingObject.CreateDrawingObject(
                  hDrawingObjectType, hv_part);
                circle.SetDrawingObjectParams("color", colorResult.ToString());
                HdrawingObj hdrawingObj = new HdrawingObj();
                hdrawingObj.dobj = circle;
                hdrawingObj.HObject = obj;
                hdrawingObj.DragEvne += sobelFilterEvne;
                hdrawingObj.DetachEvne += sobelFilterEvne;
                hdrawingObj.SelectEvne += sobelFilterEvne;
                hdrawingObj.ResizeEvne += sobelFilterEvne;
                hdrawingObj.AttachEvne += sobelFilterEvne;
                AttachDrawObj(name, hdrawingObj);
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// 绘制触发事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="obj"></param>
        /// <param name="hDrawingObjectType"></param>
        /// <param name="sobelFilterEvne"></param>
        public void AddAttachDrawObjName(string name,  HDrawingObject hDrawingObjectType, HdrawingObj.SobelFilterEvne sobelFilterEvne, ColorResult colorResult = ColorResult.green)
        {
            try
            {

                hDrawingObjectType.SetDrawingObjectParams("color", colorResult.ToString());
                HdrawingObj hdrawingObj = new HdrawingObj();
                hdrawingObj.dobj = hDrawingObjectType;
                hdrawingObj.HObject = hDrawingObjectType.GetDrawingObjectIconic();
                hdrawingObj.DragEvne += sobelFilterEvne;
                hdrawingObj.DetachEvne += sobelFilterEvne;
                hdrawingObj.SelectEvne += sobelFilterEvne;
                hdrawingObj.ResizeEvne += sobelFilterEvne;
                hdrawingObj.AttachEvne += sobelFilterEvne;
                AttachDrawObj(name, hdrawingObj);
            }
            catch (Exception ex)
            {
            }
        }
        private void OnSelectDrawingObject(HDrawingObject dobj, HWindow hwin, string type)
        {
            SobelFilter(dobj, hwin, type);
        }
        public void SelectDrawing(string name)
        {
            if (Dict.ContainsKey(name))
            {

                //selected_drawing_object.SetDrawingObjectParams("color", ColorResult.green.ToString());
                //Dict[name].dobj.SetDrawingObjectParams("color", ColorResult.red.ToString());
                //selected_drawing_object.OnDetach(SobelFilter);
                //selected_drawing_object.ClearDrawingObject();
                //hWindow.DetachDrawingObjectFromWindow(selected_drawing_object);
                //Dict[name].dobj.OnDrag(SobelFilter);
                //Dict[name].dobj.OnAttach(SobelFilter);
                //Dict[name].dobj.OnResize(SobelFilter);
                //Dict[name].dobj.OnSelect(SobelFilter);
                //Dict[name].dobj.SetDrawingObjectCallback(SobelFilter);
                //Dict[name].dobj.OnSelect(OnSelectDrawingObject);
                //hWindow.AttachDrawingObjectToWindow(Dict[name].dobj);
            }

        }
        /// <summary>
        /// 绘制区域的
        /// </summary>
        public Dictionary<string, HdrawingObj> Dict = new Dictionary<string, HdrawingObj>();
        /// <summary>
        /// 触发事件选中区域
        /// </summary>
        /// <param name="dobj"></param>
        /// <param name="hwin"></param>
        /// <param name="type"></param>
        public void SobelFilter(HDrawingObject dobj, HWindow hwin, string type)
        {
            try
            {
                selected_drawing_object = dobj;
                foreach (var item in Dict)
                {
                    if (item.Value.dobj.ID == dobj.ID)
                    {
                        item.Value.HObject = dobj.GetDrawingObjectIconic();
                        item.Value.OnAttachEvne();
                        break;
                    }
                }
            }
            catch (HalconException hex)
            {
                MessageBox.Show(hex.GetErrorMessage(), "HALCON error", MessageBoxButtons.OK);
            }
        }
        #endregion


        #region Halcon视觉常用算子
   

  



        ///// <summary>
        ///// 静态错误登录
        ///// </summary>
        //private static HslCommunication.LogNet.ILogNet logNet = new HslCommunication.LogNet.LogNetSingle(Application.StartupPath + @"\HalconErrLog.txt");


        /// <summary>
        /// 把Halcon图像转换到OpenCV图像
        /// </summary>
        /// <param name="hImage">Halcon图像_HObject</param>
        /// <returns>OpenCV图像_Mat</returns>
        //public static Mat HImageToMat(HObject hImage)
        //{
        //    try
        //    {
        //        Mat mImage;      // 返回值
        //        HTuple htChannels;       // 通道
        //        HTuple cType = null;     // 类型
        //        HTuple width, height;    // 宽，高
        //        width = height = 0;

        //        htChannels = null;
        //        HOperatorSet.CountChannels(hImage, out htChannels);  // 获取通道

        //        // 通道存在值
        //        if (htChannels.Length == 0)
        //        {
        //            return null;
        //        }
        //        if (htChannels[0].I == 1)           // 单通道
        //        {
        //            HTuple ptr;                     // HTuple_单通道值指针
        //            HOperatorSet.GetImagePointer1(hImage, out ptr, out cType, out width, out height);       // 单通道取值方法（图片，输出“单通道值指针”，输出“类型”，输出“宽”，输出“高”）  // （ptr=2157902018096    cType=byte    width=830    height=822）         
        //            mImage = new Mat(new OpenCvSharp.Size(width, height), MatType.CV_8UC1, new Scalar(0));  // 实例化mImage（大小，MatType.CV_8UC1，new Scalar(0)）
        //            int Width = width;

        //            unsafe
        //            {
        //                //for (int i = 0; i < height; i++)  // 循环赋值
        //                //{
        //                //    IntPtr start = IntPtr.Add(mImage.Data, i * width);                              // Mat的单通道_Data地址+偏移（start=0x000001f66d4df300     Data=0x000001f66d4df300     i * 830）
        //                //    CopyMemory(start, new IntPtr((byte*)ptr.IP + width * i), width);                // CopyMemory（要复制到的地址，复制源的地址，复制的长度）
        //                //}
        //                CopyMemory(mImage.Data, new IntPtr((byte*)ptr.IP), width * height);// CopyMemory（要复制到的地址，复制源的地址，复制的长度）
        //            }
        //            return mImage;
        //        }
        //        else if (htChannels[0].I == 3)      // 三通道
        //        {
        //            HTuple ptrRed;                  // HTuple_R通道值指针
        //            HTuple ptrGreen;                // HTuple_G通道值指针
        //            HTuple ptrBlue;                 // HTuple_B通道值指针

        //            HOperatorSet.GetImagePointer3(hImage, out ptrRed, out ptrGreen, out ptrBlue, out cType, out width, out height);  // 三通道取值方法（图片，输出“R通道值指针”，输出“G通道值指针”，输出“B通道值指针”，输出“类型”，输出“宽”，输出“高”）
        //            Mat pImageRed = new Mat(new OpenCvSharp.Size(width, height), MatType.CV_8UC1);                                   // Mat_R通道值指针（大小，MatType.CV_8UC1）
        //            Mat pImageGreen = new Mat(new OpenCvSharp.Size(width, height), MatType.CV_8UC1);                                 // Mat_G通道值指针（大小，MatType.CV_8UC1）
        //            Mat pImageBlue = new Mat(new OpenCvSharp.Size(width, height), MatType.CV_8UC1);                                  // Mat_B通道值指针（大小，MatType.CV_8UC1）
        //            mImage = new Mat(new OpenCvSharp.Size(width, height), MatType.CV_8UC3, new Scalar(0, 0, 0));                     // Mat_图片（大小，MatType.CV_8UC1，new Scalar(0, 0, 0)）
        //            unsafe
        //            {
        //                //for (int i = 0; i < height; i++)
        //                //{
        //                //    //long step = mImage.Step();
        //                //    IntPtr startRed = IntPtr.Add(pImageRed.Data, i * width);                    // Mat的Red_Data地址+偏移（start=0x000001f66d4df300     Data=0x000001f66d4df300     i * 830）
        //                //    IntPtr startGreen = IntPtr.Add(pImageGreen.Data, i * width);                // Mat的Green_Data地址+偏移（start=0x000001f66d4df300     Data=0x000001f66d4df300     i * 830）
        //                //    IntPtr startBlue = IntPtr.Add(pImageBlue.Data, i * width);                  // Mat的Blue_Data地址+偏移（start=0x000001f66d4df300     Data=0x000001f66d4df300     i * 830）
        //                //    CopyMemory(startRed, new IntPtr((byte*)ptrRed.IP + width * i), width);      // CopyMemory（要复制到的地址，复制源的地址，复制的长度）_Red
        //                //    CopyMemory(startGreen, new IntPtr((byte*)ptrGreen.IP + width * i), width);  // CopyMemory（要复制到的地址，复制源的地址，复制的长度）_Green
        //                //    CopyMemory(startBlue, new IntPtr((byte*)ptrBlue.IP + width * i), width);    // CopyMemory（要复制到的地址，复制源的地址，复制的长度）_Blue
        //                //}
        //                CopyMemory(pImageRed.Data, new IntPtr((byte*)ptrRed.IP), width * height);        // CopyMemory（要复制到的地址，复制源的地址，复制的长度）_Red
        //                CopyMemory(pImageGreen.Data, new IntPtr((byte*)ptrGreen.IP), width * height);  // CopyMemory（要复制到的地址，复制源的地址，复制的长度）_Green
        //                CopyMemory(pImageBlue.Data, new IntPtr((byte*)ptrBlue.IP), width * height);    // CopyMemory（要复制到的地址，复制源的地址，复制的长度）_Blue
        //            }
        //            Mat[] multi = new Mat[] { pImageBlue, pImageGreen, pImageRed };   // 存储rgb三通道
        //            Cv2.Merge(multi, mImage);                                         // rgb三通道合成一张图
        //            pImageRed.Dispose();                                              // Mat_R通道值指针销毁
        //            pImageGreen.Dispose();                                            // Mat_G通道值指针销毁
        //            pImageBlue.Dispose();                                             // Mat_B通道值指针销毁
        //            return mImage;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        ///// <summary>
        ///// 把OpenCV图像转换到Halcon图像
        ///// </summary>
        ///// <param name="mImage">OpenCV图像_Mat</param>
        ///// <returns>Halcon图像_HObject</returns>
        //public static HObject MatToHImage(Mat mImage)
        //{
        //    try
        //    {
        //        HObject hImage;
        //        int matChannels = 0;   // 通道数
        //        Type matType = null;
        //        int width, height;   // 宽，高
        //        width = height = 0;  // 宽，高初始化
        //                             // 获取通道数
        //        matChannels = mImage.Channels();
        //        if (matChannels == 0)
        //        {
        //            return null;
        //        }
        //        if (matChannels == 1)        // 单通道
        //        {
        //            IntPtr ptr;      // 灰度图通道
        //            Mat[] mats = mImage.Split();
        //            // 改自：Mat.GetImagePointer1(mImage, out ptr, out matType, out width, out height);     // ptr=2157902018096    cType=byte    width=830    height=822
        //            ptr = mats[0].Data;          // 取灰度图值
        //            matType = mImage.GetType();  // byte
        //            height = mImage.Rows;        // 高
        //            width = mImage.Cols;         // 宽
        //                                         // 改自：hImage = new HObject(new OpenCvSharp.Size(width, height), MatType.CV_8UC1, new Scalar(0));
        //            byte[] dataGrayScaleImage = new byte[width * height];      //Mat dataGrayScaleImage = new Mat(new OpenCvSharp.Size(width, height), MatType.CV_8UC1);
        //            unsafe
        //            {
        //                fixed (byte* ptrdata = dataGrayScaleImage)
        //                {
        //                    CopyMemory((IntPtr)ptrdata, new IntPtr((long)ptr), width * height);
        //                    HOperatorSet.GenImage1(out hImage, "byte", width, height, (IntPtr)ptrdata);
        //                }
        //            }
        //            return hImage;
        //        }
        //        else if (matChannels == 3)   // 三通道
        //        {
        //            IntPtr ptrRed;    // R通道图
        //            IntPtr ptrGreen;  // G通道图
        //            IntPtr ptrBlue;   // B通道图
        //            Mat[] mats = mImage.Split();
        //            ptrRed = mats[0].Data;    // 取R通道值
        //            ptrGreen = mats[1].Data;  // 取G通道值
        //            ptrBlue = mats[2].Data;   // 取B通道值
        //            matType = mImage.GetType();  // 类型
        //            height = mImage.Rows;        // 高
        //            width = mImage.Cols;         // 宽
        //                                         // 改自：hImage = new HObject(new OpenCvSharp.Size(width, height), MatType.CV_8UC1, new Scalar(0));
        //            byte[] dataRed = new byte[width * height];      //Mat dataGrayScaleImage = new Mat(new OpenCvSharp.Size(width, height), MatType.CV_8UC1);
        //            byte[] dataGreen = new byte[width * height];
        //            byte[] dataBlue = new byte[width * height];
        //            unsafe
        //            {
        //                fixed (byte* ptrdataRed = dataRed, ptrdataGreen = dataGreen, ptrdataBlue = dataBlue)
        //                {
        //                    CopyMemory((IntPtr)ptrdataRed, new IntPtr((long)ptrRed), width * height);        // 复制R通道
        //                    CopyMemory((IntPtr)ptrdataGreen, new IntPtr((long)ptrGreen), width * height);    // 复制G通道
        //                    CopyMemory((IntPtr)ptrdataBlue, new IntPtr((long)ptrBlue), width * height);      // 复制B通道
        //                    HOperatorSet.GenImage3(out hImage, "byte", width, height, (IntPtr)ptrdataRed, (IntPtr)ptrdataGreen, (IntPtr)ptrdataBlue);   // 合成
        //                }
        //            }
        //            return hImage;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// 转换
        ///// </summary>
        ///// <param name="image"></param>
        ///// <param name="res"></param>
        //public static void HObject2Mat24(HObject image, out Mat res)
        //{
        //    HTuple hred, hgreen, hblue, type, width, height;
        //    HOperatorSet.GetImagePointer3(image, out hred, out hgreen, out hblue, out type, out width, out height);
        //    int bytes = width * height * 3;
        //    byte[] rgbvalues = new byte[bytes];
        //    unsafe
        //    {
        //        IntPtr intPtrR = hred;
        //        IntPtr intPtrG = hgreen;
        //        IntPtr intPtrB = hblue;
        //        byte* r = ((byte*)intPtrR);
        //        byte* g = ((byte*)intPtrG);
        //        byte* b = ((byte*)intPtrB);

        //        int lengh = width * height;
        //        for (int i = 0; i < lengh; i++)
        //        {
        //            rgbvalues[i * 3] = (b)[i];
        //            rgbvalues[i * 3 + 1] = (g)[i];
        //            rgbvalues[i * 3 + 2] = (r)[i];
        //            //bptr[i * 4 + 3] = 255;
        //        }
        //    }
        //    res = new Mat(height, width, MatType.CV_8UC3, rgbvalues);
        //}

        /// <summary>
        /// 图片转换BITMAP转Himage
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static HImage ToHImage(Bitmap bitmap)
        {
            HImage h_img = new HImage();
            try
            {
                Bitmap image = (Bitmap)bitmap.Clone();

                BitmapData bmData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                unsafe
                {
                    // Create HALCON image from the pointer.  bgr
                    h_img.GenImageInterleaved(bmData.Scan0, "bgr", image.Width, image.Height, -1, "byte", image.Width, image.Height, 0, 0, -1, 0);
                    //tmp = h_img;  
                }
                image.UnlockBits(bmData);
                image.Dispose();
                bitmap.Dispose();
            }
            catch
            {
                //h_img = tmp;  
            }
            return h_img;
        }


        /// <summary>
        /// halcon图像转Bitmap
        /// </summary>
        /// <param name="ho_Image"></param>
        /// <returns></returns>
        public static Bitmap HObjectToBitmap(HObject ho_Image)
        {
            try
            {
                if (!IsObjectValided(ho_Image))
                {
                    return null;
                }
                HOperatorSet.CountChannels(ho_Image, out HTuple compactness);
                if (compactness.Length == 0)
                {
                    return null;
                }
                if (compactness == 3)
                {
                    HOperatorSet.GetImagePointer3(ho_Image, out HTuple hv_Red, out HTuple hv_Green, out HTuple hv_Blue, out HTuple type, out HTuple width, out HTuple height);
                    Bitmap bmpImage = new Bitmap(width.I, height.I, PixelFormat.Format32bppArgb);
                    BitmapData bitmapData = bmpImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                    unsafe
                    {
                        IntPtr intPtrR = hv_Red;
                        IntPtr intPtrG = hv_Green;
                        IntPtr intPtrB = hv_Blue;
                        byte* bptr = (byte*)bitmapData.Scan0;
                        //byte* r = ((byte*)hv_Red.I);
                        //byte* g = ((byte*)hv_Green.I);
                        //byte* b = ((byte*)hv_Blue.I);
                        byte* r = ((byte*)intPtrR);
                        byte* g = ((byte*)intPtrG);
                        byte* b = ((byte*)intPtrB);
                        int length = width * height;
                        for (int i = 0; i < length; i++)
                        {
                            bptr[i * 4] = (b)[i];
                            bptr[i * 4 + 1] = (g)[i];
                            bptr[i * 4 + 2] = (r)[i];
                            bptr[i * 4 + 3] = 255;
                        }
                    }
                    bmpImage.UnlockBits(bitmapData);
                    //32位Bitmap转24位
                    Bitmap bmpImage24 = new Bitmap(bmpImage.Width, bmpImage.Height, PixelFormat.Format24bppRgb);
                    Graphics graphics = Graphics.FromImage(bmpImage24);
                    graphics.DrawImage(bmpImage, new Rectangle(0, 0, bmpImage.Width, bmpImage.Height));
                    return bmpImage24;
                }
                else
                {
                    try
                    {
                        HOperatorSet.GetImagePointer1(ho_Image, out HTuple pointer, out HTuple type, out HTuple width, out HTuple height);
                        Bitmap bmpImage = new Bitmap(width.I, height.I, PixelFormat.Format8bppIndexed);
                        ColorPalette pal = bmpImage.Palette;
                        for (int i = 0; i < 256; i++)
                        {
                            pal.Entries[i] = Color.FromArgb(255, i, i, i);
                        }
                        bmpImage.Palette = pal;
                        BitmapData bitmapData = bmpImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
                        int pixelSize = Bitmap.GetPixelFormatSize(bitmapData.PixelFormat) / 8;
                        int stride = bitmapData.Stride;

                        IntPtr ptr = (IntPtr)bitmapData.Scan0;
                        if (width % 4 == 0)
                            CopyMemory(ptr, pointer, width * height * pixelSize);
                        else
                        {
                            for (int i = 0; i < height; i++)
                            {
                                CopyMemory(ptr, pointer, width * pixelSize);
                                pointer += width;
                                ptr += bitmapData.Stride;
                            }
                        }
                        bmpImage.UnlockBits(bitmapData);
                        return bmpImage;
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        [DllImport("kernel32.dll")]
        public static extern void CopyMemory(int Destination, int add, int Length);

        [DllImport("kernel32.dll")]
        public static extern void CopyMemory(IntPtr Destination, IntPtr add, int Length);
        /// <summary>
        /// 24位实际使用时，假如原图8位灰度图，那么BitmapToHObjectBpp8 和BitmapToHObjectBpp24的结果是一样的。而为24位彩色图时，只能用BitmapToHObjectBpp24。
        ///可先得到Bitmap 图的PixelFormat ，而后再进行转换。
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static HObject ToBitmap2HObjectBpp24(Bitmap bmp)
        {
            HObject image = new HObject();
            try
            {
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                BitmapData srcBmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                HOperatorSet.GenImageInterleaved(out image, srcBmpData.Scan0, "bgr", bmp.Width, bmp.Height, 0, "byte", 0, 0, 0, 0, -1, 0);
                bmp.UnlockBits(srcBmpData);
            }
            catch (Exception ex)
            {
                image = null;
            }
            return image;
        }

        /// <summary>
        /// 24位实际使用时，假如原图8位灰度图，那么BitmapToHObjectBpp8 和BitmapToHObjectBpp24的结果是一样的。而为24位彩色图时，只能用BitmapToHObjectBpp24。
        ///可先得到Bitmap 图的PixelFormat ，而后再进行转换。
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static HObject ToBitmap2HObjectBpp8(Bitmap bmp)
        {
            HObject image = new HObject();
            try
            {
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

                BitmapData srcBmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);

                HOperatorSet.GenImage1(out image, "byte", bmp.Width, bmp.Height, srcBmpData.Scan0);
                bmp.UnlockBits(srcBmpData);
            }
            catch (Exception ex)
            {
                image = null;
            }
            return image;
        }

        /// <summary>
        /// 图像转换某些像素图像无法转换
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static HObject GenImageInterleaved(Bitmap bitmap)
        {
            HObject image;
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);

            BitmapData bitmapData = bitmap.LockBits(rectangle, ImageLockMode.ReadOnly, bitmap.PixelFormat);
            try
            {
                if (bitmap.PixelFormat == PixelFormat.Format8bppIndexed)
                {
                    HOperatorSet.GenImage1(out image, "byte", bitmap.Width, bitmap.Height, bitmapData.Scan0);
                }
                else
                {
                    HOperatorSet.GenImageInterleaved(out image, bitmapData.Scan0, "bgr", bitmap.Width, bitmap.Height, 0, "byte", 0, 0, 0, 0, -1, 0);
                }
                //HOperatorSet.AccessChannel(image, out image, 1);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
                bitmap.Dispose();
            }

            return image;
        }

        /// <summary>
        /// 改变图像大小
        /// </summary>
        /// <param name="imgToResize"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Image ResizeImage(Image imgToResize, System.Drawing.Size size)
        {
            //获取图片宽度
            int sourceWidth = imgToResize.Width;
            //获取图片高度
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            //计算宽度的缩放比例
            nPercentW = ((float)size.Width / (float)sourceWidth);
            //计算高度的缩放比例
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;
            //期望的宽度
            int destWidth = (int)(sourceWidth * nPercent);
            //期望的高度
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //绘制图像
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (System.Drawing.Image)b;
        }


        public static void HtupleSrot(HTuple rows, HTuple cols, bool isRowCol, bool isD_R, out HTuple indext, out HTuple rowsSrot, out HTuple colsSrot)
        {

            indext = new HTuple();
            rowsSrot = new HTuple();
            colsSrot = new HTuple();
            List<XYZPoint2D> RandomPointList = new List<XYZPoint2D>();
            List<XYZPoint2D> SortedPointList = new List<XYZPoint2D>();
            List<XYZPoint2D> YSortedPointList = new List<XYZPoint2D>();
            List<XYZPoint2D> RowPointList = new List<XYZPoint2D>();

            try
            {

                for (int i = 0; i < rows.Length; i++)
                {
                    RandomPointList.Add(new XYZPoint2D(cols.TupleSelect(i).D, rows.TupleSelect(i).D));

                }
                indext = rows.TupleSortIndex();

                //rowsSrot = rows.TupleSelect(indext);
                //colsSrot = cols.TupleSelect(indext);


                YSortedPointList = RandomPointList.OrderBy(o => o.Y).ToList();   //坐标点按Y值升序排序（Y值从小到大的排序）


                const double LineSpacing = 150;
                for (int i = 0; i < YSortedPointList.Count - 1; i++)
                {
                    //通过Y值之间的差值大小来判断坐标点是否属于同一行
                    if (Math.Abs(YSortedPointList[i].Y - YSortedPointList[i + 1].Y) < LineSpacing)
                    {
                        RowPointList.Add(YSortedPointList[i]);
                        //如果最后一个点不是单独一行的情况
                        if (YSortedPointList.Count - 2 == i)
                        {
                            RowPointList.Add(YSortedPointList[i + 1]);      //将最后一个坐标元素添加进来
                            RowPointList = RowPointList.OrderBy(o => o.X).ToList();
                            SortedPointList = SortedPointList.Concat(RowPointList).ToList();
                            RowPointList.Clear();
                        }
                    }
                    else
                    {
                        //如果第一个点是单独一行的情况
                        if (0 == i)
                        {
                            SortedPointList.Add(YSortedPointList[i]);
                            continue;
                        }
                        RowPointList.Add(YSortedPointList[i]);
                        RowPointList = RowPointList.OrderBy(o => o.X).ToList();                  //坐标点按X值升序排序
                        SortedPointList = SortedPointList.Concat(RowPointList).ToList();
                        RowPointList.Clear();
                        //如果最后一个点是单独一行的情况
                        if (YSortedPointList.Count - 2 == i)
                        {
                            SortedPointList.Add(YSortedPointList[i + 1]);
                        }
                    }
                }

                for (int i = 0; i < SortedPointList.Count; i++)
                {
                    rowsSrot.Append(SortedPointList[i].Y);
                    colsSrot.Append(SortedPointList[i].X);
                }
            }
            catch (Exception)
            {

            }
        }
        #region 判断点是否在图形上
        /// <summary>
        /// 判断点是否在图形上
        /// </summary>
        /// <param name="mousePoint">鼠标坐标</param>
        /// <param name="startPoint">起始点</param>
        /// <param name="endPoint">终点</param>
        /// <returns></returns>
        public static bool IsContains(XYZPoint2D mousePoint, XYZPoint2D startPoint, XYZPoint2D endPoint)
        {
            bool result = false;
          
            //左上右下
            if (endPoint.X > startPoint.X && endPoint.Y > startPoint.Y)
            {
                if (mousePoint.X >= startPoint.X &&
                    mousePoint.X <= endPoint.X &&
                    mousePoint.Y >= startPoint.Y &&
                    mousePoint.Y <= endPoint.Y)
                {
                    result = true;
                }
            }
            //右上左下
            else if (endPoint.X < startPoint.X && endPoint.Y > startPoint.Y)
            {
                if (mousePoint.X <= startPoint.X &&
                    mousePoint.X >= endPoint.X &&
                    mousePoint.Y >= startPoint.Y &&
                    mousePoint.Y <= endPoint.Y)
                {
                    result = true;
                }
            }
            //右下左上
            else if (endPoint.X < startPoint.X && endPoint.Y < startPoint.Y)
            {
                if (mousePoint.X <= startPoint.X &&
                    mousePoint.X >= endPoint.X &&
                    mousePoint.Y <= startPoint.Y &&
                    mousePoint.Y >= endPoint.Y)
                {
                    result = true;
                }
            }
            //左下右上
            else if (endPoint.X > startPoint.X && endPoint.Y < startPoint.Y)
            {
                if (mousePoint.X >= startPoint.X &&
                    mousePoint.X <= endPoint.X &&
                    mousePoint.Y <= startPoint.Y &&
                    mousePoint.Y >= endPoint.Y)
                {
                    result = true;
                }
            }
            return result;
        }
        public static bool IsContains(HTuple row, HTuple col, HTuple rowStrat, HTuple colStrat, HTuple rowEnd, HTuple colEnd)
        {
            return IsContains(new XYZPoint2D(row.D, col.D), new XYZPoint2D(rowStrat.D, colStrat.D), new XYZPoint2D(rowEnd.D, colEnd.D));
        }
        #endregion


        /// <summary>
        ///
        /// </summary>
        /// <param name="hv_WindowHandle"></param>
        /// <param name="hv_Size"></param>
        /// <param name="hv_Font"></param>
        /// <param name="hv_Bold"></param>
        /// <param name="hv_Slant"></param>
        public static void Set_Display_Font(HTuple hv_WindowHandle, HTuple hv_Size, HTuple hv_Font,
            HTuple hv_Bold, HTuple hv_Slant)
        {
            // Local iconic variables

            // Local control variables

            HTuple hv_OS = null, hv_BufferWindowHandle = new HTuple();
            HTuple hv_Ascent = new HTuple(), hv_Descent = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_Scale = new HTuple(), hv_Exception = new HTuple();
            HTuple hv_SubFamily = new HTuple(), hv_Fonts = new HTuple();
            HTuple hv_SystemFonts = new HTuple(), hv_Guess = new HTuple();
            HTuple hv_I = new HTuple(), hv_Index = new HTuple(), hv_AllowedFontSizes = new HTuple();
            HTuple hv_Distances = new HTuple(), hv_Indices = new HTuple();
            HTuple hv_FontSelRegexp = new HTuple(), hv_FontsCourier = new HTuple();
            HTuple hv_Bold_COPY_INP_TMP = hv_Bold.Clone();
            HTuple hv_Font_COPY_INP_TMP = hv_Font.Clone();
            HTuple hv_Size_COPY_INP_TMP = hv_Size.Clone();
            HTuple hv_Slant_COPY_INP_TMP = hv_Slant.Clone();

            // Initialize local and output iconic variables
            //This procedure sets the text font of the current window with
            //the specified attributes.
            //It is assumed that following fonts are installed on the system:
            //Windows: Courier New, Arial Times New Roman
            //Mac OS X: CourierNewPS, Arial, TimesNewRomanPS
            //Linux: courier, helvetica, times
            //Because fonts are displayed smaller on Linux than on Windows,
            //a scaling factor of 1.25 is used the get comparable results.
            //For Linux, only a limited number of font sizes is supported,
            //to get comparable results, it is recommended to use one of the
            //following sizes: 9, 11, 14, 16, 20, 27
            //(which will be mapped internally on Linux systems to 11, 14, 17, 20, 25, 34)
            //
            //Input parameters:
            //WindowHandle: The graphics window for which the font will be set
            //Size: The font size. If Size=-1, the default of 16 is used.
            //Bold: If set to 'true', a bold font is used
            //Slant: If set to 'true', a slanted font is used
            //
            HOperatorSet.GetSystem("operating_system", out hv_OS);
            // dev_get_preferences(...); only in hdevelop
            // dev_set_preferences(...); only in hdevelop
            if ((int)((new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(new HTuple()))).TupleOr(
                new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(-1)))) != 0)
            {
                hv_Size_COPY_INP_TMP = 16;
            }
            if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Win"))) != 0)
            {
                //Set font on Windows systems
                try
                {
                    //Check, if font scaling is switched on
                    HOperatorSet.OpenWindow(0, 0, 256, 256, 0, "buffer", "", out hv_BufferWindowHandle);
                    HOperatorSet.SetFont(hv_BufferWindowHandle, "-Consolas-16-*-0-*-*-1-");
                    HOperatorSet.GetStringExtents(hv_BufferWindowHandle, "test_string", out hv_Ascent,
                        out hv_Descent, out hv_Width, out hv_Height);
                    //Expected width is 110
                    hv_Scale = 110.0 / hv_Width;
                    hv_Size_COPY_INP_TMP = ((hv_Size_COPY_INP_TMP * hv_Scale)).TupleInt();
                    HOperatorSet.CloseWindow(hv_BufferWindowHandle);
                }
                // catch (Exception)
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    //throw (Exception)
                }
                if ((int)((new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("Courier"))).TupleOr(
                    new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("courier")))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "Courier New";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "Consolas";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "Arial";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "Times New Roman";
                }
                if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_Bold_COPY_INP_TMP = 1;
                }
                else if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("false"))) != 0)
                {
                    hv_Bold_COPY_INP_TMP = 0;
                }
                else
                {
                    hv_Exception = "Wrong value of control parameter Bold";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_Slant_COPY_INP_TMP = 1;
                }
                else if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("false"))) != 0)
                {
                    hv_Slant_COPY_INP_TMP = 0;
                }
                else
                {
                    hv_Exception = "Wrong value of control parameter Slant";
                    throw new HalconException(hv_Exception);
                }
                try
                {
                    HOperatorSet.SetFont(hv_WindowHandle, ((((((("-" + hv_Font_COPY_INP_TMP) + "-") + hv_Size_COPY_INP_TMP) + "-*-") + hv_Slant_COPY_INP_TMP) + "-*-*-") + hv_Bold_COPY_INP_TMP) + "-");
                }
                // catch (Exception)
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    //throw (Exception)
                }
            }
            else if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Dar"))) != 0)
            {
                //Set font on Mac OS X systems. Since OS X does not have a strict naming
                //scheme for font attributes, we use tables to determine the correct font
                //name.
                hv_SubFamily = 0;
                if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_SubFamily = hv_SubFamily.TupleBor(1);
                }
                else if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleNotEqual("false"))) != 0)
                {
                    hv_Exception = "Wrong value of control parameter Slant";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_SubFamily = hv_SubFamily.TupleBor(2);
                }
                else if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleNotEqual("false"))) != 0)
                {
                    hv_Exception = "Wrong value of control parameter Bold";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))) != 0)
                {
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "Menlo-Regular";
                    hv_Fonts[1] = "Menlo-Italic";
                    hv_Fonts[2] = "Menlo-Bold";
                    hv_Fonts[3] = "Menlo-BoldItalic";
                }
                else if ((int)((new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("Courier"))).TupleOr(
                    new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("courier")))) != 0)
                {
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "CourierNewPSMT";
                    hv_Fonts[1] = "CourierNewPS-ItalicMT";
                    hv_Fonts[2] = "CourierNewPS-BoldMT";
                    hv_Fonts[3] = "CourierNewPS-BoldItalicMT";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
                {
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "ArialMT";
                    hv_Fonts[1] = "Arial-ItalicMT";
                    hv_Fonts[2] = "Arial-BoldMT";
                    hv_Fonts[3] = "Arial-BoldItalicMT";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
                {
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "TimesNewRomanPSMT";
                    hv_Fonts[1] = "TimesNewRomanPS-ItalicMT";
                    hv_Fonts[2] = "TimesNewRomanPS-BoldMT";
                    hv_Fonts[3] = "TimesNewRomanPS-BoldItalicMT";
                }
                else
                {
                    //Attempt to figure out which of the fonts installed on the system
                    //the user could have meant.
                    HOperatorSet.QueryFont(hv_WindowHandle, out hv_SystemFonts);
                    hv_Fonts = new HTuple();
                    hv_Fonts = hv_Fonts.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Fonts = hv_Fonts.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Fonts = hv_Fonts.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Fonts = hv_Fonts.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Guess = new HTuple();
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-Regular");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "MT");
                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Guess.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                    {
                        HOperatorSet.TupleFind(hv_SystemFonts, hv_Guess.TupleSelect(hv_I), out hv_Index);
                        if ((int)(new HTuple(hv_Index.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_Fonts == null)
                                hv_Fonts = new HTuple();
                            hv_Fonts[0] = hv_Guess.TupleSelect(hv_I);
                            break;
                        }
                    }
                    //Guess name of slanted font
                    hv_Guess = new HTuple();
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-Italic");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-ItalicMT");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-Oblique");
                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Guess.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                    {
                        HOperatorSet.TupleFind(hv_SystemFonts, hv_Guess.TupleSelect(hv_I), out hv_Index);
                        if ((int)(new HTuple(hv_Index.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_Fonts == null)
                                hv_Fonts = new HTuple();
                            hv_Fonts[1] = hv_Guess.TupleSelect(hv_I);
                            break;
                        }
                    }
                    //Guess name of bold font
                    hv_Guess = new HTuple();
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-Bold");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-BoldMT");
                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Guess.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                    {
                        HOperatorSet.TupleFind(hv_SystemFonts, hv_Guess.TupleSelect(hv_I), out hv_Index);
                        if ((int)(new HTuple(hv_Index.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_Fonts == null)
                                hv_Fonts = new HTuple();
                            hv_Fonts[2] = hv_Guess.TupleSelect(hv_I);
                            break;
                        }
                    }
                    //Guess name of bold slanted font
                    hv_Guess = new HTuple();
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-BoldItalic");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-BoldItalicMT");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-BoldOblique");
                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Guess.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                    {
                        HOperatorSet.TupleFind(hv_SystemFonts, hv_Guess.TupleSelect(hv_I), out hv_Index);
                        if ((int)(new HTuple(hv_Index.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_Fonts == null)
                                hv_Fonts = new HTuple();
                            hv_Fonts[3] = hv_Guess.TupleSelect(hv_I);
                            break;
                        }
                    }
                }
                hv_Font_COPY_INP_TMP = hv_Fonts.TupleSelect(hv_SubFamily);
                try
                {
                    HOperatorSet.SetFont(hv_WindowHandle, (hv_Font_COPY_INP_TMP + "-") + hv_Size_COPY_INP_TMP);
                }
                // catch (Exception)
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    //throw (Exception)
                }
            }
            else
            {
                //Set font for UNIX systems
                hv_Size_COPY_INP_TMP = hv_Size_COPY_INP_TMP * 1.25;
                hv_AllowedFontSizes = new HTuple();
                hv_AllowedFontSizes[0] = 11;
                hv_AllowedFontSizes[1] = 14;
                hv_AllowedFontSizes[2] = 17;
                hv_AllowedFontSizes[3] = 20;
                hv_AllowedFontSizes[4] = 25;
                hv_AllowedFontSizes[5] = 34;
                if ((int)(new HTuple(((hv_AllowedFontSizes.TupleFind(hv_Size_COPY_INP_TMP))).TupleEqual(
                    -1))) != 0)
                {
                    hv_Distances = ((hv_AllowedFontSizes - hv_Size_COPY_INP_TMP)).TupleAbs();
                    HOperatorSet.TupleSortIndex(hv_Distances, out hv_Indices);
                    hv_Size_COPY_INP_TMP = hv_AllowedFontSizes.TupleSelect(hv_Indices.TupleSelect(
                        0));
                }
                if ((int)((new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))).TupleOr(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual(
                    "Courier")))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "courier";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "helvetica";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "times";
                }
                if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_Bold_COPY_INP_TMP = "bold";
                }
                else if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("false"))) != 0)
                {
                    hv_Bold_COPY_INP_TMP = "medium";
                }
                else
                {
                    hv_Exception = "Wrong value of control parameter Bold";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("times"))) != 0)
                    {
                        hv_Slant_COPY_INP_TMP = "i";
                    }
                    else
                    {
                        hv_Slant_COPY_INP_TMP = "o";
                    }
                }
                else if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("false"))) != 0)
                {
                    hv_Slant_COPY_INP_TMP = "r";
                }
                else
                {
                    hv_Exception = "Wrong value of control parameter Slant";
                    throw new HalconException(hv_Exception);
                }
                try
                {
                    HOperatorSet.SetFont(hv_WindowHandle, ((((((("-adobe-" + hv_Font_COPY_INP_TMP) + "-") + hv_Bold_COPY_INP_TMP) + "-") + hv_Slant_COPY_INP_TMP) + "-normal-*-") + hv_Size_COPY_INP_TMP) + "-*-*-*-*-*-*-*");
                }
                // catch (Exception)
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    if ((int)((new HTuple(((hv_OS.TupleSubstr(0, 4))).TupleEqual("Linux"))).TupleAnd(
                        new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("courier")))) != 0)
                    {
                        HOperatorSet.QueryFont(hv_WindowHandle, out hv_Fonts);
                        hv_FontSelRegexp = (("^-[^-]*-[^-]*[Cc]ourier[^-]*-" + hv_Bold_COPY_INP_TMP) + "-") + hv_Slant_COPY_INP_TMP;
                        hv_FontsCourier = ((hv_Fonts.TupleRegexpSelect(hv_FontSelRegexp))).TupleRegexpMatch(
                            hv_FontSelRegexp);
                        if ((int)(new HTuple((new HTuple(hv_FontsCourier.TupleLength())).TupleEqual(
                            0))) != 0)
                        {
                            hv_Exception = "Wrong font name";
                            //throw (Exception)
                        }
                        else
                        {
                            try
                            {
                                HOperatorSet.SetFont(hv_WindowHandle, (((hv_FontsCourier.TupleSelect(
                                    0)) + "-normal-*-") + hv_Size_COPY_INP_TMP) + "-*-*-*-*-*-*-*");
                            }
                            // catch (Exception)
                            catch (HalconException HDevExpDefaultException2)
                            {
                                HDevExpDefaultException2.ToHTuple(out hv_Exception);
                                //throw (Exception)
                            }
                        }
                    }
                    //throw (Exception)
                }
            }
            // dev_set_preferences(...); only in hdevelop

            return;
        }
        /// <summary>
        /// 获取相机参数
        /// </summary>
        /// <param name="hv_CameraParam"></param>
        /// <param name="hv_CameraType"></param>
        /// <param name="hv_ParamNames"></param>
        /// <exception cref="HalconException"></exception>
        // Chapter: Calibration / Camera Parameters
        // Short Description: Get the names of the parameters in a camera parameter tuple.
        public static void get_cam_par_names(HTuple hv_CameraParam, out HTuple hv_CameraType,
            out HTuple hv_ParamNames)
        {
            // Local iconic variables
            // Local control variables
            HTuple hv_CameraParamAreaScanDivision = null;
            HTuple hv_CameraParamAreaScanPolynomial = null, hv_CameraParamAreaScanTelecentricDivision = null;
            HTuple hv_CameraParamAreaScanTelecentricPolynomial = null;
            HTuple hv_CameraParamAreaScanTiltDivision = null, hv_CameraParamAreaScanTiltPolynomial = null;
            HTuple hv_CameraParamAreaScanImageSideTelecentricTiltDivision = null;
            HTuple hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial = null;
            HTuple hv_CameraParamAreaScanBilateralTelecentricTiltDivision = null;
            HTuple hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial = null;
            HTuple hv_CameraParamAreaScanObjectSideTelecentricTiltDivision = null;
            HTuple hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial = null;
            HTuple hv_CameraParamLinesScan = null, hv_CameraParamAreaScanTiltDivisionLegacy = null;
            HTuple hv_CameraParamAreaScanTiltPolynomialLegacy = null;
            HTuple hv_CameraParamAreaScanTelecentricDivisionLegacy = null;
            HTuple hv_CameraParamAreaScanTelecentricPolynomialLegacy = null;
            HTuple hv_CameraParamAreaScanBilateralTelecentricTiltDivisionLegacy = null;
            HTuple hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy = null;
            // Initialize local and output iconic variables
            hv_CameraType = new HTuple();
            hv_ParamNames = new HTuple();
            //get_cam_par_names returns for each element in the camera
            //parameter tuple that is passed in CameraParam the name
            //of the respective camera parameter. The parameter names
            //are returned in ParamNames. Additionally, the camera
            //type is returned in CameraType. Alternatively, instead of
            //the camera parameters, the camera type can be passed in
            //CameraParam in form of one of the following strings:
            //  - 'area_scan_division'
            //  - 'area_scan_polynomial'
            //  - 'area_scan_tilt_division'
            //  - 'area_scan_tilt_polynomial'
            //  - 'area_scan_telecentric_division'
            //  - 'area_scan_telecentric_polynomial'
            //  - 'area_scan_tilt_bilateral_telecentric_division'
            //  - 'area_scan_tilt_bilateral_telecentric_polynomial'
            //  - 'area_scan_tilt_object_side_telecentric_division'
            //  - 'area_scan_tilt_object_side_telecentric_polynomial'
            //  - 'line_scan'
            //
            hv_CameraParamAreaScanDivision = new HTuple();
            hv_CameraParamAreaScanDivision[0] = "focus";
            hv_CameraParamAreaScanDivision[1] = "kappa";
            hv_CameraParamAreaScanDivision[2] = "sx";
            hv_CameraParamAreaScanDivision[3] = "sy";
            hv_CameraParamAreaScanDivision[4] = "cx";
            hv_CameraParamAreaScanDivision[5] = "cy";
            hv_CameraParamAreaScanDivision[6] = "image_width";
            hv_CameraParamAreaScanDivision[7] = "image_height";
            hv_CameraParamAreaScanPolynomial = new HTuple();
            hv_CameraParamAreaScanPolynomial[0] = "focus";
            hv_CameraParamAreaScanPolynomial[1] = "k1";
            hv_CameraParamAreaScanPolynomial[2] = "k2";
            hv_CameraParamAreaScanPolynomial[3] = "k3";
            hv_CameraParamAreaScanPolynomial[4] = "p1";
            hv_CameraParamAreaScanPolynomial[5] = "p2";
            hv_CameraParamAreaScanPolynomial[6] = "sx";
            hv_CameraParamAreaScanPolynomial[7] = "sy";
            hv_CameraParamAreaScanPolynomial[8] = "cx";
            hv_CameraParamAreaScanPolynomial[9] = "cy";
            hv_CameraParamAreaScanPolynomial[10] = "image_width";
            hv_CameraParamAreaScanPolynomial[11] = "image_height";
            hv_CameraParamAreaScanTelecentricDivision = new HTuple();
            hv_CameraParamAreaScanTelecentricDivision[0] = "magnification";
            hv_CameraParamAreaScanTelecentricDivision[1] = "kappa";
            hv_CameraParamAreaScanTelecentricDivision[2] = "sx";
            hv_CameraParamAreaScanTelecentricDivision[3] = "sy";
            hv_CameraParamAreaScanTelecentricDivision[4] = "cx";
            hv_CameraParamAreaScanTelecentricDivision[5] = "cy";
            hv_CameraParamAreaScanTelecentricDivision[6] = "image_width";
            hv_CameraParamAreaScanTelecentricDivision[7] = "image_height";
            hv_CameraParamAreaScanTelecentricPolynomial = new HTuple();
            hv_CameraParamAreaScanTelecentricPolynomial[0] = "magnification";
            hv_CameraParamAreaScanTelecentricPolynomial[1] = "k1";
            hv_CameraParamAreaScanTelecentricPolynomial[2] = "k2";
            hv_CameraParamAreaScanTelecentricPolynomial[3] = "k3";
            hv_CameraParamAreaScanTelecentricPolynomial[4] = "p1";
            hv_CameraParamAreaScanTelecentricPolynomial[5] = "p2";
            hv_CameraParamAreaScanTelecentricPolynomial[6] = "sx";
            hv_CameraParamAreaScanTelecentricPolynomial[7] = "sy";
            hv_CameraParamAreaScanTelecentricPolynomial[8] = "cx";
            hv_CameraParamAreaScanTelecentricPolynomial[9] = "cy";
            hv_CameraParamAreaScanTelecentricPolynomial[10] = "image_width";
            hv_CameraParamAreaScanTelecentricPolynomial[11] = "image_height";
            hv_CameraParamAreaScanTiltDivision = new HTuple();
            hv_CameraParamAreaScanTiltDivision[0] = "focus";
            hv_CameraParamAreaScanTiltDivision[1] = "kappa";
            hv_CameraParamAreaScanTiltDivision[2] = "image_plane_dist";
            hv_CameraParamAreaScanTiltDivision[3] = "tilt";
            hv_CameraParamAreaScanTiltDivision[4] = "rot";
            hv_CameraParamAreaScanTiltDivision[5] = "sx";
            hv_CameraParamAreaScanTiltDivision[6] = "sy";
            hv_CameraParamAreaScanTiltDivision[7] = "cx";
            hv_CameraParamAreaScanTiltDivision[8] = "cy";
            hv_CameraParamAreaScanTiltDivision[9] = "image_width";
            hv_CameraParamAreaScanTiltDivision[10] = "image_height";
            hv_CameraParamAreaScanTiltPolynomial = new HTuple();
            hv_CameraParamAreaScanTiltPolynomial[0] = "focus";
            hv_CameraParamAreaScanTiltPolynomial[1] = "k1";
            hv_CameraParamAreaScanTiltPolynomial[2] = "k2";
            hv_CameraParamAreaScanTiltPolynomial[3] = "k3";
            hv_CameraParamAreaScanTiltPolynomial[4] = "p1";
            hv_CameraParamAreaScanTiltPolynomial[5] = "p2";
            hv_CameraParamAreaScanTiltPolynomial[6] = "image_plane_dist";
            hv_CameraParamAreaScanTiltPolynomial[7] = "tilt";
            hv_CameraParamAreaScanTiltPolynomial[8] = "rot";
            hv_CameraParamAreaScanTiltPolynomial[9] = "sx";
            hv_CameraParamAreaScanTiltPolynomial[10] = "sy";
            hv_CameraParamAreaScanTiltPolynomial[11] = "cx";
            hv_CameraParamAreaScanTiltPolynomial[12] = "cy";
            hv_CameraParamAreaScanTiltPolynomial[13] = "image_width";
            hv_CameraParamAreaScanTiltPolynomial[14] = "image_height";
            hv_CameraParamAreaScanImageSideTelecentricTiltDivision = new HTuple();
            hv_CameraParamAreaScanImageSideTelecentricTiltDivision[0] = "focus";
            hv_CameraParamAreaScanImageSideTelecentricTiltDivision[1] = "kappa";
            hv_CameraParamAreaScanImageSideTelecentricTiltDivision[2] = "tilt";
            hv_CameraParamAreaScanImageSideTelecentricTiltDivision[3] = "rot";
            hv_CameraParamAreaScanImageSideTelecentricTiltDivision[4] = "sx";
            hv_CameraParamAreaScanImageSideTelecentricTiltDivision[5] = "sy";
            hv_CameraParamAreaScanImageSideTelecentricTiltDivision[6] = "cx";
            hv_CameraParamAreaScanImageSideTelecentricTiltDivision[7] = "cy";
            hv_CameraParamAreaScanImageSideTelecentricTiltDivision[8] = "image_width";
            hv_CameraParamAreaScanImageSideTelecentricTiltDivision[9] = "image_height";
            hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial = new HTuple();
            hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial[0] = "focus";
            hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial[1] = "k1";
            hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial[2] = "k2";
            hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial[3] = "k3";
            hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial[4] = "p1";
            hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial[5] = "p2";
            hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial[6] = "tilt";
            hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial[7] = "rot";
            hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial[8] = "sx";
            hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial[9] = "sy";
            hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial[10] = "cx";
            hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial[11] = "cy";
            hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial[12] = "image_width";
            hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial[13] = "image_height";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivision = new HTuple();
            hv_CameraParamAreaScanBilateralTelecentricTiltDivision[0] = "magnification";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivision[1] = "kappa";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivision[2] = "tilt";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivision[3] = "rot";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivision[4] = "sx";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivision[5] = "sy";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivision[6] = "cx";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivision[7] = "cy";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivision[8] = "image_width";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivision[9] = "image_height";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial = new HTuple();
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial[0] = "magnification";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial[1] = "k1";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial[2] = "k2";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial[3] = "k3";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial[4] = "p1";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial[5] = "p2";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial[6] = "tilt";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial[7] = "rot";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial[8] = "sx";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial[9] = "sy";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial[10] = "cx";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial[11] = "cy";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial[12] = "image_width";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial[13] = "image_height";
            hv_CameraParamAreaScanObjectSideTelecentricTiltDivision = new HTuple();
            hv_CameraParamAreaScanObjectSideTelecentricTiltDivision[0] = "magnification";
            hv_CameraParamAreaScanObjectSideTelecentricTiltDivision[1] = "kappa";
            hv_CameraParamAreaScanObjectSideTelecentricTiltDivision[2] = "image_plane_dist";
            hv_CameraParamAreaScanObjectSideTelecentricTiltDivision[3] = "tilt";
            hv_CameraParamAreaScanObjectSideTelecentricTiltDivision[4] = "rot";
            hv_CameraParamAreaScanObjectSideTelecentricTiltDivision[5] = "sx";
            hv_CameraParamAreaScanObjectSideTelecentricTiltDivision[6] = "sy";
            hv_CameraParamAreaScanObjectSideTelecentricTiltDivision[7] = "cx";
            hv_CameraParamAreaScanObjectSideTelecentricTiltDivision[8] = "cy";
            hv_CameraParamAreaScanObjectSideTelecentricTiltDivision[9] = "image_width";
            hv_CameraParamAreaScanObjectSideTelecentricTiltDivision[10] = "image_height";
            hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial = new HTuple();
            hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial[0] = "magnification";
            hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial[1] = "k1";
            hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial[2] = "k2";
            hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial[3] = "k3";
            hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial[4] = "p1";
            hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial[5] = "p2";
            hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial[6] = "image_plane_dist";
            hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial[7] = "tilt";
            hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial[8] = "rot";
            hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial[9] = "sx";
            hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial[10] = "sy";
            hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial[11] = "cx";
            hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial[12] = "cy";
            hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial[13] = "image_width";
            hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial[14] = "image_height";
            hv_CameraParamLinesScan = new HTuple();
            hv_CameraParamLinesScan[0] = "focus";
            hv_CameraParamLinesScan[1] = "kappa";
            hv_CameraParamLinesScan[2] = "sx";
            hv_CameraParamLinesScan[3] = "sy";
            hv_CameraParamLinesScan[4] = "cx";
            hv_CameraParamLinesScan[5] = "cy";
            hv_CameraParamLinesScan[6] = "image_width";
            hv_CameraParamLinesScan[7] = "image_height";
            hv_CameraParamLinesScan[8] = "vx";
            hv_CameraParamLinesScan[9] = "vy";
            hv_CameraParamLinesScan[10] = "vz";
            //Legacy parameter names
            hv_CameraParamAreaScanTiltDivisionLegacy = new HTuple();
            hv_CameraParamAreaScanTiltDivisionLegacy[0] = "focus";
            hv_CameraParamAreaScanTiltDivisionLegacy[1] = "kappa";
            hv_CameraParamAreaScanTiltDivisionLegacy[2] = "tilt";
            hv_CameraParamAreaScanTiltDivisionLegacy[3] = "rot";
            hv_CameraParamAreaScanTiltDivisionLegacy[4] = "sx";
            hv_CameraParamAreaScanTiltDivisionLegacy[5] = "sy";
            hv_CameraParamAreaScanTiltDivisionLegacy[6] = "cx";
            hv_CameraParamAreaScanTiltDivisionLegacy[7] = "cy";
            hv_CameraParamAreaScanTiltDivisionLegacy[8] = "image_width";
            hv_CameraParamAreaScanTiltDivisionLegacy[9] = "image_height";
            hv_CameraParamAreaScanTiltPolynomialLegacy = new HTuple();
            hv_CameraParamAreaScanTiltPolynomialLegacy[0] = "focus";
            hv_CameraParamAreaScanTiltPolynomialLegacy[1] = "k1";
            hv_CameraParamAreaScanTiltPolynomialLegacy[2] = "k2";
            hv_CameraParamAreaScanTiltPolynomialLegacy[3] = "k3";
            hv_CameraParamAreaScanTiltPolynomialLegacy[4] = "p1";
            hv_CameraParamAreaScanTiltPolynomialLegacy[5] = "p2";
            hv_CameraParamAreaScanTiltPolynomialLegacy[6] = "tilt";
            hv_CameraParamAreaScanTiltPolynomialLegacy[7] = "rot";
            hv_CameraParamAreaScanTiltPolynomialLegacy[8] = "sx";
            hv_CameraParamAreaScanTiltPolynomialLegacy[9] = "sy";
            hv_CameraParamAreaScanTiltPolynomialLegacy[10] = "cx";
            hv_CameraParamAreaScanTiltPolynomialLegacy[11] = "cy";
            hv_CameraParamAreaScanTiltPolynomialLegacy[12] = "image_width";
            hv_CameraParamAreaScanTiltPolynomialLegacy[13] = "image_height";
            hv_CameraParamAreaScanTelecentricDivisionLegacy = new HTuple();
            hv_CameraParamAreaScanTelecentricDivisionLegacy[0] = "focus";
            hv_CameraParamAreaScanTelecentricDivisionLegacy[1] = "kappa";
            hv_CameraParamAreaScanTelecentricDivisionLegacy[2] = "sx";
            hv_CameraParamAreaScanTelecentricDivisionLegacy[3] = "sy";
            hv_CameraParamAreaScanTelecentricDivisionLegacy[4] = "cx";
            hv_CameraParamAreaScanTelecentricDivisionLegacy[5] = "cy";
            hv_CameraParamAreaScanTelecentricDivisionLegacy[6] = "image_width";
            hv_CameraParamAreaScanTelecentricDivisionLegacy[7] = "image_height";
            hv_CameraParamAreaScanTelecentricPolynomialLegacy = new HTuple();
            hv_CameraParamAreaScanTelecentricPolynomialLegacy[0] = "focus";
            hv_CameraParamAreaScanTelecentricPolynomialLegacy[1] = "k1";
            hv_CameraParamAreaScanTelecentricPolynomialLegacy[2] = "k2";
            hv_CameraParamAreaScanTelecentricPolynomialLegacy[3] = "k3";
            hv_CameraParamAreaScanTelecentricPolynomialLegacy[4] = "p1";
            hv_CameraParamAreaScanTelecentricPolynomialLegacy[5] = "p2";
            hv_CameraParamAreaScanTelecentricPolynomialLegacy[6] = "sx";
            hv_CameraParamAreaScanTelecentricPolynomialLegacy[7] = "sy";
            hv_CameraParamAreaScanTelecentricPolynomialLegacy[8] = "cx";
            hv_CameraParamAreaScanTelecentricPolynomialLegacy[9] = "cy";
            hv_CameraParamAreaScanTelecentricPolynomialLegacy[10] = "image_width";
            hv_CameraParamAreaScanTelecentricPolynomialLegacy[11] = "image_height";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivisionLegacy = new HTuple();
            hv_CameraParamAreaScanBilateralTelecentricTiltDivisionLegacy[0] = "focus";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivisionLegacy[1] = "kappa";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivisionLegacy[2] = "tilt";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivisionLegacy[3] = "rot";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivisionLegacy[4] = "sx";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivisionLegacy[5] = "sy";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivisionLegacy[6] = "cx";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivisionLegacy[7] = "cy";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivisionLegacy[8] = "image_width";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivisionLegacy[9] = "image_height";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy = new HTuple();
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy[0] = "focus";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy[1] = "k1";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy[2] = "k2";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy[3] = "k3";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy[4] = "p1";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy[5] = "p2";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy[6] = "tilt";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy[7] = "rot";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy[8] = "sx";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy[9] = "sy";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy[10] = "cx";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy[11] = "cy";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy[12] = "image_width";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy[13] = "image_height";
            //
            //If the camera type is passed in CameraParam
            if ((int)((new HTuple((new HTuple(hv_CameraParam.TupleLength())).TupleEqual(1))).TupleAnd(
                ((hv_CameraParam.TupleSelect(0))).TupleIsString())) != 0)
            {
                hv_CameraType = hv_CameraParam.TupleSelect(0);
                if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_division"))) != 0)
                {
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanDivision);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_polynomial"))) != 0)
                {
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanPolynomial);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_telecentric_division"))) != 0)
                {
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanTelecentricDivision);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_telecentric_polynomial"))) != 0)
                {
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanTelecentricPolynomial);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_tilt_division"))) != 0)
                {
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanTiltDivision);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_tilt_polynomial"))) != 0)
                {
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanTiltPolynomial);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_tilt_image_side_telecentric_division"))) != 0)
                {
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanImageSideTelecentricTiltDivision);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_tilt_image_side_telecentric_polynomial"))) != 0)
                {
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_tilt_bilateral_telecentric_division"))) != 0)
                {
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanBilateralTelecentricTiltDivision);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_tilt_bilateral_telecentric_polynomial"))) != 0)
                {
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_tilt_object_side_telecentric_division"))) != 0)
                {
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanObjectSideTelecentricTiltDivision);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_tilt_object_side_telecentric_polynomial"))) != 0)
                {
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("line_scan"))) != 0)
                {
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamLinesScan);
                }
                else
                {
                    throw new HalconException(("Unknown camera type '" + hv_CameraType) + "' passed in CameraParam.");
                }

                return;
            }
            //
            //If the camera parameters are passed in CameraParam
            if ((int)(((((hv_CameraParam.TupleSelect(0))).TupleIsString())).TupleNot()) != 0)
            {
                //Format of camera parameters for HALCON 12 and earlier
                switch ((new HTuple(hv_CameraParam.TupleLength()
                    )).I)
                {
                    //
                    //Area Scan
                    case 8:
                        //CameraType: 'area_scan_division' or 'area_scan_telecentric_division'
                        if ((int)(new HTuple(((hv_CameraParam.TupleSelect(0))).TupleNotEqual(0.0))) != 0)
                        {
                            hv_ParamNames = hv_CameraParamAreaScanDivision.Clone();
                            hv_CameraType = "area_scan_division";
                        }
                        else
                        {
                            hv_ParamNames = hv_CameraParamAreaScanTelecentricDivisionLegacy.Clone();
                            hv_CameraType = "area_scan_telecentric_division";
                        }
                        break;

                    case 10:
                        //CameraType: 'area_scan_tilt_division' or 'area_scan_telecentric_tilt_division'
                        if ((int)(new HTuple(((hv_CameraParam.TupleSelect(0))).TupleNotEqual(0.0))) != 0)
                        {
                            hv_ParamNames = hv_CameraParamAreaScanTiltDivisionLegacy.Clone();
                            hv_CameraType = "area_scan_tilt_division";
                        }
                        else
                        {
                            hv_ParamNames = hv_CameraParamAreaScanBilateralTelecentricTiltDivisionLegacy.Clone();
                            hv_CameraType = "area_scan_tilt_bilateral_telecentric_division";
                        }
                        break;

                    case 12:
                        //CameraType: 'area_scan_polynomial' or 'area_scan_telecentric_polynomial'
                        if ((int)(new HTuple(((hv_CameraParam.TupleSelect(0))).TupleNotEqual(0.0))) != 0)
                        {
                            hv_ParamNames = hv_CameraParamAreaScanPolynomial.Clone();
                            hv_CameraType = "area_scan_polynomial";
                        }
                        else
                        {
                            hv_ParamNames = hv_CameraParamAreaScanTelecentricPolynomialLegacy.Clone();
                            hv_CameraType = "area_scan_telecentric_polynomial";
                        }
                        break;

                    case 14:
                        //CameraType: 'area_scan_tilt_polynomial' or 'area_scan_telecentric_tilt_polynomial'
                        if ((int)(new HTuple(((hv_CameraParam.TupleSelect(0))).TupleNotEqual(0.0))) != 0)
                        {
                            hv_ParamNames = hv_CameraParamAreaScanTiltPolynomialLegacy.Clone();
                            hv_CameraType = "area_scan_tilt_polynomial";
                        }
                        else
                        {
                            hv_ParamNames = hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy.Clone();
                            hv_CameraType = "area_scan_tilt_bilateral_telecentric_polynomial";
                        }
                        break;
                    //
                    //Line Scan
                    case 11:
                        //CameraType: 'line_scan'
                        hv_ParamNames = hv_CameraParamLinesScan.Clone();
                        hv_CameraType = "line_scan";
                        break;

                    default:
                        throw new HalconException("Wrong number of values in CameraParam.");
                }
            }
            else
            {
                //Format of camera parameters since HALCON 13
                hv_CameraType = hv_CameraParam.TupleSelect(0);
                if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_division"))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_CameraParam.TupleLength())).TupleNotEqual(
                        9))) != 0)
                    {
                        throw new HalconException("Wrong number of values in CameraParam.");
                    }
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanDivision);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_polynomial"))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_CameraParam.TupleLength())).TupleNotEqual(
                        13))) != 0)
                    {
                        throw new HalconException("Wrong number of values in CameraParam.");
                    }
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanPolynomial);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_telecentric_division"))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_CameraParam.TupleLength())).TupleNotEqual(
                        9))) != 0)
                    {
                        throw new HalconException("Wrong number of values in CameraParam.");
                    }
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanTelecentricDivision);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_telecentric_polynomial"))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_CameraParam.TupleLength())).TupleNotEqual(
                        13))) != 0)
                    {
                        throw new HalconException("Wrong number of values in CameraParam.");
                    }
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanTelecentricPolynomial);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_tilt_division"))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_CameraParam.TupleLength())).TupleNotEqual(
                        12))) != 0)
                    {
                        throw new HalconException("Wrong number of values in CameraParam.");
                    }
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanTiltDivision);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_tilt_polynomial"))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_CameraParam.TupleLength())).TupleNotEqual(
                        16))) != 0)
                    {
                        throw new HalconException("Wrong number of values in CameraParam.");
                    }
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanTiltPolynomial);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_tilt_image_side_telecentric_division"))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_CameraParam.TupleLength())).TupleNotEqual(
                        11))) != 0)
                    {
                        throw new HalconException("Wrong number of values in CameraParam.");
                    }
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanImageSideTelecentricTiltDivision);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_tilt_image_side_telecentric_polynomial"))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_CameraParam.TupleLength())).TupleNotEqual(
                        15))) != 0)
                    {
                        throw new HalconException("Wrong number of values in CameraParam.");
                    }
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_tilt_bilateral_telecentric_division"))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_CameraParam.TupleLength())).TupleNotEqual(
                        11))) != 0)
                    {
                        throw new HalconException("Wrong number of values in CameraParam.");
                    }
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanBilateralTelecentricTiltDivision);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_tilt_bilateral_telecentric_polynomial"))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_CameraParam.TupleLength())).TupleNotEqual(
                        15))) != 0)
                    {
                        throw new HalconException("Wrong number of values in CameraParam.");
                    }
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_tilt_object_side_telecentric_division"))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_CameraParam.TupleLength())).TupleNotEqual(
                        12))) != 0)
                    {
                        throw new HalconException("Wrong number of values in CameraParam.");
                    }
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanObjectSideTelecentricTiltDivision);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_tilt_object_side_telecentric_polynomial"))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_CameraParam.TupleLength())).TupleNotEqual(
                        16))) != 0)
                    {
                        throw new HalconException("Wrong number of values in CameraParam.");
                    }
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("line_scan"))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_CameraParam.TupleLength())).TupleNotEqual(
                        12))) != 0)
                    {
                        throw new HalconException("Wrong number of values in CameraParam.");
                    }
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamLinesScan);
                }
                else
                {
                    throw new HalconException("Unknown camera type in CameraParam.");
                }
            }

            return;
        }
        /// <summary>
        /// 获取相机参数
        /// </summary>
        /// <param name="hv_CameraParam"></param>
        /// <param name="hv_ParamName"></param>
        /// <param name="hv_ParamValue"></param>
        /// <exception cref="HalconException"></exception>
        // Chapter: Calibration / Camera Parameters
        // Short Description: Get the value of a specified camera parameter from the camera parameter tuple.
        public static void Get_cam_par_data(HTuple hv_CameraParam, HTuple hv_ParamName, out HTuple hv_ParamValue)
        {
            // Local iconic variables

            // Local control variables

            HTuple hv_CameraType = null, hv_CameraParamNames = null;
            HTuple hv_Index = null, hv_ParamNameInd = new HTuple();
            HTuple hv_I = new HTuple();
            // Initialize local and output iconic variables
            //get_cam_par_data returns in ParamValue the value of the
            //parameter that is given in ParamName from the tuple of
            //camera parameters that is given in CameraParam.
            //
            //Get the parameter names that correspond to the
            //elements in the input camera parameter tuple.
            get_cam_par_names(hv_CameraParam, out hv_CameraType, out hv_CameraParamNames);
            //
            //Find the index of the requested camera data and return
            //the corresponding value.
            hv_ParamValue = new HTuple();
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_ParamName.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
            {
                hv_ParamNameInd = hv_ParamName.TupleSelect(hv_Index);
                if ((int)(new HTuple(hv_ParamNameInd.TupleEqual("camera_type"))) != 0)
                {
                    hv_ParamValue = hv_ParamValue.TupleConcat(hv_CameraType);
                    continue;
                }
                hv_I = hv_CameraParamNames.TupleFind(hv_ParamNameInd);
                if ((int)(new HTuple(hv_I.TupleNotEqual(-1))) != 0)
                {
                    hv_ParamValue = hv_ParamValue.TupleConcat(hv_CameraParam.TupleSelect(hv_I));
                }
                else
                {
                    throw new HalconException("Unknown camera parameter " + hv_ParamNameInd);
                }
            }

            return;
        }

        /// <summary>
        /// 获取3D位置坐标
        /// </summary>
        /// <param name="hv_CamParam"></param>
        /// <param name="hv_Pose"></param>
        /// <param name="hv_CoordAxesLength"></param>
        /// <param name="hv_WindowHandle"></param>
        /// <returns></returns>
        // Chapter: Graphics / Output
        // Short Description: Display the axes of a 3d coordinate system
        public static HObject Disp3DCoordSystem(HTuple hv_CamParam, HTuple hv_Pose,
            HTuple hv_CoordAxesLength, HTuple hv_WindowHandle = null)
        {
            // Local iconic variables
            // Local control variables
            HTuple hv_CameraType = null, hv_IsTelecentric = null;
            HTuple hv_TransWorld2Cam = null, hv_OrigCamX = null, hv_OrigCamY = null;
            HTuple hv_OrigCamZ = null, hv_Row0 = null, hv_Column0 = null;
            HTuple hv_X = null, hv_Y = null, hv_Z = null, hv_RowAxX = null;
            HTuple hv_ColumnAxX = null, hv_RowAxY = null, hv_ColumnAxY = null;
            HTuple hv_RowAxZ = null, hv_ColumnAxZ = null, hv_Distance = null;
            HTuple hv_HeadLength = null;
            // Initialize local and output iconic variables

            try
            {
                //This procedure displays a 3D coordinate system.
                //It needs the procedure gen_arrow_contour_xld.
                //
                //Input parameters:
                //WindowHandle: The window where the coordinate system shall be displayed
                //CamParam: The camera paramters
                //Pose: The pose to be displayed
                //CoordAxesLength: The length of the coordinate axes in world coordinates
                //
                //Check, if Pose is a correct pose tuple.
                if ((int)(new HTuple((new HTuple(hv_Pose.TupleLength())).TupleNotEqual(7))) != 0)
                {
                    //ho_Arrows.Dispose();

                    return new HObject();
                }
                Get_cam_par_data(hv_CamParam, "camera_type", out hv_CameraType);
                hv_IsTelecentric = new HTuple(((hv_CameraType.TupleStrstr("telecentric"))).TupleNotEqual(
                    -1));
                if ((int)((new HTuple(((hv_Pose.TupleSelect(2))).TupleEqual(0.0))).TupleAnd(
                    hv_IsTelecentric.TupleNot())) != 0)
                {
                    //For projective cameras:
                    //Poses with Z position zero cannot be projected
                    //(that would lead to a division by zero error).
                    //ho_Arrows.Dispose();
                    return new HObject();
                }
                //Convert to pose to a transformation matrix
                HOperatorSet.PoseToHomMat3d(hv_Pose, out hv_TransWorld2Cam);
                //Project the world origin into the image
                HOperatorSet.AffineTransPoint3d(hv_TransWorld2Cam, 0, 0, 0, out hv_OrigCamX,
                    out hv_OrigCamY, out hv_OrigCamZ);
                HOperatorSet.Project3dPoint(hv_OrigCamX, hv_OrigCamY, hv_OrigCamZ, hv_CamParam,
                    out hv_Row0, out hv_Column0);
                //Project the coordinate axes into the image
                HOperatorSet.AffineTransPoint3d(hv_TransWorld2Cam, hv_CoordAxesLength, 0, 0,
                    out hv_X, out hv_Y, out hv_Z);
                HOperatorSet.Project3dPoint(hv_X, hv_Y, hv_Z, hv_CamParam, out hv_RowAxX, out hv_ColumnAxX);
                HOperatorSet.AffineTransPoint3d(hv_TransWorld2Cam, 0, hv_CoordAxesLength, 0,
                    out hv_X, out hv_Y, out hv_Z);
                HOperatorSet.Project3dPoint(hv_X, hv_Y, hv_Z, hv_CamParam, out hv_RowAxY, out hv_ColumnAxY);
                HOperatorSet.AffineTransPoint3d(hv_TransWorld2Cam, 0, 0, hv_CoordAxesLength,
                    out hv_X, out hv_Y, out hv_Z);
                HOperatorSet.Project3dPoint(hv_X, hv_Y, hv_Z, hv_CamParam, out hv_RowAxZ, out hv_ColumnAxZ);
                //
                //Generate an XLD contour for each axis
                HOperatorSet.DistancePp(((hv_Row0.TupleConcat(hv_Row0))).TupleConcat(hv_Row0),
                    ((hv_Column0.TupleConcat(hv_Column0))).TupleConcat(hv_Column0), ((hv_RowAxX.TupleConcat(
                    hv_RowAxY))).TupleConcat(hv_RowAxZ), ((hv_ColumnAxX.TupleConcat(hv_ColumnAxY))).TupleConcat(
                    hv_ColumnAxZ), out hv_Distance);
                hv_HeadLength = (((((((hv_Distance.TupleMax()) / 12.0)).TupleConcat(5.0))).TupleMax()
                    )).TupleInt();

                //Vision.Gen_arrow_contour_xld(out ho_Arrows, ((hv_Row0.TupleConcat(hv_Row0))).TupleConcat(
                //      hv_Row0), ((hv_Column0.TupleConcat(hv_Column0))).TupleConcat(hv_Column0),
                //      ((hv_RowAxX.TupleConcat(hv_RowAxY))).TupleConcat(hv_RowAxZ), ((hv_ColumnAxX.TupleConcat(
                //      hv_ColumnAxY))).TupleConcat(hv_ColumnAxZ), hv_HeadLength, hv_HeadLength);

                Gen_arrow_contour_xld(out HObject ho_ArrowsX, hv_Row0, hv_Column0, hv_RowAxX, hv_ColumnAxX, hv_HeadLength, hv_HeadLength);
                Gen_arrow_contour_xld(out HObject ho_ArrowsY, hv_Row0, hv_Column0, hv_RowAxY, hv_ColumnAxY, hv_HeadLength, hv_HeadLength);
                Gen_arrow_contour_xld(out HObject ho_ArrowsZ, hv_Row0, hv_Column0, hv_RowAxZ, hv_ColumnAxZ, hv_HeadLength, hv_HeadLength);

                if (hv_WindowHandle != null)
                {
                    HOperatorSet.SetColor(hv_WindowHandle, "red");
                    HOperatorSet.DispObj(ho_ArrowsX, hv_WindowHandle);
                    HOperatorSet.SetColor(hv_WindowHandle, "green");
                    HOperatorSet.DispObj(ho_ArrowsY, hv_WindowHandle);
                    HOperatorSet.SetColor(hv_WindowHandle, "blue");
                    HOperatorSet.DispObj(ho_ArrowsZ, hv_WindowHandle);
                    Disp_message(hv_WindowHandle, "X", hv_RowAxX + 3, hv_ColumnAxX + 3, false,
              "red", "box");
                    Disp_message(hv_WindowHandle, "Y", hv_RowAxY + 3, hv_ColumnAxY + 3, false,
                        "green", "box");
                    Disp_message(hv_WindowHandle, "Z", hv_RowAxZ + 3, (hv_ColumnAxZ + 3) + 3, false,
                       "blue", "box");
                }

                return ho_ArrowsX.ConcatObj(ho_ArrowsY).ConcatObj(ho_ArrowsZ);
            }
            catch (HalconException HDevExpDefaultException)
            {
                //ho_Arrows.Dispose();

                throw HDevExpDefaultException;
            }
        }
        /// <summary>
        /// 获取坐标
        /// </summary>
        /// <param name="hv_CamParam">相机内参</param>
        /// <param name="hv_Pose">坐标</param>
        /// <param name="hv_CoordAxesLength">轴长</param>
        /// <param name="ho_ArrowsX">X箭头区域</param>
        /// <param name="ho_ArrowsY">Y</param>
        /// <param name="ho_ArrowsZ">Z</param>
        /// <param name="halconRun">显示</param>
        public static void Disp3DCoordSystem(HTuple hv_CamParam, HTuple hv_Pose,
     HTuple hv_CoordAxesLength, out HObject ho_ArrowsX, out HObject ho_ArrowsY, out HObject ho_ArrowsZ, HWindID halconRun = null)
        {
            // Local iconic variables
            // Local control variables
            HTuple hv_CameraType = null, hv_IsTelecentric = null;
            HTuple hv_TransWorld2Cam = null, hv_OrigCamX = null, hv_OrigCamY = null;
            HTuple hv_OrigCamZ = null, hv_Row0 = null, hv_Column0 = null;
            HTuple hv_X = null, hv_Y = null, hv_Z = null, hv_RowAxX = null;
            HTuple hv_ColumnAxX = null, hv_RowAxY = null, hv_ColumnAxY = null;
            HTuple hv_RowAxZ = null, hv_ColumnAxZ = null, hv_Distance = null;
            HTuple hv_HeadLength = null;
            ho_ArrowsX = new HObject();
            ho_ArrowsY = new HObject();
            ho_ArrowsZ = new HObject();
            // Initialize local and output iconic variables
            try
            {
                //
                //Check, if Pose is a correct pose tuple.
                if ((int)(new HTuple((new HTuple(hv_Pose.TupleLength())).TupleNotEqual(7))) != 0)
                {
                    //ho_Arrows.Dispose();
                    return;
                }
                Get_cam_par_data(hv_CamParam, "camera_type", out hv_CameraType);
                hv_IsTelecentric = new HTuple(((hv_CameraType.TupleStrstr("telecentric"))).TupleNotEqual(
                    -1));
                if ((int)((new HTuple(((hv_Pose.TupleSelect(2))).TupleEqual(0.0))).TupleAnd(
                    hv_IsTelecentric.TupleNot())) != 0)
                {
                    //For projective cameras:
                    //Poses with Z position zero cannot be projected
                    //(that would lead to a division by zero error).
                    //ho_Arrows.Dispose();
                    return;
                }
                //Convert to pose to a transformation matrix
                HOperatorSet.PoseToHomMat3d(hv_Pose, out hv_TransWorld2Cam);
                //Project the world origin into the image
                HOperatorSet.AffineTransPoint3d(hv_TransWorld2Cam, 0, 0, 0, out hv_OrigCamX,
                    out hv_OrigCamY, out hv_OrigCamZ);
                HOperatorSet.Project3dPoint(hv_OrigCamX, hv_OrigCamY, hv_OrigCamZ, hv_CamParam,
                    out hv_Row0, out hv_Column0);
                //Project the coordinate axes into the image
                HOperatorSet.AffineTransPoint3d(hv_TransWorld2Cam, hv_CoordAxesLength, 0, 0,
                    out hv_X, out hv_Y, out hv_Z);
                HOperatorSet.Project3dPoint(hv_X, hv_Y, hv_Z, hv_CamParam, out hv_RowAxX, out hv_ColumnAxX);
                HOperatorSet.AffineTransPoint3d(hv_TransWorld2Cam, 0, hv_CoordAxesLength, 0,
                    out hv_X, out hv_Y, out hv_Z);
                HOperatorSet.Project3dPoint(hv_X, hv_Y, hv_Z, hv_CamParam, out hv_RowAxY, out hv_ColumnAxY);
                HOperatorSet.AffineTransPoint3d(hv_TransWorld2Cam, 0, 0, hv_CoordAxesLength,
                    out hv_X, out hv_Y, out hv_Z);
                HOperatorSet.Project3dPoint(hv_X, hv_Y, hv_Z, hv_CamParam, out hv_RowAxZ, out hv_ColumnAxZ);
                //
                //Generate an XLD contour for each axis
                HOperatorSet.DistancePp(((hv_Row0.TupleConcat(hv_Row0))).TupleConcat(hv_Row0),
                    ((hv_Column0.TupleConcat(hv_Column0))).TupleConcat(hv_Column0), ((hv_RowAxX.TupleConcat(
                    hv_RowAxY))).TupleConcat(hv_RowAxZ), ((hv_ColumnAxX.TupleConcat(hv_ColumnAxY))).TupleConcat(
                    hv_ColumnAxZ), out hv_Distance);
                hv_HeadLength = (((((((hv_Distance.TupleMax()) / 12.0)).TupleConcat(5.0))).TupleMax()
                    )).TupleInt();
                Gen_arrow_contour_xld(out ho_ArrowsX, hv_Row0, hv_Column0, hv_RowAxX, hv_ColumnAxX, hv_HeadLength, hv_HeadLength);
                Gen_arrow_contour_xld(out ho_ArrowsY, hv_Row0, hv_Column0, hv_RowAxY, hv_ColumnAxY, hv_HeadLength, hv_HeadLength);
                Gen_arrow_contour_xld(out ho_ArrowsZ, hv_Row0, hv_Column0, hv_RowAxZ, hv_ColumnAxZ, hv_HeadLength, hv_HeadLength);
                if (halconRun != null)
                {
                    halconRun.GetOneImageR().AddImageMassage(hv_RowAxX + 3, hv_ColumnAxX + 3, "X", ColorResult.red);
                    halconRun.GetOneImageR().AddImageMassage(hv_RowAxY + 3, hv_ColumnAxY + 3, "Y", ColorResult.green);
                    halconRun.GetOneImageR().AddImageMassage(hv_RowAxZ + 3, hv_ColumnAxZ + 3, "Z", ColorResult.yellow);
                }
            }
            catch (HalconException HDevExpDefaultException)
            {
                //ho_Arrows.Dispose();
                throw HDevExpDefaultException;
            }
        }

        /// <summary>
        /// 根据矩形中心点,创建直线偏移2个点位
        /// </summary>
        /// <param name="row">中心r</param>
        /// <param name="column">中心c</param>
        /// <param name="phi">角度</param>
        /// <param name="length1">长度</param>
        /// <param name="rows1">点1r</param>
        /// <param name="columns1">点1c</param>
        /// <param name="rows2"></param>
        /// <param name="columns2"></param>
        public static void gen_rectangle2_line_point(HTuple row, HTuple column, HTuple phi, HTuple length1, out HTuple rows1, out HTuple columns1, out HTuple rows2, out HTuple columns2)
        {
            HTuple row2 = new HTuple();
            HTuple row3 = new HTuple();
            HTuple column2 = new HTuple();
            HTuple column3 = new HTuple();
            rows1 = new HTuple();
            columns1 = new HTuple();
            rows2 = new HTuple();
            columns2 = new HTuple();
            for (int i = 0; i < row.Length; i++)
            {
                HTuple homd = new HTuple();
                HOperatorSet.HomMat2dIdentity(out homd);
                HOperatorSet.HomMat2dRotate(homd, phi[i], 0, 0, out homd);
                HOperatorSet.HomMat2dTranslate(homd, row[i], column[i], out homd);
                HTuple rowt;
                HTuple columnt;
                HOperatorSet.AffineTransPixel(homd, new HTuple(new int[2]), new HTuple(new HTuple[] { -length1.TupleSelect(i) / 2, length1.TupleSelect(i) / 2 }), out rowt, out columnt);
                rows1.Append(rowt[0]);
                columns1.Append(columnt[0]);
                rows2.Append(rowt[1]);
                columns2.Append(columnt[1]);
            }
        }
        /// <summary>
        /// 跟随矩形变换坐标XY
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="phi"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="rows1"></param>
        /// <param name="columns1"></param>
        public static void Gen_Rectangle2_XYPoint(HTuple row, HTuple column, HTuple phi, HTuple x, HTuple y, out HTuple rows1, out HTuple columns1)
        {
            HTuple row2 = new HTuple();
            HTuple row3 = new HTuple();
            HTuple column2 = new HTuple();
            HTuple column3 = new HTuple();
            rows1 = new HTuple();
            columns1 = new HTuple();
            try
            {
                for (int i = 0; i < x.Length; i++)
                {
                    HTuple homd = new HTuple();
                    HOperatorSet.HomMat2dIdentity(out homd);
                    HOperatorSet.HomMat2dRotate(homd, phi, 0, 0, out homd);
                    HOperatorSet.HomMat2dTranslate(homd, row, column, out homd);
                    HTuple rowt;
                    HTuple columnt;
                    HOperatorSet.AffineTransPoint2d(homd, y.TupleSelect(i), x.TupleSelect(i), out rowt, out columnt);
                    rows1.Append(rowt);
                    columns1.Append(columnt);
                }
            }
            catch (Exception)
            {
            }

        }


        /// <summary>
        /// 根据矩形生成4点坐标,
        /// </summary>
        /// <param name="row">中心r</param>
        /// <param name="column">中心c</param>
        /// <param name="phi">角度</param>
        /// <param name="length1">长度</param>
        /// <param name="rows1">点1r</param>
        /// <param name="columns1">点1c</param>
        /// <param name="rows2"></param>
        /// <param name="columns2"></param>
        public static void gen_rectangle2_point(HTuple row, HTuple column, HTuple phi, HTuple length1, HTuple length2, out HTuple rows1, out HTuple columns1)
        {
            rows1 = new HTuple();
            columns1 = new HTuple();
            if (length1 <= 0 || length2 <= 2)
            {
                return;
            }
            HTuple Hcos = phi.TupleCos();
            HTuple HSin = phi.TupleSin();
            for (int i = 0; i < row.Length; i++)
            {
                HTuple homd = new HTuple();
                HOperatorSet.HomMat2dIdentity(out homd);
                HOperatorSet.HomMat2dRotate(homd, phi[i], 0, 0, out homd);
                HOperatorSet.HomMat2dTranslate(homd, row[i], column[i], out homd);
                HTuple rowt;
                HTuple columnt;
                HOperatorSet.AffineTransPixel(homd, new HTuple(new HTuple[] { -length2.TupleSelect(i), -length2.TupleSelect(i), length2.TupleSelect(i), length2.TupleSelect(i) }),
                    new HTuple(new HTuple[]
                {     -length1.TupleSelect(i) ,  length1.TupleSelect(i),  length1.TupleSelect(i) , - length1.TupleSelect(i) }), out rowt, out columnt);
                rows1.Append(rowt);
                columns1.Append(columnt);
            }
        }
        /// <summary>
        /// 拉点测量距离
        /// </summary>
        public static void DPointsM(HWindID drawh)
        {
            try
            {
                drawh.Focus();
                HTuple hv_Button = null;
                HTuple hv_Row = null, hv_Column = null;
                if (drawh.Drawing)
                {
                    MessageBox.Show("当前绘制中,请结束绘制");
                    return;
                }
                drawh.DrawType = 2;
                drawh.DrawErasure = true;
                HObject brush_region_affine = new HObject();
                HObject final_region = new HObject();
                final_region.GenEmptyObj();
                brush_region_affine.GenEmptyObj();
                //drawh.Drawing = true;
                HOperatorSet.SetDraw(drawh.GetHWindowControl().HalconWindow, "fill");
                HOperatorSet.SetColor(drawh.GetHWindowControl().HalconWindow, "red");
                hv_Button = 0;
                HTuple rows2 = new HTuple();
                HTuple cols2 = new HTuple();
                HTuple rows = new HTuple();
                HTuple cols = new HTuple();
                HTuple mindists2 = new HTuple();
                HTuple mindists = new HTuple();
                HObject hObject = new HObject();
                HObject hObjectDT = new HObject();
                hObjectDT.GenEmptyObj();
                bool ButtMot = false;
                List<HObject> ListObjLines = new List<HObject>();
                List<HObject> ListObjCross = new List<HObject>();
                List<HTuple> ListHtuple = new List<HTuple>();
                List<HTuple> hTuplesRows = new List<HTuple>();
                List<HTuple> hTuplesCols = new List<HTuple>();
                // 4为鼠标右键
                while (hv_Button != 4)
                {
                    //一直在循环,需要让halcon控件也响应事件,不然到时候跳出循环,之前的事件会一起爆发触发,
                    //Application.DoEvents();
                    try
                    {
                        HOperatorSet.SetSystem("flush_graphic", "false");
                        HOperatorSet.ClearWindow(drawh.GetHWindowControl().HalconWindow);
                        HOperatorSet.GetMbuttonSubPix(drawh.GetHWindowControl().HalconWindow, out hv_Row, out hv_Column, out hv_Button);
                        try
                        {
                            HOperatorSet.SetSystem("flush_graphic", "true");
                            HOperatorSet.DispObj(drawh.Image(), drawh.GetHWindowControl().HalconWindow);
                            if (hv_Button == 1)
                            {
                                HOperatorSet.GenCrossContourXld(out final_region, hv_Row, hv_Column, 40, 0);
                                if (rows.Length == 0)
                                {
                                    HOperatorSet.GenCrossContourXld(out hObjectDT, hv_Row, hv_Column, 40, 0);
                                    rows = hv_Row;
                                    cols = hv_Column;
                                }
                                else
                                {
                                    rows2 = hv_Row;
                                    cols2 = hv_Column;
                                }
                                ListObjCross.Add(final_region);
                                if (rows2.Length != 0)
                                {
                                    HOperatorSet.DistancePp(rows, cols, rows2, cols2, out HTuple mindist);
                                    HOperatorSet.GenContourPolygonXld(out brush_region_affine, new HTuple(rows, rows2), new HTuple(cols, cols2));
                                    HOperatorSet.SetColor(drawh.GetHWindowControl().HalconWindow, "green");
                                    mindists2 = drawh.GetCaliConstMM(mindist);
                                    hTuplesRows.Add(rows2);
                                    hTuplesCols.Add(cols2);
                                    mindist = mindist + "Px";
                                    mindist.Append(mindists2 + "mm");
                                    mindists2 = mindist;
                                    ListObjLines.Add(brush_region_affine);
                                    ListHtuple.Add(mindists2);
                                }
                            }
                            else
                            {
                                ButtMot = false;
                                HOperatorSet.GenCrossContourXld(out hObject, hv_Row, hv_Column, 100, 0);
                                //HOperatorSet.SetColor(drawh.GetHWindowControl().HalconWindow, ColorResult.green.ToString());
                            }
                            HOperatorSet.SetColor(drawh.GetHWindowControl().HalconWindow, "red");
                            HOperatorSet.DispObj(hObject, drawh.GetHWindowControl().HalconWindow);
                            if (rows.Length == 1)
                            {
                                HOperatorSet.DistancePp(hv_Row, hv_Column, rows, cols, out mindists);
                                HOperatorSet.GenContourPolygonXld(out HObject controu, new HTuple(hv_Row, rows), new HTuple(hv_Column, cols));
                                HOperatorSet.DispObj(controu, drawh.GetHWindowControl().HalconWindow);
                                HTuple distMM = drawh.GetCaliConstMM(mindists);
                                Disp_message(drawh.GetHWindowControl().HalconWindow, new HTuple(mindists + "Px", distMM + "mm"), hv_Row, hv_Column + 20);
                            }
                            HOperatorSet.SetColor(drawh.GetHWindowControl().HalconWindow, ColorResult.blue.ToString());
                            for (int i = 0; i < ListObjCross.Count; i++)
                            {
                                HOperatorSet.DispObj(ListObjCross[i], drawh.GetHWindowControl().HalconWindow);
                            }
                            for (int i = 0; i < ListHtuple.Count; i++)
                            {
                                HOperatorSet.DispObj(ListObjLines[i], drawh.GetHWindowControl().HalconWindow);
                                Disp_message(drawh.GetHWindowControl().HalconWindow, ListHtuple[i], hTuplesRows[i] - 10, hTuplesCols[i]);
                            }
                            HOperatorSet.SetColor(drawh.GetHWindowControl().HalconWindow, ColorResult.green.ToString());
                            HOperatorSet.DispObj(hObjectDT, drawh.GetHWindowControl().HalconWindow);
                        }
                        catch (Exception ex)
                        {


                        }
                    }
                    catch (HalconException ex)
                    {
                        hv_Button = 0;
                    }
                }
            }
            catch (Exception)
            {
            }
            HOperatorSet.SetDraw(drawh.GetHWindowControl().HalconWindow, "margin");
            drawh.Drawing = false;
            drawh.DrawErasure = false;

        }
        /// <summary>
        /// 拉点测量坐标
        /// </summary>
        /// <param name="drawh">显示窗口</param>
        /// <param name="Hom2DMat">坐标矩阵</param>  
        public static void DPointsXY(HWindID drawh, HTuple homMat2D)
        {
            try
            {
                drawh.Focus();
                HTuple hv_Button = null;
                HTuple hv_Row = null, hv_Column = null;
                if (drawh.Drawing)
                {
                    MessageBox.Show("当前绘制中,请结束绘制");
                    return;
                }
                drawh.DrawType = 2;
                drawh.DrawErasure = true;
                HObject brush_region_affine = new HObject();
                HObject final_region = new HObject();
                final_region.GenEmptyObj();
                brush_region_affine.GenEmptyObj();
                //drawh.Drawing = true;
                HOperatorSet.SetDraw(drawh.GetHWindowControl().HalconWindow, "fill");
                HOperatorSet.SetColor(drawh.GetHWindowControl().HalconWindow, "red");
                hv_Button = 0;
                HTuple rows2 = new HTuple();
                HTuple cols2 = new HTuple();
                HTuple rows = new HTuple();
                HTuple cols = new HTuple();
                HTuple mindists2 = new HTuple();
                HTuple mindists = new HTuple();
                HObject hObject = new HObject();
                HObject hObjectDT = new HObject();
                hObjectDT.GenEmptyObj();
                bool ButtMot = false;
                List<HObject> ListObjLines = new List<HObject>();
                List<HObject> ListObjCross = new List<HObject>();
                List<HTuple> ListHtuple = new List<HTuple>();
                List<HTuple> hTuplesRows = new List<HTuple>();
                List<HTuple> hTuplesCols = new List<HTuple>();
                // 4为鼠标右键
                while (hv_Button != 4)
                {
                    //一直在循环,需要让halcon控件也响应事件,不然到时候跳出循环,之前的事件会一起爆发触发,
                    Application.DoEvents();
                    try
                    {
                        HOperatorSet.SetSystem("flush_graphic", "false");
                        HOperatorSet.ClearWindow(drawh.GetHWindowControl().HalconWindow);
                        HOperatorSet.GetMposition(drawh.GetHWindowControl().HalconWindow, out hv_Row, out hv_Column, out hv_Button);
                        try
                        {
                            HOperatorSet.SetSystem("flush_graphic", "true");
                            HOperatorSet.DispObj(drawh.Image(), drawh.GetHWindowControl().HalconWindow);
                            if (hv_Button == 1)
                            {
                                HOperatorSet.GenCrossContourXld(out final_region, hv_Row, hv_Column, 40, 0);
                                if (rows.Length == 0)
                                {
                                    HOperatorSet.GenCrossContourXld(out hObjectDT, hv_Row, hv_Column, 40, 0);
                                    rows = hv_Row;
                                    cols = hv_Column;
                                }
                                else
                                {
                                    rows2 = hv_Row;
                                    cols2 = hv_Column;
                                }
                                ListObjCross.Add(final_region);
                                if (rows2.Length != 0)
                                {
                                    HOperatorSet.DistancePp(rows, cols, rows2, cols2, out HTuple mindist);
                                    HOperatorSet.GenContourPolygonXld(out brush_region_affine, new HTuple(rows, rows2), new HTuple(cols, cols2));
                                    HOperatorSet.SetColor(drawh.GetHWindowControl().HalconWindow, "green");
                                    mindists2 = drawh.GetCaliConstMM(mindist);
                                    hTuplesRows.Add(rows2);
                                    hTuplesCols.Add(cols2);
                                    mindist = mindist + "Px";
                                    mindist.Append(mindists2 + "mm");
                                    HOperatorSet.HomMat2dInvert(homMat2D, out HTuple homMat2dInvert);
                                    HOperatorSet.AffineTransPoint2d(homMat2dInvert, rows2, cols2, out HTuple rowTrans, out HTuple colTrans);
                                    mindist.Append("X" + rowTrans + "Y" + colTrans);
                                    mindists2 = mindist;
                                    ListObjLines.Add(brush_region_affine);
                                    ListHtuple.Add(mindists2);
                                }
                            }
                            else
                            {
                                ButtMot = false;
                                HOperatorSet.GenCrossContourXld(out hObject, hv_Row, hv_Column, 100, 0);
                                //HOperatorSet.SetColor(drawh.GetHWindowControl().HalconWindow, ColorResult.green.ToString());
                            }
                            HOperatorSet.SetColor(drawh.GetHWindowControl().HalconWindow, "red");
                            HOperatorSet.DispObj(hObject, drawh.GetHWindowControl().HalconWindow);
                            if (rows.Length == 1)
                            {
                                //ListHtuple.Clear();
                                //hTuplesRows.Clear();
                                //hTuplesCols.Clear();
                                mindists2 = new HTuple();
                                HOperatorSet.DistancePp(hv_Row, hv_Column, rows, cols, out mindists);
                                HOperatorSet.GenContourPolygonXld(out HObject controu, new HTuple(hv_Row, rows), new HTuple(hv_Column, cols));
                                HOperatorSet.DispObj(controu, drawh.GetHWindowControl().HalconWindow);
                                HTuple distMM = drawh.GetCaliConstMM(mindists);
                                HOperatorSet.HomMat2dInvert(homMat2D, out HTuple homMat2dInvert);
                                HOperatorSet.AffineTransPoint2d(homMat2dInvert, hv_Row, hv_Column, out HTuple rowTrans, out HTuple colTrans);

                                mindists2.Append(new HTuple(mindists + "Px", distMM + "mm"));
                                mindists2.Append("X" + rowTrans + "Y" + colTrans);
                                //ListHtuple.Add(mindists2);
                                //hTuplesRows.Add(hv_Row);
                                //hTuplesCols.Add(hv_Column);
                                Disp_message(drawh.GetHWindowControl().HalconWindow, mindists2, hv_Row, hv_Column + 20);

                            }
                            HOperatorSet.SetColor(drawh.GetHWindowControl().HalconWindow, ColorResult.blue.ToString());
                            for (int i = 0; i < ListObjCross.Count; i++)
                            {
                                HOperatorSet.DispObj(ListObjCross[i], drawh.GetHWindowControl().HalconWindow);
                            }
                            for (int i = 0; i < ListHtuple.Count; i++)
                            {
                                HOperatorSet.DispObj(ListObjLines[i], drawh.GetHWindowControl().HalconWindow);
                                Disp_message(drawh.GetHWindowControl().HalconWindow, ListHtuple[i], hTuplesRows[i] - 10, hTuplesCols[i], false, ColorResult.green.ToString(), "false");
                            }
                            HOperatorSet.SetColor(drawh.GetHWindowControl().HalconWindow, ColorResult.green.ToString());
                            HOperatorSet.DispObj(hObjectDT, drawh.GetHWindowControl().HalconWindow);
                        }
                        catch (Exception ex)
                        {


                        }
                    }
                    catch (HalconException ex)
                    {
                        hv_Button = 0;
                    }
                }
            }
            catch (Exception)
            {
            }
            HOperatorSet.SetDraw(drawh.GetHWindowControl().HalconWindow, "margin");
            drawh.Drawing = false;
            drawh.DrawErasure = false;
        }

        /// <summary>
        /// Pose转3D
        /// </summary>
        /// <param name="pose"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <returns>成功返回Ture</returns>
        public static bool PoseToXYZUVW(HTuple pose, out Single x, out Single y, out Single z, out Single u, out Single v, out Single w)
        {
            x = y = z = u = v = w = 0;
            try
            {
                if (pose.Length >= 7)
                {
                    x = (Single)pose.TupleSelect(0).TupleMult(1000).D;
                    y = (Single)pose.TupleSelect(1).TupleMult(1000).D;
                    z = (Single)pose.TupleSelect(2).TupleMult(1000).D;
                    u = (Single)pose.TupleSelect(5).D;
                    if (u > 180)
                    {
                        u = u - 360;
                    }
                    v = (Single)pose.TupleSelect(4).D;
                    if (v > 180)
                    {
                        v = v - 360;
                    }
                    w = (Single)pose.TupleSelect(3).D;
                    if (w > 180)
                    {
                        w = w - 360;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
            }
            return false;
        }




        /// <summary>
        /// 计算图像清晰度,Deviation方差法,laplace拉普拉斯能量函数,energy能量梯度函数,Brenner函数法,Tenegrad函数法
        /// </summary>
        /// <param name="hObjectImage">图像</param>
        /// <param name="method">计算方法</param>
        /// <returns>清晰度</returns>
        public static double Evaluate_definition(HObject Image, string method = "Brenner")
        {
            HTuple value = new HTuple();
            HTuple Deviation = new HTuple();
            HObject hObjectImage;
            try
            {
                HOperatorSet.ScaleImageMax(Image, out hObjectImage);
                HOperatorSet.GetImageSize(hObjectImage, out HTuple width, out HTuple height);
                if (method == "Deviation")
                {
                    HOperatorSet.RegionToMean(hObjectImage, hObjectImage, out HObject ImageMean);
                    HOperatorSet.ConvertImageType(ImageMean, out ImageMean, "real");
                    HOperatorSet.ConvertImageType(hObjectImage, out hObjectImage, "real");
                    HOperatorSet.SubImage(hObjectImage, ImageMean, out HObject imageSub, 1, 0);
                    HOperatorSet.MultImage(imageSub, imageSub, out HObject imageResult, 1, 0);
                    HOperatorSet.Intensity(imageResult, imageResult, out value, out Deviation);
                    imageResult.Dispose();
                    ImageMean.Dispose();
                    imageSub.Dispose();
                }//方差法
                else if (method == "laplace")
                {
                    HOperatorSet.Laplace(hObjectImage, out HObject imageLaplace4, "signed", 3, "n_4");
                    HOperatorSet.Laplace(hObjectImage, out HObject imageLaplace8, "signed", 3, "n_8");
                    HOperatorSet.AddImage(imageLaplace4, imageLaplace4, out HObject imageResulit1, 1, 0);
                    HOperatorSet.AddImage(imageLaplace8, imageResulit1, out imageResulit1, 1, 0);
                    HOperatorSet.MultImage(imageResulit1, imageResulit1, out HObject imageResulit, 1, 0);
                    HOperatorSet.Intensity(imageResulit, imageResulit, out value, out Deviation);
                    imageResulit.Dispose();
                    imageResulit1.Dispose();
                    imageLaplace8.Dispose();
                    imageLaplace4.Dispose();
                }  //*拉普拉斯能量函数
                else if (method == "energy")
                {
                    HOperatorSet.CropPart(hObjectImage, out HObject imagePart0, 0, 0, width - 1, height - 1);
                    HOperatorSet.CropPart(hObjectImage, out HObject imagePart1, 0, 1, width - 1, height - 1);
                    HOperatorSet.CropPart(hObjectImage, out HObject imagePart10, 1, 0, width - 1, height - 1);
                    HOperatorSet.ConvertImageType(imagePart0, out imagePart0, "real");
                    HOperatorSet.ConvertImageType(imagePart1, out imagePart1, "real");
                    HOperatorSet.ConvertImageType(imagePart10, out imagePart10, "real");
                    HOperatorSet.SubImage(imagePart10, imagePart0, out HObject imagesub1, 1, 0);
                    HOperatorSet.MultImage(imagesub1, imagesub1, out HObject imageResult1, 1, 0);
                    HOperatorSet.SubImage(imagePart1, imagePart0, out HObject imageSub2, 1, 0);
                    HOperatorSet.MultImage(imageSub2, imageSub2, out HObject imageResult2, 1, 0);
                    HOperatorSet.AddImage(imageResult1, imageResult2, out HObject imageResult, 1, 0);
                    HOperatorSet.Intensity(imageResult, imageResult, out value, out Deviation);
                    imagesub1.Dispose();
                    imagePart0.Dispose();
                    imagePart1.Dispose();
                    imagePart10.Dispose();
                    imageResult1.Dispose();
                    imageResult2.Dispose();
                    imageResult.Dispose();
                }       //能量梯度函数
                else if (method == "Brenner")
                {
                    HOperatorSet.CropPart(hObjectImage, out HObject ImagePart00, 0, 0, width, height - 2);
                    HOperatorSet.ConvertImageType(ImagePart00, out ImagePart00, "real");
                    HOperatorSet.CropPart(hObjectImage, out HObject ImagePart20, 2, 0, width, height - 2);
                    HOperatorSet.ConvertImageType(ImagePart20, out ImagePart20, "real");
                    HOperatorSet.SubImage(ImagePart20, ImagePart00, out HObject ImageSub, 1, 0);
                    HOperatorSet.MultImage(ImageSub, ImageSub, out HObject ImageResult, 1, 0);
                    HOperatorSet.Intensity(ImageResult, ImageResult, out value, out Deviation);
                    ImageResult.Dispose();
                    ImagePart20.Dispose();
                    ImagePart00.Dispose();
                    ImageSub.Dispose();
                }//Brenner函数法
                else if (method == "Tenegrad")
                {
                    HOperatorSet.SobelAmp(hObjectImage, out HObject EdgeAmplitude, "sum_sqrt", 3);
                    HOperatorSet.MinMaxGray(EdgeAmplitude, EdgeAmplitude, 0, out HTuple min, out HTuple max, out HTuple Range);
                    HOperatorSet.Threshold(EdgeAmplitude, out HObject Region1, 11.8, 255);
                    HOperatorSet.RegionToBin(Region1, out HObject binImage, 1, 0, width, height);
                    HOperatorSet.MultImage(EdgeAmplitude, binImage, out HObject ImageResult4, 1, 0);
                    HOperatorSet.MultImage(ImageResult4, ImageResult4, out HObject imageResult, 1, 0);
                    HOperatorSet.Intensity(imageResult, imageResult, out value, out Deviation);
                    imageResult.Dispose();
                    binImage.Dispose();
                    EdgeAmplitude.Dispose();
                    Region1.Dispose();
                }//*Tenegrad函数法
                else
                {
                    MessageBox.Show("输入参数method错误,不支持的分析方法" + method + "请输入正确的方法：Deviation，laplace，energy，Brenner，Tenegrad");
                }
                hObjectImage.Dispose();
            }
            catch (Exception ex)
            {
                //ErrLog("获取清晰度", ex);
            }
            return value.D;
        }





   
        public static void Disp_message(HTuple hv_WindowHandle, HTuple hv_String,
          double hv_Row = 20, double hv_Column = 20)
        {
            Disp_message(hv_WindowHandle, hv_String, hv_Row, hv_Column, false, "red", "false");
        }

        /// <summary>
        /// 在窗口显示文本
        /// </summary>
        /// <param name="hv_WindowHandle"></param>
        /// <param name="hv_String">文本</param>
        /// <param name="hv_Row">行</param>
        /// <param name="hv_Column">列</param>
        /// <param name="hv_CoordSystem">true窗口,或图像</param>
        /// <param name="hv_Color"></param>
        /// <param name="hv_Box"></param>
        public static void Disp_message(HTuple hv_WindowHandle, HTuple hv_String,
          HTuple hv_Row, HTuple hv_Column, bool hv_CoordSystem = false, string hv_Color = "red", string hv_Box = "true")
        {
            try
            {
                if (!hv_CoordSystem)
                {
                    HOperatorSet.DispText(hv_WindowHandle, hv_String, "image", hv_Row, hv_Column, hv_Color,
                        new HTuple("box", "shadow"), new HTuple(hv_Box, "false"));
                }
                else
                {
                    HOperatorSet.DispText(hv_WindowHandle, hv_String, "window", hv_Row, hv_Column, hv_Color,
                       new HTuple("box", "shadow"), new HTuple(hv_Box, "false"));
                }
            }
            catch (Exception ex)
            {
            }
            return;
            if (hv_Box == null)
            {
                hv_Box = "";
            }
            if (hv_Color == null)
            {
                hv_Color = "yellow";
            }
            if (hv_Column == null)
            {
                hv_Column = 20;
            }
            if (hv_Row == null)
            {
                hv_Row = 20;
            }
            HTuple hv_Red, hv_Green, hv_Blue, hv_Row1Part;
            HTuple hv_Column1Part, hv_Row2Part, hv_Column2Part, hv_RowWin;
            HTuple hv_ColumnWin, hv_WidthWin, hv_HeightWin, hv_MaxAscent;
            HTuple hv_MaxDescent, hv_MaxWidth, hv_MaxHeight, hv_R1 = new HTuple();
            HTuple hv_C1 = new HTuple(), hv_FactorRow = new HTuple(), hv_FactorColumn = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Index = new HTuple(), hv_Ascent = new HTuple();
            HTuple hv_Descent = new HTuple(), hv_W = new HTuple(), hv_H = new HTuple();
            HTuple hv_FrameHeight = new HTuple(), hv_FrameWidth = new HTuple();
            HTuple hv_R2 = new HTuple(), hv_C2 = new HTuple(), hv_DrawMode = new HTuple();
            HTuple hv_Exception = new HTuple(), hv_CurrentColor = new HTuple();
            HTuple hv_Column_COPY_INP_TMP = hv_Column;
            HTuple hv_Row_COPY_INP_TMP = hv_Row;
            HTuple hv_String_COPY_INP_TMP = hv_String.Clone();
            try
            {
                HOperatorSet.GetRgb(hv_WindowHandle, out hv_Red, out hv_Green, out hv_Blue);
                HOperatorSet.GetPart(hv_WindowHandle, out hv_Row1Part, out hv_Column1Part, out hv_Row2Part,
                    out hv_Column2Part);
                HOperatorSet.GetWindowExtents(hv_WindowHandle, out hv_RowWin, out hv_ColumnWin,
                    out hv_WidthWin, out hv_HeightWin);
                HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_HeightWin - 1, hv_WidthWin - 1);
                //
                //default settings
                if ((int)(new HTuple(hv_Row_COPY_INP_TMP.TupleEqual(-1))) != 0)
                {
                    hv_Row_COPY_INP_TMP = 12;
                }
                if ((int)(new HTuple(hv_Column_COPY_INP_TMP.TupleEqual(-1))) != 0)
                {
                    hv_Column_COPY_INP_TMP = 12;
                }

                //
                hv_String_COPY_INP_TMP = ((("" + hv_String_COPY_INP_TMP) + "")).TupleSplit("\n");
                //Estimate extentions of text depending on font size.
                HOperatorSet.GetFontExtents(hv_WindowHandle, out hv_MaxAscent, out hv_MaxDescent,
                   out hv_MaxWidth, out hv_MaxHeight);
                if (hv_CoordSystem)
                {
                    hv_R1 = hv_Row_COPY_INP_TMP.Clone();
                    hv_C1 = hv_Column_COPY_INP_TMP.Clone();
                }
                else
                {
                    //transform image to window coordinates
                    hv_FactorRow = (1.0 * hv_HeightWin) / ((hv_Row2Part - hv_Row1Part) + 1);
                    hv_FactorColumn = (1.0 * hv_WidthWin) / ((hv_Column2Part - hv_Column1Part) + 1);
                    hv_R1 = ((hv_Row_COPY_INP_TMP - hv_Row1Part) + 0.5) * hv_FactorRow;
                    hv_C1 = ((hv_Column_COPY_INP_TMP - hv_Column1Part) + 0.5) * hv_FactorColumn;
                }
                //
                //display text box depending on text size
                if (hv_Box.ToLower() == "true")
                {
                    //calculate box extents
                    hv_String_COPY_INP_TMP = (" " + hv_String_COPY_INP_TMP) + " ";
                    hv_Width = new HTuple();
                    for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                        )) - 1); hv_Index = (int)hv_Index + 1)
                    {
                        HOperatorSet.GetStringExtents(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(
                            hv_Index), out hv_Ascent, out hv_Descent, out hv_W, out hv_H);
                        hv_Width = hv_Width.TupleConcat(hv_W);
                    }
                    if (hv_CoordSystem)
                    {
                        hv_FrameHeight = hv_MaxHeight * (new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                  ));
                    }
                    else
                    {
                        hv_FrameHeight = hv_MaxHeight;
                    }

                    hv_FrameWidth = (((new HTuple(0)).TupleConcat(hv_Width))).TupleMax();
                    hv_R2 = hv_R1 + hv_FrameHeight;
                    hv_C2 = hv_C1 + hv_FrameWidth;
                    //display rectangles
                    HOperatorSet.GetDraw(hv_WindowHandle, out hv_DrawMode);
                    HOperatorSet.SetDraw(hv_WindowHandle, "fill");
                    HOperatorSet.SetColor(hv_WindowHandle, "light gray");
                    HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1 + 3, hv_C1 + 3, hv_R2 + 3, hv_C2 + 3);
                    HOperatorSet.SetColor(hv_WindowHandle, "white");
                    HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1, hv_C1, hv_R2, hv_C2);
                    HOperatorSet.SetDraw(hv_WindowHandle, hv_DrawMode);
                }
                else
                {
                }

                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
           )) - 1); hv_Index = (int)hv_Index + 1)
                {
                    hv_CurrentColor = hv_Color;
                    if ((int)((new HTuple(hv_CurrentColor.TupleNotEqual(""))).TupleAnd(new HTuple(hv_CurrentColor.TupleNotEqual(
                        "auto")))) != 0)
                    {
                        HOperatorSet.SetColor(hv_WindowHandle, hv_CurrentColor);
                    }
                    else
                    {
                        HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
                    }
                    hv_Row_COPY_INP_TMP = hv_R1[0] + (hv_MaxHeight * hv_Index);
                    HTuple hTuple = hv_C1[0];
                    if (hv_R1.Length > 1)
                    {
                        if (hv_Index < hv_R1.Length)
                        {
                            hv_Row_COPY_INP_TMP = hv_R1[hv_Index];
                            hTuple = hv_C1[hv_Index];
                        }
                    }
                    try
                    {
                        HOperatorSet.SetTposition(hv_WindowHandle, hv_Row_COPY_INP_TMP.TupleInt(), hTuple.TupleInt());
                        //HOperatorSet.DispText(hv_WindowHandle, hv_String_COPY_INP_TMP[hv_Index], "image", hv_Row_COPY_INP_TMP, hTuple, hv_CurrentColor, "box", "false");
                        HOperatorSet.WriteString(hv_WindowHandle, hv_String_COPY_INP_TMP[hv_Index]);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                //HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
                HOperatorSet.SetPart(hv_WindowHandle, hv_Row1Part, hv_Column1Part, hv_Row2Part,
                    hv_Column2Part);

                return;
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 绘制直线
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="phi"></param>
        /// <param name="lieng"></param>
        /// <returns></returns>
        public static HObject GenLine(double row, double column, double phi, double lieng)
        {
            HObject hObject = new HObject();
            hObject.GenEmptyObj();
            try
            {
                HTuple rows = new HTuple(0);
                rows.Append(0);
                HTuple cols = new HTuple(-lieng);
                cols.Append(lieng);
                HOperatorSet.HomMat2dIdentity(out HTuple homMat2d);
                HOperatorSet.HomMat2dRotate(homMat2d, phi, 0, 0, out homMat2d);
                HOperatorSet.HomMat2dTranslate(homMat2d, row, column, out homMat2d);
                HOperatorSet.AffineTransPoint2d(homMat2d, rows, cols, out rows, out cols);
                HOperatorSet.GenContourPolygonXld(out hObject, rows, cols);
            }
            catch (Exception)
            {
            }
            return hObject;
        }
        /// <summary>
        /// 获取直线坐标
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="phi"></param>
        /// <param name="lieng"></param>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        public static void GetLinePoint(double row, double column, double phi, double lieng, out HTuple rows, out HTuple cols)
        {
            rows = new HTuple(0);
            cols = new HTuple(0);
            try
            {
                rows.Append(0);
                cols.Append(lieng);
                HOperatorSet.HomMat2dIdentity(out HTuple homMat2d);
                HOperatorSet.HomMat2dRotate(homMat2d, phi, 0, 0, out homMat2d);
                HOperatorSet.HomMat2dTranslate(homMat2d, row, column, out homMat2d);
                HOperatorSet.AffineTransPoint2d(homMat2d, rows, cols, out rows, out cols);
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 形成四角矩阵
        /// </summary>
        /// <param name="arooRows"></param>
        /// <param name="arooCols"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static HObject GetRectangularObj(HTuple arooRows, HTuple arooCols, int width, int height, int length = 120)
        {
            HObject roboj = new HObject();
            roboj.GenEmptyObj();
            try
            {

                HOperatorSet.GenRegionPolygon(out HObject hObject2, new HTuple(length + arooRows,
                     arooRows, arooRows), new HTuple(arooCols,
                     arooCols, length + arooCols));
                HOperatorSet.GenRegionPolygon(out HObject hObject3, new HTuple(-length + arooRows + height,
                  arooRows + height, arooRows + height), new HTuple(arooCols,
                  arooCols, length + arooCols));

                HOperatorSet.GenRegionPolygon(out HObject hObject4, new HTuple(-length + arooRows + height,
                            arooRows + height, arooRows + height), new HTuple(arooCols + width,
                            arooCols + width, -length + arooCols + width));

                HOperatorSet.GenRegionPolygon(out HObject hObject5, new HTuple(length + arooRows,
                  arooRows, arooRows), new HTuple(arooCols + width,
                  arooCols + width, -length + arooCols + width));
                roboj = hObject2.ConcatObj(hObject3);
                roboj = roboj.ConcatObj(hObject4);
                roboj = roboj.ConcatObj(hObject5);
            }
            catch (Exception)
            {
            }
            return roboj;
        }
        /// <summary>
        /// 绘制箭头
        /// </summary>
        /// <param name="ho_Arrow">轮廓组</param>
        /// <param name="hv_Row1"></param>
        /// <param name="hv_Column1"></param>
        /// <param name="hv_Row2"></param>
        /// <param name="hv_Column2"></param>
        /// <param name="hv_HeadLength">长</param>
        /// <param name="hv_HeadWidth">高</param>
        public static void Gen_arrow_contour_xld(out HObject ho_Arrow, HTuple hv_Row1, HTuple hv_Column1,
            HTuple hv_Row2, HTuple hv_Column2, HTuple hv_HeadLength, HTuple hv_HeadWidth)
        {
            // Stack for temporary objects
            HObject[] OTemp = new HObject[20];
            long SP_O = 0;
            // Local iconic variables

            HObject ho_TempArrow = null;

            // Local control variables

            HTuple hv_Length, hv_ZeroLengthIndices, hv_DR;
            HTuple hv_DC, hv_HalfHeadWidth, hv_RowP1, hv_ColP1, hv_RowP2;
            HTuple hv_ColP2, hv_Index;

            // Initialize local and output iconic variables
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            HOperatorSet.GenEmptyObj(out ho_TempArrow);

            try
            {
                ho_Arrow.Dispose();
                HOperatorSet.GenEmptyObj(out ho_Arrow);
                //
                //Calculate the arrow length
                HOperatorSet.DistancePp(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_Length);
                //
                //Mark arrows with identical start and end point
                //(set Length to -1 to avoid division-by-zero exception)
                hv_ZeroLengthIndices = hv_Length.TupleFind(0);
                if ((int)(new HTuple(hv_ZeroLengthIndices.TupleNotEqual(-1))) != 0)
                {
                    hv_Length[hv_ZeroLengthIndices] = -1;
                }
                //
                //Calculate auxiliary variables.
                hv_DR = (1.0 * (hv_Row2 - hv_Row1)) / hv_Length;
                hv_DC = (1.0 * (hv_Column2 - hv_Column1)) / hv_Length;
                hv_HalfHeadWidth = hv_HeadWidth / 2.0;
                //
                //Calculate end points of the arrow head.
                hv_RowP1 = (hv_Row1 + ((hv_Length - hv_HeadLength) * hv_DR)) + (hv_HalfHeadWidth * hv_DC);
                hv_ColP1 = (hv_Column1 + ((hv_Length - hv_HeadLength) * hv_DC)) - (hv_HalfHeadWidth * hv_DR);
                hv_RowP2 = (hv_Row1 + ((hv_Length - hv_HeadLength) * hv_DR)) - (hv_HalfHeadWidth * hv_DC);
                hv_ColP2 = (hv_Column1 + ((hv_Length - hv_HeadLength) * hv_DC)) + (hv_HalfHeadWidth * hv_DR);
                //
                //Finally create output XLD contour for each input point pair
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Length.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
                {
                    if ((int)(new HTuple(((hv_Length.TupleSelect(hv_Index))).TupleEqual(-1))) != 0)
                    {
                        //Create_ single points for arrows with identical start and end point
                        ho_TempArrow.Dispose();
                        HOperatorSet.GenContourPolygonXld(out ho_TempArrow, hv_Row1.TupleSelect(
                            hv_Index), hv_Column1.TupleSelect(hv_Index));
                    }
                    else
                    {
                        //Create arrow contour
                        ho_TempArrow.Dispose();
                        HOperatorSet.GenContourPolygonXld(out ho_TempArrow, ((((((((((hv_Row1.TupleSelect(
                            hv_Index))).TupleConcat(hv_Row2.TupleSelect(hv_Index)))).TupleConcat(
                            hv_RowP1.TupleSelect(hv_Index)))).TupleConcat(hv_Row2.TupleSelect(hv_Index)))).TupleConcat(
                            hv_RowP2.TupleSelect(hv_Index)))).TupleConcat(hv_Row2.TupleSelect(hv_Index)),
                            ((((((((((hv_Column1.TupleSelect(hv_Index))).TupleConcat(hv_Column2.TupleSelect(
                            hv_Index)))).TupleConcat(hv_ColP1.TupleSelect(hv_Index)))).TupleConcat(
                            hv_Column2.TupleSelect(hv_Index)))).TupleConcat(hv_ColP2.TupleSelect(
                            hv_Index)))).TupleConcat(hv_Column2.TupleSelect(hv_Index)));
                    }
                    OTemp[SP_O] = ho_Arrow.CopyObj(1, -1);
                    SP_O++;
                    ho_Arrow.Dispose();
                    HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_TempArrow, out ho_Arrow);
                    OTemp[SP_O - 1].Dispose();
                    SP_O = 0;
                }
                ho_TempArrow.Dispose();

                return;
            }
            catch (HalconException)
            {
                ho_TempArrow.Dispose();

                //throw HDevExpDefaultException;
            }
        }

        /// <summary>
        /// 绘制箭头
        /// </summary>
        /// <param name="ho_Arrow"></param>
        /// <param name="hv_Row1"></param>
        /// <param name="hv_Column1"></param>
        /// <param name="hv_Row2"></param>
        /// <param name="hv_Column2"></param>
        public static void Gen_arrow_contour_xld(out HObject ho_Arrow, HTuple hv_Row1, HTuple hv_Column1,
    HTuple hv_Row2, HTuple hv_Column2)
        {
            HTuple hv_HeadLength = 20;
            HTuple hv_HeadWidth = 2;

            // Stack for temporary objects
            HObject[] OTemp = new HObject[20];
            long SP_O = 0;
            // Local iconic variables

            HObject ho_TempArrow = null;

            // Local control variables

            HTuple hv_Length, hv_ZeroLengthIndices, hv_DR;
            HTuple hv_DC, hv_HalfHeadWidth, hv_RowP1, hv_ColP1, hv_RowP2;
            HTuple hv_ColP2, hv_Index;

            // Initialize local and output iconic variables
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            HOperatorSet.GenEmptyObj(out ho_TempArrow);

            try
            {
                ho_Arrow.Dispose();
                HOperatorSet.GenEmptyObj(out ho_Arrow);
                //
                //Calculate the arrow length
                HOperatorSet.DistancePp(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_Length);

                hv_HeadLength = hv_Length / 10 + 10;
                hv_HeadWidth = hv_Length / 30 + 2;
                //
                //Mark arrows with identical start and end point
                //(set Length to -1 to avoid division-by-zero exception)
                hv_ZeroLengthIndices = hv_Length.TupleFind(0);
                if ((int)(new HTuple(hv_ZeroLengthIndices.TupleNotEqual(-1))) != 0)
                {
                    hv_Length[hv_ZeroLengthIndices] = -1;
                }
                //
                //Calculate auxiliary variables.
                hv_DR = (1.0 * (hv_Row2 - hv_Row1)) / hv_Length;
                hv_DC = (1.0 * (hv_Column2 - hv_Column1)) / hv_Length;
                hv_HalfHeadWidth = hv_HeadWidth / 2.0;
                //
                //Calculate end points of the arrow head.
                hv_RowP1 = (hv_Row1 + ((hv_Length - hv_HeadLength) * hv_DR)) + (hv_HalfHeadWidth * hv_DC);
                hv_ColP1 = (hv_Column1 + ((hv_Length - hv_HeadLength) * hv_DC)) - (hv_HalfHeadWidth * hv_DR);
                hv_RowP2 = (hv_Row1 + ((hv_Length - hv_HeadLength) * hv_DR)) - (hv_HalfHeadWidth * hv_DC);
                hv_ColP2 = (hv_Column1 + ((hv_Length - hv_HeadLength) * hv_DC)) + (hv_HalfHeadWidth * hv_DR);
                //
                //Finally create output XLD contour for each input point pair
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Length.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
                {
                    if ((int)(new HTuple(((hv_Length.TupleSelect(hv_Index))).TupleEqual(-1))) != 0)
                    {
                        //Create_ single points for arrows with identical start and end point
                        ho_TempArrow.Dispose();
                        HOperatorSet.GenContourPolygonXld(out ho_TempArrow, hv_Row1.TupleSelect(
                            hv_Index), hv_Column1.TupleSelect(hv_Index));
                    }
                    else
                    {
                        //Create arrow contour
                        ho_TempArrow.Dispose();
                        HOperatorSet.GenContourPolygonXld(out ho_TempArrow, ((((((((((hv_Row1.TupleSelect(
                            hv_Index))).TupleConcat(hv_Row2.TupleSelect(hv_Index)))).TupleConcat(
                            hv_RowP1.TupleSelect(hv_Index)))).TupleConcat(hv_Row2.TupleSelect(hv_Index)))).TupleConcat(
                            hv_RowP2.TupleSelect(hv_Index)))).TupleConcat(hv_Row2.TupleSelect(hv_Index)),
                            ((((((((((hv_Column1.TupleSelect(hv_Index))).TupleConcat(hv_Column2.TupleSelect(
                            hv_Index)))).TupleConcat(hv_ColP1.TupleSelect(hv_Index)))).TupleConcat(
                            hv_Column2.TupleSelect(hv_Index)))).TupleConcat(hv_ColP2.TupleSelect(
                            hv_Index)))).TupleConcat(hv_Column2.TupleSelect(hv_Index)));
                    }
                    OTemp[SP_O] = ho_Arrow.CopyObj(1, -1);
                    SP_O++;
                    ho_Arrow.Dispose();
                    HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_TempArrow, out ho_Arrow);
                    OTemp[SP_O - 1].Dispose();
                    SP_O = 0;
                }
                ho_TempArrow.Dispose();

                return;
            }
            catch (HalconException)
            {
                ho_TempArrow.Dispose();

                //throw HDevExpDefaultException;
            }
        }

        /// <summary>
        /// 画出直线检测边缘区域
        /// </summary>
        /// <param name="ho_Regions">区域</param>
        /// <param name="hv_WindowHandle">窗口</param>
        /// <param name="hv_Elements">边缘点数</param>
        /// <param name="hv_DetectHeight">卡尺高度</param>
        /// <param name="hv_DetectWidth">卡尺宽度</param>
        /// <param name="hv_Row1">起点y</param>
        /// <param name="hv_Column1">起点x</param>
        /// <param name="hv_Row2">终点y</param>
        /// <param name="hv_Column2">终点x</param>
        public static void Draw_rake(out HObject ho_Regions, HTuple hv_WindowHandle, HTuple hv_Elements,
            HTuple hv_DetectHeight, HTuple hv_DetectWidth, out HTuple hv_Row1, out HTuple hv_Column1,
            out HTuple hv_Row2, out HTuple hv_Column2)
        {
            hv_Column2 = hv_Row1 = hv_Row2 = hv_Column1 = 0;

            // Stack for temporary objects
            HObject[] OTemp = new HObject[20];
            long SP_O = 0;

            // Local iconic variables

            HObject ho_RegionLines, ho_Rectangle = null;
            HObject ho_Arrow1 = null;

            // Local control variables

            HTuple hv_ATan, hv_i, hv_RowC = new HTuple();
            HTuple hv_ColC = new HTuple(), hv_Distance = new HTuple();
            HTuple hv_RowL2 = new HTuple(), hv_RowL1 = new HTuple(), hv_ColL2 = new HTuple();
            HTuple hv_ColL1 = new HTuple();

            // Initialize local and output iconic variables
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_RegionLines);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_Arrow1);

            try
            {
                //提示
                Disp_message(hv_WindowHandle, "点击鼠标左键画一条直线,点击右键确认",
                    12, 12, true);
                //产生一个空显示对象，用于显示
                ho_Regions.Dispose();
                HOperatorSet.GenEmptyObj(out ho_Regions);
                //画矢量检测直线
                HOperatorSet.DrawLine(hv_WindowHandle, out hv_Row1, out hv_Column1, out hv_Row2,
                    out hv_Column2);
                //产生直线xld
                ho_RegionLines.Dispose();
                HOperatorSet.GenContourPolygonXld(out ho_RegionLines, hv_Row1.TupleConcat(hv_Row2),
                    hv_Column1.TupleConcat(hv_Column2));
                //存储到显示对象
                OTemp[SP_O] = ho_Regions.CopyObj(1, -1);
                SP_O++;
                ho_Regions.Dispose();
                HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_RegionLines, out ho_Regions);
                OTemp[SP_O - 1].Dispose();
                SP_O = 0;
                //计算直线与x轴的夹角，逆时针方向为正向。
                HOperatorSet.AngleLx(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_ATan);

                //边缘检测方向垂直于检测直线：直线方向正向旋转90°为边缘检测方向
                hv_ATan = hv_ATan + ((new HTuple(90)).TupleRad());

                //根据检测直线按顺序产生测量区域矩形，并存储到显示对象
                for (hv_i = 1; hv_i.Continue(hv_Elements, 1); hv_i = hv_i.TupleAdd(1))
                {
                    //如果只有一个测量矩形，作为卡尺工具，宽度为检测直线的长度
                    if ((int)(new HTuple(hv_Elements.TupleEqual(1))) != 0)
                    {
                        hv_RowC = (hv_Row1 + hv_Row2) * 0.5;
                        hv_ColC = (hv_Column1 + hv_Column2) * 0.5;
                        HOperatorSet.DistancePp(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_Distance);
                        ho_Rectangle.Dispose();
                        HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle, hv_RowC, hv_ColC,
                            hv_ATan, hv_DetectHeight / 2, hv_Distance / 2);
                    }
                    else
                    {
                        //如果有多个测量矩形，产生该测量矩形xld
                        hv_RowC = hv_Row1 + (((hv_Row2 - hv_Row1) * (hv_i - 1)) / (hv_Elements - 1));
                        hv_ColC = hv_Column1 + (((hv_Column2 - hv_Column1) * (hv_i - 1)) / (hv_Elements - 1));
                        ho_Rectangle.Dispose();
                        HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle, hv_RowC, hv_ColC,
                            hv_ATan, hv_DetectHeight / 2, hv_DetectWidth / 2);
                    }
                    //把测量矩形xld存储到显示对象
                    OTemp[SP_O] = ho_Regions.CopyObj(1, -1);
                    SP_O++;
                    ho_Regions.Dispose();
                    HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_Rectangle, out ho_Regions);
                    OTemp[SP_O - 1].Dispose();
                    SP_O = 0;
                    if ((int)(new HTuple(hv_i.TupleEqual(1))) != 0)
                    {
                        //在第一个测量矩形绘制一个箭头xld，用于只是边缘检测方向
                        hv_RowL2 = hv_RowC + ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleSin()));
                        hv_RowL1 = hv_RowC - ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleSin()));
                        hv_ColL2 = hv_ColC + ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleCos()));
                        hv_ColL1 = hv_ColC - ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleCos()));
                        ho_Arrow1.Dispose();
                        Gen_arrow_contour_xld(out ho_Arrow1, hv_RowL1, hv_ColL1, hv_RowL2, hv_ColL2,
                            25, 25);
                        //把xld存储到显示对象
                        OTemp[SP_O] = ho_Regions.CopyObj(1, -1);
                        SP_O++;
                        ho_Regions.Dispose();
                        HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_Arrow1, out ho_Regions);
                        OTemp[SP_O - 1].Dispose();
                        SP_O = 0;
                    }
                }

                ho_RegionLines.Dispose();
                ho_Rectangle.Dispose();
                ho_Arrow1.Dispose();

                return;
            }
            catch (HalconException)
            {
                ho_RegionLines.Dispose();
                ho_Rectangle.Dispose();
                ho_Arrow1.Dispose();
            }
        }


        public static void Draw_rake(out HObject ho_Regions, HSmartWindowControl hSmartWindow, HTuple hv_Elements,
            HTuple hv_DetectHeight, HTuple hv_DetectWidth, out HTuple hv_Row1, out HTuple hv_Column1,
            out HTuple hv_Row2, out HTuple hv_Column2)
        {
            hv_Column2 = hv_Row1 = hv_Row2 = hv_Column1 = 0;

            // Stack for temporary objects
            HObject[] OTemp = new HObject[20];
            long SP_O = 0;

            // Local iconic variables

            HObject ho_RegionLines, ho_Rectangle = null;
            HObject ho_Arrow1 = null;

            // Local control variables

            HTuple hv_ATan, hv_i, hv_RowC = new HTuple();
            HTuple hv_ColC = new HTuple(), hv_Distance = new HTuple();
            HTuple hv_RowL2 = new HTuple(), hv_RowL1 = new HTuple(), hv_ColL2 = new HTuple();
            HTuple hv_ColL1 = new HTuple();

            // Initialize local and output iconic variables
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_RegionLines);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_Arrow1);
            HTuple hv_WindowHandle = hSmartWindow.HalconWindow;
            try
            {
                //提示
                Disp_message(hv_WindowHandle, "点击鼠标左键画一条直线,点击右键确认",
                    12, 12, true);
                //产生一个空显示对象，用于显示
                ho_Regions.Dispose();
                HOperatorSet.GenEmptyObj(out ho_Regions);
                //画矢量检测直线
                HOperatorSet.DrawLine(hv_WindowHandle, out hv_Row1, out hv_Column1, out hv_Row2,
                    out hv_Column2);
                //产生直线xld
                ho_RegionLines.Dispose();
                HOperatorSet.GenContourPolygonXld(out ho_RegionLines, hv_Row1.TupleConcat(hv_Row2),
                    hv_Column1.TupleConcat(hv_Column2));
                //存储到显示对象
                OTemp[SP_O] = ho_Regions.CopyObj(1, -1);
                SP_O++;
                ho_Regions.Dispose();
                HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_RegionLines, out ho_Regions);
                OTemp[SP_O - 1].Dispose();
                SP_O = 0;
                //计算直线与x轴的夹角，逆时针方向为正向。
                HOperatorSet.AngleLx(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_ATan);

                //边缘检测方向垂直于检测直线：直线方向正向旋转90°为边缘检测方向
                hv_ATan = hv_ATan + ((new HTuple(90)).TupleRad());

                //根据检测直线按顺序产生测量区域矩形，并存储到显示对象
                for (hv_i = 1; hv_i.Continue(hv_Elements, 1); hv_i = hv_i.TupleAdd(1))
                {
                    //如果只有一个测量矩形，作为卡尺工具，宽度为检测直线的长度
                    if ((int)(new HTuple(hv_Elements.TupleEqual(1))) != 0)
                    {
                        hv_RowC = (hv_Row1 + hv_Row2) * 0.5;
                        hv_ColC = (hv_Column1 + hv_Column2) * 0.5;
                        HOperatorSet.DistancePp(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_Distance);
                        ho_Rectangle.Dispose();
                        HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle, hv_RowC, hv_ColC,
                            hv_ATan, hv_DetectHeight / 2, hv_Distance / 2);
                    }
                    else
                    {
                        //如果有多个测量矩形，产生该测量矩形xld
                        hv_RowC = hv_Row1 + (((hv_Row2 - hv_Row1) * (hv_i - 1)) / (hv_Elements - 1));
                        hv_ColC = hv_Column1 + (((hv_Column2 - hv_Column1) * (hv_i - 1)) / (hv_Elements - 1));
                        ho_Rectangle.Dispose();
                        HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle, hv_RowC, hv_ColC,
                            hv_ATan, hv_DetectHeight / 2, hv_DetectWidth / 2);
                    }
                    //把测量矩形xld存储到显示对象
                    OTemp[SP_O] = ho_Regions.CopyObj(1, -1);
                    SP_O++;
                    ho_Regions.Dispose();
                    HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_Rectangle, out ho_Regions);
                    OTemp[SP_O - 1].Dispose();
                    SP_O = 0;
                    if ((int)(new HTuple(hv_i.TupleEqual(1))) != 0)
                    {
                        //在第一个测量矩形绘制一个箭头xld，用于只是边缘检测方向
                        hv_RowL2 = hv_RowC + ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleSin()));
                        hv_RowL1 = hv_RowC - ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleSin()));
                        hv_ColL2 = hv_ColC + ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleCos()));
                        hv_ColL1 = hv_ColC - ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleCos()));
                        ho_Arrow1.Dispose();
                        Gen_arrow_contour_xld(out ho_Arrow1, hv_RowL1, hv_ColL1, hv_RowL2, hv_ColL2,
                            25, 25);
                        //把xld存储到显示对象
                        OTemp[SP_O] = ho_Regions.CopyObj(1, -1);
                        SP_O++;
                        ho_Regions.Dispose();
                        HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_Arrow1, out ho_Regions);
                        OTemp[SP_O - 1].Dispose();
                        SP_O = 0;
                    }
                }

                ho_RegionLines.Dispose();
                ho_Rectangle.Dispose();
                ho_Arrow1.Dispose();

                return;
            }
            catch (HalconException)
            {
                ho_RegionLines.Dispose();
                ho_Rectangle.Dispose();
                ho_Arrow1.Dispose();
            }
        }
        /// <summary>
        /// 画出圆检测边缘区域
        /// </summary>
        /// <param name="ho_Image">输入图像</param>
        /// <param name="ho_Regions">区域</param>
        /// <param name="hv_WindowHandle">窗口句柄</param>
        /// <param name="hv_Elements">边缘点数</param>
        /// <param name="hv_DetectHeight">卡尺高度</param>
        /// <param name="hv_DetectWidth">卡尺宽度</param>
        /// <param name="hv_ROIRows">spoke工具ROI的y数组</param>
        /// <param name="hv_ROICols">spoke工具ROI的X数组</param>
        /// <param name="hv_Direct">'inner'表示检测方向由边缘点指向圆心； 'outer'表示检测方向由圆心指向边缘点</param>
        public static void Daw_spoke(HObject ho_Image, out HObject ho_Regions, HTuple hv_WindowHandle,
            HTuple hv_Elements, HTuple hv_DetectHeight, HTuple hv_DetectWidth, out HTuple hv_ROIRows,
            out HTuple hv_ROICols, out HTuple hv_Direct)
        {
            // Stack for temporary objects
            HObject[] OTemp = new HObject[20];
            long SP_O = 0;

            // Local iconic variables

            HObject ho_ContOut1, ho_Contour, ho_ContCircle;
            HObject ho_Cross, ho_Rectangle1 = null, ho_Arrow1 = null;

            // Local control variables

            HTuple hv_Rows, hv_Cols, hv_Weights, hv_Length1;
            HTuple hv_RowC, hv_ColumnC, hv_Radius, hv_StartPhi, hv_EndPhi;
            HTuple hv_PointOrder, hv_RowXLD, hv_ColXLD, hv_Row1, hv_Column1;
            HTuple hv_Row2, hv_Column2, hv_DistanceStart, hv_DistanceEnd;
            HTuple hv_Length2, hv_i, hv_j = new HTuple(), hv_RowE = new HTuple();
            HTuple hv_ColE = new HTuple(), hv_ATan = new HTuple(), hv_RowL2 = new HTuple();
            HTuple hv_RowL1 = new HTuple(), hv_ColL2 = new HTuple(), hv_ColL1 = new HTuple();

            // Initialize local and output iconic variables
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_ContOut1);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_ContCircle);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_Rectangle1);
            HOperatorSet.GenEmptyObj(out ho_Arrow1);

            hv_ROIRows = new HTuple();
            hv_ROICols = new HTuple();
            hv_Direct = new HTuple();
            try
            {
                //提示
                //Disp_message(hv_WindowHandle, "1、画4个以上点确定一个圆弧,点击右键确认", "window",
                //    12, 12, "red", "false");
                //产生一个空显示对象，用于显示
                ho_Regions.Dispose();
                HOperatorSet.GenEmptyObj(out ho_Regions);
                //沿着圆弧或圆的边缘画点
                ho_ContOut1.Dispose();
                HOperatorSet.DrawNurbs(out ho_ContOut1, hv_WindowHandle, "true", "true", "true",
                    "true", 3, out hv_Rows, out hv_Cols, out hv_Weights);
                //至少要4个点
                HOperatorSet.TupleLength(hv_Weights, out hv_Length1);
                if ((int)(new HTuple(hv_Length1.TupleLess(4))) != 0)
                {
                    Disp_message(hv_WindowHandle, "提示：点数太少，请重画", 32, 12, true);
                    ho_ContOut1.Dispose();
                    ho_Contour.Dispose();
                    ho_ContCircle.Dispose();
                    ho_Cross.Dispose();
                    ho_Rectangle1.Dispose();
                    ho_Arrow1.Dispose();

                    return;
                }
                //获取点
                hv_ROIRows = hv_Rows.Clone();
                hv_ROICols = hv_Cols.Clone();
                //产生xld
                ho_Contour.Dispose();
                HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_ROIRows, hv_ROICols);
                //用回归线法（不抛出异常点，所有点权重一样）拟合圆
                HOperatorSet.FitCircleContourXld(ho_Contour, "algebraic", -1, 0, 0, 3, 2, out hv_RowC,
                    out hv_ColumnC, out hv_Radius, out hv_StartPhi, out hv_EndPhi, out hv_PointOrder);
                //根据拟合结果产生xld，并保持到显示对象
                ho_ContCircle.Dispose();
                HOperatorSet.GenCircleContourXld(out ho_ContCircle, hv_RowC, hv_ColumnC, hv_Radius,
                    hv_StartPhi, hv_EndPhi, hv_PointOrder, 3);
                OTemp[SP_O] = ho_Regions.CopyObj(1, -1);
                SP_O++;
                ho_Regions.Dispose();
                HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_ContCircle, out ho_Regions);
                OTemp[SP_O - 1].Dispose();
                SP_O = 0;

                //获取圆或圆弧xld上的点坐标
                HOperatorSet.GetContourXld(ho_ContCircle, out hv_RowXLD, out hv_ColXLD);
                //显示图像和圆弧
                if (HDevWindowStack.IsOpen())
                {
                    //HOperatorSet.DispObj(ho_Image, HDevWindowStack.GetActive());
                }
                if (HDevWindowStack.IsOpen())
                {
                    //HOperatorSet.DispObj(ho_ContCircle, HDevWindowStack.GetActive());
                }
                //产生并显示圆心
                ho_Cross.Dispose();
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_RowC, hv_ColumnC, 60, 0.785398);
                if (HDevWindowStack.IsOpen())
                {
                    //HOperatorSet.DispObj(ho_Cross, HDevWindowStack.GetActive());
                }
                //提示
                //Disp_message(hv_WindowHandle, "2、远离圆心，画箭头确定边缘检测方向，点击右键确认",
                //    "window", 32, 12, "red", "false");
                //画线，确定检测方向
                HOperatorSet.DrawLine(hv_WindowHandle, out hv_Row1, out hv_Column1, out hv_Row2,
                    out hv_Column2);
                //求圆心到检测方向直线起点的距离
                HOperatorSet.DistancePp(hv_RowC, hv_ColumnC, hv_Row1, hv_Column1, out hv_DistanceStart);
                //求圆心到检测方向直线终点的距离
                HOperatorSet.DistancePp(hv_RowC, hv_ColumnC, hv_Row2, hv_Column2, out hv_DistanceEnd);

                //求圆或圆弧xld上的点的数量
                HOperatorSet.TupleLength(hv_ColXLD, out hv_Length2);
                //判断检测的边缘数量是否过少
                if ((int)(new HTuple(hv_Elements.TupleLess(3))) != 0)
                {
                    //hv_ROIRows = new HTuple();
                    //hv_ROICols = new HTuple();
                    Disp_message(hv_WindowHandle, "检测的边缘数量太少，请重新设置!",
                        52, 12, true);
                    ho_ContOut1.Dispose();
                    ho_Contour.Dispose();
                    ho_ContCircle.Dispose();
                    ho_Cross.Dispose();
                    ho_Rectangle1.Dispose();
                    ho_Arrow1.Dispose();

                    return;
                }
                //如果xld是圆弧，有Length2个点，从起点开始，等间距（间距为Length2/(Elements-1)）取Elements个点，作为卡尺工具的中点
                //如果xld是圆，有Length2个点，以0°为起点，从起点开始，等间距（间距为Length2/(Elements)）取Elements个点，作为卡尺工具的中点
                for (hv_i = 0; hv_i.Continue(hv_Elements - 1, 1); hv_i = hv_i.TupleAdd(1))
                {
                    if ((int)(new HTuple(((hv_RowXLD.TupleSelect(0))).TupleEqual(hv_RowXLD.TupleSelect(
                        hv_Length2 - 1)))) != 0)
                    {
                        //xld的起点和终点坐标相对，为圆
                        HOperatorSet.TupleInt(((1.0 * hv_Length2) / hv_Elements) * hv_i, out hv_j);
                    }
                    else
                    {
                        //否则为圆弧
                        HOperatorSet.TupleInt(((1.0 * hv_Length2) / (hv_Elements - 1)) * hv_i, out hv_j);
                    }
                    //索引越界，强制赋值为最后一个索引
                    if ((int)(new HTuple(hv_j.TupleGreaterEqual(hv_Length2))) != 0)
                    {
                        hv_j = hv_Length2 - 1;
                        //continue
                    }
                    //获取卡尺工具中心
                    hv_RowE = hv_RowXLD.TupleSelect(hv_j);
                    hv_ColE = hv_ColXLD.TupleSelect(hv_j);

                    //如果圆心到检测方向直线的起点的距离大于圆心到检测方向直线的终点的距离，搜索方向由圆外指向圆心
                    //如果圆心到检测方向直线的起点的距离不大于圆心到检测方向直线的终点的距离，搜索方向由圆心指向圆外
                    if ((int)(new HTuple(hv_DistanceStart.TupleGreater(hv_DistanceEnd))) != 0)
                    {
                        //求卡尺工具的边缘搜索方向
                        //求圆心指向边缘的矢量的角度
                        HOperatorSet.TupleAtan2((-hv_RowE) + hv_RowC, hv_ColE - hv_ColumnC, out hv_ATan);
                        //角度反向
                        hv_ATan = ((new HTuple(180)).TupleRad()) + hv_ATan;
                        //边缘搜索方向类型：'inner'搜索方向由圆外指向圆心；'outer'搜索方向由圆心指向圆外
                        hv_Direct = "inner";
                    }
                    else
                    {
                        //求卡尺工具的边缘搜索方向
                        //求圆心指向边缘的矢量的角度
                        HOperatorSet.TupleAtan2((-hv_RowE) + hv_RowC, hv_ColE - hv_ColumnC, out hv_ATan);
                        //边缘搜索方向类型：'inner'搜索方向由圆外指向圆心；'outer'搜索方向由圆心指向圆外
                        hv_Direct = "outer";
                    }

                    //产生卡尺xld，并保持到显示对象
                    ho_Rectangle1.Dispose();
                    HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle1, hv_RowE, hv_ColE,
                        hv_ATan, hv_DetectHeight / 2, hv_DetectWidth / 2);
                    OTemp[SP_O] = ho_Regions.CopyObj(1, -1);
                    SP_O++;
                    ho_Regions.Dispose();
                    HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_Rectangle1, out ho_Regions);
                    OTemp[SP_O - 1].Dispose();
                    SP_O = 0;

                    //用箭头xld指示边缘搜索方向，并保持到显示对象
                    if ((int)(new HTuple(hv_i.TupleEqual(0))) != 0)
                    {
                        hv_RowL2 = hv_RowE + ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleSin()));
                        hv_RowL1 = hv_RowE - ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleSin()));
                        hv_ColL2 = hv_ColE + ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleCos()));
                        hv_ColL1 = hv_ColE - ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleCos()));
                        ho_Arrow1.Dispose();
                        Gen_arrow_contour_xld(out ho_Arrow1, hv_RowL1, hv_ColL1, hv_RowL2, hv_ColL2,
                            25, 25);
                        OTemp[SP_O] = ho_Regions.CopyObj(1, -1);
                        SP_O++;
                        ho_Regions.Dispose();
                        HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_Arrow1, out ho_Regions);
                        OTemp[SP_O - 1].Dispose();
                        SP_O = 0;
                    }
                }

                ho_ContOut1.Dispose();
                ho_Contour.Dispose();
                ho_ContCircle.Dispose();
                ho_Cross.Dispose();
                ho_Rectangle1.Dispose();
                ho_Arrow1.Dispose();

                return;
            }
            catch (HalconException ex)
            {
                ho_ContOut1.Dispose();
                ho_Contour.Dispose();
                ho_ContCircle.Dispose();
                ho_Cross.Dispose();
                ho_Rectangle1.Dispose();
                ho_Arrow1.Dispose();

                //throw HDevExpDefaultException;
            }
        }

        /// <summary>
        /// 拟合圆
        /// </summary>
        /// <param name="ho_Circle">输出拟合圆的xld</param>
        /// <param name="hv_Rows">拟合圆的输入y数组</param>
        /// <param name="hv_Cols">拟合圆的输入x数组</param>
        /// <param name="hv_ActiveNum">最小有效点数</param>
        /// <param name="hv_ArcType">拟合圆弧类型：'arc'圆弧；'circle'圆</param>
        /// <param name="hv_RowCenter">拟合的圆中心y</param>
        /// <param name="hv_ColCenter">拟合的圆中心x</param>
        /// <param name="hv_Radius">拟合的圆半径</param>
        /// <param name="hv_StartPhi"></param>
        /// <param name="hv_EndPhi"></param>
        /// <param name="hv_PointOrder"></param>
        /// <param name="hv_ArcAngle"></param>
        public static void Pts_to_best_circle(out HObject ho_Circle, HTuple hv_Rows, HTuple hv_Cols,
            HTuple hv_ActiveNum, HTuple hv_ArcType, out HTuple hv_RowCenter, out HTuple hv_ColCenter,
            out HTuple hv_Radius, out HTuple hv_StartPhi, out HTuple hv_EndPhi, out HTuple hv_PointOrder,
            out HTuple hv_ArcAngle)
        {
            // Local iconic variables
            HObject ho_Contour = null;
            // Local control variables
            HTuple hv_Length, hv_Length1 = new HTuple();
            HTuple hv_CircleLength = new HTuple();

            // Initialize local and output iconic variables
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            //初始化
            hv_RowCenter = 0;
            hv_ColCenter = 0;
            hv_Radius = 0;
            hv_StartPhi = new HTuple();
            hv_EndPhi = new HTuple();
            hv_PointOrder = new HTuple();
            hv_ArcAngle = new HTuple();
            try
            {          //产生一个空的直线对象，用于保存拟合后的圆
                ho_Circle.Dispose();
                HOperatorSet.GenEmptyObj(out ho_Circle);
                //计算边缘数量
                HOperatorSet.TupleLength(hv_Cols, out hv_Length);
                //当边缘数量不小于有效点数时进行拟合
                if ((int)((new HTuple(hv_Length.TupleGreaterEqual(hv_ActiveNum))).TupleAnd(
                    new HTuple(hv_ActiveNum.TupleGreater(2)))) != 0)
                {
                    //halcon的拟合是基于xld的，需要把边缘连接成xld
                    if ((int)(new HTuple(hv_ArcType.TupleEqual("circle"))) != 0)
                    {
                        //如果是闭合的圆，轮廓需要首尾相连
                        ho_Contour.Dispose();
                        HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_Rows.TupleConcat(hv_Rows.TupleSelect(
                            0)), hv_Cols.TupleConcat(hv_Cols.TupleSelect(0)));
                    }
                    else
                    {
                        ho_Contour.Dispose();
                        HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_Rows, hv_Cols);
                    }
                    HTuple claset = ho_Contour.GetObjClass();
                    //拟合圆。使用的算法是''geotukey''，其他算法请参考fit_circle_contour_xld的描述部分。
                    HOperatorSet.FitCircleContourXld(ho_Contour, "geotukey", -1, 0, 0, 3, 2,
                        out hv_RowCenter, out hv_ColCenter, out hv_Radius, out hv_StartPhi, out hv_EndPhi,
                        out hv_PointOrder);
                    //判断拟合结果是否有效：如果拟合成功，数组中元素的数量大于0
                    HOperatorSet.TupleLength(hv_StartPhi, out hv_Length1);
                    if ((int)(new HTuple(hv_Length1.TupleLess(1))) != 0)
                    {
                        ho_Contour.Dispose();

                        return;
                    }
                    //根据拟合结果，产生直线xld
                    if ((int)(new HTuple(hv_ArcType.TupleEqual("arc"))) != 0)
                    {
                        //判断圆弧的方向：顺时针还是逆时针
                        //halcon求圆弧会出现方向混乱的问题
                        //tuple_mean (Rows, RowsMean)
                        //tuple_mean (Cols, ColsMean)
                        //gen_cross_contour_xld (Cross, RowsMean, ColsMean, 6, 0.785398)
                        //gen_circle_contour_xld (Circle1, RowCenter, ColCenter, Radius, StartPhi, EndPhi, 'positive', 1)
                        //求轮廓1中心
                        //area_center_points_xld (Circle1, Area, Row1, Column1)
                        //gen_circle_contour_xld (Circle2, RowCenter, ColCenter, Radius, StartPhi, EndPhi, 'negative', 1)
                        //求轮廓2中心
                        //area_center_points_xld (Circle2, Area, Row2, Column2)
                        //distance_pp (RowsMean, ColsMean, Row1, Column1, Distance1)
                        //distance_pp (RowsMean, ColsMean, Row2, Column2, Distance2)
                        //ArcAngle := EndPhi-StartPhi
                        //if (Distance1<Distance2)

                        //PointOrder := 'positive'
                        //copy_obj (Circle1, Circle, 1, 1)
                        //else

                        //PointOrder := 'negative'
                        //if (abs(ArcAngle)>3.1415926)
                        //ArcAngle := ArcAngle-2.0*3.1415926
                        //endif
                        //copy_obj (Circle2, Circle, 1, 1)
                        //endif
                        ho_Circle.Dispose();
                        HOperatorSet.GenCircleContourXld(out ho_Circle, hv_RowCenter, hv_ColCenter,
                            hv_Radius, hv_StartPhi, hv_EndPhi, hv_PointOrder, 1);

                        HOperatorSet.LengthXld(ho_Circle, out hv_CircleLength);
                        hv_ArcAngle = hv_EndPhi - hv_StartPhi;
                        if ((int)(new HTuple(hv_CircleLength.TupleGreater(((new HTuple(180)).TupleRad()
                            ) * hv_Radius))) != 0)
                        {
                            if ((int)(new HTuple(((hv_ArcAngle.TupleAbs())).TupleLess((new HTuple(180)).TupleRad()
                                ))) != 0)
                            {
                                if ((int)(new HTuple(hv_ArcAngle.TupleGreater(0))) != 0)
                                {
                                    hv_ArcAngle = ((new HTuple(360)).TupleRad()) - hv_ArcAngle;
                                }
                                else
                                {
                                    hv_ArcAngle = ((new HTuple(360)).TupleRad()) + hv_ArcAngle;
                                }
                            }
                        }
                        else
                        {
                            if ((int)(new HTuple(hv_CircleLength.TupleLess(((new HTuple(180)).TupleRad()
                                ) * hv_Radius))) != 0)
                            {
                                if ((int)(new HTuple(((hv_ArcAngle.TupleAbs())).TupleGreater((new HTuple(180)).TupleRad()
                                    ))) != 0)
                                {
                                    if ((int)(new HTuple(hv_ArcAngle.TupleGreater(0))) != 0)
                                    {
                                        hv_ArcAngle = hv_ArcAngle - ((new HTuple(360)).TupleRad());
                                    }
                                    else
                                    {
                                        hv_ArcAngle = ((new HTuple(360)).TupleRad()) + hv_ArcAngle;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        hv_StartPhi = 0;
                        hv_EndPhi = (new HTuple(360)).TupleRad();
                        hv_ArcAngle = (new HTuple(360)).TupleRad();
                        ho_Circle.Dispose();
                        HOperatorSet.GenCircleContourXld(out ho_Circle, hv_RowCenter, hv_ColCenter,
                            hv_Radius, hv_StartPhi, hv_EndPhi, hv_PointOrder, 1);
                    }
                }

                ho_Contour.Dispose();

                return;
            }
            catch (HalconException ex)
            {
                ho_Contour.Dispose();

                //  throw HDevExpDefaultException;
            }
        }

        /// <summary>
        /// 拟合直线参数
        /// </summary>
        /// <param name="ho_Line">直线XLD</param>
        /// <param name="hv_Rows">y数组</param>
        /// <param name="hv_Cols">x数组</param>
        /// <param name="hv_ActiveNum">有效点数</param>
        /// <param name="hv_Row1">直线起点y</param>
        /// <param name="hv_Column1">起点x</param>
        /// <param name="hv_Row2">终点y</param>
        /// <param name="hv_Column2">终点x</param>
        public static void Pts_to_best_line(out HObject ho_Line, HTuple hv_Rows, HTuple hv_Cols,
            HTuple hv_ActiveNum, out HTuple hv_Row1, out HTuple hv_Column1, out HTuple hv_Row2,
            out HTuple hv_Column2)
        {
            hv_Row1 = 0;
            hv_Column1 = 0;
            hv_Row2 = 0;
            hv_Column2 = 0;
            HObject ho_Contour = new HObject();
            //初始化
            HTuple hv_Length, hv_Nr = new HTuple(), hv_Nc = new HTuple();
            HTuple hv_Dist = new HTuple(), hv_Length1 = new HTuple();
            // Initialize local and output iconic variables
            HOperatorSet.GenEmptyObj(out ho_Line);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            try
            {
                //产生一个空的直线对象，用于保存拟合后的直线
                //计算边缘数量
                HOperatorSet.TupleLength(hv_Cols, out hv_Length);
                //当边缘数量不小于有效点数时进行拟合
                if ((int)((new HTuple(hv_Length.TupleGreaterEqual(hv_ActiveNum))).TupleAnd(
                    new HTuple(hv_ActiveNum.TupleGreater(1)))) != 0)
                {
                    //halcon的拟合是基于xld的，需要把边缘连接成xld

                    HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_Rows, hv_Cols);
                    //拟合直线。使用的算法是'tukey'，其他算法请参考fit_line_contour_xld的描述部分。
                    HOperatorSet.FitLineContourXld(ho_Contour, "tukey", -1, 0, 5, 2, out hv_Row1,
                        out hv_Column1, out hv_Row2, out hv_Column2, out hv_Nr, out hv_Nc, out hv_Dist);
                    //判断拟合结果是否有效：如果拟合成功，数组中元素的数量大于0
                    HOperatorSet.TupleLength(hv_Dist, out hv_Length1);
                    if ((int)(new HTuple(hv_Length1.TupleLess(1))) != 0)
                    {
                        ho_Contour.Dispose();
                        return;
                    }
                    //根据拟合结果，产生直线xld

                    HOperatorSet.GenContourPolygonXld(out ho_Line, hv_Row1.TupleConcat(hv_Row2),
                        hv_Column1.TupleConcat(hv_Column2));
                }

                return;
            }
            catch (HalconException)
            {
                ho_Line.Dispose();
                ho_Contour.Dispose();
            }
        }

        /// <summary>
        /// 直线拟合查找
        /// </summary>
        /// <param name="ho_Image">输入图像</param>
        /// <param name="ho_Regions">查找的区域ROL</param>
        /// <param name="hv_Elements">检测点数</param>
        /// <param name="hv_DetectHeight">卡尺高</param>
        /// <param name="hv_DetectWidth">卡尺宽</param>
        /// <param name="hv_Sigma">高斯滤波因子</param>
        /// <param name="hv_Threshold">边缘幅度阈值</param>
        /// <param name="hv_Transition">极性： positive表示由黑到白 negative表示由白到黑 all表示以上两种方向</param>
        /// <param name="hv_Select">first表示选择第一点 last表示选择最后一点 max表示选择边缘幅度最强点</param>
        /// <param name="hv_Row1">直线ROI起点的y值</param>
        /// <param name="hv_Column1">直线ROI起点的x值</param>
        /// <param name="hv_Row2"></param>
        /// <param name="hv_Column2"></param>
        /// <param name="hv_ResultRow">检测到的边缘的y坐标数组</param>
        /// <param name="hv_ResultColumn">检测到的边缘的x坐标数组</param>
        public static void Rake(HObject ho_Image, out HObject ho_Regions, HTuple hv_Elements,
            HTuple hv_DetectHeight, HTuple hv_DetectWidth, HTuple hv_Sigma, HTuple hv_Threshold,
            HTuple hv_Transition, HTuple hv_Select, HTuple hv_Row1, HTuple hv_Column1, HTuple hv_Row2,
            HTuple hv_Column2, out HTuple hv_ResultRow, out HTuple hv_ResultColumn)
        {
            // Stack for temporary objects
            HObject[] OTemp = new HObject[20];
            long SP_O = 0;

            // Local iconic variables

            HObject ho_RegionLines, ho_Rectangle = null;
            HObject ho_Arrow1 = null;

            // Local control variables

            HTuple hv_Width, hv_Height, hv_ATan, hv_i;
            HTuple hv_RowC = new HTuple(), hv_ColC = new HTuple(), hv_Distance = new HTuple();
            HTuple hv_RowL2 = new HTuple(), hv_RowL1 = new HTuple(), hv_ColL2 = new HTuple();
            HTuple hv_ColL1 = new HTuple(), hv_MsrHandle_Measure = new HTuple();
            HTuple hv_RowEdge = new HTuple(), hv_ColEdge = new HTuple();
            HTuple hv_Amplitude = new HTuple(), hv_tRow = new HTuple();
            HTuple hv_tCol = new HTuple(), hv_t = new HTuple(), hv_Number = new HTuple();
            HTuple hv_j = new HTuple();

            HTuple hv_DetectWidth_COPY_INP_TMP = hv_DetectWidth.Clone();
            HTuple hv_Select_COPY_INP_TMP = hv_Select.Clone();
            HTuple hv_Transition_COPY_INP_TMP = hv_Transition.Clone();

            // Initialize local and output iconic variables
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_RegionLines);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_Arrow1);
            //产生一个空显示对象，用于显示
            ho_Regions.Dispose();
            HOperatorSet.GenEmptyObj(out ho_Regions);
            //初始化边缘坐标数组
            hv_ResultRow = new HTuple();
            hv_ResultColumn = new HTuple();

            try
            {
                //获取图像尺寸
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                //产生直线xld
                HOperatorSet.GenContourPolygonXld(out ho_RegionLines, hv_Row1.TupleConcat(hv_Row2),
                    hv_Column1.TupleConcat(hv_Column2));
                //存储到显示对象
                //OTemp[SP_O] = ho_Regions.CopyObj(1, -1);
                //SP_O++;

                //HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_RegionLines, out ho_Regions);
                //OTemp[SP_O - 1].Dispose();
                SP_O = 0;
                //计算直线与x轴的夹角，逆时针方向为正向。
                HOperatorSet.AngleLx(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_ATan);

                //边缘检测方向垂直于检测直线：直线方向正向旋转90°为边缘检测方向
                hv_ATan = hv_ATan + ((new HTuple(90)).TupleRad());

                //根据检测直线按顺序产生测量区域矩形，并存储到显示对象
                for (hv_i = 1; hv_i.Continue(hv_Elements, 1); hv_i = hv_i.TupleAdd(1))
                {
                    //RowC := Row1+(((Row2-Row1)*i)/(Elements+1))
                    //ColC := Column1+(Column2-Column1)*i/(Elements+1)
                    //if (RowC>Height-1 or RowC<0 or ColC>Width-1 or ColC<0)
                    //continue
                    //endif
                    //如果只有一个测量矩形，作为卡尺工具，宽度为检测直线的长度
                    if ((int)(new HTuple(hv_Elements.TupleEqual(1))) != 0)
                    {
                        hv_RowC = (hv_Row1 + hv_Row2) * 0.5;
                        hv_ColC = (hv_Column1 + hv_Column2) * 0.5;
                        //判断是否超出图像,超出不检测边缘
                        if ((int)((new HTuple((new HTuple((new HTuple(hv_RowC.TupleGreater(hv_Height - 1))).TupleOr(
                            new HTuple(hv_RowC.TupleLess(0))))).TupleOr(new HTuple(hv_ColC.TupleGreater(
                            hv_Width - 1))))).TupleOr(new HTuple(hv_ColC.TupleLess(0)))) != 0)
                        {
                            continue;
                        }
                        HOperatorSet.DistancePp(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_Distance);
                        hv_DetectWidth_COPY_INP_TMP = hv_Distance.Clone();
                        ho_Rectangle.Dispose();
                        HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle, hv_RowC, hv_ColC,
                            hv_ATan, hv_DetectHeight / 2, hv_Distance / 2);
                    }
                    else
                    {
                        //如果有多个测量矩形，产生该测量矩形xld
                        hv_RowC = hv_Row1 + (((hv_Row2 - hv_Row1) * (hv_i - 1)) / (hv_Elements - 1));
                        hv_ColC = hv_Column1 + (((hv_Column2 - hv_Column1) * (hv_i - 1)) / (hv_Elements - 1));
                        //判断是否超出图像,超出不检测边缘
                        if ((int)((new HTuple((new HTuple((new HTuple(hv_RowC.TupleGreater(hv_Height - 1))).TupleOr(
                            new HTuple(hv_RowC.TupleLess(0))))).TupleOr(new HTuple(hv_ColC.TupleGreater(
                            hv_Width - 1))))).TupleOr(new HTuple(hv_ColC.TupleLess(0)))) != 0)
                        {
                            continue;
                        }
                        ho_Rectangle.Dispose();
                        HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle, hv_RowC, hv_ColC,
                            hv_ATan, hv_DetectHeight / 2, hv_DetectWidth_COPY_INP_TMP / 2);
                    }

                    //把测量矩形xld存储到显示对象
                    OTemp[SP_O] = ho_Regions.CopyObj(1, -1);
                    SP_O++;
                    ho_Regions.Dispose();
                    HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_Rectangle, out ho_Regions);
                    OTemp[SP_O - 1].Dispose();
                    SP_O = 0;
                    if ((int)(new HTuple(hv_i.TupleEqual(1))) != 0)
                    {
                        //在第一个测量矩形绘制一个箭头xld，用于只是边缘检测方向
                        hv_RowL2 = hv_RowC + ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleSin()));
                        hv_RowL1 = hv_RowC - ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleSin()));
                        hv_ColL2 = hv_ColC + ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleCos()));
                        hv_ColL1 = hv_ColC - ((hv_DetectHeight / 2) * (((-hv_ATan)).TupleCos()));
                        ho_Arrow1.Dispose();
                        Gen_arrow_contour_xld(out ho_Arrow1, hv_RowL1, hv_ColL1, hv_RowL2, hv_ColL2,
                            25, 25);
                        //把xld存储到显示对象
                        OTemp[SP_O] = ho_Regions.CopyObj(1, -1);
                        SP_O++;
                        ho_Regions.Dispose();
                        HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_Arrow1, out ho_Regions);
                        OTemp[SP_O - 1].Dispose();
                        SP_O = 0;
                    }
                    //产生测量对象句柄
                    HOperatorSet.GenMeasureRectangle2(hv_RowC, hv_ColC, hv_ATan, hv_DetectHeight / 2,
                        hv_DetectWidth_COPY_INP_TMP / 2, hv_Width, hv_Height, "nearest_neighbor",
                        out hv_MsrHandle_Measure);

                    //设置极性
                    if ((int)(new HTuple(hv_Transition_COPY_INP_TMP.TupleEqual("negative"))) != 0)
                    {
                        hv_Transition_COPY_INP_TMP = "negative";
                    }
                    else
                    {
                        if ((int)(new HTuple(hv_Transition_COPY_INP_TMP.TupleEqual("positive"))) != 0)
                        {
                            hv_Transition_COPY_INP_TMP = "positive";
                        }
                        else
                        {
                            hv_Transition_COPY_INP_TMP = "all";
                        }
                    }
                    //设置边缘位置。最强点是从所有边缘中选择幅度绝对值最大点，需要设置为'all'
                    if ((int)(new HTuple(hv_Select_COPY_INP_TMP.TupleEqual("first"))) != 0)
                    {
                        hv_Select_COPY_INP_TMP = "first";
                    }
                    else
                    {
                        if ((int)(new HTuple(hv_Select_COPY_INP_TMP.TupleEqual("last"))) != 0)
                        {
                            hv_Select_COPY_INP_TMP = "last";
                        }
                        else
                        {
                            hv_Select_COPY_INP_TMP = "all";
                        }
                    }
                    //检测边缘
                    HOperatorSet.MeasurePos(ho_Image, hv_MsrHandle_Measure, hv_Sigma, hv_Threshold,
                        hv_Transition_COPY_INP_TMP, hv_Select_COPY_INP_TMP, out hv_RowEdge, out hv_ColEdge,
                        out hv_Amplitude, out hv_Distance);
                    //清除测量对象句柄
                    HOperatorSet.CloseMeasure(hv_MsrHandle_Measure);

                    //临时变量初始化
                    //tRow，tCol保存找到指定边缘的坐标
                    hv_tRow = 0;
                    hv_tCol = 0;
                    //t保存边缘的幅度绝对值
                    hv_t = 0;
                    //找到的边缘必须至少为1个
                    HOperatorSet.TupleLength(hv_RowEdge, out hv_Number);
                    if ((int)(new HTuple(hv_Number.TupleLess(1))) != 0)
                    {
                        continue;
                    }
                    //有多个边缘时，选择幅度绝对值最大的边缘
                    for (hv_j = 0; hv_j.Continue(hv_Number - 1, 1); hv_j = hv_j.TupleAdd(1))
                    {
                        if ((int)(new HTuple(((((hv_Amplitude.TupleSelect(hv_j))).TupleAbs())).TupleGreater(
                            hv_t))) != 0)
                        {
                            hv_tRow = hv_RowEdge.TupleSelect(hv_j);
                            hv_tCol = hv_ColEdge.TupleSelect(hv_j);
                            hv_t = ((hv_Amplitude.TupleSelect(hv_j))).TupleAbs();
                        }
                    }
                    //把找到的边缘保存在输出数组
                    if ((int)(new HTuple(hv_t.TupleGreater(0))) != 0)
                    {
                        hv_ResultRow = hv_ResultRow.TupleConcat(hv_tRow);
                        hv_ResultColumn = hv_ResultColumn.TupleConcat(hv_tCol);
                    }
                }

                ho_RegionLines.Dispose();
                ho_Rectangle.Dispose();
                ho_Arrow1.Dispose();
                return;
            }
            catch (HalconException)
            {
                ho_RegionLines.Dispose();
                ho_Rectangle.Dispose();
                ho_Arrow1.Dispose();
            }
        }

        /// <summary>
        /// 圆查找拟合
        /// </summary>
        /// <param name="ho_Image">输入图像</param>
        /// <param name="ho_Regions">输出边缘点检测区域及方向</param>
        /// <param name="hv_Elements">检测边缘点数</param>
        /// <param name="hv_DetectHeight">卡尺工具的高度</param>
        /// <param name="hv_DetectWidth">卡尺工具的宽</param>
        /// <param name="hv_Sigma">高斯滤波因子</param>
        /// <param name="hv_Threshold">边缘检测阈值，又叫边缘强度</param>
        /// <param name="hv_Transition">极性： positive表示由黑到白 negative表示由白到黑 all表示以上两种方向</param>
        /// <param name="hv_Select">first表示选择第一点 last表示选择最后一点 max表示选择边缘强度最强点</param>
        /// <param name="hv_ROIRows">检测区域起点的y值</param>
        /// <param name="hv_ROICols">检测区域起点的x值</param>
        /// <param name="hv_Direct">'inner'表示检测方向由边缘点指向圆心; 'outer'表示检测方向由圆心指向边缘点</param>
        /// <param name="hv_ResultRow">检测到的边缘点的y坐标数组</param>
        /// <param name="hv_ResultColumn">检测到的边缘点的x坐标数组</param>
        /// <param name="hv_ArcType">拟合圆弧类型：'arc'圆弧；'circle'圆</param>
        public static void Spoke(HObject ho_Image, out HObject ho_Regions, HTuple hv_Elements,
            HTuple hv_DetectHeight, HTuple hv_DetectWidth, HTuple hv_Sigma, HTuple hv_Threshold,
            HTuple hv_Transition, HTuple hv_Select, HTuple hv_ROIRows, HTuple hv_ROICols,
            HTuple hv_Direct, out HTuple hv_ResultRow, out HTuple hv_ResultColumn, out HTuple hv_ArcType)
        {
            // Stack for temporary objects
            HObject[] OTemp = new HObject[20];
            long SP_O = 0;

            // Local iconic variables

            HObject ho_Contour, ho_ContCircle, ho_Rectangle1 = null;
            HObject ho_Arrow1 = null;

            // Local control variables

            HTuple hv_Width, hv_Height, hv_RowC, hv_ColumnC;
            HTuple hv_Radius, hv_StartPhi, hv_EndPhi, hv_PointOrder;
            HTuple hv_RowXLD, hv_ColXLD, hv_Length2, hv_WindowHandle = new HTuple();
            HTuple hv_i, hv_j = new HTuple(), hv_RowE = new HTuple(), hv_ColE = new HTuple();
            HTuple hv_ATan = new HTuple(), hv_RowL2 = new HTuple(), hv_RowL1 = new HTuple();
            HTuple hv_ColL2 = new HTuple(), hv_ColL1 = new HTuple(), hv_MsrHandle_Measure = new HTuple();
            HTuple hv_RowEdge = new HTuple(), hv_ColEdge = new HTuple();
            HTuple hv_Amplitude = new HTuple(), hv_Distance = new HTuple();
            HTuple hv_tRow = new HTuple(), hv_tCol = new HTuple(), hv_t = new HTuple();
            HTuple hv_Number = new HTuple(), hv_k = new HTuple();

            HTuple hv_Select_COPY_INP_TMP = hv_Select.Clone();
            HTuple hv_Transition_COPY_INP_TMP = hv_Transition.Clone();

            // Initialize local and output iconic variables
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_ContCircle);
            HOperatorSet.GenEmptyObj(out ho_Rectangle1);
            HOperatorSet.GenEmptyObj(out ho_Arrow1);
            //产生一个空显示对象，用于显示
            //初始化边缘坐标数组
            hv_ResultRow = new HTuple();
            hv_ResultColumn = new HTuple();
            hv_ArcType = new HTuple();
            try
            {
                //获取图像尺寸
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);

                //产生xld
                ho_Contour.Dispose();
                HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_ROIRows, hv_ROICols);
                //用回归线法（不抛出异常点，所有点权重一样）拟合圆
                HOperatorSet.FitCircleContourXld(ho_Contour, "algebraic", -1, 0, 0, 3, 2, out hv_RowC,
                    out hv_ColumnC, out hv_Radius, out hv_StartPhi, out hv_EndPhi, out hv_PointOrder);
                //根据拟合结果产生xld，并保持到显示对象
                ho_ContCircle.Dispose();
                HOperatorSet.GenCircleContourXld(out ho_ContCircle, hv_RowC, hv_ColumnC, hv_Radius,
                    hv_StartPhi, hv_EndPhi, hv_PointOrder, 3);
                OTemp[SP_O] = ho_Regions.CopyObj(1, -1);
                SP_O++;
                ho_Regions.Dispose();
                HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_ContCircle, out ho_Regions);
                OTemp[SP_O - 1].Dispose();
                SP_O = 0;

                //获取圆或圆弧xld上的点坐标
                HOperatorSet.GetContourXld(ho_ContCircle, out hv_RowXLD, out hv_ColXLD);

                //求圆或圆弧xld上的点的数量
                HOperatorSet.TupleLength(hv_ColXLD, out hv_Length2);
                if ((int)(new HTuple(hv_Elements.TupleLess(3))) != 0)
                {
                    Disp_message(hv_WindowHandle, "检测的边缘数量太少，请重新设置!",
                        52, 12, true);
                    ho_Contour.Dispose();
                    ho_ContCircle.Dispose();
                    ho_Rectangle1.Dispose();
                    ho_Arrow1.Dispose();

                    return;
                }
                //如果xld是圆弧，有Length2个点，从起点开始，等间距（间距为Length2/(Elements-1)）取Elements个点，作为卡尺工具的中点
                //如果xld是圆，有Length2个点，以0°为起点，从起点开始，等间距（间距为Length2/(Elements)）取Elements个点，作为卡尺工具的中点
                for (hv_i = 0; hv_i.Continue(hv_Elements - 1, 1); hv_i = hv_i.TupleAdd(1))
                {
                    if ((int)(new HTuple(((hv_RowXLD.TupleSelect(0))).TupleEqual(hv_RowXLD.TupleSelect(
                        hv_Length2 - 1)))) != 0)
                    {
                        //xld的起点和终点坐标相对，为圆
                        HOperatorSet.TupleInt(((1.0 * hv_Length2) / hv_Elements) * hv_i, out hv_j);
                        hv_ArcType = "circle";
                    }
                    else
                    {
                        //否则为圆弧
                        HOperatorSet.TupleInt(((1.0 * hv_Length2) / (hv_Elements - 1)) * hv_i, out hv_j);
                        hv_ArcType = "arc";
                    }
                    //索引越界，强制赋值为最后一个索引
                    if ((int)(new HTuple(hv_j.TupleGreaterEqual(hv_Length2))) != 0)
                    {
                        hv_j = hv_Length2 - 1;
                        //continue
                    }
                    //获取卡尺工具中心
                    hv_RowE = hv_RowXLD.TupleSelect(hv_j);
                    hv_ColE = hv_ColXLD.TupleSelect(hv_j);

                    //超出图像区域，不检测，否则容易报异常
                    if ((int)((new HTuple((new HTuple((new HTuple(hv_RowE.TupleGreater(hv_Height - 1))).TupleOr(
                        new HTuple(hv_RowE.TupleLess(0))))).TupleOr(new HTuple(hv_ColE.TupleGreater(
                        hv_Width - 1))))).TupleOr(new HTuple(hv_ColE.TupleLess(0)))) != 0)
                    {
                        continue;
                    }
                    //边缘搜索方向类型：'inner'搜索方向由圆外指向圆心；'outer'搜索方向由圆心指向圆外
                    if ((int)(new HTuple(hv_Direct.TupleEqual("inner"))) != 0)
                    {
                        //求卡尺工具的边缘搜索方向
                        //求圆心指向边缘的矢量的角度
                        HOperatorSet.TupleAtan2((-hv_RowE) + hv_RowC, hv_ColE - hv_ColumnC, out hv_ATan);
                        //角度反向
                        hv_ATan = ((new HTuple(180)).TupleRad()) + hv_ATan;
                    }
                    else
                    {
                        //求卡尺工具的边缘搜索方向
                        //求圆心指向边缘的矢量的角度
                        HOperatorSet.TupleAtan2((-hv_RowE) + hv_RowC, hv_ColE - hv_ColumnC, out hv_ATan);
                    }

                    //产生卡尺xld，并保持到显示对象
                    ho_Rectangle1.Dispose();
                    HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle1, hv_RowE, hv_ColE,
                        hv_ATan, hv_DetectHeight.TupleInt() / 2, hv_DetectWidth.TupleInt() / 2);
                    OTemp[SP_O] = ho_Regions.CopyObj(1, -1);
                    SP_O++;
                    ho_Regions.Dispose();
                    HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_Rectangle1, out ho_Regions);
                    OTemp[SP_O - 1].Dispose();
                    SP_O = 0;
                    //用箭头xld指示边缘搜索方向，并保持到显示对象
                    if ((int)(new HTuple(hv_i.TupleEqual(0))) != 0)
                    {
                        hv_RowL2 = hv_RowE + ((hv_DetectHeight.TupleInt() / 2) * (((-hv_ATan)).TupleSin()));
                        hv_RowL1 = hv_RowE - ((hv_DetectHeight.TupleInt() / 2) * (((-hv_ATan)).TupleSin()));
                        hv_ColL2 = hv_ColE + ((hv_DetectHeight.TupleInt() / 2) * (((-hv_ATan)).TupleCos()));
                        hv_ColL1 = hv_ColE - ((hv_DetectHeight.TupleInt() / 2) * (((-hv_ATan)).TupleCos()));
                        ho_Arrow1.Dispose();
                        Gen_arrow_contour_xld(out ho_Arrow1, hv_RowL1, hv_ColL1, hv_RowL2, hv_ColL2,
                            25, 25);
                        OTemp[SP_O] = ho_Regions.CopyObj(1, -1);
                        SP_O++;
                        ho_Regions.Dispose();
                        HOperatorSet.ConcatObj(OTemp[SP_O - 1], ho_Arrow1, out ho_Regions);
                        OTemp[SP_O - 1].Dispose();
                        SP_O = 0;
                    }

                    //产生测量对象句柄
                    HOperatorSet.GenMeasureRectangle2(hv_RowE, hv_ColE, hv_ATan, hv_DetectHeight.TupleInt() / 2,
                        hv_DetectWidth.TupleInt() / 2, hv_Width, hv_Height, "nearest_neighbor", out hv_MsrHandle_Measure);

                    //设置极性
                    if ((int)(new HTuple(hv_Transition_COPY_INP_TMP.TupleEqual("negative"))) != 0)
                    {
                        hv_Transition_COPY_INP_TMP = "negative";
                    }
                    else
                    {
                        if ((int)(new HTuple(hv_Transition_COPY_INP_TMP.TupleEqual("positive"))) != 0)
                        {
                            hv_Transition_COPY_INP_TMP = "positive";
                        }
                        else
                        {
                            hv_Transition_COPY_INP_TMP = "all";
                        }
                    }
                    //设置边缘位置。最强点是从所有边缘中选择幅度绝对值最大点，需要设置为'all'
                    if ((int)(new HTuple(hv_Select_COPY_INP_TMP.TupleEqual("first"))) != 0)
                    {
                        hv_Select_COPY_INP_TMP = "first";
                    }
                    else
                    {
                        if ((int)(new HTuple(hv_Select_COPY_INP_TMP.TupleEqual("last"))) != 0)
                        {
                            hv_Select_COPY_INP_TMP = "last";
                        }
                        else
                        {
                            hv_Select_COPY_INP_TMP = "all";
                        }
                    }
                    //检测边缘
                    HOperatorSet.MeasurePos(ho_Image, hv_MsrHandle_Measure, hv_Sigma, hv_Threshold,
                        hv_Transition_COPY_INP_TMP, hv_Select_COPY_INP_TMP, out hv_RowEdge, out hv_ColEdge,
                        out hv_Amplitude, out hv_Distance);
                    //清除测量对象句柄
                    HOperatorSet.CloseMeasure(hv_MsrHandle_Measure);
                    //临时变量初始化
                    //tRow，tCol保存找到指定边缘的坐标
                    hv_tRow = 0;
                    hv_tCol = 0;
                    //t保存边缘的幅度绝对值
                    hv_t = 0;
                    HOperatorSet.TupleLength(hv_RowEdge, out hv_Number);
                    //找到的边缘必须至少为1个
                    if ((int)(new HTuple(hv_Number.TupleLess(1))) != 0)
                    {
                        continue;
                    }
                    //有多个边缘时，选择幅度绝对值最大的边缘
                    for (hv_k = 0; hv_k.Continue(hv_Number - 1, 1); hv_k = hv_k.TupleAdd(1))
                    {
                        if ((int)(new HTuple(((((hv_Amplitude.TupleSelect(hv_k))).TupleAbs())).TupleGreater(
                            hv_t))) != 0)
                        {
                            hv_tRow = hv_RowEdge.TupleSelect(hv_k);
                            hv_tCol = hv_ColEdge.TupleSelect(hv_k);
                            hv_t = ((hv_Amplitude.TupleSelect(hv_k))).TupleAbs();
                        }
                    }
                    //把找到的边缘保存在输出数组
                    if ((int)(new HTuple(hv_t.TupleGreater(0))) != 0)
                    {
                        hv_ResultRow = hv_ResultRow.TupleConcat(hv_tRow);
                        hv_ResultColumn = hv_ResultColumn.TupleConcat(hv_tCol);
                    }
                }

                ho_Contour.Dispose();
                ho_ContCircle.Dispose();
                ho_Rectangle1.Dispose();
                ho_Arrow1.Dispose();

                return;
            }
            catch (HalconException)
            {
                ho_Contour.Dispose();
                ho_ContCircle.Dispose();
                ho_Rectangle1.Dispose();
                ho_Arrow1.Dispose();

                //throw HDevExpDefaultException;
            }
        }

        /// <summary>
        /// 测量顶点
        /// </summary>
        /// <param name="ho_Image">图像</param>
        /// <param name="hv_Row">原点位置Row</param>
        /// <param name="hv_Coloumn">原点位置Col</param>
        /// <param name="hv_Phi">角度</param>
        /// <param name="hv_Length1">第一主轴长度</param>
        /// <param name="hv_Length2">第二主轴长度</param>
        /// <param name="hv_DetectWidth">测量点长度</param>
        /// <param name="hv_Sigma">高斯滤波因子</param>
        /// <param name="hv_Threshold">边缘幅度阈值</param>
        /// <param name="hv_Transition">positive表示由黑到白 negative表示由白到黑 all表示以上两种方向</param>
        /// <param name="hv_Select">first表示选择第一点 last表示选择最后一点 max表示选择边缘幅度最强点</param>
        /// <param name="hv_EdgeRows">输出点组Rows</param>
        /// <param name="hv_EdgeColumns">输出点组Cols</param>
        /// <param name="hv_ResultRow">输出顶点row</param>
        /// <param name="hv_ResultColumn">输出顶点col</param>
        public static void Peak(HObject ho_Image, HTuple hv_Row, HTuple hv_Coloumn, HTuple hv_Phi,
            HTuple hv_Length1, HTuple hv_Length2, HTuple hv_DetectWidth, HTuple hv_Sigma,
            HTuple hv_Threshold, HTuple hv_Transition, HTuple hv_Select, out HTuple hv_EdgeRows,
            out HTuple hv_EdgeColumns, out double hv_ResultRow, out double hv_ResultColumn)
        {
            // Local iconic variables
            HObject ho_Rectangle, ho_Regions1;
            // Local control variables
            HTuple hv_ROILineRow1 = null, hv_ROILineCol1 = null;
            HTuple hv_ROILineRow2 = null, hv_ROILineCol2 = null, hv_StdLineRow1 = null;
            HTuple hv_StdLineCol1 = null, hv_StdLineRow2 = null, hv_StdLineCol2 = null;
            HTuple hv_Cos = null, hv_Sin = null, hv_Col1 = null, hv_Row1 = null;
            HTuple hv_Col2 = null, hv_Row2 = null, hv_Col3 = null;
            HTuple hv_Row3 = null, hv_Col4 = null, hv_Row4 = null;
            HTuple hv_ResultRows = null, hv_ResultColumns = null, hv_Max = null;
            HTuple hv_i = new HTuple(), hv_Distance1 = new HTuple();
            // Initialize local and output iconic variables
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_Regions1);
            //初始化
            hv_ResultRow = 0;
            hv_ResultColumn = 0;
            hv_EdgeColumns = new HTuple();
            hv_EdgeRows = new HTuple();
            //仿射矩形Length2所在直线作为rake工具的ROI
            hv_ROILineRow1 = 0;
            hv_ROILineCol1 = 0;
            hv_ROILineRow2 = 0;
            hv_ROILineCol2 = 0;

            //仿射矩形方向所直线的边做基准线
            hv_StdLineRow1 = 0;
            hv_StdLineCol1 = 0;
            hv_StdLineRow2 = 0;
            hv_StdLineCol2 = 0;
            //判断仿射矩形是否有效
            if ((int)((new HTuple(hv_Length1.TupleLessEqual(0))).TupleOr(new HTuple(hv_Length2.TupleLessEqual(
                0)))) != 0)
            {
                ho_Rectangle.Dispose();
                ho_Regions1.Dispose();

                return;
            }
            //计算仿射矩形角度的正弦值、余弦值
            HOperatorSet.TupleCos(hv_Phi, out hv_Cos);
            HOperatorSet.TupleSin(hv_Phi, out hv_Sin);

            //矩形第一个端点坐标
            hv_Col1 = 1.0 * ((hv_Coloumn - (hv_Length1 * hv_Cos)) - (hv_Length2 * hv_Sin));
            hv_Row1 = 1.0 * (hv_Row - (((-hv_Length1) * hv_Sin) + (hv_Length2 * hv_Cos)));

            //矩形第二个端点坐标
            hv_Col2 = 1.0 * ((hv_Coloumn + (hv_Length1 * hv_Cos)) - (hv_Length2 * hv_Sin));
            hv_Row2 = 1.0 * (hv_Row - ((hv_Length1 * hv_Sin) + (hv_Length2 * hv_Cos)));

            //矩形第三个端点坐标
            hv_Col3 = 1.0 * ((hv_Coloumn + (hv_Length1 * hv_Cos)) + (hv_Length2 * hv_Sin));
            hv_Row3 = 1.0 * (hv_Row - ((hv_Length1 * hv_Sin) - (hv_Length2 * hv_Cos)));

            //矩形第四个端点坐标
            hv_Col4 = 1.0 * ((hv_Coloumn - (hv_Length1 * hv_Cos)) + (hv_Length2 * hv_Sin));
            hv_Row4 = 1.0 * (hv_Row - (((-hv_Length1) * hv_Sin) - (hv_Length2 * hv_Cos)));
            //仿射矩形方向所直线的边做基准线
            hv_StdLineRow1 = hv_Row2.Clone();
            hv_StdLineCol1 = hv_Col2.Clone();
            hv_StdLineRow2 = hv_Row3.Clone();
            hv_StdLineCol2 = hv_Col3.Clone();
            //仿射矩形Length2所在直线作为rake工具的ROI
            hv_ROILineRow1 = (hv_Row1 + hv_Row2) * 0.5;
            hv_ROILineCol1 = (hv_Col1 + hv_Col2) * 0.5;
            hv_ROILineRow2 = (hv_Row3 + hv_Row4) * 0.5;
            hv_ROILineCol2 = (hv_Col3 + hv_Col4) * 0.5;
            ho_Rectangle.Dispose();
            HOperatorSet.GenRectangle2ContourXld(out ho_Rectangle, hv_Row, hv_Coloumn, hv_Phi,
                hv_Length1, hv_Length2);
            ho_Regions1.Dispose();
            Rake(ho_Image, out ho_Regions1, hv_Length2, hv_Length1 * 2, hv_DetectWidth, hv_Sigma,
                hv_Threshold, hv_Transition, hv_Select, hv_ROILineRow1, hv_ROILineCol1, hv_ROILineRow2,
                hv_ROILineCol2, out hv_ResultRows, out hv_ResultColumns);
            //求所有边缘点到基准线的距离，保存最大距离及其对应的边缘点坐标，作为顶点
            hv_Max = 0;
            if ((int)(new HTuple((new HTuple(hv_ResultColumns.TupleLength())).TupleGreater(
                0))) != 0)
            {
                hv_EdgeRows = hv_ResultRows.Clone();
                hv_EdgeColumns = hv_ResultColumns.Clone();
                for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv_ResultColumns.TupleLength())) - 1); hv_i = (int)hv_i + 1)
                {
                    HOperatorSet.DistancePl(hv_ResultRows.TupleSelect(hv_i), hv_ResultColumns.TupleSelect(
                        hv_i), hv_StdLineRow1, hv_StdLineCol1, hv_StdLineRow2, hv_StdLineCol2,
                        out hv_Distance1);
                    if ((int)(new HTuple(hv_Max.TupleLess(hv_Distance1))) != 0)
                    {
                        hv_Max = hv_Distance1.Clone();
                        hv_ResultRow = hv_ResultRows.TupleSelect(hv_i);
                        hv_ResultColumn = hv_ResultColumns.TupleSelect(hv_i);
                    }
                }
            }
            ho_Rectangle.Dispose();
            ho_Regions1.Dispose();

            return;
        }

        /// <summary>
        /// 图像灰度值缩放
        /// </summary>
        /// <param name="ho_Image"></param>
        /// <param name="ho_ImageScaled"></param>
        /// <param name="hv_Min"></param>
        /// <param name="hv_Max"></param>
        public static void Scale_image_range(HObject ho_Image, out HObject ho_ImageScaled, HTuple hv_Min,
           HTuple hv_Max)
        {
            ho_ImageScaled = new HObject();
            if (ho_Image == null)
            {
                return;
            }
            // Stack for temporary objects
            HObject[] OTemp = new HObject[20];

            // Local iconic variables

            HObject ho_ImageSelected = null, ho_SelectedChannel = null;
            HObject ho_LowerRegion = null, ho_UpperRegion = null, ho_ImageSelectedScaled = null;

            // Local copy input parameter variables
            HObject ho_Image_COPY_INP_TMP;
            ho_Image_COPY_INP_TMP = ho_Image.CopyObj(1, -1);

            // Local control variables

            HTuple hv_LowerLimit = new HTuple(), hv_UpperLimit = new HTuple();
            HTuple hv_Mult = null, hv_Add = null, hv_NumImages = null;
            HTuple hv_ImageIndex = null, hv_Channels = new HTuple();
            HTuple hv_ChannelIndex = new HTuple(), hv_MinGray = new HTuple();
            HTuple hv_MaxGray = new HTuple(), hv_Range = new HTuple();
            HTuple hv_Max_COPY_INP_TMP = hv_Max.Clone();
            HTuple hv_Min_COPY_INP_TMP = hv_Min.Clone();

            // Initialize local and output iconic variables
            HOperatorSet.GenEmptyObj(out ho_ImageScaled);
            HOperatorSet.GenEmptyObj(out ho_ImageSelected);
            HOperatorSet.GenEmptyObj(out ho_SelectedChannel);
            HOperatorSet.GenEmptyObj(out ho_LowerRegion);
            HOperatorSet.GenEmptyObj(out ho_UpperRegion);
            HOperatorSet.GenEmptyObj(out ho_ImageSelectedScaled);

            if ((int)(new HTuple((new HTuple(hv_Min_COPY_INP_TMP.TupleLength())).TupleEqual(
                2))) != 0)
            {
                hv_LowerLimit = hv_Min_COPY_INP_TMP.TupleSelect(1);
                hv_Min_COPY_INP_TMP = hv_Min_COPY_INP_TMP.TupleSelect(0);
            }
            else
            {
                hv_LowerLimit = 0.0;
            }
            if ((int)(new HTuple((new HTuple(hv_Max_COPY_INP_TMP.TupleLength())).TupleEqual(
                2))) != 0)
            {
                hv_UpperLimit = hv_Max_COPY_INP_TMP.TupleSelect(1);
                hv_Max_COPY_INP_TMP = hv_Max_COPY_INP_TMP.TupleSelect(0);
            }
            else
            {
                hv_UpperLimit = 255.0;
            }
            //
            //Calculate scaling parameters.
            hv_Mult = (((hv_UpperLimit - hv_LowerLimit)).TupleReal()) / (hv_Max_COPY_INP_TMP - hv_Min_COPY_INP_TMP);
            hv_Add = ((-hv_Mult) * hv_Min_COPY_INP_TMP) + hv_LowerLimit;
            //
            //Scale image.
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.ScaleImage(ho_Image_COPY_INP_TMP, out ExpTmpOutVar_0, hv_Mult, hv_Add);
                ho_Image_COPY_INP_TMP.Dispose();
                ho_Image_COPY_INP_TMP = ExpTmpOutVar_0;
            }
            //
            //Clip gray values if necessary.
            //This must be done for each image and channel separately.
            ho_ImageScaled.Dispose();
            HOperatorSet.GenEmptyObj(out ho_ImageScaled);
            HOperatorSet.CountObj(ho_Image_COPY_INP_TMP, out hv_NumImages);
            HTuple end_val49 = hv_NumImages;
            HTuple step_val49 = 1;
            for (hv_ImageIndex = 1; hv_ImageIndex.Continue(end_val49, step_val49); hv_ImageIndex = hv_ImageIndex.TupleAdd(step_val49))
            {
                ho_ImageSelected.Dispose();
                HOperatorSet.SelectObj(ho_Image_COPY_INP_TMP, out ho_ImageSelected, hv_ImageIndex);
                HOperatorSet.CountChannels(ho_ImageSelected, out hv_Channels);
                HTuple end_val52 = hv_Channels;
                HTuple step_val52 = 1;
                for (hv_ChannelIndex = 1; hv_ChannelIndex.Continue(end_val52, step_val52); hv_ChannelIndex = hv_ChannelIndex.TupleAdd(step_val52))
                {
                    ho_SelectedChannel.Dispose();
                    HOperatorSet.AccessChannel(ho_ImageSelected, out ho_SelectedChannel, hv_ChannelIndex);
                    HOperatorSet.MinMaxGray(ho_SelectedChannel, ho_SelectedChannel, 0, out hv_MinGray,
                        out hv_MaxGray, out hv_Range);
                    ho_LowerRegion.Dispose();
                    HOperatorSet.Threshold(ho_SelectedChannel, out ho_LowerRegion, ((hv_MinGray.TupleConcat(
                        hv_LowerLimit))).TupleMin(), hv_LowerLimit);
                    ho_UpperRegion.Dispose();
                    HOperatorSet.Threshold(ho_SelectedChannel, out ho_UpperRegion, hv_UpperLimit,
                        ((hv_UpperLimit.TupleConcat(hv_MaxGray))).TupleMax());
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.PaintRegion(ho_LowerRegion, ho_SelectedChannel, out ExpTmpOutVar_0,
                            hv_LowerLimit, "fill");
                        ho_SelectedChannel.Dispose();
                        ho_SelectedChannel = ExpTmpOutVar_0;
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.PaintRegion(ho_UpperRegion, ho_SelectedChannel, out ExpTmpOutVar_0,
                            hv_UpperLimit, "fill");
                        ho_SelectedChannel.Dispose();
                        ho_SelectedChannel = ExpTmpOutVar_0;
                    }
                    if ((int)(new HTuple(hv_ChannelIndex.TupleEqual(1))) != 0)
                    {
                        ho_ImageSelectedScaled.Dispose();
                        HOperatorSet.CopyObj(ho_SelectedChannel, out ho_ImageSelectedScaled, 1,
                            1);
                    }
                    else
                    {
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.AppendChannel(ho_ImageSelectedScaled, ho_SelectedChannel,
                                out ExpTmpOutVar_0);
                            ho_ImageSelectedScaled.Dispose();
                            ho_ImageSelectedScaled = ExpTmpOutVar_0;
                        }
                    }
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_ImageScaled, ho_ImageSelectedScaled, out ExpTmpOutVar_0
                        );
                    ho_ImageScaled.Dispose();
                    ho_ImageScaled = ExpTmpOutVar_0;
                }
            }
            ho_Image_COPY_INP_TMP.Dispose();
            ho_ImageSelected.Dispose();
            ho_SelectedChannel.Dispose();
            ho_LowerRegion.Dispose();
            ho_UpperRegion.Dispose();
            ho_ImageSelectedScaled.Dispose();

            return;
        }

        /// <summary>
        /// 保存图像
        /// </summary>
        /// <param name="Image">图像</param>
        /// <param name="Path">地址</param>
        public static void Write_Image(HObject Image, string Path)
        {
            //图像为空，不保存
            if (Image.IsInitialized() == false)
            {
                return;
            }
            int nPos;
            string ImageType;
            Directory.CreateDirectory(Path);
            //通常保存文件的路径格式为xxxx.xxx，最后一个.后的字符为图像的扩展名，获取扩展名，作为write_image的输入参数
            //从右边开始，查询.的位置

            nPos = Path.LastIndexOf('.');
            if (nPos > -1)
            {
                //获取图像扩展名
                ImageType = Path.Substring(nPos + 1, Path.Length - nPos - 1);
            }
            else
                ImageType = "bmp";
            //保存图像
            HOperatorSet.WriteImage(Image, ImageType, 0, Path);
        }

        /// <summary>
        /// 读取图像
        /// </summary>
        /// <param name="Image"></param>
        /// <param name="Path">地址</param>
        /// <returns></returns>
        public static bool Read_Image(out HObject Image, string Path)
        {
            Image = new HObject();
            //文件是否存在
            try
            {
                HOperatorSet.ReadImage(out Image, Path);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        /// <summary>
        /// 保存变量Tuple
        /// </summary>
        /// <param name="Tuple"></param>
        /// <param name="Path">地址</param>
        public static void Write_Tuple(HTuple Tuple, string Path)
        {
            //数组为空，不保存
            if (Tuple.Length == 0)
            {
                return;
            }
            //ZazaniaoDll.CreateAllDirectoryEx(Path);
            Directory.CreateDirectory(Path);

            HOperatorSet.WriteTuple(Tuple, Path);
        }

        /// <summary>
        /// 读取变量Tuple
        /// </summary>
        /// <param name="Tuple"></param>
        /// <param name="Path">地址</param>
        /// <returns></returns>
        public static bool Read_Tuple(ref HTuple Tuple, string Path)
        {
            //文件是否存在
            try
            {
                HOperatorSet.ReadTuple(Path, out Tuple);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        /// <summary>
        /// 保存区域
        /// </summary>
        /// <param name="Region">区域</param>
        /// <param name="Path">地址</param>
        public static void Write_Region(HObject Region, string Path)
        {
            //区域为空，不保存
            if (!Region.IsInitialized())
            {
                return;
            }
            Directory.CreateDirectory(Path);

            HOperatorSet.WriteRegion(Region, Path);
        }

        /// <summary>
        /// 读取区域
        /// </summary>
        /// <param name="Region"></param>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static bool Read_Region(ref HObject Region, string Path)
        {
            try
            {
                Region.Dispose();
                HOperatorSet.ReadRegion(out Region, Path);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        /// <summary>
        /// 链接元素
        /// </summary>
        /// <param name="Obj1">元素1</param>
        /// <param name="Obj2">元素2</param>
        /// <param name="Obj3">新的元素</param>
        public static void Concat_Obj(ref HObject Obj1, ref HObject Obj2, ref HObject Obj3)
        {
            if (!Obj1.IsInitialized())
            {
                HOperatorSet.GenEmptyObj(out Obj1);
            }
            if (!Obj2.IsInitialized())
            {
                HOperatorSet.GenEmptyObj(out Obj2);
            }
            if (!(Obj1.IsInitialized()) || (Obj1.CountObj() < 1))
            {
                HOperatorSet.CopyObj(Obj1, out Obj3, 1, -1);
            }
            else
            {
                HOperatorSet.ConcatObj(Obj1, Obj2, out Obj3);
            }
        }

        /// <summary>
        /// 检测Object是否有效
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public static bool IsObjectValided(HObject Obj)
        {
            if (Obj == null || !Obj.IsInitialized())
            {
                return false;
            }
            if (Obj.CountObj() < 1)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 设置窗口字体大小
        /// </summary>
        /// <param name="window"></param>
        public static void SetFont(HTuple window)
        {
            try
            {
                //HOperatorSet.QueryFont(window, out HTuple hv_Fonts);
                //HOperatorSet.GetFix(window, out HTuple modet);
                if (HWindID.Bold)
                {
                    HOperatorSet.SetFont(window,HWindID.Font + "-Bold-" + HWindID.FontSize);
                }
                else
                {
                    HOperatorSet.SetFont(window, HWindID.Font + "-" + HWindID.FontSize);
                }
            }
            catch (Exception ex)
            {
                //Vision.ErrLog("设置字体失败", ex);
            }
            //HOperatorSet.SetFont(window, "-Consolas-16-*-0-*-*-1-");
        }

        /// <summary>
        /// 剔除重复的XLD
        /// </summary>
        /// <param name="XLD"></param>
        /// <returns></returns>
        public static HObject RemoveDuplicatesXld(HObject XLD)
        {
            HObject hObjectt = new HObject();
            HObject XldS = new HObject();
            XldS.GenEmptyObj();
            int dst = XLD.CountObj();

            for (int i = 0; i < XLD.CountObj(); i++)
            {
                HOperatorSet.SelectObj(XLD, out hObjectt, i + 1);
                if (XldS.CountObj() == 0)
                {
                    XldS = XldS.ConcatObj(hObjectt);
                }
                HOperatorSet.GetContourXld(hObjectt, out HTuple rows, out HTuple cols);

                for (int i2 = 0; i2 < XldS.CountObj(); i2++)
                {
                    HOperatorSet.SelectObj(XldS, out hObjectt, i2 + 1);
                    HOperatorSet.GetContourXld(hObjectt, out HTuple rows2, out HTuple cols2);
                    if (rows.TupleEqual(rows2) || cols.TupleEqual(cols2))
                    {
                        goto st;
                    }
                }
                XldS = XldS.ConcatObj(hObjectt);
            st:
                if (true)
                {
                }
            }
            dst = XldS.CountObj();

            return XldS;
        }

        /// <summary>
        /// 拟合直线
        /// </summary>
        /// <param name="Image">输入图像</param>
        /// <param name="objDisp">图形</param>
        /// <param name="HomMat2D">仿射变换</param>
        /// <param name="Elements">边缘点数</param>
        /// <param name="Threshold">边缘阀值</param>
        /// <param name="Sigma">边缘滤波系数</param>
        /// <param name="Transition">边缘极性</param>
        /// <param name="Point_Select">边缘点的选择</param>
        /// <param name="ROI_X">rake工具x数组</param>
        /// <param name="ROI_Y">rake工具y数组</param>
        /// <param name="Caliper_Height">卡尺工具高</param>
        /// <param name="Caliper_Width">卡尺工具宽</param>
        /// <param name="Min_Points_Num">最小数量</param>
        /// <param name="Caliper_Regions">产生的卡尺工具图形</param>
        /// <param name="Edges_X">找到的边缘点x数据</param>
        /// <param name="Edges_Y">找到的边缘点y数据</param>
        /// <param name="Result_xld">拟合得到的直线</param>
        /// <param name="Result_X">拟合得到的直线的点的x数组</param>
        /// <param name="Result_Y">拟合得到的直线的点的y数组</param>
        /// <param name="Error">错误信息</param>
        public static void Fit_Line(HObject Image, ref HObject objDisp, HTuple HomMat2D, int Elements, int Threshold, double Sigma, string Transition, string Point_Select,
            HTuple ROI_X, HTuple ROI_Y, int Caliper_Height, int Caliper_Width, int Min_Points_Num, out HObject Caliper_Regions, out HTuple Edges_X,
            out HTuple Edges_Y, out HObject Result_xld, out HTuple Result_X, out HTuple Result_Y)
        {
            HObject Cross;
            HOperatorSet.GenEmptyObj(out Cross);
            HOperatorSet.GenEmptyObj(out Caliper_Regions);
            HOperatorSet.GenEmptyObj(out Result_xld);
            Edges_X = new HTuple();
            Edges_Y = new HTuple();
            Result_X = new HTuple();
            Result_Y = new HTuple();
            try
            {
                //判断图像是否为空
                if (!IsObjectValided(Image))
                {
                    return;
                }
                HTuple Row0, Row1, Col0, Col1;

                //判断rake工具的ROI是否有效
                if (ROI_Y.Length < 2)
                {
                    Cross.Dispose();
                    return;
                }
                //判断ROI仿射变换矩阵是否有效，有效的时候，有6个数据
                if (HomMat2D.Length < 6)
                {
                    //矩阵无效，直接用原始ROI执行rake工具找边缘点
                    Result_xld.Dispose();
                    Rake(Image, out Caliper_Regions, Elements, Caliper_Height, Caliper_Width, Sigma, Threshold,
                    Transition, Point_Select, ROI_Y[0], ROI_X[0],
                    ROI_Y[1], ROI_X[1], out Edges_Y, out Edges_X);
                }
                else
                {
                    HTuple New_ROI_Y, New_ROI_X;
                    //矩阵有效，先产生新的ROI,用新的ROI执行rake工具找边缘点
                    HOperatorSet.AffineTransPoint2d(HomMat2D, ROI_Y, ROI_X, out New_ROI_Y, out New_ROI_X);
                    Rake(Image, out Caliper_Regions, Elements, Caliper_Height, Caliper_Width, Sigma, Threshold,
                    Transition, Point_Select, New_ROI_Y[0], New_ROI_X[0], New_ROI_Y[1], New_ROI_X[1], out Edges_Y, out Edges_X);
                }
                //把产生的卡尺工具图像添加到显示图形
                Concat_Obj(ref objDisp, ref Caliper_Regions, ref objDisp);

                //判断是否找到有边缘点，如果有，产生边缘点x图形，并添加到显示图形
                if (Edges_Y.Length > 0)
                {
                    HOperatorSet.GenCrossContourXld(out Cross, Edges_Y, Edges_X, 20, (new HTuple(45)).TupleRad());
                    Concat_Obj(ref objDisp, ref Cross, ref objDisp);
                }
                //如果边缘点数大于等于最小点数，进行直线拟合；否则返回错误信息
                if (Edges_Y.Length >= Min_Points_Num)
                {
                    //拟合直线
                    Pts_to_best_line(out Result_xld, Edges_Y, Edges_X, Min_Points_Num, out Row0, out Col0, out Row1, out Col1);
                    //把直线的点添加到结果数组
                    Result_Y = Row0.TupleConcat(Row1);
                    Result_X = Col0.TupleConcat(Col1);
                    //把拟合的直线图形添加到显示图形
                    Concat_Obj(ref objDisp, ref Result_xld, ref objDisp);
                }
                else
                {
                }
            }
            catch (HalconException HDevExpDefaultException)
            {
                Cross.Dispose();

                //Log(HDevExpDefaultException.Message);
            }
            Cross.Dispose();
        }

        /// <summary>
        /// XLD转换为区域
        /// </summary>
        /// <param name="xld">XLD</param>
        /// <returns>区域</returns>
        public static HObject XLD_To_Region(HObject xld)
        {
            if (!xld.IsInitialized())
            {
                return null;
            }
            HTuple hTuple = xld.GetObjClass();

            if (hTuple.Length == 0 || hTuple == "region")
            {
                return xld;
            }
            HObject hObject = new HObject();
            hObject.GenEmptyObj();
            try
            {
                int ds = xld.CountObj();
                HOperatorSet.GenRegionContourXld(xld, out hObject, "filled");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return hObject;
        }

        /// <summary>
        /// 区域转换为XLD
        /// </summary>
        /// <param name="Region">区域</param>
        /// <returns>返回XLD</returns>
        public static HObject Region_To_XLD(HObject Region)
        {
            HObject hObject = new HObject();
            hObject.GenEmptyObj();
            int ds = Region.CountObj();
            HTuple classTd = Region.GetObjClass();
            try
            {
                if (classTd == "region")
                {
                    return Region;
                }

                for (int i = 0; i < ds; i++)
                {
                    HOperatorSet.SelectObj(Region, out HObject hObject1, i + 1);
                    HOperatorSet.GetRegionContour(hObject1, out HTuple rows, out HTuple columns);
                    if (rows.Length > 1)
                    {
                        HOperatorSet.GenContourPolygonXld(out HObject hObject2, rows, columns);
                        hObject = hObject.ConcatObj(hObject2);
                    }
                }
                int dts = hObject.CountObj();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return hObject;
        }

        public static HObject GetReing2(HTuple rows, HTuple cols, HTuple phi, HTuple length1, HTuple length2)
        {

            try
            {
                gen_rectangle2_point(rows, cols, phi, length1, length2, out HTuple row1, out HTuple column1);
                gen_rectangle2_line_point(rows, cols, phi, length1, out HTuple rows1, out HTuple columns1, out HTuple row2s, out HTuple columns2);
                row1.Append(rows1[0]);
                column1.Append(columns1[0]);
                row1.Append(row1[0]);
                column1.Append(column1[0]);
                row1.Append(row1[3]);
                column1.Append(column1[3]);
                //HOperatorSet.GenRegionPolygon(out HObject hObject, row1, column1);
                HOperatorSet.GenRegionPolygonFilled(out HObject hObject, row1, column1);
                //gen_rectangle2_line_point(rows,cols,phi,length1)
                return hObject;

            }
            catch (Exception)
            {
            }

            return new HObject();
        }

        #endregion Halcon视觉常用算子


    }
    /// <summary>
    /// 绘制区域触发
    /// </summary>
    public class HdrawingObj
    {


        public void OnEonve()
        {
            this.dobj.OnDrag(DragFilterT);//画
            this.dobj.OnDetach(DetachFilter);//拆
            this.dobj.OnResize(ResizeFilter);//移动大小
            this.dobj.OnSelect(SelecFilter);//首次选中
            this.dobj.OnAttach(AttachFilter);//连接
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="dobj"></param>
        /// <param name="hwin"></param>
        /// <param name="type"></param>
        public void AttachFilter(HDrawingObject dobj, HWindow hwin, string type)
        {
            try
            {
                AttachEvne?.Invoke(this, type);
            }
            catch (HalconException hex)
            {
                MessageBox.Show(hex.GetErrorMessage(), "HALCON error", MessageBoxButtons.OK);
            }
        }
        /// <summary>
        /// 首次选中
        /// </summary>
        /// <param name="dobj"></param>
        /// <param name="hwin"></param>
        /// <param name="type"></param>
        public void SelecFilter(HDrawingObject dobj, HWindow hwin, string type)
        {
            try
            {
                IsShow = true;
                string typcs = dobj.GetDrawingObjectParams("type");
          
                HObject = dobj.GetDrawingObjectIconic();
                SelectEvne?.Invoke(this, type);
            }
            catch (HalconException hex)
            {
                MessageBox.Show(hex.GetErrorMessage(), "HALCON error", MessageBoxButtons.OK);
            }
        }
        /// <summary>
        /// 开始画执行
        /// </summary>
        /// <param name="dobj"></param>
        /// <param name="hwin"></param>
        /// <param name="type"></param>
        public void DragFilterT(HDrawingObject dobj, HWindow hwin, string type)
        {
            try
            {
                if (type != "on_drag")
                {

                }
                string typcs = dobj.GetDrawingObjectParams("type");
        
                HObject = dobj.GetDrawingObjectIconic();
                DragEvne?.Invoke(this, type);
            }
            catch (HalconException hex)
            {
                MessageBox.Show(hex.GetErrorMessage(), "HALCON error", MessageBoxButtons.OK);
            }
        }
        /// <summary>
        /// 拆掉绘制
        /// </summary>
        /// <param name="dobj"></param>
        /// <param name="hwin"></param>
        /// <param name="type"></param>
        public void DetachFilter(HDrawingObject dobj, HWindow hwin, string type)
        {
            try
            {
                IsShow = true;
                //string typcs = dobj.GetDrawingObjectParams("type");
    
                //this.HObject = dobj.GetDrawingObjectIconic();
                DetachEvne?.Invoke(this, type);
            }
            catch (HalconException hex)
            {
                MessageBox.Show(hex.GetErrorMessage(), "HALCON error", MessageBoxButtons.OK);
            }
        }
        /// <summary>
        /// 移动大小
        /// </summary>
        /// <param name="dobj"></param>
        /// <param name="hwin"></param>
        /// <param name="type"></param>
        public void ResizeFilter(HDrawingObject dobj, HWindow hwin, string type)
        {
            try
            {
                this.HObject = dobj.GetDrawingObjectIconic();
                ResizeEvne?.Invoke(this, type);
            }
            catch (HalconException hex)
            {
                MessageBox.Show(hex.GetErrorMessage(), "HALCON error", MessageBoxButtons.OK);
            }
        }
        public bool IsShow = false;


        public HTuple Measges;
        public HDrawingObject dobj;

        public HObject HObject;

        public delegate void SobelFilterEvne(HdrawingObj hdrawingObj, string type);


        public event SobelFilterEvne AttachEvne;
        /// <summary>
        /// 拆下绘制
        /// </summary>
        public event SobelFilterEvne DetachEvne;
        /// <summary>
        /// 画
        /// </summary>
        public event SobelFilterEvne DragEvne;
        /// <summary>
        /// 首次选中
        /// </summary>
        public event SobelFilterEvne SelectEvne;
        /// <summary>
        /// 更改大小
        /// </summary>
        public event SobelFilterEvne ResizeEvne;
        /// <summary>
        /// 
        /// </summary>
        public void OnAttachEvne()
        {
            try
            {
                if (AttachEvne != null)
                {
          
                    AttachEvne.Invoke(this, "");

                }
            }
            catch (Exception)
            {
            }

        }
    }


}