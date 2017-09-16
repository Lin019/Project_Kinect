using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Timers;
using System.Threading;
using Kinect_WpfProject.Extends;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Kinect_WpfProject
{
    public class KinectModel
    {
        private string myDocPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        SkeletonFileConvertor FileConvertor;

        private Gesture dtwGesture;

        private DtwGestureRecognizer dtw;

        private KinectSensor _sensor;
        private MultiSourceFrameReader _reader;

        public ImageSource rgbImage;

        public KinectModel ()
        {
            dtw = new DtwGestureRecognizer(3, 34, 2, 20);
            _sensor = KinectSensor.GetDefault();
            OpenCamera();
        }

        public ImageSource GetRGBImage()
        {
            
            return rgbImage;
        }

        private void OpenCamera()
        {
            if (_sensor != null)
            {
                _sensor.Open();
                _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color |
                                             FrameSourceTypes.Depth |
                                             FrameSourceTypes.Body);
                //_reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
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

        private List<ArrayList> AddSequence(string bodypart)
        {
            List<ArrayList> sequence = new List<ArrayList>();
            if (bodypart == "hands")
            {
                sequence.Add(dtwGesture.JointSequence[(int)JointPointType.ShoulderLeft]);
                sequence.Add(dtwGesture.JointSequence[(int)JointPointType.ElbowLeft]);
                sequence.Add(dtwGesture.JointSequence[(int)JointPointType.WristLeft]);
                sequence.Add(dtwGesture.JointSequence[(int)JointPointType.HandLeft]);
                sequence.Add(dtwGesture.JointSequence[(int)JointPointType.HandTipLeft]);
                sequence.Add(dtwGesture.JointSequence[(int)JointPointType.ThumbLeft]);
                sequence.Add(dtwGesture.JointSequence[(int)JointPointType.ShoulderRight]);
                sequence.Add(dtwGesture.JointSequence[(int)JointPointType.ElbowRight]);
                sequence.Add(dtwGesture.JointSequence[(int)JointPointType.WristRight]);
                sequence.Add(dtwGesture.JointSequence[(int)JointPointType.HandRight]);
                sequence.Add(dtwGesture.JointSequence[(int)JointPointType.HandTipRight]);
                sequence.Add(dtwGesture.JointSequence[(int)JointPointType.ThumbRight]);
            }
            return sequence;
        }

        

        #region DTW

        public void Recognize(List<Skeleton> sequence, string bodypart)
        {
            dtwGesture = new Gesture(sequence);
            List<ArrayList> seqHands = new List<ArrayList>();
            seqHands = AddSequence(bodypart);
            dtw.Recognize(sequence);
            
            //Console.WriteLine(dtw.Recognize(sequence));
        }

        public string Recognize(ArrayList bodySequence)
        {
            List<Skeleton> seq = new List<Skeleton>();

            for (int i = 0; i < bodySequence.Count; i++ )
            {
                seq.Add((Skeleton)bodySequence[i]);
            }
            return dtw.Recognize(seq);
        }
        #endregion
    }
}
