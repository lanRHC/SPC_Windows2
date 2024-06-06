
using SPC_Windows.Project;
using SPC_Windows.SPCFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Vision2.SPCFile
{
    public static class SPC
    {

        public delegate void RefreshData();

        public static event RefreshData RefreshDataEvent;

        /// <summary>
        /// 刷新SPC
        /// </summary>
        public static void OnRefreshDataEvent()
        {
            WriteSpcData();
            RefreshDataEvent?.Invoke();
        }
        /// <summary>
        /// 读取计数数据
        /// </summary>
        public static void ReadData(string productName)
        {
            try
            {
                string dateTimeString = DateTime.Now.ToString("yy年MM月dd日");
                Directory.CreateDirectory(ProjectINI.TempPath + "计数\\");
                if (File.Exists(ProjectINI.TempPath + "计数\\" + dateTimeString + "每时数据.txt"))
                {
                    if (!ProjectINI.ReadPathJsonToCalssEX(ProjectINI.TempPath + "计数\\" + dateTimeString + "每时数据.txt", out oKNumberClasses))
                    {
                        oKNumberClasses = new OKNGNumber[24];
                        for (int i = 0; i < oKNumberClasses.Length; i++)
                        {
                            oKNumberClasses[i] = new OKNGNumber();
                        }
                    }
                }
                else
                {
                    oKNumberClasses = new OKNGNumber[24];
                    for (int i = 0; i < oKNumberClasses.Length; i++)
                    {
                        oKNumberClasses[i] = new OKNGNumber();
                    }
                }
                if (File.Exists(ProjectINI.TempPath + productName + "计数Json.txt"))
                {
                    if (!ProjectINI.ReadPathJsonToCalssEX(ProjectINI.TempPath + productName + "计数Json.txt", out OKNumber))
                    {
                        OKNumber = new OKNumberClass();
                    }
                }
                else
                {
                    OKNumber = new OKNumberClass();
                }
                if (File.Exists(ProjectINI.TempPath + "计数\\" + dateTimeString + "计数.txt"))
                {
                    if (!ProjectINI.ReadPathJsonToCalssEX(ProjectINI.TempPath + "计数\\" + dateTimeString + "计数.txt", out SPCDicProduct))
                    {
                        SPCDicProduct = new Dictionary<string, OKNumberClass>();
                    }
                }

            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 写入文本
        /// </summary>
        public static void WriteSpcData()
        {
            try
            {
                string dateTimeString = DateTime.Now.ToString("yy年MM月dd日");
                ProjectINI.ClassToJsonSavePath(SPC.OKNumber, ProjectINI.TempPath + OKNumber.ProductName + "计数Json.txt");
                Directory.CreateDirectory(ProjectINI.TempPath + "计数\\");
                ProjectINI.ClassToJsonSavePath(SPC.SPCDicProduct, ProjectINI.TempPath + "计数\\" + dateTimeString + "计数.txt");
                ProjectINI.ClassToJsonSavePath(SPC.oKNumberClasses, ProjectINI.TempPath + "计数\\" + dateTimeString + "每时数据.txt");
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ResetDATA()
        {
            if (OKNumber==null)
            {
                OKNumber = new OKNumberClass();
            }
            //OKNumber.ProductName =Product.ProductionName;
            OKNumber.OKNumber = 0;
            OKNumber.NGNumber = 0;
            OKNumber.AutoNGNumber = 0;
            OKNumber.AutoOKNumber = 0;
            OKNumber.IsOK = false;
            OKNumber.TrayNumber = 0;
            OKNumber.TrayOKNumber = 0;
            OKNumber.ComponentS.Clear();
            OnRefreshDataEvent();

        }

        /// <summary>
        /// 获取计数显示文本
        /// </summary>
        /// <returns></returns>

        public static string GetSpcText()
        {
            string text = "";
            try
            {
                text += "OK:" + OKNumber.OKNumber+ " NG:" + OKNumber.NGNumber + Environment.NewLine;
                text += "良率%:" + OKNumber.OKNG.ToString("0.000") + Environment.NewLine;
                text += "元件数:" + OKNumber.ComponentCont + Environment.NewLine;
                var distinctValues = OKNumber.ComponentS.Select(p => p.Value.BankNmae).Distinct().ToList(); distinctValues.Remove("");
                text += "库总数:" + distinctValues.Count + Environment.NewLine;
                int ok = 0;
                int ng = 0;
                foreach (var item in OKNumber.ComponentS)
                {
                    ok += item.Value.OKNumber;
                    ng += item.Value.NGNumber;
                }
                text += "元件OK:" + ok + Environment.NewLine;
                text += "元件NG:" + ng + Environment.NewLine;
                Single cdetcont = (Single)Math.Round(((double)ok / (double)(ok + ng)) * 100.0F, 2);
                text += "元件良率:" + cdetcont.ToString("0.000") + Environment.NewLine;
                var list = from n in OKNumber.ComponentS
                           orderby n.Value.NGNumber descending
                           select n;
                int number = 1;
                foreach (var item in list)
                {
                    text += "Top" + number + ":" + item.Value.Nmae + " 数量:" + item.Value.NGNumber + Environment.NewLine;
                    number++;
                    if (number >= 6)
                    {
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
            }
            return text;
        }
        /// <summary>
        ///添加单个计数
        /// </summary>
        /// <param name="IsOk">OK或NG</param>
        public static void AddOKNumber(bool IsOk)
        {
            try
            {
                int det = DateTime.Now.Hour;
                if (IsOk)
                {
                    oKNumberClasses[det].OKNumber++;
                    OKNumber.OKNumber++;
                }
                else
                {
                    oKNumberClasses[det].NGNumber++;
                    OKNumber.NGNumber++;
                }
                OKNumber.IsOK = IsOk;
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static void AddRlsNumber()
        {
            try
            {
                OKNumber.AutoNGNumber++;
                //int det = DateTime.Now.Hour;
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 直通OK
        /// </summary>
        public static void AddRlsOKNumber()
        {
            try
            {
                OKNumber.AutoOKNumber++;
                //int det = DateTime.Now.Hour;
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        public static void AddNGNumber(int number = 1)
        {
            try
            {
                int det = DateTime.Now.Hour;
                OKNumber.NGNumber += number;
                oKNumberClasses[det].NGNumber += number;
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        public static void AddOKNumber(int number = 1)
        {
            try
            {
                int det = DateTime.Now.Hour;
                OKNumber.OKNumber += number;
                oKNumberClasses[det].OKNumber += number;
            }
            catch (Exception)
            {

            }
        }


        /// <summary>
        /// 更改复判结果
        /// </summary>
        /// <param name="isOK">结果</param>
        /// <param name="id">结果I</param>
        public static void AlterNumber(bool isOK)
        {
            if (OKNumber.NGNumber < 0)
            {
                OKNumber.NGNumber = 0;
            }
        }

        /// <summary>
        /// 当天产品数据统计
        /// </summary>
        public static Dictionary<string, OKNumberClass> SPCDicProduct = new Dictionary<string, OKNumberClass>();


        /// <summary>
        /// 当前产品
        /// </summary>
        public static OKNumberClass OKNumber = new OKNumberClass();


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="trayID"></param>
        //public static void ADDTrayNumber(int trayID)
        //{
        //    try
        //    {
        //        if (trayID < 0)
        //        {
        //            return;
        //        }
        //        ErosSocket.DebugPLC.Robot.TrayRobot trayRobot = DebugCompiler.GetTray(trayID);
        //        trayRobot.EnbleSNCont = ProductEX.GetProductEX().EnbleSNCont;
        //        trayRobot.SNCont = ProductEX.GetProductEX().SNCont;
        //        List<int> NG = new List<int>();
        //        List<int> NGCont = new List<int>();
        //        NgQRID ngQRID = new NgQRID();
        //        ngQRID.NGNubmerID = NG;
        //        ngQRID.NGContNumber = NGCont;
        //        bool ok = true;
        //        for (int i = 0; i < trayRobot.Count; i++)
        //        {
        //            ok = true;
        //            trayRobot.GetTrayData().GetOneDataVale(i).NotNull = true;
        //            if (trayRobot.GetTrayData().GetOneDataVale(i).PanelID == "")
        //            {
        //                ok = false;
        //                NG.Add(i);
        //                trayRobot.GetTrayData().GetOneDataVale(i).NGColor = System.Drawing.Color.Red;
        //            }
        //            if (trayRobot.EnbleSNCont)
        //            {
        //                if (trayRobot.GetTrayData().GetOneDataVale(i).PanelID.Length != 0)
        //                {
        //                    if (trayRobot.GetTrayData().GetOneDataVale(i).PanelID.Length != trayRobot.SNCont)
        //                    {
        //                        ok = false;
        //                        NGCont.Add(i + 1);
        //                        trayRobot.GetTrayData().GetOneDataVale(i).NGColor = System.Drawing.Color.Blue;
        //                    }
        //                }
        //            }
        //            trayRobot.GetTrayData().GetOneDataVale(i).SetOK(ok);
        //        }
        //        ngQRID.QRS = trayRobot.GetTrayData().TrayIDQR;
        //        //DebugCompiler.GetTray(trayID).NG_QR_IDT.Add(ngQRID);
        //        //if (DebugCompiler.GetTray(trayID).GetITrayRobot() != null)
        //        //{
        //        //    DebugCompiler.GetTray(trayID).GetITrayRobot().UpData();
        //        //}
        //        if (trayRobot.AddNumber)
        //        {
        //            AddOKNumber(trayRobot.GetTrayData().OKNumber);
        //            AddNGNumber(trayRobot.GetTrayData().NGNubmer);
        //        }
        //        if (trayRobot.GetTrayData().OK)
        //        {
        //            OKNumber.AddTrayNumber(true);
        //        }
        //        else
        //        {
        //            OKNumber.AddTrayNumber(false);
        //        }
        //        bool NGCorr = false;
        //        trayRobot.GetTrayData().ErrString = "";
        //        if (trayRobot.CrossNGNumber != 0 && trayRobot.NGCrossNumberBool)
        //        {
        //            trayRobot.GetTrayData().GetNgLtImage(out NGCorr, out int ngNumber);
        //            if (NGCorr)
        //            {
        //                trayRobot.GetTrayData().ErrString += "托盘相连NG数量超出设定值" + trayRobot.CrossNGNumber + "当前" + ngNumber + ";" + Environment.NewLine;
        //            }

        //        }
        //        int errNumber = 0;
        //        if (!trayRobot.GetTrayData().OK && trayRobot.NGPercentumBool)
        //        {
        //            if (trayRobot.GetTrayData().IsTryaSN && trayRobot.GetTrayData().TrayIDQR == "")
        //            {
        //                trayRobot.GetTrayData().ErrString += "托盘SN未识别;" + Environment.NewLine;
        //            }
        //            if (trayRobot.GetTrayData().Percentum != 100 && trayRobot.GetTrayData().PercentumN <= trayRobot.GetTrayData().Percentum)
        //            {
        //                trayRobot.GetTrayData().ErrString += "NG率超出设定值" +
        //                   trayRobot.GetTrayData().Percentum +
        //                 "=>当前" + trayRobot.GetTrayData().PercentumN + ";" + Environment.NewLine;
        //            }
        //        }
        //        if (NGCont.Count != 0)
        //        {
        //            trayRobot.GetTrayData().ErrString += "SN长度不正确，位置:";
        //            for (int i = 0; i < NGCont.Count; i++)
        //            {
        //                trayRobot.GetTrayData().ErrString += NGCont[i] + ",";
        //            }
        //            trayRobot.GetTrayData().ErrString += ";" + Environment.NewLine;
        //        }

        //        //string NGtext = "位置:";
        //        if (trayRobot.NGNumber != 0 && trayRobot.NGNumberBool)
        //        {
        //            if (trayRobot.NG_QR_IDT.Count >= trayRobot.NGNumber)
        //            {
        //                for (int i = 0; i < trayRobot.NG_QR_IDT.Count; i++)
        //                {
        //                    for (int j = 0; j < NG.Count; j++)
        //                    {
        //                        if (trayRobot.NG_QR_IDT[i].NGNubmerID.Contains(NG[j]))
        //                        {
        //                            //NGtext += NG[j] + ",";
        //                            errNumber++;
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //            if (errNumber >= trayRobot.NGNumber)
        //            {
        //              //  AlarmText.AddTextNewLine("托盘" + trayID + ",连续NG数量超出设定值" + errNumber + ">=" + trayRobot.NGNumber, System.Drawing.Color.Red);
        //                trayRobot.GetTrayData().ErrString += "连续NG数量超出设定值" + errNumber + ">=" + trayRobot.NGNumber + ";" + Environment.NewLine;
        //            }
        //        }
        //        if (trayRobot.GetTrayData().ErrString != "")
        //        {
        //         //   switch (ProductEX.GetProductEX().NGRunCode)
        //         //   {
        //         //       //NG后停机
        //         //       //NG后报警等待
        //         //       //NG后提示并继续
        //         //       //NG后不处理
        //         //       case "NG后报警等待":
        //         //               SimulateQRForm.ShowMesabe(trayRobot.GetTrayData().ErrString, false, -1, "托盘报警");
        //         //           break;
        //         //       case "NG后提示并继续":
        //         //           SimulateQRForm.ShowMesabe(trayRobot.GetTrayData().ErrString, true, 10, "托盘报警");
        //         //   break;
        //         //   case "NG后不处理":
        //         //       break;
        //         //   default:
        //         //       SimulateQRForm.ShowMesabe(trayRobot.GetTrayData().ErrString, false, -1, "托盘报警");
        //         //       DebugCompiler.Stop();
        //         //       Thread.Sleep(1000);
        //         //   break;
        //         //}
        //        }
            
        //        return;
        //    }
        //    catch (Exception ex)
        //    {
        //        //AlarmText.AddTextNewLine("托盘" + trayID + "计数失败" + ex.Message);
        //    }
        //    OKNumber.TrayNumber++;
        //}
        /// <summary>
        /// 
        /// </summary>
        private static OKNGNumber[] oKNumberClasses;

        /// <summary>
        /// 记录
        /// </summary>
        /// <param name="trayData"></param>
        public static void Add(TrayData trayData)
        {
            try
            {
                try
                {
                    int det = DateTime.Now.Hour;
                    //if (!SPCDicProduct.ContainsKey(Product.ProductionName))
                    //{
                    //    SPCDicProduct.Add(Product.ProductionName, new OKNumberClass());
                    //}
                    //if (SPCDicProduct[Product.ProductionName].ProductName != Product.ProductionName)
                    //{
                    //    SPCDicProduct[Product.ProductionName].ProductName = Product.ProductionName;
                    //}
                    //if (OKNumber.ProductName != Product.ProductionName)
                    //{
                    //    OKNumber.ProductName = Product.ProductionName;
                    //}
                    //for (int i = 0; i < trayData.Count; i++)
                    //{
                    //    if (trayData.dataVales1[i].NotNull)
                    //    {
                    //        if (trayData.dataVales1[i].AutoOK)
                    //        {
                    //            OKNumber.AutoOKNumber ++;
                    //            SPCDicProduct[Product.ProductionName].AutoOKNumber ++;
                    //            //oKNumberClasses[det].AutoOKNumber++;
                    //        }
                    //         if (trayData.dataVales1[i].OK)
                    //        {
                    //            OKNumber.OKNumber++;
                    //            SPCDicProduct[Product.ProductionName].OKNumber++;
                    //            oKNumberClasses[det].OKNumber++;
                    //        }
                    //        else
                    //        {
                    //            SPCDicProduct[Product.ProductionName].NGNumber++;
                    //            OKNumber.NGNumber++;
                    //            oKNumberClasses[det].NGNumber++;
                    //        }
                    //        foreach (var item in trayData.dataVales1[i].GetAllCompOBJs().DicOnes)
                    //        {
                    //            OKNumber.AddComponentNumber(item.Value.ComponentID, item.Value.BankName, item.Value.aOK,item.Value.AutoOK);
                    //            SPCDicProduct[Product.ProductionName].AddComponentNumber(item.Value.ComponentID, item.Value.BankName, item.Value.aOK, item.Value.AutoOK);
                            
                    //        }
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                }
                OnRefreshDataEvent();
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 日期切换
        /// </summary>
        public static void SwitchoverSPC()
        {
            SPCDicProduct.Clear();
            oKNumberClasses = new OKNGNumber[24];
            for (int i = 0; i < oKNumberClasses.Length; i++)
            {
                oKNumberClasses[i] = new OKNGNumber();
            }
            WriteSpcData();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static OKNGNumber[] GetOKNumberList()
        {

            if (oKNumberClasses == null)
            {
                oKNumberClasses = new OKNGNumber[24];
                for (int i = 0; i < oKNumberClasses.Length; i++)
                {
                    oKNumberClasses[i] = new OKNGNumber();
                }
            }
            return oKNumberClasses;
        }

    }
}
