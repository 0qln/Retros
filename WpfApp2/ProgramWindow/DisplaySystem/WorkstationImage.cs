using SharpDX.Multimedia;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
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

namespace Retros.ProgramWindow.DisplaySystem {
    public partial class WorkstationImage : IFrameworkElement {
        public FrameworkElement FrameworkElement => CurrentImage;

        /// <summary>
        /// Used to display the current image.
        /// </summary>
        private Image currentImage = new();
        public Image CurrentImage => currentImage;

        /// <summary>
        /// Chaches the source image.
        /// </summary>
        private Uri source;
        public Uri Source => source;
        
        /// <summary>
        /// Acts as the source image for the runtime image.
        /// </summary>
        private WriteableBitmap resizedSourceBitmap;
        public WriteableBitmap ResizedSourceBitmap => resizedSourceBitmap;

        /// <summary>
        /// Works as a computation/backend image for the filters to work on.
        /// </summary>
        public WriteableBitmap DummyImage { get; set; }


        private Grid imagesGrid = new();
        public Grid Grid => imagesGrid;

        DispatcherTimer actionTimer = new();
        Queue<Action> actionQueue = new();

        public ChangeHistory History => changeHistory;
        private ChangeHistory changeHistory = new();

        public ImageChangeManager GetFilterManager => filterManager;
        private ImageChangeManager filterManager;


        public WorkstationImage(string path) {
            source = new Uri(path);
            filterManager = new(this);

            StartUpdating();

            currentImage.HorizontalAlignment = HorizontalAlignment.Stretch;
            currentImage.Margin = new Thickness(50);
            currentImage.Effect = new DropShadowEffect { BlurRadius = 30, ShadowDepth = 15, Color = Colors.Black, Opacity = 0.8, Direction = 270 };
            imagesGrid.Children.Add(currentImage); //

            SetSourceImage(source);
        }

        public WorkstationImage() {
            filterManager = new(this);
            StartUpdating();
            currentImage.HorizontalAlignment = HorizontalAlignment.Stretch;
            currentImage.Margin = new Thickness(50);
            imagesGrid.Children.Add(currentImage);
            currentImage.Effect = new DropShadowEffect { BlurRadius = 30, ShadowDepth = 15, Color = Colors.Black, Opacity = 0.8, Direction = 270 };

        }
        public void SetSourceImage(Uri source) {
            this.source = source;

            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            resizedSourceBitmap = ResizeWritableBitmap(new BitmapImage(source), (int) screenWidth, (int) screenHeight);

            currentImage.Source = resizedSourceBitmap.Clone();
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
            WriteableBitmap writeableBitmap = 
                filterManager.ApplyChanges(
                    new WriteableBitmap(
                        new BitmapImage(source)));

            System.Windows.Media.Imaging.BitmapSource bitmapSource = writeableBitmap;
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(
                bitmapSource.PixelWidth,
                bitmapSource.PixelHeight,
                System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            System.Drawing.Imaging.BitmapData bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.WriteOnly,
                bitmap.PixelFormat);

            bitmapSource.CopyPixels(
                new System.Windows.Int32Rect(0, 0, bitmapSource.PixelWidth, bitmapSource.PixelHeight),
                bitmapData.Scan0,
                bitmapData.Height * bitmapData.Stride,
                bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);
            
            return bitmap;
        }

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

        /*
        public void SetSource(ImageSource source) {
            sourceImage.Source = source;
            currentImage.Source = source;
            DummyImage = new WriteableBitmap((BitmapSource)SourceImage.Source);
        }
        */

        public void ResetCurrent() {
            AddTaskToQueue(__Execute__ResetCurrent);
        }
        private void __Execute__ResetCurrent() {
            currentImage.Source = new BitmapImage(source);
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
        private static float totalInterpolationTime = 250; /// reccomendet to have this the same as the interval in which the filters are applied
        public float TotalInterpolationTime {
            get => totalInterpolationTime;
            set {
                totalInterpolationTime = value;
                isCalculated = false;
            }
        }
        private float interval = 6.94444444444f; /// time between ticks
        private bool enableStartboost = true;
        private float startBoost { get {
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



        public void ChangeCurentImage(ImageSource imageSource) {
            AddTaskToQueue(() => __Execute__ChangeCurentImage(imageSource));
        }
        private void __Execute__ChangeCurentImage(ImageSource imageSource) {
            // Prepare
            imageCount++;
            Image newImage = new Image { Source = imageSource };
            newImage.Opacity = 0;
            Helper.SetChildInGrid((currentImage.Parent as Grid)!, newImage, Grid.GetRow(currentImage), Grid.GetColumn(currentImage));
            newImage.Margin = currentImage.Margin;
            Canvas.SetZIndex(newImage, 10);
            Canvas.SetZIndex(currentImage, 10 / imageCount);

            // Interpolate
            if (isCalculated) {
                int i = (int)startBoost;
                var timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(interval);
                timer.Tick += (s, e) => {
                    newImage.Opacity = dp_tValues[i];

                    i++;
                    if (i >= dp_tValues.Count) {
                        newImage.Effect = currentImage.Effect;
                        currentImage.Effect = null;
                        (currentImage.Parent as Grid)!.Children.Remove(currentImage);
                        currentImage = newImage;
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
                    //float val = InterpolationFuntion(t);
                    float val = ExtendedMath.RootStep(ExtendedMath.RootStep(t, smoothness, 0, totalInterpolationTime));
                    newImage.Opacity = val;
                    dp_tValues.Add(val);

                    t += interval;
                    if (t >= totalInterpolationTime) {
                        newImage.Effect = currentImage.Effect;
                        currentImage.Effect = null;
                        (currentImage.Parent as Grid)!.Children.Remove(currentImage);
                        currentImage = newImage;
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

