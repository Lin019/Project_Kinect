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

        public List<JointPointType> GetErrorJoint(List<Skeleton> userSequence, string fileName)
        {
            return dtw.RecognizeAndGetError(userSequence, fileName);
        }
        #endregion
    }
}
