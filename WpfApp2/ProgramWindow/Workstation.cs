using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Retros.ProgramWindow.Interactive;
using Retros.ProgramWindow.DisplaySystem;
using System.Windows.Navigation;
using Retros.ProgramWindow.Interactive.Tabs.Bodies;
using Retros.ProgramWindow.Interactive.Tabs.Handles;
using Retros.ProgramWindow.Interactive.Tabs;
using System.Windows.Media;

namespace Retros.ProgramWindow {
    public partial class Workstation {
        // UI
        private Grid _mainGrid = new();
        private Border _border = new();
        private WorkstationPage _page;
        private Frame _pageFrame = new();

        public Grid MainGrid => _mainGrid;
        public FrameworkElement FrameworkElement => _pageFrame;

        // System
        public WorkstationImage ImageElement { get; } = new(DefaultPath);
        public WorkstationTable TableElement { get; } = new();

        public readonly static string DefaultPath =
            @"C:\Users\User\OneDrive\Bilder\Wallpapers\43666-Kaf-Virtual-YoutuberVirtual-Youtuber-HD-Wallpaper.jpg";


        public Workstation(double topPadding) {
            // Table
            TableElement.AddTab(new ImageFilterTab(new ImageFilter(ImageElement), new DefaultHandle("Filters")));
            TableElement.AddTab(new ImageHistoryTab(new ImageHistory(ImageElement), new DefaultHandle("Change History")));
            TableElement.SelectTab(0);

            // Image
            ImageElement.Page.WindowInitiated += (window) => {
                window.Closed += (_,_) => {
                    ShowImageUI();
                    ImageElement.FrameworkElement.Visibility = Visibility.Visible;
                };
            };

            // Page
            _page = new WorkstationPage(ImageElement, TableElement, topPadding);
            _pageFrame.Content = _page;

            // Border
            _border.VerticalAlignment = VerticalAlignment.Stretch;
            _border.HorizontalAlignment = HorizontalAlignment.Stretch;
            _border.Margin = new Thickness(10);
            _border.Child = _mainGrid;
            _border.BorderBrush = System.Windows.Media.Brushes.Transparent;
            _border.Background = System.Windows.Media.Brushes.Transparent;
            _border.BorderThickness = new Thickness(1);

        }


        private double prevWidth;
        public void HideImageUI() {
            prevWidth = _page.MainGrid.ColumnDefinitions[0].ActualWidth;
            _page.MainGrid.ColumnDefinitions[0].Width = new GridLength(0);
        }
        public void ShowImageUI() {
            _page.MainGrid.ColumnDefinitions[0].Width = new GridLength(prevWidth);
        }
    }
}
