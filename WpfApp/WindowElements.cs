using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using WpfCustomControls;

namespace Retros {

    internal class WindowElements {
        //Window
        public Grid MainGrid = new();

        // Client Area
        public Grid ClientGrid = new();
        public Grid WorkActionsGrid = new();
        public Grid WorkStationGrid = new();
        public System.Windows.Shapes.Rectangle shadow = new System.Windows.Shapes.Rectangle();
        private double shadowRectWidth = 50;

        public Brush WorkStationGrid_BG = Helper.StringToSolidColorBrush("#585858");
        public Brush WorkActionGrid_BG = Helper.StringToSolidColorBrush("#48484c");

        //WindowHandle
        public WindowHandle windowHandle = new();


        public WindowElements(Canvas canvas) {
            // Main Grid
            MainGrid.Loaded += (object sender, RoutedEventArgs e) => { 
                MainGrid.Height = canvas.ActualHeight;
                MainGrid.Width = canvas.ActualWidth;
            };
            canvas.Children.Add(MainGrid);

            // Client Grid
            AddColumn(ClientGrid, 21, GridUnitType.Star);
            AddColumn(ClientGrid, 13, GridUnitType.Star);
            Helper.SetChildInGrid(MainGrid, ClientGrid, 1, 0);
            ClientGrid.ClipToBounds = false;

            // WorkActionsGrid
            AddRow(WorkActionsGrid, 21, GridUnitType.Star);
            AddRow(WorkActionsGrid, 13, GridUnitType.Star);
            Helper.SetChildInGrid(ClientGrid, WorkActionsGrid, 0, 1);
            WorkActionsGrid.Background = WorkActionGrid_BG;
            WorkActionsGrid.ClipToBounds = false;
            WorkActionsGrid.Background = Brushes.Tomato;

            // shadow Rect
            DropShadowEffect effect = new DropShadowEffect { BlurRadius = 50, ShadowDepth = 15, Color = Colors.Black, Opacity = 0.5, Direction = 180};
            shadow.Effect = effect;
            shadow.Width = shadowRectWidth; 
            shadow.Fill = Brushes.Gainsboro;
            Canvas.SetZIndex(shadow, -10);
            shadow.VerticalAlignment = VerticalAlignment.Top;
            shadow.HorizontalAlignment = HorizontalAlignment.Right;
            Helper.SetChildInGrid(WorkStationGrid, shadow, 0, 0);

            // WorkStationGrid
            Helper.SetChildInGrid(ClientGrid, WorkStationGrid, 0, 0);
            WorkStationGrid.Background = WorkStationGrid_BG;

            //WindowHandle
            windowHandle.SetParentWindow(canvas);
            windowHandle.SetBGColor(Helper.StringToSolidColorBrush(windowHandle.BGColor.ToString(), 0.75));
            windowHandle.ApplicationButtons.ColorWhenButtonHover = Helper.StringToSolidColorBrush("#000000", 0.2);

            Application.Current.MainWindow.Loaded += (object sender, RoutedEventArgs e) => UpdateGridSizes_T2();
            canvas.LayoutUpdated += (object? sender, EventArgs e) => UpdateGridSizes_T2();
        }

        public void UpdateGridSizes_Old() {
            try {

            MainGrid.Height = Application.Current.MainWindow.ActualHeight;
            MainGrid.Width = Application.Current.MainWindow.ActualWidth;

            double a = MainGrid.ActualHeight;
            double b = MainGrid.ActualWidth - a;
            ClientGrid.ColumnDefinitions[0].Width = new GridLength(a, GridUnitType.Pixel);
            ClientGrid.ColumnDefinitions[1].Width = new GridLength(b, GridUnitType.Pixel);
            }
            catch {
                UpdateGridSizes_T2();
            }
        }
        public void UpdateGridSizes_T2() {

            MainGrid.Height = Application.Current.MainWindow.ActualHeight;
            MainGrid.Width = Application.Current.MainWindow.ActualWidth;

            ClientGrid.ColumnDefinitions[0].Width = new GridLength(21, GridUnitType.Star);
            ClientGrid.ColumnDefinitions[1].Width = new GridLength(13, GridUnitType.Star);


            shadow.Height = WorkStationGrid.ActualHeight;
        }


        public static void AddRow(Grid grid, double value, GridUnitType type) {
            RowDefinition rowDefinition = new RowDefinition { Height = new GridLength(value, type) };
            grid.RowDefinitions.Add(rowDefinition);
        }

        public static void AddColumn(Grid grid, double value, GridUnitType type) {
            ColumnDefinition columnDefinition = new ColumnDefinition { Width = new GridLength(value, type) };
            grid.ColumnDefinitions.Add(columnDefinition);
        }


    }

}
