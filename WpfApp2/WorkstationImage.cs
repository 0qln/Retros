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

namespace Retros {
    partial class Workstation {
        public partial class WorkstationImage : IFrameworkElement {

            public ChangeHistory History {
                get;
            }

            public FrameworkElement FrameworkElement => Image;

            private Image image = new();
            public Image Image {
                get => image;
                set => ChangeImage(value);
            }
            public Bitmap Bitmap { get; set; }

            DispatcherTimer timer = new();
            Queue<Action> actionQueue = new();


            public WorkstationImage(string path) {
                Helper.SetImageSource(image, path);
                Bitmap = new(path);
                History = new ChangeHistory();
                StartUpdating();
            }

            private void StartUpdating() {
                timer.Interval = UIManager.Framerate;
                timer.Tick += (s, e) => {
                    if (actionQueue.Count > 0) {
                        Action action = actionQueue.Dequeue();
                        action.Invoke();
                        Debugger.Console.Log("Action");
                    }
                };
                timer.Start();
            }


            public void AddTaskToQueue(Action action) {
                actionQueue.Enqueue(action);
            }


            private Bitmap? _oldBitmap;
            public void ChangeImage(Image newImage) {
                //Return if the bitmap has not changed
                if (Bitmap.Equals(_oldBitmap))
                    return;


                //Prepare
                newImage.Opacity = 0;
                Helper.SetChildInGrid((Image.Parent as Grid)!, newImage, Grid.GetRow(Image), Grid.GetColumn(Image));
                Panel.SetZIndex(newImage, 20);

                //Interpolate
                int smoothness = 3; 
                float totalLerpTime = 500;
                float interval = 16.666f; /// time between ticks /// const
                float startBoost = interval * smoothness; /// recomendet for high smoothness (This approximates, will not work for very high values)
                float t = 0;
                var timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(interval);
                timer.Tick += (s, e) => {
                    float val = SmoothStep(t, smoothness, 0, totalLerpTime);
                    Image.Opacity = 1-val;
                    newImage.Opacity = val;

                    t += interval;
                    if (t >= totalLerpTime) {
                        timer.Stop();
                        image = newImage;
                    }
                };
                timer.Start();
            }
            private float Smootherstep(float x, float edge0 = 0.0f, float edge1 = 1.0f) {
                x = Clamp((x - edge0) / (edge1 - edge0));
                return x * x * x * (3.0f * x * (2.0f * x - 5.0f) + 10.0f);
            }
            private float SmoothStep(float x, int n, float edge0 = 0, float edge1 = 1.0f) {
                x = Clamp((x - edge0) / (edge1 - edge0));
                float result = 0;
                for (int i = 0; i <= n; ++i) {
                    result += PascalTriangle(-n - 1, i) * PascalTriangle(2 * n + 1, n - i) * MathF.Pow(x, n + i + 1);
                }
                return result;
            }
            private float PascalTriangle(int a, int b) {
                float result = 1;
                for (int i = 0; i < b; i++) {
                    result *= (a - i) / (a + i);
                }
                return result;
            }
            private float Clamp(float x, float lowerlimit = 0.0f, float upperlimit = 1.0f) 
                => (x < lowerlimit) ? (lowerlimit) : ((x > upperlimit) ? (upperlimit) : (x));
            


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
        }
    }
}
