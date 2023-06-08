using Retros.Settings;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Shell;
using Utillities.Wpf;

namespace Retros {
    public partial class SettingsWindow : Window {
        public static WindowHandle? WindowHandle;

        public SettingsWindow() {

            InitializeComponent();

            WindowHandle = new(this);
            WindowHandle.SetWindowChromeActiveAll();
            WindowHandle.AddIcon(UIManager.SettingsIconPath);
            UIManager.ColorThemeManager.Set_BG3((brush) => WindowHandle.SetBGColor(brush));

            SettingsList_Padding.MinHeight = WindowHandle.Height;
            SettingDetailDisplay_Padding.MinHeight = WindowHandle.Height;
            UIManager.ColorThemeManager.Set_BG1(newBrush => SettingsList.Background = newBrush);
            UIManager.ColorThemeManager.Set_BG2(newBrush => SettingDetailDisplaySP.Background = newBrush);

            TextBlock windowTitle = new TextBlock { Text = "Settings", FontSize = 21, FontWeight = FontWeights.Light };
            UIManager.ColorThemeManager.Set_FC1(b => windowTitle.Foreground = b);
            WindowHandle.AddElement(windowTitle);

            Loaded += (s, e) => {
                Tab appearance = new Tab("Appearance");
                appearance.AddDetail(new TabDetail(new Settings.Pages.ColorTheme()));
                appearance.AddDetail(new TabDetail(new Settings.Pages.WorkstationImageSettingsPage()));

                appearance._Body.Show();
            };
            
        }

    }
}
