using Vision2.UI;

namespace Vision2.SPCFile
{
    partial class ProductSPCControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.label3 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.导出数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看当天Top表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看库统计ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看元件统计ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看元件Top表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看每时表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出全部数据到表格ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new Vision2.UI.DataGridViewF.DataGridViewEx();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chartType = new Vision2.UI.ChartEX();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartType)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ContextMenuStrip = this.contextMenuStrip1;
            this.label3.Dock = System.Windows.Forms.DockStyle.Right;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(314, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 221);
            this.label3.TabIndex = 0;
            this.label3.Text = "名称:ste15\r\nOK:12210 NG:12200\r\n良率%:0123\r\n元件数量:12333\r\n库总数:1233\r\n元件OK:122222\r\n元件NG:21" +
    "\r\n元件良率:92.1\r\nTop1 12\r\nTop2 12\r\nTop3 12\r\nTop4 3\r\nTop5 1";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导出数据ToolStripMenuItem,
            this.查看当天Top表ToolStripMenuItem,
            this.查看库统计ToolStripMenuItem,
            this.查看元件统计ToolStripMenuItem,
            this.查看元件Top表ToolStripMenuItem,
            this.查看每时表ToolStripMenuItem,
            this.导出全部数据到表格ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(173, 158);
            // 
            // 导出数据ToolStripMenuItem
            // 
            this.导出数据ToolStripMenuItem.Name = "导出数据ToolStripMenuItem";
            this.导出数据ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.导出数据ToolStripMenuItem.Text = "导出表格数据";
            this.导出数据ToolStripMenuItem.Click += new System.EventHandler(this.导出数据ToolStripMenuItem_Click);
            // 
            // 查看当天Top表ToolStripMenuItem
            // 
            this.查看当天Top表ToolStripMenuItem.Name = "查看当天Top表ToolStripMenuItem";
            this.查看当天Top表ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.查看当天Top表ToolStripMenuItem.Text = "查看OKNGTop表";
            this.查看当天Top表ToolStripMenuItem.Click += new System.EventHandler(this.查看当天Top表ToolStripMenuItem_Click);
            // 
            // 查看库统计ToolStripMenuItem
            // 
            this.查看库统计ToolStripMenuItem.Name = "查看库统计ToolStripMenuItem";
            this.查看库统计ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.查看库统计ToolStripMenuItem.Text = "查看库统计";
            this.查看库统计ToolStripMenuItem.Click += new System.EventHandler(this.查看库统计ToolStripMenuItem_Click);
            // 
            // 查看元件统计ToolStripMenuItem
            // 
            this.查看元件统计ToolStripMenuItem.Name = "查看元件统计ToolStripMenuItem";
            this.查看元件统计ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.查看元件统计ToolStripMenuItem.Text = "查看元件统计";
            this.查看元件统计ToolStripMenuItem.Click += new System.EventHandler(this.查看元件统计ToolStripMenuItem_Click);
            // 
            // 查看元件Top表ToolStripMenuItem
            // 
            this.查看元件Top表ToolStripMenuItem.Name = "查看元件Top表ToolStripMenuItem";
            this.查看元件Top表ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.查看元件Top表ToolStripMenuItem.Text = "查看元件Top表";
            this.查看元件Top表ToolStripMenuItem.Click += new System.EventHandler(this.查看元件Top表ToolStripMenuItem_Click);
            // 
            // 查看每时表ToolStripMenuItem
            // 
            this.查看每时表ToolStripMenuItem.Name = "查看每时表ToolStripMenuItem";
            this.查看每时表ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.查看每时表ToolStripMenuItem.Text = "查看每时表";
            this.查看每时表ToolStripMenuItem.Click += new System.EventHandler(this.查看每时表ToolStripMenuItem_Click);
            // 
            // 导出全部数据到表格ToolStripMenuItem
            // 
            this.导出全部数据到表格ToolStripMenuItem.Name = "导出全部数据到表格ToolStripMenuItem";
            this.导出全部数据到表格ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.导出全部数据到表格ToolStripMenuItem.Text = "查看细节数据";
            this.导出全部数据到表格ToolStripMenuItem.Click += new System.EventHandler(this.导出全部数据到表格ToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Controls.Add(this.chartType);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(314, 357);
            this.panel1.TabIndex = 3;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column5,
            this.Column4});
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Location = new System.Drawing.Point(-26, 333);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(340, 20);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.Visible = false;
            this.dataGridView1.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_ColumnHeaderMouseClick);
            this.dataGridView1.Sorted += new System.EventHandler(this.dataGridView1_Sorted);
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "Nmae";
            this.Column1.HeaderText = "元件名称";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 81;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "OKNumber";
            this.Column2.HeaderText = "OK";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 51;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "NGNumber";
            this.Column3.HeaderText = "NG";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 52;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "Yield";
            this.Column5.HeaderText = "良率%";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 68;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "FalseCall";
            this.Column4.HeaderText = "误判";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.ToolTipText = "FalseCall";
            this.Column4.Width = 57;
            // 
            // chartType
            // 
            this.chartType.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.chartType.BorderlineColor = System.Drawing.Color.Gray;
            chartArea1.BackColor = System.Drawing.Color.Gray;
            chartArea1.Name = "ChartArea1";
            this.chartType.ChartAreas.Add(chartArea1);
            this.chartType.ContextMenuStrip = this.contextMenuStrip1;
            this.chartType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartType.Location = new System.Drawing.Point(0, 0);
            this.chartType.Margin = new System.Windows.Forms.Padding(4);
            this.chartType.Name = "chartType";
            series1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.DiagonalLeft;
            series1.BackImageTransparentColor = System.Drawing.Color.White;
            series1.ChartArea = "ChartArea1";
            series1.Name = "Series1";
            this.chartType.Series.Add(series1);
            this.chartType.Size = new System.Drawing.Size(314, 357);
            this.chartType.TabIndex = 2;
            this.chartType.Text = "chartType";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Nmae";
            this.dataGridViewTextBoxColumn1.HeaderText = "元件名称";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 78;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "OKNumber";
            this.dataGridViewTextBoxColumn2.HeaderText = "OK数量";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 66;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "NGNumber";
            this.dataGridViewTextBoxColumn3.HeaderText = "NG数量";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 66;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "Yield";
            this.dataGridViewTextBoxColumn4.HeaderText = "良率";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 54;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "Cont";
            this.dataGridViewTextBoxColumn5.HeaderText = "总数";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.ToolTipText = "FalseCall";
            this.dataGridViewTextBoxColumn5.Width = 54;
            // 
            // ProductSPCControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label3);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ProductSPCControl";
            this.Size = new System.Drawing.Size(439, 357);
            this.Load += new System.EventHandler(this.ProductSPCControl_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartType)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 查看库统计ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看元件统计ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看元件Top表ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看当天Top表ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看每时表ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导出全部数据到表格ToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem 导出数据ToolStripMenuItem;
        private UI.DataGridViewF.DataGridViewEx dataGridView1;
        private ChartEX chartType;
    }
}
