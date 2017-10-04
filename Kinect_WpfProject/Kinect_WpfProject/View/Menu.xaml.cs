using Kinect_WpfProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kinect_WpfProject.View
{
    /// <summary>
    /// Menu.xaml 的互動邏輯
    /// </summary>
    public partial class Menu : Window
    {
        public static KinectControl kinectControl;
        public Menu()
        {
            InitializeComponent();
            triggerTimer = new TimerTool(TimerTick, 0, 3000);
            kinectControl = new KinectControl();
        }

        private Storyboard my_sb;
        int Order = 1;
        private TimerTool triggerTimer;

        private void TimerTick()
        {
            triggerTimer.StopTimer();
            button1_Click();
        }

        private void button1_ClickTrigger(object sender , RoutedEventArgs e)
        {
            if (Order == 1)//主畫面
            {
                my_sb = (Storyboard)FindResource("LeftStoryboard_F");
                my_sb.Begin(this);//畫面切至左畫面
                Order = 0;//畫面旗標設為左畫面
            }
            else if (Order == 0)//左畫面
            {
                my_sb = (Storyboard)FindResource("LeftStoryboard_B");
                my_sb.Begin(this);//畫面切至主畫面
                Order = 1;//畫面旗標設為主畫面
            }
            //triggerTimer.StartTimer();
        }
        private void button1_Leave(object sender, RoutedEventArgs e)
        {
            triggerTimer.StopTimer();
        }

        private void button1_Click()
        {
            if (Order == 1)//主畫面
            {
                my_sb = (Storyboard)FindResource("LeftStoryboard_F");
                my_sb.Begin(this);//畫面切至左畫面
                Order = 0;//畫面旗標設為左畫面
            }
            else if (Order == 0)//左畫面
            {
                my_sb = (Storyboard)FindResource("LeftStoryboard_B");
                my_sb.Begin(this);//畫面切至主畫面
                Order = 1;//畫面旗標設為主畫面
            }
        }

        private void buttonSampleRecord_Click(object sender, RoutedEventArgs e)
        {
            SampleRecord sampleRecordForm = new SampleRecord();
            App.Current.MainWindow = sampleRecordForm;
            
            this.Close();
            sampleRecordForm.Show();
        }

        private void buttonUserGestureRecognize_Click(object sender, RoutedEventArgs e)
        {
            GestureSelect gestureSelectForm = new GestureSelect();
            App.Current.MainWindow = gestureSelectForm;
            this.Close();
            gestureSelectForm.Show();
        }
    }
}
