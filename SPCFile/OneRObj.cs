using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPC_Windows.SPCFile
{
    /// <summary>
    /// 单个缺陷
    /// </summary>
    public class OneRObj
    {
        private HObject Roi;

        public HObject ROI(HObject roi = null)
        {
            if (roi != null)
            {
                Roi = roi.Clone();
            }
            return Roi;
        }

        public HObject NGROI = new HObject();

        public double Row = -999;
        public double Col = -999;
        public double Anlge;

        public double Length1;

        public double Length2;


        public OneRObj()
        {
            NGROI.GenEmptyObj();
            //Roi = new HObject();
            //Roi.GenEmptyObj();
        }

        public OneRObj(OneRObj oneRObj, bool isClone = true)
        {
            //this.ComponentID = oneRObj.ComponentID;
            this.RestText = oneRObj.RestText;
            if (!oneRObj.aOK)
            {
                this.NGText = oneRObj.NGText;
            }
            else
            {
                this.CNGText = "Pass";
            }
            this.RunName = oneRObj.RunName;
            aOK = oneRObj.aOK;
            this.dataMinMax = oneRObj.dataMinMax;
            this.Done = oneRObj.Done;
            if (isClone)
            {
                this.NGROI = oneRObj.NGROI.Clone();
                this.ROI(oneRObj.ROI().Clone());
            }
            else
            {
                this.NGROI = oneRObj.NGROI;
                this.ROI(oneRObj.ROI());
            }
            massageText.Rows = oneRObj.massageText.Rows;
            massageText.Columns = oneRObj.massageText.Columns;
            massageText.Massage = oneRObj.massageText.Massage;
            //messages = oneRObj.messages;
            //rows = oneRObj.rows;
            //cols = oneRObj.cols;
            //OneOdata[] oneOdat= new OneOdata[] { };
            //oneRObj.oneOdatas.CopyTo(oneOdat);
            //oneOdatas.AddRange(oneOdat);
            this.restStrings = oneRObj.RestStrings();
        }
        public void AddImageMessage(HTuple row, HTuple col, HTuple message)
        {
            massageText.AddImageMassage(row, col, message);
        }
        public MassageText massageText = new MassageText();

        /// <summary>
        /// 参数集合
        /// </summary>
        public DataMinMax dataMinMax { get; set; } = null;
        /// <summary>
        /// 是否OK
        /// </summary>
        public bool aOK
        {
            get { return ok; }
            set
            {
                if (value)
                {
                    Done = true;
                }
                ok = value;
            }
        }

        bool ok;
        /// <summary>
        /// 完成复判
        /// </summary>
        public bool Done;
        /// <summary>
        /// 相机面
        /// </summary>
        public string RunName = "";

        /// <summary>
        /// 程序库
        /// </summary>

        public string BankName = "";

        /// <summary>
        /// 元件名称
        /// </summary>
     //   public string ComponentID { get; set; } = "";

        public int Location { get; set; }
        /// <summary>
        /// 缺陷代码
        /// </summary>
        public int RestCode { get; set; }

        /// <summary>
        /// NG缺陷
        /// </summary>
        public string NGText
        {
            get { return ngText; }
            set
            {
                if (CNGText == "" || CNGText == "NOLL")
                {
                    CNGText = value;
                }
                ngText = value;
            }
        }

        private string ngText = "";

        /// <summary>
        /// 机器NG判断
        /// </summary>
        public string CNGText = "NOLL";

        /// <summary>
        /// 复判缺陷
        /// </summary>
        public string RestText { get; set; } = "";

        /// <summary>
        /// 缺陷种类
        /// </summary>
        public string DefectName = "";

        public void AddRestStrings(string errName)
        {
            if (restStrings == null)
            {
                restStrings = new List<string>();
                restStrings.Add("OK");
                restStrings.Add(errName);
            }
        }


        public List<string> RestStrings()
        {
            if (restStrings == null)
            {
                restStrings = new List<string>();
                restStrings.Add("PASS");
                //restStrings.Add(CNGText);
                //if (DefectName == "")
                //{
                //    foreach (var item in Vision.Instance.DicDrawbackNameS)
                //    {
                //        restStrings.AddRange(item.Value.GetBackName().ToSArr());
                //    }
                //}
                //else if ((Vision.Instance.DicDrawbackNameS.ContainsKey(DefectName)))
                //{
                //    restStrings.AddRange(Vision.Instance.DicDrawbackNameS[DefectName].GetBackName().ToSArr());
                //}
            }
            return restStrings;
        }

        /// <summary>
        /// 缺陷下拉选项
        /// </summary>
        List<string> restStrings { get; set; }

        /// <summary>
        /// 修改缺陷为OK
        /// </summary>
        public void RAddOK()
        {

            //RestText = Vision.Instance.RestOKText;
            aOK = true;
            Done = true;
        }
        /// <summary>
        /// 修改NG缺陷项
        /// </summary>
        /// <param name="restText"></param>
        public void RAddNG(string restText)
        {
            RestText = restText;
            if (DefectName != null)
            {
                //if (Vision.Instance.DicDrawbackNameS.ContainsKey(DefectName))
                //{
                //    if (Vision.Instance.DicDrawbackNameS[DefectName].GetDrawBack(restText) != null)
                //    {
                //        RestCode = Vision.Instance.DicDrawbackNameS[DefectName].GetDrawBack(restText).DrawbackIndex;
                //    }
                //    else
                //    {
                //        RestCode = -1;
                //    }
                //}
            }
            Done = true;
            aOK = false;
        }
        public void Dispose()
        {
            try
            {


                if (this.ROI() != null)
                {
                    this.ROI().Dispose();
                }
                if (this.NGROI != null)
                {
                    this.NGROI.Dispose();
                }
                this.dataMinMax = null;
            }
            catch (System.Exception EX)
            {

            }
        }
    }

}
