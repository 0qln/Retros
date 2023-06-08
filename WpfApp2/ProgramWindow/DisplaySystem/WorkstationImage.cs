using SharpDX.Multimedia;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Utillities;
using Utillities.Wpf;
using Retros.Settings;
using System.Windows.Automation;

namespace Retros.ProgramWindow.DisplaySystem {
    public partial class WorkstationImage : IFrameworkElement {
        public FrameworkElement FrameworkElement => pageFrame;
        private WriteableBitmap resizedSourceBitmap;
        private Uri source;
        private ChangeHistory changeHistory = new();
        private ImageChangeManager filterManager;
        private DispatcherTimer actionTimer = new();
        private Queue<Action> actionQueue = new();
        private Frame pageFrame = new();

        /// <summary>Used to display the image.</summary>
        private readonly WorkstationImagePage Page = new();

        /// <summary>Used to display the current image.</summary>
        public Image CurrentImage => Page.Image;

        /// <summary>Chaches the source image.</summary>
        public Uri Source => source;
        
        /// <summary>Acts as the source image for the runtime image.</summary>
        public WriteableBitmap ResizedSourceBitmap => resizedSourceBitmap;

        /// <summary>Works as a computation/backend image for the filters to work on.</summary>
        public WriteableBitmap DummyImage { get; set; }

        /// <summary>Main Grid</summary>
        public Grid Grid => Page.MainGrid;

        /// <summary>The FilterManager instance for this WorkstationImage.</summary>
        public ImageChangeManager GetFilterManager => filterManager;

        //TODO:
        /// <summary>Keeps track of all the changes that were applied to the image.</summary>
        public ChangeHistory History => changeHistory;



        private Thickness _margin = new Thickness(50);
        public Thickness ImageMargin {
            get {
                return _margin;
            }
            set {
                _margin = value;
                Page.Image.Margin = _margin;
            }
        }
        private List<float> dp_tValues = new(); /// caches the opacity curve
        private bool isCalculated = false;
        private static int smoothness = 1;
        public int InterpolationSmoothness {
            get => smoothness;
            set {
                smoothness = value;
                isCalculated = false;
            }
        }
        private static float totalInterpolationTime = 150; /// reccomendet to have this the same as the interval in which the filters are applied
        public float TotalInterpolationTime {
            get => totalInterpolationTime;
            set {
                totalInterpolationTime = value;
                isCalculated = false;
            }
        }
        private float interval = 6.94444444444f; /// time between ticks
        private bool enableStartboost = false;
        private float startBoost {
            get {
                if (enableStartboost) {
                    return smoothness;
                }
                else {
                    return 0;
                }
            }
        } /// recomendet for high smoothness (This approximates, will not work for very high values)
        private int imageCount = 1; /// used to set the newest images to the front
        public delegate float InterpolationFuntionHandler(float t);
        public InterpolationFuntionHandler InterpolationFuntion = (t) => ExtendedMath.RootStep(ExtendedMath.RootStep(t, smoothness, 0, totalInterpolationTime));




#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        public WorkstationImage(string path) {
            _init();

            source = new Uri(path);
            SetSourceImage(source);
        }
        public WorkstationImage() { 
            _init(); 
        }

        private void _init() {
            pageFrame.Content = Page;
            filterManager = new(this);
            StartUpdating();
        }
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.


        public void SetSourceImage(Uri source) {
            this.source = source;

            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            resizedSourceBitmap = ResizeWritableBitmap(new BitmapImage(source), (int) screenWidth, (int) screenHeight);

            Page.Image.Source = resizedSourceBitmap.Clone();
            DummyImage = new WriteableBitmap(resizedSourceBitmap);
        }

        public static WriteableBitmap ResizeWritableBitmap(BitmapImage originalBitmap, int newWidth, int newHeight) {
            DebugLibrary.Console.Log(newHeight +", "+ newHeight);

            // Check if resizing is required
            if (newWidth >= originalBitmap.PixelWidth && newHeight >= originalBitmap.PixelHeight) {
                return new WriteableBitmap(originalBitmap);
            }

            // Convert the original bitmap to a compatible pixel format (e.g., Pbgra32)
            FormatConvertedBitmap convertedBitmap = new FormatConvertedBitmap(originalBitmap, PixelFormats.Pbgra32, null, 0);

            // Calculate the aspect ratios
            double aspectRatioOriginal = (double)originalBitmap.PixelWidth / originalBitmap.PixelHeight;
            double aspectRatioNew = (double)newWidth / newHeight;

            DebugLibrary.Console.Log(aspectRatioOriginal + " -> " + aspectRatioNew);

            // Determine the scaling factor to use (based on the aspect ratios)
            double scale = aspectRatioNew >= aspectRatioOriginal ? (double)newHeight / originalBitmap.PixelHeight : (double)newWidth / originalBitmap.PixelWidth;

            // Calculate the new dimensions
            int scaledWidth = (int)Math.Round(originalBitmap.PixelWidth * scale);
            int scaledHeight = (int)Math.Round(originalBitmap.PixelHeight * scale);

            // Create a new RenderTargetBitmap with the scaled dimensions
            RenderTargetBitmap resizedRenderTarget = new RenderTargetBitmap(scaledWidth, scaledHeight, convertedBitmap.DpiX, convertedBitmap.DpiY, PixelFormats.Pbgra32);

            // Create a Transform to apply scaling
            ScaleTransform transform = new ScaleTransform(scale, scale);

            // Apply the scaling transform to a DrawingVisual
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen()) {
                drawingContext.PushTransform(transform);
                drawingContext.DrawImage(convertedBitmap, new Rect(0, 0, originalBitmap.PixelWidth, originalBitmap.PixelHeight));
            }

            // Render the DrawingVisual to the resized RenderTargetBitmap
            resizedRenderTarget.Render(drawingVisual);

            // Create a new WriteableBitmap with the resized RenderTargetBitmap
            WriteableBitmap resizedBitmap = new WriteableBitmap(resizedRenderTarget);

            DebugLibrary.Console.Log(originalBitmap.Width + ", " + originalBitmap.Height + ", " + originalBitmap.Width/ originalBitmap.Height);
            DebugLibrary.Console.Log(resizedBitmap.Width + ", " + resizedBitmap.Height + ", " + resizedBitmap.Width/ resizedBitmap.Height);

            return resizedBitmap;
        }

        public System.Drawing.Bitmap Render() {
            // Get the image Source
            WriteableBitmap writeableBitmap = 
                filterManager.ApplyChanges(
                    new WriteableBitmap(
                        new BitmapImage(source)));

            // Default the pixel format
            WriteableBitmap formattedBitmap = SwitchPixelFormat(writeableBitmap, System.Windows.Media.PixelFormats.Bgra32);

            // Create the bitmap
            System.Windows.Media.Imaging.BitmapSource bitmapSource = formattedBitmap;
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(
                bitmapSource.PixelWidth,
                bitmapSource.PixelHeight,
                GetSwappedPixelFormats()[bitmapSource.Format]);

            // Create the bitmapData
            System.Drawing.Imaging.BitmapData bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.WriteOnly,
                bitmap.PixelFormat);

            // Copy the bixels of the bitmapData to the bitmap
            bitmapSource.CopyPixels(
                new System.Windows.Int32Rect(0, 0, bitmapSource.PixelWidth, bitmapSource.PixelHeight),
                bitmapData.Scan0,
                bitmapData.Height * bitmapData.Stride,
                bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);
            
            return bitmap;
        }

        public static Image ToImage(System.Drawing.Bitmap bitmap) {
            BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return new Image { Source = bitmapSource };
        }

        public static WriteableBitmap SwitchPixelFormat(WriteableBitmap originalBitmap, System.Windows.Media.PixelFormat newPixelFormat) {
            // Convert WriteableBitmap to Bitmap
            System.Drawing.Bitmap bitmap;
            using (MemoryStream stream = new MemoryStream()) {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(originalBitmap));
                encoder.Save(stream);
                bitmap = new (stream);
            }

            // Create a new Bitmap with the desired pixel format
            System.Drawing.Bitmap newBitmap = new System.Drawing.Bitmap(bitmap.Width, bitmap.Height, GetEqualPixelFormat(newPixelFormat));

            // Copy pixel values from the original Bitmap to the new Bitmap
            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(newBitmap)) {
                graphics.DrawImage(bitmap, new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height));
            }

            // Convert Bitmap back to WriteableBitmap
            BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                newBitmap.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            WriteableBitmap newWriteableBitmap = new WriteableBitmap(bitmapSource);

            // Dispose the intermediary Bitmap
            bitmap.Dispose();
            newBitmap.Dispose();

            return newWriteableBitmap;
        }
        public static Dictionary<System.Drawing.Imaging.PixelFormat, System.Windows.Media.PixelFormat> GetEqualPixelFormats() {
            Dictionary<System.Drawing.Imaging.PixelFormat, System.Windows.Media.PixelFormat> equalFormats = new Dictionary<System.Drawing.Imaging.PixelFormat, System.Windows.Media.PixelFormat>();

            equalFormats[System.Drawing.Imaging.PixelFormat.Format1bppIndexed] = System.Windows.Media.PixelFormats.Indexed1;
            equalFormats[System.Drawing.Imaging.PixelFormat.Format4bppIndexed] = System.Windows.Media.PixelFormats.Indexed4;
            equalFormats[System.Drawing.Imaging.PixelFormat.Format8bppIndexed] = System.Windows.Media.PixelFormats.Indexed8;

            equalFormats[System.Drawing.Imaging.PixelFormat.Format16bppRgb555] = System.Windows.Media.PixelFormats.Bgr555;
            equalFormats[System.Drawing.Imaging.PixelFormat.Format16bppRgb565] = System.Windows.Media.PixelFormats.Bgr565;

            equalFormats[System.Drawing.Imaging.PixelFormat.Format24bppRgb] = System.Windows.Media.PixelFormats.Bgr24;
            equalFormats[System.Drawing.Imaging.PixelFormat.Format32bppRgb] = System.Windows.Media.PixelFormats.Bgr32;
            equalFormats[System.Drawing.Imaging.PixelFormat.Format48bppRgb] = System.Windows.Media.PixelFormats.Rgb48;

            equalFormats[System.Drawing.Imaging.PixelFormat.Format32bppArgb] = System.Windows.Media.PixelFormats.Bgra32;
            equalFormats[System.Drawing.Imaging.PixelFormat.Format64bppArgb] = System.Windows.Media.PixelFormats.Rgba64;

            equalFormats[System.Drawing.Imaging.PixelFormat.Format32bppPArgb] = System.Windows.Media.PixelFormats.Pbgra32;

            equalFormats[System.Drawing.Imaging.PixelFormat.Format16bppGrayScale] = System.Windows.Media.PixelFormats.Gray16;

            return equalFormats;
        }
        public static Dictionary<System.Windows.Media.PixelFormat, System.Drawing.Imaging.PixelFormat> GetSwappedPixelFormats() {
            Dictionary<System.Windows.Media.PixelFormat, System.Drawing.Imaging.PixelFormat> swappedFormats = new Dictionary<System.Windows.Media.PixelFormat, System.Drawing.Imaging.PixelFormat>();
            Dictionary<System.Drawing.Imaging.PixelFormat, System.Windows.Media.PixelFormat> originalFormats = GetEqualPixelFormats();
            foreach (var kvp in originalFormats) {
                swappedFormats[kvp.Value] = kvp.Key;
            }
            return swappedFormats;
        }
        public static System.Windows.Media.PixelFormat GetEqualPixelFormat(System.Drawing.Imaging.PixelFormat oldFormat) => GetEqualPixelFormats()[oldFormat];
        public static System.Drawing.Imaging.PixelFormat GetEqualPixelFormat(System.Windows.Media.PixelFormat oldFormat) => GetSwappedPixelFormats()[oldFormat];

        /* Goofy ahhhh resize
        public WriteableBitmap ResizeWritableBitmap(WriteableBitmap wBitmap, int reqWidth, int reqHeight) {
            int Stride = wBitmap.PixelWidth * ((wBitmap.Format.BitsPerPixel + 7) / 8);
            int NumPixels = Stride * wBitmap.PixelHeight;
            ushort[] ArrayOfPixels = new ushort[NumPixels];


            wBitmap.CopyPixels(ArrayOfPixels, Stride, 0);

            int OriWidth = (int)wBitmap.PixelWidth;
            int OriHeight = (int)wBitmap.PixelHeight;

            double nXFactor = (double)OriWidth / (double)reqWidth;
            double nYFactor = (double)OriHeight / (double)reqHeight;

            double fraction_x, fraction_y, one_minus_x, one_minus_y;
            int ceil_x, ceil_y, floor_x, floor_y;

            ushort pix1, pix2, pix3, pix4;
            int nStride = reqWidth * ((wBitmap.Format.BitsPerPixel + 7) / 8);
            int nNumPixels = reqWidth * reqHeight;
            ushort[] newArrayOfPixels = new ushort[nNumPixels];

            for (int y = 0; y < reqHeight; y++) {
                for (int x = 0; x < reqWidth; x++) {

                    floor_x = (int)Math.Floor(x * nXFactor);
                    floor_y = (int)Math.Floor(y * nYFactor);

                    ceil_x = floor_x + 1;
                    if (ceil_x >= OriWidth) ceil_x = floor_x;

                    ceil_y = floor_y + 1;
                    if (ceil_y >= OriHeight) ceil_y = floor_y;

                    fraction_x = x * nXFactor - floor_x;
                    fraction_y = y * nYFactor - floor_y;

                    one_minus_x = 1.0 - fraction_x;
                    one_minus_y = 1.0 - fraction_y;

                    pix1 = ArrayOfPixels[floor_x + floor_y * OriWidth];
                    pix2 = ArrayOfPixels[ceil_x + floor_y * OriWidth];
                    pix3 = ArrayOfPixels[floor_x + ceil_y * OriWidth];
                    pix4 = ArrayOfPixels[ceil_x + ceil_y * OriWidth];

                    ushort g1 = (ushort)(one_minus_x * pix1 + fraction_x * pix2);
                    ushort g2 = (ushort)(one_minus_x * pix3 + fraction_x * pix4);
                    ushort g = (ushort)(one_minus_y * (double)(g1) + fraction_y * (double)(g2));
                    newArrayOfPixels[y * reqWidth + x] = g;
                }
            }

            WriteableBitmap newWBitmap = new WriteableBitmap(reqWidth, reqHeight, 96, 96, PixelFormats.Gray16, null);
            Int32Rect Imagerect = new Int32Rect(0, 0, reqWidth, reqHeight);
            int newStride = reqWidth * ((PixelFormats.Gray16.BitsPerPixel + 7) / 8);
            newWBitmap.WritePixels(Imagerect, newArrayOfPixels, newStride, 0);
            return newWBitmap;
        }
        */

        private void StartUpdating() {
            actionTimer.Interval = WindowManager.Framerate;
            actionTimer.Tick += (s, e) => {
                if (actionQueue.Count > 0) {
                    Action action = actionQueue.Dequeue();
                    action.Invoke();
                }
            };
            actionTimer.Start();
        }
        private void AddTaskToQueue(Action action) => actionQueue.Enqueue(action);

        public void ResetCurrent() {
            AddTaskToQueue(__Execute__ResetCurrent);
        }
        private void __Execute__ResetCurrent() {
            Page.Image.Source = new BitmapImage(source);
        }

        public void ChangeCurentImage(ImageSource imageSource) {
            AddTaskToQueue(() => __Execute__ChangeCurentImage(imageSource));
        }
        private void __Execute__ChangeCurentImage(ImageSource imageSource) {
            // Prepare
            imageCount++;
            Image newImage = new Image { Source = imageSource };
            newImage.Opacity = 0;
            Helper.SetChildInGrid(Page.MainGrid, newImage, Grid.GetRow(Page.Image), Grid.GetColumn(Page.Image));
            //newImage.Margin = new Thickness(
            //currentImage.Margin.Left + 10 * imageCount, 
            //CurrentImage.Margin.Top + 10 * imageCount, 
            //currentImage.Margin.Right - 10 * imageCount, 
            //CurrentImage.Margin.Bottom - 10 * imageCount);
            newImage.Margin = _margin;
            Canvas.SetZIndex(newImage, 10);
            Canvas.SetZIndex(Page.Image, 10 / imageCount);

            // Interpolate
            if (isCalculated) { // isCalculated
                int i = (int)startBoost;
                var timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(interval);
                timer.Tick += (s, e) => {
                    newImage.Opacity = dp_tValues[i];

                    i++;
                    if (i >= dp_tValues.Count) {
                        newImage.Effect = Page.Image.Effect;
                        Page.Image.Effect = null;
                        Page.MainGrid.Children.Remove(Page.Image);
                        Page.Image = newImage;
                        Page.Image.Margin = _margin;
                        imageCount--;
                        timer.Stop();
                    }
                };
                timer.Start();
            }
            else {
                dp_tValues.Clear();
                float t = startBoost;
                var timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(interval);
                timer.Tick += (s, e) => {
                    isCalculated = true;
                    float val = InterpolationFuntion(t);
                    newImage.Opacity = val;
                    dp_tValues.Add(val);

                    t += interval;
                    if (t >= totalInterpolationTime) {
                        newImage.Effect = Page.Image.Effect;
                        Page.Image.Effect = null;
                        Page.MainGrid.Children.Remove(Page.Image);
                        Page.Image = newImage;
                        imageCount--;
                        isCalculated = true;
                        timer.Stop();
                    }
                };
                timer.Start();
            }


        }
    }
}

