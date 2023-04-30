using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Threading;
using WpfCustomControls;

namespace Retros {

    internal class WindowElements {
        //Window
        public Grid MainGrid = new();
        private Canvas canvas;

        // Client Area
        public ClientWorkStation workStation;
        public Grid ClientGrid = new();
        public Grid WorkStationGrid = new();
        public Grid WorkStationImageGrid = new();
        public System.Windows.Shapes.Rectangle shadow = new System.Windows.Shapes.Rectangle();
        private double shadowRectWidth = 5;

        public Brush WorkStationGrid_BG = Helper.StringToSolidColorBrush("#1f1f1f");
        public Brush WorkActionGrid_BG = Helper.StringToSolidColorBrush("#2e2e2e");

        public Image CurrentImage = new();

        //WindowHandle
        public WindowHandle windowHandle = new();
        public DropDownMenu fileMenu = new("File");
        public DropDownMenu editMenu = new("Edit");
        public DropDownMenu viewMenu = new("View");
        public DropDownMenu settingsMenu = new("Settings");


        public WindowElements(Canvas canvas) {
            this.canvas = canvas;

            // Main Grid
            MainGrid.Loaded += (object sender, RoutedEventArgs e) => { 
                MainGrid.Height = canvas.ActualHeight;
                MainGrid.Width = canvas.ActualWidth;
            };
            canvas.Children.Add(MainGrid);


            // Client Grid
            Helper.AddColumn(ClientGrid, 21, GridUnitType.Star);
            Helper.AddColumn(ClientGrid, 13, GridUnitType.Star);
            Helper.SetChildInGrid(MainGrid, ClientGrid, 1, 0);


            // WorkActionsGrid
            Helper.AddRow(WorkStationGrid, windowHandle.Height, GridUnitType.Pixel);
            Helper.AddRow(WorkStationGrid, 1, GridUnitType.Star);
            Helper.SetChildInGrid(ClientGrid, WorkStationGrid, 0, 1);
            WorkStationGrid.Background = WorkActionGrid_BG;

            workStation = new();
            Helper.SetChildInGrid(WorkStationGrid, workStation.FrameworkElement, 1, 0);
            workStation.AddTab(new ClientWorkStation.Tab("Filter", new TabBodies.ImageEditing.Filters()));
            workStation.AddTab(new ClientWorkStation.Tab("Pixel Sorter", new TabBodies.ImageEditing.PixelSorting()));
            workStation.SelectTab(0);

            // WorkStation
            Helper.SetChildInGrid(ClientGrid, WorkStationImageGrid, 0, 0);
            WorkStationImageGrid.Background = WorkStationGrid_BG;

            Helper.AddColumn(WorkStationImageGrid, 1, GridUnitType.Star);
            Helper.AddColumn(WorkStationImageGrid, 1, GridUnitType.Auto);
            Helper.SetImageSource(CurrentImage, "C:\\Users\\User\\OneDrive\\Bilder\\Wallpapers\\mountain-lake-reflection-nature-scenery-hd-wallpaper-uhdpaper.com-385@0@h.jpg");
            CurrentImage.HorizontalAlignment = HorizontalAlignment.Stretch;
            CurrentImage.Margin = new Thickness(50);
            Helper.SetChildInGrid(WorkStationImageGrid, CurrentImage, 0, 0);
            Helper.SetChildInGrid(WorkStationImageGrid, shadow, 0, 1);

            DropShadowEffect effect1 = new DropShadowEffect { BlurRadius = 30, ShadowDepth = 15, Color = Colors.Black, Opacity = 0.8, Direction = 270 };
            CurrentImage.Effect = effect1;



            // shadow
            DropShadowEffect effect2 = new DropShadowEffect { BlurRadius = 25, ShadowDepth = 15, Color = Colors.Black, Opacity = 0.80, Direction = 180};
            shadow.Effect = effect2;
            shadow.Width = shadowRectWidth;
            shadow.Fill = WorkActionGrid_BG;
            Canvas.SetZIndex(shadow, -10);
            shadow.VerticalAlignment = VerticalAlignment.Top;
            shadow.HorizontalAlignment = HorizontalAlignment.Right;
            shadow.MouseLeftButtonDown += Shadow_MouseDown;
            shadow.MouseLeftButtonUp += Shadow_MouseUp;
            shadow.MouseEnter += (s, e) => Application.Current.MainWindow.Cursor = Cursors.SizeWE; ;
            shadow.MouseLeave += (s, e) => Application.Current.MainWindow.Cursor = Cursors.Arrow; ;
            



            SetupWindowHandle();
        }

        public void SetupWindowHandle() {
            // Viusals
            windowHandle.SetParentWindow(canvas);
            windowHandle.SetBGColor(Helper.StringToSolidColorBrush("#000000", 0.45));
            windowHandle.ApplicationButtons.ColorWhenButtonHover = Helper.StringToSolidColorBrush("#000000", 0.2);

            Application.Current.MainWindow.Loaded += (object sender, RoutedEventArgs e) => UpdateGridSizes_T2();
            canvas.LayoutUpdated += (object? sender, EventArgs e) => UpdateGridSizes_T2();

            // Client Buttons
            windowHandle.CreateClientButton(fileMenu);
            windowHandle.CreateClientButton(viewMenu);
            windowHandle.CreateClientButton(editMenu);
            windowHandle.CreateClientButton(settingsMenu);

            fileMenu.AddOption("Save").SetKeyboardShortcut("Strg + S");
            fileMenu.AddOption("Open").SetKeyboardShortcut("Strg + O");
            editMenu.AddOption("Save Filter").SetKeyboardShortcut("Strg + S + F");
            viewMenu.AddOption("Zoom").SetKeyboardShortcut("Bla bla");
            settingsMenu.AddOption("Change Layout").SetKeyboardShortcut("Strg + LShift + L");

            windowHandle.ActivateAllClientButtons();
        }

        // Client area Handle 
        private double offsetOnEnter = 0;
        private void Shadow_MouseUp(object sender, MouseButtonEventArgs e) {
            ((System.Windows.Shapes.Rectangle)sender).ReleaseMouseCapture();
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
            offsetOnEnter = 0;
        }
        private void Shadow_MouseDown(object sender, MouseButtonEventArgs e) {
            ((System.Windows.Shapes.Rectangle)sender).CaptureMouse();
            CompositionTarget.Rendering += CompositionTarget_Rendering;

            double shadowPos = Helper.GetAbsolutePosition(shadow).X;
            double mousePos = Mouse.GetPosition(Application.Current.MainWindow).X; // Mouse Position Relative to the window
            double mousePosDelta = mousePos - (shadowPos); // Mouse Position Relative to the AHsoq 
            offsetOnEnter = mousePosDelta;
        }
        private void CompositionTarget_Rendering(object? sender, EventArgs e) {
            double mousePos = Mouse.GetPosition(Application.Current.MainWindow).X; // Mouse Position Relative to the window

            double a = mousePos / (ClientGrid.ActualWidth);
            double b = 1 - a;

            if (a < 0) a = shadowRectWidth / ClientGrid.ActualWidth;
            if (b < 0) b = 0;
            ClientGrid.ColumnDefinitions[0].Width = new GridLength(a, GridUnitType.Star);
            ClientGrid.ColumnDefinitions[1].Width = new GridLength(b, GridUnitType.Star);
        }


        public void GoldenRatio_GridSizes() {
            MainGrid.Height = Application.Current.MainWindow.ActualHeight;
            MainGrid.Width = Application.Current.MainWindow.ActualWidth;

            double a = MainGrid.ActualHeight;
            double b = MainGrid.ActualWidth - a;
            ClientGrid.ColumnDefinitions[0].Width = new GridLength(a, GridUnitType.Pixel);
            ClientGrid.ColumnDefinitions[1].Width = new GridLength(b, GridUnitType.Pixel);
        }
        public void UpdateGridSizes_T2() {
            MainGrid.Height = Application.Current.MainWindow.ActualHeight;
            MainGrid.Width = Application.Current.MainWindow.ActualWidth;

            shadow.Height = WorkStationImageGrid.ActualHeight;
        }
    }
}
