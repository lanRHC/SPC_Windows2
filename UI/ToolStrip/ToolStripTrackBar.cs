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
    public class ToolStripTrackBar : ToolStripControlHost
    {
        public ToolStripTrackBar() : base(new TrackBar() { TickStyle=TickStyle.None,AutoSize=false, Height=22})
        {
            //this.AutoSize = false;
            //this.Height = 22;
            //GetBase().TickStyle = TickStyle.None;
        }

        public TrackBar GetBase()
        {
            return this.Control as TrackBar;
        }

        private void InitializeDateTimePickerHost()
        {
        }
    }
}
