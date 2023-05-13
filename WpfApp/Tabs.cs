using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using WpfCustomControls;
using Retros.ImageEditing;
using System.Diagnostics;
using DebugLibrary.Benchmarks;
using Debugger;
using Console = Debugger.Console;
using System.Windows.Threading;

namespace Retros {
    namespace ClientWorkStation {
        namespace Tabs {
            public abstract class Tab {
                protected Handle handle;
                public Handle Handle => handle;
                protected Body body;
                public Body Body => body;


                protected Border border = new();
                public FrameworkElement FrameworkElement => border;

                protected int index = -1;
                public int Index { get => index; set => index = value; }


                public Tab(Body body, Handle handle) {
                    border.Child = handle.FrameworkElement;
                    this.body = body;
                    this.handle = handle;
                }
            }
            public abstract class Handle {
                protected Border border = new();
                protected StackPanel stackPanel = new();
                protected TextBlock name = new();
                protected System.Windows.Controls.Image icon = new();

                public FrameworkElement FrameworkElement => border;

                public Handle(string name) {
                    stackPanel.Orientation = Orientation.Horizontal;
                    stackPanel.Children.Add(this.name);
                    stackPanel.Background = System.Windows.Media.Brushes.Transparent;
                    stackPanel.Margin = new Thickness(3, 0, 5, 0);
                    stackPanel.MouseEnter += (s, e) => { border.BorderBrush = System.Windows.Media.Brushes.Gainsboro; };
                    stackPanel.MouseLeave += (s, e) => { border.BorderBrush = System.Windows.Media.Brushes.Transparent; };

                    border.Child = stackPanel;
                    border.Background = Helper.StringToSolidColorBrush("#252525");
                    border.BorderBrush = System.Windows.Media.Brushes.Transparent;
                    border.BorderThickness = new Thickness(0.5);

                    this.name.Text = name;
                    this.name.VerticalAlignment = VerticalAlignment.Center;
                    this.name.Foreground = System.Windows.Media.Brushes.Gainsboro;
                    this.name.TextDecorations = TextDecorations.Underline;
                }

                public void SetIcon(string path) {
                    Helper.SetImageSource(icon, path);
                    stackPanel.Children.Insert(0, icon);
                }
            }
            public abstract class Body {
                protected Grid mainGrid = new();
                protected Border border = new();
                public FrameworkElement FrameworkElement => border;

                public Body() {
                    border.BorderBrush = Helper.StringToSolidColorBrush("#3d3d3d");
                    border.Child = mainGrid;
                    border.BorderThickness = new Thickness(1);
                }

                public void Hide() => mainGrid.Visibility = Visibility.Collapsed;
                public void Show() => mainGrid.Visibility = Visibility.Visible;
            }
        }

        namespace Tabs {
            public class ImageFilterTab : Tab {
                public ImageFilterTab(Body body, Handle handle) : base(body, handle) { }
            }

            public class PixelSortingTab : Tab {
                public PixelSortingTab(Body body, Handle handle) : base(body, handle) { }
            }
        }

        namespace Tabs.Handles {
            public class DefaultHandle : Handle{
                public DefaultHandle(string name) : base(name) {

                }
            }
        }

        namespace Tabs.Bodies {

            public class ImageFilter : Body {
                public ImageEditor ImageEditor = new();
                private StackPanel stackPanel = new();

                public Button grayScaleButton = new();
                public Slider slider = new();


                public ImageFilter() {
                    Helper.SetChildInGrid(mainGrid, stackPanel, 0, 0);
                    stackPanel.Children.Add(slider);
                    stackPanel.Children.Add(grayScaleButton);

                    slider.ValueChanged += (s, e) => Slider_ValueChanged();

                    DispatcherTimer timer = new();
                    timer.Interval = UIManager.Framerate;
                    timer.Tick += (s, e) => ImageEditor.ApplyChange();
                    timer.Start();

                    grayScaleButton.Background = Brushes.Transparent;
                    grayScaleButton.Click += GrayScaleButton_Click;
                    grayScaleButton.Content = "GrayScale";
                }


                private void Slider_ValueChanged() {
                    Debugger.Console.Log("Slider_ValueChanged");
                    GrayScale grayScale = new(WorkStation.WorkstationImage.Bitmap, slider.Value / 10);
                    ImageEditor.AddChange(grayScale);
                    WorkStation.WorkstationImage.GetHistory.Add(grayScale);
                }

                private void GrayScaleButton_Click(object sender, RoutedEventArgs e) {
                    GrayScale grayScale = new(WorkStation.WorkstationImage.Bitmap);
                    ImageEditor.AddChange(grayScale);
                    WorkStation.WorkstationImage.GetHistory.Add(grayScale);
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
    }

}
