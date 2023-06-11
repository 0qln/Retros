using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Retros.ProgramWindow {

    // represents a general change to the images change history 
    public interface IChange : ICloneable { }


    // old IChange, represents a positive change (e.g. smth that adds to the image)
    public interface IPositiveChange : IChange {
        public void Generate(WriteableBitmap bitmap);
        public bool Applied { get; }
    }

    // represents the removal of a positive change
    public interface INegativeChange : IChange {
        public IPositiveChange Value { get; } // the change that was removed
    }



    // represents filters
    public interface IFilterChange : IPositiveChange {
        public double FilterIntensity { get; set; }
    }

    // represents a resize 
    public interface IResizeChange : IPositiveChange {

    }

    // represents dummy, does not have an impact to the image
    public interface IEmptyChange : IPositiveChange {

    }

}
