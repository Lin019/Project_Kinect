using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Kinect;
using System.Collections;
using System.Threading;
using Kinect_v2.extends;
using System.Drawing;
using System.Drawing.Imaging;
using System.Timers;

namespace Kinect_v2
{
    public class KinectPresenter
    {
        public KinectModel model;
        public MenuForm menu;
        public HandJointForm handJointForm;
        public SampleRecordForm sampleRecordForm;

        private int showSampleCount;

        public const int FRAMES_COUNT = 22;
        public const int HANDS_JOINTS = 12;
        private string fileName;

        private int progressValue;
        SkeletonFileConvertor FileConvertor;
        private List<Skeleton> sequence;

        private ArrayList bodySequence;
        private int showSkeletonCount;


        public KinectPresenter() {

            showSampleCount = 0;
            menu = new MenuForm();
            model = new KinectModel();
            FileConvertor = new SkeletonFileConvertor();
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
        bool bodiesTracked;

        private Skeleton skeleton;
        /// <summary>
        /// determine whether to show video or not
        /// </summary>
        private bool flagShowColor, flagShowSkeleton;

        /// <summary>
        /// flag of recording state
        /// </summary>
        private bool recording;

        /// <summary>
        /// flag of loading state
        /// </summary>
        private bool loading;

        private System.Timers.Timer frameCountTimer;

        private Body tempBody;

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

            skeleton = new Skeleton();
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

                    sampleRecordForm.RGBPicBox.Image = bmp;
                    sampleRecordForm.RGBPicBox.Refresh();
                    
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
                        tempBody = body;
                        skeleton.SetJointPoints(tempBody);
                        if (sampleRecordForm.skeletonPicBox != null && !loading)
                        {
                            DrawSkeleton(skeleton);
                            //Console.WriteLine("TRACKED!!");
                            
                        }
                        bodiesTracked = true;
                    }
                }
                if (recording && bodiesTracked)
                {
                    recording = false;
                    bodiesTracked = false;
                    sequence = new List<Skeleton>();
                    frameCountTimer = new System.Timers.Timer();
                    frameCountTimer.Interval = 250;
                    frameCountTimer.Elapsed += new ElapsedEventHandler(frameCountTimer_Tick);
                    frameCountTimer.Enabled = true;
                }
            }
        }

        #endregion

        #region Draw

        private void DrawSkeleton(Skeleton skeleton)
        {
            JointPoint[] jointPoints = skeleton.jointPoints;

            #region DrawLine

            /*DrawLine(jointPoints[(int)JointPointType.Head], jointPoints[(int)JointPointType.Neck]);
            DrawLine(jointPoints[(int)JointPointType.Neck], jointPoints[(int)JointPointType.SpineShoulder]);
            DrawLine(jointPoints[(int)JointPointType.SpineShoulder], jointPoints[(int)JointPointType.ShoulderLeft]);
            DrawLine(jointPoints[(int)JointPointType.SpineShoulder], jointPoints[(int)JointPointType.ShoulderRight]);
            DrawLine(jointPoints[(int)JointPointType.ShoulderLeft], jointPoints[(int)JointPointType.ElbowLeft]);
            DrawLine(jointPoints[(int)JointPointType.ElbowLeft], jointPoints[(int)JointPointType.WristLeft]);
            DrawLine(jointPoints[(int)JointPointType.WristLeft], jointPoints[(int)JointPointType.HandLeft]);
            DrawLine(jointPoints[(int)JointPointType.HandLeft], jointPoints[(int)JointPointType.HandTipLeft]);
            DrawLine(jointPoints[(int)JointPointType.HandLeft], jointPoints[(int)JointPointType.ThumbLeft]);*/
            DrawLine(jointPoints[(int)JointPointType.ShoulderRight], jointPoints[(int)JointPointType.ElbowRight]);
            DrawLine(jointPoints[(int)JointPointType.ElbowRight], jointPoints[(int)JointPointType.WristRight]);
            DrawLine(jointPoints[(int)JointPointType.WristRight], jointPoints[(int)JointPointType.HandRight]);
            DrawLine(jointPoints[(int)JointPointType.HandRight], jointPoints[(int)JointPointType.HandTipRight]);
            DrawLine(jointPoints[(int)JointPointType.HandRight], jointPoints[(int)JointPointType.ThumbRight]);
            /*DrawLine(jointPoints[(int)JointPointType.SpineShoulder], jointPoints[(int)JointPointType.SpineMid]);
            DrawLine(jointPoints[(int)JointPointType.SpineMid], jointPoints[(int)JointPointType.SpineBase]);
            DrawLine(jointPoints[(int)JointPointType.SpineBase], jointPoints[(int)JointPointType.HipLeft]);
            DrawLine(jointPoints[(int)JointPointType.SpineBase], jointPoints[(int)JointPointType.HipRight]);
            DrawLine(jointPoints[(int)JointPointType.HipLeft], jointPoints[(int)JointPointType.KneeLeft]);
            DrawLine(jointPoints[(int)JointPointType.KneeLeft], jointPoints[(int)JointPointType.AnkleLeft]);
            DrawLine(jointPoints[(int)JointPointType.AnkleLeft], jointPoints[(int)JointPointType.FootLeft]);
            DrawLine(jointPoints[(int)JointPointType.HipRight], jointPoints[(int)JointPointType.KneeRight]);
            DrawLine(jointPoints[(int)JointPointType.KneeRight], jointPoints[(int)JointPointType.AnkleRight]);
            DrawLine(jointPoints[(int)JointPointType.AnkleRight], jointPoints[(int)JointPointType.FootRight]);*/
            sampleRecordForm.skeletonPicBox.InvokeIfRequired(refresh);
            #endregion

            
        }
        private void refresh()
        {
            sampleRecordForm.skeletonPicBox.Refresh();
        }
        private void DrawLine(JointPoint first, JointPoint second)
        {
            if (first.IsNull() || second.IsNull()) return;

            Pen pen = new Pen(Color.LightBlue, 8);
            int X1 = (int)(first.X * sampleRecordForm.skeletonPicBox.Width / 1.8 + 150);
            int Y1 = (int)(-first.Y * sampleRecordForm.skeletonPicBox.Height / 1.8 + 150);
            int X2 = (int)(second.X * sampleRecordForm.skeletonPicBox.Width / 1.8 + 150);
            int Y2 = (int)(-second.Y * sampleRecordForm.skeletonPicBox.Height / 1.8 + 150);

            if (sampleRecordForm.skeletonPicBox != null)
            {

                using (Graphics g = sampleRecordForm.skeletonPicBox.CreateGraphics())
                    g.DrawLine(pen, X2, Y2, X1, Y1);

            }

        }

        #endregion

        #region save and load

        /// <summary>
        /// temp.
        /// </summary>
        private bool IsCompare;

        //temp IsCompareClick.
        public void StartRecord(string fileName, bool IsCompareClick)
        {
            this.fileName = fileName;
            recording = true;
            //temp IsCompare.
            IsCompare = IsCompareClick;
        }

        private void frameCountTimer_Tick(object sender, EventArgs e)
        {
            if (sequence.Count < FRAMES_COUNT)
            {
                foreach (Body body in bodies)
                {
                    if (body.IsTracked)
                    {
                        Skeleton tempSke = new Skeleton();
                        tempSke.SetJointPoints(body);
                        //if (sequence.Count >= 1 && body.Joints[JointType.WristLeft].Position.X == sequence.get.Joints[JointType.WristLeft].Position.X) Console.WriteLine("DETECTED!!");
                        sequence.Add(tempSke);
                        //Console.WriteLine(body.Joints[JointType.WristLeft].Position.X);
                    }
                }

                progressValue += 1;
                //Console.WriteLine(sequence.Count + "=" + sequence..Joints[JointType.WristLeft].Position.X);
                Console.WriteLine("Save");
            }
            else if (sequence.Count >= FRAMES_COUNT)
            {
                frameCountTimer.Enabled = false;
                recording = false;
                progressValue = 0;
                Console.WriteLine("----");
                //Console.WriteLine(sequence[2].Joints[JointType.WristLeft].Position.X);
                //Console.WriteLine(sequence[4].Joints[JointType.WristLeft].Position.X);
                Console.WriteLine();

                //temp.
                if (!IsCompare) FileConvertor.Save(sequence, fileName);
                else Recognize(sequence, "test");
                //sequence.Clear();
            }
        }

        public void LoadSample(string fileName, PictureBox pictureBox)
        {
            bodySequence = new ArrayList();
            bodySequence = SkeletonFileConvertor.Load(fileName);

            if (bodySequence == null)
            {
                //error name
                return;
            }
            //  else error.Visible = false;

            showSkeletonCount = 0;
            System.Timers.Timer drawTimer = new System.Timers.Timer();
            drawTimer.Interval = 250;
            drawTimer.Elapsed += new ElapsedEventHandler(DrawTimer_Tick);
            drawTimer.Enabled = true;

            skeleton = new Skeleton();

            if (showSkeletonCount >= FRAMES_COUNT)
            {
                loading = false;
                drawTimer.Enabled = false;
                showSkeletonCount = 0;
                return;
            }
        }

        private void DrawTimer_Tick(object sender, EventArgs e)
        {
            showSkeletonCount++;
            if (showSkeletonCount < FRAMES_COUNT)
            {
                progressValue++;
                skeleton = (Skeleton)bodySequence[showSkeletonCount];
                DrawSkeleton(skeleton);
                sampleRecordForm.skeletonPicBox.InvokeIfRequired(() =>
                {
                    sampleRecordForm.skeletonPicBox.Refresh();
                });
            }
            else if (loading)
            {

                progressValue = 0;
            }
        }
        #endregion

        /// <summary>
        /// Control forms to open or close
        /// </summary>
        public void Main() {

            if (menu.GetBtnClick() == "hand")
            {
                handJointForm = new HandJointForm(this);
                Application.Run(handJointForm);
            }
            else if (menu.GetBtnClick() == "record")
            {
                sampleRecordForm = new SampleRecordForm(this);
                Application.Run(sampleRecordForm);
                
            }
        }

        /// <summary>
        /// Open kinect sensor and reader
        /// </summary>
        public void OpenCamera()
        {
            Form_Load();
        }

        //temp isCompareClick. label.
        public void StartRecord(string fileName, ProgressBar bar, bool IsCompareClick, Label label)
        {
            StartRecord(fileName, IsCompareClick);
            bar.Maximum = KinectModel.FRAMES_COUNT + 1;

            if (progressValue == 0)
                bar.Visible = false;
            else
                bar.Value = progressValue;
        }
        public void OpenFile(string fileName, PictureBox pictureBox, Label error)
        {
            error.Visible = false;
            LoadSample(fileName, sampleRecordForm.skeletonPicBox);


            int counter = 0;
            
            while (counter < 17)
            {
                counter++;
                //pictureBox.Refresh();
                if (counter > 12 && error.Visible == false)
                {
                    error.Text = "DOWN";
                    error.Visible = true;
                }
                else if (error.Visible || counter < 12)
                    Thread.Sleep(400);
                
            }
            counter = 0;       
        }
        
        private void Recognize(List<Skeleton> sequence, String bodypart)
        {
            model.Recognize(sequence, bodypart);
        }

        public void Recognize()
        {
            ArrayList tmpSequence = SkeletonFileConvertor.Load("test042701");
            List<Skeleton> testSequence;
            testSequence = new List<Skeleton>();
            foreach (Skeleton ske in tmpSequence)
            {
                testSequence.Add(ske);
            }
            //model.Recognize(testSequence, "test");
            //model.Recognize(sequence, "test");
        }
    }
}
