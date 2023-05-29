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
using System.Windows.Media.Imaging;
using Retros.ProgramWindow;
using System.IO.Pipes;

namespace Retros {
    public static class UIManager {
        public static ColorThemeManager ColorThemeManager => colorThemeManager;
        private static ColorThemeManager colorThemeManager = new(new ColorThemes.DefaultDark());
        public static string SettingsIconPath => "pack://application:,,,/settings5.png";

        public static void LoadImage() {
            string path = ShowImagePickerDialog();
            if (path != null) {
                WindowManager.MainWindow!.Workstation.ImageElement.SetSource(new BitmapImage(new Uri(path)));
            }
            WindowManager.MainWindow!.windowHandle!.HideAllMenus();
        }
        public static void SaveImage() {
            string path = ShowFolderPickerDialog();

            if (String.IsNullOrEmpty(path)) return;
            if (!Path.Exists(path)) return;


            System.Windows.Controls.Image image = WindowManager.MainWindow!.Workstation.ImageElement.CurrentImage;
            System.Windows.Media.Imaging.BitmapSource bitmapSource = (System.Windows.Media.Imaging.BitmapSource)image.Source;
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(
                bitmapSource.PixelWidth,
                bitmapSource.PixelHeight,
                System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            System.Drawing.Imaging.BitmapData bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.WriteOnly,
                bitmap.PixelFormat);

            bitmapSource.CopyPixels(
                new System.Windows.Int32Rect(0, 0, bitmapSource.PixelWidth, bitmapSource.PixelHeight),
                bitmapData.Scan0,
                bitmapData.Height * bitmapData.Stride,
                bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);

            string filePath = path + "\\image.png";
            bitmap.Save(filePath);

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
