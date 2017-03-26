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
using System.IO;
using Newtonsoft.Json;
using System.Runtime.Serialization;


namespace Kinect_v2
{
    public class KinectModel
    {
        private string myDocPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        private const int FRAMES_COUNT = 22;
        private const int HANDS_JOINTS = 12;
        private string fileName;

        private ProgressBar progressBar;
        SkeletonFileConvertor FileConvertor;
        private List<Skeleton> sequence;
        private Body tempBody;

        private ArrayList bodySequence;
        private int showSkeletonCount;

        private bool record;

        private Timer frameCountTimer;

        private Gesture dtwGesture;

        private DtwGestureRecognizer dtw;

        private Label lbl_error;
        /// <summary>
        /// temp.
        /// </summary>
        private bool IsCompare;

        public KinectModel ()
        {
            sequence = new List<Skeleton>();
            FileConvertor = new SkeletonFileConvertor();
            dtw = new DtwGestureRecognizer(3, 0.6, 2, 20);
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
                        tempBody = body;
                        if (skeletonPicBox != null && showSkeleton)
                            DrawSkeleton(body);
                        bodiesTracked = true;
                        
                    }
                }
                if (record && bodiesTracked)
                {
                    record = false;
                    bodiesTracked = false;
                    frameCountTimer = new Timer();
                    frameCountTimer.Interval = 250;
                    frameCountTimer.Start();
                    frameCountTimer.Tick += new EventHandler(frameCountTimer_Tick);
                }
            }
        }

        #endregion

        #region draw

        /// <summary>
        /// Draw the skeleton
        /// </summary>
        /// <param name="body">body data</param>
        /// <param name="pictureBox">the pictureBox to show skeleton</param>
        public void DrawSkeleton(Body body)
        {
            if (body == null) return;

            Skeleton skeleton = new Skeleton(skeletonPicBox);
            skeleton.SetJointPoints(body);
            //skeleton.Draw(skeletonPicBox);
        }
        #endregion

        #region save and load

        //temp IsCompareClick.
        public void StartRecord(string fileName, ProgressBar bar, bool IsCompareClick, Label label)
        {
            lbl_error = label;
            this.fileName = fileName;
            this.progressBar = bar;
            progressBar.Maximum = FRAMES_COUNT + 1;
            record = true;
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
                        Console.WriteLine(body.Joints[JointType.WristLeft].Position.X);                 
                    }
                }
                
                progressBar.Value += 1;
                //Console.WriteLine(sequence.Count + "=" + sequence..Joints[JointType.WristLeft].Position.X);
                Console.WriteLine();
            }
            
            else if (sequence.Count >= FRAMES_COUNT)
            {
                frameCountTimer.Stop();
                record = false;
                progressBar.Value = 0;
                progressBar.Visible = false;
                Console.WriteLine("----");
                //Console.WriteLine(sequence[2].Joints[JointType.WristLeft].Position.X);
                //Console.WriteLine(sequence[4].Joints[JointType.WristLeft].Position.X);
                Console.WriteLine();

                //temp.
                if (!IsCompare) FileConvertor.Save(sequence, fileName);
                else Recognize("hands");
                //sequence.Clear();
            }           
        }
        
        public void LoadSample(string fileName, PictureBox pictureBox, Label error)
        {
            bodySequence = new ArrayList();
            bodySequence = SkeletonFileConvertor.Load(fileName);

            if (bodySequence == null)
            {
                error.Visible = true;
                return;
            }
            else error.Visible = false;
                 
            showSkeletonCount = 0;
            Timer drawTimer = new Timer();
            drawTimer.Interval = 250;
            drawTimer.Start();
            drawTimer.Tick += new EventHandler(DrawTimer_Tick);

            skeletonPicBox = pictureBox;
            skeleton = new Skeleton(skeletonPicBox);

            if (showSkeletonCount >= FRAMES_COUNT)
            {
                drawTimer.Stop();
                showSkeletonCount = 0;
                return;
            }
        }

        private void DrawTimer_Tick(object sender, EventArgs e)
        {
            showSkeletonCount++;
            if (showSkeletonCount < FRAMES_COUNT)
            {   
                skeleton = (Skeleton)bodySequence[showSkeletonCount];
                skeletonPicBox.Refresh();
                skeleton.Draw(skeletonPicBox);
                
            }
        }
        #endregion

        #region DTW

        
        

        public void Recognize(string bodypart)
        {
            dtwGesture = new Gesture(sequence);
            List<ArrayList> seqHands = new List<ArrayList>();

            if (bodypart == "hands")
            {               
                seqHands.Add(dtwGesture.JointSequence[(int)JointPointType.ShoulderLeft]);
                seqHands.Add(dtwGesture.JointSequence[(int)JointPointType.ElbowLeft]);
                seqHands.Add(dtwGesture.JointSequence[(int)JointPointType.WristLeft]);
                seqHands.Add(dtwGesture.JointSequence[(int)JointPointType.HandLeft]);
                seqHands.Add(dtwGesture.JointSequence[(int)JointPointType.HandTipLeft]);
                seqHands.Add(dtwGesture.JointSequence[(int)JointPointType.ThumbLeft]);
                seqHands.Add(dtwGesture.JointSequence[(int)JointPointType.ShoulderRight]);
                seqHands.Add(dtwGesture.JointSequence[(int)JointPointType.ElbowRight]);
                seqHands.Add(dtwGesture.JointSequence[(int)JointPointType.WristRight]);
                seqHands.Add(dtwGesture.JointSequence[(int)JointPointType.HandRight]);
                seqHands.Add(dtwGesture.JointSequence[(int)JointPointType.HandTipRight]);
                seqHands.Add(dtwGesture.JointSequence[(int)JointPointType.ThumbRight]);
                
                lbl_error.Text = dtw.Recognize(seqHands, bodypart);
                lbl_error.Visible = true;
            }
        }

        #endregion
    }
}
