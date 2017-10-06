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
    /// UserGestureRecognize.xaml 的互動邏輯
    /// </summary>
    public partial class UserGestureRecognize : Window
    {
        
        public UserGestureRecognize(string gestureName)
        {
            InitializeComponent();
            var vm = (UserGestureRecognizeViewModel)this.DataContext;
            vm.gestureName = gestureName;
            film.Source = new Uri(@"C:\Film\" + gestureName+".MOV");
        }
        private void FilmEnded(object sender, RoutedEventArgs e)
        {
            film.Position = new TimeSpan(0, 0, 1);
        }

        void UserGestureRecognizeForm_Closing(object sender, CancelEventArgs e)
        {
            Menu menu = new Menu();
            App.Current.MainWindow = menu;
            menu.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackButton(object sender, RoutedEventArgs e)
        {
            Menu menuForm = new Menu();
            App.Current.MainWindow = menuForm;
            this.Close();
            menuForm.Show();
        }
    }
}
