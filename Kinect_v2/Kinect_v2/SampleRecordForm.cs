using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kinect_v2
{
    public partial class SampleRecordForm : Form
    {
        private KinectPresenter presenter;
        private Timer captureCountdownTimer;
        private int captureCountdown;
        private const int CAPTURE_COUNTDOWN = 3;
        private string fileName;

        private bool IsCompareClick;

        public SampleRecordForm(KinectPresenter presenter)
        {
            InitializeComponent();
            this.presenter = presenter;
            progressBar1.Visible = false;
            label_countdown.Visible = false;
            label_error.Visible = false;
            captureCountdown = 0;

            typeof(PictureBox).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, pictureBox2, new object[] { true });
        }

        private void SampleRecordFrom_Load(object sender, EventArgs e)
        {
            this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            presenter.OpenCamera();
            presenter.ShowColorVideo(pictureBox1);
            presenter.ShowSkeletonVideo(pictureBox2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fileName = txtBox_fileName.Text;
            IsCompareClick = false;

            if (fileName != null)
            {               
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
                label_countdown.Visible = true;
                label_countdown.Text = (CAPTURE_COUNTDOWN - captureCountdown++).ToString();
                
            }
            else if (captureCountdown >= CAPTURE_COUNTDOWN)
            {
                progressBar1.Visible = true;
                label_countdown.Text = "0";
                label_countdown.Visible = false;
                captureCountdownTimer.Stop();
                captureCountdown = 0;
                //temp iscompareclick.
                presenter.StartRecord(fileName, progressBar1, IsCompareClick, label_error);
                //presenter.Recognize(label_error);
            }
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            fileName = txtBox_fileName.Text;
            presenter.OpenFile(fileName, pictureBox2, label_error);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            IsCompareClick = true;

            captureCountdownTimer = new Timer();
            captureCountdownTimer.Interval = 1000;
            captureCountdownTimer.Start();
            captureCountdownTimer.Tick += CaptureCountdown;
        }
    }
}
