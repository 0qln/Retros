using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.IO;
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
    public partial class DefaultExportPath : Page
    {
        public DefaultExportPath()
        {
            InitializeComponent();
        }

        private void SaveJson_Click(object sender, RoutedEventArgs e) {
            string json = ColorThemes.Json.Serialize(UIManager.ColorThemeManager.CurrentTheme);
            Debugger.Console.Log(json);

            var c = ColorThemes.Json.Deserialize(json);

            File.WriteAllText("\\Styles\\style.json", json);

            UIManager.ColorThemeManager.SetTheme(new ColorThemes.Test());
        }
    }
}
