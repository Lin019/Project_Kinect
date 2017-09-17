﻿using System;
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
using System.Windows.Media;

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

        private ArrayList bodySequence;
        private int showSkeletonCount;

        private TimerTool skeletonTimer;
        private TimerTool rgbTimer;

        private KinectModel kinectModel;
        private KinectCamera kinectCamera;
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

        public SampleRecordViewModel()
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
            skeletonTimer.StartTimer();
        }

        private ICommand _Save;
        public ICommand Save
        {
            get
            {
                if (_Save == null)
                {
                    _Save = new RelayCommand(
                        this.SaveExecute,
                        this.CanSaveExecute
                    );
                }
                return _Save;
            }
        }
        private bool CanSaveExecute()
        {
            return true;
        }
        private void SaveExecute()
        {
            /*
             * WIP: Save file
            */
        }

        #region Timer

        private void RGBTimerTick()
        {
            image = kinectCamera.GetRGBImage();

            if (!skeletonTimer.IsActive())
            {
                Skeleton skeleton = new Skeleton();
                skeleton = kinectCamera.GetSkeleton();
                SetSkeletonLines(skeleton);
            }
        }

        private void Timer_Tick()
        {
            if (showSkeletonCount < Common.FRAMES_COUNT)
            {
                Skeleton skeleton = new Skeleton();
                skeleton = (Skeleton)bodySequence[showSkeletonCount];
                SetSkeletonLines(skeleton);
                showSkeletonCount++;
            }
            else
            {
                skeletonTimer.StopTimer();
                showSkeletonCount = 0;
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
