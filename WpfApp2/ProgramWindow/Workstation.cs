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
        public WorkstationImage ImageElement = new(DefaultPath);
        public WorkstationTable TableElement = new(MainGrid);

        public readonly static string DefaultPath = @"C:\Users\User\OneDrive\Bilder\alexandre-cabanel-fallen-angel-1847-obelisk-art-history.png";

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
