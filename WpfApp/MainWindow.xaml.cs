using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shell;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using Retros;
using WpfCustomControls;



namespace WpfApp {
    public partial class MainWindow : Window {
        private static WindowElements ?windowElements;

        public MainWindow() {
            InitializeComponent();
            LayoutUpdated += MainWindow_LayoutUpdated; ;

            windowElements = new(MainCanvas);

            
            
        }

        private void MainWindow_LayoutUpdated(object? sender, EventArgs e) {
            MainCanvas.Height = ActualHeight;
            MainCanvas.Width = ActualWidth;
        }


    }
}
