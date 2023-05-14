using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using WpfCustomControls;
using Retros.WorkstationTableElements.Tabs;
using Retros.WorkstationTableElements.Bodies;
using Retros.WorkstationTableElements.Handles;


namespace Retros {
    static class UIManager {
        //Window
        public static Grid MainGrid = new();
        private static Canvas? canvas;

        // Client Area
        public static Workstation Workstation = new();

        public static Grid ClientGrid = new();
        public static Grid WorkStationGrid = new();
        public static Grid WorkStationImageGrid = new();
        public static System.Windows.Shapes.Rectangle shadow = new System.Windows.Shapes.Rectangle();
        private static double shadowRectWidth = 5;

        public static Brush WorkStationGrid_BG = Helper.StringToSolidColorBrush("#1f1f1f");
        public static Brush WorkActionGrid_BG = Helper.StringToSolidColorBrush("#2e2e2e");

        public static TimeSpan Framerate = TimeSpan.FromMilliseconds(1000 / 60);

        //WindowHandle
        private static WindowHandle windowHandle = new();
        public static DropDownMenu fileMenu = new("File");
        public static DropDownMenu editMenu = new("Edit");
        public static DropDownMenu viewMenu = new("View");
        public static DropDownMenu settingsMenu = new("Settings");

        public static void Start(Canvas pCanvas) {

            canvas = pCanvas;

            // Main Grid
            MainGrid.Loaded += (sender, e) => {
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

            Helper.SetChildInGrid(WorkStationGrid, Workstation.FrameworkElement, 1, 0);
            Workstation.TableElement.AddTab(new ImageFilterTab(new ImageFilter(), new DefaultHandle("Filters")));
            Workstation.TableElement.SelectTab(0);

            // WorkStation
            Helper.SetChildInGrid(ClientGrid, WorkStationImageGrid, 0, 0);
            WorkStationImageGrid.Background = WorkStationGrid_BG;

            Helper.AddColumn(WorkStationImageGrid, 1, GridUnitType.Star);
            Helper.AddColumn(WorkStationImageGrid, 1, GridUnitType.Auto);
            Workstation.ImageElement.CurrentImage.HorizontalAlignment = HorizontalAlignment.Stretch;
            Workstation.ImageElement.CurrentImage.Margin = new Thickness(50);
            Helper.SetChildInGrid(WorkStationImageGrid, Workstation.ImageElement.CurrentImage, 0, 0);
            Helper.SetChildInGrid(WorkStationImageGrid, shadow, 0, 1);

            DropShadowEffect effect1 = new DropShadowEffect { BlurRadius = 30, ShadowDepth = 15, Color = Colors.Black, Opacity = 0.8, Direction = 270 };
            Workstation.ImageElement.CurrentImage.Effect = effect1;



            // shadow
            DropShadowEffect effect2 = new DropShadowEffect { BlurRadius = 25, ShadowDepth = 15, Color = Colors.Black, Opacity = 0.80, Direction = 180 };
            shadow.Effect = effect2;
            shadow.Width = shadowRectWidth;
            shadow.Fill = WorkActionGrid_BG;
            Panel.SetZIndex(shadow, -10);
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

            Application.Current.MainWindow.Loaded += (sender, e) => UpdateGridSizes_T2();
            canvas!.LayoutUpdated += (sender, e) => UpdateGridSizes_T2();

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

        public static void UpdateGridSizes_T2() {
            MainGrid.Height = Application.Current.MainWindow.ActualHeight;
            MainGrid.Width = Application.Current.MainWindow.ActualWidth;

            shadow.Height = WorkStationImageGrid.ActualHeight;
        }

        public static void LoadImage() {
            string path = ShowImagePickerDialog();
            if (path != null) {
                Image image = new();
                Helper.SetImageSource(image, path);
                Workstation.ImageElement.CurrentImage = image;
            }
            windowHandle.HideAllMenus();
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
            openFileDialog.ShowDialog(Application.Current.MainWindow);

            // Get the selected folder path
            string? selectedFolderPath = Path.GetDirectoryName(openFileDialog.FileName);
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
            openFileDialog.ShowDialog(Application.Current.MainWindow);

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
            double mousePosDelta = mousePos - shadowPos; // Mouse Position Relative to the AHsoq 
            offsetOnEnter = mousePosDelta;
        }
        private static void CompositionTarget_Rendering(object? sender, EventArgs e) {
            double mousePos = Mouse.GetPosition(Application.Current.MainWindow).X; // Mouse Position Relative to the window

            double a = mousePos / ClientGrid.ActualWidth;
            double b = 1 - a;

            if (a < 0) a = shadowRectWidth / ClientGrid.ActualWidth;
            if (b < 0) b = 0;
            ClientGrid.ColumnDefinitions[0].Width = new GridLength(a, GridUnitType.Star);
            ClientGrid.ColumnDefinitions[1].Width = new GridLength(b, GridUnitType.Star);
        }
    }
}
