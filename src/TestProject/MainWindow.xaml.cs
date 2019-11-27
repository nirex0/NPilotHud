using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace TestProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BackgroundWorker bgw = new BackgroundWorker();

        public float RollState;
        public float PitchState;
        public float YawState;
        public float BetaState;
        public float VertGainState;
        public float AlphaState;
        public float AltitudeState;
        public float RollCommandState;
        public MainWindow()
        {
            InitializeComponent();

            RollState = 0;
            PitchState = 0;
            YawState = 0;
            BetaState = 0;

            bgw.DoWork += Bgw_DoWork;
            bgw.RunWorkerCompleted += Bgw_RunWorkerCompleted;
            bgw.RunWorkerAsync();

            PreviewKeyDown += MainWindow_PreviewKeyDown;
            PreviewKeyUp += MainWindow_PreviewKeyUp;

            MouseDown += MainWindow_MouseDown1;
        }

        private void MainWindow_MouseDown1(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                try { DragMove(); } catch { }
            }
        }

        private void MainWindow_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }

            if (e.Key == Key.D)
            {
                RollState = 0;
            }
            if (e.Key == Key.A)
            {
                RollState = 0;
            }

            if (e.Key == Key.W)
            {
                PitchState = 0;
            }
            if (e.Key == Key.S)
            {
                PitchState = 0;
            }

            if (e.Key == Key.E)
            {
                YawState = 0;
            }
            if (e.Key == Key.Q)
            {
                YawState = 0;
            }

            if (e.Key == Key.Space)
            {
                VertGainState = 0;
                AltitudeState = 0;
            }
            if (e.Key == Key.LeftCtrl)
            {
                VertGainState = 0;
                AltitudeState = 0;
            }

            if (e.Key == Key.C)
            {
                AlphaState = 0;
            }
            if (e.Key == Key.Z)
            {
                AlphaState = 0;
            }
        }

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.R)
            {
                RollState = 0;
                PitchState = 0;
                YawState = 0;
                BetaState = 0;
                VertGainState = 0;
                AlphaState = 0;
                AltitudeState = 0;
                RollCommandState = 0;

                Hud_1.RollAngle = 0;
                Hud_1.PitchAngle = 0;
                Hud_1.YawAngle = 0;
                Hud_1.Beta = 0;
                Hud_1.VerticalSpeed = 0;
                Hud_1.Alpha = 0;
                Hud_1.Altitude = 0;
                Hud_1.RollCommand = 0;
                Hud_1.GroundSpeed = 0;
            }

            if (e.Key == Key.D)
            {
                RollState = 0.25f;
            }
            if (e.Key == Key.A)
            {
                RollState = -0.25f;
            }

            if (e.Key == Key.W)
            {
                PitchState = -0.25f;
            }
            if (e.Key == Key.S)
            {
                PitchState = 0.25f;
            }

            if (e.Key == Key.E)
            {
                YawState = 0.05f;
            }
            if (e.Key == Key.Q)
            {
                YawState = -0.05f;
            }

            if (e.Key == Key.Space)
            {
                VertGainState = 0.2f;
            }
            if (e.Key == Key.LeftCtrl)
            {
                VertGainState = -0.2f;
            }

            if (e.Key == Key.C)
            {
                AlphaState = 0.25f;
            }
            if (e.Key == Key.Z)
            {
                AlphaState = -0.25f;
            }
        }

        private void Bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Hud_1.RollAngle += RollState;
            Hud_1.PitchAngle += PitchState;
            Hud_1.YawAngle += YawState;
            Hud_1.GroundSpeed += VertGainState;
            Hud_1.Beta += BetaState;
            Hud_1.VerticalSpeed += VertGainState;
            Hud_1.Altitude += VertGainState;
            Hud_1.Alpha += AlphaState;
            Hud_1.Altitude += AltitudeState;
            Hud_1.RollCommand += RollCommandState;
            bgw.RunWorkerAsync();
        }

        private void Bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(1);
        }
    }
}
