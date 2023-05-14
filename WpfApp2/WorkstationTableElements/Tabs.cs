using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using WpfCustomControls;
using System.Diagnostics;
using DebugLibrary.Benchmark;
using Debugger;
using Console = Debugger.Console;
using System.Windows.Threading;
using System.Drawing.Imaging;
using System.Drawing;
using static Retros.Workstation;
using System.Windows.Media.Imaging;

namespace Retros {
    namespace WorkstationTableElements {

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


        namespace Tabs {
            public class ImageFilterTab : Tab {
                public ImageFilterTab(Body body, Handle handle) : base(body, handle) { }

                public class RedChannel : IChange {
                    private WorkstationImage image;
                    private double filterIntensity;

                    public RedChannel(WorkstationImage image, double filterIntensity = 1) {
                        this.image = image;
                        this.filterIntensity = filterIntensity;
                    }

                    public void Apply() {
                        BitmapSource bitmapSource = (BitmapSource)image.Original.Source;

                        WriteableBitmap writeableBitmap = new WriteableBitmap(bitmapSource);
                        int bytesPerPixel = (writeableBitmap.Format.BitsPerPixel + 7) / 8;
                        byte[] pixelData = new byte[writeableBitmap.PixelWidth * writeableBitmap.PixelHeight * bytesPerPixel];
                        writeableBitmap.CopyPixels(pixelData, writeableBitmap.PixelWidth * bytesPerPixel, 0);

                        for (int y = 0; y < writeableBitmap.PixelHeight; y++) {
                            for (int x = 0; x < writeableBitmap.PixelWidth; x++) {
                                int index = (y * writeableBitmap.PixelWidth + x) * bytesPerPixel;

                                byte r = pixelData[index + 2];
                                byte g = pixelData[index + 1];
                                byte b = pixelData[index + 0];

                                byte newVal = r;

                                var newR = (byte)((newVal * filterIntensity) + (r * (1 - filterIntensity)));
                                var newG = (byte)((newVal * filterIntensity) + (g * (1 - filterIntensity)));
                                var newB = (byte)((newVal * filterIntensity) + (b * (1 - filterIntensity)));

                                pixelData[index + 2] = newR;    // Red
                                pixelData[index + 1] = newG;    // Green
                                pixelData[index + 0] = newB;    // Blue
                            }
                        }

                        writeableBitmap.WritePixels(new Int32Rect(0, 0, writeableBitmap.PixelWidth, writeableBitmap.PixelHeight), pixelData, writeableBitmap.PixelWidth * bytesPerPixel, 0);
                        image.Image.Source = writeableBitmap;
                    }
                }
                public class GreenChannel : IChange {
                    private WorkstationImage image;
                    private double filterIntensity;

                    public GreenChannel(WorkstationImage image, double filterIntensity = 1) {
                        this.image = image;
                        this.filterIntensity = filterIntensity;
                    }

                    public void Apply() {
                        BitmapSource bitmapSource = (BitmapSource)image.Original.Source;

                        WriteableBitmap writeableBitmap = new WriteableBitmap(bitmapSource);
                        int bytesPerPixel = (writeableBitmap.Format.BitsPerPixel + 7) / 8;
                        byte[] pixelData = new byte[writeableBitmap.PixelWidth * writeableBitmap.PixelHeight * bytesPerPixel];
                        writeableBitmap.CopyPixels(pixelData, writeableBitmap.PixelWidth * bytesPerPixel, 0);

                        for (int y = 0; y < writeableBitmap.PixelHeight; y++) {
                            for (int x = 0; x < writeableBitmap.PixelWidth; x++) {
                                int index = (y * writeableBitmap.PixelWidth + x) * bytesPerPixel;

                                byte r = pixelData[index + 2];
                                byte g = pixelData[index + 1];
                                byte b = pixelData[index + 0];

                                byte newVal = g;

                                var newR = (byte)((newVal * filterIntensity) + (r * (1 - filterIntensity)));
                                var newG = (byte)((newVal * filterIntensity) + (g * (1 - filterIntensity)));
                                var newB = (byte)((newVal * filterIntensity) + (b * (1 - filterIntensity)));

                                pixelData[index + 2] = newR;    // Red
                                pixelData[index + 1] = newG;    // Green
                                pixelData[index + 0] = newB;    // Blue
                            }
                        }

                        writeableBitmap.WritePixels(new Int32Rect(0, 0, writeableBitmap.PixelWidth, writeableBitmap.PixelHeight), pixelData, writeableBitmap.PixelWidth * bytesPerPixel, 0);
                        image.Image.Source = writeableBitmap;
                    }
                }
                public class BlueChannel : IChange {
                    private WorkstationImage image;
                    private double filterIntensity;

                    public BlueChannel(WorkstationImage image, double filterIntensity = 1) {
                        this.image = image;
                        this.filterIntensity = filterIntensity;
                    }

                    public void Apply() {
                        BitmapSource bitmapSource = (BitmapSource)image.Original.Source;

                        WriteableBitmap writeableBitmap = new WriteableBitmap(bitmapSource);
                        int bytesPerPixel = (writeableBitmap.Format.BitsPerPixel + 7) / 8;
                        byte[] pixelData = new byte[writeableBitmap.PixelWidth * writeableBitmap.PixelHeight * bytesPerPixel];
                        writeableBitmap.CopyPixels(pixelData, writeableBitmap.PixelWidth * bytesPerPixel, 0);

                        for (int y = 0; y < writeableBitmap.PixelHeight; y++) {
                            for (int x = 0; x < writeableBitmap.PixelWidth; x++) {
                                int index = (y * writeableBitmap.PixelWidth + x) * bytesPerPixel;

                                byte r = pixelData[index + 2];
                                byte g = pixelData[index + 1];
                                byte b = pixelData[index + 0];

                                byte newVal = b;

                                var newR = (byte)((newVal * filterIntensity) + (r * (1 - filterIntensity)));
                                var newG = (byte)((newVal * filterIntensity) + (g * (1 - filterIntensity)));
                                var newB = (byte)((newVal * filterIntensity) + (b * (1 - filterIntensity)));

                                pixelData[index + 2] = newR;    // Red
                                pixelData[index + 1] = newG;    // Green
                                pixelData[index + 0] = newB;    // Blue
                            }
                        }

                        writeableBitmap.WritePixels(new Int32Rect(0, 0, writeableBitmap.PixelWidth, writeableBitmap.PixelHeight), pixelData, writeableBitmap.PixelWidth * bytesPerPixel, 0);
                        image.Image.Source = writeableBitmap;
                    }
                }

                public class GrayScale : IChange {
                    private WorkstationImage image;
                    private double filterIntensity;

                    public GrayScale(WorkstationImage image, double filterIntensity = 1) {
                        this.image = image;
                        this.filterIntensity = filterIntensity;
                    }

                    public void Apply() {
                        BitmapSource bitmapSource = (BitmapSource)image.Original.Source;

                        WriteableBitmap writeableBitmap = new WriteableBitmap(bitmapSource);
                        int bytesPerPixel = (writeableBitmap.Format.BitsPerPixel + 7) / 8;
                        byte[] pixelData = new byte[writeableBitmap.PixelWidth * writeableBitmap.PixelHeight * bytesPerPixel];
                        writeableBitmap.CopyPixels(pixelData, writeableBitmap.PixelWidth * bytesPerPixel, 0);

                        for (int y = 0; y < writeableBitmap.PixelHeight; y++) {
                            for (int x = 0; x < writeableBitmap.PixelWidth; x++) {
                                int index = (y * writeableBitmap.PixelWidth + x) * bytesPerPixel;

                                byte r = pixelData[index + 2];
                                byte g = pixelData[index + 1];
                                byte b = pixelData[index + 0];

                                byte gray = (byte)(0.299 * r + 0.587 * g + 0.114 * b);  // (`0.299`, `0.587`, `0.114`)  ITU-R BT.709 standard

                                var newR = (byte)((gray * filterIntensity) + (r * (1 - filterIntensity)));
                                var newG = (byte)((gray * filterIntensity) + (g * (1 - filterIntensity)));
                                var newB = (byte)((gray * filterIntensity) + (b * (1 - filterIntensity)));

                                pixelData[index + 2] = newR;    // Red
                                pixelData[index + 1] = newG;    // Green
                                pixelData[index + 0] = newB;    // Blue
                            }
                        }

                        writeableBitmap.WritePixels(new Int32Rect(0, 0, writeableBitmap.PixelWidth, writeableBitmap.PixelHeight), pixelData, writeableBitmap.PixelWidth * bytesPerPixel, 0);
                        image.Image.Source = writeableBitmap;
                    }

                    /*
                    private Bitmap bitmap;
                    private byte filterIntensity;

                    public GrayScale(Bitmap bitmap, double filterIntensity = 1) {
                        this.bitmap = bitmap;
                        this.filterIntensity = (byte)filterIntensity;
                    }

                    // Changes the Bitmap data
                    public void Apply() {
                        BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        int regionHeight = bitmap.Height / Environment.ProcessorCount;
                        Parallel.For(0, Environment.ProcessorCount, i => {
                            int startY = i * regionHeight;
                            int endY = (i == Environment.ProcessorCount - 1) ? bitmap.Height : startY + regionHeight;
                            unsafe {
                                byte* ptr = (byte*)bitmapData.Scan0.ToPointer() + startY * bitmapData.Stride;
                                for (int y = startY; y < endY; y++) {
                                    for (int x = 0; x < bitmapData.Width; x++) {
                                        byte grayScale = (byte)((ptr[2] + ptr[1] + ptr[0]) / 3);
                                        ptr[0] = (byte)(grayScale * filterIntensity + ptr[0] * (1 - filterIntensity));
                                        ptr[1] = (byte)(grayScale * filterIntensity + ptr[1] * (1 - filterIntensity));
                                        ptr[2] = (byte)(grayScale * filterIntensity + ptr[2] * (1 - filterIntensity));
                                        ptr += 4;
                                    }
                                }
                            }
                        });
                        bitmap.UnlockBits(bitmapData);
                    }
                    */
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
                }
            }
        }

    }

}