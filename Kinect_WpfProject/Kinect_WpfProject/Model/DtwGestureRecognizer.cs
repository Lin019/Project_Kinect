﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kinect_WpfProject.Extends;

namespace Kinect_WpfProject
{
    class DtwGestureRecognizer
    {
        private readonly int _dimension;

        /// <summary>
        /// Maximum distance between the last observations of each sequence.
        /// </summary>
        private readonly double _firstThreshold;

        /// <summary>
        /// Minimum length of a gesture before it can be recognised
        /// </summary>
        private readonly double _minimumLength;

        /// <summary>
        /// Maximum DTW distance between an example and a sequence being classified.
        /// </summary>
        private readonly double _globalThreshold;

        /// <summary>
        /// The gesture names. Index matches that of the sequences array in _sequences
        /// </summary>
        private readonly ArrayList _labels;

        /// <summary>
        /// Maximum vertical or horizontal steps in a row.
        /// </summary>
        private readonly int _maxSlope;

        /// <summary>
        /// The recorded gesture sequences
        /// </summary>
        private readonly List<Gesture> _sampleGestures;

        private List<ArrayList> seqJointPoint;

        private readonly ArrayList _sequences;

        /// <summary>
        /// Initializes a new instance of the DtwGestureRecognizer class
        /// First DTW constructor
        /// </summary>
        /// <param name="dim">Vector size</param>
        /// <param name="threshold">Maximum distance between the last observations of each sequence</param>
        /// <param name="firstThreshold">Minimum threshold</param>
        public DtwGestureRecognizer(int dim, double threshold, double firstThreshold, double minLen)
        {
            _sampleGestures = new List<Gesture>();

            List<string> allSampleNames = new List<string>();
            allSampleNames = SkeletonFileConvertor.GetAllSampleNames();
            Gesture gesture;
            for (int i = 0; i < allSampleNames.Count; i++)
            {
                gesture = new Gesture(allSampleNames[i]);
                _sampleGestures.Add(gesture);
            }

            _dimension = dim;

            _labels = new ArrayList();
            _globalThreshold = threshold;
            _firstThreshold = firstThreshold;
            _maxSlope = int.MaxValue;
            _minimumLength = minLen;
        }

        /// <summary>
        /// Initializes a new instance of the DtwGestureRecognizer class
        /// Second DTW constructor
        /// </summary>
        /// <param name="dim">Vector size</param>
        /// <param name="threshold">Maximum distance between the last observations of each sequence</param>
        /// <param name="firstThreshold">Minimum threshold</param>
        /// <param name="ms">Maximum vertical or horizontal steps in a row</param>
        public DtwGestureRecognizer(int dim, double threshold, double firstThreshold, int ms, double minLen)
        {
            _dimension = dim;
            _sampleGestures = new List<Gesture>();
            _sequences = new ArrayList();
            _sequences = SkeletonFileConvertor.Load("test042701");
            _labels = new ArrayList();
            _globalThreshold = threshold;
            _firstThreshold = firstThreshold;
            _maxSlope = ms;
            _minimumLength = minLen;
        }

        public string Recognize(List<Skeleton> sequence)
        {

            Gesture test = new Gesture(sequence);
            List<Gesture> examples = new List<Gesture>();
            List<string> exampleNames = new List<string>();
            exampleNames = SkeletonFileConvertor.GetAllSampleNames();

            for (int i = 0; i < exampleNames.Count; i++)
            {
                examples.Add(new Gesture(exampleNames[i]));
            }

            double d, maxDist = double.PositiveInfinity;
            double minDist = double.PositiveInfinity;
            string name = "", finalName = "";
            for (int j = 0; j < examples.Count; j++)
            {
                Console.WriteLine("@" + exampleNames[j]);
                for (int i = 0; i < Common.JOINTS_COUNT; i++)
                {
                    d = Dtw(test.JointSequence[i], examples[j].JointSequence[i]);
                    Console.WriteLine(d);
                    if (d < maxDist) maxDist = d;
                }
                Console.WriteLine("------------");
                if (maxDist < minDist && maxDist != 0)
                {
                    minDist = maxDist;
                    name = examples[j].name;
                }
                finalName = name;
            }
            Console.WriteLine("===========");
            Console.WriteLine(minDist);
            Console.WriteLine(finalName);
            return (minDist < _globalThreshold ? finalName : "__UNKNOWN");
        }


        public void Recognize(List<Skeleton> userSequence, string fileName)
        {
            Gesture userGesture = new Gesture(userSequence);
            Gesture sampleGesture = new Gesture(fileName);

            List<JointPointType> handsjoints = GetPartialJoint(fileName);

            for (int i = 0; i < Common.JOINTS_COUNT; i++)
            {
                for (int j = 0; j < handsjoints.Count; j++)
                {
                    if (i == (int)handsjoints[j])
                        Console.WriteLine(Dtw(userGesture.JointSequence[i], sampleGesture.JointSequence[i]));
                }
            }
        }

        public List<JointPointType> RecognizeAndGetError(List<Skeleton> userSequence, string fileName)
        {
            Gesture userGesture = new Gesture(userSequence);
            Gesture sampleGesture = new Gesture(fileName);

            List<JointPointType> handsjoints = GetPartialJoint(fileName);

            List<JointPointType> errorJoints = new List<JointPointType>();
            double d;
            for (int i = 0; i < Common.JOINTS_COUNT; i++)
            {
                for (int j = 0; j < handsjoints.Count; j++)
                {
                    if (i == (int)handsjoints[j])
                    {
                        d = Dtw(userGesture.GetPartialJointSequence(userGesture.skeletons.Count - Common.RECOGNIZE_INTERNAL, userGesture.skeletons.Count)[i],
                                sampleGesture.GetPartialJointSequence(userGesture.skeletons.Count - Common.RECOGNIZE_INTERNAL, userGesture.skeletons.Count)[i]);
                        Console.WriteLine(d);
                        if ((double)d > (double)_globalThreshold / ((double)Common.FRAMES_COUNT / (double)Common.RECOGNIZE_INTERNAL))
                            errorJoints.Add((JointPointType)i);
                    }

                }
            }

            return errorJoints;
        }

        private List<JointPointType> GetPartialJoint(string fileName)
        {
            List<JointPointType> handsJoints = new List<JointPointType>();
            string[] bodyPart = fileName.Split('_');
            
            if (bodyPart[0] == "Elbow" || fileName == "Shoulder_internal_rotation" || fileName == "Shoulder_external_rotation")
            {
                handsJoints.Add(JointPointType.ElbowLeft);
                handsJoints.Add(JointPointType.WristLeft);

                handsJoints.Add(JointPointType.ElbowRight);
                handsJoints.Add(JointPointType.WristRight);
            }
            else if (bodyPart[0] == "Shoulder")
            {
                handsJoints.Add(JointPointType.ShoulderLeft);
                handsJoints.Add(JointPointType.ElbowLeft);

                handsJoints.Add(JointPointType.ShoulderRight);
                handsJoints.Add(JointPointType.ElbowRight);
            }
            else if (bodyPart[0] == "Forearm" || bodyPart[0] == "Wrist")
            {
                handsJoints.Add(JointPointType.WristLeft);
                handsJoints.Add(JointPointType.HandLeft);
                handsJoints.Add(JointPointType.HandTipLeft);
                handsJoints.Add(JointPointType.ThumbLeft);

                handsJoints.Add(JointPointType.WristRight);
                handsJoints.Add(JointPointType.HandRight);
                handsJoints.Add(JointPointType.HandTipRight);
                handsJoints.Add(JointPointType.ThumbRight);
            }
            else
            {
                handsJoints.Add(JointPointType.ShoulderRight);
                handsJoints.Add(JointPointType.ElbowRight);
                handsJoints.Add(JointPointType.WristRight);
                handsJoints.Add(JointPointType.HandRight);
                handsJoints.Add(JointPointType.HandTipRight);
                handsJoints.Add(JointPointType.ThumbRight);
            }

            return handsJoints;
        }

        /// <summary>
        /// Compute the min DTW distance between seq2 and all possible endings of seq1.
        /// </summary>
        /// <param name="seq1">The first array of sequences to compare</param>
        /// <param name="seq2">The second array of sequences to compare</param>
        /// <returns>The best match</returns>
        public double Dtw(ArrayList seq1, ArrayList seq2)
        {
            // Init
            var seq1R = new ArrayList(seq1);
            seq1R.Reverse();
            var seq2R = new ArrayList(seq2);
            seq2R.Reverse();
            var tab = new int[seq1R.Count + 1, seq2R.Count + 1];
            var slopeI = new int[seq1R.Count + 1, seq2R.Count + 1];
            var slopeJ = new int[seq1R.Count + 1, seq2R.Count + 1];

            for (int i = 0; i < seq1R.Count + 1; i++)
            {
                for (int j = 0; j < seq2R.Count + 1; j++)
                {
                    tab[i, j] = int.MaxValue;
                    slopeI[i, j] = 0;
                    slopeJ[i, j] = 0;
                }
            }

            tab[0, 0] = 0;

            // Dynamic computation of the DTW matrix.
            for (int i = 1; i < seq1R.Count + 1; i++)
            {
                for (int j = 1; j < seq2R.Count + 1; j++)
                {
                    if (tab[i, j - 1] < tab[i - 1, j - 1] && tab[i, j - 1] < tab[i - 1, j] &&
                        slopeI[i, j - 1] < _maxSlope)
                    {
                        tab[i, j] = (int)Dist2((JointPoint)seq1R[i - 1], (JointPoint)seq2R[j - 1]) + tab[i, j - 1];
                        slopeI[i, j] = slopeJ[i, j - 1] + 1;
                        slopeJ[i, j] = 0;
                    }
                    else if (tab[i - 1, j] < tab[i - 1, j - 1] && tab[i - 1, j] < tab[i, j - 1] &&
                             slopeJ[i - 1, j] < _maxSlope)
                    {
                        tab[i, j] = (int)Dist2((JointPoint)seq1R[i - 1], (JointPoint)seq2R[j - 1]) + tab[i - 1, j];
                        slopeI[i, j] = 0;
                        slopeJ[i, j] = slopeJ[i - 1, j] + 1;
                    }
                    else
                    {
                        tab[i, j] = (int)Dist2((JointPoint)seq1R[i - 1], (JointPoint)seq2R[j - 1]) + tab[i - 1, j - 1];
                        slopeI[i, j] = 0;
                        slopeJ[i, j] = 0;
                    }
                }
            }

            double minDist = 0;
            int x = 0, y = 0;

            //temp
            //var testTable = new int[100, 100];

            while (x != seq1R.Count || y != seq2R.Count)
            {
                //try { testTable[x, y] = 1; } catch { }
                if (y == seq1R.Count)
                {
                    minDist++;
                    x++;
                }
                else if (x == seq1R.Count)
                {
                    minDist++;
                    y++;
                }
                else if (tab[x + 1, y + 1] <= tab[x + 1, y] && tab[x + 1, y + 1] <= tab[x, y + 1])
                {
                    minDist++;
                    x++;
                    y++;
                }
                else if (tab[x + 1, y] < tab[x, y + 1])
                {
                    minDist++;
                    x++;
                }
                else
                {
                    minDist++;
                    y++;
                }
            }
            return minDist;
        }

        /// <summary>
        /// Computes a 1-distance between two observations. (aka Manhattan distance).
        /// </summary>
        /// <param name="a">Point a (double)</param>
        /// <param name="b">Point b (double)</param>
        /// <returns>Manhattan distance between the two points</returns>
        private double Dist1(double[] a, double[] b)
        {
            double d = 0;
            for (int i = 0; i < _dimension; i++)
            {
                d += Math.Abs(a[i] - b[i]);
            }

            return d;
        }

        /// <summary>
        /// Computes a 2-distance between two observations. (aka Euclidean distance).
        /// </summary>
        /// <param name="a">Point a (double)</param>
        /// <param name="b">Point b (double)</param>
        /// <returns>Euclidian distance between the two points</returns>
        private double Dist2(JointPoint a, JointPoint b)
        {
            double d = 0;
            int ratio = 10;
            d += Math.Pow(ratio * a.X - ratio * b.X, 2);
            d += Math.Pow(ratio * a.Y - ratio * b.Y, 2);

            return Math.Sqrt(d);
        }
    }
}
