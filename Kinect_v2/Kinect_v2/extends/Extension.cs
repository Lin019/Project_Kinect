using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kinect_v2.extends
{
    public static class Extension
    {
        public static void InvokeIfRequired(
        this Control control, MethodInvoker action)
        {
            if (control.InvokeRequired)//在非當前執行緒內 使用委派
            {
                control.Invoke(action);
            }
            else
            {
                control.Invoke(action);
            }
        }
    }
}
