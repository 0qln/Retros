using Retros.Settings;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
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
using System.Windows.Threading;
using Utillities.Wpf;

namespace Retros.ProgramWindow.DisplaySystem {
    public partial class WorkstationImagePage : Page {

        public DropShadowEffect ImageEffect;
        private WorkstationImageWindow? _imageWindow;
        private WorkstationImage _workstationImage;

        public delegate void OnWindowInitHandle(WorkstationImageWindow window);
        public event OnWindowInitHandle? WindowInitiated;


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



        public Thickness ImageMargin {
            get {
                return _margin;
            }
            set {
                _margin = value;
                Image.Margin = _margin;
            }
        }
        private Thickness _margin = new Thickness(50);
        private int _imageCount = 1; /// used to set the newest images to the front

        public void ChangeCurrent(ImageSource imageSource) {
            // Prepare
            _imageCount++;
            Image newImage = CreateFadeImage(Page, imageSource);

            // Interpolate
            if (_workstationImage.IsCalculated) { // isCalculated
                int i = (int)_workstationImage.StartBoost;
                var timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(_workstationImage.Interval);
                timer.Tick += (s, e) => {
                    newImage.Opacity = _workstationImage.Dp_tValues[i];

                    i++;
                    if (i >= _workstationImage.Dp_tValues.Count) {
                        newImage.Effect = Page.Image.Effect;
                        Page.Image.Effect = null;
                        Page.MainGrid.Children.Remove(Page.Image);
                        Page.Image = newImage;
                        Page.Image.Margin = _margin;
                        _imageCount--;
                        timer.Stop();
                    }
                };
                timer.Start();
            }
            else {
                _workstationImage.Dp_tValues.Clear();
                float t = _workstationImage.StartBoost;
                var timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(_workstationImage.Interval);
                timer.Tick += (s, e) => {
                    _workstationImage.IsCalculated = true;
                    float val = _workstationImage.InterpolationFuntion(t);
                    newImage.Opacity = val;
                    _workstationImage.Dp_tValues.Add(val);

                    t += _workstationImage.Interval;
                    if (t >= _workstationImage.TotalInterpolationTime) {
                        newImage.Effect = Page.Image.Effect;
                        Page.Image.Effect = null;
                        Page.MainGrid.Children.Remove(Page.Image);
                        Page.Image = newImage;
                        _imageCount--;
                        _workstationImage.IsCalculated = true;
                        timer.Stop();
                    }
                };
                timer.Start();
            }
        }
        private Image CreateFadeImage(WorkstationImagePage page, ImageSource imageSource) {
            Image newImage = new Image { Source = imageSource };
            newImage.Opacity = 0;
            Helper.SetChildInGrid(page.MainGrid, newImage, Grid.GetRow(page.Image), Grid.GetColumn(page.Image));
            newImage.Margin = _margin;
            Canvas.SetZIndex(newImage, 10);
            Canvas.SetZIndex(page.Image, 10 / _imageCount);
            return newImage;
        }


        private void ImageHandleActivation_MouseEnter(object sender, MouseEventArgs e) => ImageHandle.Visibility = Visibility.Visible;
        private void ImageHandleActivation_MouseLeave(object sender, MouseEventArgs e) => ImageHandle.Visibility = Visibility.Collapsed;


        private void ImageHandle_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
            ImageHandleActivation.Children.RemoveAt(0);
            ImageHandleActivation.Height = 0;

            Image.Margin = new Thickness(0);
            ImageMargin = new Thickness(0);
            _imageWindow = new WorkstationImageWindow(Image.ActualWidth, Image.ActualHeight);
            _imageWindow.Show();
            _imageWindow.Top = Mouse.GetPosition(WindowManager.MainWindow).Y + WindowManager.MainWindow.Top - 10;
            _imageWindow.Left = Mouse.GetPosition(WindowManager.MainWindow).X + WindowManager.MainWindow.Left - Image.ActualWidth / 2;

            CaptureWindow(_imageWindow);

            ///WindowManager.MainWindow.Activated += ActivateOnce;

            WindowInitiated?.Invoke(_imageWindow);

            WindowManager.MainWindow.SelectedWorkstation.HideImageUI();
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
