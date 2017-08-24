using Microsoft.Kinect;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kinect_WpfProject.Extends;

namespace Kinect_WpfProject
{
    class SkeletonFileConvertor
    {
        /// <summary>
        /// the path of MyDocuments
        /// </summary>
        private static string myDocPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        /// <summary>
        /// path of the sample gesture file
        /// </summary>
        private static string path = myDocPath + @"\sample gesture.txt";
        private static FileStream file = new FileStream(path, FileMode.OpenOrCreate | FileMode.Append, FileAccess.Write);

        public SkeletonFileConvertor()
        {

        }

        /// <summary>
        /// Save gesture to file
        /// </summary>
        /// <param name="bodySequence">the gesture</param>
        /// <param name="fileName">the name of the gesture</param>
        public void Save(List<Skeleton> bodySequence, string fileName)
        {
            /*bool IsFound = false;
            int line = 0;
            using (StreamReader sr = new StreamReader(path))
            {
                while (!sr.EndOfStream)
                { 
                    if (sr.ReadLine() == @"@" + fileName)
                        IsFound = true;
                    line++;
                }
                sr.Close();
            }*/
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.WriteLine("@" + fileName);
                foreach (Skeleton skeleton in bodySequence)
                {
                    //Console.WriteLine(body.Joints[JointType.WristLeft].Position.X);
                    sw.WriteLine("~");
                    for (int i = 0; i < Common.JOINTS_COUNT; i++)
                        WritePosition(sw, skeleton, i);
                }
                sw.WriteLine("----");
                sw.Close();
            }
            
        }
        private static void WritePosition(StreamWriter sw, Skeleton skeleton, int jointIndex)
        {
            
            string jointName = Enum.GetName(typeof(JointPointType), jointIndex);
            sw.WriteLine(jointName);

            JointPointType item = (JointPointType)jointIndex;

            sw.WriteLine(skeleton.jointPoints[jointIndex].X);
            sw.WriteLine(skeleton.jointPoints[jointIndex].Y);
            sw.WriteLine(skeleton.jointPoints[jointIndex].Z);
        }

        /// <summary>
        /// load bodySequence form file
        /// </summary>
        /// <param name="fileName">the name of the gesture</param>
        /// <returns></returns>
        public static ArrayList Load(string fileName)
        {
            //file.Close();
            ArrayList bodySequence = new ArrayList();
            bool IsFound = false;
            using (StreamReader sr = new StreamReader(path))
            {
                while (!sr.EndOfStream)
                {
                    if (sr.ReadLine() == @"@" + fileName)
                    {
                        IsFound = true;
                        while (sr.Peek() != '-')
                        {
                            //read ~
                            sr.ReadLine();

                            Skeleton skeleton = new Skeleton();

                            while (sr.Peek() != '~')
                            {
                                if (sr.Peek() == '-') break;

                                double x, y, z;
                                JointPointType item = (JointPointType)Enum.Parse(typeof(JointPointType), sr.ReadLine());
                                x = Convert.ToDouble(sr.ReadLine());
                                y = Convert.ToDouble(sr.ReadLine());
                                z = Convert.ToDouble(sr.ReadLine());

                                skeleton.jointPoints[(int)item].SetPoint(x, y, z);
                            }
                            
                            bodySequence.Add(skeleton);
                        }
                    }
                }
                sr.Close();
            }
            if (!IsFound) return null;
            return bodySequence;
        }

        /// <summary>
        /// Load all sample gestures from file
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllSampleNames()
        {
            List<string> all = new List<string>();
            file.Close();
            using (StreamReader sr = new StreamReader(path))
            {
                while (!sr.EndOfStream)
                {
                    if (sr.Read() == '@')
                        all.Add(sr.ReadLine());
                    else
                        sr.ReadLine();
                }
                sr.Close();
            }
            return all; 
        }
    }
}
