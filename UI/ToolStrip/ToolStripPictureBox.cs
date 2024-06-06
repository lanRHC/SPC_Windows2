using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Vision2.UI.ToolStrip
{

    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
    public class ToolStripPictureBox : ToolStripControlHost
    {
        public ToolStripPictureBox() : base(new PictureBox())
        {
        }

        [DefaultValue(true)]
        public PictureBox GetBase()
        {
            return this.Control as PictureBox;
        }
    }
}
