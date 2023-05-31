using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Retros.ProgramWindow {
    public interface IChange {
        public void Generate();
        public bool Applied { get; }
        //public delegate void ChangeEventHandler();
        //public event ChangeEventHandler ChangeEvent;

    }

    public interface IFilterChange {
        public double FilterIntensity { get; set; }
    }

    public interface IResizeChange {

    }
}
