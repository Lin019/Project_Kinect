using Kinect_WpfProject.Extends;
using Kinect_WpfProject.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Kinect_WpfProject.ViewModel
{
    class UserGestureRecognizeViewModel:INotifyPropertyChanged
    {
        private ObservableCollection<UIElement> _visibility;
        public ObservableCollection<UIElement> visibility
        {
            get { return _visibility; }
            set
            {
                _visibility = value;
                NotifyPropertyChanged("visibility");
            }
        }

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

        private string _gestureName;
        public string gestureName
        {
            get { return _gestureName; }
            set
            {
                _gestureName = value;
                NotifyPropertyChanged("gestureName");
            }
        }

        private int _recordProgress;
        public int recordProgress
        {
            get { return _recordProgress; }
            set
            {
                _recordProgress = value;
                NotifyPropertyChanged("recordProgress");
            }
        }

        public int recordEnd
        {
            get { return Common.FRAMES_COUNT; }
        }

        private ImageSource _image;
        public ImageSource image
        {
            get { return _image; }
            set
            {
                _image = value;
                NotifyPropertyChanged("image");
            }
        }

        private ArrayList bodySequence;

        private TimerTool skeletonTimer;
        private TimerTool rgbTimer;

        private KinectModel kinectModel;
        private KinectCamera kinectCamera;

        public UserGestureRecognizeViewModel()
        {
            

            kinectModel = new KinectModel();
            kinectCamera = new KinectCamera();
            skeletonTimer = new TimerTool(Timer_Tick, 0, Common.TIMER_PERIOD);
            rgbTimer = new TimerTool(RGBTimerTick, 0, Common.FRAME_RATE);
            rgbTimer.StartTimer();

            x1 = new ObservableCollection<double>();
            y1 = new ObservableCollection<double>();
            x2 = new ObservableCollection<double>();
            y2 = new ObservableCollection<double>();

            _x1 = new ObservableCollection<double>();
            _y1 = new ObservableCollection<double>();
            _x2 = new ObservableCollection<double>();
            _y2 = new ObservableCollection<double>();

            for (int i = 0; i < Common.BONE_COUNT; i++)
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

        private ICommand _Recognize;
        public ICommand Recognize
        {
            get
            {
                if (_Recognize == null)
                {
                    _Recognize = new RelayCommand(
                        this.RecognizeExecute,
                        this.CanRecognizeExecute
                    );
                }
                return _Recognize;
            }
        }
        private bool CanRecognizeExecute()
        {
            return true;
        }
        private void RecognizeExecute()
        {
            kinectCamera.Record();

            kinectModel.Recognize_Test();
        }

        #region Timer

        private void RGBTimerTick()
        {
            image = kinectCamera.GetRGBImage();
            gestureName = kinectCamera.GetFileName();
            recordProgress = kinectCamera.GetRecordProgress();

            if (!skeletonTimer.IsActive())
            {
                Skeleton skeleton = new Skeleton();
                skeleton = kinectCamera.GetSkeleton();
                SetSkeletonLines(skeleton);
            }
        }

        //load skeleton timer
        private void Timer_Tick()
        {
            if (bodySequence.Count > 0)
            {
                Skeleton skeleton = new Skeleton();
                skeleton = (Skeleton)bodySequence[0];
                bodySequence.RemoveAt(0);
                SetSkeletonLines(skeleton);
            }
            else
            {
                skeletonTimer.StopTimer();
                return;
            }
        }

        #endregion

        private void SetSkeletonLines(Skeleton skeleton)
        {
            x1 = new ObservableCollection<double>(skeleton.getDrawingSequences()["x1"]);
            y1 = new ObservableCollection<double>(skeleton.getDrawingSequences()["y1"]);
            x2 = new ObservableCollection<double>(skeleton.getDrawingSequences()["x2"]);
            y2 = new ObservableCollection<double>(skeleton.getDrawingSequences()["y2"]);
        }

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
