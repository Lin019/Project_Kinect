using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kinect_v2
{
    public partial class MenuForm : Form
    {
        private string btnClick = "";
        
        public MenuForm()
        {
            InitializeComponent();
        }

        public string GetBtnClick()
        {
            return btnClick;
        }

        private void MenuForm_Load(object sender, EventArgs e)
        {

        }

        public void btnHandJoint_Click(object sender, EventArgs e)
        {
            btnClick = "hand";
            this.Close();
        }

        private void btnRecordSample_Click(object sender, EventArgs e)
        {
            btnClick = "record";
            this.Close();
        }
    }
}
