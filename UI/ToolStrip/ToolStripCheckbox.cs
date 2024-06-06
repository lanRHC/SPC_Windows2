using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Vision2.UI.ToolStrip
{

    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
    public class ToolStripCheckbox : ToolStripControlHost
    {
        public ToolStripCheckbox() : base(new CheckBox())
        {
            //this.Control as CheckBox;
        }

        //public CheckBox  CheckBox1 {
        //    get { return checkBoxEX; }

        //    set { checkBoxEX = value; }
        //}
        //CheckBox checkBoxEX;
        public CheckBox GetBase()
        {
            return this.Control as CheckBox;
        }
    }

}
