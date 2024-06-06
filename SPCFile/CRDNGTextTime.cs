using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPC_Windows.SPCFile
{


    /// <summary>
    /// 时间段内连续报警 元件和库记录信息
    /// </summary>
    public class CRDNGTextTime
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// NG时间
        /// </summary>
        public List<DateTime> NGTime { get; set; } = new List<DateTime>();
        /// <summary>
        /// NG信息
        /// </summary>
        public List<string> NGText { get; set; } = new List<string>();
        /// <summary>
        /// 位置信息
        /// </summary>
        public List<string> CrdName { get; set; }
        /// <summary>
        /// NG次数
        /// </summary>

        public int NGCont { get { return NGText.Count; } }

        /// <summary>
        /// 超时剔除
        /// </summary>
        /// <param name="dateTimeHH"></param>
        public void RemoveTime(double dateTimeHH)
        {
            List<int> ints = new List<int>();

            for (int i = 0; i < NGTime.Count; i++)
            {
                if (DateTime.Compare(NGTime[i].AddHours(dateTimeHH), DateTime.Now) < 0)
                {
                    ints.Add(i);
                }
            }
            ints.Sort();
            for (int i = 0; i < ints.Count; i++)
            {
                NGTime.RemoveAt(0);
                NGText.RemoveAt(0);
            }
        }
    }

}
