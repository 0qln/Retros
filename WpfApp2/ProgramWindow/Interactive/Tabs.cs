using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using WpfUtillities;
using System.Diagnostics;
using DebugLibrary.Benchmark;
using Debugger;
using Console = Debugger.Console;
using System.Windows.Threading;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Security.RightsManagement;
using Retros.ProgramWindow.Interactive.Tabs.Bodies;
using Retros.ProgramWindow.Interactive.Tabs.Handles;
using Retros.ProgramWindow.DisplaySystem;

namespace Retros.ProgramWindow.Interactive.Tabs {
    public abstract class Tab {
        protected Handle handle;
        public Handle Handle => handle;
        protected Body body;
        public Body Body => body;


        protected Border border = new();
        public FrameworkElement FrameworkElement => border;

        protected int index = -1;
        public int Index { get => index; set => index = value; }


        public Tab(Body body, Handle handle) {
            border.Child = handle.FrameworkElement;
            this.body = body;
            this.handle = handle;
        }
    }


    public class ImageFilterTab : Tab {
        public ImageFilterTab(Body body, Handle handle) : base(body, handle) { }

    }

    public class PixelSortingTab : Tab {
        public PixelSortingTab(Body body, Handle handle) : base(body, handle) { }

    }

}

