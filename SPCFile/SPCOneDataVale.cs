using SPC_Windows.SPCFile;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace Vision2.SPCFile
{
     public  class SPCOneDataVale
    {
        OneDataVale oneDataVale;
        public OneDataVale GetOneDataVale()
        {
          return  oneDataVale;
        }
        public string SNTime { get; set; }
        public string SN { get; set; }
        public string ProductName { get; set; }

        public int Location { get; set; }
        public string TestResult { get; set; }

        public string AutoResult { get; set; }
        public string StratTime { get; set; }

        public string EndTime { get; set; }

        public double Cyclicality { get; set; }

        public string CarrierID { get; set; }

        public string CoverID { get; set; }
        public int NGNumber { get; set; }

        public string ImagePaht { get; set; }

        public string DeviceName { get; set; }
        public string CRDTest { get; set; }
        public string NGCRD { get; set; }
        public OneDataVale oneDataValeCRD { get; set; }
        public static SPCOneDataVale ConvertToModel( DataRow dr)
        {
            SPCOneDataVale SPCOneDataVale = new SPCOneDataVale();
             PropertyInfo[] pi = SPCOneDataVale.GetType().GetProperties();//取类的属性
                                                            //属性赋值
            foreach (PropertyInfo p in pi)
            {
                if (dr.Table.Columns.Contains(p.Name) && !string.IsNullOrWhiteSpace(dr[p.Name].ToString()))
                {
                    p.SetValue(SPCOneDataVale, Convert.ChangeType(dr[p.Name], p.PropertyType), null);
                }
            }
            //ProjectINI.StringJsonToCalss<OneDataVale>(SPCOneDataVale.CRDTest, out SPCOneDataVale.oneDataVale);
            //SPCOneDataVale.CRDTest = null;
            return SPCOneDataVale; //Return
        }

        //MySqlParameter[] parameters = {
        //            new MySqlParameter("@SNTime", MySqlDbType.Text,100),
        //            new MySqlParameter("@SN", MySqlDbType.Text),
        //            new MySqlParameter("@ProductName", MySqlDbType.Text),
        //            new MySqlParameter("@Location", MySqlDbType.Int16),
        //            new MySqlParameter("@TestResult", MySqlDbType.VarChar),
        //            new MySqlParameter("@AutoResult", MySqlDbType.Bit),
        //            new MySqlParameter("@StratTime", MySqlDbType.DateTime),
        //            new MySqlParameter("@EndTime", MySqlDbType.DateTime),
        //            new MySqlParameter("@Cyclicality", MySqlDbType.Double),
        //            new MySqlParameter("@CarrierID", MySqlDbType.Text),
        //            new MySqlParameter("@CoverID", MySqlDbType.Text),
        //            new MySqlParameter("@NGNumber", MySqlDbType.Int32),
        //            new MySqlParameter("@CRDTest", MySqlDbType.Text),
        //            new MySqlParameter("@ImagePaht", MySqlDbType.Text),
        //            new MySqlParameter("@DeviceName", MySqlDbType.Text),
        //        };
    }
}
