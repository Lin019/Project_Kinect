using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_v2
{
    class Gesture
    {
        public List<Skeleton> skeletons;
        private const int FRAMES_COUNT = 22;
        public string name;
        public List<ArrayList> JointSequence;
        private ArrayList temp;
        public Gesture(string name)
        {
            this.name = name;
            
            skeletons = SkeletonFileConvertor.Load(name).Cast<Skeleton>().ToList();
            JointSequence = new List<ArrayList>(25);
            
            for (int i = 0; i < 25; i++)
            {               
                JointSequence.Add(new ArrayList());
                for (int j = 0; j < FRAMES_COUNT; j++)
                    JointSequence[i].Add(skeletons[j].jointPoints[i]);                
            }
        }

        public Gesture(List<Skeleton> skeletons)
        {
            JointSequence = new List<ArrayList>();
            
            for (int i = 0; i < 25; i++)
            {
                JointSequence.Add(new ArrayList());
                for (int j = 0; j < FRAMES_COUNT; j++)
                    JointSequence[i].Add(skeletons[j].jointPoints[i]);
            }
        }
    }
}
