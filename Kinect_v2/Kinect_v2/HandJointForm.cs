using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Kinect;
using System.Drawing.Imaging;
using System.Reflection;

namespace Kinect_v2
{
    public partial class HandJointForm : Form
    {
        
        private KinectPresenter presenter;

        public HandJointForm(KinectPresenter presenter)
        {
            this.presenter = presenter;
            InitializeComponent();
        }

        private void HandJointForm_Load(object sender, EventArgs e)
        {
            typeof(PictureBox).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, pictureBox2, new object[] { true });

            this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            presenter.OpenCamera(pictureBox1, pictureBox2);
        }

          
    }
}
