using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPC_Windows.Project
{
    public partial class ProjectForm1 : Form
    {
        public ProjectForm1()
        {
            InitializeComponent();
        }

        private void ProjectForm1_Load(object sender, EventArgs e)
        {
          propertyGrid1.SelectedObject=  ProjectINI.In;
        }
    }
}
