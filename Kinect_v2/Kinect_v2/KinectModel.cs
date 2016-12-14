using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Kinect_v2
{
    public class KinectModel
    {
        public KinectModel ()
        {
            
        }

        #region Camera

        /// <summary>
        /// kinect sensor
        /// </summary>
        private KinectSensor kinectSensor = null;

        /// <summary>
        /// the description of the frame
        /// </summary>
        private FrameDescription frameDes = null;

        /// <summary>
        /// the reader to get the color frame data
        /// </summary>
        private ColorFrameReader colorReader;

        /// <summary>
        /// bitmap to store the RGB frame
        /// </summary>
        private Bitmap bmp;
        private UInt32 size;
        private Rectangle rect;

        /// <summary>
        /// the reader to get the body data
        /// </summary>
        BodyFrameReader bodyFrameReader = null;

        /// <summary>
        /// store all bodies data
        /// </summary>
        Body[] bodies = null;

        /// <summary>
        /// skeletonPicBox to show the video at View
        /// </summary>
        private PictureBox colorPicBox, skeletonPicBox;

        /// <summary>
        /// determine whether to show video or not
        /// </summary>
        private bool showColor, showSkeleton;

        /// <summary>
        /// initialize all the sensor and reader
        /// </summary>
        public void Form_Load()
        {
            kinectSensor = KinectSensor.GetDefault(); //Kinect v2感測器獲取
            kinectSensor.Open();

            frameDes = kinectSensor.ColorFrameSource.FrameDescription;
            //RGB攝影機的位元圖格式數據流的數據(1920x1080p)
            bmp = new Bitmap(frameDes.Width, frameDes.Height, PixelFormat.Format32bppRgb);
            rect = new Rectangle(0, 0, frameDes.Width, frameDes.Height);
            size = (uint)(frameDes.Width * frameDes.Height * 4);

            //open colorReader and get color data
            colorReader = kinectSensor.ColorFrameSource.OpenReader();
            colorReader.FrameArrived += colorReader_FrameArrived;

            //open bodyReader and get body data
            bodyFrameReader = kinectSensor.BodyFrameSource.OpenReader();
            bodyFrameReader.FrameArrived += bodyReader_FrameArrived;
        }

        /// <summary>
        /// Set the skeletonPicBox to show RGB frames
        /// </summary>
        /// <param name="picBox">the skeletonPicBox to show the RGB frames</param>
        public void SetColorVideoAt(PictureBox picBox)
        {
            colorPicBox = picBox;
            showColor = true;
        }

        /// <summary>
        /// Set the skeletonPicBox to show Skeleton
        /// </summary>
        /// <param name="picBox">the skeletonPicBox to show the skeleton</param>
        public void SetSkeletonAt(PictureBox picBox)
        {
            skeletonPicBox = picBox;
            showSkeleton = true;
        }

        private void colorReader_FrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {

            //throw new NotImplementedException();
            using (ColorFrame colorFrame = e.FrameReference.AcquireFrame())
            {
                if (colorFrame != null && colorPicBox != null && showColor)
                {
                    //.NET Compact Framework 提供 LockBits 方法的支援
                    //這個方法可讓您在未受管理的記憶體緩衝區中操作點陣圖的像素陣列
                    //將點陣圖中的像素換成來自緩衝區的像素。
                    BitmapData cBitmapData = bmp.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);

                    colorFrame.CopyConvertedFrameDataToIntPtr(cBitmapData.Scan0, size, ColorImageFormat.Bgra);
                    bmp.UnlockBits(cBitmapData);

                    
                    colorPicBox.Image = bmp;
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

                        if (skeletonPicBox != null && showSkeleton)
                            DrawSkeleton(body);

                    }
                }
            }
        }

        #endregion

        #region draw

        /// <summary>
        /// Draw the skeleton
        /// </summary>
        /// <param name="body">body data</param>
        /// <param name="skeletonPicBox">the skeletonPicBox to show skeleton</param>
        public void DrawSkeleton(Body body)
        {
            if (body == null) return;

            //Draw all the point of the joints
            foreach (Joint joint in body.Joints.Values)
            {
                DrawPoint(joint);
            }

            //Draw all the line of the body's skeletons
            #region draw lines
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
            #endregion
        }

        /// <summary>
        /// Draw the point of the joints
        /// </summary>
        /// <param name="joint">joints of the body</param>
        public void DrawPoint(Joint joint)
        {
            SolidBrush brush = new SolidBrush(Color.Red);
            int X1 = (int)(joint.Position.X * skeletonPicBox.Width + 100);
            int Y1 = (int)(-joint.Position.Y * skeletonPicBox.Width + 100);
            int X2 = 10;
            int Y2 = 10;

            Rectangle rect = new Rectangle(X1, Y1, X2, Y2);

            if (skeletonPicBox != null)
            {
                using (Graphics g = skeletonPicBox.CreateGraphics())
                    g.FillEllipse(brush, rect);
            }
              
        }

        /// <summary>
        /// Draw the line of the skeleton
        /// </summary>
        /// <param name="first">first joint as the beginning point</param>
        /// <param name="second">second joint as the destination point</param>
        public void DrawLine(Joint first, Joint second)
        {
            if (first.TrackingState == TrackingState.NotTracked || second.TrackingState == TrackingState.NotTracked) return;

            Pen pen = new Pen(Color.LightBlue, 8);
            int X1 = (int)(first.Position.X * skeletonPicBox.Width / 1.8 + 150);
            int Y1 = (int)(-first.Position.Y * skeletonPicBox.Height / 1.8 + 150);
            int X2 = (int)(second.Position.X * skeletonPicBox.Width / 1.8 + 150);
            int Y2 = (int)(-second.Position.Y * skeletonPicBox.Height / 1.8 + 150);

            if (skeletonPicBox != null)
            {
                using (Graphics g = skeletonPicBox.CreateGraphics())
                {
                    g.DrawLine(pen, X2, Y2, X1, Y1);
                    
                }

                //refersh the skeletonPicBox
                skeletonPicBox.Invalidate();
            }
        }

        #endregion
    }
}
