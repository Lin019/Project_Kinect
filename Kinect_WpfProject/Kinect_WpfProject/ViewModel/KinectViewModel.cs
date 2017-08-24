using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kinect_WpfProject.Model;
using Kinect_WpfProject.Extends;
using System.Collections;
using System.Timers;
using Kinect_WpfProject.View;
using System.Windows.Input;

namespace Kinect_WpfProject.ViewModel
{
    class KinectViewModel
    {
        public double[] x1 { get; set; }
        public double[] y1 { get; set; }
        public double[] x2 { get; set; }
        public double[] y2 { get; set; }

        public string fileName { get; set; }

        private ICommand _LoadSample;
        public ICommand LoadSample 
        { 
            get 
            {
                if (_LoadSample == null)
                {
                    _LoadSample = new RelayCommand(
                        this.LoadSampleExecute,
                        this.CanLoadSampleExecute
                    );
                }
                return _LoadSample;
            } 
        }

        private ArrayList bodySequence;
        private Skeleton skeleton;
        private int showSkeletonCount;
        
        private Boolean loading;
        private int progressValue;
        private int lineCount;

        private JointPoint[] jointPoints;

        public KinectViewModel()
        {
            x1 = new double[Common.BONE_COUNT];
            y1 = new double[Common.BONE_COUNT];
            x2 = new double[Common.BONE_COUNT];
            y2 = new double[Common.BONE_COUNT];
            bodySequence = new ArrayList();
        }

        private bool CanLoadSampleExecute()
        {
            try
            {
                bodySequence = SkeletonFileConvertor.Load(fileName);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private void LoadSampleExecute()
        {
            bodySequence = new ArrayList();

            bodySequence = SkeletonFileConvertor.Load(fileName);
            
            showSkeletonCount = 0;
            System.Timers.Timer drawTimer = new System.Timers.Timer();
            drawTimer.Interval = 250;
            drawTimer.Elapsed += new ElapsedEventHandler(DrawTimer_Tick);
            drawTimer.Enabled = true;

            skeleton = new Skeleton();

            if (showSkeletonCount >= Common.FRAMES_COUNT)
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
            if (showSkeletonCount < Common.FRAMES_COUNT)
            {
                progressValue++;
                skeleton = (Skeleton)bodySequence[showSkeletonCount];
                DrawSkeleton(skeleton);
            }
            else if (loading)
            {
                progressValue = 0;
            }
        }

        private void DrawSkeleton(Skeleton skeleton)
        {
            jointPoints = skeleton.jointPoints;
            lineCount = 0;

            //Spine
            DrawLine(JointPointType.Head, JointPointType.Neck);
            DrawLine(JointPointType.Head, JointPointType.SpineShoulder);
            DrawLine(JointPointType.SpineShoulder, JointPointType.SpineMid);
            DrawLine(JointPointType.SpineMid, JointPointType.SpineBase);

            //Left arm
            DrawLine(JointPointType.SpineShoulder, JointPointType.ShoulderLeft);
            DrawLine(JointPointType.ShoulderLeft, JointPointType.ElbowLeft);
            DrawLine(JointPointType.ElbowLeft, JointPointType.WristLeft);
            DrawLine(JointPointType.WristLeft, JointPointType.HandLeft);
            DrawLine(JointPointType.HandLeft, JointPointType.HandTipLeft);
            DrawLine(JointPointType.HandLeft, JointPointType.ThumbLeft);

            //Right arm
            DrawLine(JointPointType.SpineShoulder, JointPointType.ShoulderRight);
            DrawLine(JointPointType.ShoulderRight, JointPointType.ElbowRight);
            DrawLine(JointPointType.ElbowRight, JointPointType.WristRight);
            DrawLine(JointPointType.WristRight, JointPointType.HandRight);
            DrawLine(JointPointType.HandRight, JointPointType.HandTipRight);
            DrawLine(JointPointType.HandRight, JointPointType.ThumbRight);

            //Left leg
            DrawLine(JointPointType.SpineBase, JointPointType.HipLeft);
            DrawLine(JointPointType.HipLeft, JointPointType.KneeLeft);
            DrawLine(JointPointType.KneeLeft, JointPointType.AnkleLeft);
            DrawLine(JointPointType.AnkleLeft, JointPointType.FootLeft);

            //Right leg
            DrawLine(JointPointType.SpineBase, JointPointType.HipRight);
            DrawLine(JointPointType.HipRight, JointPointType.KneeRight);
            DrawLine(JointPointType.KneeRight, JointPointType.AnkleRight);
            DrawLine(JointPointType.AnkleRight, JointPointType.FootRight);
        }

        private void DrawLine(JointPointType point1, JointPointType point2)
        {
            SetLine(jointPoints[(int)point1], jointPoints[(int)point2]);
        }

        
        private void SetLine(JointPoint first, JointPoint second)
        {
            x1[lineCount] = first.X;
            y1[lineCount] = first.Y;
            x2[lineCount] = second.X;
            y2[lineCount] = second.Y;

            lineCount++;
        }
    }
}
