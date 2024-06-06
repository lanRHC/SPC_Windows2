namespace Vision2.SPCFile
{
    partial class SPCControl
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
            this.dgv_SPC = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.trV_ComLib = new System.Windows.Forms.TreeView();
            this.txb_componentLib = new System.Windows.Forms.TextBox();
            this.btn_loadComLib = new System.Windows.Forms.Button();
            this.btn_loadBatchComLib = new System.Windows.Forms.Button();
            this.btn_createNewComLib = new System.Windows.Forms.Button();
            this.cmb_proType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txb_comLibName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txb_component = new System.Windows.Forms.TextBox();
            this.btn_loadBatchCom = new System.Windows.Forms.Button();
            this.btn_loadCom = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_SPC)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgv_SPC
            // 
            this.dgv_SPC.AllowUserToAddRows = false;
            this.dgv_SPC.AllowUserToDeleteRows = false;
            this.dgv_SPC.AllowUserToResizeColumns = false;
            this.dgv_SPC.AllowUserToResizeRows = false;
            this.dgv_SPC.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgv_SPC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_SPC.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6});
            this.dgv_SPC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_SPC.Location = new System.Drawing.Point(200, 0);
            this.dgv_SPC.Name = "dgv_SPC";
            this.dgv_SPC.RowTemplate.Height = 23;
            this.dgv_SPC.Size = new System.Drawing.Size(719, 573);
            this.dgv_SPC.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "元件名称";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "总数";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.HeaderText = "OK数";
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.HeaderText = "NG数";
            this.Column4.Name = "Column4";
            // 
            // Column5
            // 
            this.Column5.HeaderText = "良率";
            this.Column5.Name = "Column5";
            // 
            // Column6
            // 
            this.Column6.HeaderText = "不良率";
            this.Column6.Name = "Column6";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(58, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 12);
            this.label1.TabIndex = 1;
            // 
            // trV_ComLib
            // 
            this.trV_ComLib.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trV_ComLib.Location = new System.Drawing.Point(0, 23);
            this.trV_ComLib.Name = "trV_ComLib";
            this.trV_ComLib.Size = new System.Drawing.Size(200, 550);
            this.trV_ComLib.TabIndex = 2;
            this.trV_ComLib.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trV_ComLib_AfterSelect_1);
            // 
            // txb_componentLib
            // 
            this.txb_componentLib.Location = new System.Drawing.Point(126, 120);
            this.txb_componentLib.Name = "txb_componentLib";
            this.txb_componentLib.Size = new System.Drawing.Size(152, 21);
            this.txb_componentLib.TabIndex = 4;
            // 
            // btn_loadComLib
            // 
            this.btn_loadComLib.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btn_loadComLib.Location = new System.Drawing.Point(23, 120);
            this.btn_loadComLib.Name = "btn_loadComLib";
            this.btn_loadComLib.Size = new System.Drawing.Size(75, 23);
            this.btn_loadComLib.TabIndex = 5;
            this.btn_loadComLib.Text = "录入元件库";
            this.btn_loadComLib.UseVisualStyleBackColor = false;
            this.btn_loadComLib.Click += new System.EventHandler(this.btn_loadComLib_Click);
            // 
            // btn_loadBatchComLib
            // 
            this.btn_loadBatchComLib.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btn_loadBatchComLib.Location = new System.Drawing.Point(23, 159);
            this.btn_loadBatchComLib.Name = "btn_loadBatchComLib";
            this.btn_loadBatchComLib.Size = new System.Drawing.Size(106, 23);
            this.btn_loadBatchComLib.TabIndex = 6;
            this.btn_loadBatchComLib.Text = "批量导入元件库";
            this.btn_loadBatchComLib.UseVisualStyleBackColor = false;
            this.btn_loadBatchComLib.Click += new System.EventHandler(this.btn_loadBatchComLib_Click);
            // 
            // btn_createNewComLib
            // 
            this.btn_createNewComLib.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btn_createNewComLib.Location = new System.Drawing.Point(23, 80);
            this.btn_createNewComLib.Name = "btn_createNewComLib";
            this.btn_createNewComLib.Size = new System.Drawing.Size(89, 23);
            this.btn_createNewComLib.TabIndex = 7;
            this.btn_createNewComLib.Text = "创建新元件库";
            this.btn_createNewComLib.UseVisualStyleBackColor = false;
            this.btn_createNewComLib.Click += new System.EventHandler(this.btn_createNewComLib_Click);
            // 
            // cmb_proType
            // 
            this.cmb_proType.FormattingEnabled = true;
            this.cmb_proType.Location = new System.Drawing.Point(113, 37);
            this.cmb_proType.Name = "cmb_proType";
            this.cmb_proType.Size = new System.Drawing.Size(152, 20);
            this.cmb_proType.TabIndex = 8;
            this.cmb_proType.SelectedIndexChanged += new System.EventHandler(this.cmb_proType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "产品类型";
            // 
            // txb_comLibName
            // 
            this.txb_comLibName.Location = new System.Drawing.Point(126, 82);
            this.txb_comLibName.Name = "txb_comLibName";
            this.txb_comLibName.Size = new System.Drawing.Size(139, 21);
            this.txb_comLibName.TabIndex = 10;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txb_component);
            this.groupBox1.Controls.Add(this.btn_loadBatchCom);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btn_loadCom);
            this.groupBox1.Controls.Add(this.txb_comLibName);
            this.groupBox1.Controls.Add(this.txb_componentLib);
            this.groupBox1.Controls.Add(this.btn_createNewComLib);
            this.groupBox1.Controls.Add(this.btn_loadBatchComLib);
            this.groupBox1.Controls.Add(this.btn_loadComLib);
            this.groupBox1.Controls.Add(this.cmb_proType);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox1.Location = new System.Drawing.Point(919, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(290, 573);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "元器件库导入";
            // 
            // txb_component
            // 
            this.txb_component.Location = new System.Drawing.Point(113, 209);
            this.txb_component.Name = "txb_component";
            this.txb_component.Size = new System.Drawing.Size(152, 21);
            this.txb_component.TabIndex = 12;
            // 
            // btn_loadBatchCom
            // 
            this.btn_loadBatchCom.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btn_loadBatchCom.Location = new System.Drawing.Point(23, 249);
            this.btn_loadBatchCom.Name = "btn_loadBatchCom";
            this.btn_loadBatchCom.Size = new System.Drawing.Size(106, 23);
            this.btn_loadBatchCom.TabIndex = 14;
            this.btn_loadBatchCom.Text = "批量导入元件";
            this.btn_loadBatchCom.UseVisualStyleBackColor = false;
            this.btn_loadBatchCom.Click += new System.EventHandler(this.button3_Click);
            // 
            // btn_loadCom
            // 
            this.btn_loadCom.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btn_loadCom.Location = new System.Drawing.Point(23, 207);
            this.btn_loadCom.Name = "btn_loadCom";
            this.btn_loadCom.Size = new System.Drawing.Size(75, 23);
            this.btn_loadCom.TabIndex = 13;
            this.btn_loadCom.Text = "录入元件";
            this.btn_loadCom.UseVisualStyleBackColor = false;
            this.btn_loadCom.Click += new System.EventHandler(this.button4_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(200, 23);
            this.label3.TabIndex = 12;
            this.label3.Text = "元件列表";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.trV_ComLib);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 573);
            this.panel1.TabIndex = 13;
            // 
            // SPC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgv_SPC);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Name = "SPC";
            this.Size = new System.Drawing.Size(1209, 573);
            this.Load += new System.EventHandler(this.SPC_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_SPC)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_SPC;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView trV_ComLib;
        private System.Windows.Forms.TextBox txb_componentLib;
        private System.Windows.Forms.Button btn_loadComLib;
        private System.Windows.Forms.Button btn_loadBatchComLib;
        private System.Windows.Forms.Button btn_createNewComLib;
        private System.Windows.Forms.ComboBox cmb_proType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txb_comLibName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txb_component;
        private System.Windows.Forms.Button btn_loadBatchCom;
        private System.Windows.Forms.Button btn_loadCom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.Panel panel1;
    }
}
