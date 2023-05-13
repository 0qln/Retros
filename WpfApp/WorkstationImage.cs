using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;
using WpfCustomControls;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Threading;
using System.Threading;
using System.Data;
using COnsole = Debugger.Console;
using System.ComponentModel;

namespace Retros.ClientWorkStation {
    public class WorkstationImage {
        private ChangeHistory history;
        public ChangeHistory GetHistory => history;

        private string Path = "";
        public string GetPath => Path;

        public Bitmap Bitmap;
        public System.Windows.Controls.Image Image = new();

        Queue<Action> tasks = new();
        DispatcherTimer timer_tasks = new();

        public WorkstationImage(string path) {
            history = new(this);
            Path = path;
            Bitmap = new(path);
            Helper.SetImageSource(Image, path);
        }

        public void AddTaskToQueue(Action action) {
            tasks.Enqueue(action);
        }


        Bitmap? oldBitmap;
        public void Update() {
            if (Bitmap.Equals(oldBitmap)) 
                return;
            
            BitmapImage newSource = new();
            Task.Run(() => {
                using (MemoryStream stream = new MemoryStream()) {
                    Bitmap.Save(stream, ImageFormat.Bmp);
                    stream.Seek(0, SeekOrigin.Begin);

                    newSource.BeginInit();
                    newSource.CacheOption = BitmapCacheOption.OnLoad;
                    newSource.StreamSource = stream;
                    newSource.EndInit();
                }
            });
            
            Image.Source = newSource;
            oldBitmap = new Bitmap(Bitmap);
        }

        // Causes memory issues
        private BitmapSource? oldSource;
        private void Update_OLD() {
            //Update UI
            BitmapSource? source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                Bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions()
            );

            if (oldSource == source) {
                return;
            }

            Image.Source = source;
            oldSource = source;
        }


        public class ChangeHistory {
            private WorkstationImage workstationImage;

            private Stack<System.Windows.Controls.Image> imageChache = new();
            private Stack<IChange> changes = new();


            public ChangeHistory(WorkstationImage workstationImage) {
                this.workstationImage = workstationImage;
            }

            public void Add(IChange change) {

            }

            public void Forward() {

            }

            public void Backward() {

            }
        }
    }
}
