using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Kinect;
using System.Drawing.Imaging;

namespace Kinect_v2
{
    public partial class HandJointForm : Form
    {
        private KinectSensor KsOpen = null; //啟動Kinect感測器
        private FrameDescription frameDes = null;  //影格描述
        private ColorFrameReader colorReader;

        private Bitmap bmp; //給pictureBox
        private UInt32 size;
        private Rectangle rect;

        public HandJointForm()
        {
            InitializeComponent();
        }

        private void HandJointForm_Load(object sender, EventArgs e)
        {
            KsOpen = KinectSensor.GetDefault(); //Kinect v2感測器獲取

            frameDes = KsOpen.ColorFrameSource.FrameDescription;
            //RGB攝影機的位元圖格式數據流的數據(1920x1080p)
            bmp = new Bitmap(frameDes.Width, frameDes.Height, PixelFormat.Format32bppRgb);
            rect = new Rectangle(0, 0, frameDes.Width, frameDes.Height);
            size = (uint)(frameDes.Width * frameDes.Height * 4);

            this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            colorReader = KsOpen.ColorFrameSource.OpenReader(); //從感測器接收到的彩色數據中開啟彩色Reader
            colorReader.FrameArrived += colorReader_FrameArrived;
            KsOpen.Open();
        }

        private void colorReader_FrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            //throw new NotImplementedException();
            using (ColorFrame colorFrame = e.FrameReference.AcquireFrame())
            {
                if (colorFrame != null)
                {
                    //.NET Compact Framework 提供 LockBits 方法的支援
                    //這個方法可讓您在未受管理的記憶體緩衝區中操作點陣圖的像素陣列
                    //將點陣圖中的像素換成來自緩衝區的像素。
                    BitmapData cBitmapData = bmp.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);

                    colorFrame.CopyConvertedFrameDataToIntPtr(cBitmapData.Scan0, size, ColorImageFormat.Bgra);
                    bmp.UnlockBits(cBitmapData);

                    pictureBox1.Image = bmp;
                }
            }
        }



                
            
    }
}
