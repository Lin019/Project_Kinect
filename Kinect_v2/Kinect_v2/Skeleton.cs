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
    struct JointPoint {
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
    class Skeleton
    {
        /// <summary>
        /// all the joint point of the body 
        /// </summary>
        public JointPoint[] jointPoints = new JointPoint[25];

        /// <summary>
        /// the pictureBox to show the skeleton
        /// </summary>
        private PictureBox picBox;

        public Skeleton()
        {
            for (int i = 0; i < 25; i++)
            {
                jointPoints[i].SetPoint(0,0,0);        
            }      
        }

        public Skeleton(PictureBox pictureBox)
        {
            for (int i = 0; i < 25; i++)
            {
                jointPoints[i].SetPoint(0, 0, 0);
            }

            picBox = pictureBox;
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
        

        /// <summary>
        /// Draw the skeleton
        /// </summary>
        /// <param name="pictureBox">the pictureBox to show the skeleton</param>
        public void Draw(PictureBox pictureBox)
        {
            picBox = pictureBox;
            
            foreach (JointPoint joint in jointPoints)
            {
                //DrawPoint(joint);
            }

            #region DrawLine
            
            DrawLine(jointPoints[(int)JointPointType.Head], jointPoints[(int)JointPointType.Neck]);
            DrawLine(jointPoints[(int)JointPointType.Neck], jointPoints[(int)JointPointType.SpineShoulder]);
            DrawLine(jointPoints[(int)JointPointType.SpineShoulder], jointPoints[(int)JointPointType.ShoulderLeft]);
            DrawLine(jointPoints[(int)JointPointType.SpineShoulder], jointPoints[(int)JointPointType.ShoulderRight]);
            DrawLine(jointPoints[(int)JointPointType.ShoulderLeft], jointPoints[(int)JointPointType.ElbowLeft]);
            DrawLine(jointPoints[(int)JointPointType.ElbowLeft], jointPoints[(int)JointPointType.WristLeft]);
            DrawLine(jointPoints[(int)JointPointType.WristLeft], jointPoints[(int)JointPointType.HandLeft]);
            DrawLine(jointPoints[(int)JointPointType.HandLeft], jointPoints[(int)JointPointType.HandTipLeft]);
            DrawLine(jointPoints[(int)JointPointType.HandLeft], jointPoints[(int)JointPointType.ThumbLeft]);
            DrawLine(jointPoints[(int)JointPointType.ShoulderRight], jointPoints[(int)JointPointType.ElbowRight]);
            DrawLine(jointPoints[(int)JointPointType.ElbowRight], jointPoints[(int)JointPointType.WristRight]);
            DrawLine(jointPoints[(int)JointPointType.WristLeft], jointPoints[(int)JointPointType.HandLeft]);
            DrawLine(jointPoints[(int)JointPointType.HandRight], jointPoints[(int)JointPointType.HandTipRight]);
            DrawLine(jointPoints[(int)JointPointType.HandRight], jointPoints[(int)JointPointType.ThumbRight]);
            DrawLine(jointPoints[(int)JointPointType.SpineShoulder], jointPoints[(int)JointPointType.SpineMid]);
            DrawLine(jointPoints[(int)JointPointType.SpineMid], jointPoints[(int)JointPointType.SpineBase]);
            DrawLine(jointPoints[(int)JointPointType.SpineBase], jointPoints[(int)JointPointType.HipLeft]);
            DrawLine(jointPoints[(int)JointPointType.SpineBase], jointPoints[(int)JointPointType.HipRight]);
            DrawLine(jointPoints[(int)JointPointType.HipLeft], jointPoints[(int)JointPointType.KneeLeft]);
            DrawLine(jointPoints[(int)JointPointType.KneeLeft], jointPoints[(int)JointPointType.AnkleLeft]);
            DrawLine(jointPoints[(int)JointPointType.AnkleLeft], jointPoints[(int)JointPointType.FootLeft]);
            DrawLine(jointPoints[(int)JointPointType.HipRight], jointPoints[(int)JointPointType.KneeRight]);
            DrawLine(jointPoints[(int)JointPointType.KneeRight], jointPoints[(int)JointPointType.AnkleRight]);
            DrawLine(jointPoints[(int)JointPointType.AnkleRight], jointPoints[(int)JointPointType.FootRight]);
            #endregion
            
        }
        private void DrawPoint(JointPoint joint)
        {
            SolidBrush brush = new SolidBrush(Color.Red);
            int X1 = (int)(joint.X * picBox.Width / 1.8 + 150 -4);
            int Y1 = (int)(-joint.Y * picBox.Width / 1.8 + 150 -4);
            int X2 = 8;
            int Y2 = 8;

            Rectangle rect = new Rectangle(X1, Y1, X2, Y2);

            if (picBox != null)
            {
                using (Graphics g = picBox.CreateGraphics())
                    g.FillEllipse(brush, rect);
            }
        }
        private void DrawLine(JointPoint first, JointPoint second)
        {
            if (first.IsNull() || second.IsNull()) return;

            Pen pen = new Pen(Color.LightBlue, 8);
            int X1 = (int)(first.X * picBox.Width / 1.8 + 150);
            int Y1 = (int)(-first.Y * picBox.Height / 1.8 + 150);
            int X2 = (int)(second.X * picBox.Width / 1.8 + 150);
            int Y2 = (int)(-second.Y * picBox.Height / 1.8 + 150);

            if (picBox != null)
            {
                using (Graphics g = picBox.CreateGraphics())
                {
                    g.DrawLine(pen, X2, Y2, X1, Y1);

                }

                //refersh the skeletonPicBox
                
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
