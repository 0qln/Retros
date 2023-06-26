using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Retros.Settings.Pages {
    /// <summary>
    /// Interaktionslogik für WorkstationImageSettingsPage.xaml
    /// </summary>
    public partial class WorkstationImageSettingsPage : Page {

        public WorkstationImageSettingsPage() {
            InitializeComponent();
            UIManager.ColorThemeManager.SetStyle(Headline, TabDetail.Body.HeadlineStyle);
            UIManager.ColorThemeManager.SetStyle(Enable_Text, TabDetail.Body.TextblockStyle);
            UIManager.ColorThemeManager.SetStyle(BlurRadius_Text, TabDetail.Body.TextblockStyle);
            UIManager.ColorThemeManager.SetStyle(BlurRadius_Value, TabDetail.Body.Textbox_NumericStyle);
            UIManager.ColorThemeManager.SetStyle(ShadowDepth_Text, TabDetail.Body.TextblockStyle);
            UIManager.ColorThemeManager.SetStyle(ShadowDepth_Value, TabDetail.Body.Textbox_NumericStyle);
            UIManager.ColorThemeManager.SetStyle(Opacity_Text, TabDetail.Body.TextblockStyle);
            UIManager.ColorThemeManager.SetStyle(Enable_CheckBox, TabDetail.Body.CheckBoxStyle);
            UIManager.ColorThemeManager.SetStyle(Opacity_Value, TabDetail.Body.SliderStyle);
            Enable_CheckBox.TemplateApplied += ()
                => UIManager.ColorThemeManager.SetStyle(Enable_CheckBox.ButtonElement!, TabDetail.Body.CheckBoxButtonStyle);

            Enable_CheckBox.Diameter = SettingsManager.SettingsPages.TextBlockHeight_Normal.Value;

            Enable_CheckBox.Toggled += (value) => SettingsManager.WorkstationImageShadow.Enabled.Value = value;
            BlurRadius_Value.TextChanged += BlurRadius_Value_TextChanged;
            BlurRadius_Value.Text = SettingsManager.WorkstationImageShadow.BlurRadius.Value.ToString();
            ShadowDepth_Value.TextChanged += ShadowDepth_Value_TextChanged;
            ShadowDepth_Value.Text = SettingsManager.WorkstationImageShadow.ShadowDepth.Value.ToString();
            Opacity_Value.ValueChanged += Opacity_Value_ValueChanged;
            Opacity_Value.Value = SettingsManager.WorkstationImageShadow.Opacity.Value;
        }

        private void Opacity_Value_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SettingsManager.WorkstationImageShadow.Opacity.Value = e.NewValue;
        }

        private void ShadowDepth_Value_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Validate the user input
            if (!ValidateNumber(ShadowDepth_Value.Text))
            {
                return;
            }

            // Use the user input as the new value
            SettingsManager.WorkstationImageShadow.ShadowDepth.Value = Double.Parse(ShadowDepth_Value.Text);
        }

        private void BlurRadius_Value_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Validate the user input
            if (!ValidateNumber(BlurRadius_Value.Text))
            {
                return;
            }

            // Use the user input as the new value
            SettingsManager.WorkstationImageShadow.BlurRadius.Value = Double.Parse(BlurRadius_Value.Text);
        }

        private bool ValidateNumber(string s)
        {
            if (String.IsNullOrWhiteSpace(s))
            {
                return false;
            }
            MatchCollection matchCollection = Regex.Matches(s, $"(\\d+|\\.)");
            string matchStr = String.Join(String.Empty, matchCollection.ToList());
            return matchStr.Equals(s);
        }
    }
}
