using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using Utillities.Wpf;
using System.Diagnostics;
using DebugLibrary.Benchmark;
using System.Windows.Threading;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Security.RightsManagement;

namespace Retros.Program.Workstation.TabUI.Tabs
{
    public abstract class Tab
    {
        protected Handle handle;
        public Handle Handle => handle;
        protected Body body;
        public Body Body => body;


        protected Border border = new();
        public FrameworkElement FrameworkElement => border;

        protected int index = -1;
        public int Index { get => index; set => index = value; }


        public Tab(Body body, Handle handle)
        {
            border.Child = handle.FrameworkElement;
            this.body = body;
            this.handle = handle;
        }
        public Tab(Body body)
        {
            this.body = body;
            string handleName = body.GetType().Name;
            if (handleName.Substring(handleName.Length-4) == "Body") { 
                handleName = handleName.Substring(0, handleName.Length - 4);
            }
            this.handle = new DefaultHandle(handleName);
            border.Child = handle.FrameworkElement;
        }

        public void Reset() => body.Reset();
    }


    public class ImageHistoryTab : Tab
    {
        public ImageHistoryTab(Body body, Handle handle) : base(body, handle) { }
        public ImageHistoryTab(Body body) : base(body) { }
    }

    public class ImageFilterTab : Tab
    {
        public ImageFilterTab(Body body, Handle handle) : base(body, handle) { }
        public ImageFilterTab(Body body) : base(body) { }
    }

    public class TestTab : Tab
    {
        public TestTab(Body body, Handle handle) : base(body, handle) { }
        public TestTab(Body body) : base(body) { }
    }

    public class PixelSortingTab : Tab
    {
        public PixelSortingTab(Body body, Handle handle) : base(body, handle) { }
        public PixelSortingTab(Body body) : base(body) { }
    }

}

