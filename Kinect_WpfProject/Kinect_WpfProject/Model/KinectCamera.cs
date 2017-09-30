using Kinect_WpfProject.Extends;
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
        private const int INSTR_SAVE = 0;
        private const int INSTR_RECORD = 1;

        private KinectSensor _sensor;
        private MultiSourceFrameReader _reader;

        private ImageSource rgbImage;

        private Skeleton skeleton;
        private Body[] bodies;

        private TimerTool recordTimer;
        private List<Skeleton> recordSquence;
        private List<Skeleton> lastSkeletons;
        private int bodyNumber;
        private int instr;


        private string fileName;

        public KinectCamera()
        {
            recordTimer = new TimerTool(RecordTimerTick, 0, Common.FRAME_RATE);
            lastSkeletons = new List<Skeleton>();
            for (int i = 0; i < 6; i++)
            {
                lastSkeletons.Add(new Skeleton());
            }

            skeleton = new Skeleton();
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

        public Skeleton GetSkeleton()
        {
            if(bodies != null)
            {
                for(int i = 0; i < 6; i++)
                {
                    if (bodies[i].IsTracked)
                    {
                        Skeleton tempSkeleton = new Skeleton();
                        tempSkeleton.SetJointPoints(bodies[i]);

                        if (tempSkeleton.jointPoints[0].X != lastSkeletons[i].jointPoints[0].X)
                        {
                            lastSkeletons[i] = tempSkeleton;
                            skeleton = tempSkeleton;
                            bodyNumber = i;
                        }
                    }
                }
            }
                
            return skeleton;
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
                    //rgbImage = ToBitmap(frame);
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
                    if (bodies == null) bodies = new Body[frame.BodyCount];
                    frame.GetAndRefreshBodyData(bodies);
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

        private void RecordTimerTick()
        {
            if (recordSquence.Count < Common.FRAMES_COUNT)
                recordSquence.Add(lastSkeletons[bodyNumber]);
            else if (instr == INSTR_SAVE)
            {
                recordTimer.StopTimer();
                SkeletonFileConvertor.Save(recordSquence, fileName);
            }
            else if (instr == INSTR_RECORD)
            {
                recordTimer.StopTimer();
                KinectModel kinectModel = new KinectModel();
                fileName = kinectModel.Recognize(recordSquence);
            }
        }

        public void Save(string fileName)
        {
            recordSquence = new List<Skeleton>();
            this.fileName = fileName;
            instr = INSTR_SAVE;
            recordTimer.StartTimer();
        }
        
        public void Record()
        {
            recordSquence = new List<Skeleton>();
            instr = INSTR_RECORD;
            recordTimer.StartTimer();
        }

        public string GetFileName()
        {
            return fileName;
        }

        public List<Skeleton> GetRecordSequence()
        {
            return recordSquence;
        }

        public int GetRecordProgress()
        {
            if (recordSquence == null)
                return 0; 
            return recordSquence.Count;
        }
    }
}
