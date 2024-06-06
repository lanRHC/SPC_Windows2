using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPC_Windows.SPCFile
{
    public class SpcData
    {
        public SpcData(string dataString)
        {
            string[] datas = dataString.Split(',');
            try
            {

                if (datas.Length >= 17
                    )
                {
                    this.ProductName = datas[1];
                    this.Result = datas[2];
                    this.Material_no = datas[14];
                    this.Name = datas[3];
                    this.SN = datas[4];
                    this.CamName = datas[15];
                    if (datas[5] == "Fail") OK = false;
                    else OK = true;

                    DateTime.TryParse(datas[6] + " " + datas[7], out DateTime formatProvider);
                    Start_DataTime = formatProvider;
                    DateTime.TryParse(datas[6] + " " + datas[8], out formatProvider);
                    this.End_DataTime = formatProvider;
                    this.Start_Time = datas[7];
                    this.UserName = datas[9];
                    this.Placement = datas[10];
                    this.Location = datas[11];
                    this.AutoOk = datas[12];
                }
                else if (datas.Length == 15)
                {


                    this.ProductName = datas[1];
                    this.Result = datas[2];
                    this.Name = datas[3];
                    this.SN = datas[4];
                    if (datas[5] == "Fail") OK = false;
                    else OK = true;
                    DateTime.TryParse(datas[6] + " " + datas[7], out DateTime formatProvider);
                    Start_DataTime = formatProvider;
                    DateTime.TryParse(datas[6] + " " + datas[8], out formatProvider);
                    this.End_DataTime = formatProvider;
                    this.Start_Time = datas[7];
                    this.UserName = datas[9];
                    this.Placement = datas[10];
                    this.Location = datas[11];
                    this.AutoOk = datas[12];
                }
            }
            catch (Exception)
            {

            }
        }
        public SpcData(string[] datas, string imagePath)
        {
            try
            {

                if (datas.Length >= 17)
                {
                    this.ProductName = datas[1];
                    this.Result = datas[2];
                    this.Material_no = datas[14];
                    this.Name = datas[3];
                    this.SN = datas[4];
                    if (datas[5] == "Fail") OK = false;
                    else OK = true;
                    this.CamName = datas[15];

                    DateTime.TryParse(datas[6] + " " + datas[7], out DateTime formatProvider);
                    Start_DataTime = formatProvider;
                    DateTime.TryParse(datas[6] + " " + datas[8], out formatProvider);
                    this.End_DataTime = formatProvider;
                    this.Start_Time = datas[7];
                    this.UserName = datas[9];
                    this.Placement = datas[10];
                    this.Location = datas[11];
                    this.AutoOk = datas[12];
                }
                else if (datas.Length == 15)
                {
                    this.ProductName = datas[1];
                    this.Result = datas[2];
                    this.Name = datas[3];
                    this.SN = datas[4];
                    if (datas[5] == "Fail") OK = false;
                    else OK = true;
                    DateTime.TryParse(datas[6] + " " + datas[7], out DateTime formatProvider);
                    Start_DataTime = formatProvider;
                    DateTime.TryParse(datas[6] + " " + datas[8], out formatProvider);
                    this.End_DataTime = formatProvider;
                    this.Start_Time = datas[7];
                    this.UserName = datas[9];
                    this.Placement = datas[10];
                    this.Location = datas[11];
                    this.AutoOk = datas[12];
                }
                string dataPaths = imagePath + "\\" +
                this.Start_DataTime.ToString("yyyy年MM月dd日") + "\\" + this.ProductName + "\\" + this.SN;
                string imageFile = this.Material_no + "_" + this.Name + "_" + this.Location + "_" + this.CamName + "_" +
                      DateTime.Parse(this.Start_Time).ToString("HHmmss");
                if (Directory.Exists(dataPaths))
                {
                    string paths = dataPaths + "\\" + imageFile + "_";
                    if (this.OK)
                        paths += "P.jpg";
                    else paths += "F.jpg";
                    if (!File.Exists(paths))
                    {
                        string[] imagepahts = Directory.GetFiles(dataPaths);
                        //paths = imagepahts.First(n => n.Contains(this.Name));
                    }
                    this.ImagePath = paths;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public SpcData()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public string SN { get; set; }
        /// <summary>
        /// 元件类型
        /// </summary>
        public string Material_no { get; set; }
        /// <summary>
        /// 原件名
        /// </summary>
        public string Name { get; set; }

        public bool OK { get; set; }
        /// <summary>
        /// 复盘结果
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public string Start_Time { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Start_DataTime { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime End_DataTime { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        ///机台号
        /// </summary>
        public string Placement { get; set; }
        /// <summary>
        ///测试值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 检查面
        /// </summary>
        public string CamName { get; set; }
        /// <summary>
        /// 产品型号
        /// </summary>

        public string ProductName { get; set; }
        /// <summary>
        /// 位置
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 机检结果
        /// </summary>
        public string AutoOk { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImagePath { get; set; }



    }
    public class SpcData2
    {
        public SpcData2(string dataString)
        {
            string[] datas = dataString.Split(',');
            string data_1 = datas[6].Split(' ')[0];
            try
            {

                if (datas.Length >= 16
                    )
                {
                    this.ProductName = datas[1];
                    this.Result = datas[2];
                    this.Material_no = datas[14];
                    this.Name = datas[3];
                    this.SN = datas[4];
                    this.CamName = datas[15];
                    if (datas[5] == "Fail") OK = false;
                    else OK = true;

                    DateTime.TryParse(data_1 + " " + datas[7], out DateTime formatProvider);
                    Start_DataTime = formatProvider;
                    DateTime.TryParse(data_1 + " " + datas[8], out formatProvider);
                    this.End_DataTime = formatProvider;
                    this.Start_Time = datas[7];
                    this.UserName = datas[9];
                    this.Placement = datas[10];
                    this.Location = datas[11];
                    this.AutoOk = datas[12];
                }
                else if (datas.Length == 15)
                {
                    //string data_1 = datas[6].Split(' ')[0];

                    this.ProductName = datas[1];
                    this.Result = datas[2];
                    this.Name = datas[3];
                    this.SN = datas[4];
                    if (datas[5] == "Fail") OK = false;
                    else OK = true;
                    DateTime.TryParse(data_1 + " " + datas[7], out DateTime formatProvider);
                    Start_DataTime = formatProvider;
                    DateTime.TryParse(data_1 + " " + datas[8], out formatProvider);
                    this.End_DataTime = formatProvider;
                    this.Start_Time = datas[7];
                    this.UserName = datas[9];
                    this.Placement = datas[10];
                    this.Location = datas[11];
                    this.AutoOk = datas[12];
                }
            }
            catch (Exception)
            {

            }
        }
        public SpcData2(string[] datas, string imagePath)
        {
            try
            {
                string data_1 = datas[6].Split(' ')[0];
                if (datas.Length >= 16)
                {
                    this.ProductName = datas[1];
                    this.Result = datas[2];
                    this.Material_no = datas[14];
                    this.Name = datas[3];
                    this.SN = datas[4];
                    if (datas[5] == "Fail") OK = false;
                    else OK = true;
                    this.CamName = datas[15];

                    DateTime.TryParse(data_1 + " " + datas[7], out DateTime formatProvider);
                    Start_DataTime = formatProvider;
                    DateTime.TryParse(data_1 + " " + datas[8], out formatProvider);
                    this.End_DataTime = formatProvider;
                    this.Start_Time = datas[7];
                    this.UserName = datas[9];
                    this.Placement = datas[10];
                    this.Location = datas[11];
                    this.AutoOk = datas[12];
                }
                else if (datas.Length == 15)
                {
                    this.ProductName = datas[1];
                    this.Result = datas[2];
                    this.Name = datas[3];
                    this.SN = datas[4];
                    if (datas[5] == "Fail") OK = false;
                    else OK = true;
                    DateTime.TryParse(data_1 + " " + datas[7], out DateTime formatProvider);
                    Start_DataTime = formatProvider;
                    DateTime.TryParse(data_1 + " " + datas[8], out formatProvider);
                    this.End_DataTime = formatProvider;
                    this.Start_Time = datas[7];
                    this.UserName = datas[9];
                    this.Placement = datas[10];
                    this.Location = datas[11];
                    this.AutoOk = datas[12];
                }
                string dataPaths = imagePath + "\\" +
                this.Start_DataTime.ToString("yyyy年MM月dd日") + "\\" + this.ProductName + "\\" + this.SN;
                string imageFile = this.Material_no + "_" + this.Name + "_" + this.Location + "_" + this.CamName + "_" +
                      DateTime.Parse(this.Start_Time).ToString("HHmmss");
                if (Directory.Exists(dataPaths))
                {
                    string paths = dataPaths + "\\" + imageFile + "_";
                    if (this.OK)
                        paths += "P.jpg";
                    else paths += "F.jpg";
                    if (!File.Exists(paths))
                    {
                        string[] imagepahts = Directory.GetFiles(dataPaths);
                        //paths = imagepahts.First(n => n.Contains(this.Name));
                    }
                    this.ImagePath = paths;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public SpcData2()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public string SN { get; set; }
        /// <summary>
        /// 元件类型
        /// </summary>
        public string Material_no { get; set; }
        /// <summary>
        /// 原件名
        /// </summary>
        public string Name { get; set; }

        public bool OK { get; set; }
        /// <summary>
        /// 复盘结果
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public string Start_Time { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Start_DataTime { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime End_DataTime { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        ///机台号
        /// </summary>
        public string Placement { get; set; }
        /// <summary>
        ///测试值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 检查面
        /// </summary>
        public string CamName { get; set; }
        /// <summary>
        /// 产品型号
        /// </summary>

        public string ProductName { get; set; }
        /// <summary>
        /// 位置
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 机检结果
        /// </summary>
        public string AutoOk { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImagePath { get; set; }



    }

}
