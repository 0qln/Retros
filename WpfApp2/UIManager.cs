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
using WpfUtillities;
using Retros.WorkstationTableElements.Tabs;
using Retros.WorkstationTableElements.Bodies;
using Retros.WorkstationTableElements.Handles;


namespace Retros {
    public static class UIManager {
        public static void LoadImage() {
            string path = ShowImagePickerDialog();
            if (path != null) {
                Image image = new();
                Helper.SetImageSource(image, path);
                WindowManager.MainWindow!.Workstation.ImageElement.CurrentImage = image;
            }
            WindowManager.MainWindow!.windowHandle.HideAllMenus();
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
