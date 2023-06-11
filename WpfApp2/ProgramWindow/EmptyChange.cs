using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Retros.ProgramWindow {
    public class RootChange : IEmptyChange {
        public bool Applied => false;
        public void Generate(WriteableBitmap bitmap) { }
        public object Clone() => new RootChange();
    }
}
