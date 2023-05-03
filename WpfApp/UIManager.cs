using Microsoft.Win32;
using Retros.ClientWorkStation;
using Retros.ClientWorkStation.Tabs;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Threading;
using WpfCustomControls;

namespace Retros {

    public static class UIManager {
        //Window
        public static Grid MainGrid = new();
        private static Canvas? canvas;

        // Client Area
        public static Grid ClientGrid = new();
        public static Grid WorkStationGrid = new();
        public static Grid WorkStationImageGrid = new();
        public static System.Windows.Shapes.Rectangle shadow = new System.Windows.Shapes.Rectangle();
        private static double shadowRectWidth = 5;

        public static Brush WorkStationGrid_BG = Helper.StringToSolidColorBrush("#1f1f1f");
        public static Brush WorkActionGrid_BG = Helper.StringToSolidColorBrush("#2e2e2e");


        //WindowHandle
        public static WindowHandle windowHandle = new();
        public static DropDownMenu fileMenu = new("File");
        public static DropDownMenu editMenu = new("Edit");
        public static DropDownMenu viewMenu = new("View");
        public static DropDownMenu settingsMenu = new("Settings");


        public static void Instanciate(Canvas pCanvas) {
            canvas = pCanvas;

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

            ClientWorkStation.WorkStation.Instanciate();
            Helper.SetChildInGrid(WorkStationGrid, ClientWorkStation.WorkStation.FrameworkElement, 1, 0);
            ClientWorkStation.WorkStation.WorkTable.AddTab(new ImageFilterTab(new ClientWorkStation.Tabs.Bodies.ImageFilter(), new ClientWorkStation.Tabs.Handles.DefaultHandle("Filters")));
            ClientWorkStation.WorkStation.WorkTable.SelectTab(0);

            // WorkStation
            Helper.SetChildInGrid(ClientGrid, WorkStationImageGrid, 0, 0);
            WorkStationImageGrid.Background = WorkStationGrid_BG;

            Helper.AddColumn(WorkStationImageGrid, 1, GridUnitType.Star);
            Helper.AddColumn(WorkStationImageGrid, 1, GridUnitType.Auto);
            ClientWorkStation.WorkStation.WorkstationImage.Image.HorizontalAlignment = HorizontalAlignment.Stretch;
            ClientWorkStation.WorkStation.WorkstationImage.Image.Margin = new Thickness(50);
            Helper.SetChildInGrid(WorkStationImageGrid, ClientWorkStation.WorkStation.WorkstationImage.Image, 0, 0);
            Helper.SetChildInGrid(WorkStationImageGrid, shadow, 0, 1);

            DropShadowEffect effect1 = new DropShadowEffect { BlurRadius = 30, ShadowDepth = 15, Color = Colors.Black, Opacity = 0.8, Direction = 270 };
            ClientWorkStation.WorkStation.WorkstationImage.Image.Effect = effect1;



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




            // Set up WindowHandle
                // Viusals
            windowHandle.SetParentWindow(canvas!);
            windowHandle.SetBGColor(Helper.StringToSolidColorBrush("#000000", 0.45));
            windowHandle.ApplicationButtons.ColorWhenButtonHover = Helper.StringToSolidColorBrush("#000000", 0.2);

            Application.Current.MainWindow.Loaded += (object sender, RoutedEventArgs e) => UpdateGridSizes_T2();
            canvas!.LayoutUpdated += (object? sender, EventArgs e) => UpdateGridSizes_T2();

                // Client Buttons
            windowHandle.CreateClientButton(fileMenu);
            windowHandle.CreateClientButton(viewMenu);
            windowHandle.CreateClientButton(editMenu);
            windowHandle.CreateClientButton(settingsMenu);

            fileMenu.AddOption("Save").SetKeyboardShortcut("Strg + S");
            fileMenu.AddOption("Open").SetKeyboardShortcut("Strg + O").AddCommand(LoadImage);
            editMenu.AddOption("Save Filter").SetKeyboardShortcut("Strg + S + F");
            viewMenu.AddOption("Zoom").SetKeyboardShortcut("Bla bla");
            settingsMenu.AddOption("Change Layout").SetKeyboardShortcut("Strg + LShift + L");

            windowHandle.ActivateAllClientButtons();
        }

        public static void LoadImage() {
            string path = ShowImagePickerDialog();
            if (path != null) { 
                WorkStation.WorkstationImage = new(ShowImagePickerDialog());
            }
        }
        public static string ShowFolderPickerDialog() {
            var openFileDialog = new OpenFileDialog {
                Title = "Select a folder",
                Filter = "Folders|*.000|All Files|*.*",
                FileName = "Folder Selection",
                CheckFileExists = false,
                CheckPathExists = true,
                ReadOnlyChecked = true,
                Multiselect = false,
                ValidateNames = false,
                DereferenceLinks = true,
                ShowReadOnly = false,
                AddExtension = false,
                RestoreDirectory = true,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                CustomPlaces = null,
            };

            // Set the FolderPicker option
            openFileDialog.ShowDialog(System.Windows.Application.Current.MainWindow);

            // Get the selected folder path
            string ?selectedFolderPath = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
            if (selectedFolderPath != null && Directory.Exists(selectedFolderPath)) {
                return selectedFolderPath;
            }

            return "";
        }
        public static string ShowImagePickerDialog() {
            var openFileDialog = new OpenFileDialog {
                Title = "Select an image",
                Filter = "Image files (*.png;*.jpeg;*.jpg;*.gif)|*.png;*.jpeg;*.jpg;*.gif|All files (*.*)|*.*",
                FileName = "Image Selection",
                CheckFileExists = true,
                CheckPathExists = true,
                ReadOnlyChecked = true,
                Multiselect = false,
                ValidateNames = true,
                DereferenceLinks = true,
                ShowReadOnly = false,
                AddExtension = true,
                RestoreDirectory = true,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                CustomPlaces = null,
            };

            // Set the ImagePicker option
            openFileDialog.ShowDialog(System.Windows.Application.Current.MainWindow);

            // Get the selected image path
            string selectedImagePath = openFileDialog.FileName;
            if (!string.IsNullOrEmpty(selectedImagePath) && File.Exists(selectedImagePath)) {
                return selectedImagePath;
            }

            return "";
        }


        // shadow 
        private static double offsetOnEnter = 0;
        private static void Shadow_MouseUp(object sender, MouseButtonEventArgs e) {
            ((System.Windows.Shapes.Rectangle)sender).ReleaseMouseCapture();
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
            offsetOnEnter = 0;
        }
        private static void Shadow_MouseDown(object sender, MouseButtonEventArgs e) {
            ((System.Windows.Shapes.Rectangle)sender).CaptureMouse();
            CompositionTarget.Rendering += CompositionTarget_Rendering;

            double shadowPos = Helper.GetAbsolutePosition(shadow).X;
            double mousePos = Mouse.GetPosition(Application.Current.MainWindow).X; // Mouse Position Relative to the window
            double mousePosDelta = mousePos - (shadowPos); // Mouse Position Relative to the AHsoq 
            offsetOnEnter = mousePosDelta;
        }
        private static void CompositionTarget_Rendering(object? sender, EventArgs e) {
            double mousePos = Mouse.GetPosition(Application.Current.MainWindow).X; // Mouse Position Relative to the window

            double a = mousePos / (ClientGrid.ActualWidth);
            double b = 1 - a;

            if (a < 0) a = shadowRectWidth / ClientGrid.ActualWidth;
            if (b < 0) b = 0;
            ClientGrid.ColumnDefinitions[0].Width = new GridLength(a, GridUnitType.Star);
            ClientGrid.ColumnDefinitions[1].Width = new GridLength(b, GridUnitType.Star);
        }


        public static void GoldenRatio_GridSizes() {
            MainGrid.Height = Application.Current.MainWindow.ActualHeight;
            MainGrid.Width = Application.Current.MainWindow.ActualWidth;

            double a = MainGrid.ActualHeight;
            double b = MainGrid.ActualWidth - a;
            ClientGrid.ColumnDefinitions[0].Width = new GridLength(a, GridUnitType.Pixel);
            ClientGrid.ColumnDefinitions[1].Width = new GridLength(b, GridUnitType.Pixel);
        }
        public static void UpdateGridSizes_T2() {
            MainGrid.Height = Application.Current.MainWindow.ActualHeight;
            MainGrid.Width = Application.Current.MainWindow.ActualWidth;

            shadow.Height = WorkStationImageGrid.ActualHeight;
        }
    }
}
