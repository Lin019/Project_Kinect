using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Timers;
using System.Threading;
using Kinect_WpfProject.Extends;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Collections.ObjectModel;

namespace Kinect_WpfProject
{
    public class KinectModel
    {
        private string myDocPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        private Gesture dtwGesture;

        private DtwGestureRecognizer dtw;

        public KinectModel ()
        {
            dtw = new DtwGestureRecognizer(3, 34, 2, 20);
        }

        #region DTW
        public string Recognize(List<Skeleton> bodySequence)
        {
            return dtw.Recognize(bodySequence);
        }

        public void Recognize_Test()
        {
            //Gesture test = new Gesture("right_hand_down_test");
            //dtw.Recognize(test.skeletons, "right_hand_down");
        }

        public ObservableCollection<Visibility> GetErrorJoint(List<Skeleton> userSequence, string fileName)
        {
            List<JointPointType> errorJoints = dtw.RecognizeAndGetError(userSequence, fileName);
            ObservableCollection<Visibility> jointSequence = new ObservableCollection<Visibility>();
            for (int i = 0; i < 12; i++)
                jointSequence.Add(Visibility.Hidden);
            
            if (errorJoints.Contains(JointPointType.ShoulderLeft)) jointSequence[0] = Visibility.Visible;
            if (errorJoints.Contains(JointPointType.ElbowLeft)) jointSequence[1] = Visibility.Visible;
            if (errorJoints.Contains(JointPointType.WristLeft)) jointSequence[2] = Visibility.Visible;
            if (errorJoints.Contains(JointPointType.HandLeft)) jointSequence[3] = Visibility.Visible;
            if (errorJoints.Contains(JointPointType.HandTipLeft)) jointSequence[4] = Visibility.Visible;
            if (errorJoints.Contains(JointPointType.ThumbLeft)) jointSequence[5] = Visibility.Visible;
            if (errorJoints.Contains(JointPointType.ShoulderRight)) jointSequence[6] = Visibility.Visible;
            if (errorJoints.Contains(JointPointType.ElbowRight)) jointSequence[7] = Visibility.Visible;
            if (errorJoints.Contains(JointPointType.WristRight)) jointSequence[8] = Visibility.Visible;
            if (errorJoints.Contains(JointPointType.HandRight)) jointSequence[9] = Visibility.Visible;
            if (errorJoints.Contains(JointPointType.HandTipRight)) jointSequence[10] = Visibility.Visible;
            if (errorJoints.Contains(JointPointType.ThumbRight)) jointSequence[11] = Visibility.Visible;

            return jointSequence;
        }
        #endregion
    }
}
