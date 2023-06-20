using Retros.Program.DisplaySystem;
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
    public partial class WorkstationImageWindow : Window {

        public readonly WindowHandle WindowHandle;
        public readonly WorkstationImagePage Page = new WorkstationImagePage(WindowManager.MainWindow.SelectedWorkstation.ImageElement);

        public WorkstationImageWindow(double width, double height) {
            InitializeComponent();
            // Window Handle
            WindowHandle = new(this);
            UIManager.ColorThemeManager.Set_BG6(b => WindowHandle.SetBGColor(b));
            UIManager.ColorThemeManager.Set_BG1(b => Background = b);
            UIManager.ColorThemeManager.Set_BGh2(b => WindowHandle.ApplicationButtons.ColorWhenButtonHover = b);

            WindowHandle.ApplicationButtons.AddFullcreenButton();
            WindowHandle.ApplicationButtons.FullscreenButtonImageSource = UIManager.FullscreenIconPath;
            WindowHandle.ApplicationButtons.FullscreenButtonImagePadding = new Thickness(5);

            WindowHandle.ApplicationButtons.ActivateExitButtonSprite();
            WindowHandle.ApplicationButtons.ExitButtonImageSource = UIManager.ExitIconPath;
            WindowHandle.ApplicationButtons.ExitButtonImagePadding = new Thickness(5);

            WindowHandle.ApplicationButtons.ActivateMaximizeButtonSprite();
            WindowHandle.ApplicationButtons.MaximizeButtonImageSource = UIManager.MaximizeIconPath;
            WindowHandle.ApplicationButtons.MaximizeButtonImageSourceWhenMaximized = UIManager.MaximizeIconPath;
            WindowHandle.ApplicationButtons.MaximizeButtonImageSourceWhenWindowed = UIManager.WindowedIconPath;
            WindowHandle.ApplicationButtons.MaximizeButtonImagePadding = new Thickness(3);

            WindowHandle.ApplicationButtons.ActivateMinimizeButtonSprite();
            WindowHandle.ApplicationButtons.MinimizeButtonImageSource = UIManager.MinimizeIconPath;
            WindowHandle.ApplicationButtons.MinimizeButtonImagePadding = new Thickness(5);


            // UI
            WindowManager.MainWindow.SelectedWorkstation.ImageElement.Pages.Add(Page);
            PageFrame.Content = Page;

            Page.Width = width;
            Page.Height = height;
            Page.ImageMargin = new Thickness(0);
            Page.MainGrid.Children.Remove(Page.ImageHandleActivation);

            Width = width;
            Height = height;

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e) {
            if (PageFrame.Content is not null) {
                Page.Width = e.NewSize.Width;
                Page.Height = e.NewSize.Height;
            }
        }
    }
}
