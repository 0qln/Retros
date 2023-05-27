using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retros.ProgramWindow {
    public interface IChange {
        public void Apply();
    }

    public interface IFilterChange {
        public double FilterIntensity { get; set; }
    }

    public interface IResizeChange {

    }
}
