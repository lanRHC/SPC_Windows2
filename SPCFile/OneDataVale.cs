using HalconDotNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPC_Windows.SPCFile
{
    [JsonObject(MemberSerialization.OptOut)]
    /// <summary>
    /// 单个产品信息
    /// </summary>
    public class OneDataVale : IDisposable
    {
        public OneDataVale()
        {
            //if (RecipeCompiler.GetProductEX() != null)
            //{
            //    IsSNOK = RecipeCompiler.GetProductEX().IsSNOK;
            //}
            //product_name = Product.ProductionName;
        }

        public Color NGColor { get; set; } = Color.Red;

        [DescriptionAttribute("IsSNOK,判断SN是否ok"), Category("基础"), DisplayName("判断SN")]
        public bool IsSNOK { get; set; }

        public void Dispose()
        {
            try
            {
                KeyPamgr.Clear();
                foreach (var item in ListCamsData)
                {
                    if (item.Value.ResuOBj() != null)
                    {
                        for (int i = 0; i < item.Value.ResuOBj().Count; i++)
                        {
                            if (item.Value.ResuOBj()[i] != null)
                            {
                                item.Value.ResuOBj()[i].Dispose();
                            }
                        }
                    }
                    item.Value.Dispose();
                    //if (item.Value.GetImagePlus() != null)
                    //{
                    //    item.Value.GetImagePlus().Dispose();
                    //}
                }

            }
            catch (Exception)
            {

            }

        }
        public void DisposeImage()
        {
            try
            {
                KeyPamgr.Clear();
                foreach (var item in ListCamsData)
                {
                    if (item.Value.ResuOBj() != null)
                    {
                        for (int i = 0; i < item.Value.ResuOBj().Count; i++)
                        {
                            if (item.Value.ResuOBj()[i] != null)
                            {
                                item.Value.ResuOBj()[i].Dispose();
                            }
                        }
                    }
                    item.Value.Dispose();
                    if (item.Value.GetImagePlus() != null)
                    {
                        item.Value.GetImagePlus().Dispose();
                    }
                }

            }
            catch (Exception)
            {

            }

        }

        ~OneDataVale()
        {
            try
            {
                Dispose();
            }
            catch (Exception)
            {
            }
        }

        [DescriptionAttribute("数据存储地址"), Category("数据"), DisplayName("单面整图存储地址"), ReadOnly(false)]
        /// <summary>
        /// 整版图像地址
        /// </summary>
        public string ImagePath2 { get; set; } = "";

        [DescriptionAttribute("数据存储地址"), Category("数据"), DisplayName("FOV图存储地址"), ReadOnly(false)]

        public string FovImagePaht { get; set; } = "";
        /// <summary>
        /// 图像文件夹
        /// </summary>
        public string ImagePaht = "";

        [DescriptionAttribute("数据存储地址"), Category("数据"), DisplayName("数据存储地址"), ReadOnly(false)]
        public string DataPathFile { get; set; } = "";

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StrTime { get; set; } = DateTime.MinValue;
        [DescriptionAttribute(""), Category("数据"), DisplayName("整盘平均CT"), ReadOnly(false)]
        public double CycleTime { get; set; } = 1;

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }


        public string EndTimeString
        { get { return EndTime.ToString("HH:mm:ss"); } }

        public string StrTimeString
        { get { return StrTime.ToString("yy/MM/dd HH:mm:ss"); } }

        /// <summary>
        /// 动态参数
        /// </summary>

        public Dictionary<string, string> KeyPamgr { get; set; } = new Dictionary<string, string>();

        [DescriptionAttribute("DeviceName,设备名称"), Category("设备"), DisplayName("设备名称")]
        public string DeviceName { get; set; } = "AVI";

        [DescriptionAttribute(""), Category("设备"), DisplayName("线体名称"), ReadOnly(false)]
        public string LineName { get; set; }

        [DescriptionAttribute("UesrRest,是否人工处理"), Category("基础"), DisplayName("人工处理")]
        public bool UesrRest { get; set; }

        public void ResetOK()
        {
            Dictionary<string, bool> keyValuePairs = new Dictionary<string, bool>();
            foreach (var item in NGName)
            {
                keyValuePairs.Add(item.Key, true);
            }
            NGName = keyValuePairs;
            RestOk();
            SetOK(true);
            SetDone(true);
        }

        public void ResetNG()
        {
            SetOK(false);
            SetDone(false);
        }

        public void AddNG(string name, bool OKt = false)
        {
            NotNull = true;
            if (NGName == null)
            {
                NGName = new Dictionary<string, bool>();
            }
            if (!NGName.ContainsKey(name))
            {
                NGName.Add(name, OKt);
            }
            else
            {
                NGName[name] = OKt;
            }
        }

        [DescriptionAttribute("NGName,"), Category("基础"), DisplayName("NG附加")]
        public Dictionary<string, bool> NGName { get; set; } = new Dictionary<string, bool>();

        [DescriptionAttribute("PanelID,"), Category("数据"), DisplayName("产品SN")]
        public string PanelID
        {
            get { return sn; }
            set
            {
                sn = value;
            }

        }

        private string sn = "";
        [DescriptionAttribute("TraySN,"), Category("数据"), DisplayName("托盘/载具SN")]
        /// <summary>
        /// 载具SN
        /// </summary>
        public string TraySN { get; set; }
        [DescriptionAttribute("CoverID,"), Category("数据"), DisplayName("盖板SN")]
        /// <summary>
        /// 盖板SN
        /// </summary>
        public string CoverID { get; set; }
        [DescriptionAttribute("PoleID,"), Category("数据"), DisplayName("压盖SN")]
        /// <summary>
        /// 压杆SN
        /// </summary>
        public string PoleID { get; set; } = "";

        [DescriptionAttribute("Product_Name,"), Category("数据"), DisplayName("产品型号")]
        public string Product_Name
        {
            get
            {
                if (product_name == null)
                {
                    //product_name = Product.ProductionName;
                }
                return product_name;
            }
            set { product_name = value; }
        }

        private string product_name;

        [DescriptionAttribute("TrayLocation,"), Category("数据"), DisplayName("托盘位置")]
        public int TrayLocation { get; set; }

        [DescriptionAttribute("Data,"), Category("数据"), DisplayName("测试数据Listdouble")]
        public List<double> Data
        {
            get;
            set;
        }

        /// <summary>
        /// 更新托盘数据
        /// </summary>
        /// <returns></returns>
        public String GetDataString()
        {
            String DAT = TrayLocation.ToString();
            try
            {
                DAT += "SN:" + PanelID + Environment.NewLine;
                if (Data != null)
                {
                    DAT += "数据:" + Data.Count + Environment.NewLine;
                }
                foreach (var item in NGName)
                {
                    DAT += item.Key + ":";
                }
                if (MesStr != "")
                {
                    DAT += "Mes:" + MesStr;
                }
            }
            catch (Exception)
            { }
            return DAT;
        }

        [DescriptionAttribute("AutoOK,"), Category("数据"), DisplayName("机检OK")]
        public bool AutoOK { get; set; }
        /// <summary>
        /// OK结果 Pass/Fail
        /// </summary>
        [JsonIgnore]
        public string PassState
        {
            get
            {
                if (OK)
                {
                    return "Pass";
                }
                return "Fail";
            }
        }


        [DescriptionAttribute("Done,"), Category("数据"), DisplayName("完成")]
        public bool Done
        {
            get
            {
                foreach (var item in ListCamsData)
                {
                    if (!item.Value.Done) return false;
                }
                return true;
            }

        }
        public void SetDone(bool value)
        {
            foreach (var item in ListCamsData)
            {
                item.Value.SetDone(value);
            }
        }

        /// <summary>
        /// 复判OK
        /// </summary>
        /// <param name="okB"></param>
        public void SetOKBit(bool okB = true)
        {
            ok = okB;
        }

        [DescriptionAttribute("MesStr,"), Category("数据"), DisplayName("Mes信息")]
        public string MesStr { get; set; } = "";

        public bool MesRestOK { get; set; }

        [DescriptionAttribute("FVTStr,"), Category("数据"), DisplayName("FVT信息")]
        public string FVTStr { get; set; } = "";
        /// <summary>
        /// 是否有产品
        /// </summary>
        [DescriptionAttribute("NotNull,"), Category("数据"), DisplayName("是否有产品")]
        public bool NotNull { get; set; }

        private bool ok = true;

        /// <summary>
        /// 设置结果
        /// </summary>
        /// <param name="value"></param>
        public void SetOK(bool value)
        {
            Dictionary<string, bool> keyValuePairs = new Dictionary<string, bool>();
            foreach (var item in NGName)
            {
                keyValuePairs.Add(item.Key, value);
            }
            NGName = keyValuePairs;
            foreach (var item in ListCamsData)
            {
                item.Value.SetOK(value);
            }
            ok = value;
        }
        public void RestOk()
        {
            foreach (var item in ListCamsData)
            {
                foreach (var itemOBJ in item.Value.DicNGObj.DicOnes)
                {
                    itemOBJ.Value.RAddOK();
                }

            }
        }

        [DescriptionAttribute("OK,"), Category("数据"), DisplayName("结果OK")]
        public bool OK
        {
            get
            {
                if (!NotNull)
                {
                    return false;
                }
                foreach (var item in ListCamsData)
                {
                    if (!item.Value.aOK)
                        return false;
                }
                foreach (var item in NGName)
                {
                    if (!item.Value)
                        return false;
                }
                if (!ok)
                {
                    return false;
                }
                //if (IsSNOK && this.PanelID == "")
                //{
                //    return false;
                //}
                return true;
            }
            //set
            //{
            //    if (!Done)
            //    {
            //        Dictionary<string, bool> keyValuePairs = new Dictionary<string, bool>();
            //        foreach (var item in NGName)
            //        {
            //            keyValuePairs.Add(item.Key, value);
            //        }
            //        NGName = keyValuePairs;
            //        foreach (var item in ListCamsData)
            //        {
            //            item.Value.SetOK ( value);
            //        }
            //        ok = value;
            //    }
            //}
        }
        public void SetROK(bool value)
        {
            if (!Done)
            {
                Dictionary<string, bool> keyValuePairs = new Dictionary<string, bool>();
                foreach (var item in NGName)
                {
                    keyValuePairs.Add(item.Key, value);
                }
                NGName = keyValuePairs;
                foreach (var item in ListCamsData)
                {
                    item.Value.SetOK(value);
                }
                ok = value;
            }
        }

        [DescriptionAttribute("NGNumber,"), Category("数据"), DisplayName("NG数量")]
        public int NGNumber
        {
            get
            {
                int nubmer = 0;

                foreach (var item in GetAllCompOBJs().DicOnes)
                {
                    if (!item.Value.aOK)
                    {
                        nubmer++;
                    }
                }
                return nubmer;
            }
        }

        [DescriptionAttribute("ErrJudNumber,"), Category("数据"), DisplayName("误判数量")]
        public int ErrJudNumber
        {
            get
            {
                int errNumber = 0;
                foreach (var item in ListCamsData)
                {
                    errNumber = +item.Value.ErrJudNumber;
                }
                return errNumber;
            }
        }

        public Dictionary<string, OneCamData> ListCamsData { get; set; } = new Dictionary<string, OneCamData>();


        public Dictionary<string, List<DataMinMax.Reference_Valuves>> M3Dmove { get; set; } = new Dictionary<string, List<DataMinMax.Reference_Valuves>>();

        /// <summary>
        /// 获取未复判的ng区域
        /// </summary>
        /// <returns></returns>
        public OneCompOBJs GetNGCompData(string Cam = null)
        {
            string data = "";
            if (Cam == null)
            {
                foreach (var item in ListCamsData)
                {
                    data = item.Key;
                    if (!item.Value.Done)
                    {
                        return item.Value.DicNGObj;
                    }
                }
                return ListCamsData[data].DicNGObj;
            }
            else if (ListCamsData.ContainsKey(Cam))
            {
                return ListCamsData[Cam].DicNGObj;
            }
            foreach (var item in ListCamsData)
            {
                data = item.Key;
                return ListCamsData[data].DicNGObj;
            }
            return null;
        }

        /// <summary>
        /// 获取全部结果
        /// </summary>
        /// <returns></returns>
        public OneCompOBJs GetAllCompOBJs()
        {
            OneCompOBJs OneCompO = new OneCompOBJs();
            foreach (var item in ListCamsData)
            {
                foreach (var itemdet in item.Value.DicNGObj.DicOnes)
                {
                    OneCompO.Add(itemdet.Value);
                }
            }
            return OneCompO;
        }

        public HObject GetNGImage()
        {
            foreach (var item in ListCamsData)
            {
                if (!item.Value.Done)
                {
                    return item.Value.GetImagePlus();
                }
            }
            foreach (var item in ListCamsData)
            {
                return item.Value.GetImagePlus();
            }
            return null;
        }

        public HObject[] GetImages(string camName)
        {
            List<HObject> hObjects = new List<HObject>();
            List<int> runIDs = new List<int>();
            try
            {
                if (ListCamsData.ContainsKey(camName))
                {
                    for (int i = 0; i < ListCamsData[camName].ResuOBj().Count; i++)
                    {
                        if (!runIDs.Contains(ListCamsData[camName].ResuOBj()[i].LiyID))
                        {
                            runIDs.Add(ListCamsData[camName].ResuOBj()[i].LiyID);
                            hObjects.Add(ListCamsData[camName].ResuOBj()[i].Image);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return hObjects.ToArray();
        }

        public void Scale_Image()
        {
            try
            {
        
                foreach (var item in ListCamsData)
                {
                    if (item.Value.GetImagePlus().IsInitialized())
                    {
                        HOperatorSet.GetImageSize(item.Value.GetImagePlus(), out HTuple width, out HTuple height);
                        HOperatorSet.ZoomImageSize(item.Value.GetImagePlus(), out HObject hObject, width , height, "constant");
                        item.Value.GetImagePlus(hObject);
                    }

                    for (int i = 0; i < item.Value.ResuOBj().Count; i++)
                    {
                        HOperatorSet.ZoomImageSize(item.Value.ResuOBj()[i].Image, out HObject hObject, item.Value.ResuOBj()[i].Width, item.Value.ResuOBj()[i].Height, "constant");
                        item.Value.ResuOBj()[i].Image.Dispose();
                        item.Value.ResuOBj()[i].Image = hObject;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 获取NG面
        /// </summary>
        /// <returns></returns>
        public string GetNGCamName()
        {
            foreach (var item in ListCamsData)
            {
                if (!item.Value.Done)
                {
                    return item.Key;
                }
            }
            return "";
        }

        public void AddCamsData(string runName, OneCamData oneCam)
        {
            if (!ListCamsData.ContainsKey(runName))
            {
                ListCamsData.Add(runName, oneCam);
            }
            else
            {
                ListCamsData[runName] = oneCam;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="runName"></param>
        /// <param name="imagePalus"></param>
        public void AddCamsData(string runName, HObject imagePalus)
        {
            if (!ListCamsData.ContainsKey(runName))
            {
                // ListCamsData.Add(runName, oneCam);
            }
            else
            {
                ListCamsData[runName].GetImagePlus(imagePalus);
            }
        }

        public void AddCamOBj(string camName, OneCompOBJs.OneComponent oneRObj)
        {
            if (oneRObj.AutoOK == false)
            {
            }
            if (ListCamsData.ContainsKey(camName))
            {
                ListCamsData[camName].DicNGObj.Add(oneRObj);
            }
            else
            {
                ListCamsData.Add(camName, new OneCamData());
                ListCamsData[camName].DicNGObj.Add(oneRObj);
            }
        }
    }
}
