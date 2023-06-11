using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Retros.ProgramWindow {
    public interface IChange : ICloneable {
        public void Generate(WriteableBitmap bitmap);
        public bool Applied { get; }
    }

    public interface IFilterChange : IChange {
        public double FilterIntensity { get; set; }
    }

    public interface IResizeChange : IChange {

    }

    public interface IEmptyChange : IChange {

    }

    public interface IRenderChange : IChange {

    }
}
