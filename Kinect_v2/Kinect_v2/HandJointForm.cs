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
        private FrameDescription frameDes = null;  //frame的描述
        private ColorFrameReader colorReader; //讀取彩色影像

        private Bitmap bmp; //給pictureBox
        private UInt32 size;
        private Rectangle rect;

        private int count = 0;

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
                    bodyFrame.GetAndRefreshBodyData(bodies);
                    dataReceived = true;
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

                        DrawSkeleton(body);
                    }
                }
            }
        }

        public void DrawSkeleton(Body body)
        {
            if (body == null) return;

            foreach (Joint joint in body.Joints.Values)
            {
                //DrawPoint(joint);
            }

            DrawLine(body.Joints[JointType.Head], body.Joints[JointType.Neck]);
            DrawLine(body.Joints[JointType.Neck], body.Joints[JointType.SpineShoulder]);
            DrawLine(body.Joints[JointType.SpineShoulder], body.Joints[JointType.ShoulderLeft]);
            DrawLine(body.Joints[JointType.SpineShoulder], body.Joints[JointType.ShoulderRight]);
            DrawLine(body.Joints[JointType.SpineShoulder], body.Joints[JointType.SpineMid]);
            DrawLine(body.Joints[JointType.ShoulderLeft], body.Joints[JointType.ElbowLeft]);
            DrawLine(body.Joints[JointType.ShoulderRight], body.Joints[JointType.ElbowRight]);
            DrawLine(body.Joints[JointType.ElbowLeft], body.Joints[JointType.WristLeft]);
            DrawLine(body.Joints[JointType.ElbowRight], body.Joints[JointType.WristRight]);
            DrawLine(body.Joints[JointType.WristLeft], body.Joints[JointType.HandLeft]);
            DrawLine(body.Joints[JointType.WristRight], body.Joints[JointType.HandRight]);
            DrawLine(body.Joints[JointType.HandLeft], body.Joints[JointType.HandTipLeft]);
            DrawLine(body.Joints[JointType.HandRight], body.Joints[JointType.HandTipRight]);
            DrawLine(body.Joints[JointType.HandTipLeft], body.Joints[JointType.ThumbLeft]);
            DrawLine(body.Joints[JointType.HandTipRight], body.Joints[JointType.ThumbRight]);
            DrawLine(body.Joints[JointType.SpineMid], body.Joints[JointType.SpineBase]);
            DrawLine(body.Joints[JointType.SpineBase], body.Joints[JointType.HipLeft]);
            DrawLine(body.Joints[JointType.SpineBase], body.Joints[JointType.HipRight]);
            DrawLine(body.Joints[JointType.HipLeft], body.Joints[JointType.KneeLeft]);
            DrawLine(body.Joints[JointType.HipRight], body.Joints[JointType.KneeRight]);
            DrawLine(body.Joints[JointType.KneeLeft], body.Joints[JointType.AnkleLeft]);
            DrawLine(body.Joints[JointType.KneeRight], body.Joints[JointType.AnkleRight]);
            DrawLine(body.Joints[JointType.AnkleLeft], body.Joints[JointType.FootLeft]);
            DrawLine(body.Joints[JointType.AnkleRight], body.Joints[JointType.FootRight]);
        }

        public void DrawPoint(Joint joint)
        {
            SolidBrush brush = new SolidBrush(Color.Red);
            int X1 = (int)(joint.Position.X * pictureBox2.Width + 100);
            int Y1 = (int)(-joint.Position.Y * pictureBox2.Width + 100);
            int X2 = 10;
            int Y2 = 10;

            Rectangle rect = new Rectangle (X1, Y1, X2, Y2);
            

            using (Graphics g = pictureBox2.CreateGraphics())
                g.FillEllipse(brush, rect);

            
        }

        public void DrawLine(Joint first, Joint second)
        {
            if (first.TrackingState == TrackingState.NotTracked || second.TrackingState == TrackingState.NotTracked) return;

            
            Pen pen = new Pen(Color.LightBlue, 8);
            int X1 = (int)(first.Position.X * pictureBox1.Width/1.5 +350);
            int Y1 = (int)(-first.Position.Y * pictureBox1.Height/1.5 +300);
            int X2 = (int)(second.Position.X * pictureBox1.Width/1.5 +350);
            int Y2 = (int)(-second.Position.Y * pictureBox1.Height/1.5 +300);

            using (Graphics g = pictureBox1.CreateGraphics())
                g.DrawLine(pen, X2, Y2, X1, Y1);

            pictureBox1.Invalidate();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void HandJointForm_Paint(object sender, PaintEventArgs e)
        {
            
        }
    }
}
