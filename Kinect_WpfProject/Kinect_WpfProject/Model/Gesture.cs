using Kinect_WpfProject.Extends;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_WpfProject
{
    class Gesture
    {
        public List<Skeleton> skeletons;
        public string name;
        public List<ArrayList> JointSequence;

        public Gesture(string name)
        {
            this.name = name;
            this.skeletons = SkeletonFileConvertor.Load(name).Cast<Skeleton>().ToList();
            setJointSequence();
        }

        public Gesture(List<Skeleton> skeletons)
        {
            this.skeletons = skeletons;
            setJointSequence();
        }

        private void setJointSequence()
        {
            JointSequence = new List<ArrayList>();
            
            for (int i = 0; i < Common.JOINTS_COUNT; i++)
            {
                JointSequence.Add(new ArrayList());
                for (int j = 0; j < Common.FRAMES_COUNT; j++)
                    JointSequence[i].Add(skeletons[j].jointPoints[i]);
            }
        }
    }
}
