using DebugLibrary.Benchmark;
using Retros.ProgramWindow.DisplaySystem;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Retros.ProgramWindow.Filters {

    public class OnlyRedChannel : IChange, IFilterChange {
        private WorkstationImage image;
        private double filterIntensity;
        public double FilterIntensity {
            set {
                filterIntensity = value;
                applied = false;
            }
            get => filterIntensity;
        }
        private bool applied = false;
        public bool Applied => applied;

        public OnlyRedChannel(WorkstationImage image) {
            this.image = image;
        }

        public void Generate() {
            int bytesPerPixel = (image.DummyImage.Format.BitsPerPixel + 7) / 8;
            byte[] pixelData = new byte[image.DummyImage.PixelWidth * image.DummyImage.PixelHeight * bytesPerPixel];
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

            applied = true;
        }
        public void Generate(WriteableBitmap bitmap) {
            int bytesPerPixel = (bitmap.Format.BitsPerPixel + 7) / 8;
            byte[] pixelData = new byte[bitmap.PixelWidth * bitmap.PixelHeight * bytesPerPixel];
            bitmap.CopyPixels(pixelData, bitmap.PixelWidth * bytesPerPixel, 0);
            int pixelHeight = bitmap.PixelHeight;
            int pixelWidth = bitmap.PixelWidth;

            Parallel.For(0, pixelHeight, y => {
                Parallel.For(0, pixelWidth, x => {
                    int index = (y * pixelWidth + x) * bytesPerPixel;
                    pixelData[index + 1] = (byte)(pixelData[index + 1] * (1 - filterIntensity));    // Green
                    pixelData[index + 0] = (byte)(pixelData[index + 0] * (1 - filterIntensity));    // Blue
                });
            });

            bitmap.WritePixels(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight), pixelData, bitmap.PixelWidth * bytesPerPixel, 0);

            applied = true;
        }
    }
    public class OnlyGreenChannel : IChange, IFilterChange {
        private WorkstationImage image;
        private double filterIntensity;
        public double FilterIntensity {
            set {
                filterIntensity = value;
                applied = false;
            }
            get => filterIntensity;
        }
        private bool applied = false;
        public bool Applied => applied;

        public OnlyGreenChannel(WorkstationImage image) {
            this.image = image;
        }

        public void Generate() {
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

            applied = true;
        }
        public void Generate(WriteableBitmap bitmap) {
            int bytesPerPixel = (bitmap.Format.BitsPerPixel + 7) / 8;
            byte[] pixelData = new byte[bitmap.PixelWidth * bitmap.PixelHeight * bytesPerPixel];
            bitmap.CopyPixels(pixelData, bitmap.PixelWidth * bytesPerPixel, 0);
            int pixelHeight = bitmap.PixelHeight;
            int pixelWidth = bitmap.PixelWidth;


            Parallel.For(0, pixelHeight, y => {
                Parallel.For(0, pixelWidth, x => {
                    int index = (y * pixelWidth + x) * bytesPerPixel;
                    pixelData[index + 2] = (byte)(pixelData[index + 2] * (1 - (filterIntensity)));    // Red
                    pixelData[index + 0] = (byte)(pixelData[index + 0] * (1 - (filterIntensity)));    // Blue
                });
            });

            bitmap.WritePixels(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight), pixelData, bitmap.PixelWidth * bytesPerPixel, 0);

            applied = true;
        }
    }
    public class OnlyBlueChannel : IChange, IFilterChange {
        private WorkstationImage image;
        private double filterIntensity;
        public double FilterIntensity {
            set {
                filterIntensity = value;
                applied = false;
            }
            get => filterIntensity;
        }
        private bool applied = false;
        public bool Applied => applied;

        public OnlyBlueChannel(WorkstationImage image) {
            this.image = image;
        }

        public void Generate() {
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

            applied = true;
        }
        public void Generate(WriteableBitmap bitmap) {
            int bytesPerPixel = (bitmap.Format.BitsPerPixel + 7) / 8;
            byte[] pixelData = new byte[bitmap.PixelWidth * bitmap.PixelHeight * bytesPerPixel];
            bitmap.CopyPixels(pixelData, bitmap.PixelWidth * bytesPerPixel, 0);
            int pixelHeight = bitmap.PixelHeight;
            int pixelWidth = bitmap.PixelWidth;

            Parallel.For(0, pixelHeight, y => {
                Parallel.For(0, pixelWidth, x => {
                    int index = (y * pixelWidth + x) * bytesPerPixel;
                    pixelData[index + 2] = (byte)(pixelData[index + 2] * (1 - filterIntensity));    // Red
                    pixelData[index + 1] = (byte)(pixelData[index + 1] * (1 - filterIntensity));    // Green
                });
            });

            bitmap.WritePixels(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight), pixelData, bitmap.PixelWidth * bytesPerPixel, 0);

            applied = true;
        }

    }

    public class TestBlue : IChange, IFilterChange {
        private WorkstationImage image;
        private double filterIntensity;
        public double FilterIntensity {
            set {
                filterIntensity = value;
                applied = false;
            }
            get => filterIntensity;
        }
        private bool applied = false;
        public bool Applied => applied;

        public TestBlue(WorkstationImage image) {
            this.image = image;
        }


        public void Generate() {
            int bytesPerPixel = (image.DummyImage.Format.BitsPerPixel + 7) / 8;
            int pixelHeight = image.DummyImage.PixelHeight;
            int pixelWidth = image.DummyImage.PixelWidth;
            int stride = image.DummyImage.BackBufferStride;

            // Lock the WriteableBitmap to get a reference to its pixel buffer
            image.DummyImage.Lock();

            try {
                unsafe {
                    // Get a pointer to the pixel buffer
                    IntPtr bufferPtr = image.DummyImage.BackBuffer;
                    byte* pixelData = (byte*)bufferPtr;

                    // Modify the pixel values directly in the pixel buffer
                    Parallel.For(0, pixelHeight, y => {
                        byte* row = pixelData + (y * stride);
                        Parallel.For(0, pixelWidth, x => {
                            int index = x * bytesPerPixel;
                            row[index + 0] = (byte)(255 * (1 - filterIntensity));
                        });
                    });
                }
            }
            finally {
                // Unlock the WriteableBitmap to release the pixel buffer
                image.DummyImage.Unlock();
            }

            applied = true;
        }
        public void Generate(WriteableBitmap bitmap) {
            int bytesPerPixel = (bitmap.Format.BitsPerPixel + 7) / 8;
            int pixelHeight = bitmap.PixelHeight;
            int pixelWidth = bitmap.PixelWidth;
            int stride = bitmap.BackBufferStride;

            // Lock the WriteableBitmap to get a reference to its pixel buffer
            bitmap.Lock();

            try {
                unsafe {
                    // Get a pointer to the pixel buffer
                    IntPtr bufferPtr = bitmap.BackBuffer;
                    byte* pixelData = (byte*)bufferPtr;

                    // Modify the pixel values directly in the pixel buffer
                    Parallel.For(0, pixelHeight, y => {
                        byte* row = pixelData + (y * stride);
                        Parallel.For(0, pixelWidth, x => {
                            int index = x * bytesPerPixel;
                            row[index + 0] = (byte)(255 * (1 - filterIntensity));
                        });
                    });
                }
            }
            finally {
                // Unlock the WriteableBitmap to release the pixel buffer
                bitmap.Unlock();
            }

            applied = true;
        }
    }

    public class GrayScale : IChange, IFilterChange {
        private WorkstationImage image;
        private double filterIntensity;
        private bool applied = false;

        public double FilterIntensity {
            set {
                filterIntensity = value;
                applied = false;
            }
            get => filterIntensity;
        }
        public bool Applied => applied;

        public GrayScale(WorkstationImage image) {
            this.image = image;
        }


        public void Generate() {
            /* Sadly, the clean approach is about 3 times as slow :(
                image.DummyImage.IteratePixels(
                    (r, g, b, x, y) => {
                        byte gray = (byte)(0.299 * r + 0.587 * g + 0.114 * b);
                        return (byte)((r * (1-filterIntensity)) + (gray * filterIntensity));
                    }, 
                    (r, g, b, x, y) => {
                        byte gray = (byte)(0.299 * r + 0.587 * g + 0.114 * b);
                        return (byte)((g * (1 - filterIntensity)) + (gray * filterIntensity));
                    }, 
                    (r, g, b, x, y) => {
                        byte gray = (byte)(0.299 * r + 0.587 * g + 0.114 * b);
                        return (byte)((b * (1 - filterIntensity)) + (gray * filterIntensity));
                    }
                );
                applied = true;
            */

            int bytesPerPixel = (image.DummyImage.Format.BitsPerPixel + 7) / 8;
            int pixelHeight = image.DummyImage.PixelHeight;
            int pixelWidth = image.DummyImage.PixelWidth;
            int stride = image.DummyImage.BackBufferStride;

            // Lock the WriteableBitmap to get a reference to its pixel buffer
            image.DummyImage.Lock();

            try {
                unsafe {
                    // Get a pointer to the pixel buffer
                    IntPtr bufferPtr = image.DummyImage.BackBuffer;
                    byte* pixelData = (byte*)bufferPtr;

                    // Modify the pixel values directly in the pixel buffer
                    Parallel.For(0, pixelHeight, y =>
                    {
                        byte* row = pixelData + (y * stride);
                        Parallel.For(0, pixelWidth, x =>
                        {
                            int index = x * bytesPerPixel;
                            byte gray = (byte)(0.299 * row[index + 2] + 0.587 * row[index + 1] + 0.114 * row[index + 0]);   // (`0.299`, `0.587`, `0.114`) is ITU-R BT.709 standard
                            row[index + 2] = (byte)((gray * filterIntensity) + (row[index + 2] * (1 - filterIntensity)));   // Red
                            row[index + 1] = (byte)((gray * filterIntensity) + (row[index + 1] * (1 - filterIntensity)));   // Green
                            row[index + 0] = (byte)((gray * filterIntensity) + (row[index + 0] * (1 - filterIntensity)));   // Blue
                        });
                    });
                }
            }
            finally {
                // Unlock the WriteableBitmap to release the pixel buffer
                image.DummyImage.Unlock();
            }

            applied = true;

        }
        public void Generate(WriteableBitmap bitmap) {
            int bytesPerPixel = (bitmap.Format.BitsPerPixel + 7) / 8;
            int pixelHeight = bitmap.PixelHeight;
            int pixelWidth = bitmap.PixelWidth;
            int stride = bitmap.BackBufferStride;

            // Lock the WriteableBitmap to get a reference to its pixel buffer
            bitmap.Lock();

            try {
                unsafe {
                    // Get a pointer to the pixel buffer
                    IntPtr bufferPtr = bitmap.BackBuffer;
                    byte* pixelData = (byte*)bufferPtr;

                    // Modify the pixel values directly in the pixel buffer
                    Parallel.For(0, pixelHeight, y => {
                        byte* row = pixelData + (y * stride);
                        Parallel.For(0, pixelWidth, x => {
                            int index = x * bytesPerPixel;

                            byte r = row[index + 2];
                            byte g = row[index + 1];
                            byte b = row[index + 0];
                            byte gray = (byte)(0.299 * r + 0.587 * g + 0.114 * b);  // (`0.299`, `0.587`, `0.114`) is ITU-R BT.709 standard

                            row[index + 2] = (byte)((gray * filterIntensity) + (r * (1 - filterIntensity)));     // Red
                            row[index + 1] = (byte)((gray * filterIntensity) + (g * (1 - filterIntensity)));     // Green
                            row[index + 0] = (byte)((gray * filterIntensity) + (b * (1 - filterIntensity)));     // Blue
                        });
                    });

                }
            }
            finally {
                // Unlock the WriteableBitmap to release the pixel buffer
                bitmap.Unlock();
            }

            applied = true;
        }


        /*
        public void Generate() {
            DebugLibrary.Console.Log("Generating: " + 
            Measure.Execute(() => {
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
            }).ElapsedMilliseconds);

            applied = true;
        }
        */
    }

    /*
    public class NoRedChannel : IChange, IFilterChange {
        public WriteableBitmap Bitmap { get; }
        private WorkstationImage image;
        private double filterIntensity;
        public double FilterIntensity {
            set {
                filterIntensity = value;
                applied = false;
            }
            get => filterIntensity;
        }
        private bool applied = false;
        public bool Applied => applied;

        public NoRedChannel(WorkstationImage image) {
            this.image = image;
            Bitmap = new((BitmapSource)image.SourceImage.Source);
        }

        public void Generate() {
            int bytesPerPixel = (Bitmap.Format.BitsPerPixel + 7) / 8;
            byte[] pixelData = new byte[Bitmap.PixelWidth * Bitmap.PixelHeight * bytesPerPixel];
            Bitmap.CopyPixels(pixelData, Bitmap.PixelWidth * bytesPerPixel, 0);
            int pixelHeight = Bitmap.PixelHeight;
            int pixelWidth = Bitmap.PixelWidth;

            Parallel.For(0, pixelHeight, y => {
                Parallel.For(0, pixelWidth, x => {
                    int index = (y * pixelWidth + x) * bytesPerPixel;
                    pixelData[index + 2] = (byte)(pixelData[index + 2] * (1 - filterIntensity));    // Red
                });
            });

            Bitmap.WritePixels(new Int32Rect(0, 0, Bitmap.PixelWidth, Bitmap.PixelHeight), pixelData, Bitmap.PixelWidth * bytesPerPixel, 0);

            applied = true;
        }
    }
    public class OnlyRedChannel : IChange, IFilterChange {
        public WriteableBitmap Bitmap { get; }
        private WorkstationImage image;
        private double filterIntensity;
        public double FilterIntensity {
            set {
                filterIntensity = value;
                applied = false;
            }
            get => filterIntensity;
        }
        private bool applied = false;
        public bool Applied => applied;

        public OnlyRedChannel(WorkstationImage image) {
            this.image = image;
            Bitmap = new((BitmapSource)image.SourceImage.Source);
        }

        public void Generate() {
            int bytesPerPixel = (Bitmap.Format.BitsPerPixel + 7) / 8;
            byte[] pixelData = new byte[Bitmap.PixelWidth * Bitmap.PixelHeight * bytesPerPixel];
            Bitmap.CopyPixels(pixelData, Bitmap.PixelWidth * bytesPerPixel, 0);
            int pixelHeight = Bitmap.PixelHeight;
            int pixelWidth = Bitmap.PixelWidth;

            Parallel.For(0, pixelHeight, y => {
                Parallel.For(0, pixelWidth, x => {
                    int index = (y * pixelWidth + x) * bytesPerPixel;
                    pixelData[index + 1] = (byte)(pixelData[index + 1] * (1 - filterIntensity));    // Green
                    pixelData[index + 0] = (byte)(pixelData[index + 0] * (1 - filterIntensity));    // Blue
                });
            });

            Bitmap.WritePixels(new Int32Rect(0, 0, Bitmap.PixelWidth, Bitmap.PixelHeight), pixelData, Bitmap.PixelWidth * bytesPerPixel, 0);

            applied = true;
        }
    }
    public class OnlyGreenChannel : IChange, IFilterChange {
        public WriteableBitmap Bitmap { get; }
        private WorkstationImage image;
        private double filterIntensity;
        public double FilterIntensity {
            set {
                filterIntensity = value;
                applied = false;
            }
            get => filterIntensity;
        }
        private bool applied = false;
        public bool Applied => applied;

        public OnlyGreenChannel(WorkstationImage image) {
            this.image = image;
            Bitmap = new((BitmapSource)image.SourceImage.Source);
        }

        public void Generate() {
            int bytesPerPixel = (Bitmap.Format.BitsPerPixel + 7) / 8;
            byte[] pixelData = new byte[Bitmap.PixelWidth * Bitmap.PixelHeight * bytesPerPixel];
            Bitmap.CopyPixels(pixelData, Bitmap.PixelWidth * bytesPerPixel, 0);
            int pixelHeight = Bitmap.PixelHeight;
            int pixelWidth = Bitmap.PixelWidth;

            Parallel.For(0, pixelHeight, y => {
                Parallel.For(0, pixelWidth, x => {
                    int index = (y * pixelWidth + x) * bytesPerPixel;
                    pixelData[index + 2] = (byte)(pixelData[index + 2] * (1 - (filterIntensity)));    // Red
                    pixelData[index + 0] = (byte)(pixelData[index + 0] * (1 - (filterIntensity)));    // Blue
                });
            });

            Bitmap.WritePixels(new Int32Rect(0, 0, Bitmap.PixelWidth, Bitmap.PixelHeight), pixelData, Bitmap.PixelWidth * bytesPerPixel, 0);

            applied = true;
        }
    }
    public class OnlyBlueChannel : IChange, IFilterChange {
        public WriteableBitmap Bitmap { get; }
        private WorkstationImage image;
        private double filterIntensity;
        public double FilterIntensity {
            set {
                filterIntensity = value;
                applied = false;
            }
            get => filterIntensity;
        }
        private bool applied = false;
        public bool Applied => applied;

        public OnlyBlueChannel(WorkstationImage image) {
            this.image = image;
            Bitmap = new((BitmapSource)image.SourceImage.Source);
        }

        public void Generate() {
            int bytesPerPixel = (Bitmap.Format.BitsPerPixel + 7) / 8;
            byte[] pixelData = new byte[Bitmap.PixelWidth * Bitmap.PixelHeight * bytesPerPixel];
            Bitmap.CopyPixels(pixelData, Bitmap.PixelWidth * bytesPerPixel, 0);
            int pixelHeight = Bitmap.PixelHeight;
            int pixelWidth = Bitmap.PixelWidth;

            Parallel.For(0, pixelHeight, y => {
                Parallel.For(0, pixelWidth, x => {
                    int index = (y * pixelWidth + x) * bytesPerPixel;
                    pixelData[index + 2] = (byte)(pixelData[index + 2] * (1 - filterIntensity));    // Red
                    pixelData[index + 1] = (byte)(pixelData[index + 1] * (1 - filterIntensity));    // Green
                });
            });

            Bitmap.WritePixels(new Int32Rect(0, 0, Bitmap.PixelWidth, Bitmap.PixelHeight), pixelData, Bitmap.PixelWidth * bytesPerPixel, 0);

            applied = true;
        }
    }
    public class GrayScale : IChange, IFilterChange {
        public WriteableBitmap Bitmap { get; }
        private WorkstationImage image;
        private double filterIntensity;
        public double FilterIntensity {
            set {
                filterIntensity = value;
                applied = false;
            }
            get => filterIntensity;
        }
        private bool applied = false;
        public bool Applied => applied;

        public GrayScale(WorkstationImage image) {
            this.image = image;
            Bitmap = new((BitmapSource)image.SourceImage.Source);
        }

        public void Generate() {
            int bytesPerPixel = (Bitmap.Format.BitsPerPixel + 7) / 8;
            byte[] pixelData = new byte[Bitmap.PixelWidth * Bitmap.PixelHeight * bytesPerPixel];
            Bitmap.CopyPixels(pixelData, Bitmap.PixelWidth * bytesPerPixel, 0);
            int pixelHeight = Bitmap.PixelHeight;
            int pixelWidth = Bitmap.PixelWidth;

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


            Bitmap.WritePixels(new Int32Rect(0, 0, Bitmap.PixelWidth, Bitmap.PixelHeight), pixelData, Bitmap.PixelWidth * bytesPerPixel, 0);
            //image.DummyImage.WritePixels(new Int32Rect(0, 0, image.DummyImage.PixelWidth, image.DummyImage.PixelHeight), pixelData, image.DummyImage.PixelWidth * bytesPerPixel, 0);

            applied = true;
        }
    }
    */
}
