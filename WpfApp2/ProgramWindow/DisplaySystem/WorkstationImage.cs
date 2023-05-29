using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Utillities.Wpf;
using Utillities;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Retros.ProgramWindow.DisplaySystem {
    public partial class WorkstationImage : IFrameworkElement {
        public FrameworkElement FrameworkElement => CurrentImage;

        private Image currentImage = new();
        public Image CurrentImage {
            get => currentImage;
            set => currentImage = value;
        }
        private Image original = new();
        public Image Original => original;

        public WriteableBitmap DummyImage { get; set; }


        private Grid imagesGrid = new();
        public Grid Grid => imagesGrid;

        DispatcherTimer actionTimer = new();
        Queue<Action> actionQueue = new();

        public ChangeHistory History => changeHistory;
        private ChangeHistory changeHistory = new();

        public FilterManager GetFilterManager => filterManager;
        private FilterManager filterManager;


        public WorkstationImage(string path) {

            filterManager = new(this);

            StartUpdating();

            Helper.SetImageSource(currentImage, path);
            currentImage.HorizontalAlignment = HorizontalAlignment.Stretch;
            currentImage.Margin = new Thickness(50);
            imagesGrid.Children.Add(currentImage);
            currentImage.Effect = new DropShadowEffect { BlurRadius = 30, ShadowDepth = 15, Color = Colors.Black, Opacity = 0.8, Direction = 270 };

            Helper.SetImageSource(original, path);

            DummyImage = new WriteableBitmap((BitmapSource)Original.Source);
        }


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
        public void AddTaskToQueue(Action action) => actionQueue.Enqueue(action);


        public void SetSource(ImageSource source) {
            original.Source = source;
            currentImage.Source = source;
            ChangeImage(source);
        }

        public void ResetCurrent() {
            currentImage.Source = original.Source.Clone();
        }


        private List<float> dp_tValues = new(); /// caches the opacity curve
        private bool isCalculated = false;
        private int smoothness = 2;
        public int InterpolationSmoothness {
            get => smoothness;
            set {
                smoothness = value;
                isCalculated = false;
            }
        }
        private float totalInterpolationTime = 100;
        public float TotalInterpolationTime {
            get => totalInterpolationTime;
            set {
                totalInterpolationTime = value;
                isCalculated = false;
            }
        }
        private float interval = 6.94444444444f; /// time between ticks
        private float startBoost => smoothness; /// recomendet for high smoothness (This approximates, will not work for very high values)
        private int imageCount = 1; /// used to set the newest images to the front

        private void ChangeImage(ImageSource imageSource) {
            //Prepare
            imageCount++;
            Image newImage = new Image { Source = imageSource };
            newImage.Opacity = 0;
            Helper.SetChildInGrid((currentImage.Parent as Grid)!, newImage, Grid.GetRow(currentImage), Grid.GetColumn(currentImage));
            newImage.Margin = currentImage.Margin;
            Canvas.SetZIndex(newImage, 10);
            Canvas.SetZIndex(currentImage, 10 / imageCount);

            //Interpolate
            if (isCalculated) {
                int i = (int)startBoost;
                var timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(interval);
                timer.Tick += (s, e) => {
                    newImage.Opacity = dp_tValues[i];

                    i++;
                    if (i >= dp_tValues.Count) {
                        newImage.Effect = currentImage.Effect;
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
                    float val = ExtendedMath.LinearStep(t, smoothness, 0, totalInterpolationTime);
                    newImage.Opacity = val;
                    dp_tValues.Add(val);

                    t += interval;
                    if (t >= totalInterpolationTime) {
                        newImage.Effect = currentImage.Effect;
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

