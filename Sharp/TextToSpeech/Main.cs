using System;
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
            speech.Started += speech_Started;
            speech.Stopped += speech_Stopped;
        }

        private void btStartStop_Click(object sender, EventArgs e)
        {
            if (speech.Active)
            {
                speech.Stop();
            }
            else
            {
                speech.Text = mmText.Text;
                speech.Start();
            }
        }

        private void fmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            speech.Stop();
        }

        private void speech_Started(object sender, EventArgs e)
        {
            btStartStop.Text = "Зупинити";
        }

        private void speech_Stopped(object sender, EventArgs e)
        {
            btStartStop.Text = "Читати";
        }
    }
}
