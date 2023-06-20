using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Threading.Tasks;
using Utillities.Wpf;
using Retros.Program;

namespace Retros {
    public static class WindowManager {
        public static TimeSpan Framerate = TimeSpan.FromMilliseconds(1000 / 60);

        public static MainWindow MainWindow;
        public static SettingsWindow? SettingsWindow;


        public static void Start(MainWindow mainWindow) {
            MainWindow = mainWindow;
        }

        public static void ToggleSettings() {
            if (SettingsWindow == null || !SettingsWindow.IsVisible) {
                SettingsWindow = new SettingsWindow();
            }

            if (!SettingsWindow.IsVisible) {
                SettingsWindow.Show();
            }
            else {
                SettingsWindow.Close();
            }
        }

    }
}
