using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision2.SPCFile
{
    /// <summary>
    /// 元件计数
    /// </summary>
    public class ComponentNumber
    {
        /// <summary>
        /// 元件名称
        /// </summary>
        public string Nmae { get; set; }
        /// <summary>
        /// 库名称
        /// </summary>
        public string BankNmae { get; set; }
        /// <summary>
        /// OK数量
        /// </summary>
        public int OKNumber { get; set; }
        /// <summary>
        /// 误判数量
        /// </summary>
        public int FalseCall { get; set; }
        /// <summary>
        /// NG数量
        /// </summary>
        public int NGNumber { get; set; }

       ///// <summary>
       ///// 
       ///// </summary>
        public string Yield { get { return Math.Round(((double)OKNumber / (double)Cont) * 100.0F, 2).ToString("0.00"); } }

        /// <summary>
        /// 总数量
        /// </summary>
        public int Cont { get { return OKNumber + NGNumber; } }

    }

    /// <summary>
    /// 元件计数
    /// </summary>
    public class BankNumber
    {
        /// <summary>
        /// 元件名称
        /// </summary>
        public string Nmae { get; set; }

        /// <summary>
        /// OK数量
        /// </summary>
        public int OKNumber { get; set; }
        /// <summary>
        /// NG数量
        /// </summary>
        public int NGNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int FalseCall { get; set; }

        public double Yield { get { return Math.Round(((double)OKNumber / (double)Cont) * 100.0F, 2); } }

        /// <summary>
        /// 数量
        /// </summary>
        public int Cont { get { return OKNumber + NGNumber; } }

    }
}
