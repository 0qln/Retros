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
    public partial class ImageHistoryTab : Page {
        public ImageHistoryTab() {
            InitializeComponent();

            UIManager.ColorThemeManager.SetStyle(Headline, TabDetail.Body.HeadlineStyle);
            UIManager.ColorThemeManager.SetStyle(ShowFilterIntensityInName_Text, TabDetail.Body.TextblockStyle);
            UIManager.ColorThemeManager.SetStyle(ShowFilterIntensityInName_Checkbox, TabDetail.Body.CheckBoxStyle);
            ShowFilterIntensityInName_Checkbox.TemplateApplied += () 
                => UIManager.ColorThemeManager.SetStyle(ShowFilterIntensityInName_Checkbox.ButtonElement!, TabDetail.Body.CheckBoxButtonStyle);
            
            ShowFilterIntensityInName_Checkbox.Diameter = SettingsManager.SettingsPages.TextBlockHeight_Normal.Value;

            foreach (FrameworkElement child in SubStackpanel.Children) {
                if (child.Tag.ToString() == "isLine") {
                    SettingsManager.SettingsPages.LineMarginTop.ValueChanged += (value) => 
                        child.Margin = new Thickness(0, value, 0, 0);
                    
                }
            }
        }
    }
}
