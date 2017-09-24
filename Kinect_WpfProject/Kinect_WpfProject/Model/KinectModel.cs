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
        
        private List<ArrayList> AddSequence(string bodypart)
        {
            List<ArrayList> sequence = new List<ArrayList>();
            if (bodypart == "hands")
            {
                sequence.Add(dtwGesture.JointSequence[(int)JointPointType.ShoulderLeft]);
                sequence.Add(dtwGesture.JointSequence[(int)JointPointType.ElbowLeft]);
                sequence.Add(dtwGesture.JointSequence[(int)JointPointType.WristLeft]);
                sequence.Add(dtwGesture.JointSequence[(int)JointPointType.HandLeft]);
                sequence.Add(dtwGesture.JointSequence[(int)JointPointType.HandTipLeft]);
                sequence.Add(dtwGesture.JointSequence[(int)JointPointType.ThumbLeft]);
                sequence.Add(dtwGesture.JointSequence[(int)JointPointType.ShoulderRight]);
                sequence.Add(dtwGesture.JointSequence[(int)JointPointType.ElbowRight]);
                sequence.Add(dtwGesture.JointSequence[(int)JointPointType.WristRight]);
                sequence.Add(dtwGesture.JointSequence[(int)JointPointType.HandRight]);
                sequence.Add(dtwGesture.JointSequence[(int)JointPointType.HandTipRight]);
                sequence.Add(dtwGesture.JointSequence[(int)JointPointType.ThumbRight]);
            }
            return sequence;
        }

        

        #region DTW

        public void Recognize(List<Skeleton> sequence, string bodypart)
        {
            dtwGesture = new Gesture(sequence);
            List<ArrayList> seqHands = new List<ArrayList>();
            seqHands = AddSequence(bodypart);
            dtw.Recognize(sequence);
            
            //Console.WriteLine(dtw.Recognize(sequence));
        }

        public string Recognize(List<Skeleton> bodySequence)
        {
            return dtw.Recognize(bodySequence);
        }
        #endregion
    }
}
