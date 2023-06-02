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
            UIManager.ColorThemeManager.SetStyle(Headline, () => TabDetail.Body.HeadlineStyle(Title));
            UIManager.ColorThemeManager.SetStyle(Backdrop_Shadow_CheckBox, TabDetail.Body.CheckBoxStyle);
            UIManager.ColorThemeManager.SetStyle(Backdrop_Shadow_Text, () => TabDetail.Body.TextblockStyle("Enable backdrop shadow"));
        }
    }
}
