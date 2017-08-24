using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kinect_v2
{
    public struct JointPoint {
        public double X;
        public double Y;
        public double Z;
        public void SetPoint(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public bool IsNull() { return (X == 0 && Y == 0 && Z == 0) ? true : false; }
    };
    public class Skeleton
    {
        /// <summary>
        /// all the joint point of the body 
        /// </summary>
        public JointPoint[] jointPoints = new JointPoint[25];

        /// <summary>
        /// the pictureBox to show the skeleton
        /// </summary>
        public Skeleton()
        {
            for (int i = 0; i < 25; i++)
            {
                jointPoints[i].SetPoint(0,0,0);        
            }      
        }

        /// <summary>
        /// Convert body into skeleton
        /// </summary>
        /// <param name="body">body data</param>
        public void SetJointPoints(Body body)
        {
            Joint[] joints = new Joint[25];
            #region Assign joints
            joints[0] = body.Joints[JointType.Head];
            joints[1] = body.Joints[JointType.Neck];
            joints[2] = body.Joints[JointType.SpineShoulder];
            joints[3] = body.Joints[JointType.ShoulderLeft];
            joints[4] = body.Joints[JointType.ShoulderRight];
            joints[5] = body.Joints[JointType.ElbowLeft];
            joints[6] = body.Joints[JointType.WristLeft];
            joints[7] = body.Joints[JointType.HandLeft];
            joints[8] = body.Joints[JointType.HandTipLeft];
            joints[9] = body.Joints[JointType.ThumbLeft];
            joints[10] = body.Joints[JointType.ElbowRight];
            joints[11] = body.Joints[JointType.WristRight];
            joints[12] = body.Joints[JointType.HandRight];
            joints[13] = body.Joints[JointType.HandTipRight];
            joints[14] = body.Joints[JointType.ThumbRight];
            joints[15] = body.Joints[JointType.SpineMid];
            joints[16] = body.Joints[JointType.SpineBase];
            joints[17] = body.Joints[JointType.HipLeft];
            joints[18] = body.Joints[JointType.HipRight];
            joints[19] = body.Joints[JointType.KneeLeft];
            joints[20] = body.Joints[JointType.AnkleLeft];
            joints[21] = body.Joints[JointType.FootLeft];
            joints[22] = body.Joints[JointType.KneeRight];
            joints[23] = body.Joints[JointType.AnkleRight];
            joints[24] = body.Joints[JointType.FootRight];
            #endregion

            for (int i = 0; i < 25; i++)
            {
                jointPoints[i].SetPoint(joints[i].Position.X, joints[i].Position.Y, joints[i].Position.Z);
            }
        }
        
    }
    enum JointPointType
    {
        Head=0,
        Neck=1,
        SpineShoulder,
        ShoulderLeft,
        ShoulderRight,
        ElbowLeft,
        WristLeft,
        HandLeft,
        HandTipLeft,
        ThumbLeft,
        ElbowRight,
        WristRight,
        HandRight,
        HandTipRight,
        ThumbRight,
        SpineMid,
        SpineBase,
        HipLeft,
        HipRight,
        KneeLeft,
        AnkleLeft,
        FootLeft,
        KneeRight,
        AnkleRight,
        FootRight
    }
}
