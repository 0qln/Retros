using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Drawing;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Interop;
using System.Windows;
using System.IO;
using Color = System.Drawing.Color;
using Retros.ClientWorkStation;
using System.Diagnostics;
using System.Drawing.Imaging;
using COnsole = Debugger.Console;
using DebugLibrary.Benchmarks;

namespace Retros {
    public class ImageEditor {
        private Queue<IChange> changes = new();


        public ImageEditor() {

        }


        public void AddChange(IChange change) {
            changes.Enqueue(change);
        }

        public void ApplyAllChanges() {

            while (changes.Count > 0) {
                using (IChange change = changes.Dequeue()) {
                    change.Apply();
                }

            }
        }
    }

    namespace ImageEditing {
        public class PixelSort : IChange, IDisposable {
            private Bitmap image;
            private bool disposed = false;

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


            protected virtual void Dispose(bool disposing) {
                if (!disposed) {
                    if (disposing) {
                        // Dispose of managed resources (if any)

                    }

                    // Dispose of unmanaged resources (if any)
                    disposed = true;
                }
            }

            public void Dispose() {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        public class GrayScale : IChange, IDisposable {
            private Bitmap bitmap;
            private bool disposed = false;

            public GrayScale(Bitmap bitmap) {
                this.bitmap = bitmap;
            }

            public void Apply() {
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                int regionHeight = bitmap.Height / Environment.ProcessorCount;
                Parallel.For(0, Environment.ProcessorCount, i =>
                {
                    int startY = i * regionHeight;
                    int endY = (i == Environment.ProcessorCount - 1) ? bitmap.Height : startY + regionHeight;
                    unsafe {
                        byte* ptr = (byte*)bitmapData.Scan0.ToPointer() + startY * bitmapData.Stride;
                        for (int y = startY; y < endY; y++) {
                            for (int x = 0; x < bitmapData.Width; x++) {
                                byte grayScale = (byte)((ptr[2] + ptr[1] + ptr[0]) / 3);
                                ptr[0] = grayScale;
                                ptr[1] = grayScale;
                                ptr[2] = grayScale;
                                ptr += 4;
                            }
                        }
                    }
                });
                bitmap.UnlockBits(bitmapData);
            }

            protected virtual void Dispose(bool disposing) {
                if (!disposed) {
                    if (disposing) {
                        // Dispose of managed resources (if any)
                        
                    }

                    // Dispose of unmanaged resources (if any)
                    disposed = true;
                }
            }

            public void Dispose() {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

    }

    public interface IChange : IDisposable {
        public void Apply();
    }

    
}
