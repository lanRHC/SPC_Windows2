using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vision2.vision;

namespace SPC_Windows.SPCFile
{
    public class MassageText
    {
        public List<HTuple> Rows = new List<HTuple>();

        public List<HTuple> Columns = new List<HTuple>();

        public List<HTuple> Massage = new List<HTuple>();

        public string MassageBlute = "false";

        public ColorResult GetColorResult()
        {
            return ColorResult.red;
        }
        public string color = "red";

        public void ShowMassage(HTuple hWindowHalconID)
        {
            if (Massage.Count >= 1 && Massage.Count == Rows.Count && Columns.Count == Rows.Count)
            {
                for (int i = 0; i < Massage.Count; i++)
                {
                    try
                    {
                        HWindID.Disp_message(hWindowHalconID, Massage[i], Rows[i], this.Columns[i], false, color, MassageBlute);
                    }
                    catch (System.Exception)
                    { }
                }
            }
        }

        public void AddImageMassage(HTuple rows, HTuple columns, HTuple Massage)
        {
            if (columns.Length > Massage.Length)
            {
                Massage = HTuple.TupleGenConst(columns.Length, Massage);
            }

            if (rows.Length == columns.Length)
            {
                this.color = color = "red";
                this.Rows.Add(rows);
                this.Columns.Add(columns);
                this.Massage.Add(Massage);
            }
        }
    }

}
