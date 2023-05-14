using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;
using Image = System.Windows.Controls.Image;
using System.Windows.Threading;
using WpfCustomControls;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Media;

namespace Retros {
    partial class Workstation {
        /*
        public partial class WorkstationImage0 : IFrameworkElement {

            public ChangeHistory History {
                get;
            }

            public FrameworkElement FrameworkElement => Image;

            private Image image = new();
            public Image Image {
                get => image;
                set => ChangeImage(value);
            }
            private Bitmap bitmap;
            public Bitmap Bitmap {
                get => bitmap;
                set => ChangeImage(value);
            }



            DispatcherTimer actionTimer = new();
            Queue<Action> actionQueue = new();


            public WorkstationImage0(string path) {
                Helper.SetImageSource(image, path);
                bitmap = new(path);
                History = new ChangeHistory();
                StartUpdating();

                // Pre calculate the interpolation values for image changes
                float t = startBoost;
                while (t < totalInterpolationTime) {
                    dp_tValues.Add(SmoothStep(t, smoothness, 0, totalInterpolationTime));
                    t += interval;
                }
                Debugger.Console.Log(dp_tValues.Count);
                isCalculated = true;
            }

            private void StartUpdating() {
                actionTimer.Interval = UIManager.Framerate;
                actionTimer.Tick += (s, e) => {
                    if (actionQueue.Count > 0) {
                        Action action = actionQueue.Dequeue();
                        action.Invoke();
                        ForceUpdate();
                    }
                };
                actionTimer.Start();
            }


            public void AddTaskToQueue(Action action) {
                actionQueue.Enqueue(action);
            }


            public void ForceUpdate() {
                Image = ConvertToImage(Bitmap);
            }

            private List<float> dp_tValues = new(); /// caches the opacity curve
            private bool isCalculated = false;
            private int smoothness = 1;
            public int InterpolationSmoothness {
                get => smoothness;
                set {
                    smoothness = value;
                    isCalculated = false;
                }
            }
            private float totalInterpolationTime = 500;
            public float TotalInterpolationTime {
                get => totalInterpolationTime;
                set {
                    totalInterpolationTime = value;
                    isCalculated = false;
                }
            }
            private float interval = 16.666f; /// time between ticks /// const
            public float startBoost {   /// recomendet for high smoothness (This approximates, will not work for very high values)
                get => interval * smoothness;
            }

            public void ChangeImage(Image newImage) {
                //Prepare
                newImage.Opacity = 0;
                Helper.SetChildInGrid((Image.Parent as Grid)!, newImage, Grid.GetRow(Image), Grid.GetColumn(Image));
                newImage.Margin = Image.Margin;

                //Interpolate
                if (isCalculated) {
                    Debugger.Console.Log("calculated");

                    int i = 0;
                    var timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromMilliseconds(interval);
                    timer.Tick += (s, e) => {
                        float val = dp_tValues[i];
                        Image.Opacity = 1 - val;
                        newImage.Opacity = val;

                        i++;
                        if (i >= dp_tValues.Count) {
                            timer.Stop();
                            image = newImage;
                            //bitmap = ConvertImageToBitmap(image);
                        }
                    };
                    timer.Start();
                }
                else {
                    Debugger.Console.Log("Not calculated");

                    dp_tValues.Clear();
                    float t = startBoost;
                    int i = 0;
                    var timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromMilliseconds(interval);
                    timer.Tick += (s, e) => {
                        float val = SmoothStep(t, smoothness, 0, totalInterpolationTime);
                        Image.Opacity = 1-val;
                        newImage.Opacity = val;

                        dp_tValues.Add(val);

                        t += interval;
                        i++;
                        if (t >= totalInterpolationTime) {
                            timer.Stop();
                            image = newImage;
                            //bitmap = ConvertImageToBitmap(image);
                            isCalculated = true;
                        }
                    };
                    timer.Start();
                }
            }
            public void ChangeImage(Bitmap newBitmap) {
                //Return if the bitmap has not changed
                if (newBitmap.Equals(Bitmap))
                    return;

                Image = ConvertToImage(newBitmap);
            }

            private float Smootherstep(float x, float edge0 = 0.0f, float edge1 = 1.0f) {
                x = Clamp((x - edge0) / (edge1 - edge0));
                return x * x * x * (3.0f * x * (2.0f * x - 5.0f) + 10.0f);
            }
            private float SmoothStep(float x, int n = 1, float edge0 = 0, float edge1 = 1.0f) {
                x = Clamp((x - edge0) / (edge1 - edge0));
                float result = 0;
                for (int i = 0; i <= n; ++i) {
                    result += PascalTriangle(-n - 1, i) * PascalTriangle(2 * n + 1, n - i) * MathF.Pow(x, n + i + 1);
                }
                return result;
            }
            private float Clamp(float x, float lowerlimit = 0.0f, float upperlimit = 1.0f) 
                => (x < lowerlimit) ? (lowerlimit) : ((x > upperlimit) ? (upperlimit) : (x));
            private float PascalTriangle(int a, int b) {
                float result = 1;
                for (int i = 0; i < b; i++) {
                    result *= (a - i) / (a + i);
                }
                return result;
            }
            


            private Image ConvertToImage(Bitmap bitmap) {
                BitmapImage source = new();
                using (MemoryStream stream = new MemoryStream()) {
                    bitmap.Save(stream, ImageFormat.Bmp);
                    stream.Seek(0, SeekOrigin.Begin);

                    source.BeginInit();
                    source.CacheOption = BitmapCacheOption.OnLoad;
                    source.StreamSource = stream;
                    source.EndInit();
                }
                return new Image { Source = source };
            }
            private Bitmap ConvertImageToBitmap(Image image) {
                RenderTargetBitmap rtb = new RenderTargetBitmap((int)image.ActualWidth, (int)image.ActualHeight, 96, 96, PixelFormats.Pbgra32);
                rtb.Render(image);
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(rtb));
                MemoryStream ms = new();
                encoder.Save(ms);

                return new Bitmap(ms);
            }
        }
        */
    }
}
