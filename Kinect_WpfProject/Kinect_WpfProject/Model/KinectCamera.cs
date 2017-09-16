using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kinect_WpfProject.Model
{
    public class KinectCamera
    {
        private KinectSensor _sensor;
        private MultiSourceFrameReader _reader;

        public ImageSource rgbImage;

        public KinectCamera()
        {
            OpenCamera();
        }

        public ImageSource GetRGBImage()
        {
            return rgbImage;
        }

        public void StopCamera()
        {
            _sensor.Close();
            _reader.Dispose();
            _reader = null;
        }

        public void OpenCamera()
        {
            _sensor = KinectSensor.GetDefault();
              
            if (_sensor != null)
            {
                _sensor.Open();
                _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color |
                                             FrameSourceTypes.Depth |
                                             FrameSourceTypes.Body);
                _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
            }
        }

        private void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            // Get a reference to the multi-frame
            var reference = e.FrameReference.AcquireFrame();

            // Open color frame
            using (var frame = reference.ColorFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    rgbImage = ToBitmap(frame);
                }
            }

            // Open depth frame
            using (var frame = reference.DepthFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    // Do something with the frame...
                }
            }

            // Open body frame
            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    // Do something with the frame...
                }
            }
        }

        private ImageSource ToBitmap(ColorFrame frame)
        {
            int width = frame.FrameDescription.Width;
            int height = frame.FrameDescription.Height;
            PixelFormat format = PixelFormats.Bgr32;

            byte[] pixels = new byte[width * height * ((PixelFormats.Bgr32.BitsPerPixel + 7) / 8)];

            if (frame.RawColorImageFormat == ColorImageFormat.Bgra)
            {
                frame.CopyRawFrameDataToArray(pixels);
            }
            else
            {
                frame.CopyConvertedFrameDataToArray(pixels, ColorImageFormat.Bgra);
            }

            int stride = width * format.BitsPerPixel / 8;

            return BitmapSource.Create(width, height, 96, 96, format, null, pixels, stride);
        }
    }
}
