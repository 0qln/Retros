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

namespace Retros.Settings.Pages
{
    public partial class ColorTheme : Page
    {
        public ColorTheme()
        {
            InitializeComponent();
        }

        bool b = true;
        private void ChangeButton_Click(object sender, RoutedEventArgs e) {
            if (b) {
                UIManager.ColorThemeManager.SetTheme(new ColorThemes.Test());
                b = false;
            }
            else {
                UIManager.ColorThemeManager.SetTheme(new ColorThemes.DefaultDark());
                b = true;
            }
        }
    }
}
