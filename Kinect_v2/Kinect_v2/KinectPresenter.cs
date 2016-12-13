using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Kinect;

namespace Kinect_v2
{
    public class KinectPresenter
    {
        public KinectModel model;
        public MenuForm menu;
        public HandJointForm handJointForm;
        public SampleRecordFrom sampleRecordForm;

        public string closedWindow;

        public KinectPresenter() {
            
            menu = new MenuForm();
            model = new KinectModel();
        }

        public void Main() {

            if (menu.GetBtnClick() == "hand")
            {
                handJointForm = new HandJointForm(this);
                Application.Run(handJointForm);
            }
            else if (menu.GetBtnClick() == "record")
            {
                sampleRecordForm = new SampleRecordFrom(this);
                Application.Run(sampleRecordForm);
            }
        }
        public void OpenCamera(PictureBox picBox1, PictureBox picBox2)
        {
            model.Form_Load(picBox1, picBox2);
        }

        public void DrawSkeleton(Body body, PictureBox pictureBox)
        {
            model.DrawSkeleton(body, pictureBox);
        } 
    }
}
