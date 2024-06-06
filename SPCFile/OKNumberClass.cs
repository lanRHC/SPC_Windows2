using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Vision2.SPCFile
{
    /// <summary>
    /// 以产品计数的SPC
    /// </summary>
    public class OKNumberClass
    {
        public string GetSPC()
        {
            return "总数:" + Number + "良率%:" + OKNG.ToString("0.000") + "\n\rOK:" + OKNumber + "  NG:" + NGNumber + 
                "  NOK:" + AutoNGNumber + "  Tray:" + TrayNumber;
        }

        public string GetTextSPC()
        {
            string text = "";
            try
            {
                text += "名称:" + this.ProductName + Environment.NewLine;
                text += "OK:" + this.OKNumber + " NG:" + this.NGNumber + Environment.NewLine;
                string autdo=         Math.Round(((double)AutoOKNumber / (double)Number) * 100.0F, 2).ToString();
                text += "直通率:" + autdo +"% 数"+AutoOKNumber+ Environment.NewLine;
                text += "良率%:" + this.OKNG.ToString("0.000") + Environment.NewLine;
                text += "元件数:" + this.ComponentCont ;
                var distinctValues = this.ComponentS.Select(p => p.Value.BankNmae).Distinct().ToList(); distinctValues.Remove("");
                text += " 库总数:" + distinctValues.Count + Environment.NewLine;
                int ok = 0;
                int ng = 0;
                int falseCill = 0;
                foreach (var item in this.ComponentS)
                {
                    ok += item.Value.OKNumber;
                    ng += item.Value.NGNumber;
                    falseCill+= item.Value.FalseCall;
                }
                text += "元件OK:" + ok  ;
                text += " NG:" + ng + Environment.NewLine;
                text += "元件误判:" + falseCill + Environment.NewLine;
                Single cdetcont = (Single)Math.Round(((double)ok / (double)(ok + ng)) * 100.0F, 2);
                text += "元件良率:" + cdetcont.ToString("0.000") + Environment.NewLine;
                var list = from n in this.ComponentS
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
        [Description(""), Category("产品计数"), DisplayName("产品名称")]
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 添加元件错误SPC统计
        /// </summary>
        /// <param name="name">元件名称</param>
        /// <param name="bankNmae">库名称</param>
        /// <param name="OK">结果</param>
        /// <param name="autoOk">机器结果</param>
        public void AddComponentNumber(string name, string bankNmae, bool OK,bool autoOk)
        {
            if (!ComponentS.ContainsKey(name))
            {
                ComponentS.Add(name, new ComponentNumber());
            }
            ComponentS[name].Nmae = name;
            ComponentS[name].BankNmae = bankNmae;
        
            if (OK)
            {
                ComponentS[name].OKNumber++;
                if (!autoOk)
                {
                    ComponentS[name].FalseCall ++;
                    //autoPointNGNumber ++;
                }
            }
            else
            {
                ComponentS[name].NGNumber++;
            }
        }
        [Description(""), Category("库"), DisplayName("库总数量")]
        /// <summary>
        /// 库总数
        /// </summary>
        public int ComponentCont { get { return ComponentS.Count; } }

        [Description(""), Category("库"), DisplayName("库计数")]
        /// <summary>
        /// 元件计数
        /// </summary>
        public Dictionary<string, ComponentNumber> ComponentS { get; set; } = new Dictionary<string, ComponentNumber>();

        public void AddTrayNumber(bool ok)
        {
            TrayNumber++;
            if (ok)
            {
                TrayOKNumber++;
            }
            //RecipeCompiler.GetSPC();
        }
        [Description(""), Category("产品计数"), DisplayName("托盘数量")]
        /// <summary>
        /// 托盘数量
        /// </summary>
        public int TrayNumber { get; set; }
        [Description(""), Category("产品计数"), DisplayName("托盘OK数量")]

        /// <summary>
        /// 托盘数量
        /// </summary>
        public int TrayOKNumber { get; set; }
        [Description(""), Category("产品计数"), DisplayName("")]
        /// <summary>
        /// 是否OK
        /// </summary>
        public bool IsOK { get; set; }
        [Description(""), Category("产品计数"), DisplayName("OK数量")]
        /// <summary>
        /// OK数量
        /// </summary>
        public int OKNumber { get; set; }
        [Description(""), Category("产品计数"), DisplayName("NG数量")]
        /// <summary>
        /// NG数量
        /// </summary>
        public int NGNumber { get; set; }

        [Description(""), Category("产品计数"), DisplayName("总数")]
        /// <summary>
        /// 总数数量
        /// </summary>
        public int Number { get { return OKNumber + NGNumber; } }
        [Description(""), Category("产品计数"), DisplayName("良率")]
        /// <summary>
        /// 良率
        /// </summary>
        public Single OKNG
        {
            get { return (Single)Math.Round(((double)OKNumber / (double)Number) * 100.0F, 2); }
        }
        [Description(""), Category("产品计数"), DisplayName("机器误判数量")]
        /// <summary>
        /// 机器误判数量
        /// </summary>
        public int AutoNGNumber { get; set; }

        [Description(""), Category("产品计数"), DisplayName("机判OK数量")]
        /// <summary>
        /// 机器OK数量
        /// </summary>
        public int AutoOKNumber { get; set; }


        [Description("CRDNG位置数量"), Category("库计数"), DisplayName("NG点数量")]

        /// <summary>
        /// NG点数量
        /// </summary>
        public int PointNGNumber { get; set; }

        [Description(""), Category("库计数"), DisplayName("NG误判点数量")]
        /// <summary>
        /// NG误判点数量
        /// </summary>
        public int AutoPointNGNumber { get; set; }


    }
    /// <summary>
    /// 每时数据
    /// </summary>
    public class OKNGNumber
    {
        [Description(""), Category("产品计数"), DisplayName("OK数量")]
        /// <summary>
        /// OK数量
        /// </summary>
        public int OKNumber { get; set; }
        [Description(""), Category("产品计数"), DisplayName("NG数量")]
        /// <summary>
        /// NG数量
        /// </summary>
        public int NGNumber { get; set; }
        [Description(""), Category("产品计数"), DisplayName("总数")]
        /// <summary>
        /// 总数数量
        /// </summary>
        public int Number { get { return OKNumber + NGNumber; } }
        [Description(""), Category("产品计数"), DisplayName("良率")]
        /// <summary>
        /// 良率
        /// </summary>
        public Single OKNG
        {
            get { return (Single)Math.Round(((double)OKNumber / (double)Number) * 100.0F, 2); }
        }
        [Description(""), Category("产品计数"), DisplayName("机器误判数量")]

        /// <summary>
        /// 机器误判数量
        /// </summary>
        public int AutoNGNumber { get; set; }


    }

}
