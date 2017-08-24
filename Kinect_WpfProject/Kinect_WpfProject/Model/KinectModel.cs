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

namespace Kinect_WpfProject
{
    public class KinectModel
    {
        private string myDocPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        SkeletonFileConvertor FileConvertor;

        private Gesture dtwGesture;

        private DtwGestureRecognizer dtw;

        public KinectModel ()
        {
            FileConvertor = new SkeletonFileConvertor();
            dtw = new DtwGestureRecognizer(3, 34, 2, 20);
        }

        #region DTW

        public void Recognize(List<Skeleton> sequence, string bodypart)
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

                dtw.Recognize(seqHands, bodypart);
            }
            else
            {
                Console.WriteLine(dtw.Recognize(sequence));
            }
        }

        #endregion
    }
}
