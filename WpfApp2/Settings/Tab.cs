using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Retros.Settings {
    internal partial class Tab : IFrameworkElement {
        public FrameworkElement FrameworkElement => Details;

        public StackPanel Details = new();

    }


    internal partial class Tab{
        public class Header : IFrameworkElement {
            public FrameworkElement FrameworkElement => throw new NotImplementedException();

        }
    }

    internal partial class Tab {
        public class Body : IFrameworkElement {
            public FrameworkElement FrameworkElement => throw new NotImplementedException();
        }
    }


}
