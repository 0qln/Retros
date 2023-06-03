using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Utillities.Wpf;

namespace Retros.Settings.Pages {
    public partial class ColorTheme : Page
    {
        private SelectionBox_OLD<TextBlock> ThemeSelection;

        public ColorTheme()
        {
            InitializeComponent();


            UIManager.ColorThemeManager.SetStyle(Headline, TabDetail.Body.HeadlineStyle);
            UIManager.ColorThemeManager.SetStyle(ChangeButton, TabDetail.Body.ButtonStyle);
            UIManager.ColorThemeManager.SetStyle(ImportButton, TabDetail.Body.ButtonStyle);
            UIManager.ColorThemeManager.SetStyle(ExportButton, TabDetail.Body.ButtonStyle);
            UIManager.ColorThemeManager.SetStyle(ImportText, TabDetail.Body.TextboxStyle);
            UIManager.ColorThemeManager.SetStyle(ExportText, TabDetail.Body.TextboxStyle);
            UIManager.ColorThemeManager.SetStyle(ImportDescription, TabDetail.Body.TextblockStyle);
            UIManager.ColorThemeManager.SetStyle(ExportDescription, TabDetail.Body.TextblockStyle);

            UpdateAvailableThemes();

            ThemeSelection = new(_canvas);
            Binding binding = new Binding("ActualWidth") { Source = ChangeButton };
            ThemeSelection.FrameworkElement.SetBinding(FrameworkElement.MarginProperty, binding);

            ChangeButton.Loaded += (s, e) => 
            DebugLibrary.Console.Log(ChangeButton.ActualWidth);
        }

        public void UpdateAvailableThemes() {
            ///ThemeSelection.Items.Clear();
            ///UIManager.ColorThemeManager.ColorThemes.ForEach(theme =>  ThemeSelection.Items.Add(theme.Name));
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e) {
            ///var selected = ThemeSelection.SelectedIndex;
            ///if (selected == -1) return;

            ///UIManager.ColorThemeManager.SetTheme(UIManager.ColorThemeManager.ColorThemes[selected]);
        }

        private void Import_Click(object sender, RoutedEventArgs e) {
            UIManager.ColorThemeManager.LoadFromFile(ImportText.Text);
            UpdateAvailableThemes();
        }

        private void Export_Click(object sender, RoutedEventArgs e) {
            UIManager.ColorThemeManager.SaveToFile(ExportText.Text);
        }
    }
}
