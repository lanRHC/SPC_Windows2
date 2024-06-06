using HalconDotNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vision2.vision;

namespace SPC_Windows.SPCFile
{
    [JsonObject(MemberSerialization.OptOut)]
    /// <summary>
    /// 单次拍照
    /// </summary>
    public class OneResultOBj
    {
        public OneResultOBj(int liyaID = 0, int runid = 0)
        {
            try
            {
                LiyID = liyaID;
                RunID = runid;
                image = new HObject();
                image.GenEmptyObj();
                HObjectRed = new HObject();
                HObjectRed.GenEmptyObj();
                HObjectGreen = new HObject();
                HObjectGreen.GenEmptyObj();
                HObjectYellow = new HObject();
                HObjectYellow.GenEmptyObj();
                HObjectBlue = new HObject();
                HObjectBlue.GenEmptyObj();
                hObject = new HObject();
                hObject.GenEmptyObj();
                Colrrs = new HObject();
                Colrrs.GenEmptyObj();
            }
            catch (Exception)
            {
            }
        }
        ~OneResultOBj()
        {
            //if (HObject != null)
            //{
            //    HObject.Dispose();
            //}
            //this.Dispose();
            //oneContOBJs.Dispose();
            //foreach (var item in Dick)
            //{
            //    item.Value.Dispose();
            //}
            //for (int i = 0; i < ListHobj.Count; i++)
            //{
            //    ListHobj[i].Dispose();
            //}
        }
        /// <summary>
        /// 是否保存图像
        /// </summary>
        public bool IsSave = true;
        public bool SaveDone = false;
        /// <summary>
        /// 
        /// </summary>
        public bool IsTiff = true;

        public DateTime CamNewTime;
        /// <summary>
        /// 设置填充或空白，true=fill;
        /// </summary>
        public bool SetDraw { get; set; }

        private Dictionary<string, RModelHomMat> dicModelR = new Dictionary<string, RModelHomMat>();
        public HTuple GetHomdeMobel(string name)
        {
            if (dicModelR.ContainsKey(name))
            {
                if (dicModelR[name].HomMat.Count >= 1)
                {
                    return dicModelR[name].HomMat[0];
                }
            }
            return null;
        }
        public Dictionary<string, RModelHomMat> GetDicModelR()
        {
            return dicModelR;
        }
        public RModelHomMat MRModelHomMat { get; set; }

        public RModelHomMat GetHomdeMobelEx(string name)
        {
            if (dicModelR.ContainsKey(name))
            {
                if (dicModelR[name].HomMat.Count >= 1)
                {
                    return dicModelR[name];
                }
            }
            return null;
        }

        public double PointX { get; set; }

        public double PointY { get; set; }




        /// <summary>
        /// 刷新显示
        /// </summary>
        private HObject hObject = new HObject();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hObject"></param>
        public void SetHobject(HObject hObjectImage)
        {
            if (hObjectImage == null)
            {
                hObject = hObjectImage;
            }
            else
            {
                if (hObject != null)
                {
                    hObject.Dispose();
                    hObject.GenEmptyObj();
                }

            }

        }

        public HTuple RowsData { get; set; } = new HTuple();
        public HTuple ColumnsData { get; set; } = new HTuple();

        int runid;

        public int Height;

        public int Width;

        /// <summary>
        /// 执行ID
        /// </summary>
        public int RunID
        {
            get
            { return runid; }
            set
            {
                runid = value;
            }
        }

        public void SetSaveImageDone()
        {
            SaveImageDone = true;
        }
        bool SaveImageDone;
        /// <summary>
        /// 库ID
        /// </summary>
        public int LiyID { get; set; }

        [JsonIgnore]
        public string SN { get; set; }
        /// <summary>
        /// 红色区域
        /// </summary>
        public HObject HObjectRed { get; set; }


        public Dictionary<string, HObject> GetDicImage()
        {
            return imageS;
        }

        /// <summary>
        ///图层
        /// </summary>
        private Dictionary<string, HObject> imageS = new Dictionary<string, HObject>();
        /// <summary>
        ///
        /// </summary>
        /// <param name="hObject"></param>
        public void AddImage(string index, HObject hObject)
        {
            if (!imageS.ContainsKey(index))
            {
                imageS.Add(index, hObject);
            }
            else
            {
                imageS[index] = hObject;
            }

        }

        public void ClerImage()
        {
            imageS.Clear();
        }

        public void OnimageDone(HObject Image, HObject imageR, HObject imageG, HObject imageB)
        {
            this.Image = image;
            himageHSVRGB.R = imageR; himageHSVRGB.G = imageG; himageHSVRGB.B = imageB;
            ImageDone?.Invoke(null, this);
        }
        public event EventHandler<OneResultOBj> ImageDone;

        [JsonIgnore]
        /// <summary>
        /// 主图像
        /// </summary>
        public HObject Image
        {
            get { return image; }
            set
            {

                himageHSVRGB.Image = value;
                image = value;
            }
        }

        public HObject GetImageOBJ(ImageTypeObj imageType)
        {
            return himageHSVRGB.GetImageOBJ(imageType);
        }


        HObject image;

        HimageHSVRGB himageHSVRGB = new HimageHSVRGB();

        public string ImagePath { get; set; } = "";


        /// <summary>
        /// 图像预处理完成
        /// </summary>

        public bool ImagePretreatmentDone;

        /// <summary>
        /// 调试
        /// </summary>
        public int DebugID { get; set; }
        /// <summary>
        /// 相机容器名
        /// </summary>
        public string RunName { get; set; }

        public string NGMestage { get; set; } = "";

        public HTuple Massage { get; set; } = new HTuple();
        public bool IsMassageBack { get; set; }

        public ColorResult ColorResu = ColorResult.green;

        public HTuple OKMassage { get; set; } = new HTuple();
        public HTuple NGMassage { get; set; } = new HTuple();

        /// <summary>
        /// 移动图像
        /// </summary>

        public bool IsMoveBool;


        /// <summary>
        /// 显示区域
        /// </summary>
        public bool IsXLDOrImage;


        /// <summary>
        /// 结果
        /// </summary>
        private bool ResultBool
        {
            get
            {
                foreach (var item in oneContOBJs.DicOnes)
                {
                    if (!item.Value.AutoOK)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// 结果
        /// </summary>
        public bool OK
        {
            get
            {
                foreach (var item in oneContOBJs.DicOnes)
                {
                    if (!item.Value.aOK)
                    {
                        return false;
                    }
                }
                return true;
            }
            set
            {
                if (value)
                {
                    foreach (var item in oneContOBJs.DicOnes)
                    {
                        item.Value.SetOK(true);
                    }
                }
                autoOk = value;
            }
        }
        public bool Done
        {
            get
            {
                foreach (var item in oneContOBJs.DicOnes)
                {
                    if (!item.Value.Done)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        private bool autoOk;

        /// <summary>
        /// 元件集合
        /// </summary>
        private OneCompOBJs oneContOBJs = new OneCompOBJs();

        /// <summary>
        /// 关联元件集合
        /// </summary>
        /// <param name="oneContOB"></param>
        /// <returns></returns>
        public OneCompOBJs GetNgOBJS(OneCompOBJs oneContOB = null)
        {
            if (oneContOB != null)
            {
                oneContOBJs = oneContOB;
            }
            return oneContOBJs;
        }
        /// <summary>
        /// 读取图片
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool ReadImage(string path)
        {
            try
            {
                HOperatorSet.ReadImage(out HObject hObject, path);
                Image = hObject;
                CamNewTime = DateTime.Now;
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }
        /// <summary>
        /// 添加OK元件
        /// </summary>
        /// <param name="oneComponent"></param>
        public void AddOKOBj(OneCompOBJs.OneComponent oneComponent)
        {
            oneContOBJs.Add(oneComponent);
        }

        /// <summary>
        /// 添加NG结果区域数据
        /// </summary>
        /// <param name="rObj"></param>
        public void AddNGOBJ(string cName, OneRObj rObj)
        {

            //if (!rObj.RestStrings().Contains(rObj.NGText))
            //{
            //    rObj.RestStrings().Add(rObj.NGText);
            //}
            oneContOBJs.AddCont(cName, rObj);
        }



        /// <summary>
        /// 添加NG结果区域数据
        /// </summary>
        /// <param name="component">元件名称</param>
        /// <param name="nGText">NG信息</param>
        /// <param name="roi">搜索区域</param>
        /// <param name="err">NG区域</param>
        /// <param name="ngText">NG信息集合</param>
        /// <param name="runPa">库名称</param>
        public void AddNGOBJ(string component, string nGText, HObject roi, HObject err,
            HTuple ngText, string runPa = "")
        {
            OneRObj rObj = new OneRObj()
            {
                NGText = nGText,
                NGROI = err,
                BankName = runPa,
            };
            rObj.ROI(roi);
            if (roi.IsInitialized())
            {
                HOperatorSet.AreaCenter(roi, out HTuple area, out HTuple row, out HTuple colu);
                if (row.Length > 0)
                {
                    rObj.Row = row.TupleInt();
                    rObj.Col = colu.TupleInt();
                }
            }
            //if (ngText != null)
            //{
            //    for (int i = 0; i < ngText.Length; i++)
            //    {
            //        if (!rObj.RestStrings().Contains(ngText[i]))
            //        {
            //            rObj.RestStrings().Add(ngText[i]);
            //        }
            //    }
            //}
            AddNGOBJ(component, rObj);
        }

        /// <summary>
        /// 添加信息到图像显示
        /// </summary>
        /// <param name="rows">坐标</param>
        /// <param name="columns">坐标</param>
        /// <param name="Massage">信息</param>
        /// <param name="color">颜色</param>
        /// <param name="meblut"></param>
        public void AddImageMassage(HTuple rows, HTuple columns, HTuple Massage, ColorResult color, string meblut = "false")
        {
            if (columns.Length > Massage.Length)
            {
                Massage = HTuple.TupleGenConst(columns.Length, Massage);
            }
            meblut = meblut.ToLower();
            switch (color)
            {
                case ColorResult.red:

                    MaRed.color = "red";
                    MaRed.Rows.Add(rows);
                    MaRed.Columns.Add(columns);
                    MaRed.Massage.Add(Massage);
                    MaRed.MassageBlute = meblut;

                    break;

                case ColorResult.yellow:

                    if (rows.Length == columns.Length)
                    {
                        MaYellow.color = "yellow";
                        MaYellow.Rows.Add(rows);
                        MaYellow.Columns.Add(columns);
                        MaYellow.Massage.Add(Massage);
                        MaYellow.MassageBlute = meblut;
                    }
                    break;

                case ColorResult.green:
                    if (rows.Length == columns.Length)
                    {
                        MaGreen.color = "green";
                        MaGreen.Rows.Add(rows);
                        MaGreen.Columns.Add(columns);
                        MaGreen.Massage.Add(Massage);
                        MaGreen.MassageBlute = meblut;
                    }
                    break;

                case ColorResult.blue:
                    if (rows.Length == columns.Length)
                    {
                        MaBlue.color = "blue";
                        MaBlue.Rows.Add(rows);
                        MaBlue.Columns.Add(columns);
                        MaBlue.Massage.Add(Massage);
                        MaBlue.MassageBlute = meblut;
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        ///  添加信息到图像显示
        /// </summary>
        /// <param name="rows">坐标</param>
        /// <param name="columns">坐标</param>
        /// <param name="Massage">信息</param>
        public void AddImageMassage(HTuple rows, HTuple columns, HTuple Massage, ColorResult colorResult = ColorResult.green)
        {
            if (rows.TupleType().TupleFind(4) == 1)
            {

            }
            string meblut = "false";
            ColorResult color = colorResult;
            if (columns.Length > Massage.Length)
            {
                Massage = HTuple.TupleGenConst(columns.Length, Massage);
            }
            meblut = meblut.ToLower();
            switch (color)
            {
                case ColorResult.red:

                    MaRed.color = "red";
                    MaRed.Rows.Add(rows);
                    MaRed.Columns.Add(columns);
                    MaRed.Massage.Add(Massage);
                    MaRed.MassageBlute = meblut;
                    break;
                case ColorResult.yellow:

                    if (rows.Length == columns.Length /*&& columns.Length == Massage.Length*/)
                    {
                        MaYellow.color = "yellow";
                        MaYellow.Rows.Add(rows);
                        MaYellow.Columns.Add(columns);
                        MaYellow.Massage.Add(Massage);
                        MaYellow.MassageBlute = meblut;
                    }
                    break;

                case ColorResult.green:
                    if (rows.Length == columns.Length /*&& columns.Length == Massage.Length*/)
                    {
                        MaGreen.color = "green";
                        MaGreen.Rows.Add(rows);
                        MaGreen.Columns.Add(columns);
                        MaGreen.Massage.Add(Massage);
                        MaGreen.MassageBlute = meblut;
                    }
                    break;

                case ColorResult.blue:
                    if (rows.Length == columns.Length /*&& columns.Length == Massage.Length*/)
                    {
                        MaBlue.color = "blue";
                        MaBlue.Rows.Add(rows);
                        MaBlue.Columns.Add(columns);
                        MaBlue.Massage.Add(Massage);
                        MaBlue.MassageBlute = meblut;
                    }
                    break;

                default:
                    break;
            }
        }
        /// <summary>
        /// 添加区域到显示
        /// </summary>
        /// <param name="hObject"></param>
        /// <param name="color"></param>
        public void AddObj(HObject hObject, ColorResult color)
        {
            try
            {
                if (!HWindID.IsObjectValided(hObject))
                {
                    return;
                }
                if (hObject.CountObj() == 0)
                {
                    return;
                }
                switch (color)
                {
                    case ColorResult.red:
                        HObjectRed = HObjectRed.ConcatObj(hObject);
                        break;

                    case ColorResult.yellow:
                        HObjectYellow = HObjectYellow.ConcatObj(hObject);
                        break;

                    case ColorResult.green:
                        HObjectGreen = HObjectGreen.ConcatObj(hObject);
                        break;

                    case ColorResult.blue:
                        HObjectBlue = HObjectBlue.ConcatObj(hObject);
                        break;

                    default:
                        AddObj(hObject, color.ToString());
                        break;
                }
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private HObject Colrrs = new HObject();

        public HObject GetColrrs()
        {
            return Colrrs; ;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="row1"></param>
        /// <param name="col1"></param>
        /// <param name="row2"></param>
        /// <param name="col2"></param>
        public void SetObjCross(HObject obj, out HTuple row1, out HTuple col1, out HTuple row2, out HTuple col2, bool IsUnion = true)
        {
            row1 = new HTuple();
            col1 = new HTuple();
            row2 = new HTuple();
            col2 = new HTuple();
            try
            {
                HOperatorSet.GetImageSize(this.Image, out HTuple wid, out HTuple hei);
                if (wid.Length == 0)
                {
                    return;
                }
                double d = (double)wid / (double)hei;
                HTuple hTuple = obj.GetObjClass();

                HObject hObject1 = HWindID.XLD_To_Region(obj);
                if (IsUnion)
                {
                    HOperatorSet.Union1(hObject1, out hObject1);
                }
                else
                {
                    HOperatorSet.Connection(hObject1, out hObject1);
                }

                HOperatorSet.SmallestRectangle1(hObject1, out row1, out col1, out row2, out col2);
                HOperatorSet.AreaCenter(hObject1, out HTuple area, out HTuple row, out HTuple col);
                if (row1.Length == 1 && hei.Length == 1 && row.Length == 1)
                {
                    HOperatorSet.GenContourPolygonXld(out HObject hObject, new HTuple(row, row), new HTuple(new HTuple(0), col1));
                    HOperatorSet.GenContourPolygonXld(out HObject hObject2, new HTuple(new HTuple(0), row1), new HTuple(col, col));
                    HOperatorSet.GenContourPolygonXld(out HObject hObject3, new HTuple(new HTuple(row), row), new HTuple(col2, wid));
                    HOperatorSet.GenContourPolygonXld(out HObject hObject4, new HTuple(new HTuple(row2), hei), new HTuple(col, col));
                    this.SetCross(hObject.ConcatObj(hObject2).ConcatObj(hObject3).ConcatObj(hObject4));
                    //hWindowControl3.HalconWindow.SetPart(row1 - (200 * 1), col1 - (200 * d), row2 + (200 * 1), col2 + (200 * d));
                    //hWindowControl4.HalconWindow.SetPart(row1 - (200 * 1), col1 - (200 * d), row2 + (200 * 1), col2 + (200 * d));
                }
                else
                {
                    hObject1 = HWindID.XLD_To_Region(obj);
                    HOperatorSet.AreaCenter(hObject1, out area, out row, out col);
                    HOperatorSet.SmallestRectangle1(hObject1, out row1, out col1, out row2, out col2);
                    if (row1.Length == 1)
                    {
                        //HOperatorSet.AreaCenter(hObject1, out HTuple area, out HTuple row, out HTuple col);
                        HOperatorSet.GenContourPolygonXld(out HObject hObject, new HTuple(row, row), new HTuple(new HTuple(0), col1));
                        HOperatorSet.GenContourPolygonXld(out HObject hObject2, new HTuple(new HTuple(0), row1), new HTuple(col, col));
                        HOperatorSet.GenContourPolygonXld(out HObject hObject3, new HTuple(new HTuple(row), row), new HTuple(col2, wid));
                        HOperatorSet.GenContourPolygonXld(out HObject hObject4, new HTuple(new HTuple(row2), hei), new HTuple(col, col));
                        this.SetCross(hObject.ConcatObj(hObject2).ConcatObj(hObject3).ConcatObj(hObject4));
                        //    hWindowControl3.HalconWindow.SetPart(row1 - (200 * 1), col1 - (200 * d), row2 + (200 * 1), col2 + (200 * d));
                        //    hWindowControl4.HalconWindow.SetPart(row1 - (200 * 1), col1 - (200 * d), row2 + (200 * 1), col2 + (200 * d));
                    }
                }
                row1 = row1 - (100 * 1);
                col1 = col1 - (100 * d);
                row2 = row2 + (100 * 1);
                col2 = col2 + (100 * d);
            }
            catch (Exception ex)
            { }
        }
        /// <summary>
        /// 显示区域到窗口并调整合适的窗口大小
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="row1"></param>
        /// <param name="col1"></param>
        public void SetObjPart(HObject obj, HWindID hWindID, out HTuple row, out HTuple col, out HTuple size)
        {
            row = new HTuple();
            col = new HTuple();
            size = new HTuple();
            try
            {
                if (obj == null)
                {
                    return;
                }
                HTuple hTuple = obj.GetObjClass();
                HObject hObject1 = HWindID.XLD_To_Region(obj);
                HOperatorSet.Union1(hObject1, out hObject1);
                HOperatorSet.AreaCenter(hObject1, out HTuple area, out row, out col);
                HOperatorSet.SmallestCircle(hObject1, out row, out col, out HTuple radius);
                if (radius < 200)
                {
                    radius = radius * 5;
                }
                size = radius;
                if (hWindID != null)
                {
                    HTuple row1 = row - size;
                    HTuple col1 = col - (double)(size / hWindID.H_W_Size);
                    HTuple row2 = row + size;
                    HTuple col2 = col + (double)(size / hWindID.H_W_Size);
                    hWindID.SetImage(this.image);
                    hWindID.SetPart(row.TupleInt(), col.TupleInt(), size.TupleInt());
                    hWindID.SetPerpetualPart(row1, col1, row2, col2);
                    hWindID.HobjClear();
                    hWindID.AddObj(obj);
                    hWindID.ShowImage();
                    hWindID.ShowObj();
                }
            }
            catch (Exception ex)
            {
            }
        }
        public void SetCross(HObject hObject)
        {
            Colrrs = hObject;
        }

        /// <summary>
        /// 添加区域显示
        /// </summary>
        /// <param name="hObject"></param>
        /// <param name="color"></param>
        public void AddObj(HObject hObject, HTuple color = null)
        {
            ListHobj.Add(new ObjectColor(hObject, color));
        }
        /// <summary>
        /// 设置名称区域的颜色
        /// </summary>
        /// <param name="name"></param>
        /// <param name="color"></param>
        public void SetNameOBJColor(string name, HTuple color)
        {
            try
            {
                if (Dick.ContainsKey(name))
                {
                    Dick[name].HobjectColot = color;
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 添加带名称的区域
        /// </summary>
        /// <param name="name">区域名称</param>
        /// <param name="hObject">区域</param>
        /// <param name="colr">颜色</param>
        public void AddNameOBJ(string name, HObject hObject, HTuple color = null)
        {
            try
            {
                if (Dick.ContainsKey(name))
                {
                    Dick[name] = new ObjectColor(hObject, color);
                }
                else
                {
                    Dick.Add(name, new ObjectColor(hObject, color));
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 添加带名称的区域
        /// </summary>
        /// <param name="name">区域名称</param>
        /// <param name="hObject">区域</param>
        /// <param name="colr">颜色</param>
        public void AddNameOBJ(string name, HObject hObject, ColorResult colr)
        {
            try
            {
                if (Dick.ContainsKey(name))
                {
                    Dick[name] = new ObjectColor(hObject, colr.ToString());
                }
                else
                {
                    Dick.Add(name, new ObjectColor(hObject, colr.ToString()));
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 添加信息到图像左上方
        /// </summary>
        /// <param name="massage"></param>
        public void AddMeassge(HTuple massage)
        {
            Massage.Append(massage);
        }




     
    
        /// <summary>
        ///
        /// </summary>
        public void ClearAllObj()
        {
            try
            {
                HObjectYellow.GenEmptyObj();
                HObjectGreen.GenEmptyObj();
                HObjectBlue.GenEmptyObj();
                HObjectRed.GenEmptyObj();
                Massage = new HTuple();
                MaGreen = new MassageText();
                MaRed = new MassageText();
                MaYellow = new MassageText();
                MaBlue = new MassageText();
                Colrrs = new HObject();
                Colrrs.GenEmptyObj();
                ListHobj.Clear();
                this.Dick.Clear();

                oneContOBJs.DicOnes.Clear();
                if (hObject != null)
                {
                    this.hObject.GenEmptyObj();
                }

            }
            catch (Exception ex)
            {

            }

        }

        public void ClearImageMassage()
        {
            MaGreen = new MassageText();
            MaRed = new MassageText();
            MaYellow = new MassageText();
            MaBlue = new MassageText();
        }

        public void Dispose()
        {
            try
            {
                himageHSVRGB.Dispose();
                if (Image != null)
                {
                    Image.Dispose();
                }
                if (imageS != null)
                {
                    foreach (var item in imageS)
                    {
                        if (item.Value != null)
                        {
                            item.Value.Dispose();
                        }

                    }
                }
              

                foreach (var item in oneContOBJs.DicOnes)
                {
                    foreach (var itemtd in item.Value.oneRObjs)
                    {
                        itemtd.Dispose();
                        if (itemtd.ROI() != null)
                        {
                            itemtd.ROI().Dispose();
                        }
                        itemtd.NGROI.Dispose();
                    }
                }
                oneContOBJs.DicOnes.Clear();

                for (int i = 0; i < ListHobj.Count; i++)
                {
                    ListHobj[i].Dispose();
                }

                HObjectYellow.Dispose();
                HObjectGreen.Dispose();
                HObjectBlue.Dispose();
                HObjectRed.Dispose();
                HObjectYellow.GenEmptyObj();
                HObjectGreen.GenEmptyObj();
                HObjectBlue.GenEmptyObj();
                HObjectRed.GenEmptyObj();
            }
            catch (Exception ex)
            {
            }
        }

        private HObject HObject;

        private void SelectOBJ(HObject hObject, HTuple hWindowHalconID, int rowi, int coli, bool ismove)
        {
            HTuple intd = new HTuple();
            if (ismove)
            {
                try
                {
                    HOperatorSet.GetObjClass(hObject, out HTuple classv);
                    if (classv.Length == 0)
                    {
                        return;
                    }
                    if (classv[0] == "region")
                    {
                        HOperatorSet.GetRegionIndex(hObject, rowi, coli, out intd);
                        if (intd >= 0)
                        {
                            if (HObject == null)
                            {
                                HObject = new HObject();
                                HObject.GenEmptyObj();
                            }
                            HObject = HObject.ConcatObj(hObject.SelectObj(intd));
                            if (hObject.CountObj() != 1)
                            {
                                HOperatorSet.DispObj(hObject.RemoveObj(intd), hWindowHalconID);
                            }
                            return;
                        }
                    }
                    else if (classv[0] == "xld_cont")
                    {
                        if (HObject == null)
                        {
                            HObject = new HObject();
                            HObject.GenEmptyObj();
                        }
                        HOperatorSet.SelectXldPoint(hObject, out HObject hObject1, rowi, coli);
                        HObject = HObject.ConcatObj(hObject1);
                    }
                }
                catch (Exception ex)
                {
                }
            }
            if (hObject.IsInitialized())
            {
                HOperatorSet.DispObj(hObject, hWindowHalconID);
            }

        }
        public ImageTypeObj ShowImageType
        {
            get;
            set;
        }
        public void ShowImage(HWindow hWindowHalconID, bool isFill = false)
        {
            try
            {
                HSystem.SetSystem("flush_graphic", "false");
                HOperatorSet.ClearWindow(hWindowHalconID);
                if (HWindID.IsObjectValided(this.Image))
                {
                    if (ShowImageType == ImageTypeObj.Image3)
                    {
                        HOperatorSet.DispObj(Image, hWindowHalconID);
                    }
                    else
                    {
                        HOperatorSet.DispObj(GetImageOBJ(ShowImageType), hWindowHalconID);
                    }
                    HSystem.SetSystem("flush_graphic", "true");
                    if (isFill)
                    {

                    }
                }
                else
                {
                    //HOperatorSet.ClearWindow(hWindowHalconID);
                }
            }
            catch (Exception)
            {
            }
        }

        public void ShowAll(HWindow hWindowHalconID, int rowi = 0, int coli = 0, bool ismovet = false)
        {
            try
            {

                HSystem.SetSystem("flush_graphic", "false");
                HOperatorSet.ClearWindow(hWindowHalconID);
                //if (HImage != Image)
                //{
                //    HImage = Image;
                //    hWindowHalconID.AttachBackgroundToWindow(new HImage(Image));
                //}
                //
                if (HWindID.IsObjectValided(this.Image))
                {
                    if (ShowImageType == ImageTypeObj.Image3)
                    {
                        HOperatorSet.DispObj(Image, hWindowHalconID);
                    }
                    else
                    {
                        HOperatorSet.DispObj(GetImageOBJ(ShowImageType), hWindowHalconID);
                    }
                }
                else
                {
                    //HOperatorSet.ClearWindow(hWindowHalconID);
                }
                HSystem.SetSystem("flush_graphic", "true");

                if (!IsMoveBool)
                {
                    ShouOBJ(hWindowHalconID);
                }
                if (hObject == null)
                {
                    hObject = new HObject();
                    hObject.GenEmptyObj();
                }
                HOperatorSet.SetColor(hWindowHalconID, "blue");
                HOperatorSet.DispObj(hObject, hWindowHalconID);
            }
            catch (Exception ex)
            {
                //AlarmText.AddTextNewLine("显示图像故障:" + ex.Message);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hWindowHalconID"></param>
        /// <param name="rowi"></param>
        /// <param name="coli"></param>
        /// <param name="ismovet"></param>
        public void ShouOBJ(HTuple hWindowHalconID)
        {
            try
            {
                if (HObject != null)
                {
                    HObject.Dispose();
                    HObject = new HObject();
                    HObject.GenEmptyObj();
                }
                //HSystem.SetSystem("flush_graphic", "false");
                if (SetDraw)
                {
                    HOperatorSet.SetDraw(hWindowHalconID, "fill");
                }
                else
                {
                    HOperatorSet.SetDraw(hWindowHalconID, "margin");
                }
                HOperatorSet.SetColor(hWindowHalconID, "green");
                if (HObjectGreen.IsInitialized())
                {
                    HOperatorSet.DispObj(HObjectGreen, hWindowHalconID);
                }

                HOperatorSet.SetColor(hWindowHalconID, "yellow");
                if (HObjectYellow.IsInitialized())
                {
                    HOperatorSet.DispObj(HObjectYellow, hWindowHalconID);
                }
                //HOperatorSet.SetColor(hWindowHalconID, Vision.Instance.Indicator_Box_Colr.ToString());
                HOperatorSet.SetColor(hWindowHalconID, "blue");
                HOperatorSet.DispObj(Colrrs, hWindowHalconID);

                HOperatorSet.SetColor(hWindowHalconID, "blue");
                if (HObjectBlue.IsInitialized())
                {
                    HOperatorSet.DispObj(HObjectBlue, hWindowHalconID);
                }

                HOperatorSet.SetColor(hWindowHalconID, "red");
                if (HObjectRed.IsInitialized())
                {
                    HOperatorSet.DispObj(HObjectRed, hWindowHalconID);
                }

                for (int i = 0; i < ListHobj.Count; i++)
                {
                    if (ListHobj[i]._HObject == null)
                    {
                        break;
                    }
                    if (ListHobj[i].HobjectColot.S.StartsWith("#"))
                    {
                        HOperatorSet.SetDraw(hWindowHalconID, "fill");
                    }
                    else
                    {
                        if (SetDraw)
                        {
                            HOperatorSet.SetDraw(hWindowHalconID, "fill");
                        }
                        else
                        {
                            HOperatorSet.SetDraw(hWindowHalconID, "margin");
                        }
                    }
                    HOperatorSet.SetColor(hWindowHalconID, ListHobj[i].HobjectColot);
                    if (ListHobj[i]._HObject.IsInitialized())
                    {
                        HOperatorSet.DispObj(ListHobj[i]._HObject, hWindowHalconID);
                    }
                    //SelectOBJ(ListHobj[i].Object, hWindowHalconID, rowi, coli, ismovet);
                }
                if (SetDraw)
                {
                    HOperatorSet.SetDraw(hWindowHalconID, "fill");
                }
                else
                {
                    HOperatorSet.SetDraw(hWindowHalconID, "margin");
                }
                try
                {
                    List<ObjectColor> hobjt_Colros = Dick.Values.ToList();
                    foreach (var item in hobjt_Colros)
                    {
                        if (item == null)
                        {
                            return;
                        }
                        if (item._HObject == null)
                        {
                            break;
                        }
                        if (item._HObject.IsInitialized())
                        {
                            HOperatorSet.SetColor(hWindowHalconID, item.HobjectColot);

                            HOperatorSet.DispObj(item._HObject, hWindowHalconID);
                        }
                        //if (Vision.IsObjectValided(item.Object))
                        //{

                        //}

                        //SelectOBJ(item.Value.Object, hWindowHalconID, rowi, coli, ismovet);
                    }

                }
                catch (Exception ex)
                {


                }

                foreach (var item in oneContOBJs.DicOnes)
                {
                    HTuple mesage = new HTuple();
                    mesage.Append(item.Key);

                    foreach (var itemtd in item.Value.oneRObjs)
                    {
                        if (!item.Value.aOK)
                        {

                            try
                            {
                                mesage.Append(itemtd.NGText);
                                HOperatorSet.SetColor(hWindowHalconID, "red");
                                if (itemtd.NGROI.IsInitialized())
                                {
                                    HOperatorSet.SetColor(hWindowHalconID, ColorResult.yellow.ToString());
                                    HOperatorSet.DispObj(itemtd.NGROI, hWindowHalconID);
                                }
                                if (itemtd.ROI() != null)
                                {
                                    if (itemtd.ROI().IsInitialized())
                                    {
                                        HOperatorSet.SetColor(hWindowHalconID, ColorResult.red.ToString());
                                        HOperatorSet.DispObj(itemtd.ROI(), hWindowHalconID);
                                    }
                                }
                                for (int i = 0; i < itemtd.massageText.Rows.Count; i++)
                                {
                                    if (itemtd.massageText.Rows[i].Length >= 1)
                                    {
                                        HWindID.Disp_message(hWindowHalconID, itemtd.massageText.Massage[i],
                                            itemtd.massageText.Rows[i], itemtd.massageText.Columns[i], false);
                                    }
                                }
                            }
                            catch (Exception ex) { }

                        }
                        else
                        {
                            HOperatorSet.SetColor(hWindowHalconID, "green");
                            if (HWindID.IsObjectValided(itemtd.ROI()))
                            {
                                HOperatorSet.DispObj(itemtd.ROI(), hWindowHalconID);
                            }

                        }
                    }

                }
                if (Massage.Length != 0)
                {
                    if (Massage.Length != 1 || Massage.ToString() != "")
                    {
                        HTuple text = Massage;
                        text.Append(NGMassage);
                        text.Append(OKMassage);
                        if (!OK)
                        {
                            HWindID.Disp_message(hWindowHalconID, text, 0, 0, true, "red", "false");
                        }
                        else
                        {
                            HWindID.Disp_message(hWindowHalconID, text, 0, 0, true, ColorResu.ToString(), "false");
                        }
                    }
                }
                if (!IsMoveBool)
                {
                    MaRed.ShowMassage(hWindowHalconID);
                    MaGreen.ShowMassage(hWindowHalconID);
                    MaBlue.ShowMassage(hWindowHalconID);
                    MaYellow.ShowMassage(hWindowHalconID);
                }
            }
            catch (Exception ex)
            {
                //AlarmText.AddTextNewLine("HalconResult显示故障:" + ex.StackTrace);
            }
        }

        public void ShowObjEX(HWindow hWindowHalconID)
        {
            try
            {
                if (HObject != null)
                {
                    HObject.Dispose();
                    HObject = new HObject();
                    HObject.GenEmptyObj();
                }
                // HSystem.SetSystem("flush_graphic", "false");
                if (SetDraw)
                {
                    HOperatorSet.SetDraw(hWindowHalconID, "fill");
                }
                else
                {
                    HOperatorSet.SetDraw(hWindowHalconID, "margin");
                }
                HOperatorSet.SetColor(hWindowHalconID, "green");
                if (HObjectGreen.IsInitialized())
                {
                    HOperatorSet.DispObj(HObjectGreen, hWindowHalconID);
                }

                HOperatorSet.SetColor(hWindowHalconID, "yellow");
                if (HObjectYellow.IsInitialized())
                {
                    HOperatorSet.DispObj(HObjectYellow, hWindowHalconID);
                }

                HOperatorSet.DispObj(Colrrs, hWindowHalconID);

                HOperatorSet.SetColor(hWindowHalconID, "blue");
                if (HObjectBlue.IsInitialized())
                {
                    HOperatorSet.DispObj(HObjectBlue, hWindowHalconID);
                }

                HOperatorSet.SetColor(hWindowHalconID, "red");
                if (HObjectRed.IsInitialized())
                {
                    HOperatorSet.DispObj(HObjectRed, hWindowHalconID);
                }


                for (int i = 0; i < ListHobj.Count; i++)
                {
                    if (ListHobj[i]._HObject == null)
                    {
                        break;
                    }
                    if (ListHobj[i].HobjectColot.S.StartsWith("#"))
                    {
                        HOperatorSet.SetDraw(hWindowHalconID, "fill");
                    }
                    else
                    {
                        if (SetDraw)
                        {
                            HOperatorSet.SetDraw(hWindowHalconID, "fill");
                        }
                        else
                        {
                            HOperatorSet.SetDraw(hWindowHalconID, "margin");
                        }
                    }
                    HOperatorSet.SetColor(hWindowHalconID, ListHobj[i].HobjectColot);
                    if (ListHobj[i]._HObject.IsInitialized())
                    {
                        HOperatorSet.DispObj(ListHobj[i]._HObject, hWindowHalconID);
                    }
                    //SelectOBJ(ListHobj[i].Object, hWindowHalconID, rowi, coli, ismovet);
                }
                if (SetDraw)
                {
                    HOperatorSet.SetDraw(hWindowHalconID, "fill");
                }
                else
                {
                    HOperatorSet.SetDraw(hWindowHalconID, "margin");
                }
                List<ObjectColor> hobjt_Colros = Dick.Values.ToList();

                for (int i = 0; i < hobjt_Colros.Count; i++)
                {
                    if (hobjt_Colros[i] == null)
                    {
                        return;
                    }
                    if (hobjt_Colros[i]._HObject == null)
                    {
                        break;
                    }
                    //HOperatorSet.AreaCenter(hobjt_Colros[i].Object, out HTuple area, out HTuple row, out HTuple col);

                    //if (Vision.IsObjectValided(item.Object))
                    //{
                    HOperatorSet.SetColor(hWindowHalconID, hobjt_Colros[i].HobjectColot);
                    HOperatorSet.DispObj(hobjt_Colros[i]._HObject, hWindowHalconID);
                    //}

                    //SelectOBJ(item.Value.Object, hWindowHalconID, rowi, coli, ismovet);
                }
                //foreach (var item in hobjt_Colros)
                //{

                //}
                if (true)
                {
                    var ds = from n in oneContOBJs.DicOnes
                             from de in n.Value.oneRObjs
                             where !de.aOK
                             select de;

                    foreach (var item in ds)
                    {
                        try
                        {


                            if (HWindID.IsObjectValided(item.NGROI))
                            {
                                //hObjectNGROI = hObjectNGROI.ConcatObj(item.NGROI);
                                HOperatorSet.SetColor(hWindowHalconID, ColorResult.yellow.ToString());
                                HOperatorSet.DispObj(item.NGROI, hWindowHalconID);
                            }
                            if (HWindID.IsObjectValided(item.ROI()))
                            {
                                HOperatorSet.SetColor(hWindowHalconID, ColorResult.red.ToString());
                                HOperatorSet.DispObj(item.ROI(), hWindowHalconID);
                            }
                            for (int i = 0; i < item.massageText.Rows.Count; i++)
                            {
                                if (item.massageText.Rows[i].Length >= 1)
                                {
                                    HWindID.Disp_message(hWindowHalconID, item.massageText.Massage[i],
                                        item.massageText.Rows[i], item.massageText.Columns[i], false);
                                }
                            }
                        }
                        catch (Exception ex)
                        { }
                    }

                    ds = from n in oneContOBJs.DicOnes
                         from de in n.Value.oneRObjs
                         where de.aOK
                         select de;
                    foreach (var item in ds)
                    {
                        try
                        {
                            HOperatorSet.SetColor(hWindowHalconID, "green");
                            if (HWindID.IsObjectValided(item.ROI()))
                            {
                                HOperatorSet.DispObj(item.ROI(), hWindowHalconID);
                            }
                        }
                        catch (Exception ex)
                        { }
                    }
                }
                else
                {
                    foreach (var item in oneContOBJs.DicOnes)
                    {
                        foreach (var itemtd in item.Value.oneRObjs)
                        {
                            if (!item.Value.aOK)
                            {
                                try
                                {
                                    if (HWindID.IsObjectValided(itemtd.NGROI))
                                    {
                                        HOperatorSet.SetColor(hWindowHalconID, ColorResult.yellow.ToString());
                                        HOperatorSet.DispObj(itemtd.NGROI, hWindowHalconID);
                                    }
                                    if (HWindID.IsObjectValided(itemtd.ROI()))
                                    {
                                        HOperatorSet.SetColor(hWindowHalconID, ColorResult.red.ToString());
                                        HOperatorSet.DispObj(itemtd.ROI(), hWindowHalconID);
                                    }
                                    for (int i = 0; i < itemtd.massageText.Rows.Count; i++)
                                    {
                                        if (itemtd.massageText.Rows[i].Length >= 1)
                                        {
                                            HWindID.Disp_message(hWindowHalconID, itemtd.massageText.Massage[i],
                                                itemtd.massageText.Rows[i], itemtd.massageText.Columns[i], false);
                                        }
                                    }
                                }
                                catch (Exception ex) { }
                            }
                            else
                            {
                                HOperatorSet.SetColor(hWindowHalconID, "green");
                                if (HWindID.IsObjectValided(itemtd.ROI()))
                                {
                                    HOperatorSet.DispObj(itemtd.ROI(), hWindowHalconID);
                                }

                            }
                        }
                    }
                }
                // HSystem.SetSystem("flush_graphic", "true");


                if (Massage.Length != 0)
                {
                    if (Massage.Length != 1 || Massage.ToString() != "")
                    {
                        HTuple text = Massage;
                        text.Append(NGMassage);
                        text.Append(OKMassage);
                        if (!OK)
                        {
                            HWindID.Disp_message(hWindowHalconID, text, 0, 0, true, "red", "false");
                        }
                        else
                        {
                            HWindID.Disp_message(hWindowHalconID, text, 0, 0, true, ColorResu.ToString(), "false");
                        }
                    }
                }
                if (!IsMoveBool)
                {
                    MaRed.ShowMassage(hWindowHalconID);
                    MaGreen.ShowMassage(hWindowHalconID);
                    MaBlue.ShowMassage(hWindowHalconID);
                    MaYellow.ShowMassage(hWindowHalconID);
                }
            }
            catch (Exception ex)
            {
                //AlarmText.AddTextNewLine("HalconResult显示故障:" + ex.StackTrace);
            }
        }
        /// <summary>
        /// 查看细节
        /// </summary>
        /// <param name="hWindowHalconID"></param>
        public void SelesShoOBJ(HTuple hWindowHalconID)
        {
            if (HObject == null)
            {
                return;
            }
            HOperatorSet.GetObjClass(HObject, out HTuple classv);
            HTuple row, colum, ar, height, width, ratio, circularity, compactness, convexity, Rectangularity = null;
            row = colum = ar = height = width = ratio = circularity = compactness = convexity = Rectangularity = null;
            HTuple hTuple = "";
            if (classv.Length == 0)
            {
                return;
            }
            if (classv[0] == "region")
            {
                HOperatorSet.AreaCenter(HObject, out ar, out row, out colum);
                HOperatorSet.HeightWidthRatio(HObject, out height, out width, out ratio);
                HOperatorSet.Circularity(HObject, out circularity);
                HOperatorSet.Compactness(HObject, out compactness);
                HOperatorSet.Convexity(HObject, out convexity);
                HOperatorSet.Rectangularity(HObject, out Rectangularity);
                hTuple = "X" + row + " Y" + colum + " 面积:" + ar + "高" + height + "宽" + width + "比例" + ratio + "圆度" + circularity
                + Environment.NewLine + "紧密度" + compactness + "凸面" + convexity + "长方形" + Rectangularity;
            }
            else if (classv[0] == "xld_cont")
            {
                HOperatorSet.AreaCenterXld(HObject, out ar, out row, out colum, out HTuple hTuple1);
                HOperatorSet.HeightWidthRatioXld(HObject, out height, out width, out ratio);
                HOperatorSet.CircularityXld(HObject, out circularity);
                HOperatorSet.CompactnessXld(HObject, out compactness);
                HOperatorSet.ConvexityXld(HObject, out convexity);
                hTuple = "X" + row + " Y" + colum + " 面积:" + ar + "高" + height + "宽" + width + "比例" + ratio + "圆度" + circularity
                + Environment.NewLine + "紧密度" + compactness + "凸面" + convexity;
            }
            try
            {
                HWindID.Disp_message(hWindowHalconID, hTuple, 120, 20, true, "red");
                HOperatorSet.SetColor(hWindowHalconID, "#ff000040");
                HOperatorSet.GenCrossContourXld(out HObject cross, row, colum, 20, 0);
                HOperatorSet.DispObj(cross, hWindowHalconID);
                HOperatorSet.DispObj(HObject, hWindowHalconID);
            }
            catch (Exception)
            {
            }
        }

        public static void ShowImae(HTuple hWindowHalconID, HObject image)
        {
            try
            {
                HSystem.SetSystem("flush_graphic", "false");
                //Massage = new HTuple();
                if (HWindID.IsObjectValided(image))
                {
                    HOperatorSet.GetImageSize(image, out HTuple Width, out HTuple Height);
                    HOperatorSet.SetPart(hWindowHalconID, 1, 1, Height - 1, Width - 1);
                    HOperatorSet.ClearWindow(hWindowHalconID);
                    HSystem.SetSystem("flush_graphic", "true");
                    HOperatorSet.DispObj(image, hWindowHalconID);
                    //HOperatorSet.DispObj(image, hWindowHalconID);
                }
            }
            catch (Exception)
            {
            }
        }




        /// <summary>
        /// 绿色区域
        /// </summary>
        public HObject HObjectGreen { get; set; } = new HObject();

        /// <summary>
        /// 黄色区域
        /// </summary>

        public HObject HObjectYellow { get; set; } = new HObject();

        /// <summary>
        /// 蓝色区域
        /// </summary>
        public HObject HObjectBlue { get; set; } = new HObject();

        public MassageText MaRed { get; set; } = new MassageText();

        public MassageText MaGreen { get; set; } = new MassageText();

        public MassageText MaYellow { get; set; } = new MassageText();

        public MassageText MaBlue { get; set; } = new MassageText();

        //public class Hobjt_Colro
        //{

        //    public Hobjt_Colro(HObject hObject, HTuple color = null)
        //    {
        //        Object = hObject;
        //        if (color == null)
        //        {
        //            Color = "green";
        //        }
        //        else
        //        {
        //            Color = color;
        //        }
        //    }

        //    public HObject Object = new HObject();
        //    public HTuple Color = new HTuple("green");
        //}

        public Dictionary<string, ObjectColor> GetNameHobj(Dictionary<string, ObjectColor> keyValuePairs = null)
        {
            if (keyValuePairs != null)
            {
                Dick = keyValuePairs;
            }
            return Dick;
        }

        private Dictionary<string, ObjectColor> Dick = new Dictionary<string, ObjectColor>();
        public List<ObjectColor> GetHobjt_s(List<ObjectColor> hobjt_Colros = null)
        {
            if (hobjt_Colros != null)
            {
                ListHobj = hobjt_Colros;
            }
            return ListHobj;
        }
        private List<ObjectColor> ListHobj = new List<ObjectColor>();


    }
}
