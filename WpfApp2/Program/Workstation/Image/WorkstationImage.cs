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
using Retros.Program.DisplaySystem;
using Retros.Program.Workstation.Changes;

namespace Retros.Program.Workstation.Image
{
    public partial class WorkstationImage : IFrameworkElement
    {
        private WriteableBitmap _resizedSourceBitmap;
        private Uri _source;
        private FilterManager _filterManager;
        private ChangeHistory _changeHistory = new();
        private DispatcherTimer _actionTimer = new();
        private Queue<Action> _actionQueue = new();
        private Frame _pageFrame = new();

        /// <summary>The FrameworkElement</summary>
        public FrameworkElement FrameworkElement => _pageFrame;

        /// <summary>Used to display the image.</summary>
        public readonly List<WorkstationImagePage> Pages = new();

        public WorkstationImagePage Page => Pages[0];

        /// <summary>The Frame of the WorkstaionImagePage</summary>
        public Frame PageFrame => _pageFrame;

        /// <summary>Chaches the source image.</summary>
        public Uri Source => _source;

        /// <summary>Acts as the source image for the runtime image.</summary>
        public WriteableBitmap ResizedSourceBitmap => _resizedSourceBitmap;

        /// <summary>Works as a computation/backend image for the filters to work on.</summary>
        public WriteableBitmap DummyImage { get; set; }

        /// <summary>The FilterManager instance for this WorkstationImage.</summary>
        public FilterManager GetFilterManager => _filterManager;

        /// <summary>Gets the ChangeHistory instance for this WorkstationImage.</summary>
        public ChangeHistory GetHistoryManager => _changeHistory;


        private List<float> _dp_tValues = new(); /// caches the opacity curve
        private static int _smoothness = 1;
        private bool _enableStartboost = false;
        public float StartBoost
        {
            get
            {
                if (_enableStartboost)
                {
                    return _smoothness;
                }
                else
                {
                    return 0;
                }
            }
        } /// recomendet for high smoothness (This approximates, will not work for very high values)
        private static float _totalInterpolationTime = 150; /// reccomendet to have this the same as the interval in which the filters are applied
        private float _interval = 6.94444444444f; /// time between ticks

        public List<float> Dp_tValues => _dp_tValues;
        public int InterpolationSmoothness
        {
            get => _smoothness;
            set
            {
                _smoothness = value;
                IsCalculated = false;
            }
        }
        public float TotalInterpolationTime
        {
            get => _totalInterpolationTime;
            set
            {
                _totalInterpolationTime = value;
                IsCalculated = false;
            }
        }
        public bool IsCalculated = false;
        public float Interval => _interval;

        public delegate float InterpolationFuntionHandler(float t);
        public InterpolationFuntionHandler InterpolationFuntion = (t) => ExtendedMath.RootStep(ExtendedMath.RootStep(t, _smoothness, 0, _totalInterpolationTime));



        public WorkstationImage(string path)
        {
            WorkstationImagePage page = new WorkstationImagePage(this);
            Pages.Add(page);
            PageFrame.Content = page;

            _filterManager = new(this);
            StartUpdating();

            _source = new Uri(path);
            SetSourceImage(_source);

            _changeHistory.ImageChanged += SyncHistoryCurrentWithChangeManager;
        }
        public WorkstationImage()
        {
            WorkstationImagePage page = new WorkstationImagePage(this);
            Pages.Add(page);
            PageFrame.Content = page;

            _filterManager = new(this);
            StartUpdating();
        }

        public void SyncHistoryCurrentWithChangeManager()
        {
            // Update filter manager
            _filterManager.CurrentFilters = _changeHistory.CurrentNode.Value.Filters;

            // Update UI
            ChangeCurentImages(_filterManager.ApplyChanges(new WriteableBitmap(ResizedSourceBitmap)));
        }

        public void Reset() {
            GetFilterManager.Clear();
            GetHistoryManager.Clear();
            ResetCurrents();
        }

        public void SetSourceImage(Uri source)
        {
            _source = source;

            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            _resizedSourceBitmap = ResizeWritableBitmap(new BitmapImage(source), (int)screenWidth, (int)screenHeight);

            Pages.ForEach(Page => Page.Image.Source = _resizedSourceBitmap.Clone());
            DummyImage = new WriteableBitmap(_resizedSourceBitmap);
        }

        public static WriteableBitmap ResizeWritableBitmap(BitmapImage originalBitmap, int newWidth, int newHeight)
        {
            //DebugLibrary.Console.Log(newHeight + ", " + newHeight);

            // Check if resizing is required
            if (newWidth >= originalBitmap.PixelWidth && newHeight >= originalBitmap.PixelHeight)
            {
                return new WriteableBitmap(originalBitmap);
            }

            // Convert the original bitmap to a compatible pixel format (e.g., Pbgra32)
            FormatConvertedBitmap convertedBitmap = new FormatConvertedBitmap(originalBitmap, PixelFormats.Pbgra32, null, 0);

            // Calculate the aspect ratios
            double aspectRatioOriginal = (double)originalBitmap.PixelWidth / originalBitmap.PixelHeight;
            double aspectRatioNew = (double)newWidth / newHeight;

            //DebugLibrary.Console.Log(aspectRatioOriginal + " -> " + aspectRatioNew);

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
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.PushTransform(transform);
                drawingContext.DrawImage(convertedBitmap, new Rect(0, 0, originalBitmap.PixelWidth, originalBitmap.PixelHeight));
            }

            // Render the DrawingVisual to the resized RenderTargetBitmap
            resizedRenderTarget.Render(drawingVisual);

            // Create a new WriteableBitmap with the resized RenderTargetBitmap
            WriteableBitmap resizedBitmap = new WriteableBitmap(resizedRenderTarget);

            //DebugLibrary.Console.Log(originalBitmap.Width + ", " + originalBitmap.Height + ", " + originalBitmap.Width / originalBitmap.Height);
            //DebugLibrary.Console.Log(resizedBitmap.Width + ", " + resizedBitmap.Height + ", " + resizedBitmap.Width / resizedBitmap.Height);

            return resizedBitmap;
        }

        public System.Drawing.Bitmap Render()
        {
            // Get the image Source
            WriteableBitmap writeableBitmap =
                _filterManager.ApplyChanges(
                    new WriteableBitmap(
                        new BitmapImage(_source)));

            // Default the pixel format
            WriteableBitmap formattedBitmap = SwitchPixelFormat(writeableBitmap, PixelFormats.Bgra32);

            // Create the bitmap
            BitmapSource bitmapSource = formattedBitmap;
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(
                bitmapSource.PixelWidth,
                bitmapSource.PixelHeight,
                GetSwappedPixelFormats()[bitmapSource.Format]);

            // Create the bitmapData
            BitmapData bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.WriteOnly,
                bitmap.PixelFormat);

            // Copy the bixels of the bitmapData to the bitmap
            bitmapSource.CopyPixels(
                new Int32Rect(0, 0, bitmapSource.PixelWidth, bitmapSource.PixelHeight),
                bitmapData.Scan0,
                bitmapData.Height * bitmapData.Stride,
                bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }

        public static System.Windows.Controls.Image ToImage(System.Drawing.Bitmap bitmap)
        {
            BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return new System.Windows.Controls.Image { Source = bitmapSource };
        }

        public static WriteableBitmap SwitchPixelFormat(WriteableBitmap originalBitmap, System.Windows.Media.PixelFormat newPixelFormat)
        {
            // Convert WriteableBitmap to Bitmap
            System.Drawing.Bitmap bitmap;
            using (MemoryStream stream = new MemoryStream())
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(originalBitmap));
                encoder.Save(stream);
                bitmap = new(stream);
            }

            // Create a new Bitmap with the desired pixel format
            System.Drawing.Bitmap newBitmap = new System.Drawing.Bitmap(bitmap.Width, bitmap.Height, GetEqualPixelFormat(newPixelFormat));

            // Copy pixel values from the original Bitmap to the new Bitmap
            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(newBitmap))
            {
                graphics.DrawImage(bitmap, new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height));
            }

            // Convert Bitmap back to WriteableBitmap
            BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                newBitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            WriteableBitmap newWriteableBitmap = new WriteableBitmap(bitmapSource);

            // Dispose the intermediary Bitmap
            bitmap.Dispose();
            newBitmap.Dispose();

            return newWriteableBitmap;
        }
        public static Dictionary<System.Drawing.Imaging.PixelFormat, System.Windows.Media.PixelFormat> GetEqualPixelFormats()
        {
            Dictionary<System.Drawing.Imaging.PixelFormat, System.Windows.Media.PixelFormat> equalFormats = new Dictionary<System.Drawing.Imaging.PixelFormat, System.Windows.Media.PixelFormat>();

            equalFormats[System.Drawing.Imaging.PixelFormat.Format1bppIndexed] = PixelFormats.Indexed1;
            equalFormats[System.Drawing.Imaging.PixelFormat.Format4bppIndexed] = PixelFormats.Indexed4;
            equalFormats[System.Drawing.Imaging.PixelFormat.Format8bppIndexed] = PixelFormats.Indexed8;

            equalFormats[System.Drawing.Imaging.PixelFormat.Format16bppRgb555] = PixelFormats.Bgr555;
            equalFormats[System.Drawing.Imaging.PixelFormat.Format16bppRgb565] = PixelFormats.Bgr565;

            equalFormats[System.Drawing.Imaging.PixelFormat.Format24bppRgb] = PixelFormats.Bgr24;
            equalFormats[System.Drawing.Imaging.PixelFormat.Format32bppRgb] = PixelFormats.Bgr32;
            equalFormats[System.Drawing.Imaging.PixelFormat.Format48bppRgb] = PixelFormats.Rgb48;

            equalFormats[System.Drawing.Imaging.PixelFormat.Format32bppArgb] = PixelFormats.Bgra32;
            equalFormats[System.Drawing.Imaging.PixelFormat.Format64bppArgb] = PixelFormats.Rgba64;

            equalFormats[System.Drawing.Imaging.PixelFormat.Format32bppPArgb] = PixelFormats.Pbgra32;

            equalFormats[System.Drawing.Imaging.PixelFormat.Format16bppGrayScale] = PixelFormats.Gray16;

            return equalFormats;
        }
        public static Dictionary<System.Windows.Media.PixelFormat, System.Drawing.Imaging.PixelFormat> GetSwappedPixelFormats()
        {
            Dictionary<System.Windows.Media.PixelFormat, System.Drawing.Imaging.PixelFormat> swappedFormats = new Dictionary<System.Windows.Media.PixelFormat, System.Drawing.Imaging.PixelFormat>();
            Dictionary<System.Drawing.Imaging.PixelFormat, System.Windows.Media.PixelFormat> originalFormats = GetEqualPixelFormats();
            foreach (var kvp in originalFormats)
            {
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

        private void StartUpdating()
        {
            _actionTimer.Interval = WindowManager.Framerate;
            _actionTimer.Tick += (s, e) =>
            {
                if (_actionQueue.Count > 0)
                {
                    Action action = _actionQueue.Dequeue();
                    action.Invoke();
                }
            };
            _actionTimer.Start();
        }
        private void AddTaskToQueue(Action action) => _actionQueue.Enqueue(action);

        public void ResetCurrents()
        {
            AddTaskToQueue(__Execute__ResetCurrents);
        }
        private void __Execute__ResetCurrents()
        {
            Pages.ForEach(Page => Page.Image.Source = new BitmapImage(_source));
        }

        public void ChangeCurentImages(ImageSource imageSource)
        {
            AddTaskToQueue(() => __Execute__ChangeCurentImages(imageSource));
        }
        private void __Execute__ChangeCurentImages(ImageSource imageSource)
        {
            foreach (var page in Pages)
            {
                page.ChangeCurrent(imageSource);
            }
            /*
            // Prepare
            _imageCount++;
            Dictionary<WorkstationImagePage, Image> newImages = new(Pages.Count);
            Pages.ForEach(Page => {
                if (Page.Visibility == Visibility.Visible) {
                    newImages.Add(Page, CreateFadeImage(Page, imageSource));
                }
                else {
                    newImages.Add(Page, null);
                }
            });

            // Interpolate
            if (_isCalculated) { // isCalculated
                int i = (int)_startBoost;
                var timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(_interval);
                timer.Tick += (s, e) => {
                    ForEach(newImages, kvp => kvp.Value.Opacity = _dp_tValues[i]);

                    i++;
                    if (i >= _dp_tValues.Count) {
                        ForEach(newImages, kvp => kvp.Value.Effect = kvp.Key.ImageEffect);
                        Pages.ForEach(Page => Page.Image.Effect = null);
                        Pages.ForEach(Page => Page.MainGrid.Children.Remove(Page.Image));
                        ForEach(newImages, kvp => kvp.Key.Image = kvp.Value);
                        Pages.ForEach(Page => Page.Image.Margin = _margin);
                        _imageCount--;
                        timer.Stop();
                    }
                };
                timer.Start();
            }
            else {
                _dp_tValues.Clear();
                float t = _startBoost;
                var timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(_interval);
                timer.Tick += (s, e) => {
                    _isCalculated = true;
                    float val = InterpolationFuntion(t);
                    ForEach(newImages, kvp => kvp.Value.Opacity = val);
                    _dp_tValues.Add(val);

                    t += _interval;
                    if (t >= _totalInterpolationTime) {
                        ForEach(newImages, kvp => kvp.Value.Effect = kvp.Key.ImageEffect);
                        Pages.ForEach(Page => Page.Image.Effect = null);
                        Pages.ForEach(Page => Page.MainGrid.Children.Remove(Page.Image));
                        ForEach(newImages, kvp => kvp.Key.Image = kvp.Value);
                        _imageCount--;
                        _isCalculated = true;
                        timer.Stop();
                    }
                };
                timer.Start();
            }
            */

        }

        private void ForEach<TKey, TValue>(Dictionary<TKey, TValue> dict, Action<KeyValuePair<TKey, TValue>> action) where TKey : notnull
        {
            foreach (var kvp in dict)
            {
                action.Invoke(kvp);
            }
        }
    }
}

