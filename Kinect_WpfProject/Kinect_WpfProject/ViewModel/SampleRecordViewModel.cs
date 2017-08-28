using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kinect_WpfProject.Model;
using Kinect_WpfProject.Extends;
using System.Collections;
using System.Threading;
using Kinect_WpfProject.View;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows;
using System.Collections.ObjectModel;

namespace Kinect_WpfProject.ViewModel
{
    class SampleRecordViewModel:INotifyPropertyChanged
    {
        private ObservableCollection<double> _x1, _y1, _x2, _y2;

        public ObservableCollection<double> x1 
        {
            get { return _x1; }
            set
            {
                _x1 = value;
                NotifyPropertyChanged("x1");
            }
        }

        public ObservableCollection<double> y1
        {
            get { return _y1; }
            set
            {
                _y1 = value;
                NotifyPropertyChanged("y1");
            }
        }
        public ObservableCollection<double> x2
        {
            get { return _x2; }
            set
            {
                _x2 = value;
                NotifyPropertyChanged("x2");
            }
        }
        public ObservableCollection<double> y2
        {
            get { return _y2; }
            set
            {
                _y2 = value;
                NotifyPropertyChanged("y2");
            }
        }

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
        private int showSkeletonCount;
        
        private int lineCount;

        private JointPoint[] jointPoints;

        private Timer Timer;

        public SampleRecordViewModel()
        {
            x1 = new ObservableCollection<double>();
            y1 = new ObservableCollection<double>();
            x2 = new ObservableCollection<double>();
            y2 = new ObservableCollection<double>();

            _x1 = new ObservableCollection<double>();
            _y1 = new ObservableCollection<double>();
            _x2 = new ObservableCollection<double>();
            _y2 = new ObservableCollection<double>();

            for (int i = 0; i < Common.BONE_COUNT; i++ )
            {
                x1.Add(0);
                y1.Add(0);
                x2.Add(0);
                y2.Add(0);

                _x1.Add(0);
                _y1.Add(0);
                _x2.Add(0);
                _y2.Add(0);
            }

            

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
            StartTimer();

            if (showSkeletonCount >= Common.FRAMES_COUNT)
            {
                StopTimer();
                showSkeletonCount = 0;
                return;
            }
        }

        #region Timer

        public void StartTimer()
        {
            StopTimer();
            Timer = new Timer(x => Timer_Tick(), null, 0, 500);
        }

        public void StopTimer()
        {
            if (Timer != null)
            {
                Timer.Dispose();
                Timer = null;
            }
        }

        private void Timer_Tick()
        {
            if (showSkeletonCount < Common.FRAMES_COUNT)
            {
                
                Skeleton skeleton = new Skeleton();
                skeleton = (Skeleton)bodySequence[showSkeletonCount];
                DrawSkeleton(skeleton);
                showSkeletonCount++;
            }
        }

        #endregion

        #region Draw

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

            x1 = _x1;
            y1 = _y1;
            x2 = _x2;
            y2 = _y2;
        }

        private void DrawLine(JointPointType point1, JointPointType point2)
        {
            SetLine(jointPoints[(int)point1], jointPoints[(int)point2]);
        }

        
        private void SetLine(JointPoint first, JointPoint second)
        {
            _x1[lineCount] = first.X * Common.SKELETON_SCALE + Common.SKELETON_POSITION_SHIFT;
            _y1[lineCount] = -(first.Y * Common.SKELETON_SCALE) + Common.SKELETON_POSITION_SHIFT;
            _x2[lineCount] = second.X * Common.SKELETON_SCALE + Common.SKELETON_POSITION_SHIFT;
            _y2[lineCount] = -(second.Y * Common.SKELETON_SCALE) + Common.SKELETON_POSITION_SHIFT;

            lineCount++;
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
