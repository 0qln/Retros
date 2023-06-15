using Retros.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Utillities.Wpf;

namespace Retros.ProgramWindow.DisplaySystem {
    public partial class WorkstationImagePage : Page {

        public DropShadowEffect ImageEffect;
        private WorkstationImageWindow? _imageWindow;
        private UIElement? _tempImageHandle;
        private WorkstationImage _workstationImage;


        public WorkstationImagePage(WorkstationImage workstationImage) {
            _workstationImage = workstationImage;
            InitializeComponent();

            ImageEffect = new DropShadowEffect { BlurRadius = 30, ShadowDepth = 15, Color = Colors.Black, Opacity = 0.8, Direction = 270 };
            SettingsManager.WorkstationImageShadow.BlurRadius += (value) => ImageEffect.BlurRadius = value;
            SettingsManager.WorkstationImageShadow.ShadowDepth += (value) => ImageEffect.ShadowDepth = value;
            SettingsManager.WorkstationImageShadow.Opacity += (value) => ImageEffect.Opacity = value;
            SettingsManager.WorkstationImageShadow.Direction += (value) => ImageEffect.Direction = value;
            SettingsManager.WorkstationImageShadow.Enabled += (enabled) => {
                if (enabled) Image.Effect = ImageEffect;
                else Image.Effect = null;
            };
        }

        public void SetImage(Image image) {
            Image = image;
        }

        private void ImageHandleActivation_MouseEnter(object sender, MouseEventArgs e) => ImageHandle.Visibility = Visibility.Visible;
        private void ImageHandleActivation_MouseLeave(object sender, MouseEventArgs e) => ImageHandle.Visibility = Visibility.Collapsed;


        private void ImageHandle_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
            WindowManager.MainWindow.SelectedWorkstation.HideImage();

            _tempImageHandle = ImageHandleActivation.Children[0];
            ImageHandleActivation.Children.RemoveAt(0);
            ImageHandleActivation.Height = 0;

            var width = Image.ActualWidth;
            var height = Image.ActualHeight;
            Image.Margin = new Thickness(0);
            _workstationImage.ImageMargin = new Thickness(0);
            _imageWindow = new WorkstationImageWindow(this, width, height);
            _imageWindow.Show();
            _imageWindow.Top = Mouse.GetPosition(WindowManager.MainWindow).Y + WindowManager.MainWindow.Top - 10;
            _imageWindow.Left = Mouse.GetPosition(WindowManager.MainWindow).X + WindowManager.MainWindow.Left - width / 2;

            CaptureWindow(_imageWindow);

            WindowManager.MainWindow.Activated += ActivateOnce;

        }
        private void CaptureWindow(Window window) {
            Mouse.Capture(window);
            window.MouseLeftButtonUp += (s, e) => Mouse.Capture(null);
            ReleaseCapture();
            SendMessage(new System.Windows.Interop.WindowInteropHelper(window).Handle, WM_SYSCOMMAND, (IntPtr)SC_DRAGMOVE, IntPtr.Zero);
        }
        private void ActivateOnce(object? sender, EventArgs e) {
            _imageWindow!.Activate();
            WindowManager.MainWindow!.Activated -= ActivateOnce;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        private static extern IntPtr ReleaseCapture();

        private const int WM_SYSCOMMAND = 0x112;
        private const int SC_DRAGMOVE = 0xF012;

    }
}
