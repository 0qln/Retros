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
        BrightnessAvarage,
        BrightnessGreen,
        BrightnessBlue,
        BrightnessRed,
        Hue,
        Luminance,
        Saturation,
    }
    public class PixelSorter : FilterBase<PixelSorter>, IFilter
    {
        private IComparer<(byte,byte,byte)> _comparer;

        public SortBy SortBy { get; set; } = SortBy.BrightnessAvarage;
        public Orientation Orientation { get; set; } = Orientation.Horizontal;
        public SortDirection Direction { get; set; } = SortDirection.Ascending;
        public float LowThreshhold { get; set; } = float.MinValue;
        public float HighThreshhold { get; set; } = float.MaxValue;
        public int MaxPixelSpanLength {get; set; } = 300;

        
        public PixelSorter() {
            _comparer = new Comparer(this);
        }


        public void Generate(WriteableBitmap writeableBitmap)
        {
            if (Orientation == Orientation.Horizontal)
            {
                DebugLibrary.Console.Log("Started sorting...");
 

//                var array = new byte[100];
//                var arraySpan = new Span<byte>(array);
//
//                byte data = 0;
//                for (int ctr =arraySpan.Length-1; ctr >= 0; ctr--)
//                    arraySpan[ctr] = data++;
//
//                foreach(var num in arraySpan)  
//                    DebugLibrary.Console.Log(num);
//
//                arraySpan.QuickSort();
//
//                foreach(var num in array)  
//                    DebugLibrary.Console.Log(num);


                DebugLibrary.Console.Log("GenerateHorizontal_Parallel_0: "+DebugLibrary.Benchmark.Measure.Execute(()=>GenerateHorizontal_Parallel_0(writeableBitmap)).ElapsedMilliseconds);
                DebugLibrary.Console.Log("GenerateHorizontal_Parallel_1: "+DebugLibrary.Benchmark.Measure.Execute(()=>GenerateHorizontal_Parallel_1(writeableBitmap)).ElapsedMilliseconds);

                //DebugLibrary.Console.Log("GenerateHorizontal_TaskPool: "+DebugLibrary.Benchmark.Measure.Execute(()=>GenerateHorizontal_TaskPool(writeableBitmap)).ElapsedMilliseconds);
                
                DebugLibrary.Console.Log("Finished sorting");
            }
            else
            {
                //GenerateVertical(writeableBitmap);
                DebugLibrary.Console.Log("Disabled");
            }
        }
        private unsafe void GenerateHorizontal_TaskPool(WriteableBitmap writeableBitmap)
        {
            int bytesPerPixel = (writeableBitmap.Format.BitsPerPixel + 7) / 8;
            int pixelHeight = writeableBitmap.PixelHeight;
            int pixelWidth = writeableBitmap.PixelWidth;
            int stride = writeableBitmap.BackBufferStride;

            writeableBitmap.Lock();

            try
            {
                IntPtr bufferPtr = writeableBitmap.BackBuffer;
                byte* pixelData = (byte*)bufferPtr;

                Task[] tasks = new Task[pixelHeight];
                for (int i = 0; i < pixelHeight; i++)
                {
                    int y = i;
                    tasks[i] = new Task(() => GenerateRow_v1(y, stride, pixelWidth, bytesPerPixel, pixelData, writeableBitmap));
                    tasks[i].Start();
                }

                for (int i = 0; i < tasks.Length;) {
                    if (tasks[i].IsCompleted) {
                        i++;
                    }
                }
            }
            finally
            {
                writeableBitmap.Unlock();
            }

            _applied = true;
        }
        private unsafe void GenerateRow_v0(int y, int stride, int pixelWidth, int bytesPerPixel, byte* pixelData, WriteableBitmap writeableBitmap) {
            byte* row = pixelData + y * stride;
            //iterate the row for subarrays
            int startIndex = 0;
            int endIndex = 0;
            float pixelEval;
            while (startIndex < pixelWidth * bytesPerPixel && startIndex - endIndex < MaxPixelSpanLength)
            {
                //find subarray
                pixelEval = SelectedProperty((row[startIndex + 0], row[startIndex + 1], row[startIndex + 2]));
                while (pixelEval < LowThreshhold && pixelEval > HighThreshhold && startIndex + bytesPerPixel < pixelWidth * bytesPerPixel)
                {
                    startIndex += bytesPerPixel;
                    pixelEval = SelectedProperty((row[startIndex + 0], row[startIndex + 1], row[startIndex + 2]));
                }
                endIndex = startIndex;
                do
                {
                    endIndex += bytesPerPixel;
                    pixelEval = SelectedProperty((row[endIndex + 0], row[endIndex + 1], row[endIndex + 2]));
                }
                while (pixelEval >= LowThreshhold && pixelEval <= HighThreshhold  && endIndex + bytesPerPixel < pixelWidth * bytesPerPixel);

                //get subarray
                (byte, byte, byte)[] pixels = new (byte, byte, byte)[endIndex/bytesPerPixel - startIndex/bytesPerPixel];
                for (int i = 0; i < pixels.Length; i++)
                {
                    pixels[i] = (row[startIndex + i*bytesPerPixel], row[startIndex + i*bytesPerPixel + 1], row[startIndex + i*bytesPerPixel + 2]);
                }

                //sort the subarray
                Array.Sort(pixels, Compare);

                //overwrite pixelData with sorted subarray
                for (int i = 0; i < pixels.Length; i++)
                {
                    row[i*bytesPerPixel + 0 + startIndex] = pixels[i].Item1;
                    row[i*bytesPerPixel + 1 + startIndex] = pixels[i].Item2;
                    row[i*bytesPerPixel + 2 + startIndex] = pixels[i].Item3;
                }

                startIndex += bytesPerPixel;
            }
        }
        private unsafe void GenerateRow_v1(int y, int stride, int pixelWidth, int bytesPerPixel, byte* pixelData, WriteableBitmap writeableBitmap) {
            byte* row = pixelData + y * stride;
            //iterate the row for subarrays
            int startIndex = 0;
            int endIndex = 0;
            float pixelEval;
            while (startIndex < pixelWidth * bytesPerPixel && startIndex - endIndex < MaxPixelSpanLength)
            {
                //find subarray
                pixelEval = SelectedProperty((row[startIndex + 0], row[startIndex + 1], row[startIndex + 2]));
                while (pixelEval < LowThreshhold && pixelEval > HighThreshhold && startIndex + bytesPerPixel < pixelWidth * bytesPerPixel)
                {
                    startIndex += bytesPerPixel;
                    pixelEval = SelectedProperty((row[startIndex + 0], row[startIndex + 1], row[startIndex + 2]));
                }
                endIndex = startIndex;
                do
                {
                    endIndex += bytesPerPixel;
                    pixelEval = SelectedProperty((row[endIndex + 0], row[endIndex + 1], row[endIndex + 2]));
                }
                while (pixelEval >= LowThreshhold && pixelEval <= HighThreshhold  && endIndex + bytesPerPixel < pixelWidth * bytesPerPixel);

                //copy subarray
                var span = new Span<(byte,byte,byte)>(row + startIndex, endIndex/bytesPerPixel - startIndex/bytesPerPixel);

                //sort the subarray
                span.Sort(_comparer);

                startIndex += bytesPerPixel;
            }
        } 


        public class Comparer : IComparer<(byte,byte,byte)> {
            private readonly PixelSorter _sorter;

            public Comparer(PixelSorter sorter) {
                _sorter = sorter;
            }

            public int Compare((byte,byte,byte) a, (byte,byte,byte) b) {
                int dir = _sorter.Direction == SortDirection.Ascending ? 1 : -1;
                float selA;
                float selB;
                _sorter.SelectedProperties(a, b, out selA, out selB);

                if (selA > selB) return 1 * dir;
                else if (selA < selB) return -1 * dir;
                else return 0; 
            }
        }


        private unsafe void GenerateHorizontal_Parallel_1(WriteableBitmap writeableBitmap)
        {
            int bytesPerPixel = (writeableBitmap.Format.BitsPerPixel + 7) / 8;
            int pixelHeight = writeableBitmap.PixelHeight;
            int pixelWidth = writeableBitmap.PixelWidth;
            int stride = writeableBitmap.BackBufferStride;

            writeableBitmap.Lock();

            try
            {
                IntPtr bufferPtr = writeableBitmap.BackBuffer;
                byte* pixelData = (byte*)bufferPtr;


                Parallel.For(0, pixelHeight, y =>
                {
                    GenerateRow_v1(y, stride, pixelWidth, bytesPerPixel, pixelData, writeableBitmap);
                });

            }
            finally
            {
                writeableBitmap.Unlock();
            }

            _applied = true;
        }


        private unsafe void GenerateHorizontal_Parallel_0(WriteableBitmap writeableBitmap)
        {
            int bytesPerPixel = (writeableBitmap.Format.BitsPerPixel + 7) / 8;
            int pixelHeight = writeableBitmap.PixelHeight;
            int pixelWidth = writeableBitmap.PixelWidth;
            int stride = writeableBitmap.BackBufferStride;

            writeableBitmap.Lock();

            try
            {
                IntPtr bufferPtr = writeableBitmap.BackBuffer;
                byte* pixelData = (byte*)bufferPtr;


                Parallel.For(0, pixelHeight, y =>
                {
                    GenerateRow_v0(y, stride, pixelWidth, bytesPerPixel, pixelData, writeableBitmap);
                });

            }
            finally
            {
                writeableBitmap.Unlock();
            }

            _applied = true;
        }

        private unsafe void GenerateVertical(WriteableBitmap writeableBitmap)
        {
            int bytesPerPixel = (writeableBitmap.Format.BitsPerPixel + 7) / 8;
            int pixelHeight = writeableBitmap.PixelHeight;
            int pixelWidth = writeableBitmap.PixelWidth;
            int stride = writeableBitmap.BackBufferStride;

            writeableBitmap.Lock();

            try
            {
                IntPtr bufferPtr = writeableBitmap.BackBuffer;
                byte* pixelData = (byte*)bufferPtr;

                Parallel.For(0, pixelWidth, x =>
                        {
                        byte* column = pixelData + x * bytesPerPixel;
                        //iterate the row for subarrays
                        int startIndex = 0;
                        int endIndex = 0;
                        float pixelEval;
                        while (startIndex < pixelHeight * stride)
                        {
                        //find subarray
                        pixelEval = SelectedProperty((column[startIndex + 0], column[startIndex + 1], column[startIndex + 2]));
                        while (pixelEval < LowThreshhold && pixelEval > HighThreshhold && startIndex + stride < pixelHeight * stride)
                        {
                        startIndex += stride;
                        pixelEval = SelectedProperty((column[startIndex + 0], column[startIndex + 1], column[startIndex + 2]));
                        }
                        endIndex = startIndex;
                        do
                        {
                        endIndex += stride;
                        pixelEval = SelectedProperty((column[endIndex + 0], column[endIndex + 1], column[endIndex + 2]));
                        }
                        while (pixelEval >= LowThreshhold && pixelEval <= HighThreshhold  && endIndex + stride < pixelHeight * stride);

                        //get subarray
                        (byte, byte, byte)[] pixels = new (byte, byte, byte)[endIndex/stride - startIndex/stride];
                        for (int i = 0; i < pixels.Length; i++)
                        {
                            pixels[i] = (column[startIndex + i*stride], column[startIndex + i*stride+ 1], column[startIndex + i*stride+ 2]);
                        }

                        //sort the subarray
                        Array.Sort(pixels, Compare);

                        //overwrite pixelData with sorted subarray
                        for (int i = 0; i < pixels.Length; i++)
                        {
                            column[i*stride + 0 + startIndex] = pixels[i].Item1;
                            column[i*stride + 1 + startIndex] = pixels[i].Item2;
                            column[i*stride + 2 + startIndex] = pixels[i].Item3;
                        }

                        startIndex += stride;
                        }
                        });


            }
            finally
            {
                writeableBitmap.Unlock();
            }

            _applied = true;
        }

        public int Compare((byte, byte, byte) a, (byte, byte, byte) b)
        {
            int dir = Direction == SortDirection.Ascending ? 1 : -1;
            float selA;
            float selB;
            SelectedProperties(a, b, out selA, out selB);

            if (selA > selB)
                return 1 * dir;
            else if (selA < selB)
                return -1 * dir;
            else
                return 0; 
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SelectedProperties((byte,byte,byte) a, (byte,byte,byte) b, out float selA, out float selB)
        {
            switch (this.SortBy)
            {
                case SortBy.Hue: selA = Hue(a); selB = Hue(b); break;
                case SortBy.Saturation: selA = Sat(a); selB = Sat(b); break;
                case SortBy.Luminance: selA = Lum(b); selB = Lum(a); break;
                case SortBy.BrightnessAvarage: selA = Avg(a); selB = Avg(b); break;
                case SortBy.BrightnessRed: selA = a.Item3; selB = b.Item3; break;
                case SortBy.BrightnessGreen: selA = a.Item2; selB = b.Item2; break;
                case SortBy.BrightnessBlue: selA = a.Item1; selB = b.Item1; break;
                default: selA = 0; selB = 0; break;
            }
        }
        private float SelectedProperty((byte, byte, byte) bgr)
        {
            switch (this.SortBy)
            {
                case SortBy.Hue: return Hue(bgr);
                case SortBy.Saturation: return Sat(bgr);
                case SortBy.Luminance: return Lum(bgr);
                case SortBy.BrightnessAvarage: return Avg(bgr);
                case SortBy.BrightnessRed: return bgr.Item3;
                case SortBy.BrightnessGreen: return bgr.Item2;
                case SortBy.BrightnessBlue: return bgr.Item1;
                default: return 0;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float Hue((byte, byte, byte) bgr)
        {
            byte max = Max(bgr);
            byte min = Min(bgr);
            float delta = (max - min) / 255.0f;

            if (delta == 0) return 0;

            if (max == bgr.Item3)
            {
                //red
                //(g-b)/delta % 6
                return (bgr.Item2 / 255.0f - bgr.Item1 / 255.0f) / delta % 6 * 60;
            }
            else if (max == bgr.Item2)
            {
                //green
                //(b-r)/delta + 2
                return ((bgr.Item1/ 255.0f - bgr.Item3/ 255.0f) / delta + 2) * 60;
            }
            else
            {
                //blue
                //(r-g)/delta + 4
                return ((bgr.Item3/ 255.0f - bgr.Item2/ 255.0f) / delta + 4) * 60;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float Sat((byte, byte, byte) bgr)
        {
            byte min = Min(bgr);
            byte max = Max(bgr);
            if (min == max) return 0;

            float delta = max - min;
            float lum = min + max;

            return delta / (1.0f - MathF.Abs(lum - 1.0f));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float Lum((byte, byte, byte) bgr)
        {
            return Min(bgr) + Max(bgr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)] private byte Max((byte, byte, byte) bgr) => Max(bgr.Item1, Max(bgr.Item2, bgr.Item3));
        [MethodImpl(MethodImplOptions.AggressiveInlining)] private byte Max(byte a, byte b) => (a > b) ? a : b;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] private byte Min((byte, byte, byte) bgr) => Min(bgr.Item1, Min(bgr.Item2, bgr.Item3));
        [MethodImpl(MethodImplOptions.AggressiveInlining)] private byte Min(byte a, byte b) => (a < b) ? a : b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)] private float Avg((byte, byte, byte) bgr) => (bgr.Item1 + bgr.Item2 + bgr.Item3) / 3;



        public override IChange Clone()
        {
            return base.Clone();
        }
    }


}
