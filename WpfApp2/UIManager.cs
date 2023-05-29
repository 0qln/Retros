using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using Utillities.Wpf;
using Retros.ProgramWindow.Interactive.Tabs;
using Retros.ProgramWindow.Interactive.Tabs.Bodies;
using Retros.ProgramWindow.Interactive.Tabs.Handles;
using Retros;

namespace Retros {
    public static class UIManager {
        public static ColorThemeManager ColorThemeManager => colorThemeManager;
        private static ColorThemeManager colorThemeManager = new(new ColorThemes.DefaultDark());

        // These should be getters
        /// These will get changed when the theme changes
        public static List<Brush> Background_Colors = new List<Brush> {
            Helper.StringToSolidColorBrush("#1f1f1f", 1),
            Helper.StringToSolidColorBrush("#2e2e2e", 2),
            Helper.StringToSolidColorBrush("#000000", 0.45)
        };
        public static List<Brush> Acctent_Colors = new List<Brush> {
            Helper.StringToSolidColorBrush("#bb1f1f", 1),
        };
        public static List<Brush> Highlight_Colors = new List<Brush> {
            Helper.StringToSolidColorBrush("#FFFFFF", 0.1),
            Helper.StringToSolidColorBrush("#000000", 0.4) //TODO add
        };

        public static IColorTheme CurrentTheme => colorThemes[currentThemeIndex];
        private static List<IColorTheme> colorThemes = new();
        private static int currentThemeIndex;

        // Create setters here...



        //public static Brush WorkStationImageGrid_BG = Helper.StringToSolidColorBrush("#1f1f1f");
        //public static Brush WorkStationGrid_BG = Helper.StringToSolidColorBrush("#2e2e2e");

        //public static Brush whBackground = Helper.StringToSolidColorBrush("#000000", 0.45);
        //public static Brush whApplicationButtonHover = Helper.StringToSolidColorBrush("#000000", 0.4);

        public static void LoadImage() {
            string path = ShowImagePickerDialog();
            if (path != null) {
                Image image = new();
                Helper.SetImageSource(image, path);
                WindowManager.MainWindow!.Workstation.ImageElement.CurrentImage = image;
            }
            WindowManager.MainWindow!.windowHandle!.HideAllMenus();
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
    }
}
