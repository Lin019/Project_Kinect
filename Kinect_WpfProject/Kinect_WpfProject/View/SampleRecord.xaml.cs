using Kinect_WpfProject.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kinect_WpfProject.View
{
    /// <summary>
    /// SampleRecord.xaml 的互動邏輯
    /// </summary>
    public partial class SampleRecord : Window
    {
        KinectControl kinectControl;
        public SampleRecord()
        {
            InitializeComponent();
            DataContext = new SampleRecordViewModel();
            kinectControl = new KinectControl();
        }

        void SampleRecordForm_Closing(object sender, CancelEventArgs e)
        {
            Menu menu = new Menu();
            App.Current.MainWindow = menu;
            kinectControl.Close();
            menu.Show();
        }
    }
}
