using Retros.ProgramWindow.DisplaySystem;
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

namespace Retros {
    public partial class WorkstatoinImageWindow : Window {
        public WorkstatoinImageWindow(WorkstationImagePage page) {
            InitializeComponent();            
            PageFrame.Content = page;
        }

        public WorkstatoinImageWindow() {
            InitializeComponent();
        }
    }
}
