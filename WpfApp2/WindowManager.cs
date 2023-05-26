using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Threading.Tasks;
using WpfUtillities;

namespace Retros {
    public static class WindowManager {
        public static TimeSpan Framerate = TimeSpan.FromMilliseconds(1000 / 60);

        public static MainWindow? MainWindow;
        public static SettingsWindow? SettingsWindow;

        public static Brush WorkStationImageGrid_BG = Helper.StringToSolidColorBrush("#1f1f1f");
        public static Brush WorkStationGrid_BG = Helper.StringToSolidColorBrush("#2e2e2e");

        public static Brush whBackground = Helper.StringToSolidColorBrush("#000000", 0.45);
        public static Brush whApplicationButtonHover = Helper.StringToSolidColorBrush("#000000", 0.4);


        public static void Start(MainWindow mainWindow) {
            MainWindow = mainWindow;
        }

        public static void ToggleSettings() {
            if (SettingsWindow == null || SettingsWindow.IsVisible) {
                SettingsWindow = new SettingsWindow();
            }

            if (SettingsWindow.IsVisible) {
                SettingsWindow.Show();
            }
            else {
                SettingsWindow.Close();
            }
        }

    }
}
