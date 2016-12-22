using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextToSpeech
{
    public partial class fmMain : Form
    {
        Speech speech;

        public fmMain()
        {
            InitializeComponent();
            speech = new Speech();
        }

        private void btStartStop_Click(object sender, EventArgs e)
        {
            speech.Start();
        }
    }
}
