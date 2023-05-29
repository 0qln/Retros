using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using Utillities.Wpf;

// Handles manage the funtionality as tab (e.g. dragging around) and the Tab titlebar UIElements
namespace Retros.ProgramWindow.Interactive.Tabs.Handles {
    public abstract class Handle {
        protected Border border = new();
        protected StackPanel stackPanel = new();
        protected TextBlock name = new();
        protected System.Windows.Controls.Image icon = new();

        public FrameworkElement FrameworkElement => border;

        public Handle(string name) {
            stackPanel.Orientation = Orientation.Horizontal;
            stackPanel.Children.Add(this.name);
            stackPanel.Background = System.Windows.Media.Brushes.Transparent;
            stackPanel.Margin = new Thickness(3, 0, 5, 0);
            stackPanel.MouseEnter += (s, e) => { border.BorderBrush = System.Windows.Media.Brushes.Gainsboro; };
            stackPanel.MouseLeave += (s, e) => { border.BorderBrush = System.Windows.Media.Brushes.Transparent; };

            border.Child = stackPanel;
            UIManager.ColorThemeManager.Set_BG4(newBrush => border.Background = newBrush);
            border.BorderBrush = System.Windows.Media.Brushes.Transparent;
            border.BorderThickness = new Thickness(0.5);

            this.name.Text = name;
            this.name.VerticalAlignment = VerticalAlignment.Center;
            this.name.Foreground = System.Windows.Media.Brushes.Gainsboro;
            this.name.TextDecorations = TextDecorations.Underline;
        }

        public void SetIcon(string path) {
            Helper.SetImageSource(icon, path);
            stackPanel.Children.Insert(0, icon);
        }
    }

    public class DefaultHandle : Handle {
        public DefaultHandle(string name) : base(name) { }
    }
}
