using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision2.vision;
using System.Windows.Forms.DataVisualization.Charting;
using MySql.Data.MySqlClient;
using HalconDotNet;
using SPC_Windows.SPCFile;
using Vision2.UI.DataGridViewF;
using Vision2.SQL;
using SPC_Windows.Project;

namespace Vision2.SPCFile
{
    public partial class SPCForm1 : Form
    {

        // 数据库连接字符串
        private string connectionString = "server=localhost;uid=root;pwd=r4,h1,c5,;port=3309;";
        private string selectedDatabase;

        public SPCForm1()
        {
            InitializeComponent();
            LoadDatabases();
            combox_sql.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            mysql_connect.Click += buttonConnect_Click;

            //trackBar1.Minimum = 0;
            //trackBar1.Maximum = ComponentS.Count > 0 ? ComponentS.Count - 1 : 0;
            //trackBar1.Value = 0;

            comboBoxDataCount1.Text = "10"; // 默认显示的数量
            comboBoxDataCount2.Text = "10"; // 默认显示的数量

            textBox_Countlables(null, null);
            trackBar1.Scroll += new EventHandler(trackBar1_Scroll);
        }

        private void LoadDatabases()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SHOW DATABASES";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            combox_sql.Items.Add(reader[0].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading databases: " + ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedDatabase = combox_sql.SelectedItem.ToString();
        }



        //初始化饼状图筛选
        private void InitializeComboBox()
        {

            for (int i = 1; i <= ComponentS.Count; i++)
            {
                comboBoxDataCount1.Items.Add(i);
                comboBoxDataCount2.Items.Add(i);

            }
            comboBoxDataCount1.DropDownStyle = ComboBoxStyle.DropDown; // 允许手动输入
            comboBoxDataCount1.SelectedIndexChanged += comboBoxDataCount_SelectedIndexChanged;
            comboBoxDataCount1.TextChanged += comboBoxDataCount_TextChanged;


            comboBoxDataCount2.DropDownStyle = ComboBoxStyle.DropDown; // 允许手动输入
            comboBoxDataCount2.SelectedIndexChanged += comboBoxDataCount2_SelectedIndexChanged;
            comboBoxDataCount2.TextChanged += comboBoxDataCount2_TextChanged;
        }


        private void 日数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                tabPage1.Controls.Clear();
                MControl1 mControl1 = new MControl1();
                mControl1.Dock = DockStyle.Fill;
                mControl1.UpData(SPC.SPCDicProduct);
                tabPage1.Controls.Add(mControl1);
           
            }
            catch (Exception)
            {

            }
        }

        private void SPCForm1_Load(object sender, EventArgs e)
        {
            try
            {
                chart5.Dock = DockStyle.Fill;
                tableLayoutPanel1.Dock = DockStyle.Fill;



                // 设置下拉选框选项
                comboBoxDataCount1.Items.AddRange(new object[] { 5, 10, 15, 20, 25, 30 });
                comboBoxDataCount1.SelectedIndex = 1; // 默认选择第一个选项

                comboBoxDataCount2.Items.AddRange(new object[] { 5, 10, 15, 20, 25, 30 });
                comboBoxDataCount2.SelectedIndex = 1; // 默认选择第一个选项
                UpdateChart();
                UpdateChart2();
                InitializeComboBox();

                //if (!RecipeCompiler.Instance.EnableSQL)
                //{
                //    tabControl1.TabPages.Remove(tabPage6);
                //    tabControl1.SelectTab(4);
                //}

                dataGridView5.AddCoun();
                toolStripTextBox1.Text = ProjectINI.In.SPCFindPaht;
                toolStripTextBox2.Text = ProjectINI.In.SPCFindImagePaht;
                dataGridView4.AddCoun();
                dataGridView4.AutoGenerateColumns = false;
                dataGridViewSN.AddCoun();
                dataGridViewImage.AddCoun();
                dataGridView3.AddCoun();
                dataGridView3.AutoGenerateColumns = false;
                //Binding bind2 = new Binding("Checked", vision.Vision.Instance, "EnableNGCrdNameTime");
                //checkBox1.DataBindings.Clear();
                //checkBox1.DataBindings.Add(bind2);
                //bind2 = new Binding("Value", vision.Vision.Instance, "NGCrdNameTimeCont");
                //numericUpDown1.DataBindings.Clear();
                //numericUpDown1.DataBindings.Add(bind2);
                //bind2 = new Binding("Value", vision.Vision.Instance, "NGCrdNameTimeHH");
                //numericUpDown2.DataBindings.Clear();
                //numericUpDown2.DataBindings.Add(bind2);
                dateTimePicker3.Value = DateTime.Now;
                开始日期.Value = DateTime.Now;
                dateTimePicker2.Value = DateTime.Now;
                dateTimePicker1.Value = DateTime.Now.AddDays(-7);
                dateTimePickerStratTime.Value = DateTime.Now.AddDays(-6);
                textBox1.Text = ProjectINI.TempPath;
                月数据ToolStripMenuItem_Click(null, null);
                Dictionary<string, CRDNGTextTime> keyValuePairs = GetCrdNGName();
                BindingList<CRDNGTextTime> cRDNGTextTimes = new BindingList<CRDNGTextTime>(keyValuePairs.Values.ToList());
                dataGridView1.DataSource = cRDNGTextTimes;
                //if (Vision.Instance.SaveIamgeData.IsSavePalusPaht)
                //{
                //    path = Vision.Instance.SaveIamgeData.SavePlausPath;
                //}
                //else
                //{
                //    path = Vision.Instance.SaveIamgeData.SavePath;
                //}
                textBox4.Text = path;//查询图片地址
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
       
        }

        private void 月数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                //SPC.ReadData
                tabPage1.Controls.Clear();
                panel1.Controls.Add(tabControl1);
                tabPage1.Controls.Add(panel3);

                chartType.Dock = DockStyle.Fill;
               // tabPage1.Controls.Add(chartType);
                chartType.BringToFront();
                panel2.Enabled = true;
            }
            catch (Exception)
            {
            }
        }
        List<Dictionary<string, OKNumberClass>> DesignMode = new List<Dictionary<string, OKNumberClass>>();
        List<string> DataDey = new List<string>();
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                TimeSpan ts = dateTimePicker2.Value - dateTimePicker1.Value;
                string dateTimeString = DateTime.Now.ToString("yy年MM月dd日");
                DesignMode = new List<Dictionary<string, OKNumberClass>>();
                DataDey = new List<string>();
                dateTimeString = dateTimePicker1.Value.ToString("yy年MM月dd日");
                dateTimePicker1.Value.AddDays(dateTimePicker2.Value.Day);

                chartType.Series.Clear();
                chartType.ChartAreas.Clear();
                chartType.Titles.Clear();
                chartType.Legends[0].Enabled = true;

                Series Series1 = new Series();
                chartType.Series.Add(Series1);
                Series Series2 = new Series();
                Series2.Color = Color.Green;
                Series2.ChartType = SeriesChartType.Column;
                Series2.Label = "#VALY";
                Series2.ToolTip = "#VALX";
                Series2["PointWidth"] = "0.5";
                Series2.XValueMember = "name";
                Series2.YValueMembers = "sumcount";
                Series2.LegendText = "OK";

                chartType.Series.Add(Series2);
                Series Series3 = new Series();
                chartType.Series.Add(Series3);
                Series3.ChartType = SeriesChartType.Column;
                Series3.Label = "#VALY";
                Series3.ToolTip = "#VALX";
                Series3["PointWidth"] = "0.5";
                Series3.XValueMember = "name";
                Series3.YValueMembers = "sumcount";
                Series3.LegendText = "NG";
                Series3.Color = Color.Red;
                chartType.Series["Series1"].ChartType = SeriesChartType.Column;
                chartType.Series["Series1"].LegendText = "误判";
                chartType.Series["Series1"].Label = "#VALY";
                chartType.Series["Series1"].ToolTip = "#VALX";
                chartType.Series["Series1"]["PointWidth"] = "0.5";
                ChartArea ChartArea1 = new ChartArea();
                chartType.ChartAreas.Add(ChartArea1);
                //开启三维模式的原因是为了避免标签重叠
                chartType.ChartAreas["ChartArea1"].AxisY.Interval = 300;
                chartType.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;//开启三维模式;PointDepth:厚度BorderWidth:边框宽
                chartType.ChartAreas["ChartArea1"].Area3DStyle.Rotation = 0;//起始角度
                chartType.ChartAreas["ChartArea1"].Area3DStyle.Inclination = 0;//倾斜度(0～90)
                chartType.ChartAreas["ChartArea1"].Area3DStyle.LightStyle = LightStyle.Realistic;//表面光泽度
                chartType.ChartAreas["ChartArea1"].AxisX.Interval = 1; //决定x轴显示文本的间隔，1为强制每个柱状体都显示，3则间隔3个显示
                chartType.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new Font("宋体", 9, FontStyle.Regular);
                chartType.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                chartType.Series[0].XValueMember = "name";
                chartType.Series[0].YValueMembers = "sumcount";
                //ChartArea1.AxisX.w
                ChartArea1.AxisX.Minimum = 0;
                ChartArea1.AxisX.LabelStyle.Angle = 45;
                ChartArea1.AxisX.Maximum = ts.Days + 1;
                if (ts.Days > 30)
                {
                    ChartArea1.AxisX.Maximum = 30;
                }
                ChartArea1.AxisY.Minimum = 0d;
                int x = 0;
                List<string> xName = new List<string>();
                treeView1.Nodes.Clear();
                List<int> vs = new List<int>();
                List<int> dataOK = new List<int>();
                List<int> dataNG = new List<int>();
                treeView2.Nodes.Clear();

                for (int i = 0; i < ts.Days + 1; i++)
                {
                    DateTime dateTime = dateTimePicker1.Value.AddDays(i);
                    dateTimeString = dateTime.ToString("yy年MM月dd日");
                 
                
                    if (File.Exists(textBox1.Text +"计数\\"+ dateTimeString + "计数.txt"))
                    {
                        if (!ProjectINI.ReadPathJsonToCalssEX(textBox1.Text + "计数\\"+ dateTimeString + "计数.txt", out Dictionary<string, OKNumberClass> SPCDicProduct))
                        {
                            SPCDicProduct = new Dictionary<string, OKNumberClass>();
                        }
                        System.Windows.Forms.TreeNode treeNode =    treeView2.Nodes.Add(dateTimeString);
                        treeNode.Tag = SPCDicProduct;
                        foreach (var item in SPCDicProduct)
                        {
                            System.Windows.Forms.TreeNode treeNode1= treeNode.Nodes.Add(item.Key);
                            treeNode1.Tag= item.Value;
                        }
            
           
                        DataDey.Add(dateTimeString);
                        xName.Add(dateTime.ToString("MM月dd日"));
                        int number = 0;
                        int okNu = 0;
                        int ngNu = 0;
                        DesignMode.Add(SPCDicProduct);
                        foreach (var itemdet in SPCDicProduct.Values)
                        {
                            number += itemdet.AutoNGNumber;
                            okNu += itemdet.OKNumber;
                            ngNu += itemdet.NGNumber;
                        }

                        dataOK.Add(okNu);
                        dataNG.Add(ngNu);
                        vs.Add(number);
                    }
                    treeView2.ExpandAll();
                }
                 string [] filesNames= Directory.GetFiles(textBox1.Text );
                for (int i = 0; i < filesNames.Length; i++)
                {
                    if (Path.GetFileName(filesNames[i]).Contains("计数"))
                    {
                        if (ProjectINI.ReadPathJsonToCalssEX(filesNames[i], out OKNumberClass SPCDicProduct))
                        {

                            System.Windows.Forms.TreeNode treeNode = treeView1.Nodes.Add(Path.GetFileName(filesNames[i]));
                            treeNode.Text = Path.GetFileName(filesNames[i]);
                            treeNode.Tag = SPCDicProduct;
                        }
                    }
                }
                chartType.Series["Series1"].Points.DataBindXY(xName, vs);
                chartType.Series["Series2"].Points.DataBindXY(xName, dataOK);
                chartType.Series["Series3"].Points.DataBindXY(xName, dataNG);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox1_DoubleClick(object sender, EventArgs e)
        {
       
                try
                {
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    fbd.Description = "请选择图像文件夹";
                    //fbd.SelectedPath = Vision.Instance.BankPath;
                    if (fbd.SelectedPath == "")
                    {
                        fbd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\";
                    }
                    DialogResult dialog = UI.PropertyGrid.FolderBrowserLauncher.ShowFolderBrowser(fbd);
                    if (dialog == DialogResult.OK)
                    {
                       textBox1.Text= fbd.SelectedPath;


                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
  
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            propertyGrid1.SelectedObject=   e.Node.Tag;
            OKNumberClass SPCDicProduct = propertyGrid1.SelectedObject as OKNumberClass;
            if (SPCDicProduct != null)
            {
                productSPCControl2.RefreshData(SPCDicProduct);
            }

        }

        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                propertyGrid2.SelectedObject = e.Node.Tag;
                OKNumberClass SPCDicProduct = propertyGrid2.SelectedObject as OKNumberClass;
                if (SPCDicProduct!=null)
                {
                    tabPage3.Text = e.Node.Parent.Text;
                    productSPCControl1.DateTimeNew = DateTime.Parse(e.Node.Parent.Text);
                    productSPCControl1.RefreshData(SPCDicProduct);
                }
            }
            catch (Exception)
            {
            }
        }
        private static Dictionary<string, CRDNGTextTime> CrdNgTime { get; set; } = new Dictionary<string, CRDNGTextTime>();
        public static Dictionary<string, CRDNGTextTime> GetCrdNGName()
        {
            return CrdNgTime;
        }
        public static void ReadCrdNGTime()
        {
            try
            {
                string paths = ProjectINI.ProjectPathRun +  "\\CRDNG记录\\CRDNGTime.json";
                if (ProjectINI.ReadPathJsonToCalssEX(paths, out Dictionary<string, CRDNGTextTime> NgTime))
                {
                    //foreach (var item in NgTime)
                    //{
                    //    item.Value.RemoveTime(Vision.Instance.NGCrdNameTimeHH);
                    //}
                    CrdNgTime = NgTime;
                }
                else
                {
                    CrdNgTime = new Dictionary<string, CRDNGTextTime>();
                };
            }
            catch (Exception)
            {
            }
        }
        public static void SaveCrdNGTime()
        {
            string paths = ProjectINI.ProjectPathRun + "\\" 
             + "\\"  + "\\CRDNG记录\\CRDNGTime.json";
            if (ProjectINI.ClassToJsonSave(CrdNgTime, paths))
            {
            }

        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                ReadCrdNGTime();

                Dictionary<string, CRDNGTextTime> keyValuePairs=    GetCrdNGName();
                BindingList<CRDNGTextTime> cRDNGTextTimes=new BindingList<CRDNGTextTime>(keyValuePairs.Values.ToList());
                dataGridView1.DataSource = cRDNGTextTimes;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                SaveCrdNGTime();
            }
            catch (Exception)
            {

            }

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            Dictionary<string, CRDNGTextTime> keyValuePairs = GetCrdNGName();
            BindingList<CRDNGTextTime> cRDNGTextTimes = new BindingList<CRDNGTextTime>(keyValuePairs.Values.ToList());
            dataGridView1.DataSource = cRDNGTextTimes;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                //AddCRDNGTextTime(textBox3.Text, textBox2.Text);
                Dictionary<string, CRDNGTextTime> keyValuePairs = GetCrdNGName();
                BindingList<CRDNGTextTime> cRDNGTextTimes = new BindingList<CRDNGTextTime>(keyValuePairs.Values.ToList());
                dataGridView1.DataSource = cRDNGTextTimes;
            }
            catch (Exception)
            {

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {

                Dictionary<string, CRDNGTextTime> keyValuePairs = GetCrdNGName();
                keyValuePairs.Clear();
                BindingList<CRDNGTextTime> cRDNGTextTimes = new BindingList<CRDNGTextTime>(keyValuePairs.Values.ToList());
                dataGridView1.DataSource = cRDNGTextTimes;

            }
            catch (Exception)
            {
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    CRDNGTextTime cRDNGTextTime = dataGridView1.Rows[e.RowIndex].DataBoundItem as CRDNGTextTime;
                    if (cRDNGTextTime != null)
                    {
                        dataGridView2.Rows.Clear();
                        //groupBox1.Location = dataGridView1.Rows[e.RowIndex].Cells[0].GetContentBounds(e.RowIndex).Location;
                        groupBox1.Visible = true;
                        groupBox1.Text = cRDNGTextTime.Name;
                        dataGridView2.Rows.Add(cRDNGTextTime.NGCont);
                        for (int i = 0; i < cRDNGTextTime.NGCont; i++)
                        {
                            dataGridView2.Rows[i].Cells[1].Value = cRDNGTextTime.NGTime[i];
                            dataGridView2.Rows[i].Cells[0].Value = cRDNGTextTime.NGText[i];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


        MySqlHelperEx mySqlHelperEx = new MySqlHelperEx();
        List<SPCOneDataVale> dataVales = new List<SPCOneDataVale>();
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                button7.Enabled=   button8.Enabled = false;
                Task.Run(()=> {
                    try
                    {
                    DataSet ds;
                    MySqlDataAdapter adapter;
                    MySqlCommandBuilder cb;
                    //mySqlHelperEx.connstr = RecipeCompiler.Instance.SqlContString;
                    string sql = "SELECT * FROM product WHERE DATE(StratTime) BETWEEN '" + dateTimePickerStratTime.Value.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss") + "' AND  '" + dateTimePickerEndTime.Value.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                      if (!checkBoxStratTime.Checked)
                       sql = "SELECT * FROM product  WHERE  ";
                      if (comboBoxTestResult.SelectedItem.ToString() != "NO")
                        sql += " AND TestResult = '" + comboBoxTestResult.SelectedItem.ToString() + "'";

                        if (comboBoxAutoResult.SelectedItem.ToString() != "NO")
                        {
                            if (comboBoxAutoResult.SelectedItem.ToString() == "1=PASS")
                            {
                                sql += " AND AutoResult = 1";
                            }
                            else
                            {
                                sql += " AND AutoResult = 0";
                            }
                        }
                    if (textBoxPName.Text != "")
                        sql += " AND ProductName = '" + textBoxPName.Text + "'";
                    
                    //sql = "SELECT * FROM Product where TO_DAYS(NOW()) - TO_DAYS(StatTime) <= 6";
                    //        sql = "select * from Product where to_days(StratTime) = to_days(now());";
                      System.Diagnostics.Stopwatch WatchOut = new System.Diagnostics.Stopwatch();
                    WatchOut.Start();
                     DataSet dataSetT = mySqlHelperEx.GetDataSet(sql);
                      WatchOut.Stop();
                      this.Text = "SPC  查询时间:"+WatchOut.ElapsedMilliseconds*0.001;
                        WatchOut.Restart();
                        if (dataSetT.Tables.Count == 1)
                       {
                            DataTable dt = dataSetT.Tables[0];
                            dataVales = new List<SPCOneDataVale>();
                            foreach (DataRow item in dt.Rows)
                            {
                                SPCOneDataVale oneDataVale = SPCOneDataVale.ConvertToModel(item);
                                dataVales.Add(oneDataVale);
                            }
                            Task.Run(() => {
                                for (int i = 0; i < dataVales.Count; i++)
                                {
                                   ProjectINI.StringJsonToCalss<OneDataVale>(dataVales[i].CRDTest, out OneDataVale oneDataVale);
                                    dataVales[i].oneDataValeCRD = oneDataVale;
                                    dataVales[i].CRDTest = null;
                                }
                            });
                        this.Invoke(new Action(() => { dataGridView3.DataSource = dataVales; }));
                            WatchOut.Stop();
                            this.Text += "刷新:" + WatchOut.ElapsedMilliseconds * 0.001+"查询到目标"+ dataVales.Count;
                        }
                        else
                        {
                            MessageBox.Show("无数据");
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    button7.Enabled = button8.Enabled = true; 
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds;
                MySqlDataAdapter adapter;
                MySqlCommandBuilder cb;
                string sql = "SELECT * FROM product where SN ='" + textBoxSN.Text + "'";
                DataSet dataSetT = mySqlHelperEx.GetDataSet(sql);
                DataTable dt = dataSetT.Tables[0];
                dataGridView3.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 查看细节ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView3.SelectedCells.Count != 0)
                {
                    //SVisionForm sVisionForm = new SVisionForm();
                    //sVisionForm.Show();
                    //sVisionForm.Finde(dataGridView3.Rows[dataGridView3.SelectedCells[0].RowIndex].Cells[1].Value.ToString());
                    //SPCOneDataVale oneDataVale = dataGridView3.Rows[dataGridView3.SelectedCells[0].RowIndex].DataBoundItem as SPCOneDataVale;
                    //if (oneDataVale != null)
                    //{
                    //    //oneDataVale.oneDataValeCRD
                    //}

                    //dataGridView4.DataSource = new BindingList<OneComponent>(list2.ToList());
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void 导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
            
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Title = "请选择保存路径";      //文件框名称
                                                 //    openFileDialog.Filter = "图片文件|*.jpg;*.tif;*.tiff;*.gif;*.bmp;*.jpg;*.jpeg;*.jp2;*.png;*.pcx;*.pgm;*.ppm;*.pbm;*.xwd;*.ima;*.hobj";
                saveFile.Filter =  "txt|*.txt|json|*.json";   //筛选器

                saveFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                saveFile.FileName=  DateTime.Now.ToString("yyyy年MM月dd日") + "SPC.json";
                //}
                if (saveFile.ShowDialog() != DialogResult.OK) return;
                //弹出对话框
                string path = saveFile.FileName;
                if (path == "") return;    //地址为空返回
        
                    try
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(path));
                        ProjectINI.ClassToJsonSave(dataVales, path );
                    MessageBox.Show("导出成功");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                
            }
            catch (Exception)
            {

            }
        }

        private void 导入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "请选择图片文件可多选";
                openFileDialog.Multiselect = false;
                //openFileDialog.InitialDirectory = Vision.VisionPath + "Image\\";
                openFileDialog.Filter = "json|*.json|txt|*.txt";   //筛选器
                if (openFileDialog.ShowDialog() != DialogResult.OK) return;

                try
                {
                    ProjectINI.ReadPathJsonToCalss(openFileDialog.FileName, out dataVales);
                    dataGridView3.DataSource = dataVales;
                    MessageBox.Show("导入成功");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


            }
            catch (Exception)
            {

            }
        }

        private void 导出CSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                SaveFileDialog openFileDialog = new SaveFileDialog();
                openFileDialog.Filter = "Csv文件|*csv;";
                openFileDialog.FileName = "SPC_" + DateTime.Now.ToString("yyyy年MM月dd日");
                DialogResult dialogResult = openFileDialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    Vision2.ErosProjcetDLL.Excel.Npoi.DataGridViewExportCsv(openFileDialog.FileName, dataGridView3);
                }


            }
            catch (Exception)
            {

            }
        }
        string path = "";
        private void button1FindSN_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    MessageBox.Show("不存在文件夹" + path);
                    return;
                }
                dataGridViewSN.Rows.Clear();
                int ok = 0;
                int Nok = 0;
                int reyt = 0;
                int straight = 0;
                int NGNumber = 0;
                NgNumber = 0;
                OKNumber = 0;
                Number = (0);
                string[] iamgePaths = Directory.GetDirectories(path);
                List<string> PrName = new List<string>();
                dataGridViewImage.Rows.Clear();
                for (int i = 0; i < iamgePaths.Length; i++)
                {
                    if (!iamgePaths[i].Contains(dateTimePicker3.Value.ToString("yyyy年MM月dd日")))
                    {
                        continue;
                    }
                    PrName.Add(Path.GetFileName(iamgePaths[i]));
                    string[] iamgePaths2 = Directory.GetDirectories(iamgePaths[i]);
                    foreach (var item in iamgePaths2)
                    {
                        if (Directory.GetFiles(item).Length == 0)
                        {
                        }
                        string[] difile = Directory.GetDirectories(item);
                        foreach (var item2 in difile)
                        {
                            int indxe = dataGridViewSN.Rows.Add();
                            dataGridViewSN.Rows[indxe].Cells[0].Value = Path.GetFileName(item2);
                            dataGridViewSN.Rows[indxe].Cells[0].Tag = item2;
                            string[] iamgePathsF3 = Directory.GetFiles(item2, "*F.jpg");
                            string[] iamgePathsP3 = Directory.GetFiles(item2, "*P.jpg");
                            var vstring = Directory.GetFiles(item2, "*.txt").TakeWhile(n => n.Contains("Data"));
                            foreach (var item3 in vstring)
                            {
                                string itemString = item3.Replace(".txt", "");
                                dataGridViewSN.Rows[indxe].Cells[4].Value = ProjectINI.GetStrReturnInt(itemString);
                                break;
                            }
                            if (iamgePathsF3.Length == 0 && iamgePathsP3.Length == 0)
                            {straight++;    }
                            else  { NGNumber++; }

                            if (iamgePathsF3.Length == 0 && iamgePathsP3.Length != 0)
                            { Nok++;}
                            if (iamgePathsF3.Length == 0)
                            { ok++;        dataGridViewSN.Rows[indxe].Cells[0].Style.BackColor = Color.Green; }
                            else
                            {   dataGridViewSN.Rows[indxe].Cells[0].Style.BackColor = Color.Red; }
                            dataGridViewSN.Rows[indxe].Cells[2].Value = iamgePathsF3.Length;
                            dataGridViewSN.Rows[indxe].Cells[3].Value = iamgePathsP3.Length;
                            dataGridViewSN.Rows[indxe].Cells[1].Value = Path.GetFileName(item);
                            if (comboBox1.SelectedIndex==1)
                            {
                                UpImageData(item2);
                            }
                        }
                    }
                }
                labelSN.Text = "总数:" + dataGridViewSN.Rows.Count + " Pass:" + straight + " 误判数:" + Nok + " OK数:" + ok + " NG数:" + NGNumber;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridViewSN_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewSN.SelectedCells.Count == 0)
                {
                    return;
                }
                int rowindex = dataGridViewSN.SelectedCells[0].RowIndex;
                if (rowindex >= 0)
                {
                    dataGridViewImageColumn1.Width = 200;
                    string paths = dataGridViewSN.Rows[rowindex].Cells[0].Tag as string;
                    if (paths != null)
                    {
                        if (paths == groupBox1.Text)
                        {
                            return;
                        }
                        dataGridViewImage.Rows.Clear();
                        NgNumber = 0;
                        OKNumber = 0;
                        Number = 0;
                        UpImageData(paths);
                     }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        int NgNumber = 0;
        int OKNumber = 0;
        int Number = 0;

        void UpImageData(string paths)
        {
            try
            {
                groupBox4.Width = 1000;
                int rowindex = 0;
                if (paths != null)
                {
                    int stratId = dataGridViewImage.Rows.Count;
                    string[] Directos = Directory.GetDirectories(paths);
                    List<string> ImageP3 = new List<string>();
                    List<string> ImageF3 = new List<string>();
                    string[] iamgePathsF3 = Directory.GetFiles(paths, "*F.jpg");
                    string[] iamgePathsP3 = Directory.GetFiles(paths, "*P.jpg");
                    if (iamgePathsF3.Length != 0)
                    {
                        dataGridViewImage.Rows.Add(iamgePathsF3.Length);
                    }
                    if (iamgePathsP3.Length != 0)
                    {
                        dataGridViewImage.Rows.Add(iamgePathsP3.Length);
                    }
                    HalconDotNet.HOperatorSet.ReadImage(out HalconDotNet.HObject imagesF, iamgePathsF3);
                    HalconDotNet.HOperatorSet.ReadImage(out HalconDotNet.HObject imagesP, iamgePathsP3);
                    for (int j = 0; j < iamgePathsF3.Length; j++)
                    {
                        string ImageName = Path.GetFileNameWithoutExtension(iamgePathsF3[j]);
                        string[] names = ImageName.Split('_');
                        dataGridViewImage.Rows[j+ stratId].Cells[0].Tag = iamgePathsF3[j];
                        dataGridViewImage.Rows[j+ stratId].Cells[0].Value = names[0];
                        dataGridViewImage.Rows[j+ stratId].Cells[1].Value = names[1];
                        dataGridViewImage.Rows[j+ stratId].Cells[2].Value = HWindID.HObjectToBitmap(imagesF.SelectObj(j + 1));
                        dataGridViewImage.Rows[j+ stratId].Cells[3].Value = names[names.Length - 1];
                        dataGridViewImage.Rows[j+ stratId].Cells[3].Style.BackColor = Color.Red;
                        dataGridViewImage.Rows[j+ stratId].Cells[4].Value = iamgePathsF3[j];
                    }
                    rowindex += iamgePathsF3.Length;
                    for (int j = 0; j < iamgePathsP3.Length; j++)
                    {
                        string ImageName = Path.GetFileNameWithoutExtension(iamgePathsP3[j]);
                        string[] names = ImageName.Split('_');
                        dataGridViewImage.Rows[iamgePathsF3.Length + j+ stratId].Cells[0].Tag = iamgePathsP3[j];
                        dataGridViewImage.Rows[iamgePathsF3.Length + j+ stratId].Cells[0].Value = names[0];
                        dataGridViewImage.Rows[iamgePathsF3.Length + j+ stratId].Cells[1].Value = names[1];
                        dataGridViewImage.Rows[iamgePathsF3.Length + j+ stratId].Cells[2].Value = HWindID.HObjectToBitmap(imagesP.SelectObj(j + 1));
                        dataGridViewImage.Rows[iamgePathsF3.Length + j+ stratId].Cells[3].Value = names[names.Length - 1];
                        dataGridViewImage.Rows[iamgePathsF3.Length + j+ stratId].Cells[3].Style.BackColor = Color.Green;
                        dataGridViewImage.Rows[iamgePathsF3.Length + j+ stratId].Cells[4].Value = iamgePathsP3[j];
                    }
                    rowindex += iamgePathsP3.Length;
                    ImageP3.AddRange(iamgePathsP3);
                    ImageF3.AddRange(iamgePathsF3);
                    NgNumber += ImageF3.Count;
                    OKNumber += ImageP3.Count;
                    Number += (ImageP3.Count + ImageF3.Count);

                    foreach (var item in Directos)
                    {
                        iamgePathsF3 = Directory.GetFiles(item, "*F.jpg");
                        iamgePathsP3 = Directory.GetFiles(item, "*P.jpg");
                        if (iamgePathsF3.Length != 0)
                        {
                            dataGridViewImage.Rows.Add(iamgePathsF3.Length);
                        }
                        if (iamgePathsP3.Length != 0)
                        {
                            dataGridViewImage.Rows.Add(iamgePathsP3.Length);
                        }
                        HalconDotNet.HOperatorSet.ReadImage(out imagesF, iamgePathsF3);
                        HalconDotNet.HOperatorSet.ReadImage(out imagesP, iamgePathsP3);
                        for (int j = 0; j < iamgePathsF3.Length; j++)
                        {
                            string ImageName = Path.GetFileNameWithoutExtension(iamgePathsF3[j]);
                            string[] names = ImageName.Split('_');
                            dataGridViewImage.Rows[rowindex + j].Cells[0].Tag = iamgePathsF3[j];
                            dataGridViewImage.Rows[rowindex + j].Cells[0].Value = names[0];
                            dataGridViewImage.Rows[rowindex + j].Cells[1].Value = names[1];
                            dataGridViewImage.Rows[rowindex + j].Cells[2].Value = HWindID.HObjectToBitmap(imagesF.SelectObj(j + 1));
                            dataGridViewImage.Rows[rowindex + j].Cells[3].Value = names[names.Length - 1];
                            dataGridViewImage.Rows[rowindex + j].Cells[3].Style.BackColor = Color.Red;
                            dataGridViewImage.Rows[rowindex + j].Cells[4].Value = iamgePathsF3[j];
                        }
                        rowindex += iamgePathsF3.Length;
                        for (int j = 0; j < iamgePathsP3.Length; j++)
                        {
                            string ImageName = Path.GetFileNameWithoutExtension(iamgePathsP3[j]);
                            string[] names = ImageName.Split('_');
                            dataGridViewImage.Rows[rowindex + j].Cells[0].Tag = iamgePathsP3[j];
                            dataGridViewImage.Rows[rowindex + j].Cells[0].Value = names[0];
                            dataGridViewImage.Rows[rowindex + j].Cells[1].Value = names[1];
                            dataGridViewImage.Rows[rowindex + j].Cells[2].Value = HWindID.HObjectToBitmap(imagesP.SelectObj(j + 1));
                            dataGridViewImage.Rows[rowindex + j].Cells[3].Value = names[names.Length - 1];
                            dataGridViewImage.Rows[rowindex + j].Cells[3].Style.BackColor = Color.Green;
                            dataGridViewImage.Rows[rowindex + j].Cells[4].Value = iamgePathsF3[j];
                        }
                        rowindex += iamgePathsP3.Length;
                        ImageP3.AddRange(iamgePathsP3);
                        ImageF3.AddRange(iamgePathsF3);
                    }
                    label6.Text = "元件检出数:" + (Number) + "  误判数:" + OKNumber + "  真实检出:" + NgNumber + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void textBox4_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "请选择文件夹";
                string saveImagePath = "";
                saveImagePath = path;
                fbd.SelectedPath = saveImagePath;
                DialogResult dialog = UI.PropertyGrid.FolderBrowserLauncher.ShowFolderBrowser(fbd);
                if (dialog == DialogResult.OK)
                {
                    path = fbd.SelectedPath;
                    textBox4.Text = path;
                }
            }
            catch (Exception)
            {
            }
        }

        private void comboBox1_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedIndex==1)
                {

                }
                else
                {


                }


            }
            catch (Exception)
            {

            }
        }

        private void dataGridView3_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            try
            {

                SPCOneDataVale original_Navigation = this.dataGridView3.Rows[e.RowIndex].DataBoundItem as SPCOneDataVale;
                Color color = Color.Green;
                if (original_Navigation.TestResult=="Pass")
                {
                    color = Color.Green;
                }
                else
                {
                    color = Color.Red;
                }
                if (this.dataGridView3.Rows[e.RowIndex].Cells[5].Style.BackColor != color)
                {
                    this.dataGridView3.Rows[e.RowIndex].Cells[5].Style.BackColor = color;
                }
                if (original_Navigation.oneDataValeCRD==null)
                {
                    this.dataGridView3.Rows[e.RowIndex].Cells[0].Style.BackColor = Color.Red;
                }
                else
                {
                    this.dataGridView3.Rows[e.RowIndex].Cells[0].Style.BackColor = Color.White;
                }


            }
            catch (Exception)
            {
            }
        }

        private void dataGridView3_SelectionChanged(object sender, EventArgs e)
        {
            try
            {

                SPCOneDataVale oneDataVale = dataGridView3.Rows[dataGridView3.SelectedCells[0].RowIndex].DataBoundItem as SPCOneDataVale;
                if (oneDataVale!=null)
                {
                    List<OneCompOBJs.OneComponent> oneComponents = new List<OneCompOBJs.OneComponent>();

                    var list = from n in oneDataVale.oneDataValeCRD.ListCamsData
                               from t in n.Value.DicNGObj.DicOnes
                               orderby !t.Value.aOK descending
                               select t.Value;
                    oneComponents.AddRange(list.ToList());
                    dataGridView4.DataSource = oneComponents;
                    label13.Text = "SN:" + oneDataVale.SN+" NG数量:"+oneDataVale.NGNumber+Environment.NewLine;

                    //oneDataVale.oneDataValeCRD
                }
                else
                {
                    dataGridView4.DataSource = null;
                }
            }
            catch (Exception)
            {
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            int num = 0;
            try
            {
                List<SPCOneDataVale> dataV = new List<SPCOneDataVale>();
                var datas = from n in dataVales where n.NGCRD!=null
                            where n.NGCRD.Contains(textBox5.Text)
                            select n;
                foreach (var item in datas)
                {
                    num++;
                    dataV.Add(item);
                }
                this.Invoke(new Action(() => { dataGridView3.DataSource = dataV; }));
                this.Text = "SPC:"+ "查询到目标:" + dataV.Count;
                SPCOneDataVale vale = dataVales[num];
            }
            catch (Exception ex)
            {
            }
      
        }

        private void 查看图片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SPCOneDataVale original_Navigation = this.dataGridView3.Rows[dataGridView3.SelectedCells[0].RowIndex].DataBoundItem as SPCOneDataVale;
 
                //string []paths=  Directory.GetFiles(original_Navigation.oneDataValeCRD.ImagePath2);
                if (File.Exists(original_Navigation.oneDataValeCRD.ImagePath2))
                {
                    HalconDotNet.HOperatorSet.ReadImage(out HalconDotNet.HObject image, original_Navigation.oneDataValeCRD.ImagePath2);
                    foreach (var item in original_Navigation.oneDataValeCRD.ListCamsData["上相机"].DicNGObj.DicOnes)
                    {
                        if (!item.Value.aOK)
                        {
                            for (int i = 0; i < item.Value.oneRObjs.Count; i++)
                            {
                                if (item.Value.oneRObjs[0].Row > 0)
                                {
                                    HalconDotNet.HOperatorSet.GenRectangle2(out HalconDotNet.HObject hObject, item.Value.oneRObjs[i].Row, item.Value.oneRObjs[i].Col,
                                        new HalconDotNet.HTuple(-item.Value.oneRObjs[i].Anlge).TupleRad(), item.Value.oneRObjs[i].Length1, item.Value.oneRObjs[i].Length2);
                                    item.Value.oneRObjs[i].ROI(hObject);
                                    break;
                                }
                            }
                        }
                    }
  
                }
            }
            catch (Exception)
            {
            }
        }

        private void 打开文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(dataGridViewSN.Rows[dataGridViewSN.SelectedCells[0].RowIndex].Cells[0].Tag.ToString());
            }
            catch (Exception)
            {
            }
        }


        /// <summary>
        /// 元件计数
        /// </summary>
         Dictionary<string, ComponentNumber> ComponentS { get; set; } = new Dictionary<string, ComponentNumber>();
        /// <summary>
        /// 元件计数
        /// </summary>
        Dictionary<string, DefectType> DefectTypeS { get; set; } = new Dictionary<string, DefectType>();
 
        List<SpcData> spcDa = new List<SpcData>();
        List<SpcData2> spcDa_1 = new List<SpcData2>();

        DateTime Strattime = new DateTime();
        DateTime EndTime = new DateTime();


        private async void buttonConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedDatabase))
            {
                MessageBox.Show("Please select a database.");
                return;
            }

            try
            {
                // 清空数据和图表
                //ClearDataAndCharts();

                string serverconnectionString = connectionString + "database=" + selectedDatabase + ";";
                using (MySqlConnection conn = new MySqlConnection(serverconnectionString))
                {
                    await conn.OpenAsync(); // 使用异步打开连接
                    string query = "SELECT * FROM csv_mysql"; // 优化查询，仅选择需要的列
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                    {
                        DataTable dataTable = new DataTable();
                        await Task.Run(() => adapter.Fill(dataTable)); // 异步填充数据表
                        dataGridView1.DataSource = dataTable;

                        // 将数据表转换为List<string>
                        List<string> text = new List<string>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string line = string.Join(",", row.ItemArray);
                            text.Add(line);
                        }

                        // 清空数据
                        ComponentS.Clear();
                        DefectTypeS.Clear();
                        spcDa_1.Clear();

                        // 处理数据
                        ProcessData(text);

                        // 在UI线程更新数据源和图表
                        this.Invoke(new Action(() =>
                        {
                            dataGridView5.DataSource = new BindingList<SpcData2>(spcDa_1);
                            //UpdateCharts(); // 更新图表方法
                            UpdateChart();
                            UpdateChart2();
                            textBox_Countlables(sender, e); // 调用更新最大数量显示的方法

                        }));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to database: " + ex.Message);
            }
        }

        private void ProcessData(List<string> text)
        {
            int deyNumber = 结束日期.Value.Subtract(开始日期.Value).Days + 1;
            Strattime = DateTime.Parse(开始日期.Value.ToString("yyyy-MM-dd ") + 开始时间.Value.ToString("HH:mm:ss"));
            EndTime = DateTime.Parse(结束日期.Value.ToString("yyyy-MM-dd ") + 结束时间.Value.ToString("HH:mm:ss"));

            foreach (string line in text)
            {
                string[] datas = line.Split(',');
                SpcData2 spcData_1 = new SpcData2(datas, toolStripTextBox2.Text);

                // 添加到数据列表
                spcDa_1.Add(spcData_1);

                // 处理组件数据
                if (!ComponentS.ContainsKey(spcData_1.Name))
                {
                    ComponentS.Add(spcData_1.Name, new ComponentNumber() { Nmae = spcData_1.Name, BankNmae = spcData_1.Material_no });
                }
                if (spcData_1.OK)
                {
                    ComponentS[spcData_1.Name].FalseCall++;
                    ComponentS[spcData_1.Name].OKNumber++;
                }
                else
                {
                    ComponentS[spcData_1.Name].NGNumber++;
                }

                // 处理缺陷类型数据
                if (spcData_1.AutoOk != null)
                {
                    string[] data = spcData_1.AutoOk.Split(';');
                    for (int j = 0; j < data.Length; j++)
                    {
                        if (data[j] != "OK")
                        {
                            if (!DefectTypeS.ContainsKey(data[0]))
                            {
                                DefectTypeS.Add(data[0], new DefectType() { Name = data[0], Defect_Type = spcData_1.Result, Number = 1 });
                            }
                            else
                            {
                                DefectTypeS[data[0]].Number++;
                            }
                            break;
                        }
                    }
                }
            }
        }


        private async void button10_Click(object sender, EventArgs e)
        {
            try
            {


                ComponentS.Clear();
                DefectTypeS.Clear();
                dataGridView5.AutoGenerateColumns = false;
                int deyNumber = 结束日期.Value.Subtract(开始日期.Value).Days + 1;
                spcDa.Clear();
                SeletMChecked.Clear();
                SeletMNameChecked.Clear();
                Strattime = DateTime.Parse(开始日期.Value.ToString("yyyy-MM-dd ") + 开始时间.Value.ToString("HH:mm:ss"));
                EndTime = DateTime.Parse(结束日期.Value.ToString("yyyy-MM-dd ") + 结束时间.Value.ToString("HH:mm:ss"));

                await Task.Run(() =>
                {
                    for (int d = 0; deyNumber > 0 && d < deyNumber; d++)
                    {
                        ErosProjcetDLL.Excel.Npoi.ReadText(toolStripTextBox1.Text + "\\" + Strattime.AddDays(d).ToString("yyyyMMdd") + ".CSV", out List<string> text);
                        for (int i = 1; i < text.Count; i++)
                        {
                            string[] datas = text[i].Split(',');

                            SpcData spcData = new SpcData(datas, toolStripTextBox2.Text);
                            if (Strattime.AddDays(d).ToString("yyyyMMdd") == EndTime.ToString("yyyyMMdd"))
                            {
                                DateTime Start_DataTime = DateTime.Parse(datas[6] + " " + datas[7]);
                                if (Start_DataTime.Subtract(EndTime).TotalMinutes < 0 && (checkBox3.Checked || !spcData.OK) && (checkBox4.Checked || spcData.OK))
                                    lock (spcDa)
                                    {
                                        spcDa.Add(spcData);
                                    }
                                else continue;
                            }
                            if (d == 0)
                            {
                                DateTime Start_DataTime = DateTime.Parse(datas[6] + " " + datas[7]);
                                if (Start_DataTime.Subtract(Strattime).TotalMinutes > 0 && (checkBox3.Checked || !spcData.OK) && (checkBox4.Checked || spcData.OK))
                                    lock (spcDa)
                                    {
                                        spcDa.Add(spcData);
                                    }
                                else
                                    continue;
                            }
                            else if ((checkBox3.Checked || !spcData.OK) && (checkBox4.Checked || spcData.OK))
                                lock (spcDa)
                                {
                                    spcDa.Add(spcData);
                                }

                            lock (SeletMChecked)
                            {
                                if (!SeletMChecked.ContainsKey(spcData.Name))
                                    SeletMChecked.Add(spcData.Name, true);
                            }

                            lock (SeletMNameChecked)
                            {
                                if (!SeletMNameChecked.ContainsKey(spcData.Material_no))
                                    SeletMNameChecked.Add(spcData.Material_no, true);
                            }

                            if (spcData.AutoOk != null)
                            {
                                string[] data = spcData.AutoOk.Split(';');
                                for (int j = 0; j < data.Length; j++)
                                {
                                    if (data[j] != "OK")
                                    {
                                        lock (DefectTypeS)
                                        {
                                            if (!DefectTypeS.ContainsKey(data[0]))
                                                DefectTypeS.Add(data[0], new DefectType() { Name = data[0], Defect_Type = spcData.Result, Number = 1 });
                                            else
                                                DefectTypeS[data[0]].Number++;
                                        }
                                        break;
                                    }
                                }
                            }

                            lock (ComponentS)
                            {
                                if (!ComponentS.ContainsKey(spcData.Name))
                                    ComponentS.Add(spcData.Name, new ComponentNumber() { Nmae = spcData.Name, BankNmae = spcData.Material_no });
                                if (spcData.OK)
                                {
                                    ComponentS[spcData.Name].FalseCall++;
                                    ComponentS[spcData.Name].OKNumber++;
                                }
                                else ComponentS[spcData.Name].NGNumber++;
                            }
                        }
                    }
                });

                // 在UI线程更新数据源和图表
                this.Invoke(new Action(() =>
                {
                    dataGridView5.DataSource = new BindingList<SpcData>(spcDa);
                    //UpdateCharts(); // 更新图表方法
                    UpdateChart(); // 更新图表方法
                    UpdateChart2();
                    textBox_Countlables(sender, e); // 调用更新最大数量显示的方法
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }






        //修改后chart3
        public void UpSpcColumn2(Chart chart, Dictionary<string, ComponentNumber> keyValuePairs, int dataCount)
        {
            try
            {
                chart.Visible = true;
                chart.Series.Clear();
                chart.ChartAreas.Clear();
                chart.Titles.Clear();

                Series series1 = new Series
                {
                    ChartType = SeriesChartType.Column,
                    Label = "#VALY",
                    ToolTip = "#VALX",
                    ["PointWidth"] = "0.5",
                    ["PieLabelStyle"] = "Outside", // 标签位置
                    MarkerSize = 3
                };
                chart.Series.Add(series1);

                ChartArea chartArea1 = new ChartArea
                {
                    AxisY = { Interval = 50 },
                    Area3DStyle =
            {
                Enable3D = true,
                Rotation = 20,
                Inclination = 60,
                LightStyle = LightStyle.Realistic
            },
                    AxisX =
            {
                Interval = 1,
                LabelStyle = { Font = new Font("宋体", 9, FontStyle.Regular) },
                MajorGrid = { Enabled = false },
                Minimum = 0,
                Maximum = dataCount + 1
            }
                };
                chart.ChartAreas.Add(chartArea1);

                var sortedData = keyValuePairs.OrderByDescending(kvp => kvp.Value.Cont).Take(dataCount);
                foreach (var item in sortedData)
                {
                    series1.Points.AddXY(item.Key, item.Value.Cont);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }




        public void UpSpcPie2(Chart chart, Dictionary<string, ComponentNumber> keyValuePairs, int dataCount)
        {
            try
            {
                // 清除之前的图表数据和设置
                chart.Visible = true;
                chart.Series.Clear();
                chart.ChartAreas.Clear();
                chart.Titles.Clear();

                // 创建一个新的饼图系列
                Series series1 = new Series
                {
                    ChartType = SeriesChartType.Pie, // 图表类型为饼图
                    Label = "#VALX:#PERCENT", // 饼图标签显示格式
                    ToolTip = "#VALY", // 鼠标悬停时的提示信息
                    ["PointWidth"] = "0.5", // 饼图点的宽度
                    ["PieLabelStyle"] = "Outside" // 标签位置
                };

                // 将系列添加到图表中
                chart.Series.Add(series1);

                // 创建一个新的图表区域
                ChartArea chartArea1 = new ChartArea
                {
                    // 设置 X 轴的属性
                    AxisX =
            {
                Interval = 1, // X 轴标签间隔
                Minimum = 0, // X 轴最小值
                Maximum = dataCount, // X 轴最大值为指定的数据个数
                LabelStyle = { Font = new Font("宋体", 9, FontStyle.Regular) }, // X 轴标签字体
                MajorGrid = { Enabled = false } // 不显示主要网格线
            },
                    // 设置 Y 轴的属性
                    AxisY = { Interval = 10 }, // Y 轴标签间隔
                                               // 设置 3D 效果属性
                    Area3DStyle = { Enable3D = true, Rotation = 20, Inclination = 40, LightStyle = LightStyle.Realistic }
                };

                // 将图表区域添加到图表中
                chart.ChartAreas.Add(chartArea1);

                // 对键值对按值进行排序，并只取前 dataCount 个
                var sortedData = keyValuePairs.OrderByDescending(kvp => kvp.Value.Cont).Take(dataCount);

                // 将排序后的数据添加到图表系列中
                foreach (var item in sortedData)
                {
                    series1.Points.AddXY(item.Key, item.Value.Cont);
                }
            }
            catch (Exception ex)
            {
                // 处理异常
                MessageBox.Show("Error: " + ex.Message);
            }
        }



        public void UpSpcPieDefectType(Chart chart, Dictionary<string, DefectType> keyValuePairs, int dataCount)
        {
            try
            {
                chart.Visible = true;
                chart.Series.Clear();
                chart.ChartAreas.Clear();
                chart.Titles.Clear();

                Series series1 = new Series
                {
                    ChartType = SeriesChartType.Pie,
                    Label = "#VALX:#PERCENT",
                    ToolTip = "#VALY",
                    ["PointWidth"] = "0.5",
                    ["PieLabelStyle"] = "Outside"
                };
                chart.Series.Add(series1);

                ChartArea chartArea1 = new ChartArea
                {
                    AxisY = { Interval = 10 },
                    Area3DStyle =
            {
                Enable3D = true,
                Rotation = 20,
                Inclination = 40,
                LightStyle = LightStyle.Realistic
            },
                    AxisX =
            {
                Interval = 1,
                LabelStyle = { Font = new Font("宋体", 9, FontStyle.Regular) },
                MajorGrid = { Enabled = false },
                Minimum = 0,
                Maximum = dataCount + 1
            }
                };
                chart.ChartAreas.Add(chartArea1);

                var sortedData = keyValuePairs.OrderByDescending(kvp => kvp.Value.Number).Take(dataCount);
                foreach (var item in sortedData)
                {
                    series1.Points.AddXY(item.Key, item.Value.Number);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public void UPspcDefectType(Chart chart, Dictionary<string, DefectType> keyValuePairs, int dataCount)
        {
            try
            {
                chart.Visible = true;
                chart.Series.Clear();
                chart.ChartAreas.Clear();
                chart.Titles.Clear();

                Series series1 = new Series
                {
                    ChartType = SeriesChartType.Column,
                    Label = "#VALY",
                    ToolTip = "#VALX",
                    ["PointWidth"] = "0.5",
                    MarkerSize = 3
                };
                chart.Series.Add(series1);

                ChartArea chartArea1 = new ChartArea
                {
                    AxisY = { Interval = 50 },
                    Area3DStyle =
            {
                Enable3D = true,
                Rotation = 20,
                Inclination = 60,
                LightStyle = LightStyle.Realistic
            },
                    AxisX =
            {
                Interval = 1,
                LabelStyle = { Font = new Font("宋体", 9, FontStyle.Regular) },
                MajorGrid = { Enabled = false },
                Minimum = 0,
                Maximum = dataCount + 1
            }
                };
                chart.ChartAreas.Add(chartArea1);

                var sortedData = keyValuePairs.OrderByDescending(kvp => kvp.Value.Number).Take(dataCount);
                foreach (var item in sortedData)
                {
                    series1.Points.AddXY(item.Key, item.Value.Number);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        //private void button10_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ComponentS.Clear();
        //        DefectTypeS.Clear();
        //        dataGridView5.AutoGenerateColumns = false;
        //        int deyNumber= 结束日期.Value .Subtract( 开始日期.Value).Days+1;
        //        spcDa.Clear();
        //        SeletMChecked.Clear();
        //        SeletMNameChecked.Clear();
        //        Strattime = DateTime.Parse(开始日期.Value.ToString("yyyy-MM-dd ") + 开始时间.Value.ToString("HH:mm:ss"));
        //        EndTime = DateTime.Parse(结束日期.Value.ToString("yyyy-MM-dd ") + 结束时间.Value.ToString("HH:mm:ss"));
        //        for (int d = 0; d < deyNumber; d++)
        //        {
        //            ErosProjcetDLL.Excel.Npoi.ReadText(toolStripTextBox1.Text + "\\CRD_SPC数据\\" + Strattime.AddDays(d).ToString("yyyyMMdd") + ".CSV", out List<string> text);
        //            for (int i = 1; i < text.Count; i++)
        //            {
        //                string[] datas = text[i].Split(',');

        //                SpcData spcData = new SpcData(datas, toolStripTextBox2.Text);
        //                if (Strattime.AddDays(d).ToString("yyyyMMdd")==EndTime.ToString("yyyyMMdd"))
        //                {
        //                    DateTime Start_DataTime = DateTime.Parse(datas[6] + " " + datas[7]);
        //                    if (Start_DataTime.Subtract(EndTime).TotalMinutes < 0&& (checkBox3.Checked||!spcData.OK )&& (checkBox4.Checked || spcData.OK))
        //                        spcDa.Add(spcData);
        //                    else continue;
        //                }
        //                if (d==0)
        //                {
        //                    DateTime Start_DataTime = DateTime.Parse(datas[6] + " " + datas[7]);
        //                    if (Start_DataTime.Subtract(Strattime).TotalMinutes>0 && (checkBox3.Checked || !spcData.OK) && (checkBox4.Checked || spcData.OK))
        //                        spcDa.Add(spcData);
        //                    else
        //                        continue;
        //                }
        //                else if((checkBox3.Checked || !spcData.OK) && (checkBox4.Checked || spcData.OK))    spcDa.Add(spcData);

        //                if (!SeletMChecked.ContainsKey(spcData.Name))
        //                    SeletMChecked.Add(spcData.Name, true);

        //                if (!SeletMNameChecked.ContainsKey(spcData.Material_no))
        //                    SeletMNameChecked.Add(spcData.Material_no, true);
        //                if (spcData.AutoOk!=null)
        //                {
        //                    string[] data = spcData.AutoOk.Split(';');
        //                    for (int j = 0; j < data.Length; j++)
        //                    {
        //                        if (data[j] != "OK")
        //                        {
        //                            if (!DefectTypeS.ContainsKey(data[0]))
        //                                DefectTypeS.Add(data[0], new DefectType() { Name = data[0], Defect_Type = spcData.Result });

        //                            DefectTypeS[data[0]].Number++;
        //                            break;
        //                        }
        //                    }
        //                }



        //                if (!ComponentS.ContainsKey(spcData.Name))
        //                    ComponentS.Add(spcData.Name, new ComponentNumber() { Nmae= spcData.Name , BankNmae = spcData.Material_no });
        //                if (spcData.OK)
        //                {
        //                    ComponentS[spcData.Name].FalseCall++;
        //                    ComponentS[spcData.Name].OKNumber++;
        //                }
        //                else ComponentS[spcData.Name].NGNumber++;

        //            }
        //        }

        //        dataGridView5.DataSource= new BindingList<SpcData>( spcDa);
        //        UpSpcColumn(chart3, ComponentS);
        //        UpSpcPie(chart2, ComponentS);
        //        UPspcDefectType(chart4, this.DefectTypeS);
        //        UpSpcPieDefectType(chart5, this.DefectTypeS);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}


        //public void UpSpcColumn(Chart chart, Dictionary<string, ComponentNumber>  keyValuePairs)
        //{
        //    try
        //    {
        //        chart.Visible = true;
        //        chart.Series.Clear();
        //        chart.ChartAreas.Clear();
        //        chart.Titles.Clear();
        //        Series Series1 = new Series();
        //        chart.Series.Add(Series1);
        //        chart.Series["Series1"].ChartType = SeriesChartType.Column;
        //        ChartArea ChartArea1 = new ChartArea();
        //        chart.ChartAreas.Add(ChartArea1);
        //        //开启三维模式的原因是为了避免标签重叠
        //        chart.ChartAreas["ChartArea1"].AxisY.Interval = 50;
        //        chart.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;//开启三维模式;PointDepth:厚度BorderWidth:边框宽
        //        chart.ChartAreas["ChartArea1"].Area3DStyle.Rotation = 20;//起始角度
        //        chart.ChartAreas["ChartArea1"].Area3DStyle.Inclination = 60;//倾斜度(0～90)
        //        chart.ChartAreas["ChartArea1"].Area3DStyle.LightStyle = LightStyle.Realistic;//表面光泽度
        //        chart.ChartAreas["ChartArea1"].AxisX.Interval =1; //决定x轴显示文本的间隔，1为强制每个柱状体都显示，3则间隔3个显示
        //        chart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new Font("宋体", 9, FontStyle.Regular);
        //        chart.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
        //        chart.Series[0].XValueMember = "name";
        //        chart.Series[0].YValueMembers = "sumcount";
        //        //ChartArea1.AxisX.w
        //        ChartArea1.AxisX.Minimum = 0;
        //        ChartArea1.AxisX.Maximum = keyValuePairs.Count+1;
        //        chart.Series["Series1"].Label = "#VALY";
        //        chart.Series["Series1"].ToolTip = "#VALX";
        //        chart.Series["Series1"]["PointWidth"] = "0.5";
        //        chart.Series["Series1"].Points.Clear();
        //        chart.Series["Series1"]["PieLabelStyle"] = "Outside";//标签位置
        //        chart.Series["Series1"].MarkerSize = 3;
        //       var vatdata   =  from n in keyValuePairs
        //        orderby n.Value.Cont descending
        //        select n;
        //        foreach (var item in vatdata)
        //        {
        //            chart.Series["Series1"].Points.AddXY(item.Key, item.Value.Cont);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        //public void UpSpcPie(Chart chart, Dictionary<string, ComponentNumber> keyValuePairs)
        //{
        //    try
        //    {
        //        chart.Visible = true;
        //        chart.Series.Clear();
        //        chart.ChartAreas.Clear();
        //        chart.Titles.Clear();
        //        Series Series1 = new Series();
        //        chart.Series.Add(Series1);
        //        chart.Series["Series1"].ChartType = SeriesChartType.Pie;
        //        ChartArea ChartArea1 = new ChartArea();
        //        chart.ChartAreas.Add(ChartArea1);
        //        //开启三维模式的原因是为了避免标签重叠
        //        chart.ChartAreas["ChartArea1"].AxisY.Interval = 10;
        //        chart.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;//开启三维模式;PointDepth:厚度BorderWidth:边框宽
        //        chart.ChartAreas["ChartArea1"].Area3DStyle.Rotation = 20;//起始角度
        //        chart.ChartAreas["ChartArea1"].Area3DStyle.Inclination = 40;//倾斜度(0～90)
        //        chart.ChartAreas["ChartArea1"].Area3DStyle.LightStyle = LightStyle.Realistic;//表面光泽度
        //        chart.ChartAreas["ChartArea1"].AxisX.Interval = 1; //决定x轴显示文本的间隔，1为强制每个柱状体都显示，3则间隔3个显示
        //        chart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new Font("宋体", 9, FontStyle.Regular);
        //        chart.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
        //        chart.Series[0].XValueMember = "name";
        //        chart.Series[0].YValueMembers = "sumcount";
        //        //ChartArea1.AxisX.w
        //        ChartArea1.AxisX.Minimum = 0;
        //        ChartArea1.AxisX.Maximum = keyValuePairs.Count + 1;
        //        chart.Series["Series1"].Label = "#VALX:#PERCENT";
        //        chart.Series["Series1"].ToolTip = "#VALY";
        //        chart.Series["Series1"]["PointWidth"] = "0.5";
        //        chart.Series["Series1"]["PieLabelStyle"] = "Outside";//标签位置
        //        chart.Series["Series1"].Points.Clear();
        //        var vatdata = from n in keyValuePairs
        //                      orderby n.Value.Cont descending
        //                      select n;
        //        foreach (var item in vatdata)
        //        {
        //            chart.Series["Series1"].Points.AddXY(item.Key, item.Value.Cont);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        //public void UpSpcPieDefectType(Chart chart, Dictionary<string, DefectType> keyValuePairs)
        //{
        //    try
        //    {
        //        chart.Visible = true;

        //        chart.Series.Clear();
        //        chart.ChartAreas.Clear();
        //        chart.Titles.Clear();
        //        Series Series1 = new Series();
        //        chart.Series.Add(Series1);
        //        chart.Series["Series1"].ChartType = SeriesChartType.Pie;

        //         ChartArea ChartArea1 = new ChartArea();
        //        chart.ChartAreas.Add(ChartArea1);
        //        //开启三维模式的原因是为了避免标签重叠
        //        chart.ChartAreas["ChartArea1"].AxisY.Interval = 10;
        //        chart.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;//开启三维模式;PointDepth:厚度BorderWidth:边框宽
        //        chart.ChartAreas["ChartArea1"].Area3DStyle.Rotation = 20;//起始角度
        //        chart.ChartAreas["ChartArea1"].Area3DStyle.Inclination = 40;//倾斜度(0～90)
        //        chart.ChartAreas["ChartArea1"].Area3DStyle.LightStyle = LightStyle.Realistic;//表面光泽度
        //        chart.ChartAreas["ChartArea1"].AxisX.Interval = 1; //决定x轴显示文本的间隔，1为强制每个柱状体都显示，3则间隔3个显示
        //        chart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new Font("宋体", 9, FontStyle.Regular);
        //        chart.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
        //        chart.Series[0].XValueMember = "name";
        //        chart.Series[0].YValueMembers = "sumcount";
        //        //ChartArea1.AxisX.w
        //        ChartArea1.AxisX.Minimum = 0;
        //        ChartArea1.AxisX.Maximum = keyValuePairs.Count + 1;
        //        chart.Series["Series1"].Label = "#VALX:#PERCENT";
        //        chart.Series["Series1"].ToolTip = "#VALY";
        //        chart.Series["Series1"]["PointWidth"] = "0.5";
        //        chart.Series["Series1"]["PieLabelStyle"] = "Outside";//标签位置
        //        chart.Series["Series1"].Points.Clear();
        //        var vatdata = from n in keyValuePairs
        //                      orderby n.Value.Number descending
        //                      select n;
        //        foreach (var item in vatdata)
        //        {
        //            chart.Series["Series1"].Points.AddXY(item.Key, item.Value.Number);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        //public void UPspcDefectType(Chart chart, Dictionary<string, DefectType>  keyValuePairs)
        //{
        //    try
        //    {
        //        chart.Visible = true;
        //        chart.Series.Clear();
        //        chart.ChartAreas.Clear();
        //        chart.Titles.Clear();
        //        Series Series1 = new Series();
        //        chart.Series.Add(Series1);
        //        chart.Series["Series1"].ChartType = SeriesChartType.Column;
        //        ChartArea ChartArea1 = new ChartArea();
        //        chart.ChartAreas.Add(ChartArea1);
        //        //开启三维模式的原因是为了避免标签重叠
        //        chart.ChartAreas["ChartArea1"].AxisY.Interval = 50;
        //        chart.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;//开启三维模式;PointDepth:厚度BorderWidth:边框宽
        //        chart.ChartAreas["ChartArea1"].Area3DStyle.Rotation = 20;//起始角度
        //        chart.ChartAreas["ChartArea1"].Area3DStyle.Inclination = 60;//倾斜度(0～90)
        //        chart.ChartAreas["ChartArea1"].Area3DStyle.LightStyle = LightStyle.Realistic;//表面光泽度
        //        chart.ChartAreas["ChartArea1"].AxisX.Interval = 1; //决定x轴显示文本的间隔，1为强制每个柱状体都显示，3则间隔3个显示
        //        chart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new Font("宋体", 9, FontStyle.Regular);
        //        chart.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
        //        chart.Series[0].XValueMember = "name";
        //        chart.Series[0].YValueMembers = "sumcount";
        //        //ChartArea1.AxisX.w
        //        ChartArea1.AxisX.Minimum = 0;
        //        ChartArea1.AxisX.Maximum = keyValuePairs.Count + 1;
        //        chart.Series["Series1"].Label = "#VALY";
        //        chart.Series["Series1"].ToolTip = "#VALX";
        //        chart.Series["Series1"]["PointWidth"] = "0.5";
        //        chart.Series["Series1"].Points.Clear();
        //        chart.Series["Series1"]["PieLabelStyle"] = "Outside";//标签位置
        //        chart.Series["Series1"].MarkerSize = 3;
        //        var vatdata = from n in keyValuePairs
        //                      orderby n.Value.Number descending
        //                      select n;
        //        foreach (var item in vatdata)
        //        {
        //            chart.Series["Series1"].Points.AddXY(item.Key, item.Value.Number);
        //        }


        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        private void dataGridView5_SelectionChanged(object sender, EventArgs e)
        {
            try
            {


                SpcData spcData = dataGridView5.Rows[dataGridView5.SelectedCells[0].RowIndex].DataBoundItem as SpcData;

                if (spcData != null)
                {
                    if (File.Exists(spcData.ImagePath))
                    {
                        HOperatorSet.ReadImage(out HObject image, spcData.ImagePath);
                        hSmartWindowControl1.SetFullImagePart(null);
                        hSmartWindowControl1.HalconWindow.DispObj(image);
                        HTuple mesage = new HTuple();
                        mesage.Append(spcData.Name);
                        mesage.Append(spcData.AutoOk);
                        mesage.Append(spcData.Result);
                        hSmartWindowControl1.HalconWindow.DispText(mesage, "window", new HTuple(0), new HTuple(0), new HTuple("red"), new HTuple("box"), new HTuple("false"));
                        hSmartWindowControl1.SetFullImagePart(null);
                    }
                    else
                    {
                        //hSmartWindowControl1.SetFullImagePart(null);
                        //hSmartWindowControl1.HalconWindow.ClearWindow();
                        return;
                        string dataPaths = toolStripTextBox2.Text + "\\" +
                           spcData.Start_DataTime.ToString("yyyy年MM月dd日") + "\\" + spcData.ProductName + "\\" + spcData.SN;
                        string imageFile = spcData.Material_no + "_" + spcData.Name + "_" + spcData.Location + "_" + spcData.CamName + "_" +
                              DateTime.Parse(spcData.Start_Time).ToString("HHmmss");
                        if (Directory.Exists(dataPaths))
                        {
                            string paths = dataPaths + "\\" + imageFile + "_";
                            if (spcData.Result == "OK")
                                paths += "P.jpg";
                            else paths += "F.jpg";
                            if (!File.Exists(paths))
                            {
                                string[] imagepahts = Directory.GetFiles(dataPaths);
                                paths = imagepahts.First(n => n.Contains(spcData.Name));
                            }
                            HOperatorSet.ReadImage(out HObject image, paths);
                            hSmartWindowControl1.SetFullImagePart(null);
                            hSmartWindowControl1.HalconWindow.DispObj(image);
                            HTuple mesage = new HTuple();
                            mesage.Append(spcData.Name);
                            mesage.Append(spcData.AutoOk);
                            mesage.Append(spcData.Result);
                            hSmartWindowControl1.HalconWindow.DispText(mesage, "window", new HTuple(0), new HTuple(0), new HTuple("red"), new HTuple("box"), new HTuple("false"));
                            hSmartWindowControl1.SetFullImagePart(null);
                        }
                    }
         
              
                }

            }
            catch (Exception)
            {
            }
        }

        private void comboBox2_DropDownClosed(object sender, EventArgs e)
        {
            try
            {



            }
            catch (Exception ex)
            {
            }
        }

        private void toolStripTextBox1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "请选择文件夹";
                string saveImagePath = "";
                saveImagePath = path;
                fbd.SelectedPath = saveImagePath;
                DialogResult dialog = UI.PropertyGrid.FolderBrowserLauncher.ShowFolderBrowser(fbd);
                if (dialog == DialogResult.OK)
                {
                    path = fbd.SelectedPath;
                    toolStripTextBox1.Text = path;
                    ProjectINI.In.SPCFindPaht = toolStripTextBox1.Text;
                    ProjectINI.In.SPCFindImagePaht = toolStripTextBox2.Text;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void toolStripComboBox1_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                if (toolStripComboBox1.SelectedItem == null) return;


  

            }
            catch (Exception ex)
            {
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog openFileDialog = new SaveFileDialog();
                openFileDialog.Filter = "Excel文件|*.xls;";
                string Paths = Strattime.ToString("yy年MM月dd日HH时mm分")
                     + "~" + EndTime.ToString("MM月dd日HH时mm分");
                openFileDialog.FileName = "SPC_DefectAnalysis" + DateTime.Now.ToString("yy年MM月dd日HH时mm分ss");
                //openFileDialog.FileName = Paths;
               DialogResult dialogResult = openFileDialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {

                    BindingList<SpcData> spcDaItem = new BindingList<SpcData>();
                    spcDaItem = dataGridView5.DataSource as BindingList<SpcData>;
                    //  DoWrite("<TABLE BORDER=1 style=\"word -break:break-all\"><TR> <TH>序号</TH><TH>型号</TH><TH>日期時間</TH><TH>条码</TH><TH>正反面</TH><TH>连板位号</TH><TH>元件</TH><TH>元件类型
                    //  </TH><TH>检测结果</TH><TH>复判结果</TH><TH>更新时间</TH><TH>复判人員</TH><TH>批号</TH><TH>Value</TH><TH>群组</TH><TH>AI数值</TH><TH>元件影像</TH><TH>3D Image</TH></TR>");

                    Vision2.ErosProjcetDLL.Excel.Npoi.AddSPCExcel(openFileDialog.FileName , Paths, Paths, spcDaItem.ToList());
                
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog openFileDialog = new SaveFileDialog();
                openFileDialog.Filter = "html文件|*.html;";
                openFileDialog.FileName = "SPC_" + DateTime.Now.ToString("yyyy年MM月dd日");
                DialogResult dialogResult = openFileDialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    HtmlMaker.Html.CreationData(openFileDialog.FileName, 
                        Strattime.ToString("yyyy/MM/dd HH:mm:ss")
                        +"~"+EndTime.ToString("yyyy/MM/dd HH:mm:ss"),spcDa);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripDropDownButton3_Click(object sender, EventArgs e)
        {

        }
        ProjectForm1 form1;
        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (form1==null|| form1.IsDisposed)
                {
                    form1 = new ProjectForm1();
                }
                form1.ShowDialog();


            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 元件名
        /// </summary>
        Dictionary<string, bool> SeletMChecked = new Dictionary<string, bool>();
        /// <summary>
        /// 库名
        /// </summary>
        Dictionary<string, bool> SeletMNameChecked = new Dictionary<string, bool>();


        private void dataGridView5_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 2 && true)
                {
                    dataGridView5.Controls.Clear();
                    GroupBox groupBox = new GroupBox();
                    Rectangle rect = dataGridView5.GetColumnDisplayRectangle(2, false);
                    groupBox.Location = new Point(rect.X, dataGridView5.ColumnHeadersHeight);
                    //groupBox.Location = new Point(215, 25);
                    CheckedListBox checkBoxList = new CheckedListBox();
                    System.Windows.Forms.Button button = new System.Windows.Forms.Button();
                    groupBox.Controls.Add(button);
                    button.Text = "X";
                    button.BringToFront();
                    button.Width = 20; button.Click += Button_Click;
                    button.Location = new Point(78, 0);
                    groupBox.Controls.Add(checkBoxList);
                    checkBoxList.Dock = DockStyle.Fill;
                    checkBoxList.VisibleChanged += CheckBoxList_Disposed;
                    void CheckBoxList_Disposed(object sender2, EventArgs e2)
                    {
                        try
                        {
                            SeletMChecked.Clear();
                            for (int i = 1; i < checkBoxList.Items.Count; i++)
                            {
                                SeletMChecked[checkBoxList.Items[i].ToString()] = checkBoxList.GetItemChecked(i);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                    checkBoxList.ItemCheck += CheckBoxList_ItemCheck;
                    void CheckBoxList_ItemCheck(object sender2, ItemCheckEventArgs e2)
                    {
                        try
                        {
                            if (e2.Index == 0)
                            {
                                if (e2.NewValue == CheckState.Checked)
                                {
                                    for (int i = 1; i < checkBoxList.Items.Count; i++)
                                    {
                                        checkBoxList.SetItemChecked(i, true);
                                    }

                                }
                                else
                                {
                                    for (int i = 1; i < checkBoxList.Items.Count; i++)
                                    {
                                        checkBoxList.SetItemChecked(i, false);
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                    groupBox.Text = "元件筛选";
                    groupBox.Width = 100;
                    groupBox.Height = 200;

                    checkBoxList.Items.Add("ALL", true);
                    foreach (var item in SeletMChecked)
                    {
                        checkBoxList.Items.Add(item.Key, item.Value);
                    }
                    dataGridView5.Controls.Add(groupBox);
                    groupBox.Show();
                }
                else if (e.ColumnIndex == 3)
                {
                    dataGridView5.Controls.Clear();
                    GroupBox groupBox = new GroupBox();
                    Rectangle rect = dataGridView5.GetColumnDisplayRectangle(3, false);
                    groupBox.Location = new Point(rect.X, dataGridView5.ColumnHeadersHeight);
                    //groupBox.Location = new Point(215, 25);
                    CheckedListBox checkBoxList = new CheckedListBox();
                    System.Windows.Forms.Button button = new System.Windows.Forms.Button();
                    groupBox.Controls.Add(button);
                    button.Text = "X";
                    button.BringToFront();
                    button.Width = 20; button.Click += Button_Click;
                    void Button_Click(object sender2, EventArgs e2)
                    {
                        try
                        {
                            dataGridView5.Controls.Clear();
                            var dset = from n in spcDa
                                       where SeletMNameChecked[n.Material_no]
                                       select n;
                            dataGridView5.DataSource = new BindingList<SpcData>(dset.ToArray());
                            //UpSpcColumn(chart3, ComponentS);
                            //UpSpcPie(chart2, ComponentS);
                            //UPspcDefectType(chart4, this.DefectTypeS);
                            //UpSpcPieDefectType(chart5, this.DefectTypeS);
                            UpdateChart();
                            UpdateChart2();
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    button.Location = new Point(78, 0);
                    groupBox.Controls.Add(checkBoxList);
                    checkBoxList.Dock = DockStyle.Fill;
                    checkBoxList.VisibleChanged += CheckBoxList_Disposed;
                    void CheckBoxList_Disposed(object sender2, EventArgs e2)
                    {
                        try
                        {
                            SeletMNameChecked.Clear();
                            for (int i = 1; i < checkBoxList.Items.Count; i++)
                            {
                                SeletMNameChecked[checkBoxList.Items[i].ToString()] = checkBoxList.GetItemChecked(i);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                    checkBoxList.ItemCheck += CheckBoxList_ItemCheck;
                    void CheckBoxList_ItemCheck(object sender2, ItemCheckEventArgs e2)
                    {
                        try
                        {
                            if (e2.Index == 0)
                            {
                                if (e2.NewValue == CheckState.Checked)
                                {
                                    for (int i = 1; i < checkBoxList.Items.Count; i++)
                                    {
                                        checkBoxList.SetItemChecked(i, true);
                                    }

                                }
                                else
                                {
                                    for (int i = 1; i < checkBoxList.Items.Count; i++)
                                    {
                                        checkBoxList.SetItemChecked(i, false);
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                    groupBox.Text = "库筛选";
                    groupBox.Width = 100;
                    groupBox.Height = 200;
                    checkBoxList.Items.Add("ALL", true);
                    foreach (var item in SeletMNameChecked)
                    {
                        checkBoxList.Items.Add(item.Key, item.Value);
                    }
                    dataGridView5.Controls.Add(groupBox);
                    groupBox.Show();
                }
            }
            catch (Exception)
            {

            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView5.Controls.Clear();

                var dset = from n in spcDa
                           where SeletMChecked[n.Name]
                           select n;
                dataGridView5.DataSource = new BindingList<SpcData>(dset.ToArray());
                //UpSpcColumn(chart3, ComponentS);
                //UpSpcPie(chart2, ComponentS);
                //UPspcDefectType(chart4, this.DefectTypeS);
                //UpSpcPieDefectType(chart5, this.DefectTypeS);
                UpdateChart();
                UpdateChart2();
            }
            catch (Exception ex)
            {
            }
        }




        private void comboBoxDataCount_SelectedIndexChanged(object sender, EventArgs e)
        {

            UpdateChart();
        }

        private void comboBoxDataCount_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(comboBoxDataCount1.Text))
            {
                UpdateChart();
            }
        }

        //private void UpdateChart()
        //{
        //    // Check if ComponentS is not null and contains data
        //    if (!IsComponentSInitialized())
        //    {
        //        return;  // 如果 ComponentS 尚未初始化或者没有数据，则不执行更新操作
        //    }
        //    int dataCount;
        //    if (int.TryParse(comboBoxDataCount1.Text, out dataCount))
        //    {
        //        if (dataCount > 0 && dataCount <= ComponentS.Count)
        //        {
        //            UpSpcPie2(chart2, ComponentS, dataCount);
        //            UpSpcColumn2(chart3, ComponentS, dataCount);
        //            //UPspcDefectType(chart4, DefectTypeS, dataCount);
        //            //UpSpcPieDefectType(chart5, DefectTypeS, dataCount);
        //        }
        //        else
        //        {
        //            MessageBox.Show($"请输入一个有效的数据个数。最大可输入数量为 {ComponentS.Count}。");
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("请输入一个有效的数字。");
        //    }
        //}
        private void UpdateChart()
        {
            if (!IsComponentSInitialized())
            {
                return;
            }

            int dataCount;
            if (int.TryParse(comboBoxDataCount1.Text, out dataCount))
            {
                if (dataCount > 0 && dataCount <= ComponentS.Count)
                {
                    int startIndex = trackBar1.Value;
                    if (startIndex + dataCount > ComponentS.Count)
                    {
                        startIndex = ComponentS.Count - dataCount;
                    }
                    var subData = ComponentS.Skip(startIndex).Take(dataCount).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                    UpSpcPie2(chart2, subData, dataCount);
                    UpSpcColumn2(chart3, subData, dataCount);
                }
                else
                {
                    MessageBox.Show($"请输入一个有效的数据个数。最大可输入数量为 {ComponentS.Count}。");
                }
            }
            else
            {
                MessageBox.Show("请输入一个有效的数字。");
            }
        }



        private void comboBoxDataCount2_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateChart2();
        }
        private void comboBoxDataCount2_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(comboBoxDataCount2.Text))
            {
                UpdateChart2();
            }
        }




        private void UpdateChart2()
        {
            if (!IsComponentSInitialized())
            {
                return;
            }

            int dataCount;
            if (int.TryParse(comboBoxDataCount2.Text, out dataCount))
            {
                if (dataCount > 0 && dataCount <= ComponentS.Count)
                {
                    int startIndex = trackBar1.Value;
                    if (startIndex + dataCount > DefectTypeS.Count)
                    {
                        startIndex = DefectTypeS.Count - dataCount;
                    }
                    var subData = DefectTypeS.Skip(startIndex).Take(dataCount).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                    UPspcDefectType(chart4, subData, dataCount);
                    UpSpcPieDefectType(chart5, subData, dataCount);
                }
                else
                {
                    MessageBox.Show($"请输入一个有效的数据个数。最大可输入数量为 {ComponentS.Count}。");
                }
            }
            else
            {
                MessageBox.Show("请输入一个有效的数字。");
            }
        }


        private bool IsComponentSInitialized()
        {
            return ComponentS != null && ComponentS.Count > 0;
        }

        //private void trackBar1_Scroll(object sender, EventArgs e)
        //{
        //    UpdateChart();
        //    UpdateChart2();
        //}

        private void textBox_Countlables(object sender, EventArgs e)
        {
            // 显示最大可生成图表的数值
            countlables.Text = ComponentS.Count.ToString();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            UpdateChart();
            UpdateChart2();
        }
    }
}
