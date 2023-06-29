using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Utillities.Wpf;

namespace Retros.CustomUI
{
    public class Slider : IFrameworkElement
    {
        private bool _showMinimum = false;
        private bool _showMaximum = false;
        private bool _showValue = false;

        public TextBox ValueText { get; } = new();
        public TextBox MinText { get; } = new();
        public TextBox MaxText { get; } = new();
        public System.Windows.Controls.Slider SliderElement { get; }
        public TextBlock TextElement { get; }
        public StackPanel StackPanelElement { get; }
        public Grid NameGrid { get; } = new();
        public Grid SliderGrid { get; } = new();
        public Border BorderElement { get; }
        public bool ShowMinimum
        {
            get => _showMinimum;
            set
            {
                _showMinimum = value;
                if (value is true)
                {
                    MinText.Visibility = Visibility.Visible;
                }
                else
                {
                    MinText.Visibility = Visibility.Collapsed;
                }
            }
        }
        public bool ShowMaximum
        {
            get => _showMaximum;
            set
            {
                _showMaximum = value;
                if (value is true)
                {
                    MaxText.Visibility = Visibility.Visible;
                }
                else
                {
                    MaxText.Visibility = Visibility.Collapsed;
                }
            }
        }
        public bool ShowValue
        {
            get => _showValue;
            set
            {
                _showValue = value;
                if (value is true)
                {
                    ValueText.Visibility = Visibility.Visible;
                }
                else
                {
                    ValueText.Visibility = Visibility.Collapsed;
                }
            }
        }

        public FrameworkElement FrameworkElement => BorderElement;


        public Slider(string name)
        {
            SliderElement = new();
            SliderElement.Minimum = 0;
            SliderElement.Maximum = 1;
            SliderElement.LargeChange = 0;

            TextElement = new();
            TextElement.Text = name;
            UIManager.ColorThemeManager.SetStyle(TextElement, NameTextStyle);

            UIManager.ColorThemeManager.SetStyle(MinText, ThreshholdTextStyle);
            UIManager.ColorThemeManager.SetStyle(MaxText, ThreshholdTextStyle);
            MinText.Visibility = Visibility.Collapsed;
            MaxText.Visibility = Visibility.Collapsed;
            MinText.Text = 0.ToString();
            MaxText.Text = 1.ToString();
            DependencyPropertyDescriptor.FromProperty(System.Windows.Controls.Slider.MinimumProperty, typeof(System.Windows.Controls.Slider))
                .AddValueChanged(SliderElement, (s, e) =>
            {
                MinText.Text = Math.Round(SliderElement.Minimum, 2).ToString();
            });
            MinText.TextChanged += (s, e) =>
            {
                try
                {
                    SliderElement.Minimum = Double.Parse(MinText.Text);
                }
                catch { }
            };
            DependencyPropertyDescriptor.FromProperty(System.Windows.Controls.Slider.MaximumProperty, typeof(System.Windows.Controls.Slider))
                .AddValueChanged(SliderElement, (s, e) =>
            {
                MaxText.Text = Math.Round(SliderElement.Maximum, 2).ToString();
            });
            MaxText.TextChanged += (s, e) =>
            {
                try
                {
                    SliderElement.Maximum = Double.Parse(MaxText.Text);
                }
                catch { }
            };


            StackPanelElement = new();
            StackPanelElement.Orientation = Orientation.Vertical;
            StackPanelElement.Children.Add(NameGrid);
            StackPanelElement.Children.Add(SliderGrid);

            Helper.AddColumn(SliderGrid, 1, GridUnitType.Auto);
            Helper.AddColumn(SliderGrid, 1, GridUnitType.Star);
            Helper.AddColumn(SliderGrid, 1, GridUnitType.Auto);
            Helper.SetChildInGrid(SliderGrid, MinText, 0, 0);
            Helper.SetChildInGrid(SliderGrid, SliderElement, 0, 1);
            Helper.SetChildInGrid(SliderGrid, MaxText, 0, 2);
                        
            Helper.AddColumn(NameGrid, 1, GridUnitType.Auto);
            Helper.AddColumn(NameGrid, 1, GridUnitType.Auto);
            Helper.SetChildInGrid(NameGrid, TextElement, 0, 0);
            Helper.SetChildInGrid(NameGrid, ValueText, 0, 1);


            UIManager.ColorThemeManager.SetStyle(ValueText, ValueTextStyle);
            SliderElement.ValueChanged += (s, e) =>
            {
                ValueText.Text = Math.Round(e.NewValue, 5).ToString();
            };
            ValueText.TextChanged += (s, e) =>
            {
                try
                {
                    SliderElement.Value = Double.Parse(ValueText.Text);
                }
                catch { }
            };


            BorderElement = new Border { Child = StackPanelElement };
        }

        public void BindMinToMax(Slider other)
        {
            Binding binding = new Binding();
            binding.Source = other.SliderElement.Maximum;
            binding.Mode = BindingMode.OneWay;
            BindingOperations.SetBinding(this.SliderElement, System.Windows.Controls.Slider.MinimumProperty, binding);
        }
        public void BindMinToValue(Slider other)
        {
            other.SliderElement.ValueChanged += (s, e) =>
            {
                SliderElement.Minimum = e.NewValue;
            };
        }
        public void BindMaxToMin(Slider other)
        {
            Binding binding = new Binding();
            binding.Source = other.SliderElement.Minimum;
            binding.Mode = BindingMode.OneWay;
            BindingOperations.SetBinding(this.SliderElement, System.Windows.Controls.Slider.MaximumProperty, binding);
        }
        public void BindMaxToValue(Slider other)
        {
            other.SliderElement.ValueChanged += (s, e) =>
            {
                SliderElement.Maximum = e.NewValue;
            };
        }

        public static Style NameTextStyle()
        {
            Style style = new Style(typeof(TextBlock));
            style.Setters.Add(new Setter(TextBlock.ForegroundProperty, UIManager.ColorThemeManager.Current.FC1));
            style.Setters.Add(new Setter(TextBlock.FontWeightProperty, FontWeights.Bold));
            return style;
        }
        public static Style ThreshholdTextStyle()
        {
            var style = new Style(typeof(TextBox));
            style.Setters.Add(new Setter(TextBox.BackgroundProperty, UIManager.ColorThemeManager.Current.BG6));
            style.Setters.Add(new Setter(TextBox.ForegroundProperty, UIManager.ColorThemeManager.Current.FC1));
            style.Setters.Add(new Setter(TextBox.FontWeightProperty, FontWeights.Light));
            return style;
        }
        public static Style ValueTextStyle()
        {
            var style = new Style(typeof(TextBox));
            style.Setters.Add(new Setter(TextBox.MarginProperty, new Thickness(5, 0, 0, 0)));
            style.Setters.Add(new Setter(TextBox.BackgroundProperty, UIManager.ColorThemeManager.Current.BG6));
            style.Setters.Add(new Setter(TextBox.ForegroundProperty, UIManager.ColorThemeManager.Current.FC1));
            style.Setters.Add(new Setter(TextBox.FontWeightProperty, FontWeights.Bold));
            return style;

        }
    }
}
