using NPOI.SS.Formula.Functions;
using SPC_Windows.SPCFile;
using System;
using System.Collections.Generic;
using System.IO;


using static Vision2.SPCFile.SPCForm1;

namespace Vision2.HtmlMaker
{
    public class Html
    {
        private static string filename = "";
        private static StreamWriter writer = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filepth"></param>
        /// <param name="startTimeAndEndTime"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static bool CreationData(string filepth,string startTimeAndEndTime,List<SpcData>  datas)
        {
   
                System.IO.Directory.CreateDirectory(Path.GetDirectoryName(filepth));
                // 文件存在时是否覆盖
                filename = filepth; /*+ ".html";*/

                FileInfo f = new FileInfo(filename);
                if (f.Exists)
                {
                    f.Delete();
                }
                FileStream outputfile = null;
                try
                {
                    outputfile = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
                    writer = new StreamWriter(outputfile);
                    writer.BaseStream.Seek(0, SeekOrigin.End);
                DoWrite("<HTML>");
                //DoWrite("<HEAD>");
                //DoWrite("<HEAD>");
                DoWrite("<P><FONT FACE = \"ARIAL\" SIZE = 6 COLOR =#990000>DefectAnalysis </FONT>  "  );
                DoWrite("<P><FONT FACE = \"ARIAL\" SIZE = 3 COLOR =#990000>DateTime:" + startTimeAndEndTime + "</FONT>");
                //DoWrite("<TABLE BORDER=1>");
                //DoWrite("<TITLE> Test Results For Serial Number:" + dataVale.PanelID + " Executed At 4:10:48 PM</TITLE>");
                //DoWrite("</HEAD>");
                DoWrite("<TABLE BORDER=1 style=\"word -break:break-all\"><TR> <TH>序号</TH><TH>型号</TH><TH>日期 時間</TH><TH>条码</TH><TH>正反面</TH><TH>连板位号</TH><TH>元件</TH><TH>元件类型</TH><TH>检测结果</TH><TH>复判结果</TH><TH>更新时间</TH><TH>复判人員</TH><TH>批號</TH><TH>Value</TH><TH>群组</TH><TH>AI数值</TH><TH>元件影像</TH><TH>3D Image</TH></TR>");

                //<P>< FONT FACE = "ARIAL" SIZE = 6 COLOR =#990000>DefectAnalysis </FONT>  <P><FONT FACE="ARIAL" SIZE=3 COLOR=#990000>DateTime:2024/03/27 08:00:00~2024/03/27 20:00:00</FONT>
                // <TABLE BORDER=1 style="word-break:break-all"><TR> <TH>型號</TH><TH>日期 時間</TH><TH>條碼</TH><TH>正反面</TH><TH>聯板編號</TH><TH>元件</TH><TH>元件類型</TH><TH>檢測結果</TH><TH>覆判結果</TH><TH>更新時間</TH><TH>覆判人員</TH><TH>批號</TH><TH>Value</TH><TH>群組</TH><TH>AI數值</TH><TH>元件影像</TH><TH>3D Image</TH></TR>
                // <TR><TH>639-20248-6-K</TH> <TD Align=Center>2024/03/27 08:00:31</TD>  <TD Align=Center>2024/03/27 08:00:31J25@011</TD>  <TD Align=Center>T</TD>  <TD Align=Center>4</TD>  <TD Align=Center>U2100_4</TD>  <TD Align=Center>353S01120</TD>  <TD Align=Center>缺件</TD> <TD Align=Center>RPASS</TD> <TD Align=Center>2024/03/27 08:02:26</TD>  <TD Align=Center>16071004</TD> <TD Align=Center>0</TD> <TD Align=Center>Score(49/50)pt. </TD> <TD Align=Center>U</TD> <TD Align=Center></TD> <TD Align=Center><IMG SRC="IMG\2024032708\20240327080031_U2100_4.jpg" Width=300 onMouseOver="this.width=this.width*3" onMouseOut="this.width=this.width/3" ></TD> <TH>  </TH> </TR> 
                //< TR >< TH > 639 - 20248 - 6 - K </ TH > < TD Align = Center > 2024 / 03 / 27 08:03:29 </ TD >  < TD Align = Center > 2024 / 03 / 27 08:03:29J25@011 </ TD >  < TD Align = Center > T </ TD >  < TD Align = Center > 2 </ TD >  < TD Align = Center > R0908_2 </ TD >  < TD Align = Center > 116S00022 - GG0 </ TD >  < TD Align = Center > 缺件 </ TD > < TD Align = Center > RPASS </ TD > < TD Align = Center > 2024 / 03 / 27 08:04:29 </ TD >  < TD Align = Center > 16071004 </ TD > < TD Align = Center > 0 </ TD > < TD Align = Center > ShiftY(83 / 80)um @F</ TD > < TD Align = Center > R </ TD > < TD Align = Center ></ TD > < TD Align = Center >< IMG SRC = "IMG\2024032708\20240327080329_R0908_2.jpg" Width = 300 onMouseOver = "this.width=this.width*3" onMouseOut = "this.width=this.width/3" ></ TD > < TH >  </ TH > </ TR >
                //< TR >< TH > 639 - 20248 - 6 - K </ TH > < TD Align = Center > 2024 / 03 / 27 08:04:49 </ TD >  < TD Align = Center > 639 - 20248 - 6 - KT43S0484 </ TD >  < TD Align = Center > T </ TD >  < TD Align = Center > 1 </ TD >  < TD Align = Center > U2100_1 </ TD >  < TD Align = Center > 353S01120 </ TD >  < TD Align = Center > 缺件 </ TD > < TD Align = Center > RPASS </ TD > < TD Align = Center > 2024 / 03 / 27 08:05:49 </ TD >  < TD Align = Center > 16071004 </ TD > < TD Align = Center > 0 </ TD > < TD Align = Center > Score(47 / 50)pt. </ TD > < TD Align = Center > U </ TD > < TD Align = Center ></ TD > < TD Align = Center >< IMG SRC = "IMG\2024032708\20240327080449_U2100_1.jpg" Width = 300 onMouseOver = "this.width=this.width*3" onMouseOut = "this.width=this.width/3" ></ TD > < TH >  </ TH > </ TR >
                int id = 0;
                foreach (var item in datas)
                 {
                    id++;
                    //< TR >< TH > 639 - 20248 - 6 - K </ TH >型号
                    //< TD Align = Center > 2024 / 03 / 27 08:03:29 </ TD >日期时间
                    //< TD Align = Center > 2024 / 03 / 27 08:03:29J25@011 </ TD >条码
                    //< TD Align = Center > T </ TD >正反面
                    //< TD Align = Center > 2 </ TD >位置
                    //< TD Align = Center > R0908_2 </ TD >元件
                    //< TD Align = Center > 116S00022 - GG0 </ TD >库
                    //< TD Align = Center > 缺件 </ TD >检测结果
                    //< TD Align = Center > RPASS </ TD >复判结果
                    //< TD Align = Center > 2024 / 03 / 27 08:04:29 </ TD >更新时间
                    //< TD Align = Center > 16071004 </ TD >复判人员
                    //< TD Align = Center > 0 </ TD >批号
                    //< TD Align = Center > ShiftY(83 / 80)um @F</ TD >值
                    //< TD Align = Center > R </ TD >群组
                    //< TD Align = Center ></ TD >AI值
                    //< TD Align = Center >< IMG SRC = "IMG\2024032708\20240327080329_R0908_2.jpg" Width = 300 onMouseOver = "this.width=this.width*3" onMouseOut = "this.width=this.width/3" ></ TD >
                    //< TH >  </ TH > </ TR >3D图片
                    //序号
                    DoWrite("<TR><TH> " + id + " </TH><TD> " +
                        item.ProductName + " </TD><TD Align = Center >" +
                        item.Start_DataTime + " </TD ><TD Align = Center >" +
                        item.SN + " </TD ><TD Align = Center >" +
                        item.CamName + " </TD > <TD Align = Center >" +
                        item.Location + " </TD > <TD Align = Center >" +
                        item.Name + " </TD > <TD Align = Center >" +
                        item.Material_no + " </TD > <TD Align = Center >" +
                        item.AutoOk + " </TD > <TD Align = Center >" +
                        item.Result + " </TD > <TD Align = Center >" +
                        item.Start_DataTime + " </TD > <TD Align = Center >" +
                        item.UserName + " </TD > <TD Align = Center >" +
                        item.OK + " </TD > <TD Align = Center >" +
                        item.Placement + "</TD ><TD Align = Center > R </TD ><TD Align = Center >" +
                        item.Placement + "</TD ><TD Align = Center ><IMG SRC = \"" +
                        item.ImagePath + "\" Width = 300 onMouseOver = \"this.width=this.width*3\" onMouseOut = this.width=\"this.width/3\" ></TD>" +
                        " <TH></TH> </TR> ");
                    //DoWrite("< TD > "+item.ProductName+" </ TD > ");//型号
                    //DoWrite("< TD Align = Center >"+item.Start_DataTime+" </ TD > ");//时间日期
                    //DoWrite("< TD Align = Center >" + item.SN + " </ TD > ");//SN
                    //DoWrite("< TD Align = Center >" + item.CamName + " </ TD > ");//正反面
                    //DoWrite("< TD Align = Center >" + item.Location + " </ TD > ");//位置
                    //DoWrite("< TD Align = Center >" + item.Name + " </ TD > ");//元件
                    //DoWrite("< TD Align = Center >" + item.Material_no + " </ TD > ");//库
                    //DoWrite("< TD Align = Center >" + item.AutoOk + " </ TD > ");//检测结果
                    //DoWrite("< TD Align = Center >" + item.Result + " </ TD > ");//复判结果
                    //DoWrite("< TD Align = Center >" + item.Start_DataTime + " </ TD > ");//更新时间
                    //DoWrite("< TD Align = Center >" + item.UserName + " </ TD > ");//复判人员
                    //DoWrite("< TD Align = Center >" + item.OK + " </ TD > ");//批号
                    //DoWrite("< TD Align = Center >na</ TD > ");//AI值
                    //DoWrite("< TD Align = Center >U</ TD > ");//群组
                    //DoWrite("< TD Align = Center > na</ TD > ");//值
                    //DoWrite("< TD Align = Center >  IMG SRC = \"" + "" + "\" Width = 300 onMouseOver = \"this.width=this.width*3\" onMouseOut = \"this.width=this.width/3\" </ TD > ");//元件图像
                    //DoWrite("< TD Align = Center > </ TD > ");//3D图像
                    //DoWrite("</TR>");
                }
                DoWrite("</TR></TABLE></TD>");
                  //DoWrite("<HEAD>");

                  writer.Close();
                    return true;

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception GenerateCode = " + ex);
                    //stbMsg.Text = "Error";
                    outputfile = null;
                    writer = null;
                }
                return false;
  

        }


        private static void DoWrite(String line)
        {
            writer.WriteLine(line);
            writer.Flush();
        }
    }
}