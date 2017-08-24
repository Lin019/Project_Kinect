using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kinect_v2.extends
{
    class Drawer
    {
        private PictureBox picBox;
        bool unExpectedClose = false;

        public Drawer(PictureBox picBox)
        {
            this.picBox = picBox;
        }
        public void DrawSkeleton(Skeleton skeleton)
        {
            JointPoint[] jointPoints = skeleton.jointPoints;

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
            //picBox.Refresh();
            #endregion
            picBox.Refresh();
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
                try
                {
                    using (Graphics g = picBox.CreateGraphics())
                        g.DrawLine(pen, X2, Y2, X1, Y1);
                }
                catch(ObjectDisposedException ex)
                {
                    unExpectedClose = true;
                }
            }

        }

        public bool getUnExpectedClose()
        {
            return unExpectedClose;
        }

    }
}
