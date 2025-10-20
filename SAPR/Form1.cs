using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WeightPlatePlugin
{
    public partial class WeightPlatePlugin : Form
    {
        private readonly ErrorProvider _errorProvider = new ErrorProvider();

        public WeightPlatePlugin()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxG_Validating(object sender, CancelEventArgs e)
        {
            if (double.TryParse(textBoxG.Text, out var g) &&
                double.TryParse(textBoxT.Text, out var t) &&
                g > t)
            {
                textBoxG.BackColor = Color.LightCoral;
                _errorProvider.SetError(textBoxG, "Глубина G не может превышать толщину T");
                e.Cancel = true;
            }
            else
            {
                textBoxG.BackColor = Color.White;
                _errorProvider.SetError(textBoxG, "");
            }
        }
    }
}
