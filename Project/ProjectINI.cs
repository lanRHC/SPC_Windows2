﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision2.UI.PropertyGrid;

namespace SPC_Windows.Project
{
    /// <summary>
    /// 解决方案框架
    /// </summary>
    public class ProjectINI
    {
        /// <summary>
        ///静态唯一对象
        /// </summary>
        public static ProjectINI In
        {
            get
            {
                if (iNI == null)
                {
                    iNI = new ProjectINI();
                    try
                    {
                        if (!Directory.Exists(ProjectINI.ProjietPath))
                        {
                            ProjectINI.ProjietPath = "D:\\Vision2\\";
                            if (!Directory.Exists(ProjectINI.ProjietPath))
                            {
                                MessageBox.Show("缺少项目文件");
                            }
                        }
                        if (File.Exists(iNI.ConstPathStr))
                        {
                            ProjectINI.ReadPathJsonToCalss(iNI.ConstPathStr, out iNI);
                            iNI.UserName = iNI.UserDepartment = "";
                            iNI.UserRight = new List<string>();
                            ReadTempString("RunP", "MainForm", out string staver);
                            if (staver != "")
                            {
                                iNI.MaxiForm = Convert.ToBoolean(staver);

                            }

                            string filename = Path.GetDirectoryName(In.ConstPathStr) + "\\LanguageRes.txt";
                            LanguageResources = CsvParsingHelper.CsvToDataTable(filename, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("读取ProjectInI失败：" + ex.Message);
                    }
                }
                return iNI;
            }
            set { iNI = value; }
        }
        private DateTime DateTimet;

        /// <summary>
        /// 循环委托
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public delegate void FromCyc();
        /// <summary>
        /// 循环事件
        /// </summary>
        public event FromCyc CycleEven;

 



        /// <summary>
        /// 循环事件
        /// </summary>
        private void OnCyleEven()
        {
            try
            {
                CycleEven?.Invoke();
            }
            catch (Exception)
            {
            }
        }
 

 


        private void UPCy()
        {
            Thread thread = new Thread(() =>
            {
                bool isv = false;
                bool ISa = false;
                bool ISdT = false;
                bool iste = false;
                bool ISdaMen = false;
                bool StopE = false;
                string data = "";
                Thread.Sleep(6000);
                //ToolStripLabel toolStripLabel11 = new ToolStripLabel();
                //toolStrip1.Items.Add(toolStripLabel11);
                while (true)
                {
                    try
                    {
                        OnCyleEven();
     
              
                        DateTimet = DateTime.Now;
                    }
                    catch (Exception ex)
                    {
                    }
                    Thread.Sleep(10);
                }
            });
            thread.Priority = ThreadPriority.Highest;
            thread.IsBackground = true;
            thread.Start();
        }




        [DllImport("kernel32")]
        public static extern void GlobalMemoryStatus(ref MEMORY_INFO meminfo);

        //定义内存的信息结构
        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_INFO
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public uint dwTotalPhys;
            public uint dwAvailPhys;
            public uint dwTotalPageFile;
            public uint dwAvailPageFile;
            public uint dwTotalVirtual;
            public uint dwAvailVirtual;
        }

        public static string GetMemoryall()
        {
            MEMORY_INFO MemInfo;
            MemInfo = new MEMORY_INFO();
            GlobalMemoryStatus(ref MemInfo);
            string text = "";
            text = MemInfo.dwMemoryLoad.ToString() + "%的内存正在使用" + Environment.NewLine;
            text += "物理内存共有" + GetMB(MemInfo.dwTotalPhys).ToString() + "MB" + Environment.NewLine;
            text += "可使用的物理内存有" + GetMB(MemInfo.dwAvailPhys).ToString() + "MB" + Environment.NewLine;
            text += "交换文件总大小为" + GetMB(MemInfo.dwTotalPageFile).ToString() + "MB" + Environment.NewLine;
            text += "尚可交换文件大小为" + GetMB(MemInfo.dwAvailPageFile).ToString() + "MB" + Environment.NewLine;
            text += "总虚拟内存有" + GetMB(MemInfo.dwTotalVirtual).ToString() + "MB" + Environment.NewLine;
            text += "未用虚拟内存有" + GetMB(MemInfo.dwAvailVirtual).ToString() + "MB" + Environment.NewLine;
            return text;
        }

        public static double GetMB(uint valut)
        {
            for (int i = 0; i < 2; i++)
            {
                valut /= 1024;
            }
            return valut;
        }

        /// <summary>
        /// 获取内存
        /// </summary>
        /// <returns></returns>
        public static uint GetMemoryDW()
        {
            MEMORY_INFO MemInfo;
            MemInfo = new MEMORY_INFO();
            GlobalMemoryStatus(ref MemInfo);
            return MemInfo.dwMemoryLoad;
        }

        /// <summary>
        /// 获取当前使用的内存
        /// </summary>
        /// <returns></returns>
        public static string GetMemory()
        {
            double b = 0;
            try
            {
                Process proc = Process.GetCurrentProcess();
                b = proc.PrivateMemorySize64;
                for (int i = 0; i < 2; i++)
                {
                    b /= 1024.0;
                }
                b /= 1024.0;
            }
            catch (Exception)
            {
            }
            //proc.m
            return b.ToString("0") + "G";
        }

        public static bool DebugMode { get; set; }

        public static void SetFormMax(Form frm)
        {
            //Screen.AllScreens.Initialize();//获取当前系统连接的屏幕数量：
            //string CurrentScreenName = Screen.FromControl(frm).DeviceName;//            获取当前屏幕的名称：
            Screen CurrentScreen = Screen.FromControl(frm);//            获取当前屏幕对象：
            frm.Location = new System.Drawing.Point(CurrentScreen.Bounds.X, CurrentScreen.Bounds.Y);
            //frm.Left
            //frm.Top = 0;
            //frm.Left = 0;
            frm.Width = Screen.PrimaryScreen.WorkingArea.Width;
            frm.Height = Screen.PrimaryScreen.WorkingArea.Height;
        }
        public virtual void SetFormMax2(Form frm)
        {
            frm.Top = 0;
            frm.Left = 0;
            frm.Width = Screen.PrimaryScreen.WorkingArea.Width;
            frm.Height = Screen.PrimaryScreen.WorkingArea.Height;
        }


        private static ProjectINI iNI;
        /// <summary>
        /// 将一个实体类复制到另一个实体类
        /// </summary>
        /// <param name="objectsrc">源实体类</param>
        /// <param name="objectdest">复制到的实体类</param>
        public static void CopyEntityToEntity(object objectsrc, object objectdest)
        {
            var sourceType = objectsrc.GetType();
            var destType = objectdest.GetType();

            foreach (var source in sourceType.GetProperties())
            {
                if (!source.GetType().IsValueType)
                {
                    foreach (var dest in destType.GetProperties())
                    {
                        if (dest.Name == source.Name && dest.CanWrite)
                        {
                            try
                            {
                                dest.SetValue(objectdest, source.GetValue(objectsrc));
                            }
                            catch (Exception ex)
                            { }
                            break;
                        }
                    }
                }
                else
                {
                    foreach (var dest in destType.GetProperties())
                    {
                        if (dest.Name == source.Name)
                        {
                            CopyEntityToEntity(source, dest);
                            break;
                            //dest.SetValue(objectdest, source.GetValue(objectsrc));
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 拷贝整个文件夹到目标文件夹
        /// </summary>
        /// <param name="path"></param>
        /// <param name="opaht"></param>
        public static void CopyDirectory(string path, string opaht)
        {
            try
            {
                string moveDirectory = Path.GetFileNameWithoutExtension(opaht);
                if (System.IO.Directory.Exists(path))
                {
                    string[] dat = System.IO.Directory.GetDirectories(path);
                    //string directName = Path.GetFileName(path);
                    //string Opath = opaht + "\\" + directName;
                    System.IO.Directory.CreateDirectory(opaht);

                    string[] files = System.IO.Directory.GetFiles(path);
                    for (int i = 0; i < files.Length; i++)
                    {
                        System.IO.File.Copy(files[i], opaht + "\\" + Path.GetFileName(files[i]), true);
                    }
                    string[] directorys = System.IO.Directory.GetDirectories(path);
                    for (int i = 0; i < directorys.Length; i++)
                    {
                        CopyDirectory(directorys[i], opaht + "\\" + Path.GetFileNameWithoutExtension(directorys[i]));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public string GetText(string path, string key, string name)
        {
            try
            {
                if (File.Exists(path))
                {
                    string[] da = File.ReadAllLines(path);
                    if (da.Contains("[" + key + "]"))
                    {
                    }
                }
            }
            catch (Exception)
            {
            }
            return "";
        }

        public static string TempPath
        {
            get
            {
                Directory.CreateDirectory(ProjietPath + "Temp\\");
                return ProjietPath + "Temp\\";
            }
        }


        /// <summary>
        /// 链接委托
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        ///
        public delegate bool ModeEven(string kay, bool selpMode);

        public event ModeEven ModeEvenT;

        //public static Form MainFrom;

        public string Language { get; set; } = "US-En";

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <param name="selpMode"></param>
        /// <returns></returns>
        public bool OnModeEvenT(string key, bool selpMode)
        {
            if (key == "All")
            {
                SelpMode = selpMode;
            }

            ModeEvenT?.Invoke(key, SelpMode);

            return SelpMode;
        }

        public static bool Enbt
        {
            get
            {
                if (!In.UserRight.Contains("管理权限"))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public static bool AdminEnbt
        {
            get
            {
                if (!iNI.UserRight.Contains("工程师"))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// 权限限制
        /// </summary>
        /// <param name="name">权限级别</param>
        /// <returns>是否限制</returns>
        public static bool GetUserJurisdiction(string name)
        {
            if (In.UserRight.Contains(name))
            {
                return true;
            }

            return false;
        }

        private static Form form;
        /// <summary>
        /// 获取主窗口
        /// </summary>
        /// <param name="formT"></param>
        /// <returns></returns>
        public static Form Form(Form formT = null)
        {
            if (formT != null)
            {
                form = formT;
            }
            return form;
        }

        /// <summary>
        /// 单步模式
        /// </summary>
        public static bool SelpMode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public static bool CloseBOOL;

        public UserData UsData { get; set; } = new UserData();

        /// <summary>
        ///
        /// </summary>
        public class UserData
        {
            [Description("是否必须登录运行"), Category("权限管理"), DisplayName("必须登录运行")]
            public bool IsMet { get; set; }

            [Description("是否打开软件开始登录"), Category("权限管理"), DisplayName("启动立即登录")]
            public bool Boot_The_Login { get; set; }
        }

        public ProjectINI()
        {
            Dic_Project_Path.Add("AppRun1", "\\AppRun1");
            this.RunName = "AppRun1";
        }

        /// <summary>
        /// 项目读取地址
        /// </summary>
        public static string ProjietPath = "D:\\Vision2\\";


        [DescriptionAttribute("解码方式"), CategoryAttribute("发送机制"), DisplayName("解码方式"),
        TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("GetEncodingNames")]
        public string EncodingString { get; set; } = Encoding.UTF8.HeaderName;



        private string encodingStr = "";
        /// <summary>
        /// 解码方式
        /// </summary>
        public Encoding GetEncoding()
        {
            try
            {
                if (encodingStr != EncodingString)
                {
                    encodingStr = EncodingString;
                    encoding = Encoding.GetEncoding(EncodingString);
                }

            }
            catch (Exception)
            {
            }
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            return encoding;
        }

        protected Encoding encoding;
        [Browsable(false)]
        public List<string> GetEncodingNames
        {
            get
            {
                return new List<string> { Encoding.Default.HeaderName, Encoding.UTF8.HeaderName ,
                    Encoding.ASCII.HeaderName , Encoding.Unicode.HeaderName ,Encoding.UTF32.HeaderName,Encoding.UTF7.HeaderName};
            }
        }


        public List<string> DicNameRunFacility { get; set; } = new List<string>();

        ///// <summary>
        /////
        ///// </summary>
        //private string ConstPathStr = Application.StartupPath + "\\ProjectInI\\ProjectName.Project";
        /// <summary>
        ///
        /// </summary>
        private string ConstPathStr = ProjietPath + "ProjectInI\\ProjectName.Project";

        /// <summary>
        /// 解决方案集合
        /// </summary>
        [Description("解决方案集合"), Category("方案"), DisplayName("解决方案")]
        public Dictionary<string, string> Dic_Project_Path { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 解决方案名称
        /// </summary>
        [Description("解决方案名称"), Category("方案"), DisplayName("方案名称")]
        public string ProjectName { get; set; } = string.Empty;

        [DescriptionAttribute(""), CategoryAttribute("报警记录"), DisplayName("报警记录地址")]
        public string TempString { get; set; } = "D:\\Vision2\\Temp";


        [DescriptionAttribute(""), CategoryAttribute("设备"), DisplayName("设备类型")]
        [TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("",
                  "运行设备", "远程复判", "SPC", "远程查询")]
        public string SetTypeMode { get; set; } = "运行设备";

        [Editor(typeof(PageTypeEditor_FolderBrowserDialog), typeof(UITypeEditor))]
        [DescriptionAttribute(""), CategoryAttribute("设备"), DisplayName("SPC查询地址")]
        public string SPCFindPaht { get; set; } = "E:\\历史记录";

        [Editor(typeof(PageTypeEditor_FolderBrowserDialog), typeof(UITypeEditor))]
        [DescriptionAttribute(""), CategoryAttribute("设备"), DisplayName("SPC查询图片地址")]
        public string SPCFindImagePaht { get; set; } = "E:\\整图图片";

        /// <summary>
        /// 显示窗口类型
        /// </summary>
        [Description("主窗口类型"), Category("窗口"), DisplayName("主窗口类型")]
        public bool MaxiForm { get; set; }

        /// <summary>
        /// 项目库
        /// </summary>
        [Description("项目库"), Category("库文件"), DisplayName("项目库")]
        public Dictionary<string, string> Dic_Project_Bank_Path { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 结构库
        /// </summary>
        [Description("结构库"), Category("库文件"), DisplayName("结构库")]
        public Dictionary<string, string> Dic_Combo_Bank_Path { get; set; } = new Dictionary<string, string>();

        [Editor(typeof(Vision2.UI.PropertyGrid.PageTypeEditor_FolderBrowserDialog), typeof(UITypeEditor))]
        /// <summary>
        /// 结构库
        /// </summary>
        [Description("方案运行的地址"), Category("运行参数"), DisplayName("启动方案地址")]
        public static string ProjectPathRun
        {
            get { return ProjietPath + "Project\\" + ProjectINI.In.ProjectName + "\\" + ProjectINI.In.RunName; }
        }

        /// <summary>
        /// 运行项目名称
        /// </summary>
        [Description("运行项目名称"), Category("运行参数"), DisplayName("运行项目名")]
        public string RunName { get; set; } = string.Empty;

        /// <summary>
        /// 运行模式
        /// </summary>
        [Description("调试、运行、自动、手动"), Category("运行参数"), DisplayName("运行模式")]
        public RunMode Run_Mode { get; set; } = new RunMode();



        /// <summary>
        /// 用户权限组
        /// </summary>
        [Description("用户工作组"), Category("用户管理"), DisplayName("用户权限组"), ReadOnly(true)]
        public List<string> UserRight { get; set; } = new List<string>();

        /// <summary>
        /// 用户权限
        /// </summary>
        [Description("用户权限集合"), Category("用户管理"), DisplayName("权限"), ReadOnly(true)]
        public List<string> ListRight { get; set; } = new List<string>();

        [Description("当前用户权限"), Category("用户管理"), DisplayName("当前权限"), ReadOnly(true)]
        public string Right { get; set; } = "";

        [Description("是否记忆用户名"), Category("用户管理"), DisplayName("是否记忆用户名"),]
        public bool IsMemory { get; set; } = true;

        /// <summary>
        /// 用户名
        /// </summary>
        [Description("用户名称"), Category("用户管理"), DisplayName("用户名"), ReadOnly(true)]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 用户ID
        /// </summary>
        [Description("用户名称"), Category("用户管理"), DisplayName("用户ID"), ReadOnly(true)]
        public string UserID { get; set; } = string.Empty;

        /// <summary>
        /// 部门
        /// </summary>
        [Description("用户部门"), Category("用户管理"), DisplayName("用户部门"), ReadOnly(true)]
        public string UserDepartment { get; set; } = string.Empty;

        #region 报警文本管理

        [Description("报警文本显示最大上限"), Category("报警显示"), DisplayName("报警文本上限")]
        public int MaxText
        { get { return 1000; } }

        [DescriptionAttribute("报警信息框显示。"), Category("报警显示"), DisplayName("是否显示信息框")]
        public bool IsAlramText { get; set; }

        [DescriptionAttribute("报警灯IO。"), Category("报警显示"), DisplayName("报警灯IO")]
        public int AlarmIntS { get; set; } = -1;

        #endregion 报警文本管理

 

   




        /// <summary>
        /// 运行模式
        /// </summary>
        public enum RunMode
        {
            /// <summary>
            /// 运行模式
            /// </summary>
            Run = 0,

            /// <summary>
            /// 调试
            /// </summary>
            Debug = 1,

            /// <summary>
            /// 自动模式
            /// </summary>
            Auto = 2,

            /// <summary>
            /// 手动模式
            /// </summary>
            Manual = 3,
        }



        /// <summary>
        /// 读取运行解决方案参数
        /// </summary>
        public void RaedRunPrajectData()
        {
            if (Dic_Project_Path.ContainsKey(RunName))
            {
                RaedRunPrajectData(Dic_Project_Path[RunName]);
            }
            else
            {
                MessageBox.Show("不存在的解决方案！");
            }
        }

        /// <summary>
        /// 读取解决方案参数
        /// </summary>
        public static void RaedRunPrajectData(string path)
        {
            try
            {
                if (Directory.Exists(Path.GetDirectoryName(path)))
                {
                    if (File.Exists(path))
                    {
                    }
                }
                MessageBox.Show("读取运行参数失败：文件丢失");
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取运行参数失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 保存项目，项目文件保存为Path地址，其他文件保存到项目名称下FileName
        /// </summary>
        /// <param name="path">项目设置保存地址</param>
        /// <param name="FileName">FileName为Null时，使用默认项目名</param>
        public void SaveProjectAll(string path, string FileName = null)
        {
            try
            {
                ClassToJsonSavePath(this, path);
                if (FileName == null)
                {
                    FileName = this.ProjectName;
                }
                Directory.CreateDirectory(ProjietPath + "Project\\" + FileName);//创建文件夹
                foreach (var item in Dic_Project_Path)
                {
                    Directory.CreateDirectory(ProjietPath + "Project\\" + FileName + "\\" + item.Key);//创建文件夹
                }

         
                System.GC.Collect();
                MessageBox.Show("保存成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 保存项目
        /// </summary>
        public void SaveProjectAll()
        {
            SaveProjectAll(ProjietPath + "ProjectInI\\ProjectName.Project");
        }

        /// <summary>
        /// 另存项目
        /// </summary>
        /// <param name="fileNmae"></param>
        public void SaveProject(string fileNmae)
        {
            SaveProjectAll(ProjietPath + "ProjectInI\\ProjectName.Project", fileNmae);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        public void SaveThis(string path = null)
        {
            if (path == null)
            {
                path = ProjietPath + "ProjectInI\\ProjectName.Project";
            }
            ClassToJsonSavePath(this, path);
        }








        /// <summary>
        /// 读取json文件转换为类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="obje"></param>
        /// <returns></returns>
        public static bool ReadPathJsonToCalss<T>(string path, out T obje)/* where T : new()*/
        {
            obje = default(T);
            try
            {
                if (!Path.GetFileName(path).Contains("."))
                {
                    path = path + ".txt";
                }
                if (File.Exists(path))
                {
                    string strdata = File.ReadAllText(path);
                    obje = JsonConvert.DeserializeObject<T>(strdata);
                    return true;
                }
                else
                {
                    MessageBox.Show("读取失败！文件不存在" + path);
                }
            }
            catch (Exception ex)
            {
           //     ErrForm.Show(ex, "读取文件:" + path + " 失败");
                //MessageBox.Show("读取文件:" + path + " 失败;" + ex.Message);
            }
            return false;
        }

        /// <summary>
        /// 读取不弹出错误
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="obje"></param>
        /// <returns></returns>
        public static bool ReadPathJsonToCalssEX<T>(string path, out T obje)
        {
            obje = default(T);
            try
            {
                if (!Path.GetFileName(path).Contains("."))
                {
                    path = path + ".txt";
                }
                if (File.Exists(path))
                {
                    string strdata = File.ReadAllText(path);
                    obje = JsonConvert.DeserializeObject<T>(strdata);
                    return true;
                }
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        /// <summary>
        /// 保存类到json文本文件，错误不会抛出提示
        /// </summary>
        /// <param name="obj">类</param>
        /// <param name="path">文件地址</param>
        /// <returns></returns>
        public static bool ClassToJsonSave(object obj, string path)
        {
            try
            {
                if (!Path.GetFileName(path).Contains('.'))
                {
                    path = path + ".txt";
                }
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                string jsonStr = JsonConvert.SerializeObject(obj);
                File.WriteAllText(path, jsonStr);
                return true;
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        /// <summary>
        /// 保存类到Json文件，错误会触发提示
        /// </summary>
        /// <param name="obje"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool ClassToJsonSavePath<T>(T obje, string path)
        {

            try
            {
                if (!Path.GetFileName(path).Contains('.'))
                {
                    path = path + ".txt";
                }
                string patht = Path.GetDirectoryName(path);
                if (!Directory.Exists(patht))
                {
                    Directory.CreateDirectory(patht, null);
                }
                //Serializer.ObjectToJson(obje, path);
                string jsonStr = JsonConvert.SerializeObject(obje);
                File.WriteAllText(path, jsonStr);
                //MessageBox.Show("保存成功");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败" + path + "=" + ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 时间相减
        /// </summary>
        /// <param name="InDateTime"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static TimeSpan TimeSpan(DateTime InDateTime, DateTime dateTime)
        {
            return InDateTime.Subtract(dateTime);
        }
  

        /// <summary>
        /// 类转换到Json字符
        /// </summary>
        /// <param name="obje"></param>
        /// <returns></returns>
        public static string ClassToJsonString(object obje)
        {
            string jsonStr = "";
            try
            {
                jsonStr = JsonConvert.SerializeObject(obje);
            }
            catch (Exception ex)
            {
            }
            return jsonStr;
        }

        /// <summary>
        /// 将类导出到文件
        /// </summary>
        /// <param name="calss">类对象</param>
        public static void Export_Class_File(object calss)
        {
            try
            {
                SaveFileDialog fbd = new SaveFileDialog();
                fbd.Title = "选择文件";
                fbd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); //初始路径
                fbd.Filter = "txt|*.txt;*.dt|所有文件|*.*";
                fbd.ShowDialog();
                if (fbd.FileNames.Length != 0)
                {
                    ProjectINI.ClassToJsonSavePath(calss, fbd.FileName);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 选择读取文件到对象类
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="calss">类对象</param>
        public static void ReadWeait<T>(out T calss)
        {
            calss = default(T);
            try
            {
                OpenFileDialog fbd = new OpenFileDialog();
                fbd.Title = "选择文件";
                fbd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); //初始路径
                fbd.Filter = "txt|*.txt;*.dt|所有文件|*.*";
                fbd.ShowDialog();
                if (fbd.FileNames.Length != 0)
                {
                    ProjectINI.ReadPathJsonToCalss(fbd.FileName, out calss);
                }
            }
            catch (Exception ex)
            { }
        }
        /// <summary>
        /// 复制文件夹及文件
        /// </summary>
        /// <param name="sourceFolder">原文件路径</param>
        /// <param name="destFolder">目标文件路径</param>
        /// <returns></returns>
        public static int CopyFolder2(string sourceFolder, string destFolder)
        {
            try
            {
                string folderName = System.IO.Path.GetFileName(sourceFolder);
                string destfolderdir = System.IO.Path.Combine(destFolder, folderName);
                string[] filenames = System.IO.Directory.GetFileSystemEntries(sourceFolder);
                foreach (string file in filenames)// 遍历所有的文件和目录
                {
                    if (System.IO.Directory.Exists(file))
                    {
                        string currentdir = System.IO.Path.Combine(destfolderdir, System.IO.Path.GetFileName(file));
                        if (!System.IO.Directory.Exists(currentdir))
                        {
                            System.IO.Directory.CreateDirectory(currentdir);
                        }
                        CopyFolder2(file, destfolderdir);
                    }
                    else
                    {
                        string srcfileName = System.IO.Path.Combine(destfolderdir, System.IO.Path.GetFileName(file));
                        if (!System.IO.Directory.Exists(destfolderdir))
                        {
                            System.IO.Directory.CreateDirectory(destfolderdir);
                        }
                        System.IO.File.Copy(file, srcfileName, true);
                    }
                }

                return 1;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return 0;
            }
        }

        /// <summary>
        /// 复制文件夹到新地址并改名称
        /// </summary>
        /// <param name="sourceFolder">目标文件夹</param>
        /// <param name="destFolder">目标地址及新名称</param>
        /// <returns></returns>
        public static int CopyFolder1(string sourceFolder, string destFolder)
        {
            try
            {
                string folderName = Path.GetFileName(sourceFolder);
                string destfolderdir = destFolder;/* Path.Combine(destFolder, folderName);*/
                string[] filenames = Directory.GetFileSystemEntries(sourceFolder);
                foreach (string file in filenames)// 遍历所有的文件和目录
                {
                    if (System.IO.Directory.Exists(file))
                    {
                        string currentdir = System.IO.Path.Combine(destfolderdir, System.IO.Path.GetFileName(file));
                        if (!System.IO.Directory.Exists(currentdir))
                        {
                            System.IO.Directory.CreateDirectory(currentdir);
                        }
                        CopyFolder2(file, destfolderdir);
                    }
                    else
                    {
                        string srcfileName = System.IO.Path.Combine(destfolderdir, System.IO.Path.GetFileName(file));
                        if (!System.IO.Directory.Exists(destfolderdir))
                        {
                            System.IO.Directory.CreateDirectory(destfolderdir);
                        }
                        File.Copy(file, srcfileName, true);
                    }
                }

                return 1;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return 0;
            }
        }


        /// <summary>
        /// 字符转换到类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Jonstring"></param>
        /// <param name="obje"></param>
        /// <returns></returns>
        public static bool StringJsonToCalss<T>(string Jonstring, out T obje)/* where T : new()*/
        {
            obje = default(T);
            try
            {
                obje = JsonConvert.DeserializeObject<T>(Jonstring);
                //登录窗口中 读取语言资源文件
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("读取文件:" + path + " 失败;" + ex.Message);
            }
            return false;
        }

        /// <summary>
        /// 获取路径下所有文件以及子文件夹中文件
        /// </summary>
        /// <param name="path">全路径根目录</param>
        /// <param name="FileList">存放所有文件的全路径</param>
        /// <param name="RelativePath"></param>
        /// <returns></returns>
        public static Dictionary<string, long> GetFile(string path, Dictionary<string, long> FileList, string RelativePath)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] fil = dir.GetFiles();
            DirectoryInfo[] dii = dir.GetDirectories();
            foreach (FileInfo f in fil)
            {
                //int size = Convert.ToInt32(f.Length);
                long size = f.Length;
                FileList.Add(f.FullName, size);//添加文件路径到列表中
            }
            //获取子文件夹内的文件列表，递归遍历
            foreach (DirectoryInfo d in dii)
            {
                GetFile(d.FullName, FileList, RelativePath);
            }
            return FileList;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="fAltTab"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        /// <summary>
        /// 保存项目到目标地址
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        public static void SvaeProject(ref string path, ref string name)
        {
            if (path.Length == 0)
            {
                DialogResult result = MessageBox.Show("保存路径为空！请重新选择保存路径", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    SaveFileDialog savefile = new SaveFileDialog();

                    savefile.RestoreDirectory = true;
                    savefile.FilterIndex = 1;
                    if (savefile.ShowDialog() == DialogResult.OK)
                    {
                        name = Path.GetFileNameWithoutExtension(savefile.FileName);
                        path = Path.GetDirectoryName(savefile.FileName) + "\\" + name;
                        //dSave(path);
                    }
                    else return;
                }
                else return;
            }
            else
            {
                //dSave(path);
            }
        }

        /// <summary>
        /// 查询StringBuilder数组变量
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int StingBuilderIndexOf(StringBuilder[] stringBuilder, string value)
        {
            for (int i = 0; i < stringBuilder.Length; i++)
            {
                if (stringBuilder[i].ToString() == value)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// 返回字符串中结尾数值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int GetStrReturnInt(string name)
        {
            string dsts = string.Empty;
            int intsd = 0;
            for (int i = name.Length; i > 0; i--)
            {
                if (int.TryParse(name[i - 1].ToString(), out int dss))
                {
                    dsts = dss + dsts;
                }
                else
                {
                    break;
                }
            }
            int.TryParse(dsts, out intsd);
            return intsd;
        }

        /// <summary>
        /// 获取字符结尾数值并加1返回新字符
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetStrReturnStr(string name)
        {
            int d = GetStrReturnInt(name, out string nameStr);
            return nameStr += d.ToString();
        }

        /// <summary>
        /// 返回字符串中结尾数值和字符串
        /// </summary>
        /// <param name="name">字符串</param>
        /// <param name="nameStr">截取结尾数值的字符串</param>
        /// <returns>返回结尾数值</returns>
        public static int GetStrReturnInt(string name, out string nameStr)
        {
            string dsts = string.Empty;
            nameStr = string.Empty;
            int intsd = 0;
            for (int i = name.Length; i > 0; i--)
            {
                if (int.TryParse(name[i - 1].ToString(), out int dss))
                {
                    dsts = dss + dsts;
                }
                else
                {
                    if (dsts.Length != 0)
                    {
                        nameStr = name.Remove(name.Length - dsts.Length);
                        int.TryParse(dsts, out intsd);
                    }
                    break;
                }
            }
            if (intsd == 0)
            {
                nameStr = name;
            }
            return intsd;
        }

        /// <summary>
        /// 返回字符串中开头数值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int GetStrReturnToInt(string name)
        {
            string dsts = string.Empty;
            int intsd = 0;
            int.TryParse(name.ToString(), out intsd);
            for (int i = 0; i < name.Length; i++)
            {
                if (int.TryParse(name[i].ToString(), out int dss))
                {
                    dsts = dsts + dss;
                }
                else
                {
                    int.TryParse(dsts, out intsd);
                    break;
                }
            }
            return intsd;
        }

        /// <summary>
        /// 获得文件夹下所以文件
        /// </summary>
        /// <param name="path">地址</param>
        /// <returns></returns>
        public static string[] GetFilesArrayPath(string path)
        {
            List<string> fileslist = new List<string>();
            if (!Directory.Exists(path))
            {
                return fileslist.ToArray();
            }
            string[] files = Directory.GetFiles(path);
            fileslist.AddRange(files);
            var paths = Directory.GetDirectories(path);
            for (int i = 0; i < paths.Length; i++)
            {
                var file = GetFilesArrayPath(paths[i]);
                fileslist.AddRange(file);
            }
            return fileslist.ToArray();
        }

        /// <summary>
        /// 获得文件夹下所有赛选文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="sele"></param>
        /// <returns></returns>
        public static string[] GetFilesArrayPath(string path, string sele)
        {
            return GetFilesArrayPath(path).Where(item => item.EndsWith(sele, StringComparison.Ordinal)).ToArray();
        }

        /// <summary>
        /// 获得文件夹下所有赛选文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="sele"></param>
        /// <returns></returns>
        public static Dictionary<string, List<string>> GetFilesDicListPath(string path, string sele)
        {
            string[] seles = sele.Split(',');
            Dictionary<string, List<string>> keyValuePairs = new Dictionary<string, List<string>>();
            if (!Directory.Exists(path))
            {
                return null;
            }
            List<string> fileslist = new List<string>();
            string d = path.Remove(0, path.IndexOf('\\') + 1);
            if (d == "")
            {
                d = path.Replace('\\', ' ');
            }

            string[] files = Directory.GetFiles(path);

            if (files.Length != 0)
            {
                List<string> filesSeles = new List<string>();
                for (int i = 0; i < files.Length; i++)
                {
                    for (int i1 = 0; i1 < seles.Length; i1++)
                    {
                        if (files[i].ToLower().EndsWith(seles[i1].ToLower(), StringComparison.Ordinal))
                        {
                            filesSeles.Add(files[i]);
                            break;
                        }
                    }
                }
                fileslist.AddRange(filesSeles);
                keyValuePairs.Add(d, fileslist);
            }
            var paths = Directory.GetDirectories(path);

            for (int i = 0; i < paths.Length; i++)
            {
                Dictionary<string, List<string>> file = GetFilesDicListPath(paths[i], sele);
                foreach (var item in file)
                {
                    if (!keyValuePairs.ContainsKey(item.Key))
                    {
                        keyValuePairs.Add(item.Key, item.Value);
                    }
                    else
                    {
                    }
                }
            }
            return keyValuePairs;
        }

        /// <summary>
        /// 为指定对象分配参数
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="dic">字段/值</param>
        /// <returns></returns>
        public static T Assign<T>(Dictionary<string, string> dic) where T : new()
        {
            Type t = typeof(T);
            T entity = new T();
            var fields = t.GetProperties();

            string val = string.Empty;
            object obj = null;
            foreach (var field in fields)
            {
                if (!dic.Keys.Contains(field.Name))
                    continue;
                val = dic[field.Name];
                //非泛型
                if (!field.PropertyType.IsGenericType)
                    obj = string.IsNullOrEmpty(val) ? null : Convert.ChangeType(val, field.PropertyType);
                else //泛型Nullable<>
                {
                    Type genericTypeDefinition = field.PropertyType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(Nullable<>))
                    {
                        obj = string.IsNullOrEmpty(val) ? null : Convert.ChangeType(val, Nullable.GetUnderlyingType(field.PropertyType));
                    }
                }
                field.SetValue(entity, obj, null);
            }

            return entity;
        }

        public static DataTable LanguageResources = new DataTable();

        public static void GetLanguageText<T>(T obj)
        {
            try
            {
                if (typeof(Form) == typeof(T).BaseType
                  || typeof(Control) == typeof(T))
                {
                    Control c = (Control)(object)obj;
                    DataRow[] drs = ProjectINI.LanguageResources.Select(string.Format("简体中文='{0}'", c.Text), "");
                    if (drs.Length > 0)
                    {
                        c.Text = drs[0][ProjectINI.In.Language].ToString();
                    }
                    if (c.Controls.Count > 0)
                    {
                        foreach (Control c1 in c.Controls)
                        {
                            GetLanguageText(c1);
                        }
                    }
                }
                //注意有些控件不是control类型的
                if (typeof(ToolStrip) == obj.GetType().BaseType
          || typeof(ToolStrip) == obj.GetType())
                {
                    ToolStrip c = (ToolStrip)(object)obj;

                    DataRow[] drs = ProjectINI.LanguageResources.Select(string.Format("简体中文='{0}'", c.Text), "");
                    if (drs.Length > 0)
                    {
                        c.Text = drs[0][ProjectINI.In.Language].ToString();
                    }
                    if (c.Items.Count > 0)
                    {
                        foreach (ToolStripItem c1 in c.Items)
                        {
                            GetLanguageText(c1);
                        }
                    }
                }

                if (typeof(MenuStrip) == obj.GetType().BaseType
                  || typeof(MenuStrip) == obj.GetType())
                {
                    MenuStrip c = (MenuStrip)(object)obj;
                    DataRow[] drs = ProjectINI.LanguageResources.Select(string.Format("简体中文='{0}'", c.Text), "");
                    if (drs.Length > 0)
                    {
                        c.Text = drs[0][ProjectINI.In.Language].ToString();
                    }
                    if (c.Items.Count > 0)
                    {
                        foreach (ToolStripMenuItem c1 in c.Items)
                        {
                            GetLanguageText(c1);
                        }
                    }
                }
                if (typeof(ToolStripItem) == obj.GetType().BaseType
                  || typeof(ToolStripItem) == obj.GetType())
                {
                    ToolStripItem c = (ToolStripItem)(object)obj;
                    DataRow[] drs = ProjectINI.LanguageResources.Select(string.Format("简体中文='{0}'", c.Text), "");
                    if (drs.Length > 0)
                    {
                        c.Text = drs[0][ProjectINI.In.Language].ToString();
                    }
                }
                if (typeof(ToolStripMenuItem) == obj.GetType().BaseType
                  || typeof(ToolStripMenuItem) == obj.GetType())
                {
                    ToolStripMenuItem c = (ToolStripMenuItem)(object)obj;
                    DataRow[] drs = ProjectINI.LanguageResources.Select(string.Format("简体中文='{0}'", c.Text), "");
                    if (drs.Length > 0)
                    {
                        c.Text = drs[0][ProjectINI.In.Language].ToString();
                    }
                    if (c.DropDownItems.Count > 0)
                    {
                        foreach (ToolStripItem c1 in c.DropDownItems)
                        {
                            GetLanguageText(c1);
                        }
                    }
                }
                if (typeof(DataGridView) == obj.GetType().BaseType
                  || typeof(DataGridView) == obj.GetType())
                {
                    DataGridView c = (DataGridView)(object)obj;
                    foreach (DataGridViewColumn col in c.Columns)
                    {
                        DataRow[] drs = ProjectINI.LanguageResources.Select(string.Format("简体中文='{0}'", col.HeaderText), "");
                        if (drs.Length > 0)
                        {
                            col.HeaderText = drs[0][ProjectINI.In.Language].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        #region ini 文件读写函数
        /// <summary>
        /// 写键值到指定的键值到临时TempInI文件
        /// </summary>
        /// <param name="appName">一级键</param>
        /// <param name="KeyName">参数名称</param>
        /// <param name="KeyValue">值</param>
        public static void WriteTempString(string appName, string KeyName, string KeyValue)
        {
            try
            {
                WritePrivateProfileString(appName, KeyName, KeyValue, ProjectINI.TempPath + "TempInI.ini");
            }
            catch (Exception ex)
            { }
        }

        /// <summary>
        /// 读取键值到指定的键值到临时TempInI文件
        /// </summary>
        /// <param name="appName">一级键</param>
        /// <param name="KeyName">参数名称</param>
        /// <param name="KeyValue">值</param>
        public static string ReadTempString(string appName, string KeyName, out string StirVaule)
        {
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                GetPrivateProfileString(appName, KeyName, " ", stringBuilder, 100, ProjectINI.TempPath + "TempInI.ini");
            }
            catch (Exception)
            {
            }
            StirVaule = stringBuilder.ToString();
            return StirVaule;
        }
        /// <summary>
        /// 写键值到指定的键值到临时TempInI文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="item"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetInI(string path, string item, string key)
        {
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                GetPrivateProfileString(item, key, " ", stringBuilder, 100, path);
            }
            catch (Exception)
            {
            }
            return stringBuilder.ToString();
        }

        //再一种声明，使用string作为缓冲区的类型同char[]
        /// <summary>
        /// 读取INI文件中指定的Key的值
        /// </summary>
        /// <param name="lpAppName">节点名称。如果为null,则读取INI中所有节点名称,每个节点名称之间用\0分隔</param>
        /// <param name="lpKeyName">Key名称。如果为null,则读取INI中指定节点中的所有KEY,每个KEY之间用\0分隔</param>
        /// <param name="lpDefault">读取失败时的默认值</param>
        /// <param name="lpReturnedString">读取的内容缓冲区，读取之后，多余的地方使用\0填充</param>
        /// <param name="nSize">内容缓冲区的长度</param>
        /// <param name="lpFileName">INI文件名</param>
        /// <returns>实际读取到的长度</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, [In, Out] char[] lpReturnedString, uint nSize, string lpFileName);

        //另一种声明方式,使用 StringBuilder 作为缓冲区类型的缺点是不能接受\0字符，会将\0及其后的字符截断,
        //所以对于lpAppName或lpKeyName为null的情况就不适用
        /// <summary>
        /// 读取INI文件中指定的Key的值
        /// </summary>
        /// <param name="lpAppName">节点名称。如果为null,则读取INI中所有节点名称,每个节点名称之间用\0分隔</param>
        /// <param name="lpKeyName">Key名称。如果为null,则读取INI中指定节点中的所有KEY,每个KEY之间用\0分隔</param>
        /// <param name="lpDefault">读取失败时的默认值</param>
        /// <param name="lpReturnedString">读取的内容缓冲区，读取之后，多余的地方使用\0填充</param>
        /// <param name="nSize">内容缓冲区的长度</param>
        /// <param name="lpFileName">INI文件名</param>
        /// <returns>实际读取到的长度</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);

        /// <summary>
        /// 读取INI文件中指定的Key的值
        /// </summary>
        /// <param name="lpAppName">节点名称。如果为null,则读取INI中所有节点名称,每个节点名称之间用\0分隔</param>
        /// <param name="lpKeyName">Key名称。如果为null,则读取INI中指定节点中的所有KEY,每个KEY之间用\0分隔</param>
        /// <param name="lpDefault">读取失败时的默认值</param>
        /// <param name="lpReturnedString">读取的内容缓冲区，读取之后，多余的地方使用\0填充</param>
        /// <param name="nSize">内容缓冲区的长度</param>
        /// <param name="lpFileName">INI文件名</param>
        /// <returns>实际读取到的长度</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, string lpReturnedString, uint nSize, string lpFileName);

        /// <summary>
        /// 将指定的键和值写到指定的节点，如果已经存在则替换
        /// </summary>
        /// <param name="lpAppName">节点名称</param>
        /// <param name="lpKeyName">键名称。如果为null，则删除指定的节点及其所有的项目</param>
        /// <param name="lpString">值内容。如果为null，则删除指定节点中指定的键。</param>
        /// <param name="lpFileName">INI文件</param>
        /// <returns>操作是否成功</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        #endregion ini 文件读写函数

    }

}
