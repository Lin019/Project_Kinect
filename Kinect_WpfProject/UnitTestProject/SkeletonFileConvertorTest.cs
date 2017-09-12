using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Kinect_WpfProject;
using System.Collections;

namespace UnitTestProject
{
    [TestClass]
    public class SkeletonFileConvertorTest
    {
        [TestMethod]
        public void GetFirstSampleName()
        {
            string testName = "test042701";

            List<string> names = SkeletonFileConvertor.GetAllSampleNames();
            string name = names[0];

            Assert.AreEqual(testName, name);
        }

        [TestMethod]
        public void Load_FirstSampleHeadJointPoint()
        {
            double testX = 0.228434145450592;
            double testY = 0.520056962966919;
            double testZ = 1.16235268115997;
            string fileName = "test042701";

            ArrayList skeletons = new ArrayList();
            skeletons = SkeletonFileConvertor.Load(fileName);
            Skeleton skeleton = (Skeleton)skeletons[0];

            Assert.AreEqual(testX, skeleton.jointPoints[0].X);
            Assert.AreEqual(testY, skeleton.jointPoints[0].Y);
            Assert.AreEqual(testZ, skeleton.jointPoints[0].Z);
        }
    }
}
