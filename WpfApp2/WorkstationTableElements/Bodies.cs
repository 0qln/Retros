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
        public ImageEditor imageEditor = new();
        private StackPanel stackPanel = new();

        public Button grayScaleButton = new();
        public Slider slider = new();


        public ImageFilter() {
            Helper.SetChildInGrid(mainGrid, stackPanel, 0, 0);
            stackPanel.Children.Add(slider);
            stackPanel.Children.Add(grayScaleButton);

            slider.ValueChanged += (s, e) => Slider_ValueChanged();

            grayScaleButton.Background = System.Windows.Media.Brushes.Transparent;
            grayScaleButton.Click += GrayScaleButton_Click;
            grayScaleButton.Content = "GrayScale";
        }


        private void Slider_ValueChanged() {
            GrayScale grayScale = new(UIManager.Workstation.ImageElement, slider.Value / 10);
            imageEditor.AddChange(grayScale);
            UIManager.Workstation.ImageElement.History.Add(grayScale);
            imageEditor.ApplyChange();
            ///UIManager.Workstation.ImageElement.InterpolationSmoothness = (int)slider.Value;
        }

        private void GrayScaleButton_Click(object sender, RoutedEventArgs e) {
            imageEditor.AddChange(new GrayScale(UIManager.Workstation.ImageElement, 1));
            imageEditor.ApplyChange();
            
            //UIManager.Workstation.ImageElement.History.Add(grayScale);
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
