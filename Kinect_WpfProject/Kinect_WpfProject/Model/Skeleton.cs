using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kinect_WpfProject.Extends;
using System.Collections;

namespace Kinect_WpfProject
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
        public JointPoint[] jointPoints = new JointPoint[Common.JOINTS_COUNT];

        private List<double> x1, x2, y1, y2;

        /// <summary>
        /// the pictureBox to show the skeleton
        /// </summary>
        public Skeleton()
        {
            x1 = new List<double>();
            x2 = new List<double>();
            y1 = new List<double>();
            y2 = new List<double>();

            for (int i = 0; i < Common.JOINTS_COUNT; i++)
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
            Joint[] joints = new Joint[Common.JOINTS_COUNT];
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

            for (int i = 0; i < Common.JOINTS_COUNT; i++)
            {
                jointPoints[i].SetPoint(joints[i].Position.X, joints[i].Position.Y, joints[i].Position.Z);
            }
        }

        public Dictionary<string, List<double>> getDrawingSequences()
        {
            this.DrawSkeleton();
            Dictionary<string, List<double>> sequences = new Dictionary<string,List<double>>();

            sequences.Add("x1", x1);
            sequences.Add("y1", y1);
            sequences.Add("x2", x2);
            sequences.Add("y2", y2);

            return sequences;
        }

        private void DrawSkeleton()
        {
            x1.Clear();
            x2.Clear();
            y1.Clear();
            y2.Clear();

            //Spine
            DrawLine(JointPointType.Head, JointPointType.Neck);
            DrawLine(JointPointType.Neck, JointPointType.SpineShoulder);
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
            x1.Add(first.X * Common.SKELETON_SCALE + Common.SKELETON_POSITION_SHIFT);
            y1.Add( -(first.Y * Common.SKELETON_SCALE) + Common.SKELETON_POSITION_SHIFT);
            x2.Add (second.X * Common.SKELETON_SCALE + Common.SKELETON_POSITION_SHIFT);
            y2.Add( -(second.Y * Common.SKELETON_SCALE) + Common.SKELETON_POSITION_SHIFT);
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
