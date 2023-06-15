﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace CustomControlLibrary {
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:CustomControlLibrary"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:CustomControlLibrary;assembly=CustomControlLibrary"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class SelectionBox : Control {
        //toggle Button
        public Brush BorderBrushOnHover {
            get => (Brush)GetValue(BorderBrushOnHoverProperty);
            set => SetValue(BorderBrushOnHoverProperty, value);
        }
        public static readonly DependencyProperty BorderBrushOnHoverProperty =
            DependencyProperty.Register("BorderBrushOnHover", typeof(Brush), typeof(SelectionBox), new PropertyMetadata(Brushes.Tomato));

        //Option Box
        public Brush OptionBoxBorderBrush {
            get => (Brush)GetValue(OptionBoxBorderBrushProperty);
            set => SetValue(OptionBoxBorderBrushProperty, value);
        }
        public static readonly DependencyProperty OptionBoxBorderBrushProperty =
            DependencyProperty.Register("OptionBoxBorderBrush", typeof(Brush), typeof(SelectionBox), new PropertyMetadata(Brushes.Transparent));

        public Thickness OptionBoxBorderThickness {
            get => (Thickness)GetValue(OptionBoxBorderThicknessProperty);
            set => SetValue(OptionBoxBorderThicknessProperty, value);
        }
        public static readonly DependencyProperty OptionBoxBorderThicknessProperty =
            DependencyProperty.Register("OptionBoxBorderThickness", typeof(Thickness), typeof(SelectionBox), new PropertyMetadata(new Thickness(1)));

        //Options
        public Brush OptionsBackground {
            get => (Brush)GetValue(OptionsBackgroundProperty);
            set {
                SetValue(OptionsBackgroundProperty, value);
                foreach (Border option in _options!.Children) {
                    option.Background = value;
                }
            }
        }
        public static readonly DependencyProperty OptionsBackgroundOnHoverProperty =
            DependencyProperty.Register("OptionsBackgroundOnHover", typeof(Brush), typeof(SelectionBox), new PropertyMetadata(Brushes.CornflowerBlue));

        public Brush OptionsBackgroundOnHover {
            get => (Brush)GetValue(OptionsBackgroundOnHoverProperty);
            set => SetValue(OptionsBackgroundOnHoverProperty, value);
        }
        public static readonly DependencyProperty OptionsBackgroundProperty =
            DependencyProperty.Register("OptionsBackground", typeof(Brush), typeof(SelectionBox), new PropertyMetadata(Brushes.CornflowerBlue));

        public Brush OptionsBorderBrush {
            get => (Brush)GetValue(OptionsBorderBrushProperty);
            set {
                SetValue(OptionsBorderBrushProperty, value);
                foreach (Border option in _options!.Children) {
                    option.BorderBrush = value;
                }
            }
        }
        public static readonly DependencyProperty OptionsBorderBrushProperty =
            DependencyProperty.Register("OptionsBorderBrush", typeof(Brush), typeof(SelectionBox), new PropertyMetadata(Brushes.Transparent));

        public Brush OptionsBorderBrushOnHover {
            get => (Brush)GetValue(OptionsBorderBrushPropertyOnHover);
            set => SetValue(OptionsBorderBrushPropertyOnHover, value);
        }
        public static readonly DependencyProperty OptionsBorderBrushPropertyOnHover =
            DependencyProperty.Register("OptionsBorderBrushOnHover", typeof(Brush), typeof(SelectionBox), new PropertyMetadata(Brushes.Transparent));

        public Thickness OptionsBorderThickness {
            get => (Thickness)GetValue(OptionsBorderThicknessProperty);
            set {
                SetValue(OptionsBorderThicknessProperty, value);
                foreach (Border option in _options!.Children) {
                    option.BorderThickness = value;
                }
            }
        }
        public static readonly DependencyProperty OptionsBorderThicknessProperty =
            DependencyProperty.Register("OptionsBorderThickness", typeof(Thickness), typeof(SelectionBox), new PropertyMetadata(new Thickness(1)));

        public Brush OptionsTextBrush {
            get => (Brush)GetValue(OptionsTextBrushProperty);
            set {
                SetValue(OptionsTextBrushProperty, value);
                foreach (Border option in _options!.Children) {
                    (option.Child as TextBlock)!.Foreground = value;
                }
            }
        }
        public static readonly DependencyProperty OptionsTextBrushProperty =
            DependencyProperty.Register("OptionsTextBrush", typeof(Brush), typeof(SelectionBox), new PropertyMetadata(Brushes.White));


        private Dictionary<DependencyProperty, object> Properties = new();
        private Dictionary<DependencyProperty, object> GetProperties() {
            AddProperty(BorderBrushOnHoverProperty, BorderBrushOnHover);
            AddProperty(OptionBoxBorderBrushProperty, OptionBoxBorderBrush);
            AddProperty(OptionBoxBorderThicknessProperty, OptionBoxBorderThickness);
            AddProperty(OptionsBackgroundProperty, OptionsBackground);
            AddProperty(OptionsBorderBrushProperty, OptionsBorderBrush);
            AddProperty(OptionsBorderThicknessProperty, OptionsBorderThickness);
            AddProperty(OptionsTextBrushProperty, OptionsTextBrush);
            return Properties;
        }
        private void AddProperty(DependencyProperty key, object value) {
            if (!Properties.ContainsKey(key)) {
                Properties.Add(key, value);
            }
            else if (Properties[key] == value) {
                Properties[key] = value;
            }
        }


        public new Style Style {
            get => base.Style;
            set {
                if (!IsLoaded) {
                    _doAfterLoadedActions.Enqueue(() => {
                        _setStyle(value);
                    });
                }
                else {
                    _setStyle(value);
                }
            }
        }
        private void _setStyle(Style value) {
            base.Style = value;
            GetProperties();
            BorderBrushOnHover = (Brush)GetPropertyValue(value, BorderBrushOnHoverProperty)!;
            OptionBoxBorderBrush = (Brush)GetPropertyValue(value, OptionBoxBorderBrushProperty)!;
            OptionBoxBorderThickness = (Thickness)GetPropertyValue(value, OptionBoxBorderThicknessProperty)!;
            OptionsBackground = (Brush)GetPropertyValue(value, OptionsBackgroundProperty)!;
            OptionsBorderBrush = (Brush)GetPropertyValue(value, OptionsBorderBrushProperty)!;
            OptionsBorderThickness = (Thickness)GetPropertyValue(value, OptionsBorderThicknessProperty)!;
            OptionsTextBrush = (Brush)GetPropertyValue(value, OptionsTextBrushProperty)!;
        }

        private Popup? _popup;
        private Border? _toggleRegion;
        private TextBlock? _textText;
        private TextBlock? _arrowText;
        private bool _isCollapsed = true;
        private HashSet<string> _optionSet = new();
        private StackPanel? _options;

        private Queue<Action> _doAfterLoadedActions = new();
        

        public bool IsCollapsed => _isCollapsed;
        public string? Selected => _textText!.Text;


        static SelectionBox() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SelectionBox), new FrameworkPropertyMetadata(typeof(SelectionBox)));
        }

        private object? GetPropertyValue(Style style, DependencyProperty property) {
            bool hasProperty = false;
            foreach (Setter setter in style.Setters) {
                if (setter.Property == property) {
                    hasProperty = true;
                }
            }
            if (!hasProperty) {
                // Target property not found in the style
                if (property.OwnerType == typeof(SelectionBox)) {
                    // Has the property, but is not defined in style
                    // Return this DependencyProperty
                    return Properties[property];
                }
            }

            return ((Setter)style.Setters.First(
                s => (s as Setter)!.Property == property
            )).Value;
        }

        public override void OnApplyTemplate() {
            base.OnApplyTemplate();


            _toggleRegion = GetTemplateChild("_toggleRegion") as Border;
            if (_toggleRegion != null) {
                _toggleRegion.PreviewMouseDown += (s, e) => Toggle();
                _toggleRegion.MouseEnter += (s, e) => SetToggleButtonBorderBrush(true);
                _toggleRegion.MouseLeave += (s, e) => SetToggleButtonBorderBrush(false);
            }

            _popup = GetTemplateChild("_popup") as Popup;

            _textText = GetTemplateChild("_textText") as TextBlock;

            _arrowText = GetTemplateChild("_arrowText") as TextBlock;

            _options = GetTemplateChild("_options") as StackPanel;


            _doAfterLoadedActions.Enqueue(() => {
                Window window = Window.GetWindow(_toggleRegion);
                window.LocationChanged += (s, e) => UpadteOptionsPosition();
                window.Deactivated += delegate {
                    if (!IsCollapsed) {
                        Toggle();
                    }
                };
            });

            Loaded += delegate {
                while (_doAfterLoadedActions.Count > 0) {
                    _doAfterLoadedActions.Dequeue().Invoke();
                }
            };
        }

        public void UpadteOptionsPosition() {
            if (_popup is null) {
                return;
            }
            _popup.HorizontalOffset = 1;
            _popup.HorizontalOffset = 0;
            _popup.VerticalOffset = 0;
        }

        public void Toggle() {
            if (_arrowText is null
                || _popup is null) return;


            if (_isCollapsed) {
                _isCollapsed = false;
                _arrowText.Text = " ⮟";
                _popup.IsOpen = true;
            }
            else {
                _isCollapsed = true;
                _arrowText.Text = " ⮝";
                _popup.IsOpen = false;
            }
        }

        public void ChooseOption(string option) {
            if (_textText is null ||
                !_optionSet.Contains(option)) return;
            _textText.Text = option;

            Toggle();
        }

        public void AddOption(string newOption) {
            if (!IsLoaded) {
                _doAfterLoadedActions.Enqueue(() => AddOption(newOption));
                return;
            }

            if (_optionSet.Contains(newOption)
                || _options == null) return;

            _optionSet.Add(newOption);
            var newButton = GetNewOptionButton(newOption);
            newButton.PreviewMouseDown += (s, e) => ChooseOption(newOption);
            _options.Children.Add(newButton);
        }

        private Border GetNewOptionButton(string name) {
            Border border = new Border {
                Background = Background,
                BorderBrush = OptionsBorderBrush,
                BorderThickness = OptionsBorderThickness,
            };
            TextBlock text = new TextBlock {
                Text = name,
                Foreground = OptionsTextBrush
            };

            border.MouseEnter += delegate {
                border.Background = OptionsBackgroundOnHover;
                border.BorderBrush = OptionsBorderBrushOnHover;
            };
            border.MouseLeave += delegate {
                border.Background = OptionsBackground;
                border.BorderBrush = OptionsBorderBrush;
            };

            border.Child = text;
            return border;
        }

        public void RemoveOption(string option) {
            if (!_optionSet.Contains(option)) return;

            _optionSet.Remove(option);
            Button? optionButton = null;
            foreach (Button child in _options!.Children) {
                if (child.Content.ToString() == option) {
                    optionButton = child;
                    break;
                }
            }
            _options.Children.Remove(optionButton);
        }


        private void SetToggleButtonBorderBrush(bool isHover) {
            if (_toggleRegion is null) return;

            if (isHover) {
                _toggleRegion.BorderBrush = BorderBrushOnHover;
            }
            else {
                _toggleRegion.BorderBrush = BorderBrush;
            }
        }
    }




    public class TextField : Control {
        static TextField() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextField), new FrameworkPropertyMetadata(typeof(TextField)));
        }
    }
}