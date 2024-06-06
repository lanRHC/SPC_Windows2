using HalconDotNet;
using MySql.Data.MySqlClient;
using NPOI.SS.Formula.Functions;
using SPC_Windows.SPCFile;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Vision2.vision.RestVisionForm
{
    public partial class SVisionForm : Form
    {
        public SVisionForm()
        {
            InitializeComponent();
        }
        HWindID HWindd = new HWindID();

        private void sVisionForm_Load(object sender, EventArgs e)
        {
            try
            {
                dataGridView4.AutoGenerateColumns = false;

                dataGridView3.AutoGenerateColumns = false;
                imageMode.GenEmptyObj();
                Column12.Width = 30; comboBox5.Items.Clear();
                comboBox5.Items.Add (ColorResult.green.ToString());
                comboBox5.Items.Add(ColorResult.blue.ToString());
                comboBox5.Items.Add(ColorResult.yellow.ToString());
                comboBox5.Items.Add(ColorResult.pink.ToString());
                comboBox5.Items.Add(ColorResult.red.ToString());
                comboBox5.SelectedItem = "red";
                try
                {
                    tabControl1.TabPages.Remove(tabPage8);
                    tabControl1.TabPages.Remove(tabPage7);
                    HOperatorSet.SetDraw(hSmartWindowControl1.HalconWindow, "margin");
                    HOperatorSet.SetLineWidth(hSmartWindowControl1.HalconWindow, HWindID.LineWidth);
                    HWindID.SetFont(hSmartWindowControl1.HalconWindow);
                    HOperatorSet.SetColor(hSmartWindowControl1.HalconWindow, "red");
                    HOperatorSet.SetDraw(hWindowControl3.HalconWindow, "margin");
                    HOperatorSet.SetLineWidth(hWindowControl3.HalconWindow, HWindID.LineWidth);
                    Vision.SetFont(hWindowControl3.HalconWindow);
                    HOperatorSet.SetColor(hWindowControl3.HalconWindow, "red");
                }
                catch (Exception) { }
                UI.DataGridViewF.StCon.AddCount(dataGridView2);
                UI.DataGridViewF.StCon.AddCount(dataGridView1);
                UI.DataGridViewF.StCon.AddCount(dataGridView3);
                UI.DataGridViewF.StCon.AddCount(dataGridView4);
                HWindd.Initialize(hWindowControl1);
                HWindID.AddMouseWheelZoon(hSmartWindowControl1);
                hSmartWindowControl1.Resize += hSmartWindowControl1_Resize;
                //hSmartWindowControl1.HMouseWheel += hSmartWindowControl1_HMouseWheel;
                //hSmartWindowControl1.HMouseDown += hSmartWindowControl1_HMouseDown;
                //hSmartWindowControl1.HMouseMove += hSmartWindowControl1_HMouseMove;
                //hSmartWindowControl1.HMouseUp += hSmartWindowControl1_HMouseUp;

                hWindowControl1.HMouseUp += HWindowControl1_HMouseUp;
                hWindowControl1.HMouseMove += HWindowControl1_HMouseMove;
                hWindowControl1.HMouseDown += HWindowControl1_HMouseDown;
                hWindowControl1.HMouseWheel += hSmartWindowControl1_HMouseWheel;
            }
            catch (Exception ex)
            {
            }
        }
        private void hSmartWindowControl1_HMouseUp(object sender, HMouseEventArgs e)
        {
            try
            {
                isMoveT2 = false;
            }
            catch (Exception)
            {
            }
        }
        bool isMoveT2;
        private void hSmartWindowControl1_HMouseDown(object sender, HMouseEventArgs e)
        {
            try
            {

                if (e.Button==MouseButtons.Left)
                {
                    isMoveT2 = true;
                    stratX = e.X;
                    stratY = e.Y;
                 
                }
                if (e.Clicks == 2)
                {
                    HOperatorSet.AreaCenter(OneRObjT.ROI(), out HTuple area, out HTuple row, out HTuple column);
                    UP(row.TupleInt(), column.TupleInt(), length1.TupleInt());
                    if (checkBox8.Checked)
                    {
                        hSmartWindowControl1.HalconWindow.DispObj(OneProductV.ListCamsData[OneRObjT.RunVisionName].GetImagePlus());
                    }
                    else
                    {
                        hSmartWindowControl1.HalconWindow.DispObj(OneRObjT.GetImage());
                    }
                    if (CamModeImage.ContainsKey(OneRObjT.RunVisionName))
                    {
                        hWindowControl3.HalconWindow.DispObj(CamModeImage[OneRObjT.RunVisionName]);
                    }
                    hSmartWindowControl1.HalconWindow.SetColor(ColorResult.red.ToString());
                    hSmartWindowControl1.HalconWindow.DispObj(OneRObjT.ROI());
       
                    if (Vision.IsObjectValided(OneRObjT.NGROI))
                    {
                        hSmartWindowControl1.HalconWindow.SetColor(comboBox5.SelectedItem.ToString());
                        hSmartWindowControl1.HalconWindow.DispObj(OneRObjT.NGROI);
                    }
                    hSmartWindowControl1.HalconWindow.DispText(OneRObjT.ComponentID + "{" + OneRObjT.NGText + "}", "window", 0, 0, "red", new HTuple(), new HTuple());
                    hWindowControl1.HalconWindow.DispText("参考图", "window", 0, 0, "green", new HTuple(), new HTuple());
                }
            }
            catch (Exception)
            {

            }

        }
        private HTuple m_ImageRow1, m_ImageCol1;
        private double stratX;
        private double stratY;  
        /// <summary>
        /// 
         /// </summary>
        public double WidthImage = 2000;
        /// <summary>
        /// 
        /// </summary>
        public double HeigthImage = 2000;

        private void hSmartWindowControl1_HMouseMove(object sender, HMouseEventArgs e)
        {
            try
            {
                if (isMoveT2)
                {
                  
                        double motionX, motionY;
                        motionX = ((e.X - stratX));
                        motionY = ((e.Y - stratY));
                        if (((int)motionX != 0) || ((int)motionY != 0))
                        {
                            if (m_ImageRow1 == null)
                            {
                                m_ImageRow1 = WidthImage;
                                m_ImageCol1 = HeigthImage;
                            }

                            System.Drawing.Rectangle rect2 = hSmartWindowControl1.HImagePart;
                            HTuple row = rect2.Y + -motionY;
                            HTuple colum = rect2.X + -motionX;
                            rect2.X = (int)Math.Round(colum.D);
                            rect2.Y = (int)Math.Round(row.D);
                            hSmartWindowControl1.HImagePart = rect2;
                            stratX = e.X - motionX;
                            stratY = e.Y - motionY;
                        }
                    
                    if (OneRObjT == null)
                    {
                        return;
                    }
                    hSmartWindowControl1.HalconWindow.ClearWindow();
                    if (checkBox8.Checked)
                    {
                        if (Vision.IsObjectValided(OneProductV.ListCamsData[OneRObjT.RunVisionName].GetImagePlus()))
                        {
                            hSmartWindowControl1.HalconWindow.DispObj(OneProductV.ListCamsData[OneRObjT.RunVisionName].GetImagePlus());
                        }
                        hSmartWindowControl1.HalconWindow.SetColor(ColorResult.red.ToString());
                        hSmartWindowControl1.HalconWindow.DispObj(OneRObjT.ROI());
                    }
                    else
                    {
                        if (Vision.IsObjectValided(OneRObjT.GetImage()))
                        {
                            hSmartWindowControl1.HalconWindow.DispObj(OneRObjT.GetImage());
                        }
                    }
                    if (CamModeImage.ContainsKey(OneRObjT.RunVisionName))
                    {
                        hWindowControl3.HalconWindow.DispObj(CamModeImage[OneRObjT.RunVisionName]);
                    }
              
                    if (Vision.IsObjectValided(OneRObjT.NGROI))
                    {
                        hSmartWindowControl1.HalconWindow.SetColor(comboBox5.SelectedItem.ToString());
                        hSmartWindowControl1.HalconWindow.DispObj(OneRObjT.NGROI);
                    }
                    hSmartWindowControl1.HalconWindow.DispText(OneRObjT.ComponentID + "{" + OneRObjT.NGText + "}", "window", 0, 0, "red", new HTuple(), new HTuple());
                }
            }
            catch (Exception)
            {
            }
        }
        private HTuple m_ImageRow0, m_ImageCol0, ptX, ptY, hv_Button;
        private HTuple H_Scale = 0.2; //缩放步长
        private HTuple MaxScale = 10000;//最大放大系数
        private HTuple Row0_1, Col0_1, Row1_1, Col1_1;
        private void hSmartWindowControl1_HMouseWheel(object sender, HMouseEventArgs e)
        {
            try
            {
                //if (e.Delta > 0)
                //{
                //    length1 = length1 + 50;
                //    UP((int)stratY, (int)stratX, length1.TupleInt());
                //}
                //else
                //{
                //    length1 = length1 - 50;
                //    UP((int)stratY, (int)stratX, length1.TupleInt());
                //}
                hSmartWindowControl1.HalconWindow.GetPart(out m_ImageRow0, out m_ImageCol0, out m_ImageRow1, out m_ImageCol1);
                HOperatorSet.GetMposition(hSmartWindowControl1.HalconWindow, out ptY, out ptX, out hv_Button);
                if (m_ImageRow1 == null)
                {
                    m_ImageRow1 = WidthImage;
                    m_ImageCol1 = HeigthImage;
                }
                //向上滑动滚轮，图像缩小。以当前鼠标的坐标为支点进行缩小或放大
                if (e.Delta > 0)
                {//重新计算缩小后的图像区域
                    Row0_1 = ptY - 1 / (1 - H_Scale) * (ptY - m_ImageRow0);
                    Row1_1 = ptY - 1 / (1 - H_Scale) * (ptY - m_ImageRow1);
                    Col0_1 = ptX - 1 / (1 - H_Scale) * (ptX - m_ImageCol0);
                    Col1_1 = ptX - 1 / (1 - H_Scale) * (ptX - m_ImageCol1);     //限定缩小范围
                    if ((Col1_1 - Col0_1).TupleAbs() / WidthImage <= 100)  //设置在图形窗口中显示局部图像
                    {
                        m_ImageRow0 = Row0_1;
                        m_ImageCol0 = Col0_1;
                        m_ImageRow1 = Row1_1;
                        m_ImageCol1 = Col1_1;
                    }
                }
                else
                {          //重新计算放大后的图像区域
                    Row0_1 = ptY - 1 / (1 + H_Scale) * (ptY - m_ImageRow0);
                    Row1_1 = ptY - 1 / (1 + H_Scale) * (ptY - m_ImageRow1);
                    Col0_1 = ptX - 1 / (1 + H_Scale) * (ptX - m_ImageCol0);
                    Col1_1 = ptX - 1 / (1 + H_Scale) * (ptX - m_ImageCol1);
                    HTuple dw = (WidthImage / (Col1_1 - Col0_1).TupleAbs());
                    if ((WidthImage / (Col1_1 - Col0_1).TupleAbs()) <= MaxScale)                    //限定放大范围
                    {
                        //设置在图形窗口中显示局部图像
                        m_ImageRow0 = Row0_1;
                        m_ImageCol0 = Col0_1;
                        m_ImageRow1 = Row1_1;
                        m_ImageCol1 = Col1_1;
                    }
                }
                HOperatorSet.SetPart(hSmartWindowControl1.HalconWindow, m_ImageRow0, m_ImageCol0, m_ImageRow1, m_ImageCol1);
                if (checkBox8.Checked)
                {
                    hSmartWindowControl1.HalconWindow.DispObj(OneProductV.ListCamsData[OneRObjT.RunVisionName].GetImagePlus());
                }
                else
                {
                    hSmartWindowControl1.HalconWindow.ClearWindow();
                    hSmartWindowControl1.HalconWindow.DispObj(OneRObjT.GetImage());
                }
         
                hSmartWindowControl1.HalconWindow.SetColor(ColorResult.red.ToString());
                hSmartWindowControl1.HalconWindow.DispObj(OneRObjT.ROI());
                hSmartWindowControl1.HalconWindow.SetColor(comboBox5.SelectedItem.ToString());
                if (Vision.IsObjectValided(OneRObjT.NGROI))
                {
                    hSmartWindowControl1.HalconWindow.DispObj(OneRObjT.NGROI);
                }

                hSmartWindowControl1.HalconWindow.DispText(OneRObjT.ComponentID + "{" + OneRObjT.NGText + "}", "window", 0, 0, "red", new HTuple(), new HTuple());
                //UpData(OneProductV, OneRObjT);
            }
            catch (Exception)
            {
            }
        }


        private void HWindowControl1_HMouseDown(object sender, HMouseEventArgs e)
        {
            try
            {
                isMoveT = true;
                UP((int)e.Y, (int)e.X, length1.TupleInt());
            }
            catch (Exception)
            {
            }
        }

        private void HWindowControl1_HMouseMove(object sender, HMouseEventArgs e)
        {
            try
            {
                if (isMoveT)
                {
                    UP((int)e.Y, (int)e.X, length1.TupleInt());
                    if (OneRObjT==null)
                    {
                        return;
                    }
                    if (Vision.IsObjectValided(OneProductV.ListCamsData[OneRObjT.RunVisionName].GetImagePlus()))
                    {
                        hSmartWindowControl1.HalconWindow.DispObj(OneProductV.ListCamsData[OneRObjT.RunVisionName].GetImagePlus());
                    }
                    if (CamModeImage.ContainsKey(OneRObjT.RunVisionName))
                    {
                        hWindowControl3.HalconWindow.DispObj(CamModeImage[OneRObjT.RunVisionName]);
                    }
                    hSmartWindowControl1.HalconWindow.SetColor(ColorResult.red.ToString());
                    hSmartWindowControl1.HalconWindow.DispObj(OneRObjT.ROI());
                    hSmartWindowControl1.HalconWindow.SetColor(comboBox5.SelectedItem.ToString());
                    if (Vision.IsObjectValided(OneRObjT.NGROI))
                    {
                        hSmartWindowControl1.HalconWindow.DispObj(OneRObjT.NGROI);
                    }
                    hSmartWindowControl1.HalconWindow.DispText(OneRObjT.ComponentID + "{" + OneRObjT.NGText + "}", "window", 0, 0, "red", new HTuple(), new HTuple());
                }
            }
            catch (Exception)
            {
            }
        }
        bool isMoveT;
        int ROWy;
        int colX;
        public void UP(int row, int col, int length1)
        {
            try
            {
                if (OneRObjT==null)
                {
                    return;
                }
                if (length1<0)
                {
                    return;
                }
                if (length1<200)
                {
                    length1 = length1*4;
                }
                ROWy = row;
                colX = col;
    
                hWindowControl3.HalconWindow.ClearWindow();

                HWindID.SetPart(hWindowControl3.HalconWindow, row, col, length1, ratio);
                if (CamModeImage.ContainsKey(OneRObjT.RunVisionName))
                {
                    hWindowControl3.HalconWindow.DispObj(CamModeImage[OneRObjT.RunVisionName]);
                }
      
            }
            catch (Exception ex)
            {
            }
        }
        private void HWindowControl1_HMouseUp(object sender, HMouseEventArgs e)
        {
            try
            {
                isMoveT = false;
            }
            catch (Exception)
            {
            }
        }

        private void hSmartWindowControl1_Resize(object sender, EventArgs e)
        {
            UpData(OneProductV);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    button1FindSN.PerformClick();
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 查询图片文件夹SN,
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="pathImage"></param>
        /// <returns></returns>
        bool FindImageSN(string sn, string pathImage, out string[] imagePaht)
        {
            imagePaht = null;
            try
            {
                if (!Directory.Exists(pathImage))
                {
                    return false;
                }
                string selePath = pathImage;
                string dataTime = dateTimePicker1.Value.ToString("yyyy年MM月dd日");
                if (File.Exists(selePath + "\\" + dataTime))
                {
                    string[] paths = Directory.GetDirectories(selePath + "\\" + dataTime);
                }
                int cont = 1;
                if (checkBox1.Checked)
                {
                    cont = 7;
                }
                for (int j = 0; j < cont; j++)
                {
                    if (!checkBox2.Checked)
                    {
                        if (finDone)
                        {
                            return false;
                        }
                    }
                    dataTime = dateTimePicker1.Value.AddDays((-1 * j)).ToString("yyyy年MM月dd日");
                    if (Directory.Exists(selePath + "\\" + dataTime))
                    {
                        string[] paths = Directory.GetDirectories(selePath + "\\" + dataTime);
                        for (int i = 0; i < paths.Length; i++)
                        {
                            if (!checkBox2.Checked)
                            {
                                if (finDone)
                                {
                                    return false;
                                }
                            }
                            string[] pathst = Directory.GetDirectories(paths[i]);
                            string[] imagePaths = Array.FindAll(pathst, delegate (string x) { return x.Contains(sn); });
                            if (imagePaths.Length == 1)
                            {
                                pathst = Directory.GetFiles(imagePaths[0]);
                                imagePaths = pathst;
                                imagePaht = Array.FindAll(imagePaths, delegate (string x) { return !x.Contains(" NG "); });//返回不包含NG的文件
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        /// <summary>
        /// 查找SN文件地址
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="pathImage"></param>
        /// <param name="imagePaht"></param>
        /// <returns></returns>
        bool FindPlusImageSN(string sn, string pathImage, out string[] imagePaht)
        {
            imagePaht = null;
            try
            {

                if (!Directory.Exists(pathImage))
                {
                    return false;
                }
                string selePath = pathImage;
                //string dataTime = dateTimePicker1.Value.ToString("D");
                string dataTime = dateTimePicker1.Value.ToString("yyyy年MM月dd日");
                if (File.Exists(selePath + "\\" + dataTime))
                {
                    string[] paths = Directory.GetDirectories(selePath + "\\" + dataTime);
                }
                int cont = 1;
                if (checkBox1.Checked)
                {
                    cont = 7;
                }
                for (int j = 0; j < cont; j++)
                {
                    if (!checkBox2.Checked)
                    {
                        if (finDone)
                        {
                            return false;
                        }
                    }
                    dataTime = dateTimePicker1.Value.AddDays((-1 * j)).ToString("yyyy年MM月dd日");
                    if (Directory.Exists(selePath + "\\" + dataTime))
                    {
                        string[] paths = Directory.GetFiles(selePath + "\\" + dataTime).ToList().FindAll(x => x.EndsWith(".txt")).ToArray();
                        if (!checkBox2.Checked)
                        {
                            if (finDone)
                            {
                                return false;
                            }
                        }
                        string[] imagePaths = Array.FindAll(paths, delegate (string x) { return x.Contains(sn); });
                        if (imagePaths.Length != 0)
                        {
                            imagePaht = imagePaths;
                            return true;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        List<string> SNLsitD = new List<string>();
        /// <summary>
        /// 
        /// </summary>
        List<string> pathS;
        string distName;
        bool finDone;
        /// <summary>
        /// 产品文件夹下文件地址
        /// </summary>
        string[] ImagePath;

        string DistPath;
        Dictionary<string, List<string>> Device = new Dictionary<string, List<string>>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="sortPaht"></param>
        /// <returns></returns>
        public static string TPath(string path, string sortPaht,bool isNetLog)
        {
            string itemString = path;
            try
            {
                if (sortPaht==null)
                {
                    return path;
                }
                if (isNetLog)
                {
                    string[] data = sortPaht.Split('\\');
                    itemString =  data[0]   + path.Remove(0, 2);
                }
                else
                {

                }
                if (sortPaht.StartsWith(@"\\"))
                {
                    string[] data = sortPaht.Split('\\');
                    itemString = @"\\" + data[2] + "\\" + path.Substring(0, 1) + path.Remove(0, 2);
                }
            }
            catch (Exception ex)
            { }
            return itemString;
        }
        public void Finde(string sn)
        {
            try
            {
                if (sn == "")
                {
                    return;
                }
                finDone = false;
                OneRObjT = null;

                //ImagePath = null;
                //textBox1.Text = "";
                Device.Clear();
                treeView3.Nodes.Clear();
                if (checkBox7.Checked)
                {
                    Task.Run(() => {
                        FindeSQLSN(sn);
                    });
                }
                else
                {
                    Task[] tasks = new Task[Vision.Instance.PathImageFind.Count];
                    int j = 0;
                    foreach (var item in Vision.Instance.PathImageFind)
                    {
                        TreeNode treeNode = treeView3.Nodes.Add(item.Key + "=>" + item.Value);
                        treeNode.Name = item.Key;
                        if (!Device.ContainsKey(item.Key))
                        {
                            Device.Add(item.Key, new List<string>());
                        }
                        tasks[j] = new Task(() =>
                        {
                            if (FindImageSN(sn, item.Value, out string[] imagePatht))
                            {
                                Device[item.Key].AddRange(imagePatht);
                                finDone = true;
                                DistPath = item.Value;
                                //distName = item.Key;
                            }
                            if (!finDone)
                            {

                                if (FindPlusImageSN(sn, item.Value, out string[] imagePaht))
                                {
                                    if (!Device.ContainsKey(item.Key))
                                    {
                                        Device.Add(item.Key, new List<string>());
                                    }
                                    Device[item.Key].AddRange(imagePaht);
                                    DistPath = item.Value;
                                    finDone = true;
                                }
                            }

                        });
                        tasks[j].Start();
                        j++;
                    }
                    for (int i = 0; i < tasks.Length; i++)
                    {
                        tasks[i].Wait();
                    }
                    if (Vision.Instance.PathImageFind.Count == 0)
                    {
                        label2.Text = sn + "未设置查询地址！";
                        return;
                    }
                    label2.Text = sn;
                    if (Device.Count == 0)
                    {
                        label2.Text += "不存在！";
                        return;
                    }
                    if (!SNLsitD.Contains(sn))
                    {
                        SNLsitD.Add(sn);
                    }
                    if (SNLsitD.Count > 20)
                    {
                        SNLsitD.RemoveAt(0);
                    }
                    List<string > strings = new List<string>();
                    foreach (var item in Device)
                    {
                        TreeNode[] treeNodes = treeView3.Nodes.Find(item.Key, true);
                        TreeNode treeNode = treeNodes[0];
                        treeNode.Tag = item.Value;
                        treeNode.Text = item.Key + "=>" + Vision.Instance.PathImageFind[item.Key];
                        pathS = item.Value.FindAll(x => x.Contains("Data"));
                        distName = item.Key;
                        strings.AddRange(pathS);
                        for (int i = 0; i < pathS.Count; i++)
                        {
                            TreeNode treeNode1 = treeNode.Nodes.Add(Path.GetFileNameWithoutExtension(pathS[i]));
                            treeNode1.Tag = pathS[i];
                        }
                        treeNode.Expand();
                        ImagePath = item.Value.FindAll(x => !x.EndsWith(".txt")).ToArray();
                    }
                    treeView3.ExpandAll();
                    if (strings.Count != 0)
                    {
                        if (strings.Count >= 1)
                        {

                            if (strings.Count > 1)
                            {
                                tabControl2.SelectedIndex = 2;
                            }
                        }
                        if (strings.Count != 0)
                        {
                            button1FindSN.Enabled = false;
                            //Task.Run(() => {
                                ReadPathOneProduct(strings[strings.Count - 1]);
                                button1FindSN.Enabled = true;
                            //});
              
                        }
                    }
                    else
                    {
                        label2.Text += "Data.txt文件丢失！";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void FindeSQLSN(string sn)
        {
            try
            {
                if (OneProductV!=null)
                {
                    OneProductV.Dispose();
                }
        
                int j = 0;
                if (Vision.Instance.ListSQLConnt.Count == 0)
                {
                    Vision.Instance.ListSQLConnt.Add("本机", RecipeCompiler.Instance.SqlContString);
                }
                foreach (var item in Vision.Instance.ListSQLConnt)
                {
                    TreeNode treeNode = treeView3.Nodes.Add(item.Key);
                    treeNode.Name = item.Key;
                    if (!Device.ContainsKey(item.Key))
                    {
                        Device.Add(item.Key, new List<string>());
                    }
                    DataSet ds;
                    MySqlDataAdapter adapter;
                    MySqlCommandBuilder cb;
                    string sql = "SELECT * FROM product where SN ='" + sn + "'";
                    mySqlHelperEx.connstr = item.Value;
                    DataSet dataSetT = mySqlHelperEx.GetDataSet(sql);
                    DataTable dt = dataSetT.Tables[0];
                    if (dt.Rows.Count != 0)
                    {
                        DistPath = item.Key;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            TreeNode treeNode1 = treeNode.Nodes.Add(dt.Rows[i]["SNTime"].ToString() + "=>" + dt.Rows[i]["TestResult"] + ":" + dt.Rows[i]["NGNumber"]);
                            OneDataVale oneDataVale = new OneDataVale();
                            ProjectINI.StringJsonToCalss(dt.Rows[i]["CRDTest"].ToString(), out oneDataVale);
                            treeNode1.Tag = oneDataVale;
                            if (i==0)
                            {
                                UpOneData(oneDataVale);
                            }
                        }
            
                    }
                }

                if (Device.Count == 0)
                {
                    label2.Text += "不存在！";
                    return;
                }
                if (!SNLsitD.Contains(sn))
                {
                    SNLsitD.Add(sn);
                }
                if (SNLsitD.Count > 20)
                {
                    SNLsitD.RemoveAt(0);
                }
                treeView3.ExpandAll();
                //textBox1.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        double ratio;

        /// <summary>
        /// 单个元件
        /// </summary>
        private SPC_Windows.SPCFile.OneCompOBJs.OneComponent OneRObjT;

        OneDataVale OneProductV;
        /// <summary>
        /// 模板图片
        /// </summary>
        HObject imageMode = new HObject();
 

       Dictionary<string, HObject> CamModeImage=new Dictionary<string, HObject>();

        HTuple length1 = new HTuple(500);

        HObject ROI = new HObject();

        HObject NGErr = new HObject();

        /// <summary>
        /// 显示产品缺陷，
        /// </summary>
        /// <param name="OneProductV">产品</param>
        /// <param name="oneC">缺陷元件</param>
        public void UpData(OneDataVale OneProductV, OneCompOBJs.OneComponent oneC = null)
        {
            try
            {
                if (OneProductV == null)
                {
                    return;
                }

                if (OneRObjT!=oneC)
                {
                    OneRObjT = oneC;
                }
                if (OneRObjT==null)
                {
                    OneRObjT = oneC;
                }
                List<string> CamsName = new List<string>();
                if (!comboBox3.Items.Contains("ALL"))
                {
                    comboBox3.Items.Add("ALL");
                }
                if (comboBox3.SelectedIndex==-1)
                {
                    comboBox3.SelectedItem = "ALL";
                }
                RaedCamImage(OneProductV, oneC);
                foreach (var item in OneProductV.ListCamsData)
                {
                    if (!comboBox3.Items.Contains(item.Key))
                    {
                        comboBox3.Items.Add(item.Key);
                    }
                    CamsName.Add(item.Key);
                    string imagePath = "";
                    continue;
                }
                List<string> RemoveNames = new List<string>();
                for (int i = 0; i < comboBox3.Items.Count; i++)
                {
                    if (!CamsName.Contains(comboBox3.Items[i]))
                    {
                        RemoveNames.Add(comboBox3.Items[i].ToString());
                    }
                }
                for (int i = 0; i < RemoveNames.Count; i++)
                {
                    if (RemoveNames[i]!="ALL")
                    {
                        comboBox3.Items.Remove(RemoveNames[i]);
                    }
                }
                if (oneC!=null)
                {
                    //VisionCam = oneC.RunVisionName;
                    if (!Vision.IsObjectValided(OneProductV.ListCamsData[oneC.RunVisionName].GetImagePlus()))
                    {
                        RaedCamImage(OneProductV);
                    }
                }
                else
                {
                    foreach (var item in OneProductV.GetNGCompData().DicOnes)
                    {
                        oneC = item.Value;
                        break;
                    }
                    OneRObjT = oneC;
                }

                HTuple width = 0;
                HTuple heigth =0;
                if (checkBox8.Checked)
                {
                    if (OneProductV != null && Vision.IsObjectValided(OneProductV.ListCamsData[OneRObjT.RunVisionName].GetImagePlus()))
                    {
                        HOperatorSet.GetImageSize(OneProductV.ListCamsData[OneRObjT.RunVisionName].GetImagePlus(), out width, out heigth);
                        if (width.Length == 1)
                        {
                            HTuple det = width.D / heigth.D;
                            hWindowControl1.Dock = DockStyle.None;
                            double heig = (double)hWindowControl1.Width / det.D;
                            hWindowControl1.Height = (int)heig;
                            if (hWindowControl1.Height < 400)
                            {
                                hWindowControl1.Height = 400;
                                hWindowControl1.Width = (int)(400 * det.D);
                            }
                        }
                        HWindd.SetImage(OneProductV.ListCamsData[oneC.RunVisionName].GetImagePlus(), true);
                    }
                }
                else
                {
                
                    if (OneProductV != null && Vision.IsObjectValided(CamModeImage[oneC.RunVisionName]))
                    {
                        HOperatorSet.GetImageSize(CamModeImage[oneC.RunVisionName], out width, out heigth);
                        if (width.Length == 1)
                        {
                            HTuple det = width.D / heigth.D;
                            hWindowControl1.Dock = DockStyle.None;
                            double heig = (double)hWindowControl1.Width / det.D;
                            hWindowControl1.Height = (int)heig;
                            if (hWindowControl1.Height < 400)
                            {
                                hWindowControl1.Height = 400;
                                hWindowControl1.Width = (int)(400 * det.D);
                            }
                        }
                        HWindd.SetImage(CamModeImage[oneC.RunVisionName], true);
                    }
                }
     
                string PatText = OneProductV.PanelID + ": ";
                if (OneProductV.OK)
                {
                    PatText += "Pass";
                }
                else
                {
                    PatText += "Fail";
                }
                PatText += "  NG点:" + OneProductV.NGNumber + Environment.NewLine + "托盘号:" + OneProductV.TrayLocation + "  机台号:" + OneProductV.DeviceName + "  线号:" + OneProductV.LineName + Environment.NewLine;
                PatText += "开始:" + OneProductV.StrTime.ToString() + Environment.NewLine +
                    "结束:" + OneProductV.EndTime.ToLongTimeString();
                label2.Text = PatText;
;
                HObject hObject6 = new HObject();
                hObject6.GenEmptyObj();
                hSmartWindowControl1.HalconWindow.ClearWindow();
                hWindowControl3.HalconWindow.ClearWindow();
                HTuple mesage=new HTuple();
                mesage.Append(oneC.ComponentID + "{" + oneC.NGText + "}");
                if (oneC != null)
                {
                    dataGridView1.Rows.Clear();
                    for (int i = 0; i < oneC.oneRObjs.Count; i++)
                    {
                        if (oneC.oneRObjs[i].dataMinMax != null)
                        {
                            for (int j = 0; j < oneC.oneRObjs[i].dataMinMax.Reference_Name.Count; j++)
                            {
                                int det = dataGridView1.Rows.Add();
        
                                if (oneC.oneRObjs[i].dataMinMax.GetRestOK(j))
                                    dataGridView1.Rows[det].DefaultCellStyle.BackColor = Color.Green;
                                else
                                    dataGridView1.Rows[det].DefaultCellStyle.BackColor = Color.Red;
                                dataGridView1.Rows[det].Cells[0].Value = oneC.oneRObjs[i].dataMinMax.Reference_Name[j];
                                if (oneC.oneRObjs[i].dataMinMax.Reference_ValueMin.Count > j)
                                {
                                    dataGridView1.Rows[det].Cells[1].Value = oneC.oneRObjs[i].dataMinMax.Reference_ValueMin[j];
                                }
                                if (oneC.oneRObjs[i].dataMinMax.Reference_ValueMax.Count > j)
                                {
                                    dataGridView1.Rows[det].Cells[2].Value = oneC.oneRObjs[i].dataMinMax.Reference_ValueMax[j];
                                }
                                if (oneC.oneRObjs[i].dataMinMax.ValueStrs.Count > j)
                                {
                                    dataGridView1.Rows[det].Cells[3].Value = oneC.oneRObjs[i].dataMinMax.ValueStrs[j];
                                }
                            }
                        }
                    }
                    if (oneC.aOK)
                    {
                        HWindd.OneResIamge.AddNameOBJ(oneC.ComponentID, oneC.ROI(), ColorResult.yellow);
                    }
                    else
                    {
                        HWindd.OneResIamge.AddNameOBJ(oneC.ComponentID, oneC.ROI(), ColorResult.red);
                    }
                
                    if (width.Length == 1)
                    {
                        ratio = (double)width / (double)heigth ;
                    }

                    HObject hObject1 = null;
                    HTuple row1 = 0;
                    HTuple col1 = 0;
                    HTuple row2 = 0;
                    HTuple col2 = 0;
                    if (!Vision.IsObjectValided(oneC.ROI()) && oneC.oneRObjs.Count != 0)
                    {
                        for (int i = 0; i < oneC.oneRObjs.Count; i++)
                        {
                            if (oneC.oneRObjs[i].Row > 0)
                            {
                                HOperatorSet.GenRectangle2(out HObject hObject, oneC.oneRObjs[i].Row, oneC.oneRObjs[i].Col, new HTuple(-oneC.oneRObjs[i].Anlge).TupleRad(), oneC.oneRObjs[i].Length1, oneC.oneRObjs[i].Length2);
                                oneC.oneRObjs[i].ROI(hObject);
                                break;
                            }
                        }
                    }
                    hObject1 = Vision.XLD_To_Region(oneC.ROI());
                    HOperatorSet.Union1(hObject1, out hObject1);
                    HWindd.OneResIamge.SetObjCross(hObject1, out row1, out col1, out row2, out col2);
                    if (row1.Length != 0)
                    {
                        HOperatorSet.SmallestRectangle2(hObject1, out HTuple row, out HTuple col, out HTuple phi, out length1, out HTuple leng2);
                        HOperatorSet.HeightWidthRatio(hObject1, out  length1, out HTuple width2t, out  HTuple rongt);
                        length1 = length1 / 2;
                        HOperatorSet.GenContourPolygonXld(out HObject hObject, new HTuple(row, row), new HTuple(new HTuple(0), col1));
                        HOperatorSet.GenContourPolygonXld(out HObject hObject2, new HTuple(new HTuple(0), row1), new HTuple(col, col));
                        if (width.Length == 1)
                        {
                            HOperatorSet.GenContourPolygonXld(out HObject hObject3, new HTuple(new HTuple(row), row), new HTuple(col2, width));
                            HOperatorSet.GenContourPolygonXld(out HObject hObject4, new HTuple(new HTuple(row2), heigth ), new HTuple(col, col));
                            HWindd.OneResIamge.SetCross(hObject.ConcatObj(hObject2).ConcatObj(hObject3).ConcatObj(hObject4));
                        }
                        //UP(row.TupleInt(), col.TupleInt(), length1.TupleInt());
                        if (Vision.GetHalconRunName(oneC.RunVisionName).Navigation_Picture.GetOriginal_Names().ContainsKey(oneC.ComponentID))
                        {
                            row= Vision.GetHalconRunName(oneC.RunVisionName).Navigation_Picture.GetOriginal_Names()[oneC.ComponentID].Row;
                            col = Vision.GetHalconRunName(oneC.RunVisionName).Navigation_Picture.GetOriginal_Names()[oneC.ComponentID].Col;
                            UP(row.TupleInt(), col.TupleInt(), length1.TupleInt());
                            if (oneC.aOK) hWindowControl3.HalconWindow.SetColor("green");
                            else hWindowControl3.HalconWindow.SetColor("red");


                            hWindowControl3.HalconWindow.DispObj(Vision.GetHalconRunName(oneC.RunVisionName).Navigation_Picture.GetOriginal_Names()[oneC.ComponentID].ROI);
                        }
                      
                    }
                    else
                    {
                        if (!Vision.IsObjectValided(oneC.ROI()) && oneC.oneRObjs.Count != 0)
                        {
                            for (int i = 0; i < oneC.oneRObjs.Count; i++)
                            {
                                if (oneC.oneRObjs[i].Row > 0)
                                {
                                    HOperatorSet.GenRectangle2(out HObject hObject, oneC.oneRObjs[i].Row, oneC.oneRObjs[i].Col, new HTuple(-oneC.oneRObjs[i].Anlge).TupleRad(), oneC.oneRObjs[i].Length1, oneC.oneRObjs[i].Length2);
                                    oneC.oneRObjs[i].ROI(hObject);
                                    break;
                                }
                            }
                        }
                        HOperatorSet.Union1(oneC.NGROI, out hObject1);
                        hObject1 = Vision.XLD_To_Region(hObject1);
                        HWindd.OneResIamge.SetObjCross(hObject1, out row1, out col1, out row2, out col2);
                        if (row1.Length != 0)
                        {
                            HOperatorSet.SmallestRectangle2(hObject1, out HTuple row, out HTuple col, out HTuple phi, out length1, out HTuple leng2);
                            HOperatorSet.GenContourPolygonXld(out HObject hObject, new HTuple(row, row), new HTuple(new HTuple(0), col1));
                            HOperatorSet.GenContourPolygonXld(out HObject hObject2, new HTuple(new HTuple(0), row1), new HTuple(col, col));
                            if (width.Length == 1)
                            {
                                HOperatorSet.GenContourPolygonXld(out HObject hObject3, new HTuple(new HTuple(row), row), new HTuple(col2, width));
                                HOperatorSet.GenContourPolygonXld(out HObject hObject4, new HTuple(new HTuple(row2), heigth ), new HTuple(col, col));
                                HWindd.OneResIamge.SetCross(hObject.ConcatObj(hObject2).ConcatObj(hObject3).ConcatObj(hObject4));
                            }
                            UP(row.TupleInt(), col.TupleInt(), length1.TupleInt());
                        }
                    }
                    NGErr = oneC.NGROI;
                    if (oneC.oneRObjs.Count > 0)
                    {
                        HWindd.OneResIamge.Massage = new HTuple();
                        if (oneC.oneRObjs[0].dataMinMax != null)
                        {
                            List<string> vstd = oneC.oneRObjs[0].dataMinMax.GetStrTextNG();
                            HTuple hTuple2 = new HTuple(vstd.ToArray());
                            HWindd.AddMeassge(hTuple2);
                            List<string> vs = oneC.oneRObjs[0].dataMinMax.GetStrNG();
                            HTuple hTuple = new HTuple(vs.ToArray());
                            HWindd.AddMeassge(hTuple);
                        }
                        if (Vision.IsObjectValided(oneC.oneRObjs[0].ROI()))
                        {
                            HOperatorSet.Union1(oneC.oneRObjs[0].ROI(), out hObject1);
                        }
                        hObject1 = Vision.XLD_To_Region(hObject1);
                        HOperatorSet.SmallestRectangle1(hObject1, out row1, out col1, out row2, out col2);
                        NGErr = oneC.oneRObjs[0].NGROI;
                        if (row1.Length != 0)
                        {
                            HOperatorSet.SmallestRectangle2(hObject1, out HTuple row, out HTuple col, out HTuple phi, out length1, out HTuple leng2);
                            HOperatorSet.GenContourPolygonXld(out HObject hObject, new HTuple(row, row), new HTuple(new HTuple(0), col1));
                            HOperatorSet.GenContourPolygonXld(out HObject hObject2, new HTuple(new HTuple(0), row1), new HTuple(col, col));
                            if (width.Length == 1)
                            {
                                HOperatorSet.GenContourPolygonXld(out HObject hObject3, new HTuple(new HTuple(row), row), new HTuple(col2, width));
                                HOperatorSet.GenContourPolygonXld(out HObject hObject4, new HTuple(new HTuple(row2), heigth ), new HTuple(col, col));
                                HWindd.OneResIamge.SetCross(hObject.ConcatObj(hObject2).ConcatObj(hObject3).ConcatObj(hObject4));
                            }
                            //UP(row.TupleInt(), col.TupleInt(), length1.TupleInt());
                        }
                        else
                        {
                            hObject1 = Vision.XLD_To_Region(oneC.oneRObjs[0].NGROI);

                            HOperatorSet.SmallestRectangle1(hObject1, out row1, out col1, out row2, out col2);
                            if (row1.Length != 0)
                            {
                                HOperatorSet.SmallestRectangle2(hObject1, out HTuple row, out HTuple col, out HTuple phi, out length1, out HTuple leng2);
                                HOperatorSet.GenContourPolygonXld(out HObject hObject, new HTuple(row, row), new HTuple(new HTuple(0), col1));
                                HOperatorSet.GenContourPolygonXld(out HObject hObject2, new HTuple(new HTuple(0), row1), new HTuple(col, col));
                                HOperatorSet.GenContourPolygonXld(out HObject hObject3, new HTuple(new HTuple(row), row), new HTuple(col2, width));
                                HOperatorSet.GenContourPolygonXld(out HObject hObject4, new HTuple(new HTuple(row2), heigth ), new HTuple(col, col));
                                HWindd.OneResIamge.SetCross(hObject.ConcatObj(hObject2).ConcatObj(hObject3).ConcatObj(hObject4));
                                HWindID.SetPart(hSmartWindowControl1.HalconWindow, row.TupleInt(), col.TupleInt(), length1.TupleInt() * 2, ratio);
                                HWindID.SetPart(hWindowControl3.HalconWindow, row.TupleInt(), col.TupleInt(), length1.TupleInt() * 2, ratio);
                            }
                        }
                    }
                    if (checkBox8.Checked)
                    {
                        if (Vision.IsObjectValided(OneProductV.ListCamsData[oneC.RunVisionName].GetImagePlus()))
                        {
                            hSmartWindowControl1.HalconWindow.DispObj(OneProductV.ListCamsData[oneC.RunVisionName].GetImagePlus());
                        }
                    }
                    else
                    {
                        if (Vision.IsObjectValided(oneC.GetImage()))
                        {
                            hSmartWindowControl1.HalconWindow.ClearWindow();
                            HOperatorSet.GetImageSize(oneC.GetImage(), out HTuple width2, out HTuple height);
                            HOperatorSet.SetPart(hSmartWindowControl1.HalconWindow, 0, 0, height, width2);
                            hSmartWindowControl1.HalconWindow.DispObj(oneC.GetImage());
                            if (Vision.IsObjectValided(oneC.NGROI))
                            {
                                hSmartWindowControl1.HalconWindow.SetColor(comboBox5.SelectedItem.ToString());
                                hSmartWindowControl1.HalconWindow.DispObj(oneC.NGROI);
                            }
                     
                            hSmartWindowControl1.SetFullImagePart(null);
                        }
                    }
             
                    //if (CamModeImage.ContainsKey(oneC.RunVisionName))
                    //{
                    //    hWindowControl3.HalconWindow.DispObj(CamModeImage[oneC.RunVisionName]);
                    //}
                    HOperatorSet.AreaCenter(oneC.NGROI, out HTuple areas, out HTuple rows, out HTuple colus);

                    //if (oneC.aOK)
                    //{
                    //    hSmartWindowControl1.HalconWindow.SetColor(ColorResult.green.ToString());
                    //}
                    //else
                    //{
                    //    hSmartWindowControl1.HalconWindow.SetColor(ColorResult.red.ToString());
                    //}
                    //hSmartWindowControl1.HalconWindow.DispObj(hObject1);
    
                    if (Vision.IsObjectValided(NGErr))
                    {
                        hSmartWindowControl1.HalconWindow.SetColor(comboBox5.SelectedItem.ToString());
                        hSmartWindowControl1.HalconWindow.DispObj(NGErr);
                    }
                    if (oneC.aOK)
                        hSmartWindowControl1.HalconWindow.DispText(mesage, "window", new HTuple(0), new HTuple(0), new HTuple("green"), new HTuple("box"), new HTuple("false"));
                    else
                        hSmartWindowControl1.HalconWindow.DispText(mesage, "window", new HTuple(0), new HTuple(0), new HTuple("red"), new HTuple("box"), new HTuple("false"));
                    //  hSmartWindowControl1.HalconWindow.DispText(oneC.ComponentID + "{" + oneC.NGText + "}", "window", 0, 0, "red", new HTuple(), new HTuple());

                }
                else
                {
                    hSmartWindowControl1.HalconWindow.SetPart(0, 0, HWindd.HeigthImage, HWindd.WidthImage);
                    hSmartWindowControl1.HalconWindow.DispObj(OneProductV.ListCamsData[oneC.RunVisionName].GetImagePlus());
                    length1 = heigth  / 4;
                    if (width.Length == 0)
                    {
                        return;
                    }
                    HWindID.SetPart(hSmartWindowControl1.HalconWindow, heigth .TupleInt() / 2, width.TupleInt() / 2, length1.TupleInt() * 2, ratio);
                    HWindID.SetPart(hWindowControl3.HalconWindow, heigth .TupleInt() / 2, width.TupleInt() / 2, length1.TupleInt() * 2, ratio);
                    hSmartWindowControl1.HalconWindow.DispObj(OneProductV.ListCamsData[oneC.RunVisionName].GetImagePlus());
      
                }
                HWindd.ShowObj();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //string VisionCam;
        /// <summary>
        /// 读取对应相机名的参考图
        /// </summary>
        /// <param name="camKey">相机名称</param>
        public void RaedCamImage( OneDataVale oneDataVale, OneCompOBJs.OneComponent oneC = null)
        {
            try
            {
                //oneDataVale.ImagePath2
                   string[] paths = null;
                    foreach (var item in oneDataVale.ListCamsData)
                    {
                        if (checkBox8.Checked)
                        {
                            if (!HWindID.IsObjectValided(item.Value.GetImagePlus()))
                            {
                                if (item.Value.ImagePaht != "")
                                {
                                    string iamgePath = TPath(item.Value.ImagePaht, DistPath, false);
                                    richTextBox1.AppendText(iamgePath + Environment.NewLine);
                                    if (File.Exists(iamgePath))
                                    {
                                        HOperatorSet.ReadImage(out HObject imagePlus, iamgePath);
                                        item.Value.GetImagePlus(imagePlus);
                                    }
                                    else if (ImagePath != null)
                                    {
                                        paths = Array.FindAll(ImagePath, (x => x.Contains(item.Key)));
                                        if (paths.Length >= 1)
                                        {
                                            HOperatorSet.ReadImage(out HObject imagePlus, paths[0]);
                                            item.Value.GetImagePlus(imagePlus);
                                        }
                                    }
                                }
                            }
                        }
                        else if(oneC!=null)
                    {
                        string iamgePath = Path.GetDirectoryName(TPath(item.Value.ImagePaht, DistPath, false));
                         string path= oneDataVale.PanelID;
                        string imageFile = oneC.BankName + "_" + oneC.ComponentID + "_" + oneC.Location + "_" + oneC.RunVisionName + "_" +
                             oneDataVale.StrTime.ToString("HHmmss");
                        if (oneC.aOK) imageFile += "_P.jpg";
                        else imageFile += "_F.jpg";

                        if (File.Exists(iamgePath +"\\"+ path+"\\" + imageFile)&& oneC.GetImage()==null)
                        {
                            HOperatorSet.ReadImage(out HObject image, iamgePath + "\\" + path + "\\" + imageFile);
                            oneC.GetImage(image);
                            HOperatorSet.GetImageSize(image, out HTuple widht, out HTuple heigth);
                            HTuple hTuple=  oneC.GetNGobj().GetObjClass();
                            HOperatorSet.VectorAngleToRigid(oneC.oneRObjs[0].Row, oneC.oneRObjs[0].Col,0, heigth/2, widht/2, new HTuple(oneC.oneRObjs[0].Anlge).TupleRad(), out HTuple hTupleHomMat);
                            foreach (var itemobj in oneC.oneRObjs)
                            {
                                HOperatorSet.AffineTransRegion(itemobj.NGROI, out HObject regionAffineTrans, hTupleHomMat, "nearest_neighbor");
                                itemobj.NGROI = regionAffineTrans;
                            }
                        }
                    }
                    if (checkBox6.Checked && !CamModeImage.ContainsKey(item.Key))
                    {
                        string[] images = Directory.GetFiles(Vision.VisionProductPath + "Image\\");
                        bool isbde = false;
                        for (int i = 0; i < images.Length; i++)
                        {
                            if (images[i].StartsWith(Vision.VisionProductPath + "Image\\" + item.Key + "拼图"))
                            {
                                if (images[i].Contains(item.Key))
                                {
                                    if (images[i].Contains("拼图"))
                                    {
                                        HOperatorSet.ReadImage(out HObject ImageDT, images[i]);
                                        CamModeImage.Add(item.Key, ImageDT);
                                        break;
                                    }
                                }
                                hWindowControl3.HalconWindow.DispObj(CamModeImage[item.Key]);
                                isbde = true;
                                break;
                            }
                        }
                        if (!isbde)
                        {
                            hWindowControl3.HalconWindow.DispText("未创建参考图片" + item.Key,
                                "window", 0, 0, "red", new HTuple(), new HTuple());
                        }
                    }
                }
            }
            catch (Exception ex)
            {   }
        }
        OneCamData oneCamData;
   



        private void panel1_Click(object sender, EventArgs e)
        {
            try
            {
                if (OneProductV != null)
                {
                    propertyGrid2.SelectedObject = OneProductV;
                    textBox1.Text = OneProductV.PanelID;
                }
            }
            catch (Exception)
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
             Finde(textBox1.Text);
            Cursor = Cursors.Arrow;
        }

        MySqlHelperEx mySqlHelperEx = new MySqlHelperEx();




        private void textBox1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception)
            {
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                contextMenuStrip1.Items.Clear();
                for (int i = 0; i < SNLsitD.Count; i++)
                {
                    contextMenuStrip1.Items.Add(SNLsitD[i]);
                }
            }
            catch (Exception)
            {
            }
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                textBox1.Text = e.ClickedItem.Text;
            }
            catch (Exception)
            {
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            try
            {
                propertyGrid2.SelectedObject = OneProductV;
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public void ReadPathOneProduct(string path)
        {
            try
            {

                if (OneProductV==null ||   !path.Contains(OneProductV.StrTime.ToString("HH时mm分ss秒")))
                {
                    if (OneProductV != null)
                    {
                        OneProductV.Dispose();
                    }
                    ErosProjcetDLL.Project.ProjectINI.ReadPathJsonToCalss<OneDataVale>(path, out OneProductV);
                    if (!checkBox8.Checked)
                    {
                        //foreach (var itemCam in OneProductV.ListCamsData)
                        //{
                        //    foreach (var item in itemCam.Value.DicNGObj.DicOnes)
                        //    {
                        //        for (int i = 0; i < item.Value.oneRObjs.Count; i++)
                        //        {
                        //           if (Vision.IsObjectValided(item.Value.oneRObjs[i].NGROI))
                        //           {
                                    
                        //           }
                        //        }
                           
                        //    }
                        //}
                    }
                }
                UpOneData(OneProductV);
                UpData(OneProductV);
            }
            catch (Exception ex)
            {
                ErrForm.Show(ex, "读取产品属性错误");
            }
        }
        /// <summary>
        /// 刷新产品到表格
        /// </summary>
        /// <param name="oneDataVale"></param>
        public void UpOneData(OneDataVale oneDataVale)
        {
            try
            {
                if (oneDataVale==null)
                {
                    return;
                }
                OneProductV = oneDataVale;
                propertyGrid1.SelectedObject = OneProductV;
                Up3DDatas();
                RaedCamImage(OneProductV);
                HWindd.HobjClear();
                List<OneComponent> oneComponents = new List<OneComponent>();
                if (comboBox3.SelectedItem == null || comboBox3.SelectedItem == "ALL")
                {
                    foreach (var itemdt in OneProductV.ListCamsData)
                    {
                        if (oneCamData == null)
                        {
                            itemdt.Value.RunVisionName = itemdt.Key;
                            oneCamData = itemdt.Value;
                        }
                        var listVisionName = from n in itemdt.Value.DicNGObj.DicOnes
                                   where n.Value.RunVisionName ==""
                                   select n.Value;
                        foreach (var item in listVisionName)
                        {
                            for (int i = 0; i < item.oneRObjs.Count; i++)
                            {
                                item.oneRObjs[i].RunName = itemdt.Key; 
                            }
                        }
                        var list = from n in itemdt.Value.DicNGObj.DicOnes
                                   orderby !n.Value.aOK descending
                                   select n.Value;
                        oneComponents.AddRange(list.ToList());
                    }
                }
                else
                {
                    var list = from n in OneProductV.ListCamsData[comboBox3.SelectedItem.ToString()].DicNGObj.DicOnes
                               orderby !n.Value.aOK descending
                               select n.Value;
                    oneComponents.AddRange(list.ToList());
                }
                var list2 = from n in oneComponents
                            where n.ComponentID.Contains(textBox2.Text)
                            select n;
                List<OneComponent> oneComponents2 = new List<OneComponent>();
                oneComponents2.AddRange(list2.ToList());

                if (comboBox4.SelectedIndex==3)
                {
                    list2 = from n in oneComponents2
                            where n.AutoOK!=n.aOK
                            select n;
                }
                else if (comboBox4.SelectedIndex == 1)
                {
                    list2 = from n in oneComponents2
                            where !n.AutoOK
                            select n;
                }
                else if (comboBox4.SelectedIndex == 2)
                {
                    list2 = from n in oneComponents2
                            where n.AutoOK
                            select n;
                }

                dataGridView3.DataSource = new BindingList<OneComponent>(list2.ToList());
                OneRObjT = oneComponents[0];
                UpData(OneProductV, OneRObjT);
            }
            catch (Exception ex)
            {
                ErrForm.Show(ex, "读取产品属性错误");
            }
        }



        private void treeView3_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if (checkBox7.Checked)
                {
                   propertyGrid1.SelectedObject=     e.Node.Tag;
                    if (e.Node.Tag is OneDataVale)
                    {
                        UpOneData(e.Node.Tag as OneDataVale);
                    }
                }
                else
                {
                    if (e.Node.Tag != null)
                    {
                        if (e.Node.Tag is List<string>)
                        {
                            List<string> pahts = e.Node.Tag as List<string>;
                            pathS = pahts.FindAll(x => x.Contains("Data"));
                            ImagePath = pahts.ToArray();
                            distName = e.Node.Text;
                        }
                        else
                        {
                            if (e.Node.Parent.Text != distName)
                            {
                                distName = e.Node.Parent.Text;
                                List<string> pahts = e.Node.Parent.Tag as List<string>;

                                pathS = pahts.FindAll(x => x.Contains("Data"));
                                ImagePath = pahts.ToArray();
                            }
                            ReadPathOneProduct(e.Node.Tag.ToString());
                        }

                    }
                }
 
            }
            catch (Exception ex)
            {
            }
            Cursor = Cursors.Arrow;
        }

        private void SVisionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (OneProductV != null)
                {
                    foreach (var item in OneProductV.ListCamsData)
                    {
                        item.Value.GetImagePlus().Dispose();
                    }
                    OneProductV.Dispose();
                    OneProductV = null;
                }
                imageMode.Dispose();
                //if (Image_New!=null)
                //{
                //    Image_New.Dispose();
                //}
                foreach (var item in CamModeImage)
                {
                    item.Value.Dispose();
                }
            }
            catch (Exception ex) 
            {
            }
        }

        private void 导入文本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "请选择文件";
                openFileDialog.Multiselect = false;
                //openFileDialog.InitialDirectory = Vision.VisionPath + "Image\\";
                openFileDialog.Filter = "文件|*.txt;*.json";
                if (openFileDialog.ShowDialog() != DialogResult.OK) return;
                string item = "";
                if (ProjectINI.ReadPathJsonToCalssEX(openFileDialog.FileName, out OneDataVale OneP))
                {

                    OneP.KeyPamgr["path"] = item;
                    string PATH = item + "\\Temp";
                    Directory.CreateDirectory(PATH + "\\Time\\");
                    if (OneP.ImagePaht != "")
                    {
                        string image = OneP.ImagePaht;
                        if (item.StartsWith(@"\\"))
                        {
                            string[] data = item.Split('\\');
                            image = @"\\" + data[2] + "\\" + OneP.ImagePaht.Substring(0, 1) + OneP.ImagePaht.Remove(0, 2);
                        }
                        OneP.KeyPamgr["path"] = image;
                        if (Directory.Exists(image))
                        {
                            string[] imagephtus = Directory.GetFiles(image);
                            foreach (var itemTd in OneP.ListCamsData)
                            {
                                string imagePt = itemTd.Value.ImagePaht;
                                if (imagePt == "")
                                {
                                    for (int j = imagephtus.Length - 1; j > 0; j--)
                                    {
                                        if (imagephtus[j].Contains(itemTd.Key) && imagephtus[j].Contains("拼图"))
                                        {
                                            if (!OneP.KeyPamgr.ContainsKey(itemTd.Key + "pathImage"))
                                            {
                                                OneP.KeyPamgr.Add(itemTd.Key + "pathImage", imagephtus[j]);
                                            }
                                            OneP.KeyPamgr[itemTd.Key + "pathImage"] = imagephtus[j];
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    for (int j = imagephtus.Length - 1; j > 0; j--)
                                    {
                                        if (imagephtus[j].EndsWith(imagePt.Remove(0, 10)))
                                        {
                                            if (!OneP.KeyPamgr.ContainsKey(itemTd.Key + "pathImage"))
                                            {
                                                OneP.KeyPamgr.Add(itemTd.Key + "pathImage", imagephtus[j]);
                                            }
                                            OneP.KeyPamgr[itemTd.Key + "pathImage"] = imagephtus[j];
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            OneP.ImagePaht = Path.GetDirectoryName(openFileDialog.FileName);
                            string[] imagephtus = Directory.GetFiles(OneP.ImagePaht);
                            List<string> imageTs = new List<string>(imagephtus);
                            imageTs = imageTs.FindAll(n => n.EndsWith(".jpg"));
                            imagephtus = imageTs.ToArray();
                            foreach (var itemTd in OneP.ListCamsData)
                            {
                                string imagePt = itemTd.Value.ImagePaht;
                                if (imagePt == "")
                                {
                                    for (int j = imagephtus.Length - 1; j > 0; j--)
                                    {
                                        if (imagephtus[j].Contains(itemTd.Key) && imagephtus[j].Contains("拼图"))
                                        {
                                            if (!OneP.KeyPamgr.ContainsKey(itemTd.Key + "pathImage"))
                                            {
                                                OneP.KeyPamgr.Add(itemTd.Key + "pathImage", imagephtus[j]);
                                            }
                                            OneP.KeyPamgr[itemTd.Key + "pathImage"] = imagephtus[j];
                                            break;
                                        }
                                    }
                                    if (true)
                                    {

                                    }
                                }
                                else
                                {
                                    for (int j = imagephtus.Length - 1; j > 0; j--)
                                    {
                                        if (imagephtus[j].EndsWith(imagePt.Remove(0, 10)))
                                        {
                                            if (!OneP.KeyPamgr.ContainsKey(itemTd.Key + "pathImage"))
                                            {
                                                OneP.KeyPamgr.Add(itemTd.Key + "pathImage", imagephtus[j]);
                                            }
                                            OneP.KeyPamgr[itemTd.Key + "pathImage"] = imagephtus[j];
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    this.Invoke(new Action(() =>
                    {
                        try
                        {
                            TreeNode[] treeNodesDevices = treeView3.Nodes.Find(OneP.DeviceName, false);
                            TreeNode treeNodeDevices = null;
                            if (treeNodesDevices.Length == 0)
                            {
                                treeNodeDevices = treeView3.Nodes.Add(OneP.DeviceName);
                                treeNodeDevices.Name = OneP.DeviceName;
                            }
                            else
                            {
                                treeNodeDevices = treeNodesDevices[0];
                            }

                            TreeNode[] treeNodes = treeNodeDevices.Nodes.Find(OneP.PanelID, false);
                            if (treeNodes.Length == 0)
                            {
                                treeNodeDevices = treeNodeDevices.Nodes.Add(OneP.PanelID);
                                treeNodeDevices.Name = OneP.PanelID;
                                treeNodeDevices.Tag = OneP.ImagePaht;
                                treeNodeDevices.Tag = Path.GetFileName(openFileDialog.FileName);
                            }
                            treeView3.ExpandAll();
                        }
                        catch (Exception ex)
                        {
                        }
                    }));
                    ReadPathOneProduct(openFileDialog.FileName);
                }

            }
            catch (Exception)
            {

            }
        }

        private void comboBox1_DropDownClosed(object sender, EventArgs e)
        {

        }

        private void comboBox2_DropDownClosed(object sender, EventArgs e)
        {

        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBox7.Checked)
                {
                    dateTimePicker1.Visible = false;
                    checkBox1.Visible = false;
                    checkBox2.Visible = false;
                }
                else
                {
                    dateTimePicker1.Visible = true;
                    checkBox1.Visible = true;
                    checkBox2.Visible = true;
                }
 
  
            }
            catch (Exception)
            {
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                UpOneData(OneProductV);

                //HWindd.HobjClear();
                //List<OneComponent> oneComponents = new List<OneComponent>();
                //if (comboBox3.SelectedItem == null || comboBox3.SelectedItem == "ALL")
                //{
                //    foreach (var itemdt in OneProductV.ListCamsData)
                //    {
                //        if (oneCamData == null)
                //        {
                //            itemdt.Value.RunVisionName = itemdt.Key;
                //            oneCamData = itemdt.Value;
                //        }
                //        var list = from n in itemdt.Value.DicNGObj.DicOnes
                //                   orderby !n.Value.aOK descending
                //                   select n.Value;
                //        oneComponents.AddRange(list.ToList());
                //    }
                //}
                //else
                //{
                //    var list = from n in OneProductV.ListCamsData[comboBox3.SelectedItem.ToString()].DicNGObj.DicOnes
                //               orderby !n.Value.aOK descending
                //               select n.Value;
                //    oneComponents.AddRange(list.ToList());
                //}
                //var list2 = from n in oneComponents
                //           where n.ComponentID.Contains(textBox2.Text) 
                //           select n;
                //dataGridView3.DataSource = new BindingList<OneComponent>(list2.ToList());

            }
            catch (Exception)
            {

            }
        }

        private void comboBox5_DropDownClosed_1(object sender, EventArgs e)
        {
            try
            {


            }
            catch (Exception)
            {
            }
        }

        private void dataGridView3_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {

            }
        }
  


        private void comboBox3_DropDownClosed(object sender, EventArgs e)
        {
            try
            {

                UpOneData(OneProductV);


            }
            catch (Exception)
            {

            }
        }

        private void dataGridView3_SelectionChanged(object sender, EventArgs e)
        {
                try
                {
                    if (dataGridView3.SelectedCells.Count==0)
                    {
                        return;
                    }
                     int rowindex=dataGridView3.SelectedCells[0].RowIndex;
                    OneComponent component = dataGridView3.Rows[rowindex].DataBoundItem as OneComponent;
                    if (component != null)
                    {
                        UpData(OneProductV, component);
                    }
                    propertyGrid2.SelectedObject = dataGridView3.Rows[rowindex].DataBoundItem;
                }
                catch (Exception ex)
                {
                }

        }

        private void tableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            try
            {
                if (e.Column == 1 && e.Row == 0)
                {
                    using (SolidBrush brush = new SolidBrush(Color.GreenYellow))
                        e.Graphics.FillRectangle(brush, e.CellBounds);
                }
            }
            catch (Exception)
            {

            }
        }

        private void comboBox4_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                UpOneData(OneProductV);

            }
            catch (Exception)
            {

            }
        }

        public void Up3DDatas()
        {
            try
            {
                if (OneProductV == null)
                {
                    dataGridView3.DataSource = null;
                    return;
                }
                List<DataMinMax.Reference_Valuves> reference_Valuves = new List<DataMinMax.Reference_Valuves>();
                foreach (var item in OneProductV.M3Dmove)
                {
                    reference_Valuves.AddRange(item.Value);
                }
                dataGridView4.DataSource = new BindingList<DataMinMax.Reference_Valuves>(reference_Valuves);
            }
            catch (Exception ex)
            {

                MessageBox.Show("3D数据显示错误："+ex.Message);
            }

        }
        private void comboBox5_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedIndex >= 0)
                {
                }
                else
                {
      
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView3_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            try
            {
                OneComponent original_Navigation = this.dataGridView3.Rows[e.RowIndex].DataBoundItem as OneComponent;
                Color color = Color.Green;
                if (original_Navigation.AutoOK)
                {
                    color = Color.Green;
                }
                else
                {
                    color = Color.Yellow;
                }
                if (this.dataGridView3.Rows[e.RowIndex].Cells[2].Style.BackColor != color)
                {
                    this.dataGridView3.Rows[e.RowIndex].Cells[2].Style.BackColor = color;
                }
          
             


            }
            catch (Exception)
            {

            }
        }
    }
}
