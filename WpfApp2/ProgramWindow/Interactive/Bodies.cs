using System.Windows;
using System.Windows.Controls;
using Utillities.Wpf;

using Retros.ProgramWindow.DisplaySystem;
using Retros.ProgramWindow.Filters;

// Bodies Manage the Tab body UIElements and functionality
namespace Retros.ProgramWindow.Interactive.Tabs.Bodies {
    public abstract class Body {
        protected Grid mainGrid = new();
        protected Border border = new();
        public FrameworkElement FrameworkElement => border;

        public Body() {
            UIManager.ColorThemeManager.Set_BC1(newBrush => border.BorderBrush = newBrush);
            border.Child = mainGrid;
            border.BorderThickness = new Thickness(1);
        }

        public void Hide() => mainGrid.Visibility = Visibility.Collapsed;
        public void Show() => mainGrid.Visibility = Visibility.Visible;
    }


    public partial class ImageFilter : Body {
        WorkstationImage image = WindowManager.MainWindow!.Workstation.ImageElement; // TODO
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

            UIManager.ColorThemeManager.Set_AC1(b => resetButton.Background = b);
            resetButton.Click += ResetButton_Click;
            resetButton.Content = "Back to original";

            grayscaleSlider.SliderElement.ValueChanged += (s, e) => AddFilterChange(new GrayScale(image), grayscaleSlider.SliderElement.Value / 10);
            blueChannelSlider.SliderElement.ValueChanged += (s, e) => AddFilterChange(new OnlyBlueChannel(image), blueChannelSlider.SliderElement.Value / 10);
            redChannelSlider.SliderElement.ValueChanged += (s, e) => AddFilterChange(new OnlyRedChannel(image), redChannelSlider.SliderElement.Value / 10);
            greenChannelSlider.SliderElement.ValueChanged += (s, e) => AddFilterChange(new OnlyGreenChannel(image), greenChannelSlider.SliderElement.Value / 10); 
            noRedSlider.SliderElement.ValueChanged += (s, e) => AddFilterChange(new NoRedChannel(image), noRedSlider.SliderElement.Value / 10);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e) {
            image.History.Clear();
            image.GetFilterManager.Clear();
            image.CurrentImage.Source = image.Original.Source.Clone();
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
