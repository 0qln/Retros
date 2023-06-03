﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Utillities.Wpf;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Documents;

namespace Retros {
    internal class SelectionBox<T> : IFrameworkElement where T : FrameworkElement {
        public FrameworkElement FrameworkElement => _mainGrid;
        public bool IsCollapsed => _isCollapsed;
        public double Height { get; set; } = 20;

        private ToggleDisplay _toggleDisplay;
        private Grid _mainGrid = new();
        private StackPanel _options = new();
        private Canvas _optionsContainer = new();
        private bool _isCollapsed = true;

        public SelectionBox() {
            _toggleDisplay = new(this);

            _options.Visibility = Visibility.Collapsed;
            _isCollapsed = false;

            Helper.AddRow(_mainGrid, Height, GridUnitType.Pixel);
            Helper.AddRow(_mainGrid, 1, GridUnitType.Auto);

            Helper.SetChildInGrid(_mainGrid, _toggleDisplay.FrameworkElement, 0, 0);
            Helper.SetChildInGrid(_mainGrid, _optionsContainer, 1, 0);

            _options.Children.Add(new TextBlock { Text = "option 1" });
            _options.Children.Add(new TextBlock { Text = "option 2" });
            _options.Background = Brushes.White;

            _optionsContainer.Children.Add(_options);
            _optionsContainer.Height = 0;
            _optionsContainer.Width = 0;
            _mainGrid.ClipToBounds = false;
            _optionsContainer.ClipToBounds = false;
            _options.ClipToBounds = false;
        }

        private void _tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            
        }
        public void AddOption(T newOption) {
            _options.Children.Add(newOption);
        }
        public void RemoveOption(T option) {
            throw new NotImplementedException();
        }


        public void Toggle() {
            if (IsCollapsed) {
                _options.Visibility = Visibility.Collapsed;
                _isCollapsed = false;
            }
            else {
                _options.Visibility = Visibility.Visible;
                _isCollapsed = true;
            }
        }



        private class ToggleDisplay : IFrameworkElement {
            public FrameworkElement FrameworkElement => _mainGrid;

            private Grid _mainGrid = new();
            private Button _textButton = new();
            private Button _arrowButton = new();
            private SelectionBox<T> _parent;

            Popup codePopup;

            public ToggleDisplay(SelectionBox<T> parent) {
                _parent = parent;
                Helper.AddColumn(_mainGrid, 1, GridUnitType.Auto);
                Helper.AddColumn(_mainGrid, parent.Height /*arrow button is square*/, GridUnitType.Pixel);
                Helper.SetChildInGrid(_mainGrid, _textButton, 0, 0);
                Helper.SetChildInGrid(_mainGrid, _arrowButton, 0, 1);

                _textButton.MinWidth = 20;

                _arrowButton.Content = "⮝";

                _arrowButton.Click += (s, e) => Toggle();
                _textButton.Click += (s, e) => Toggle();
            }

            private void Toggle() {
                DebugLibrary.Console.Log($"Collapsed: {_parent.IsCollapsed}");

                _parent.Toggle();

                if (_parent.IsCollapsed) {
                    _arrowButton.Content = "⮟";
                }
                else {
                    _arrowButton.Content = "⮝";
                }
            }
        }
    }
}
