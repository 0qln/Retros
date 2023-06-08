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
using Utillities.Wpf;

namespace Retros {
    public partial class WorkstatoinImageWindow : Window {

        public readonly WindowHandle WindowHandle;

        public WorkstatoinImageWindow(WorkstationImagePage page, double width, double height) {
            InitializeComponent();            
            PageFrame.Content = page;

            Width = width;
            Height = height;

            WindowHandle = new(this);
            UIManager.ColorThemeManager.Set_BG6(b => WindowHandle.SetBGColor(b));
            UIManager.ColorThemeManager.Set_BG1(b => Background = b);
            UIManager.ColorThemeManager.Set_BGh1(b => WindowHandle.ApplicationButtons.ColorWhenButtonHover = b);
        }

        private void PageFrame_SizeChanged(object sender, SizeChangedEventArgs e) {
            if (PageFrame.Content is not null) {
                (PageFrame.Content as Page)!.Width = e.NewSize.Width;
                (PageFrame.Content as Page)!.Height = e.NewSize.Height;
            }

        }
    }
}
