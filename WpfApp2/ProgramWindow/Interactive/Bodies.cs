using System.Windows;
using System.Windows.Controls;
using Utillities.Wpf;

using Retros.ProgramWindow.DisplaySystem;
using Retros.ProgramWindow.Filters;
using System;
using System.Threading.Tasks;

// Bodies Manage the Tab body UIElements and functionality
namespace Retros.ProgramWindow.Interactive.Tabs.Bodies {
    public abstract class Body {
        protected StackPanel stackPanel = new();
        protected Grid mainGrid = new();
        protected Border border = new();
        public FrameworkElement FrameworkElement => border;

        public Body() {
            UIManager.ColorThemeManager.Set_BC1(newBrush => border.BorderBrush = newBrush);
            border.Child = mainGrid;
            border.BorderThickness = new Thickness(1);
            Helper.SetChildInGrid(mainGrid, stackPanel, 0, 0);
        }

        public void Hide() => mainGrid.Visibility = Visibility.Collapsed;
        public void Show() => mainGrid.Visibility = Visibility.Visible;
    }


    public class ImageFilter : Body {
        private readonly WorkstationImage image = WindowManager.MainWindow!.Workstation.ImageElement;

        public Button resetButton = new();

        public Slider blueChannelSlider = new("Blue Channel");
        public Slider redChannelSlider = new("Red Channel");
        public Slider greenChannelSlider = new("Green Channel");
        public Slider grayscaleSlider = new("Grayscale");
        public Slider testBlueSlider = new("TestBlue");
        public FilterDisplay FilterDisplay = new();

        public ImageFilter() {
            stackPanel.Children.Add(resetButton);
            stackPanel.Children.Add(grayscaleSlider.FrameworkElement);
            stackPanel.Children.Add(blueChannelSlider.FrameworkElement);
            stackPanel.Children.Add(redChannelSlider.FrameworkElement);
            stackPanel.Children.Add(greenChannelSlider.FrameworkElement);
            stackPanel.Children.Add(testBlueSlider.FrameworkElement);
            stackPanel.Children.Add(FilterDisplay.FrameworkElement);

            UIManager.ColorThemeManager.Set_AC1(b => resetButton.Background = b);
            resetButton.Click += ResetButton_Click;
            resetButton.Content = "Back to original";

            grayscaleSlider.SliderElement.ValueChanged += (s, e) => AddFilterChange(new GrayScale(image), grayscaleSlider.SliderElement.Value / 10);
            blueChannelSlider.SliderElement.ValueChanged += (s, e) => AddFilterChange(new OnlyBlueChannel(image), blueChannelSlider.SliderElement.Value / 10);
            redChannelSlider.SliderElement.ValueChanged += (s, e) => AddFilterChange(new OnlyRedChannel(image), redChannelSlider.SliderElement.Value / 10);
            greenChannelSlider.SliderElement.ValueChanged += (s, e) => AddFilterChange(new OnlyGreenChannel(image), greenChannelSlider.SliderElement.Value / 10); 
            testBlueSlider.SliderElement.ValueChanged += (s, e) => AddFilterChange(new TestBlue(image), testBlueSlider.SliderElement.Value / 10);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e) {
            image.History.Clear();
            image.GetFilterManager.Clear();
            image.CurrentImage.Source = image.SourceImage.Source.Clone();
        }

        private void AddFilterChange(IFilterChange filter, double value) {
            if (value == 0) {
                image.GetFilterManager.RemoveChange((IChange) filter);
                FilterDisplay.RemoveItem(filter.GetType().Name);
            }
            else {
                if (image.GetFilterManager.AddFilter(filter)) {
                    FilterDisplay.AddItem(filter.GetType().Name);
                }
                else {
                    image.GetFilterManager.SetFilterIntensity(filter, value);
                }
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


    public class Test : Body {
        private static WorkstationImage image = WindowManager.MainWindow!.Workstation.ImageElement;
        private static float filterIntensity;

        public Test() {
            ///Debugger.Console.ClearAll();

            Button normal = new Button { Content = "Execute normal" };
            ///normal.Click += (s, e) => DebugLibrary.Benchmark.Measure.Execute(Compute);
            
            Button gpu = new Button { Content = "Execute on GPU" };
            ///gpu.Click += (s, e) => DebugLibrary.Benchmark.Measure.Execute(PerformVectorAddition());

            stackPanel.Children.Add(normal);
            stackPanel.Children.Add(gpu);
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
