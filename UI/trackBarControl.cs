using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vision2.UI
{
    public partial class trackBarControl : UserControl
    {
        public trackBarControl()
        {
            InitializeComponent();
            trackBar3.ValueChanged += TrackBar3_ValueChanged;
            textBox3.TextChanged += TextBox3_TextChanged;
        }
        public double Value;
        private void TextBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Value = Convert.ToDouble(textBox3.Text);
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception)
            {

            }

        }

        private void TrackBar3_ValueChanged(object sender, EventArgs e)
        {

            //ValueChanged?.Invoke(this, EventArgs.Empty);
            textBox3.Text = trackBar3.Value.ToString();

        }

        public event EventHandler ValueChanged;

        public void SetData(object data, string dataName, string text, int min, int max)
        {
            try
            {
                label3.Text = text;
                Binding bind1 = new Binding("Text", data, dataName);
                textBox3.DataBindings.Clear();
                textBox3.DataBindings.Add(bind1);
                trackBar3.Minimum = min;
                trackBar3.Maximum = max;
                bind1 = new Binding("Value", data, dataName);
                trackBar3.DataBindings.Clear();
                trackBar3.DataBindings.Add(bind1);



            }
            catch (Exception)
            {

            }
        }


    }
}
