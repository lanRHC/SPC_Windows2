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
    public class ToolStripDateTimePicker : ToolStripControlHost
    {
        public ToolStripDateTimePicker() : base(new DateTimePicker())
        {
            //this.AutoSize = false;
            //this.Height = 22;
            //GetBase().TickStyle = TickStyle.None;
        }

        public DateTimePicker GetBase()
        {
            return this.Control as DateTimePicker;
        }

        private void InitializeDateTimePickerHost()
        {
        }
    }
}
