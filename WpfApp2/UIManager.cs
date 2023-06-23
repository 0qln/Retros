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
using Retros.Program.Workstation.TabUI.Tabs;
using Retros;
using System.Windows.Media.Imaging;
using Retros.Program;
using System.IO.Pipes;
using Retros.Program.Workstation.Image;

namespace Retros
{
    public static class UIManager {
        public static ColorThemeManager ColorThemeManager => colorThemeManager;
        private static ColorThemeManager colorThemeManager = new(new ColorThemes.DefaultDark());
        public static string SettingsPath_Txt => "pack://application:,,,/sprites/settings.txt";
        public static string SettingsPath_Xaml => "pack://application:,,,/sprites/settings.xaml";
        public static string SettingsPath_Svg => "pack://application:,,,/sprites/settings.svg";
        public static string TuneIconPath_96 => "pack://application:,,,/sprites/tune-96.png";
        public static string TuneIconPath_48 => "pack://application:,,,/sprites/tune-96.png";
        public static string SettingsIconPath => "pack://application:,,,/sprites/settings5.png";
        public static string SettingsIconPath_24 => "pack://application:,,,/sprites/settings-24.png";
        public static string SettingsIconPath_96 => "pack://application:,,,/sprites/settings-96.png";
        public static string FullscreenIconPath => "pack://application:,,,/sprites/fullscreen.png";
        public static string ExitIconPath => "pack://application:,,,/sprites/close.png";
        public static string WindowedIconPath => "pack://application:,,,/sprites/windowed.png";
        public static string MaximizeIconPath => "pack://application:,,,/sprites/maximize.png";
        public static string MinimizeIconPath => "pack://application:,,,/sprites/minimize.png";
        public readonly static Thickness IconPadding = new Thickness(8);

        public static void LoadImage(WorkstationImage image) {
            string path = ShowImagePickerDialog();
            if (!String.IsNullOrEmpty(path)) {
                image.SetSourceImage(new Uri(path));
            }
            WindowManager.MainWindow!.WindowHandle!.HideAllMenus();
        }
        public static void SaveImage(WorkstationImage image) {
            string path = ShowFolderPickerDialog();

            if (String.IsNullOrEmpty(path)) return;
            if (!Path.Exists(path)) return;

            using System.Drawing.Bitmap bitmap = image.Render();
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
