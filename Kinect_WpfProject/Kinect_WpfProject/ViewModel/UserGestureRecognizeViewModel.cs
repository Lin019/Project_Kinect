using Kinect_WpfProject.Extends;
using Kinect_WpfProject.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kinect_WpfProject.ViewModel
{
    class UserGestureRecognizeViewModel:INotifyPropertyChanged
    {
        private ObservableCollection<double> _x1, _y1, _x2, _y2;

        public ObservableCollection<double> x1 
        {
            get { return _x1; }
            set
            {
                _x1 = value;
                NotifyPropertyChanged("x1");
            }
        }
        public ObservableCollection<double> y1
        {
            get { return _y1; }
            set
            {
                _y1 = value;
                NotifyPropertyChanged("y1");
            }
        }
        public ObservableCollection<double> x2
        {
            get { return _x2; }
            set
            {
                _x2 = value;
                NotifyPropertyChanged("x2");
            }
        }
        public ObservableCollection<double> y2
        {
            get { return _y2; }
            set
            {
                _y2 = value;
                NotifyPropertyChanged("y2");
            }
        }

        private string gestureName = "none";
        public string GestureName
        {
            get { return gestureName; }
            set 
            {
                gestureName = value;
                NotifyPropertyChanged("gestureName");
            }
        }

        private ArrayList bodySequence;

        public UserGestureRecognizeViewModel()
        {
            x1 = new ObservableCollection<double>();
            y1 = new ObservableCollection<double>();
            x2 = new ObservableCollection<double>();
            y2 = new ObservableCollection<double>();

            _x1 = new ObservableCollection<double>();
            _y1 = new ObservableCollection<double>();
            _x2 = new ObservableCollection<double>();
            _y2 = new ObservableCollection<double>();

            for (int i = 0; i < Common.BONE_COUNT; i++ )
            {
                x1.Add(0);
                y1.Add(0);
                x2.Add(0);
                y2.Add(0);

                _x1.Add(0);
                _y1.Add(0);
                _x2.Add(0);
                _y2.Add(0);
            }
            bodySequence = new ArrayList();
        }

        private void SetSkeletonLines(Skeleton skeleton)
        {
            x1 = new ObservableCollection<double>(skeleton.getDrawingSequences()["x1"]);
            y1 = new ObservableCollection<double>(skeleton.getDrawingSequences()["y1"]);
            x2 = new ObservableCollection<double>(skeleton.getDrawingSequences()["x2"]);
            y2 = new ObservableCollection<double>(skeleton.getDrawingSequences()["y2"]);
        }

        #region command_recognize

        private ICommand _Recognize;
        public ICommand Recognize
        {
            get
            {
                if (_Recognize == null)
                {
                    _Recognize = new RelayCommand(
                        this.RecognizeExecute,
                        this.CanRecognizeExecute
                    );
                }
                return _Recognize;
            }
        }
        private bool CanRecognizeExecute()
        {
            return true;
        }
        private void RecognizeExecute()
        {
            KinectModel _model = new KinectModel();
            GestureName = _model.Recognize(bodySequence);
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
