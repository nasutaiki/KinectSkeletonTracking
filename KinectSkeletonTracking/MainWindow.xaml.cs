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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;

namespace KinectSkeletonTracking
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor kinect;
        
        public MainWindow()
        {
            InitializeComponent();
        }
        

       
        private void Window_Loaded(object sensor, RoutedEventArgs e)
        {
            try
            {
                kinect = KinectSensor.GetDefault();
                if (kinect == null)
                {
                    throw new Exception("Kinectを開けません");
                }

                kinect.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
        }

        private void Window_Closing(object sensor, System.ComponentModel.CancelEventArgs e)
        {
            if (kinect != null)
            {
                kinect.Close();
                kinect = null;
            }
        }
    }
}
