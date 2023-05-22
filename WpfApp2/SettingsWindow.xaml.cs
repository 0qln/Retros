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
using WpfCustomControls;

namespace Retros {
    public partial class SettingsWindow : Window {
        private WindowHandle windowHandle;

        public SettingsWindow() {
            InitializeComponent();

            windowHandle = new(this);
            windowHandle.SetParentWindow(MainCanvas!);
            windowHandle.SetBGColor(Helper.StringToSolidColorBrush("#000000", 0.45));
            windowHandle.ApplicationButtons.ColorWhenButtonHover = Helper.StringToSolidColorBrush("#000000", 0.4);
            windowHandle.SetWindowChromActiveAll();

            MainCanvas.Background = Brushes.Tomato;
        }


    }
}
