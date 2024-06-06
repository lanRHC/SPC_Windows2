using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace Vision2.SPCFile
{
    public class FilesHelper
    {
        public string inipath;

        //声明API函数

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        /// <summary> 
        /// 构造方法 
        /// </summary> 
        /// <param name="INIPath">文件路径</param> 
        public FilesHelper(string INIPath)
        {
            string di = INIPath.Remove(INIPath.LastIndexOf("\\") + 1);
            if (!Directory.Exists(di))
            {
                Directory.CreateDirectory(di);
            }
            //if (!File.Exists(INIPath))

            //{
            //    File.Create(INIPath);
            //}
            inipath = INIPath;
        }

        public FilesHelper() { }

        /// <summary> 
        /// 写入INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        /// <param name="Value">值</param> 
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.inipath);
        }
        /// <summary> 
        /// 读出INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, this.inipath);
            return temp.ToString();
        }
        /// <summary> 
        /// 验证文件是否存在 
        /// </summary> 
        /// <returns>布尔值</returns> 
        public bool ExistINIFile()
        {
            return File.Exists(inipath);
        }
        /// <summary>
        /// 写入CSV文件夹
        /// </summary>
        /// <param name="path">写入路径</param>
        /// <param name="fileName">文件夹名称</param>
        /// <param name="str">写入内容</param>
        public void WRCSVFile(string path, string fileName, string str)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);//判断路径文件夹是否存在
            }
            path = path + @"\" + fileName + ".csv";

            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
            StreamWriter sw = new StreamWriter(path, true, Encoding.UTF8);

            sw.Write(str);
            sw.Flush();
            sw.Close();
        }

        public void RD_CSV(string path, out string[] RDstr, out List<string> li_str)
        {
            string strLine;
            StreamReader sr = new StreamReader(path, Encoding.Default);
            string[] ary = { };
            li_str = new List<string>();
            RDstr = new string[1000];
            while ((strLine = sr.ReadLine()) != null)
            {
                ary = strLine.Split(new char[] { ',' });
                int i = 0;

                foreach (string str in ary)
                {

                    RDstr[i] = str;
                    i++;
                    li_str.Add(str);
                }
            }

            //RDstr = ary;
            sr.Close();
        }
    }
}
