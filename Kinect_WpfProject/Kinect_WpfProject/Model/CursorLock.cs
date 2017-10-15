using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kinect_WpfProject.Model
{
    public class CursorLock
    {
        [DllImport("user32.dll")]
        static extern void ClipCursor(ref Rectangle rect);

        [DllImport("usesr32.dll")]
        static extern void ClipCursor(IntPtr rect);

        public static void LockCursor(Window window)
        {
            var graphics = Graphics.FromHwnd(IntPtr.Zero);
            System.Windows.Point point = window.PointToScreen(new System.Windows.Point(0, 0));
            Rectangle r = new Rectangle((int)point.X, (int)point.Y, 
                (int)(point.X + window.ActualWidth * graphics.DpiX / 96.0 - 22), (int)(point.Y + window.ActualHeight * graphics.DpiY / 96.0 - 56));
            ClipCursor(ref r);
        }
    }
}
