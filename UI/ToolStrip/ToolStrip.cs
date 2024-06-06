using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Vision2.UI.ToolStrip
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
    public class ToolStripNumericUpDown : ToolStripControlHost
    {
        public ToolStripNumericUpDown() : base(new NumericUpDown())
        {
        }
        public NumericUpDown GetBase()
        {
            return this.Control as NumericUpDown;
        }


        [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
        public class ToolStripTrackBar : ToolStripControlHost
        {
            public ToolStripTrackBar() : base(new TrackBar())
            {
            }

            public TrackBar GetBase()
            {
                return this.Control as TrackBar;
            }
        }
    }




}