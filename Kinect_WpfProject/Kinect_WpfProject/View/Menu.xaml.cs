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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kinect_WpfProject.View
{
    /// <summary>
    /// Menu.xaml 的互動邏輯
    /// </summary>
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void btn_UserGestureRecognize_Click(object sender, RoutedEventArgs e)
        {
            UserGestureRecognize userGestureRecognizeForm = new UserGestureRecognize();
            App.Current.MainWindow = userGestureRecognizeForm;
            this.Close();
            userGestureRecognizeForm.Show();
        }

        private void btn_SampleRecord_Click(object sender, RoutedEventArgs e)
        {
            SampleRecord sampleRecordForm = new SampleRecord();
            App.Current.MainWindow = sampleRecordForm;
            this.Close();
            sampleRecordForm.Show();
        }
    }
}
