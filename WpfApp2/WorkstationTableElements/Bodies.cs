using Retros.WorkstationTableElements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Retros.WorkstationTableElements.Bodies.ImageFilter;
using static Retros.WorkstationTableElements.Tabs.ImageFilterTab;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows;
using WpfCustomControls;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

// Bodies Manage the Tab body UIElements and functionality
namespace Retros.WorkstationTableElements.Bodies {

    public partial class ImageFilter : Body {
        Workstation.WorkstationImage image = UIManager.Workstation.ImageElement; // TODO
        private StackPanel stackPanel = new();

        public Button originalButton = new();

        public Slider blueChannelSlider = new("Blue Channel");
        public Slider redChannelSlider = new("Red Channel");
        public Slider greenChannelSlider = new("Green Channel");
        public Slider grayscaleSlider = new("Grayscale");


        public ImageFilter() {
            Helper.SetChildInGrid(mainGrid, stackPanel, 0, 0);
            stackPanel.Children.Add(originalButton);
            stackPanel.Children.Add(grayscaleSlider.FrameworkElement);
            stackPanel.Children.Add(blueChannelSlider.FrameworkElement);
            stackPanel.Children.Add(redChannelSlider.FrameworkElement);
            stackPanel.Children.Add(greenChannelSlider.FrameworkElement);

            originalButton.Background = System.Windows.Media.Brushes.Tomato;
            originalButton.Click += OriginalButton_Click;
            originalButton.Content = "Back to original";

            //grayscale
            grayscaleSlider.SliderElement.ValueChanged += SliderElement_ValueChanged; ;

            //blue
            blueChannelSlider.BorderElement.Background = System.Windows.Media.Brushes.Transparent;
            blueChannelSlider.SliderElement.ValueChanged += BlueChannelButton_Click;

            //red
            redChannelSlider.BorderElement.Background = System.Windows.Media.Brushes.Transparent;
            redChannelSlider.SliderElement.ValueChanged += RedChannelButton_Click;

            //green
            greenChannelSlider.BorderElement.Background = System.Windows.Media.Brushes.Transparent;
            greenChannelSlider.SliderElement.ValueChanged += GreenChannelButton_Click;
        }

        private void SliderElement_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            var change = new GrayScale(image, grayscaleSlider.SliderElement.Value / 10);
            image.GetFilterManager.AddChange(change);
            image.History.Add(change);
        }

        private void OriginalButton_Click(object sender, RoutedEventArgs e) {
            image.History.Clear();
            image.GetFilterManager.Clear();
            image.CurrentImage.Source = image.Original.Source.Clone();
        }

        private void GreenChannelButton_Click(object sender, RoutedEventArgs e) {
            var change = new OnlyGreenChannel(image, greenChannelSlider.SliderElement.Value / 10);
            image.GetFilterManager.AddChange(change);
            image.History.Add(change);
        }

        private void RedChannelButton_Click(object sender, RoutedEventArgs e) {
            var change = new OnlyRedChannel(image, redChannelSlider.SliderElement.Value / 10);
            image.GetFilterManager.AddChange(change);
            image.History.Add(change);
        }

        private void BlueChannelButton_Click(object sender, RoutedEventArgs e) {
            var change = new OnlyBlueChannel(image, blueChannelSlider.SliderElement.Value / 10);
            image.GetFilterManager.AddChange(change);
            image.History.Add(change);
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
