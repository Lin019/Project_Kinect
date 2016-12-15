using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kinect_v2
{
    public partial class SampleRecordFrom : Form
    {
        private KinectPresenter presenter;
        private Timer captureCountdownTimer;
        private int captureCountdown;
        private const int CAPTURE_COUNTDOWN = 3;
        private string fileName;

        public SampleRecordFrom(KinectPresenter presenter)
        {
            InitializeComponent();
            this.presenter = presenter;

            captureCountdown = 0;
        }

        private void SampleRecordFrom_Load(object sender, EventArgs e)
        {
            this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            presenter.OpenCamera();
            presenter.ShowColorVideo(pictureBox1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fileName = txtBox_fileName.Text;

            if (fileName != null)
            {
                label_countdown.Enabled = true;
                captureCountdownTimer = new Timer();
                captureCountdownTimer.Interval = 1000;
                captureCountdownTimer.Start();
                captureCountdownTimer.Tick += CaptureCountdown;                
            }
        }

        private void CaptureCountdown(object sender, EventArgs e)
        {
            if (captureCountdown < CAPTURE_COUNTDOWN)
            {
                label_countdown.Text = (CAPTURE_COUNTDOWN - captureCountdown).ToString();
                captureCountdown++;
            }
            else if (captureCountdown >= CAPTURE_COUNTDOWN)
            {
                label_countdown.Text = "0";
                label_countdown.Enabled = false;
                captureCountdownTimer.Stop();
                captureCountdown = 0;
                presenter.StartRecord(fileName, progressBar1);
            }
        }
    }
}
