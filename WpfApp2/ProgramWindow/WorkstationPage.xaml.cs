using Retros.ProgramWindow.DisplaySystem;
using Retros.ProgramWindow.Interactive;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Utillities.Wpf;

namespace Retros.ProgramWindow {
    public partial class WorkstationPage : Page {

        private double _shadowRectWidth = 5;

        public WorkstationPage(WorkstationImage imageElement, WorkstationTable tableElement, double topPadding = 0) {
            InitializeComponent();

            UIManager.ColorThemeManager.Set_BG1(b => WorkstationImageGridParent.Background = b);
            UIManager.ColorThemeManager.Set_BG2(b => WorkstationTableGridParent.Background = b);

            WorkstationImageGrid.Children.Add(imageElement.FrameworkElement);
            WorkstationTableGrid.Children.Add(tableElement.FrameworkElement);

            Shadow.Width = _shadowRectWidth;
            Shadow.Effect = new DropShadowEffect { BlurRadius = 25, ShadowDepth = 10, Color = Colors.Black, Opacity = 0.80, Direction = 180 };
            UIManager.ColorThemeManager.Set_BG2(b => Shadow.Fill = b);

            WorkstationImageGrid.Margin = new Thickness(0, topPadding, 0, 0);
            WorkstationTableGrid.Margin = new Thickness(0, topPadding, 0, 0);
        }



        // Shadow Behaviour
        private void Shadow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            (sender as Rectangle)!.CaptureMouse();
            CompositionTarget.Rendering += CompositionTarget_Rendering;

            double shadowPos = Helper.GetAbsolutePosition(Shadow, Application.Current.MainWindow).X;
            double mousePos = Mouse.GetPosition(Application.Current.MainWindow).X; // Mouse Position Relative to the window
            double mousePosDelta = mousePos - shadowPos; // Mouse Position Relative to the AHsoq 
        }
        private void Shadow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            (sender as Rectangle)!.ReleaseMouseCapture();
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
        }
        private void CompositionTarget_Rendering(object? sender, EventArgs e) {
            double mousePos = Mouse.GetPosition(Application.Current.MainWindow).X; // Mouse Position Relative to the window

            double a = mousePos / MainGrid.ActualWidth;
            double b = 1 - a;

            if (a < 0) a = _shadowRectWidth / MainGrid.ActualWidth;
            if (b < 0) b = 0;
            MainGrid.ColumnDefinitions[0].Width = new GridLength(a, GridUnitType.Star);
            MainGrid.ColumnDefinitions[1].Width = new GridLength(b, GridUnitType.Star);
        }

        private void Shadow_MouseEnter(object sender, MouseEventArgs e) {
            Application.Current.MainWindow.Cursor = Cursors.SizeWE;
        }
        private void Shadow_MouseLeave(object sender, MouseEventArgs e) {
            Application.Current.MainWindow.Cursor = Cursors.Arrow;
        }
    }
}
