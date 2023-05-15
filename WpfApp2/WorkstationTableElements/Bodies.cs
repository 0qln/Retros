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

        public Button resetButton = new();

        public Slider blueChannelSlider = new("Blue Channel");
        public Slider redChannelSlider = new("Red Channel");
        public Slider greenChannelSlider = new("Green Channel");
        public Slider grayscaleSlider = new("Grayscale");


        public ImageFilter() {
            Helper.SetChildInGrid(mainGrid, stackPanel, 0, 0);
            stackPanel.Children.Add(resetButton);
            stackPanel.Children.Add(grayscaleSlider.FrameworkElement);
            stackPanel.Children.Add(blueChannelSlider.FrameworkElement);
            stackPanel.Children.Add(redChannelSlider.FrameworkElement);
            stackPanel.Children.Add(greenChannelSlider.FrameworkElement);

            resetButton.Background = System.Windows.Media.Brushes.Tomato;
            resetButton.Click += ResetButton_Click;
            resetButton.Content = "Back to original";

            //grayscale
            grayscaleSlider.SliderElement.ValueChanged += GrayScaleSliderElement_ValueChanged; ;

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

        private void ResetButton_Click(object sender, RoutedEventArgs e) {
            image.History.Clear();
            image.GetFilterManager.Clear();
            image.CurrentImage.Source = image.Original.Source.Clone();
        }

        private void GrayScaleSliderElement_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            AddChange(new GrayScale(image, grayscaleSlider.SliderElement.Value / 10));
        }

        private void GreenChannelButton_Click(object sender, RoutedEventArgs e)
            => AddChange(new OnlyGreenChannel(image, greenChannelSlider.SliderElement.Value / 10));

        private void RedChannelButton_Click(object sender, RoutedEventArgs e)
            => AddChange(new OnlyRedChannel(image, redChannelSlider.SliderElement.Value / 10));

        private void BlueChannelButton_Click(object sender, RoutedEventArgs e)
            => image.InterpolationSmoothness = 10;
            //=> AddChange(new OnlyBlueChannel(image, blueChannelSlider.SliderElement.Value / 10));

        private void AddChange(IChange change) {
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
