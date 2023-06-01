using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Retros.ProgramWindow {
    public static class WriteableBitmapExtensions {
        public delegate byte PixelChannekChangeHandler(byte r, byte g, byte b, int x, int y);
        public static void IteratePixels(this WriteableBitmap bitmap, PixelChannekChangeHandler changeR, PixelChannekChangeHandler changeG, PixelChannekChangeHandler changeB) {
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
                            row[index + 2] = changeR(row[index + 2], row[index + 1], row[index + 0], x, y);
                            row[index + 1] = changeG(row[index + 2], row[index + 1], row[index + 0], x, y);
                            row[index + 0] = changeB(row[index + 2], row[index + 1], row[index + 0], x, y);
                        });
                    });

                }
            }
            finally {
                // Unlock the WriteableBitmap to release the pixel buffer
                bitmap.Unlock();
            }
        }
    }
}
