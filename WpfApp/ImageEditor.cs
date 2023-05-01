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
                IChange change = changes.Dequeue();
                change.Apply();
            }
        }
    }

    public static class ImageConverter {
        public static System.Windows.Controls.Image ConvertBitmapToImage(Bitmap bitmap) {
            System.Windows.Controls.Image image = new();

            using (MemoryStream memory = new MemoryStream()) {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                image.Source = bitmapImage;
            }

            image.Width = bitmap.Width;
            image.Height = bitmap.Height;

            return image;
        }

        public static Bitmap ConvertToBitmap(System.Windows.Controls.Image wpfImage) {
            Bitmap bitmap;
            using (MemoryStream stream = new MemoryStream()) {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)wpfImage.Source));
                encoder.Save(stream);
                bitmap = new Bitmap(stream);
            }
            return bitmap;
        }
    }

    namespace ImageEditing {
        public class PixelSort : IChange {
            private Bitmap image;

            /// <summary> The upper and lower bounds of pixels that will be affected </summary>
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
            public void Apply() {
                Bitmap originalImage = ImageConverter.ConvertToBitmap(ClientWorkStation.WorkstationImage.Image);
                Bitmap newImage = new Bitmap(originalImage.Width, originalImage.Height);

                for (int x = 0; x < originalImage.Width; x++) {
                    for (int y = 0; y < originalImage.Height; y++) {
                        Color pixelColor = originalImage.GetPixel(x, y);
                        int grayScale = (int)((pixelColor.R + pixelColor.G + pixelColor.B) / 3.0);
                        Color newColor = Color.FromArgb(pixelColor.A, grayScale, grayScale, grayScale);
                        newImage.SetPixel(x, y, newColor);
                    }
                }

                ClientWorkStation.WorkstationImage.Image = ImageConverter.ConvertBitmapToImage(newImage);
            }
        }

    }

    public interface IChange {
        public void Apply();
    }

    
}
