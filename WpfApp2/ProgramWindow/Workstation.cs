using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Retros.ProgramWindow.Interactive;
using Retros.ProgramWindow.DisplaySystem;

namespace Retros.ProgramWindow {
    public partial class Workstation : IFrameworkElement {
        // UI
        static private Grid mainGrid = new();
        private Border border = new();

        static public Grid MainGrid => mainGrid;
        public FrameworkElement FrameworkElement => border;

        // System
        public WorkstationImage ImageElement = new();
        public WorkstationTable TableElement = new(MainGrid);

        private static string defaultPath => @"C:\Users\User\OneDrive\Bilder\Wallpapers\70547-Kaf-Virtual-YoutuberKaf-Virtual-Youtuber-4k-Ultra.jpg";

        public Workstation() {
            // Init Border
            border.VerticalAlignment = VerticalAlignment.Stretch;
            border.HorizontalAlignment = HorizontalAlignment.Stretch;
            border.Margin = new Thickness(10);
            border.Child = mainGrid;
            border.BorderBrush = System.Windows.Media.Brushes.Transparent;
            border.Background = System.Windows.Media.Brushes.Transparent;
            border.BorderThickness = new Thickness(1);
        }
    }
}
