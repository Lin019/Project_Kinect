using Kinect_WpfProject.Model;
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
    /// GestureSelect.xaml 的互動邏輯
    /// </summary>
    public partial class GestureSelect : Window
    {

        public GestureSelect()
        {
            InitializeComponent();
        }
        


        public void Button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            
            UserGestureRecognize sampleRecognizeForm = new UserGestureRecognize(btn.Name);
            App.Current.MainWindow = sampleRecognizeForm;
            this.Close();
            sampleRecognizeForm.Show();
        }

        public void ScrollDownStart(object sender, EventArgs e)
        {

            for(int i = 5; i > 0;i--)
            {
                ScrollViewer.LineDown();
            }
        }
        public void ScrollUpStart(object sender, EventArgs e)
        {

            for (int i = 5; i > 0; i--)
            {
                ScrollViewer.LineUp();
            }
        }


        void GestureSelectForm_Closing(object sender, CancelEventArgs e)
        {
            Menu menu = new Menu();
            App.Current.MainWindow = menu;
            menu.Show();
        }
    }
}
