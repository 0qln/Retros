using Accessibility;
using DebugLibrary.Benchmark;
using Retros.Program.DisplaySystem;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Retros.Program.Workstation.Changes
{

    public class OnlyRedChannel : FilterBase<OnlyRedChannel>, IFilter
    {
        public void Generate(WriteableBitmap writeableBitmap)
        {
            int bytesPerPixel = (writeableBitmap.Format.BitsPerPixel + 7) / 8;
            int pixelHeight = writeableBitmap.PixelHeight;
            int pixelWidth = writeableBitmap.PixelWidth;
            int stride = writeableBitmap.BackBufferStride;

            writeableBitmap.Lock();

            try
            {
                unsafe
                {
                    IntPtr bufferPtr = writeableBitmap.BackBuffer;
                    byte* pixelData = (byte*)bufferPtr;

                    Parallel.For(0, pixelHeight, y =>
                    {
                        byte* row = pixelData + y * stride;
                        Parallel.For(0, pixelWidth, x =>
                        {
                            int index = (y * pixelWidth + x) * bytesPerPixel;
                            pixelData[index + 1] = (byte)(pixelData[index + 1] * (1 - _filterIntensity));
                            pixelData[index + 0] = (byte)(pixelData[index + 0] * (1 - _filterIntensity));
                        });
                    });
                }
            }
            finally
            {
                writeableBitmap.Unlock();
            }

            _applied = true;
        }
    }
    public class OnlyGreenChannel : FilterBase<OnlyGreenChannel>, IFilter
    {
        public void Generate(WriteableBitmap writeableBitmap)
        {
            int bytesPerPixel = (writeableBitmap.Format.BitsPerPixel + 7) / 8;
            int pixelHeight = writeableBitmap.PixelHeight;
            int pixelWidth = writeableBitmap.PixelWidth;
            int stride = writeableBitmap.BackBufferStride;

            writeableBitmap.Lock();

            try
            {
                unsafe
                {
                    IntPtr bufferPtr = writeableBitmap.BackBuffer;
                    byte* pixelData = (byte*)bufferPtr;

                    Parallel.For(0, pixelHeight, y =>
                    {
                        byte* row = pixelData + y * stride;
                        Parallel.For(0, pixelWidth, x =>
                        {
                            int index = (y * pixelWidth + x) * bytesPerPixel;
                            pixelData[index + 2] = (byte)(pixelData[index + 2] * (1 - _filterIntensity));
                            pixelData[index + 0] = (byte)(pixelData[index + 0] * (1 - _filterIntensity));
                        });
                    });
                }
            }
            finally
            {
                writeableBitmap.Unlock();
            }

            _applied = true;
        }
    }
    public class OnlyBlueChannel : FilterBase<OnlyBlueChannel>, IFilter
    {
        public void Generate(WriteableBitmap writeableBitmap)
        {
            int bytesPerPixel = (writeableBitmap.Format.BitsPerPixel + 7) / 8;
            int pixelHeight = writeableBitmap.PixelHeight;
            int pixelWidth = writeableBitmap.PixelWidth;
            int stride = writeableBitmap.BackBufferStride;

            writeableBitmap.Lock();

            try
            {
                unsafe
                {
                    IntPtr bufferPtr = writeableBitmap.BackBuffer;
                    byte* pixelData = (byte*)bufferPtr;

                    Parallel.For(0, pixelHeight, y =>
                    {
                        byte* row = pixelData + y * stride;
                        Parallel.For(0, pixelWidth, x =>
                        {
                            int index = (y * pixelWidth + x) * bytesPerPixel;
                            pixelData[index + 2] = (byte)(pixelData[index + 2] * (1 - _filterIntensity));
                            pixelData[index + 1] = (byte)(pixelData[index + 1] * (1 - _filterIntensity));
                        });
                    });
                }
            }
            finally
            {
                writeableBitmap.Unlock();
            }

            _applied = true;
        }
    }
    public class TestBlue : FilterBase<TestBlue>, IFilter
    {
        public void Generate(WriteableBitmap writeableBitmap)
        {
            int bytesPerPixel = (writeableBitmap.Format.BitsPerPixel + 7) / 8;
            int pixelHeight = writeableBitmap.PixelHeight;
            int pixelWidth = writeableBitmap.PixelWidth;
            int stride = writeableBitmap.BackBufferStride;

            writeableBitmap.Lock();

            try
            {
                unsafe
                {
                    IntPtr bufferPtr = writeableBitmap.BackBuffer;
                    byte* pixelData = (byte*)bufferPtr;

                    Parallel.For(0, pixelHeight, y =>
                    {
                        byte* row = pixelData + y * stride;
                        Parallel.For(0, pixelWidth, x =>
                        {
                            int index = x * bytesPerPixel;
                            row[index + 0] = (byte)(255 * (1 - _filterIntensity));
                        });
                    });
                }
            }
            finally
            {
                writeableBitmap.Unlock();
            }

            _applied = true;
        }
    }
    public class GrayScale : FilterBase<GrayScale>, IFilter
    {
        public void Generate(WriteableBitmap writeableBitmap)
        {
            int bytesPerPixel = (writeableBitmap.Format.BitsPerPixel + 7) / 8;
            int pixelHeight = writeableBitmap.PixelHeight;
            int pixelWidth = writeableBitmap.PixelWidth;
            int stride = writeableBitmap.BackBufferStride;

            // Lock the WriteableBitmap to get a reference to its pixel buffer
            writeableBitmap.Lock();

            try
            {
                unsafe
                {
                    // Get a pointer to the pixel buffer
                    IntPtr bufferPtr = writeableBitmap.BackBuffer;
                    byte* pixelData = (byte*)bufferPtr;

                    // Modify the pixel values directly in the pixel buffer
                    Parallel.For(0, pixelHeight, y =>
                    {
                        byte* row = pixelData + y * stride;
                        Parallel.For(0, pixelWidth, x =>
                        {
                            int index = x * bytesPerPixel;

                            byte r = row[index + 2];
                            byte g = row[index + 1];
                            byte b = row[index + 0];
                            byte gray = (byte)(0.299 * r + 0.587 * g + 0.114 * b);  // (`0.299`, `0.587`, `0.114`) is ITU-R BT.709 standard

                            row[index + 2] = (byte)(gray * _filterIntensity + r * (1 - _filterIntensity));     // Red
                            row[index + 1] = (byte)(gray * _filterIntensity + g * (1 - _filterIntensity));     // Green
                            row[index + 0] = (byte)(gray * _filterIntensity + b * (1 - _filterIntensity));     // Blue
                        });
                    });

                }
            }
            finally
            {
                // Unlock the WriteableBitmap to release the pixel buffer
                writeableBitmap.Unlock();
            }

            _applied = true;
        }
    }

    public enum SortBy
    {
        Hue,
        Luminance,
        Saturation,
        BrightnessAvarage,
        BrightnessGreen,
        BrightnessBlue,
        BrightnessRed,
    }
    public class PixelSorter : FilterBase<PixelSorter>, IFilter
    {
        public SortBy SortBy { get; set; } = SortBy.BrightnessAvarage;
        public Orientation Orientation { get; set; } = Orientation.Horizontal;
        public SortDirection Direction { get; set; } = SortDirection.Ascending;
        public int LowThreshhold { get; set; } = 50;
        public int HighThreshhold { get; set; } = 200;


        public void Generate(WriteableBitmap writeableBitmap)
        {
            if (Orientation == Orientation.Horizontal)
            {

                GenerateHorizontal(writeableBitmap);

            }
            else
            {

                GenerateVertical(writeableBitmap);

            }
        }


        private unsafe void GenerateHorizontal(WriteableBitmap writeableBitmap)
        {
            int bytesPerPixel = (writeableBitmap.Format.BitsPerPixel + 7) / 8;
            int pixelHeight = writeableBitmap.PixelHeight;
            int pixelWidth = writeableBitmap.PixelWidth;
            int stride = writeableBitmap.BackBufferStride;

            // Lock the WriteableBitmap to get a reference to its pixel buffer
            writeableBitmap.Lock();

            try
            {

                IntPtr bufferPtr = writeableBitmap.BackBuffer;
                byte* pixelData = (byte*)bufferPtr;

                Parallel.For(0, pixelHeight, y =>
                {
                    byte* row = pixelData + y * stride;

                    List<(byte, byte, byte)> pixels = new();

                    for (int x = 0; x < pixelWidth; x++)
                    {
                        int index = x * bytesPerPixel;
                        pixels.Add((row[index + 0], row[index + 1], row[index + 2]));
                    }

                    pixels.Sort(Compare/*Choose from the SortBy variable*/);

                    int i = 0;
                    for (int x = 0; x < pixelWidth; x++)
                    {
                        int index = x * bytesPerPixel;
                        row[index + 0] = pixels[i].Item1;
                        row[index + 1] = pixels[i].Item2;
                        row[index + 2] = pixels[i].Item3;
                        i++;
                    }
                });

            }
            finally
            {
                // Unlock the WriteableBitmap to release the pixel buffer
                writeableBitmap.Unlock();
            }

            _applied = true;
        }
        private int Compare((byte, byte, byte) a, (byte, byte, byte) b)
        {
            if (Direction == SortDirection.Ascending)
            {
                return CompareSwitch(a, b);
            }
            else
            {
                return -1 * CompareSwitch(a, b);
            }
        }
        private int CompareSwitch((byte, byte, byte) a, (byte, byte, byte) b)
        {
            switch (this.SortBy)
            {
                case SortBy.Hue: return Hue(a) - Hue(b);
                case SortBy.Saturation: return Sat(a) - Sat(b);
                case SortBy.Luminance: return Lum(a) - Lum(b);
                case SortBy.BrightnessAvarage: return Avg(a) - Avg(b);
                case SortBy.BrightnessRed: return a.Item3 - b.Item3;
                case SortBy.BrightnessGreen: return a.Item2 - b.Item2;
                case SortBy.BrightnessBlue: return a.Item1 - b.Item1;

                default: return a.CompareTo(b);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int Hue((byte, byte, byte) bgr)
        {
            double max = Max(bgr);
            double min = Min(bgr);
            double delta = (max-min)/255.0;

            if (delta == 0) return 0;

            if (max == bgr.Item3)
            {
                //red
                //(g-b)/delta % 6
                return (int)((((bgr.Item2/255.0 - bgr.Item1/255.0) / delta) % 6) * 60);
            }
            else if (max == bgr.Item2)
            {
                //green
                //(b-r)/delta + 2
                return (int)(((bgr.Item1 / 255.0 - bgr.Item3 / 255.0) / delta + 2) * 60);
            }
            else
            {
                //blue
                //(r-g)/delta + 4
                return (int)(((bgr.Item3 / 255.0 - bgr.Item2 / 255.0) / delta + 4) * 60);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int Sat((byte, byte, byte) bgr)
        {
            decimal min = Min(bgr)/(decimal)255.0;
            decimal max = Max(bgr)/(decimal)255.0;
            if (min == max) return 0;

            decimal delta = max - min;
            decimal lum = min + max;

            return (int)(100000*(delta / (1-Math.Abs(lum - 1))));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int Lum((byte, byte, byte) bgr)
        {
            return (Min(bgr) + Max(bgr));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)] private byte Max((byte, byte, byte) bgr) => Max(bgr.Item1, Max(bgr.Item2, bgr.Item3));
        [MethodImpl(MethodImplOptions.AggressiveInlining)] private byte Max(byte a, byte b) => (a > b) ? a : b;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] private byte Min((byte, byte, byte) bgr) => Min(bgr.Item1, Min(bgr.Item2, bgr.Item3));
        [MethodImpl(MethodImplOptions.AggressiveInlining)] private byte Min(byte a, byte b) => (a < b) ? a : b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)] private int Avg((byte, byte, byte) bgr) => (bgr.Item1 + bgr.Item2 + bgr.Item3) / 3;


        private unsafe void GenerateVertical(WriteableBitmap writeableBitmap)
        {
            int bytesPerPixel = (writeableBitmap.Format.BitsPerPixel + 7) / 8;
            int pixelHeight = writeableBitmap.PixelHeight;
            int pixelWidth = writeableBitmap.PixelWidth;
            int stride = writeableBitmap.BackBufferStride;

            // Lock the WriteableBitmap to get a reference to its pixel buffer
            writeableBitmap.Lock();

            try
            {
                IntPtr bufferPtr = writeableBitmap.BackBuffer;
                byte* pixelData = (byte*)bufferPtr;

                for (int x = 0; x < pixelWidth; x++)
                {
                    byte* column = pixelData + x * bytesPerPixel;
                    List<(byte, byte, byte)> columnPixels = new();

                    // aquire the column
                    for (int y = 0; y < pixelHeight; y++)
                    {
                        int index = y * stride;
                        columnPixels.Add((column[index + 0], column[index + 1], column[index + 2]));
                    }

                    columnPixels.Sort(Compare);

                    // write the column to the buffer
                    int i = 0;
                    for (int y = 0; y < pixelHeight; y++)
                    {
                        int index = y * stride;
                        column[index + 0] = columnPixels[i].Item1;
                        column[index + 1] = columnPixels[i].Item2;
                        column[index + 2] = columnPixels[i].Item3;
                        i++;
                    }
                }
            }
            finally
            {
                // Unlock the WriteableBitmap to release the pixel buffer
                writeableBitmap.Unlock();
            }

            _applied = true;
        }


        public override IChange Clone()
        {
            return base.Clone();
        }
    }

}
