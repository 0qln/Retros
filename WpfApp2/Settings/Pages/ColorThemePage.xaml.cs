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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Utillities.Wpf;

namespace Retros.Settings.Pages {
    public partial class ColorTheme : Page
    {
        public ColorTheme()
        {
            InitializeComponent();
            UIManager.ColorThemeManager.SetStyle(Headline, () => TabDetail.Body.HeadlineStyle(Title));
            UIManager.ColorThemeManager.SetStyle(ChangeButton, () => TabDetail.Body.ButtonStyle("Change"));
            UIManager.ColorThemeManager.SetStyle(ImportButton, () => TabDetail.Body.ButtonStyle("Import"));
            UIManager.ColorThemeManager.SetStyle(ExportButton, () => TabDetail.Body.ButtonStyle("Export"));
            UIManager.ColorThemeManager.SetStyle(ImportText, () => TabDetail.Body.TextboxStyle());
            UIManager.ColorThemeManager.SetStyle(ExportText, () => TabDetail.Body.TextboxStyle());
            UIManager.ColorThemeManager.SetStyle(ImportDescription, () => TabDetail.Body.TextblockStyle("Enter the location of your .json file: "));
            UIManager.ColorThemeManager.SetStyle(ExportDescription, () => TabDetail.Body.TextblockStyle("Enter the location of the folder, that the .json file will be safed to: "));


            UpdateAvailableThemes();
        }

        public void UpdateAvailableThemes() {
            ThemeSelcetion.Items.Clear();
            UIManager.ColorThemeManager.ColorThemes.ForEach(theme =>  ThemeSelcetion.Items.Add(theme.Name));
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e) {
            var selected = ThemeSelcetion.SelectedIndex;
            if (selected == -1) return;

            UIManager.ColorThemeManager.SetTheme(UIManager.ColorThemeManager.ColorThemes[selected]);
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
