using Retros.ProgramWindow.DisplaySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Retros.ProgramWindow.Filters {

    //NEEERD
    public class NoRedChannel : IChange, IFilterChange {
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
                });
            });

            image.DummyImage.WritePixels(new Int32Rect(0, 0, image.DummyImage.PixelWidth, image.DummyImage.PixelHeight), pixelData, image.DummyImage.PixelWidth * bytesPerPixel, 0);

            applied = true;
        }
    }
    // ----

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
    }
    public class GrayScale : IChange, IFilterChange {
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

            applied = true;
        }
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
