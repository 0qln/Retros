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

// Bodies Manage the Tab body UIElements and functionality
namespace Retros.WorkstationTableElements.Bodies {

    public partial class ImageFilter : Body {
        Workstation.WorkstationImage image = UIManager.Workstation.ImageElement; // TODO
        private StackPanel stackPanel = new();

        public Button blueChannelButton = new();
        public Button redChannelButton = new();
        public Button greenChannelButton = new();
        public Button grayScaleButton = new();
        public Slider slider = new();


        public ImageFilter() {
            Helper.SetChildInGrid(mainGrid, stackPanel, 0, 0);
            stackPanel.Children.Add(slider);
            stackPanel.Children.Add(grayScaleButton);
            stackPanel.Children.Add(blueChannelButton);
            stackPanel.Children.Add(redChannelButton);
            stackPanel.Children.Add(greenChannelButton);

            slider.ValueChanged += (s, e) => Slider_ValueChanged();

            blueChannelButton.Background = System.Windows.Media.Brushes.Transparent;
            blueChannelButton.Click += BlueChannelButton_Click;
            blueChannelButton.Content = "blueChannelButton";

            redChannelButton.Background = System.Windows.Media.Brushes.Transparent;
            redChannelButton.Click += RedChannelButton_Click;
            redChannelButton.Content = "redChannelButton";

            greenChannelButton.Background = System.Windows.Media.Brushes.Transparent;
            greenChannelButton.Click += GreenChannelButton_Click;
            greenChannelButton.Content = "greenChannelButton";

            grayScaleButton.Background = System.Windows.Media.Brushes.Transparent;
            grayScaleButton.Click += GrayScaleButton_Click;
            grayScaleButton.Content = "GrayScale";
        }

        private void GreenChannelButton_Click(object sender, RoutedEventArgs e) {
            var change = new GreenChannel(UIManager.Workstation.ImageElement, 1);
            image.GetFilterManager.AddChange(change);
            UIManager.Workstation.ImageElement.History.Add(change);
        }

        private void RedChannelButton_Click(object sender, RoutedEventArgs e) {
            var change = new RedChannel(UIManager.Workstation.ImageElement, 1);
            image.GetFilterManager.AddChange(change);
            UIManager.Workstation.ImageElement.History.Add(change);
        }

        private void BlueChannelButton_Click(object sender, RoutedEventArgs e) {
            var change = new BlueChannel(UIManager.Workstation.ImageElement, 1);
            image.GetFilterManager.AddChange(change);
            UIManager.Workstation.ImageElement.History.Add(change);
        }

        private void Slider_ValueChanged() {
            GrayScale grayScale = new(image, slider.Value / 10);
            image.GetFilterManager.AddChange(grayScale);
            UIManager.Workstation.ImageElement.History.Add(grayScale);
        }

        private void GrayScaleButton_Click(object sender, RoutedEventArgs e) {
            var change = new GrayScale(UIManager.Workstation.ImageElement, 1);
            image.GetFilterManager.AddChange(change);
            UIManager.Workstation.ImageElement.History.Add(change);
        }
    }


    public class PixelSorting : Body {

        public PixelSorting() {
            TextBlock textBlock = new TextBlock();
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
