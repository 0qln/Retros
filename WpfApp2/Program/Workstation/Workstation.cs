﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Retros.Program.Workstation.TabUI.Tabs;
using Retros.Program.Workstation.TabUI;
using Retros.Program.Workstation.Image;
using System.Windows.Media;

namespace Retros.Program.Workstation {
    public partial class Workstation {
        // UI
        private Grid _mainGrid = new();
        private Border _border = new();
        private WorkstationPage _page;
        private Frame _pageFrame = new();

        public Grid MainGrid => _mainGrid;
        public FrameworkElement FrameworkElement => _pageFrame;

        // System
        public WorkstationImage ImageElement { get; } = new();
        public WorkstationTabPresenter TableElement { get; } = new();

        public readonly static string DefaultPath =
            @"";


        public Workstation(double topPadding) {
            // Table
            TableElement.AddTab(new ImageFilterTab(new ImageFilterBody(ImageElement), new DefaultHandle("Filters")));
            TableElement.AddTab(new ImageHistoryTab(new ImageHistoryBody(ImageElement), new DefaultHandle("Change History")));
            TableElement.AddTab(new PixelSortingTab(new PixelSorting(ImageElement), new DefaultHandle("Pixel Sorting")));
            TableElement.SelectTab(0);

            // Image
            ImageElement.Page.WindowInitiated += (window) => {
                window.Closed += (_, _) => ShowImageUI();
            };

            // Page
            _page = new WorkstationPage(ImageElement, TableElement, topPadding);
            _pageFrame.Content = _page;

            // Border
            _border.VerticalAlignment = VerticalAlignment.Stretch;
            _border.HorizontalAlignment = HorizontalAlignment.Stretch;
            _border.Margin = new Thickness(10);
            _border.Child = _mainGrid;
            _border.BorderBrush = Brushes.Transparent;
            _border.Background = Brushes.Transparent;
            _border.BorderThickness = new Thickness(1);

        }


        private double prevWidth;
        public void HideImageUI() {
            prevWidth = _page.MainGrid.ColumnDefinitions[0].ActualWidth;
            _page.MainGrid.ColumnDefinitions[0].Width = new GridLength(0);
        }
        public void ShowImageUI() {
            _page.MainGrid.ColumnDefinitions[0].Width = new GridLength(prevWidth);
            ImageElement.FrameworkElement.Visibility = Visibility.Visible;
        }
    }
}
