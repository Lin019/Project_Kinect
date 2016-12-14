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

        public KinectPresenter() {     
            menu = new MenuForm();
            model = new KinectModel();
        }

        /// <summary>
        /// Control forms to open or close
        /// </summary>
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

        /// <summary>
        /// Open kinect sensor and reader
        /// </summary>
        public void OpenCamera()
        {
            model.Form_Load();
        }

        /// <summary>
        /// show RGB video
        /// </summary>
        /// <param name="pictureBox">the pictureBox to show RGB video</param>
        public void ShowColorVideo(PictureBox pictureBox)
        {
            model.SetColorVideoAt(pictureBox);
        }

        /// <summary>
        /// show skeleton
        /// </summary>
        /// <param name="pictureBox">the pictureBox to show skeleton</param>
        public void ShowSkeletonVideo(PictureBox pictureBox)
        {
            model.SetSkeletonAt(pictureBox);
        } 
    }
}
