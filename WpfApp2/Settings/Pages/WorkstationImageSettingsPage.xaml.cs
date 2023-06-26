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

namespace Retros.Settings.Pages {
    /// <summary>
    /// Interaktionslogik für WorkstationImageSettingsPage.xaml
    /// </summary>
    public partial class WorkstationImageSettingsPage : Page {
        public WorkstationImageSettingsPage() {
            InitializeComponent();
            UIManager.ColorThemeManager.SetStyle(Headline, TabDetail.Body.HeadlineStyle);

            UIManager.ColorThemeManager.SetStyle(Enable_Text, TabDetail.Body.TextblockStyle);

            //UIManager.ColorThemeManager.SetStyle(BlurRadius_Text, () => TabDetail);

            Enable_CheckBox.Toggled += (value) => SettingsManager.WorkstationImageShadow.Enabled.Value = value;
            Enable_CheckBox.Diameter = SettingsManager.SettingsPages.TextBlockHeight_Normal.Value;
            SettingsManager.SettingsPages.TextBlockHeight_Normal.ValueChanged += (value)
                => Enable_CheckBox.Diameter = value;
            UIManager.ColorThemeManager.Set_FC1(b => Enable_CheckBox.Background = b);
            UIManager.ColorThemeManager.Set_FC1(b => Enable_CheckBox.BorderBrush = b);
            Enable_CheckBox.TemplateApplied += ()
                => UIManager.ColorThemeManager.SetStyle(Enable_CheckBox.ButtonElement!, TabDetail.Body.ButtonStyle);

        }


    }
}
