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

        public KinectPresenter() {
            
            menu = new MenuForm();
            model = new KinectModel();
        }

        public void Main() {

            if (menu.GetBtnClick() == "hand")
            {
                handJointForm = new HandJointForm();
                Application.Run(handJointForm);
            }
        }
        
         
    }
}
