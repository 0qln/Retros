using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using WpfUtillities;
using System.Diagnostics;
using DebugLibrary.Benchmark;
using Debugger;
using Console = Debugger.Console;
using System.Windows.Threading;
using System.Drawing.Imaging;
using System.Drawing;
using static Retros.Workstation;
using System.Windows.Media.Imaging;
using System.Security.RightsManagement;
using Retros;

namespace Retros
{
    namespace WorkstationTableElements
    {

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


        namespace Tabs
        {
            public class ImageFilterTab : Tab {
                public ImageFilterTab(Body body, Handle handle) : base(body, handle) { }


                /*
                public class NoRedChannel : IChange {
                    private WorkstationImage image;
                    private double filterIntensity;

                    public NoRedChannel(WorkstationImage image, double filterIntensity = 1) {
                        this.image = image;
                        this.filterIntensity = filterIntensity;
                    }

                    public void Apply() {
                        applied = true;
                        BitmapSource bitmapSource = (BitmapSource)image.CurrentImage.Source;

                        WriteableBitmap writeableBitmap = new WriteableBitmap(bitmapSource);
                        int bytesPerPixel = (writeableBitmap.Format.BitsPerPixel + 7) / 8;
                        byte[] pixelData = new byte[writeableBitmap.PixelWidth * writeableBitmap.PixelHeight * bytesPerPixel];
                        writeableBitmap.CopyPixels(pixelData, writeableBitmap.PixelWidth * bytesPerPixel, 0);

                        for (int y = 0; y < writeableBitmap.PixelHeight; y++) {
                            for (int x = 0; x < writeableBitmap.PixelWidth; x++) {
                                int index = (y * writeableBitmap.PixelWidth + x) * bytesPerPixel;
                                pixelData[index + 2] = (byte)(pixelData[index + 2] * (1 - filterIntensity));    // Green
                            }
                        }

                        writeableBitmap.WritePixels(new Int32Rect(0, 0, writeableBitmap.PixelWidth, writeableBitmap.PixelHeight), pixelData, writeableBitmap.PixelWidth * bytesPerPixel, 0);
                        image.AddTaskToQueue(() => image.SetSource(writeableBitmap));
                    }

                    private bool applied = false;
                    public bool Applied() => applied;
                }
                */


                public class OnlyRedChannel : IChange, IFilterChange {
                    private WorkstationImage image;
                    private double filterIntensity;
                    public double FilterIntensity { set => filterIntensity = value; get => filterIntensity; }

                    public OnlyRedChannel(WorkstationImage image) {
                        this.image = image;
                    }

                    public void Apply() {
                        int bytesPerPixel = (image.DummyImage.Format.BitsPerPixel + 7) / 8;
                        byte[] pixelData = new byte[image.DummyImage.PixelWidth *image.DummyImage.PixelHeight * bytesPerPixel];
                        image.DummyImage.CopyPixels(pixelData, image.DummyImage.PixelWidth * bytesPerPixel, 0);
                        int pixelHeight = image.DummyImage.PixelHeight;
                        int pixelWidth = image.DummyImage.PixelWidth;

                        Parallel.For(0, pixelHeight, y => {
                            Parallel.For(0, pixelWidth, x => {
                                int index = (y * pixelWidth + x) * bytesPerPixel;
                                pixelData[index + 1] = (byte)(pixelData[index + 1] * (1 - filterIntensity));    // Green
                                pixelData[index + 0] = (byte)(pixelData[index + 0] * (1 - filterIntensity));    // Blue
                            });
                        });

                        image.DummyImage.WritePixels(new Int32Rect(0, 0, image.DummyImage.PixelWidth, image.DummyImage.PixelHeight), pixelData, image.DummyImage.PixelWidth * bytesPerPixel, 0);
                    }
                }
                public class OnlyGreenChannel : IChange, IFilterChange {
                    private WorkstationImage image;
                    private double filterIntensity;
                    public double FilterIntensity { set => filterIntensity = value; get => filterIntensity; }

                    public OnlyGreenChannel(WorkstationImage image) {
                        this.image = image;
                    }

                    public void Apply() {
                        int bytesPerPixel = (image.DummyImage.Format.BitsPerPixel + 7) / 8;
                        byte[] pixelData = new byte[image.DummyImage.PixelWidth * image.DummyImage.PixelHeight * bytesPerPixel];
                        image.DummyImage.CopyPixels(pixelData, image.DummyImage.PixelWidth * bytesPerPixel, 0);
                        int pixelHeight = image.DummyImage.PixelHeight;
                        int pixelWidth = image.DummyImage.PixelWidth;

                        Parallel.For(0, pixelHeight, y => {
                            Parallel.For(0, pixelWidth, x => {
                                int index = (y * pixelWidth + x) * bytesPerPixel;
                                pixelData[index + 2] = (byte)(pixelData[index + 2] * (1 - (filterIntensity)));    // Red
                                pixelData[index + 0] = (byte)(pixelData[index + 0] * (1 - (filterIntensity)));    // Blue
                            });
                        });

                        image.DummyImage.WritePixels(new Int32Rect(0, 0, image.DummyImage.PixelWidth, image.DummyImage.PixelHeight), pixelData, image.DummyImage.PixelWidth * bytesPerPixel, 0);
                    }
                }
                public class OnlyBlueChannel : IChange, IFilterChange {
                    private WorkstationImage image;
                    private double filterIntensity;
                    public double FilterIntensity { set => filterIntensity = value; get => filterIntensity; }

                    public OnlyBlueChannel(WorkstationImage image) {
                        this.image = image;
                    }

                    public void Apply() {
                        int bytesPerPixel = (image.DummyImage.Format.BitsPerPixel + 7) / 8;
                        byte[] pixelData = new byte[image.DummyImage.PixelWidth * image.DummyImage.PixelHeight * bytesPerPixel];
                        image.DummyImage.CopyPixels(pixelData, image.DummyImage.PixelWidth * bytesPerPixel, 0);
                        int pixelHeight = image.DummyImage.PixelHeight;
                        int pixelWidth = image.DummyImage.PixelWidth;

                        Parallel.For(0, pixelHeight, y => {
                            Parallel.For(0, pixelWidth, x => {
                                int index = (y * pixelWidth + x) * bytesPerPixel;
                                pixelData[index + 2] = (byte)(pixelData[index + 2] * (1 - filterIntensity));    // Red
                                pixelData[index + 1] = (byte)(pixelData[index + 1] * (1 - filterIntensity));    // Green
                            });
                        });

                        image.DummyImage.WritePixels(new Int32Rect(0, 0, image.DummyImage.PixelWidth, image.DummyImage.PixelHeight), pixelData, image.DummyImage.PixelWidth * bytesPerPixel, 0);
                    }
                }

                public class GrayScale : IChange, IFilterChange {
                    private WorkstationImage image;
                    private double filterIntensity;
                    public double FilterIntensity { set => filterIntensity = value; get => filterIntensity; }

                    public GrayScale(WorkstationImage image, double filterIntensity = 1) {
                        this.image = image;
                        this.filterIntensity = filterIntensity;
                    }

                    public void Apply() {
                        int bytesPerPixel = (image.DummyImage.Format.BitsPerPixel + 7) / 8;
                        byte[] pixelData = new byte[image.DummyImage.PixelWidth * image.DummyImage.PixelHeight * bytesPerPixel];
                        image.DummyImage.CopyPixels(pixelData, image.DummyImage.PixelWidth * bytesPerPixel, 0);
                        int pixelHeight = image.DummyImage.PixelHeight;
                        int pixelWidth = image.DummyImage.PixelWidth;

                        Parallel.For(0, pixelHeight, y => {
                            Parallel.For(0, pixelWidth, x => {
                                int index = (y * pixelWidth + x) * bytesPerPixel;

                                byte r = pixelData[index + 2];
                                byte g = pixelData[index + 1];
                                byte b = pixelData[index + 0];
                                byte gray = (byte)(0.299 * r + 0.587 * g + 0.114 * b);  // (`0.299`, `0.587`, `0.114`) is ITU-R BT.709 standard

                                pixelData[index + 2] = (byte)((gray * filterIntensity) + (r * (1 - filterIntensity)));     // Red
                                pixelData[index + 1] = (byte)((gray * filterIntensity) + (g * (1 - filterIntensity)));     // Green
                                pixelData[index + 0] = (byte)((gray * filterIntensity) + (b * (1 - filterIntensity)));     // Blue
                            });
                        });

                        image.DummyImage.WritePixels(new Int32Rect(0, 0, image.DummyImage.PixelWidth, image.DummyImage.PixelHeight), pixelData, image.DummyImage.PixelWidth * bytesPerPixel, 0);
                    }
                }
            }

            public class PixelSortingTab : Tab {
                public PixelSortingTab(Body body, Handle handle) : base(body, handle) { }

                public class PixelSort : IChange {
                    private Bitmap image;

                    /// <summary> The upper and lower bounds of pixels that will be sorted </summary>
                    private double lowerBound;
                    private double upperBound;


                    public PixelSort(string path, double lowerBound = 50, double upperBound = 205) {
                        image = new Bitmap(path);
                        this.lowerBound = lowerBound;
                        this.upperBound = upperBound;
                    }

                    /// <summary> Sort the pixels in the image </summary>
                    public void Apply() {
                        throw new NotImplementedException();
                    }

                    public bool Applied() {
                        throw new NotImplementedException();
                    }
                }
            }
        }

    }

}