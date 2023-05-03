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

namespace Retros.ClientWorkStation {
    public class WorkstationImage {
        private ChangeHistory history;
        public ChangeHistory GetHistory => history;

        private string Path = "";
        public string GetPath => Path;

        public Bitmap Bitmap;
        public System.Windows.Controls.Image Image = new();


        public WorkstationImage(string path) {
            history = new(this);
            Path = path;
            Bitmap = new(path);
            Helper.SetImageSource(Image, path);
        }


        
        public void Update() {
            BitmapSource ?source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                Bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions()
            );
            Image.Source = source;
        }


        public class ChangeHistory {
            private WorkstationImage workstationImage;

            private List<System.Windows.Controls.Image> imageChache = new();
            private List<IChange> changes = new();

            private int currentIndex = 0;
            
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
