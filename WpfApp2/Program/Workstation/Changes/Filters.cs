using Accessibility;
using DebugLibrary.Benchmark;
using Retros.Program.DisplaySystem;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Drawing;
using System.Linq;
using System.Reflection;
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

    public class PixelSorter : FilterBase<PixelSorter>, IFilter
    {
        private Orientation _orientation = Orientation.Horizontal;
        private SortDirection _direction = SortDirection.Ascending;
        private int _lowThreshhold = 50, _highThreshhold = 200;


        public void Generate(WriteableBitmap writeableBitmap)
        {

            if (_orientation == Orientation.Horizontal)
            {
                if (_direction == SortDirection.Ascending)
                {
                    GenerateHorizontalAscending(writeableBitmap);
                }
                else
                {

                }
            }
            else
            {
                if (_direction == SortDirection.Ascending)
                {

                }
                else
                {

                }
            }
        }

        private void GenerateHorizontalAscending(WriteableBitmap writeableBitmap)
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
                            byte r = row[index + 2];
                            byte g = row[index + 1];
                            byte b = row[index + 0];

                            pixelData[index + 2] = (byte)(r * ((byte)(_filterIntensity * 255)));
                            pixelData[index + 1] = (byte)(g * ((byte)(_filterIntensity * 255)));
                            pixelData[index + 0] = (byte)(b * ((byte)(_filterIntensity * 255)));
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



        public override IChange Clone()
        {
            return base.Clone();
        }
    }

}
