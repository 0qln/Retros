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
using WpfUtillities;

namespace Retros
{
    public partial class MainWindow : Window {

        private WindowHandle windowHandle = new(Application.Current.MainWindow);
        public DropDownMenu fileMenu = new("File", Application.Current.MainWindow);
        public DropDownMenu editMenu = new("Edit", Application.Current.MainWindow);
        public DropDownMenu viewMenu = new("View", Application.Current.MainWindow);
        public DropDownMenu settingsMenu = new("Settings", Application.Current.MainWindow);

        public MainWindow() {
            InitializeComponent();
            UIManager.Start(MainCanvas);
        }
    }
}
