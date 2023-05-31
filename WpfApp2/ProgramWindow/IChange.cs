using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Retros.ProgramWindow {
    public interface IChange {
        //public WriteableBitmap Bitmap { get; }
        public void Generate();
        public bool Applied { get; }
    }

    public interface IFilterChange {
        public double FilterIntensity { get; set; }
    }

    public interface IResizeChange {

    }
}
