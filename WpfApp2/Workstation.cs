using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Retros
{
    public partial class Workstation : IFrameworkElement
    {
        // UI
        static private Grid mainGrid = new();
        private Border border = new();

        static public Grid MainGrid => mainGrid;
        public FrameworkElement FrameworkElement => border;

        // System
        public WorkstationImage ImageElement = new(defaultPath);
        public WorkstationTable TableElement = new();

        private static string defaultPath => "C:\\Users\\User\\OneDrive\\Bilder\\Wallpapers\\mountain-lake-reflection-nature-scenery-hd-wallpaper-uhdpaper.com-385@0@h.jpg";

        public Workstation()
        {
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
