using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vision2.SPCFile
{
    public partial class MControl1 : UserControl
    {
        public MControl1()
        {
            InitializeComponent();
        }
        Dictionary<string, OKNumberClass> SPCDicProduct = new Dictionary<string, OKNumberClass>();
        public void UpData(Dictionary<string, OKNumberClass> keyValuePairs)
        {
            SPCDicProduct = keyValuePairs;
            try
            {
                panel1.Controls.Clear();
                if (SPCDicProduct != null)
                {
                    foreach (var item in SPCDicProduct)
                    {
                        ProductSPCControl productSPCControls = new ProductSPCControl();
                        productSPCControls.Name = item.Value.ProductName;
    
                        panel1.Controls.Add(productSPCControls);
                        productSPCControls.Width = 500;
                        productSPCControls.Dock = DockStyle.Left;
                        productSPCControls.RefreshData(item.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void MControl1_Load(object sender, EventArgs e)
        {

        }
     
        public void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                panel1.Controls.Clear();
                if (SPCDicProduct!=null)
                {
                    foreach (var item in SPCDicProduct)
                    {
                        ProductSPCControl productSPCControls = new ProductSPCControl();
                        productSPCControls.Name = item.Value.ProductName;
                        panel1.Controls.Add(productSPCControls);
                        productSPCControls.Width = 500;
                        productSPCControls.Dock = DockStyle.Left;
                        productSPCControls.RefreshData(item.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 删除元件计数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("是否清除所有元件？", "清除全部元件计数", MessageBoxButtons.YesNo,
                MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.Yes)
                {
                    SPC.OKNumber.ComponentS.Clear();
                    //label3.Text = SPC.GetSpcText();
                }
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.Message);
            }
        }

        private void 删除产品计数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("是否清除所有元件？", "清除全部元件计数", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.Yes)
                {
                    SPC.ResetDATA();
                    //label3.Text = SPC.GetSpcText();
                }


            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.Message);
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
