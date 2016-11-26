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

namespace Kinect_v2
{
    public partial class HandJointForm : Form
    {
        private bool formClosed;

        private KinectSensor KsOpen = null; //啟動Kinect感測器
        private FrameDescription frameDes = null;  //影格描述
        private ColorFrameReader colorReader;

        private Bitmap bmp; //給pictureBox
        private UInt32 size;
        private Rectangle rect;

        BodyFrameReader bodyFrameReader = null;
        Body[] bodies = null;

        public HandJointForm()
        {
            formClosed = false;
            InitializeComponent();
        }

        private void HandJointForm_Load(object sender, EventArgs e)
        {

            KsOpen = KinectSensor.GetDefault(); //Kinect v2感測器獲取
            KsOpen.Open();

            frameDes = KsOpen.ColorFrameSource.FrameDescription;
            //RGB攝影機的位元圖格式數據流的數據(1920x1080p)
            bmp = new Bitmap(frameDes.Width, frameDes.Height, PixelFormat.Format32bppRgb);
            rect = new Rectangle(0, 0, frameDes.Width, frameDes.Height);
            size = (uint)(frameDes.Width * frameDes.Height * 4);

            this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            colorReader = KsOpen.ColorFrameSource.OpenReader(); //從感測器接收到的彩色數據中開啟彩色Reader
            colorReader.FrameArrived += colorReader_FrameArrived;
            bodyFrameReader = KsOpen.BodyFrameSource.OpenReader();
            bodyFrameReader.FrameArrived += bodyReader_FrameArrived;

        }

        private void colorReader_FrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {

            //throw new NotImplementedException();
            using (ColorFrame colorFrame = e.FrameReference.AcquireFrame())
            {
                if (colorFrame != null)
                {
                    //.NET Compact Framework 提供 LockBits 方法的支援
                    //這個方法可讓您在未受管理的記憶體緩衝區中操作點陣圖的像素陣列
                    //將點陣圖中的像素換成來自緩衝區的像素。
                    BitmapData cBitmapData = bmp.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);

                    colorFrame.CopyConvertedFrameDataToIntPtr(cBitmapData.Scan0, size, ColorImageFormat.Bgra);
                    bmp.UnlockBits(cBitmapData);

                    pictureBox1.Image = bmp;
                }
            }
        }

        private void bodyReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            bool dataReceived = false;

            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    if (bodies == null) { bodies = new Body[bodyFrame.BodyCount]; }
                    bodyFrame.GetAndRefreshBodyData(bodies); dataReceived = true;
                }
            }

            if (dataReceived)
            {

                foreach (Body body in bodies)
                {
                    if (body.IsTracked)
                    {
                        IReadOnlyDictionary<JointType, Joint> joints = body.Joints;
                        Dictionary<JointType, Point> jointPoints = new Dictionary<JointType, Point>();

                        Joint rightHand = joints[JointType.HandRight];
                        Joint leftHand = joints[JointType.HandLeft];

                        int rightHandX = (int)(rightHand.Position.X * pictureBox1.Width + pictureBox1.Width / 2);
                        int rightHandY = (int)(-rightHand.Position.Y * pictureBox1.Height + pictureBox1.Height / 2);

                        int leftHandX = (int)(leftHand.Position.X * pictureBox1.Width + pictureBox1.Width / 2);
                        int leftHandY = (int)(-leftHand.Position.Y * pictureBox1.Height + pictureBox1.Height / 2);

                        Panel rCircle = new Panel();
                        rCircle.Enabled = false;
                        rCircle.Width = 50;
                        rCircle.Height = 50;
                        rCircle.Location = new Point(rightHandX, rightHandY);
                        rCircle.BackColor = Color.Red;

                        Controls.Add(rCircle);
                        rCircle.BringToFront();
                        rCircle.Dispose();

                        Panel lCircle = new Panel();
                        lCircle.Enabled = false;
                        lCircle.Width = 50;
                        lCircle.Height = 50;
                        lCircle.Location = new Point(leftHandX, leftHandY);
                        lCircle.BackColor = Color.Red;

                        Controls.Add(lCircle);
                        lCircle.BringToFront();
                        lCircle.Dispose();
                    }
                }
            }
        }


    }
}
