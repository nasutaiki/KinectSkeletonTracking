using System;
using System.Linq;
using System.Windows;
using Microsoft.Kinect;

namespace KinectSkeletonTracking
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor kinect;

        BodyFrameReader bodyFrameReader; //
        Body[] bodies; // Bodyを保持する配列；Kinectは最大6人トラッキングできる

        public MainWindow()
        {
            InitializeComponent();
        }

        // Windowが表示されたときコールされる（リスナー？）
        private void Window_Loaded(object sensor, RoutedEventArgs e)
        {
            try
            {
                kinect = KinectSensor.GetDefault();
                // TODO: Kinectが使用可能状態か調べてから次の処理に移りたい。
                kinect.Open();

                // Bodyを入れる配列を作る
                bodies = new Body[kinect.BodyFrameSource.BodyCount];

                // ボディーリーダーを開く（ってなに？ TODO:要把握 ）
                bodyFrameReader = kinect.BodyFrameSource.OpenReader();
                bodyFrameReader.FrameArrived += bodyFrameReader_FrameArrived;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
        }


        // Windowが閉じたときにコールされる（リスナー？）
        private void Window_Closing(object sensor, System.ComponentModel.CancelEventArgs e)
        {
            if (bodyFrameReader != null)
            {
                bodyFrameReader.Dispose();
                bodyFrameReader = null;
            }

            if (kinect != null)
            {
                kinect.Close();
                kinect = null;
            }
        }

        void bodyFrameReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            UpdateBodyFrame(e);
            // TODO:GUIに対する描写は後に実装する
            // DrawBodyFrame(); 
        }

        // ボディの更新
        private void UpdateBodyFrame(BodyFrameArrivedEventArgs e)
        {
            using (var bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame == null)
                {
                    return;
                }

                // ボディデータを取得する
                bodyFrame.GetAndRefreshBodyData(bodies);
            }
        }

        private void DrawBodyFrame()
        {
            // 追跡しているBodyのみループする
            foreach (var body in bodies.Where(b => b.IsTracked))
            {
                // Bodyから取得した全関節でループする。
                foreach (var joint in body.Joints)
                {
                    // 追跡可能な状態か？
                    if (joint.Value.TrackingState == TrackingState.Tracked)
                    {
                        //関節の向きを取得する（Vector4型）。関節の指定にはJoyntType(enum)を使用する。
                        var orientation = body.JointOrientations[joint.Key].Orientation;

                        //関節のそれぞれの軸に対応する角度を取得する
                        var pitchRotate = CalcRotate.Pitch(orientation);
                        var yowRotate = CalcRotate.Yaw(orientation);
                        var rollRotate = CalcRotate.Roll(orientation);
                        
                        // TODO:↑の角度の値から必要なものをソケット通信で送信する
                    }
                }
            }
        }
    }
}