using SPC_Windows.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

using static Vision2.SPCFile.ProductSPCControl;

namespace Vision2.SPCFile
{
    public partial class ProductSPCControl : UserControl
    {
        public ProductSPCControl()
        {
            InitializeComponent();
            //UI.DataGridViewF.StCon.AddCount(dataGridView1);
            dataGridView1.AutoGenerateColumns = false;
        }
        public  enum SpcTypeShowEnum
        {
            Noll=0,
            库统计=1,
            元件统计=2,
            元件top=3,
            每时数据=4,
            细节数据=5,
            OKNGTop = 6,
            Nu=7,
        }
        public   DateTime DateTimeNew;
        SpcTypeShowEnum TypeS = SpcTypeShowEnum.OKNGTop;
        OKNumberClass oKNumberClass1;
        BindingList<BankNumber> componentNumbers;
        /// <summary>
        /// 更新数据到表格
        /// </summary>
        /// <param name="oKNumberClass"></param>
        /// <param name="isShowData"></param>
        public void RefreshData(OKNumberClass oKNumberClass,  SpcTypeShowEnum typeShow = SpcTypeShowEnum.Noll)
        {
            try
            {
                if (oKNumberClass==null)
                {
                    return;
                }
                oKNumberClass1 = oKNumberClass;
                label3.Text = oKNumberClass1.GetTextSPC();
                if (TypeS== SpcTypeShowEnum.Nu && typeShow==SpcTypeShowEnum.Noll)
                {
                    TypeS= SpcTypeShowEnum.OKNGTop;
                }
                else if(typeShow != SpcTypeShowEnum.Noll)
                {
                    TypeS = typeShow;
                }
                switch (TypeS)
                {
                    case SpcTypeShowEnum.OKNGTop:
                        chartType.Visible = true;
                        chartType.Series.Clear();
                        chartType.ChartAreas.Clear();
                        chartType.Titles.Clear();
                        Series Series1 = new Series();
                        chartType.Series.Add(Series1);
                        chartType.Series["Series1"].ChartType = SeriesChartType.Column;
                        ChartArea ChartArea1 = new ChartArea();
                        chartType.ChartAreas.Add(ChartArea1);
                        //开启三维模式的原因是为了避免标签重叠
                        chartType.ChartAreas["ChartArea1"].AxisY.Interval = 100;
                        chartType.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;//开启三维模式;PointDepth:厚度BorderWidth:边框宽
                        chartType.ChartAreas["ChartArea1"].Area3DStyle.Rotation = 10;//起始角度
                        chartType.ChartAreas["ChartArea1"].Area3DStyle.Inclination = 20;//倾斜度(0～90)
                        chartType.ChartAreas["ChartArea1"].Area3DStyle.LightStyle = LightStyle.Realistic;//表面光泽度
                        chartType.ChartAreas["ChartArea1"].AxisX.Interval = 1; //决定x轴显示文本的间隔，1为强制每个柱状体都显示，3则间隔3个显示
                        chartType.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new Font("宋体", 9, FontStyle.Regular);
                        chartType.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                        chartType.Series[0].XValueMember = "name";
                        chartType.Series[0].YValueMembers = "sumcount";
                        //ChartArea1.AxisX.w
                        ChartArea1.AxisX.Minimum = 0;
                        ChartArea1.AxisX.Maximum = 4;
                        SPC_RefreshDataEvent();
                        if (oKNumberClass1.AutoOKNumber>1500)
                        {
                            chartType.ChartAreas["ChartArea1"].AxisY.Interval = 500;
                        }
                        chartType.Series["Series1"].Label = "#VALY";
                        chartType.Series["Series1"].ToolTip = "#VALX";
                        chartType.Series["Series1"]["PointWidth"] = "0.5";
                        label3.Visible = true;
                        chartType.Dock = DockStyle.Fill;
                        dataGridView1.Visible = false;
                        break;
                    case SpcTypeShowEnum.库统计:
                        label3.Visible = false;
                        label3.Text = oKNumberClass1.GetTextSPC();
                        var distinctValues = oKNumberClass1.ComponentS.Select(p => p.Value.BankNmae).Distinct().ToList();
                        distinctValues.Remove("");
                        distinctValues.Remove(null);
                        Dictionary<string, BankNumber> keyValuePairs = new Dictionary<string, BankNumber>();
                        foreach (var item in distinctValues)
                        {
                            var datas = from n in oKNumberClass1.ComponentS
                                        where n.Value.BankNmae == item
                                        select n.Value;
                            keyValuePairs.Add(item, new BankNumber());
                            keyValuePairs[item].Nmae = item;
                            foreach (var itemtd in datas)
                            {
                                keyValuePairs[item].OKNumber += itemtd.OKNumber;
                                keyValuePairs[item].NGNumber += itemtd.NGNumber;
                                keyValuePairs[item].FalseCall += itemtd.FalseCall;
                            }
                        }
                        var Bankdatas = from n in keyValuePairs
                                        orderby n.Value.NGNumber descending
                                        select n.Value;
                        chartType.Visible = false;
                        dataGridView1.Visible = true;
                        dataGridView1.Columns.Clear();
                        dataGridView1.Columns.Add(Column1);
                        dataGridView1.Columns.Add(Column2);
                        dataGridView1.Columns.Add(Column3);
                        dataGridView1.Columns.Add(Column4);
                        dataGridView1.Columns.Add(Column5);
                        //Column5.SortMode = DataGridViewColumnSortMode.Programmatic;
                        componentNumbers = new BindingList<BankNumber>(Bankdatas.ToArray());
                        dataGridView1.DataSource = componentNumbers;
                        dataGridView1.Dock = DockStyle.Fill;
                        panel1.AutoSize = true;
                        panel1.Dock = DockStyle.Fill;
                        panel1.Width = 400;
                        label3.Dock = DockStyle.Right;
                        break;
                    case SpcTypeShowEnum.元件统计:

                         distinctValues = oKNumberClass1.ComponentS.Select(p => p.Value.BankNmae).Distinct().ToList();
                        distinctValues.Remove("");
                        distinctValues.Remove(null);
                        keyValuePairs = new Dictionary<string, BankNumber>();
                        foreach (var item in distinctValues)
                        {
                            var datas = from n in oKNumberClass1.ComponentS
                                        where n.Value.BankNmae == item
                                        select n.Value;
                            keyValuePairs.Add(item, new BankNumber());
                            keyValuePairs[item].Nmae = item;
                            foreach (var itemtd in datas)
                            {
                                keyValuePairs[item].OKNumber += itemtd.OKNumber;
                                keyValuePairs[item].NGNumber += itemtd.NGNumber;
                                keyValuePairs[item].FalseCall += itemtd.FalseCall;
                            }
                        }
                         Bankdatas = from n in keyValuePairs
                                        orderby n.Value.NGNumber descending
                                        select n.Value;
                        chartType.Visible = false;
                        dataGridView1.Visible = true;
                        dataGridView1.Columns.Clear();
                        dataGridView1.Columns.Add(Column1);
                        dataGridView1.Columns.Add(Column2);
                        dataGridView1.Columns.Add(Column3);
                        dataGridView1.Columns.Add(Column4);
                        dataGridView1.Columns.Add(Column5);
                        componentNumbers = new BindingList<BankNumber>(Bankdatas.ToArray());
                        dataGridView1.DataSource = componentNumbers;
                        dataGridView1.Dock = DockStyle.Fill;
                        panel1.AutoSize = true;
                        panel1.Dock = DockStyle.Fill;
                        panel1.Width = 400;
                        label3.Dock = DockStyle.Right;
                        label3.Visible = false;
                        chartType.Visible = false;
                        break;
                    case SpcTypeShowEnum.元件top:
                        label3.Visible = false;
                        chartType.Visible = true;
                        chartType.Series.Clear();
                        chartType.ChartAreas.Clear();
                        chartType.Titles.Clear();
                         Series1 = new Series();
                        chartType.Series.Add(Series1);
                        chartType.Series["Series1"].ChartType = SeriesChartType.Column;
   
                        chartType.Series["Series1"].LegendText = "";
                        chartType.Series["Series1"].Label = "#VALY";
                        chartType.Series["Series1"].ToolTip = "#VALX";
                        chartType.Series["Series1"]["PointWidth"] = "0.5";
                        ChartArea1 = new ChartArea();
                        chartType.ChartAreas.Add(ChartArea1);
                        //开启三维模式的原因是为了避免标签重叠
                        chartType.ChartAreas["ChartArea1"].AxisY.Interval = 50;
                        chartType.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;//开启三维模式;PointDepth:厚度BorderWidth:边框宽
                        chartType.ChartAreas["ChartArea1"].Area3DStyle.Rotation = 20;//起始角度
                        chartType.ChartAreas["ChartArea1"].Area3DStyle.Inclination = 15;//倾斜度(0～90)
                        chartType.ChartAreas["ChartArea1"].Area3DStyle.LightStyle = LightStyle.Realistic;//表面光泽度
                        chartType.ChartAreas["ChartArea1"].AxisX.Interval = 1; //决定x轴显示文本的间隔，1为强制每个柱状体都显示，3则间隔3个显示
                        chartType.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new Font("宋体", 9, FontStyle.Regular);
                        chartType.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                        chartType.Series[0].XValueMember = "name";
                        chartType.Series[0].YValueMembers = "sumcount";
                        distinctValues = oKNumberClass1.ComponentS.Select(p => p.Value.BankNmae).Distinct().ToList();
                        distinctValues.Remove("");
                        distinctValues.Remove(null);
                        var list = from n in oKNumberClass1.ComponentS
                                   orderby n.Value.NGNumber descending
                                   select n;
                        int number = 1;
                        keyValuePairs = new Dictionary<string, BankNumber>();
                        foreach (var item in list)
                        {
                            keyValuePairs.Add(item.Value.Nmae, new BankNumber());
                            keyValuePairs[item.Value.Nmae].OKNumber += item.Value.OKNumber;
                            keyValuePairs[item.Value.Nmae].NGNumber += item.Value.NGNumber;
                            keyValuePairs[item.Value.Nmae].FalseCall += item.Value.FalseCall;
                            //text += "Top" + number + ":" + item.Value.Nmae + " 数量:" + item.Value.NGNumber + Environment.NewLine;
                            number++;
                            if (number >= 6)
                            {
                                break;
                            }
                        }
                        ChartArea1.AxisX.Minimum = 0;
                        ChartArea1.AxisX.Maximum = 5;
                        ChartArea1.AxisY.Minimum = 0d;
                        foreach (var v in keyValuePairs)
                        {
                            chartType.Series["Series1"].Points.AddXY(v.Key, v.Value.NGNumber);
                        }
                        chartType.Dock = DockStyle.Fill;
                        dataGridView1.Visible = false;
                        break;
                    case SpcTypeShowEnum.每时数据:
                        label3.Visible = false;
                        dataGridView1.Visible = false;
                        chartType.Visible = true;
                        chartType.Dock = DockStyle.Fill;
                        chartType.Series.Clear();
                        chartType.ChartAreas.Clear();
                        chartType.Titles.Clear();
                         Series1 = new Series();
                        chartType.Series.Add(Series1);
                        chartType.Series["Series1"].ChartType = SeriesChartType.Column;
                        chartType.Legends[0].Enabled = false;
                        chartType.Series["Series1"].LegendText = "";
                        chartType.Series["Series1"].Label = "#VALY";
                        chartType.Series["Series1"].ToolTip = "#VALX";
                        chartType.Series["Series1"]["PointWidth"] = "0.5";
                        ChartArea1 = new ChartArea();
                        chartType.ChartAreas.Add(ChartArea1);
                        //开启三维模式的原因是为了避免标签重叠
                        chartType.ChartAreas["ChartArea1"].AxisY.Interval = 50;
                        chartType.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;//开启三维模式;PointDepth:厚度BorderWidth:边框宽
                        chartType.ChartAreas["ChartArea1"].Area3DStyle.Rotation = 20;//起始角度
                        chartType.ChartAreas["ChartArea1"].Area3DStyle.Inclination = 15;//倾斜度(0～90)
                        chartType.ChartAreas["ChartArea1"].Area3DStyle.LightStyle = LightStyle.Realistic;//表面光泽度
                        chartType.ChartAreas["ChartArea1"].AxisX.Interval = 1; //决定x轴显示文本的间隔，1为强制每个柱状体都显示，3则间隔3个显示
                        chartType.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new Font("宋体", 9, FontStyle.Regular);
                        chartType.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                        chartType.Series[0].XValueMember = "name";
                        chartType.Series[0].YValueMembers = "sumcount";
                        //ChartArea1.AxisX.w
                        ChartArea1.AxisX.Minimum = 0;
                        ChartArea1.AxisX.Maximum = 24;
                        ChartArea1.AxisY.Minimum = 0d;
                        int x = 0;
                        OKNGNumber[] oKNumberClassH = SPC.GetOKNumberList();
                        if (DateTimeNew==DateTime.MinValue)
                        {
                            oKNumberClassH = SPC.GetOKNumberList();
                        }
                        else
                        {
                            string dateTimeString = DateTimeNew.ToString("yy年MM月dd日");

                            if (!ProjectINI.ReadPathJsonToCalssEX(ProjectINI.TempPath + "计数\\" + dateTimeString + "每时数据.txt", out oKNumberClassH))
                            {
                                oKNumberClassH = new OKNGNumber[24];
                                for (int i = 0; i < oKNumberClassH.Length; i++)
                                {
                                    oKNumberClassH[i] = new OKNGNumber();
                                }
                            }
     
                        }
                        List<int> vs = new List<int>();
                        for (int i = 0; i < oKNumberClassH.Length; i++)
                        {
                            vs.Add(oKNumberClassH[i].Number);
                        }
                        foreach (int v in vs)
                        {
                            chartType.Series["Series1"].Points.AddXY(x, v);
                            x++;
                        }

                        break;
                    case SpcTypeShowEnum.细节数据:
                        label3.Visible = false;
                        dataGridView1.Visible = true;
                        dataGridView1.Dock = DockStyle.Fill;
                        chartType.Visible = false;
                        dataGridView1.DataSource = null;
                        dataGridView1.Rows.Clear();
                        dataGridView1.Columns.Clear();
                        DataGridViewTextBoxColumn column1 = new DataGridViewTextBoxColumn();
                        column1.Name = "名称";
                        column1.HeaderText = "名称";
                        column1.Width = 150;
                        column1.SortMode = DataGridViewColumnSortMode.NotSortable;
                        this.dataGridView1.Columns.Add(column1);

                        DataGridViewTextBoxColumn column2 = new DataGridViewTextBoxColumn();

                        column2.Name = "数据";
                        column2.HeaderText = "数据";
                        column2.Width = 150;
                        column2.SortMode = DataGridViewColumnSortMode.NotSortable;
                        this.dataGridView1.Columns.Add(column2);
                        DataGridViewTextBoxColumn column3 = new DataGridViewTextBoxColumn();

                        column3.Name = "数据内容";
                        column3.HeaderText = "数据内容";
                        column3.Width = 150;
                        column3.SortMode = DataGridViewColumnSortMode.NotSortable;
                        this.dataGridView1.Columns.Add(column3);
                        dataGridView1.Rows.Add(13);
                         list = from n in oKNumberClass1.ComponentS
                                   orderby n.Value.NGNumber descending
                                   select n;
                        int oknumber = 0;
                        int NGnumber = 0;
                        int FalseCall = 0;
                        keyValuePairs = new Dictionary<string, BankNumber>();
                        foreach (var item in list)
                        {
                            keyValuePairs.Add(item.Value.Nmae, new BankNumber());
                            keyValuePairs[item.Value.Nmae].OKNumber += item.Value.OKNumber;
                            keyValuePairs[item.Value.Nmae].NGNumber += item.Value.NGNumber;
                            keyValuePairs[item.Value.Nmae].FalseCall += item.Value.FalseCall;
                            oknumber += item.Value.OKNumber;
                            NGnumber += item.Value.NGNumber;
                            FalseCall += item.Value.FalseCall;
                        }
                        row = 0;
                        AddData("Total Inspection Boards", oKNumberClass1.Number, "总投产整板数");
                        AddData("Pass OK Boards", oKNumberClass1.OKNumber, "复判整板OK的数、真实整板OK数");
                        AddData("False Call OK Boards", oKNumberClass1.OKNumber - oKNumberClass1.AutoOKNumber, "机检NG板数、复判数");
                        AddData("NG Boards", oKNumberClass1.AutoOKNumber, "没有误判的OK数");
                        AddData("Loss Boards", "", "漏失板数");
                        AddData("False Call Components", FalseCall, "误判零件总数");
                        AddData("NG Components", NGnumber, "NG零件数");
                        //AddData( "元件数据", "", "");
                        AddData("Total Components", oknumber + NGnumber, "零件总数(单零件数)");
                        AddData("Over Kill PPM(By Components)", (double)FalseCall / (double)(oknumber + NGnumber) * 1000000, "误判零件总数/零件总数*10M");
                        AddData("FPY(By Board)", (double)oKNumberClass1.OKNumber / (double)oKNumberClass1.Number * 100.0, "ok板子数/总检板子数*100%");
                        AddData("VPY(By Board)", (double)(oKNumberClass1.Number - oKNumberClass1.AutoOKNumber) / (double)oKNumberClass1.Number * 100.0, "(总产品数-NG Boards)/总产品数*100%");
                        AddData("NG PPM(By Components)", (double)NGnumber / (double)(oknumber + NGnumber) * 1000000.0, "真实NG零件数/总检零件数*1000000");
                        AddData("Loss PPM(By Board)", "", "loss板数/总检板子数*1000000");
                        AddData("AUC", "", "");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void 查看库统计ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshData(oKNumberClass1, SpcTypeShowEnum.库统计);
            return;
            try
            {
                label3.Visible = false;
                label3.Text = oKNumberClass1.GetTextSPC();
                var distinctValues = oKNumberClass1.ComponentS.Select(p => p.Value.BankNmae).Distinct().ToList();
                distinctValues.Remove("");
                distinctValues.Remove(null);
                Dictionary<string, BankNumber> keyValuePairs = new Dictionary<string, BankNumber>();
                foreach (var item in distinctValues)
                {
                    var datas = from n in oKNumberClass1.ComponentS
                                where n.Value.BankNmae == item
                                select n.Value;
                    keyValuePairs.Add(item, new BankNumber());
                    keyValuePairs[item].Nmae = item;
                    foreach (var itemtd in datas)
                    {
                        keyValuePairs[item].OKNumber += itemtd.OKNumber;
                        keyValuePairs[item].NGNumber += itemtd.NGNumber;
                        keyValuePairs[item].FalseCall += itemtd.FalseCall;
                    }
                }
                var Bankdatas = from n in keyValuePairs
                                orderby n.Value.NGNumber descending
                                select n.Value;
                chartType.Visible = false;
                dataGridView1.Visible = true;
                dataGridView1.Columns.Clear();
                dataGridView1.Columns.Add(Column1);
                dataGridView1.Columns.Add(Column2);
                dataGridView1.Columns.Add(Column3);
                dataGridView1.Columns.Add(Column4);
                dataGridView1.Columns.Add(Column5);
                BindingList<BankNumber> componentNumbers = new BindingList<BankNumber>(Bankdatas.ToArray());
                dataGridView1.DataSource = componentNumbers;
                dataGridView1.Dock = DockStyle.Fill;
                panel1.AutoSize = true;
                panel1.Dock= DockStyle.Fill;
                panel1.Width = 400;
                label3.Dock = DockStyle.Right;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 查看元件统计ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshData(oKNumberClass1,SpcTypeShowEnum.元件统计);
        }

        private void 查看元件Top表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                RefreshData(oKNumberClass1, SpcTypeShowEnum.元件top);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 查看当天Top表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                RefreshData(oKNumberClass1, SpcTypeShowEnum.OKNGTop);
                //return;
                //chartType.Visible = true;
                //chartType.Series.Clear();
                //chartType.ChartAreas.Clear();
                //chartType.Titles.Clear();
                //Series Series1 = new Series();
                //chartType.Series.Add(Series1);
                //chartType.Series["Series1"].ChartType = SeriesChartType.Column;
                //chartType.Legends[0].Enabled = false;
                //chartType.Series["Series1"].LegendText = "";
                //chartType.Series["Series1"].Label = "#VALY";
                //chartType.Series["Series1"].ToolTip = "#VALX";
                //chartType.Series["Series1"]["PointWidth"] = "0.5";
                //ChartArea ChartArea1 = new ChartArea();
                //chartType.ChartAreas.Add(ChartArea1);
                ////开启三维模式的原因是为了避免标签重叠
                //chartType.ChartAreas["ChartArea1"].AxisY.Interval = 100;
                //chartType.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;//开启三维模式;PointDepth:厚度BorderWidth:边框宽
                //chartType.ChartAreas["ChartArea1"].Area3DStyle.Rotation = 1;//起始角度
                //chartType.ChartAreas["ChartArea1"].Area3DStyle.Inclination = 0;//倾斜度(0～90)
                //chartType.ChartAreas["ChartArea1"].Area3DStyle.LightStyle = LightStyle.Realistic;//表面光泽度
                //chartType.ChartAreas["ChartArea1"].AxisX.Interval = 1; //决定x轴显示文本的间隔，1为强制每个柱状体都显示，3则间隔3个显示
                //chartType.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new Font("宋体", 9, FontStyle.Regular);
                //chartType.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                //chartType.Series[0].XValueMember = "name";
                //chartType.Series[0].YValueMembers = "sumcount";
                ////ChartArea1.AxisX.w
                //ChartArea1.AxisX.Minimum = 0;
                //ChartArea1.AxisX.Maximum = 4;
                //SPC_RefreshDataEvent();
                //label3.Visible = true;
                //chartType.Dock = DockStyle.Fill;
                //dataGridView1.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 查看每时表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                RefreshData(oKNumberClass1, SpcTypeShowEnum.每时数据);
         

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SPC_RefreshDataEvent()
        {

            try
            {
                if (oKNumberClass1!=null)
                {
                    chartType.Series["Series1"].Points.Clear();
                    chartType.Series["Series1"].Points.AddXY("First PASS", oKNumberClass1.AutoOKNumber);
                    chartType.Series["Series1"].Points[0].Color = System.Drawing.Color.Green;
                    chartType.Series["Series1"].Points.AddXY("False Call", oKNumberClass1.AutoNGNumber);
                    chartType.Series["Series1"].Points[1].Color = System.Drawing.Color.Blue;
                    chartType.Series["Series1"].Points.AddXY("Fail", oKNumberClass1.NGNumber);
                    chartType.Series["Series1"].Points[2].Color = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ProductSPCControl_Load(object sender, EventArgs e)
        {
            try
            {
                查看当天Top表ToolStripMenuItem_Click(null,null);

            }
            catch (Exception)
            {
            }
        }

        private void 导出表格ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                SaveFileDialog openFileDialog = new SaveFileDialog();
                openFileDialog.Filter = "Csv文件|*csv;";
                openFileDialog.FileName = "SPC_TOP"+DateTime.Now.ToString("yyyy年MM月dd日");
                    DialogResult dialogResult= openFileDialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    Vision2.ErosProjcetDLL.Excel.Npoi.DataGridViewExportCsv(openFileDialog.FileName, dataGridView1);
                }
            }
            catch (Exception)
            {
            }
        }
        int row;
        private void 导出全部数据到表格ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                RefreshData(oKNumberClass1, SpcTypeShowEnum.细节数据);
                return;
                label3.Visible = false;
                dataGridView1.Visible = true;
                dataGridView1.Dock = DockStyle.Fill;
                chartType.Visible = false;
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                DataGridViewTextBoxColumn column1 = new DataGridViewTextBoxColumn();
                column1.Name = "名称";
                column1.HeaderText = "名称";
                column1.Width = 150;
                column1.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dataGridView1.Columns.Add(column1);

                DataGridViewTextBoxColumn column2 = new DataGridViewTextBoxColumn();

                column2.Name = "数据";
                column2.HeaderText = "数据";
                column2.Width = 150;
                column2.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dataGridView1.Columns.Add(column2);
                DataGridViewTextBoxColumn column3 = new DataGridViewTextBoxColumn();

                column3.Name = "数据内容";
                column3.HeaderText = "数据内容";
                column3.Width = 150;
                column3.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dataGridView1.Columns.Add(column3);
                dataGridView1.Rows.Add(13);
                var list = from n in oKNumberClass1.ComponentS
                           orderby n.Value.NGNumber descending
                           select n;
                int oknumber = 0;
                int NGnumber = 0;
                int FalseCall = 0;
                Dictionary<string, BankNumber> keyValuePairs = new Dictionary<string, BankNumber>();
                foreach (var item in list)
                {
                    keyValuePairs.Add(item.Value.Nmae, new BankNumber());
                    keyValuePairs[item.Value.Nmae].OKNumber += item.Value.OKNumber;
                    keyValuePairs[item.Value.Nmae].NGNumber += item.Value.NGNumber;
                    keyValuePairs[item.Value.Nmae].FalseCall += item.Value.FalseCall;
                    oknumber += item.Value.OKNumber;
                    NGnumber += item.Value.NGNumber;
                    FalseCall += item.Value.FalseCall;
                }
                row = 0;

                AddData( "Total Inspection Boards", oKNumberClass1.Number, "总投产整板数");
                AddData( "Pass OK Boards", oKNumberClass1.OKNumber, "复判整板OK的数、真实整板OK数");
                AddData( "False Call OK Boards", oKNumberClass1.OKNumber - oKNumberClass1.AutoOKNumber, "机检NG板数、复判数");
                AddData( "NG Boards", oKNumberClass1.AutoOKNumber, "没有误判的OK数");
                AddData( "Loss Boards", "", "漏失板数");
                AddData( "False Call Components", FalseCall, "误判零件总数");
                AddData( "NG Components", NGnumber, "NG零件数");
                //AddData( "元件数据", "", "");
                AddData( "Total Components", oknumber + NGnumber, "零件总数(单零件数)");
                AddData("Over Kill PPM(By Components)", (double)FalseCall / (double)(oknumber + NGnumber)*1000000, "误判零件总数/零件总数*10M");
                AddData("FPY(By Board)", (double)oKNumberClass1.OKNumber/ (double)oKNumberClass1.Number*100.0, "ok板子数/总检板子数*100%");
                AddData("VPY(By Board)", (double)(oKNumberClass1.Number- oKNumberClass1.AutoOKNumber)/ (double)oKNumberClass1.Number*100.0, "(总产品数-NG Boards)/总产品数*100%");
                AddData("NG PPM(By Components)",(double) NGnumber / (double)( oknumber + NGnumber)*1000000.0, "真实NG零件数/总检零件数*1000000");
                AddData("Loss PPM(By Board)", "", "loss板数/总检板子数*1000000");
                AddData( "AUC", "", "");
         
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        public void AddData(string key,object value,string textmessage)
        {
            try
            {
                if (row >= dataGridView1.Rows.Count)
                {
                    dataGridView1.Rows.Add();
                }
                dataGridView1.Rows[row].Cells[0].Value = key;
                dataGridView1.Rows[row].Cells[1].Value = value;
                dataGridView1.Rows[row].Cells[2].Value = textmessage;
                row++;
            }
            catch (Exception ex)
            {
            }
        
        }

        private void 导出数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog openFileDialog = new SaveFileDialog();
                openFileDialog.Filter = "Csv文件|*csv;";
                openFileDialog.FileName = "SPC_TOP" + DateTime.Now.ToString("yyyy年MM月dd日");
                DialogResult dialogResult = openFileDialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    Vision2.ErosProjcetDLL.Excel.Npoi.DataGridViewExportCsv(openFileDialog.FileName, dataGridView1);
                }

            }
            catch (Exception)
            {
            }
        }
        private ListSortDirection sortdirection = ListSortDirection.Ascending;
        private DataGridViewColumn sortcolumn = null;
        private int sortColindex = -1;
        private void dataGridView1_Sorted(object sender, EventArgs e)
        {
            sortcolumn = dataGridView1.SortedColumn;
            sortColindex = sortcolumn.Index;
            sortdirection =
            dataGridView1.SortOrder == SortOrder.Ascending ?
            ListSortDirection.Ascending : ListSortDirection.Descending;
        }
        bool locationSort;
        bool NGnumberSort;
        bool NameSort;
        bool DoseRateSort;
        bool RsrtSort;

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "OK")
                {
                    if (locationSort)
                    {
                        locationSort = false;
                        var list = from n in  componentNumbers
                                   orderby n.OKNumber ascending
                                   select n;
                        componentNumbers = new BindingList<BankNumber>(list.ToArray());
                    }
                    else
                    {
                        locationSort = true;
                        var list = from n in componentNumbers
                                   orderby n.OKNumber descending
                                   select n;
                        componentNumbers = new BindingList<BankNumber>(list.ToArray());
                    }
                }
                else if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "元件名称")
                {
                    if (NameSort)
                    {
                        NameSort = false;
                        var list = from n in componentNumbers
                                   orderby n.Nmae ascending
                                   select n;
                        componentNumbers = new BindingList<BankNumber>(list.ToArray());
                    }
                    else
                    {
                        NameSort = true;
                        var list = from n in componentNumbers
                                   orderby n.Nmae descending
                                   select n;
                        componentNumbers = new BindingList<BankNumber>(list.ToArray());
                    }
                }
                else if(dataGridView1.Columns[e.ColumnIndex].HeaderText == "NG")
                {
                    if (NGnumberSort)
                    {
                        NGnumberSort = false;
                        var list = from n in componentNumbers
                                   orderby n.NGNumber ascending
                                   select n;
                        componentNumbers = new BindingList<BankNumber>(list.ToArray());
                    }
                    else
                    {
                        NGnumberSort = true;
                        var list = from n in componentNumbers
                                   orderby n.NGNumber descending
                                   select n;
                        componentNumbers = new BindingList<BankNumber>(list.ToArray());
                    }
                }
                else if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "良率%")
                {
                    if (DoseRateSort)
                    {
                        DoseRateSort = false;
                        var list = from n in componentNumbers
                                   orderby n.Yield ascending
                                   select n;
                        componentNumbers = new BindingList<BankNumber>(list.ToArray());
                    }
                    else
                    {
                        DoseRateSort = true;
                        var list = from n in componentNumbers
                                   orderby n.Yield descending
                                   select n;
                        componentNumbers = new BindingList<BankNumber>(list.ToArray());
                    }
                }
                else if (dataGridView1.Columns[ e.ColumnIndex].HeaderText == "误判")
                {
                    if (RsrtSort)
                    {
                        RsrtSort = false;
                        var list = from n in componentNumbers
                                   orderby n.FalseCall ascending
                                   select n;
                        componentNumbers = new BindingList<BankNumber>(list.ToArray());
                    }
                    else
                    {
                        RsrtSort = true;
                        var list = from n in componentNumbers
                                   orderby n.FalseCall descending
                                   select n;
                        componentNumbers = new BindingList<BankNumber>(list.ToArray());
                    }
                }
                dataGridView1.DataSource = componentNumbers;
                
            }
            catch (Exception ex)
            {

            }
        }
    }
}
