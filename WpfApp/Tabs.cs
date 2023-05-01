using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Retros.ClientWorkStation.Tab;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using WpfCustomControls;
using Retros.ImageEditing;

namespace Retros {
    namespace TabBodies {
        namespace ImageEditing {


            public class Filters : IBody {
                private Grid mainGrid = new();
                private Border border = new();
                public FrameworkElement FrameworkElement => border;
                public ImageEditor ImageEditor = new();
                private StackPanel stackPanel = new();

                public Button grayScaleButton = new();

                public Filters() {
                    border.BorderBrush = Helper.StringToSolidColorBrush("#3d3d3d");
                    border.Child = mainGrid;
                    border.BorderThickness = new Thickness(1);

                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = "Filters";
                    stackPanel.Children.Add(textBlock);

                    Helper.SetChildInGrid(mainGrid, stackPanel, 0, 0);

                    grayScaleButton.Background = Brushes.Transparent;
                    grayScaleButton.Click += (s, e) => {
                        ImageEditor.AddChange(new GrayScale());
                        ImageEditor.ApplyAllChanges();
                    };
                    stackPanel.Children.Add(grayScaleButton);
                    grayScaleButton.Content = "GrayScale";
                }

                public void Hide() => mainGrid.Visibility = Visibility.Collapsed;
                public void Show() => mainGrid.Visibility = Visibility.Visible;
            }


            public class PixelSorting : IBody {
                private Grid mainGrid = new();
                private Border border = new();
                public FrameworkElement FrameworkElement => border;


                public PixelSorting() {
                    border.BorderBrush = Helper.StringToSolidColorBrush("#3d3d3d");
                    border.Child = mainGrid;
                    border.BorderThickness = new Thickness(1);


                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = "PixelSorting";
                    Helper.SetChildInGrid(mainGrid, textBlock, 0, 0);
                }


                public void Hide() => mainGrid.Visibility = Visibility.Collapsed;
                public void Show() => mainGrid.Visibility = Visibility.Visible;
            }
        }
        

        namespace File {
            public class Export : IBody {
                private Grid mainGrid = new();
                private Border border = new();
                public FrameworkElement FrameworkElement => border;


                public Export() {
                    border.BorderBrush = Helper.StringToSolidColorBrush("#3d3d3d");
                    border.Child = mainGrid;
                    border.BorderThickness = new Thickness(1);

                }

                public void Hide() => mainGrid.Visibility = Visibility.Collapsed;
                public void Show() => mainGrid.Visibility = Visibility.Visible;
            }

            public class Import : IBody {
                private Grid mainGrid = new();
                private Border border = new();
                public FrameworkElement FrameworkElement => border;


                public Import() {
                    border.BorderBrush = Helper.StringToSolidColorBrush("#3d3d3d");
                    border.Child = mainGrid;
                    border.BorderThickness = new Thickness(1);

                }

                public void Hide() => mainGrid.Visibility = Visibility.Collapsed;
                public void Show() => mainGrid.Visibility = Visibility.Visible;
            }
        }
    }

}
