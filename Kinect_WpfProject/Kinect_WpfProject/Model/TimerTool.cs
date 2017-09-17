using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kinect_WpfProject.Model
{
    public class TimerTool
    {
        private Timer timer;
        private Action func;
        private int dueTime;
        private int period;

        public TimerTool(Action tick, int dueTime, int period)
        {
            this.func = tick;
            this.dueTime = dueTime;
            this.period = period;
        }

        public void StartTimer()
        {
            StopTimer();
            timer = new Timer(x => TimerTick(), null, dueTime, period);
        }

        public void StopTimer()
        {
            if (timer != null)
            {
                timer.Dispose();
                timer = null;
            }
        }

        public bool IsActive()
        {
            return (timer == null) ? false : true;
        }

        private void TimerTick()
        {
            func();
        }
    }
}
