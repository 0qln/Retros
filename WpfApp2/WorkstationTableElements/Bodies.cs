﻿using System.Windows;
using System.Windows.Controls;
using WpfUtillities;
using static Retros.WorkstationTableElements.Tabs.ImageFilterTab;

// Bodies Manage the Tab body UIElements and functionality
namespace Retros.WorkstationTableElements.Bodies {

    public partial class ImageFilter : Body {
        Workstation.WorkstationImage image = UIManager.Workstation.ImageElement; // TODO
        private StackPanel stackPanel = new();

        public Button resetButton = new();

        public Slider blueChannelSlider = new("Blue Channel");
        public Slider redChannelSlider = new("Red Channel");
        public Slider greenChannelSlider = new("Green Channel");
        public Slider grayscaleSlider = new("Grayscale");
        public Slider noRedSlider = new("No Red Chnnel");


        public ImageFilter() {
            Helper.SetChildInGrid(mainGrid, stackPanel, 0, 0);
            stackPanel.Children.Add(resetButton);
            stackPanel.Children.Add(grayscaleSlider.FrameworkElement);
            stackPanel.Children.Add(blueChannelSlider.FrameworkElement);
            stackPanel.Children.Add(redChannelSlider.FrameworkElement);
            stackPanel.Children.Add(greenChannelSlider.FrameworkElement);
            stackPanel.Children.Add(noRedSlider.FrameworkElement);

            resetButton.Background = System.Windows.Media.Brushes.Tomato;
            resetButton.Click += ResetButton_Click;
            resetButton.Content = "Back to original";

            //grayscale
            grayscaleSlider.SliderElement.ValueChanged += GrayScaleSliderElement_ValueChanged;

            //blue
            blueChannelSlider.SliderElement.ValueChanged += BlueChannelButton_Click;

            //red
            redChannelSlider.SliderElement.ValueChanged += RedChannelButton_Click;

            //green
            greenChannelSlider.SliderElement.ValueChanged += GreenChannelButton_Click;

            //no red
            ///noRedSlider.SliderElement.ValueChanged += (s, e) => AddChange(new NoRedChannel(image, noRedSlider.SliderElement.Value / 10));
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e) {
            image.History.Clear();
            image.GetFilterManager.Clear();
            image.CurrentImage.Source = image.Original.Source.Clone();
        }

        private void GrayScaleSliderElement_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            AddFilterChange(new GrayScale(image), grayscaleSlider.SliderElement.Value / 10);
        }

        private void GreenChannelButton_Click(object sender, RoutedEventArgs e) {
            AddFilterChange(new OnlyGreenChannel(image), greenChannelSlider.SliderElement.Value / 10);
        }

        private void RedChannelButton_Click(object sender, RoutedEventArgs e) {
            AddFilterChange(new OnlyRedChannel(image), redChannelSlider.SliderElement.Value / 10);
        }

        private void BlueChannelButton_Click(object sender, RoutedEventArgs e) {
            AddFilterChange(new OnlyBlueChannel(image), blueChannelSlider.SliderElement.Value / 10);
        }

        private void AddChange(IChange change) {

        }

        private void AddFilterChange(IFilterChange filter, double value) {
            if (value == 0) {
                image.GetFilterManager.RemoveChange((IChange) filter);
            }
            else if (!image.GetFilterManager.AddChange(filter)) {
                image.GetFilterManager.SetFilterIntensity(filter, value);
            }
        }
    }


    public class PixelSorting : Body {

        public PixelSorting() {
            TextBlock textBlock = new();
            textBlock.Text = "PixelSorting";
            Helper.SetChildInGrid(mainGrid, textBlock, 0, 0);
        }

    }


    public class Export : Body {

        public Export() {

        }
    }



    public class Import : Body {

        public Import() {

        }
    }


}
