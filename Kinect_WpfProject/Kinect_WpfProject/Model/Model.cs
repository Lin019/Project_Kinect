using Kinect_WpfProject.Extends;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Kinect_WpfProject.Model
{
    public class Model
    {
        private ArrayList bodySequence;

        private TimerTool skeletonTimer;
        private TimerTool rgbTimer;

        private KinectModel kinectModel;
        private KinectCamera kinectCamera;

        private ObservableCollection<double> x1, y1, x2, y2;
        private ImageSource image;

        public Model()
        {
            kinectModel = new KinectModel();
            kinectCamera = new KinectCamera();
            bodySequence = new ArrayList();
            skeletonTimer = new TimerTool(Timer_Tick, 0, Common.TIMER_PERIOD);
            rgbTimer = new TimerTool(RGBTimerTick, 0, Common.FRAME_RATE);
            rgbTimer.StartTimer();
        }

        public ObservableCollection<double> GetX1() { return x1; }
        public ObservableCollection<double> GetY1() { return y1; }
        public ObservableCollection<double> GetX2() { return x2; }
        public ObservableCollection<double> GetY2() { return y2; }

        public void LoadSample(string fileName)
        {
            bodySequence = new ArrayList();
            bodySequence = SkeletonFileConvertor.Load(fileName);

            skeletonTimer.StartTimer();
        }

        public void SaveSample()
        {

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
    }
}
