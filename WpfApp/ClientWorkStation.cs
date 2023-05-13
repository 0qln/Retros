using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfCustomControls;
using Retros.ClientWorkStation.Tabs;


namespace Retros.ClientWorkStation {
    public static class WorkStation {
        // UI
        private static Grid mainGrid = new();
        private static Border border = new();

        public static Grid MainGrid => mainGrid;
        public static FrameworkElement FrameworkElement => border;

        // System
        public static WorkTable WorkTable = new();
        public static WorkstationImage WorkstationImage = new("C:\\Users\\User\\OneDrive\\Bilder\\Wallpapers\\mountain-lake-reflection-nature-scenery-hd-wallpaper-uhdpaper.com-385@0@h.jpg");


        public static void Instanciate() {
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
