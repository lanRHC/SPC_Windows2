using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SPC_Windows.SPCFile
{
    /// <summary>
    /// 托盘数据
    /// </summary>
    public class TrayData
    {
        //public bool IsSn
        //{ get { return tray1.IsSN; } }

        public bool IsTryaSN { get; set; }

        /// <summary>
        /// 完成
        /// </summary>
        public bool Await;

        public bool isRestImage;
    


        public Dictionary<string, string> CamPath { get; set; } = new Dictionary<string, string>();


        public bool GetImage
        {
            get
            {
                try
                {
                    Dictionary<string, string> CamPa = new Dictionary<string, string>();
                    foreach (var itemd in this.CamPath)
                    {
                        string image = itemd.Value;
                        string imagePt = itemd.Value;
                        if (Paths != null)
                        {
                            if (Paths.StartsWith(@"\\"))
                            {
                                string[] data = Paths.Split('\\');
                                image = @"\\" + data[2] + "\\" + imagePt.Substring(0, 1) + imagePt.Remove(0, 2);
                            }
                            if (File.Exists(image))
                            {
                                CamPath.Add(itemd.Key, image);
                            }
                        }

                    }

                }
                catch (Exception ex)
                {

                }


                return false;
            }
        }
        /// <summary>
        /// 比例
        /// </summary>
        public double Percentum { get; set; } = 100;

        public TrayData()
        {
        }


        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            try
            {
                for (int i = 0; i < this.Count; i++)
                {
                    this.GetDataVales()[i].Dispose();
                }
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetTrayString()
        {
            string mesaget = string.Format("ID:{0},当前位置:{1},TraySN:{2};X{3}*Y{4}/数量:{5},NG数:{6},OK数:{7}",
                      Name, Number, this.TrayIDQR, XNumber, YNumber, Count, NGNubmer, OKNumber);
            mesaget += "SN识别率%" + PercentumN.ToString("0.00");
            return mesaget;
        }
        /// <summary>
        /// 良率
        /// </summary>
        public double PercentumN
        {
            get
            {
                if (OKNumber == 0)
                {
                    return 0.00;
                }
                else
                {
                    int not = 0;
                    for (int i = 0; i < GetDataVales().Count; i++)
                    {
                        if (GetDataVales()[i].NotNull)
                        {
                            if (GetDataVales()[i].PanelID != "")
                            {
                                not++;
                            }
                        }
                    }
                    double percentum = (double)not / (double)Count * 100.0;
                    return Math.Round(percentum, 2);
                }
            }
        }



   

        public List<string> GetTraySN()
        {
            List<string> listSn = new List<string>();
            for (int i = 0; i < dataVales1.Count; i++)
            {
                listSn.Add(dataVales1[i].PanelID);
            }
            return listSn;
        }

  

        [DescriptionAttribute("托盘数据保存的文件夹地址。"), Category("数据"), DisplayName("托盘数据保存文件夹地址")]
        public string DataPath { get; set; } = "";

        //[DescriptionAttribute("起点位置四个角落，并根据横向或竖向共8种排列方式。"), Category("排列"), DisplayName("起点位置")]
        //public TrayDirectionEnum TrayDirection { get; set; }

        [DescriptionAttribute("横向Fales或竖向Ture。"), Category("排列"), DisplayName("横向或竖向")]
        public bool HorizontallyORvertically { get; set; }

        [DescriptionAttribute("X方向数量。"), Category("排列"), DisplayName("X方向数量")]
        public sbyte XNumber { get; set; }

        [DescriptionAttribute("Y方向数量。"), Category("排列"), DisplayName("Y方向数量")]
        public sbyte YNumber { get; set; }

        [DescriptionAttribute("结果状态。"), Category("数据"), DisplayName("托盘产品数量")]
        public List<sbyte> bitW { get; set; } = new List<sbyte>();


        [DescriptionAttribute("判断整盘结果信息，托盘ID，穴位ID，穴位数据等。"), Category("结果"), DisplayName("整盘是否OK")]
        public bool OK
        {
            get
            {
                try
                {
                    if (this.IsTryaSN)
                    {
                        if (TrayIDQR == null || TrayIDQR == "")
                        {
                            return false;
                        }
                    }
                    if (GetDataVales() != null)
                    {
                        if (Percentum == 100)
                        {
                            int not = 0;
                            for (int i = 0; i < GetDataVales().Count; i++)
                            {
                                if (GetDataVales()[i].NotNull)
                                {
                                    if (!GetDataVales()[i].OK)
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    not++;
                                }
                            }
                            if (not == GetDataVales().Count)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (PercentumN <= Percentum)
                            {
                                return false;
                            }
                        }

                    }
                    return true;
                }
                catch (Exception)
                {
                }
                return false;
            }
        }

        //bool Not = false;
        public string MesRestStr = "";

        public string ErrString = "";
        /// <summary>
        /// 
        /// </summary>

        public DateTime StadateTime = DateTime.Now;


        public double CycleTime { get; set; }
        /// <summary>
        ///人工处理
        /// </summary>
        public bool UserRest { get; set; }

        /// <summary>
        /// 远程文件地址
        /// </summary>
        public string Paths { get; set; }

        /// <summary>
        /// 复判完成
        /// </summary>
        public bool Done
        {
            get
            {
                if (dataVales1 != null)
                {
                    for (int i = 0; i < dataVales1.Count; i++)
                    {
                        if (dataVales1[i] != null)
                        {
                            if (!dataVales1[i].Done)
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
        }
        List<int> list;
        /// <summary>
        /// NG位置
        /// </summary>
        public List<int> NGLocation
        {

            get
            {

                try
                {
                    if (list == null)
                    {
                        list = new List<int>();
                    }
                    if (dataVales1 == null)
                    {
                        return list;
                    }
                    if (list.Count != dataVales1.Count)
                    {
                        list = new List<int>();
                        for (int i = 0; i < dataVales1.Count; i++)
                        {
                            if (!dataVales1[i].OK)
                            {
                                list.Add(i + 1);
                            }
                        }
                    }
                }
                catch (Exception)
                { }

                return list;
            }
            set { }
        }

        public int NGNubmer
        {
            get
            {
                int numbert = 0;
                for (int i = 0; i < dataVales1.Count; i++)
                {
                    if (!dataVales1[i].OK)
                    {
                        numbert++;
                    }
                }
                return numbert;
            }
        }

        public int OKNumber
        {
            get
            {
                int numbert = 0;
                for (int i = 0; i < dataVales1.Count; i++)
                {
                    if (dataVales1[i].OK)
                    {
                        numbert++;
                    }
                }
                return numbert;
            }
        }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string Product_Name { get; set; }

        /// <summary>
        /// 多拼或单拼
        /// </summary>
        public bool IsPlusPanl { get; set; }

        public string Name { get; set; }

        [DescriptionAttribute("托盘ID。"), Category("结果"), DisplayName("托盘ID")]
        public string TrayIDQR
        {
            get { return sn; }
            set
            {
                if (value == "")
                {

                }
                sn = value;
            }
        }
        string sn = "";
        [DescriptionAttribute("托盘位置。"), Category("数据"), DisplayName("托盘位置")]
        public int Number
        {
            get;
            set;
        }

        [DescriptionAttribute("穴位数量。"), Category("排列"), DisplayName("总数量")]
        public int Count
        {
            get
            {
                return XNumber * YNumber;
            }
        }

        public List<OneDataVale> dataVales1 { get; set; }

        public List<OneDataVale> GetDataVales(List<OneDataVale> dataVales = null)
        {
            if (dataVales != null)
            {
                dataVales1 = dataVales;
            }
            return dataVales1;
        }

        public OneCompOBJs GetOneCompOBJs(string camName)
        {
            OneCompOBJs oneCompOBJs = new OneCompOBJs();
            try
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this.GetOneDataVale(i).NotNull)
                    {
                        oneCompOBJs.AddDic(this.GetOneDataVale(i).GetNGCompData(camName), i + 1);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return oneCompOBJs;
        }
        /// <summary>
        ///获取产品索引
        /// </summary>
        /// <param name="nubemr">索引</param>
        /// <returns></returns>
        public OneDataVale GetOneDataVale(int idex)
        {
            if (dataVales1 != null)
            {
                if (this.Count > idex)
                {
                    return GetDataVales()[idex];
                }
            }
            return null;
        }

        public HObject ImagePlus;

        public void RestValue()
        {
            Clear();
      
        }

        /// <summary>
        /// 清除数据
        /// </summary>
        public void Clear()
        {
            //if (tray1 != null)
            //{
            //    //tray1.GetTrayData().StadateTime = new DateTime();
            //    XNumber = tray1.XNumber;
            //    YNumber = tray1.YNumber;
            //    TrayDirection = tray1.TrayDirection;
            //    TrayIDQR = "";
            //    HorizontallyORvertically = tray1.HorizontallyORvertically;
            //}
            Await = true;
            Number = 1;
            dataVales1 = new List<OneDataVale>(new OneDataVale[XNumber * YNumber]);
            for (int i = 0; i < dataVales1.Count; i++)
            {
                dataVales1[i] = new OneDataVale();
                dataVales1[i].TrayLocation = (i + 1);
                dataVales1[i].StrTime = DateTime.Now;
            }
            this.StadateTime = DateTime.Now;
        }


        public void SetNumberValue(bool Vaules)
        {
            for (int i = 0; i < dataVales1.Count; i++)
            {
                if (Vaules)
                {
                    if (dataVales1[i].PanelID != "")
                    {
                        dataVales1[i].SetOK(Vaules);
                    }
                }
                else
                {
                    dataVales1[i].SetOK(Vaules);
                }

                if (Vaules)
                {
                    dataVales1[i].NotNull = Vaules;
                }
            }
      

        }
 


        /// <summary>
        ///
        /// </summary>
        /// <param name="d"></param>
        public void UPsetTrayNumbar(int d)
        {
            try
            {
                d = d - 1;
                int rowt = d / XNumber;
                int colt = d % XNumber;
                bitW[d] = 1;
                Number = (sbyte)d + 1;
            }
            catch (Exception)
            {
            }
        }
  

    

 

    }
}
