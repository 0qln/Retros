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
using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics;
using System.Threading;

namespace Retros {
    public class ImageEditor {
        private Queue<IChange> changes = new();


        public ImageEditor() {

        }

        public void ApplyChange() {
            if (changes.Count <= 0) return;

            IChange change = changes.Dequeue();
            WorkStation.WorkstationImage.AddTaskToQueue(change.Apply);
        }

        public void AddChange(IChange change) {
            changes.Enqueue(change);
        }

        public void ApplyAllChanges() {
            while (changes.Count > 0) {
                IChange change = changes.Dequeue();
                WorkStation.WorkstationImage.AddTaskToQueue(change.Apply);
            }
        }
    }

    namespace ImageEditing {
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

        public class GrayScale : IChange {
            private Bitmap bitmap;
            private byte filterIntensity;

            public GrayScale(Bitmap bitmap, double filterIntensity = 1) {
                this.bitmap = bitmap;
                this.filterIntensity = (byte)filterIntensity;
            }

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
        }


    }

    public interface IChange {
        public void Apply();
    }

    
}
