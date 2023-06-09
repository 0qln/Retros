﻿using System;
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
        public ColorTheme()
        {
            InitializeComponent();
                        
            UIManager.ColorThemeManager.SetStyle(Headline, TabDetail.Body.HeadlineStyle);
            UIManager.ColorThemeManager.SetStyle(ChangeButton, TabDetail.Body.ButtonStyle);
            UIManager.ColorThemeManager.SetStyle(ImportButton, TabDetail.Body.ButtonStyle);
            UIManager.ColorThemeManager.SetStyle(ExportButton, TabDetail.Body.ButtonStyle);
            UIManager.ColorThemeManager.SetStyle(ImportText, TabDetail.Body.Textbox_TextStyle);
            UIManager.ColorThemeManager.SetStyle(ExportText, TabDetail.Body.Textbox_TextStyle);
            UIManager.ColorThemeManager.SetStyle(ImportDescription, TabDetail.Body.TextblockStyle);
            UIManager.ColorThemeManager.SetStyle(ExportDescription, TabDetail.Body.TextblockStyle);
            UIManager.ColorThemeManager.SetStyle(ThemeSelectionBox, TabDetail.Body.SelectionBoxStyle);
         
            UpdateAvailableThemes();            
        }

        public void UpdateAvailableThemes() {
            foreach (var theme in UIManager.ColorThemeManager.ColorThemes) ThemeSelectionBox.AddOption(theme.Name);
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e) {
            var selected = ThemeSelectionBox.Selected;
            var selectedTheme = UIManager.ColorThemeManager.ColorThemes.Find(
                theme => theme.Name == selected);

            if (selectedTheme != null) {
                UIManager.ColorThemeManager.SetTheme(selectedTheme);
            }
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
