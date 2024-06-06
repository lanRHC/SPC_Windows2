using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Vision2.UI.GroupBoxFile
{
    public partial class GroupBoxEx1 : GroupBox
    {


        Button btnLog = null;

        public bool isFold { get; set; }
        //Color waterMarkColor = Color.Red;
        public GroupBoxEx1()
        {
            btnLog = new Button();
            btnLog.BackgroundImageLayout = ImageLayout.Zoom;
            //btnLog.BackgroundImage = Vision2.Properties.Resources.drop_down_vector;
            btnLog.Text = "";
            btnLog.Width = 38;
            this.Controls.Add(btnLog);
            btnLog.Click += GroupBoxEx1_Click;
            btnLog.Location = new Point(Width - btnLog.Width, 0);


            // InitializeComponent();
        }

        public GroupBoxEx1(IContainer container) : this()
        {

            container.Add(this);
        }
        int height;
        DockStyle DockStyle;
        private void GroupBoxEx1_Click(object sender, EventArgs e)
        {
            try
            {
                isFold = isFold ? false:true;
                if (upData != isFold)
                {
                    upData = isFold;
                    if (height == 0)
                    {
                        height = this.Height;
                    }
                    if (isFold)
                    {
                        //btnLog.BackgroundImage = Vision2.Properties.Resources.back_vector;
                        btnLog.Text = "";
                        height = this.Height;
                        this.Height = btnLog.Height - 2;
                        DockStyle = this.Dock;
                        this.Dock = DockStyle.Top;
                    }
                    else
                    {
                        this.Dock = DockStyle;
                        //btnLog.BackgroundImage = Vision2.Properties.Resources.drop_down_vector;
                        btnLog.Text = "";
                        this.Height = height;
                    }
                }
                return;
      
            }
            catch (Exception)
            {
            }
        }
        bool upData = false;
        protected override void OnPaint(PaintEventArgs e)
        {
            if (btnLog != null)
            {
                btnLog.Location = new Point(Width - btnLog.Width, 0);
            }
            base.OnPaint(e);
            //if (upData != isFold)
            //{
            //    upData = isFold;
            //    //this.Dock = DockStyle.Top;
            //    if (!isFold)
            //    {
            //        btnLog.BackgroundImage = Vision2.Properties.Resources.back_vector;
            //        btnLog.Text = "";
            //        height = this.Height;
            //        this.Height = btnLog.Height - 2;
            //    }
            //    else
            //    {
            //        btnLog.BackgroundImage = Vision2.Properties.Resources.drop_down_vector;
            //        btnLog.Text = "";
            //        this.Height = height;
            //    }
            //}

        }


    }



}
