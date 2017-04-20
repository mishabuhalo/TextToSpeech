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

            trTempo.Minimum = Speech.MIN_TEMPO;
            trTempo.Maximum = Speech.MAX_TEMPO;

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
                speech.Tempo = trTempo.Value;
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
            trTempo.Enabled = false;
        }

        private void speech_Stopped(object sender, EventArgs e)
        {
            btStartStop.Text = "Читати";
            trTempo.Enabled = true;
        }

        private void trTempo_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                trTempo.Value = 0;
            }
        }
    }
}
