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
        public static WindowHandle WindowHandle;

        public SettingsWindow() {

            InitializeComponent();

            WindowHandle = new(this);
            WindowHandle.ClientButtons.SetWindowChromActiveAll(true);
            WindowHandle.AddIcon(UIManager.SettingsIconPath);
            UIManager.ColorThemeManager.Set_BG3(b => WindowHandle.BackgroundColor = b);

            SettingsList_Padding.MinHeight = WindowHandle.Height;
            SettingDetailDisplay_Padding.MinHeight = WindowHandle.Height;
            UIManager.ColorThemeManager.Set_BG1(b => SettingsList.Background = b);
            UIManager.ColorThemeManager.Set_BG2(b => SettingDetailDisplaySP.Background = b);

            TextBlock windowTitle = new TextBlock { Text = "Settings", FontSize = 21, FontWeight = FontWeights.Light };
            UIManager.ColorThemeManager.Set_FC1(b => windowTitle.Foreground = b);
            WindowHandle.Title = windowTitle;

            WindowHandle.ApplicationButtons.ActivateExitButtonSprite();
            WindowHandle.ApplicationButtons.ExitButtonImageSource = UIManager.ExitIconPath;
            WindowHandle.ApplicationButtons.ExitButtonImagePadding = new Thickness(5);

            WindowHandle.ApplicationButtons.ActivateMaximizeButtonSprite();
            WindowHandle.ApplicationButtons.MaximizeButtonImageSource = UIManager.MaximizeIconPath;
            WindowHandle.ApplicationButtons.MaximizeButtonImageSourceWhenMaximized = UIManager.MaximizeIconPath;
            WindowHandle.ApplicationButtons.MaximizeButtonImageSourceWhenWindowed = UIManager.WindowedIconPath;
            WindowHandle.ApplicationButtons.MaximizeButtonImagePadding = new Thickness(3);

            WindowHandle.ApplicationButtons.ActivateMinimizeButtonSprite();
            WindowHandle.ApplicationButtons.MinimizeButtonImageSource = UIManager.MinimizeIconPath;
            WindowHandle.ApplicationButtons.MinimizeButtonImagePadding = new Thickness(5);


            Loaded += (s, e) => {
                Tab appearance = new Tab("Appearance");
                appearance.AddDetail(new TabDetail(new Settings.Pages.ColorTheme()));
                appearance.AddDetail(new TabDetail(new Settings.Pages.WorkstationImageSettingsPage()));
                appearance.AddDetail(new TabDetail(new Settings.Pages.ImageHistoryTab()));

                appearance._Body.Show();
            };
            
        }

    }
}
