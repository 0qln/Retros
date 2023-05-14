using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Drawing;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Interop;
using System.Windows;
using System.IO;
using Color = System.Drawing.Color;
using System.Diagnostics;
using System.Drawing.Imaging;
using COnsole = Debugger.Console;
using DebugLibrary.Benchmark;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics;
using System.Threading;

namespace Retros.WorkstationTableElements.Bodies {
    public partial class ImageFilter {
        public class ImageEditor {
            private Queue<IChange> changes = new();

            public void ApplyChange() {
                if (changes.Count <= 0) return;

                IChange change = changes.Dequeue();
                UIManager.Workstation.ImageElement.AddTaskToQueue(change.Apply); 
            }

            public void AddChange(IChange change) {
                changes.Enqueue(change);
            }

            public void ApplyAllChanges() {
                while (changes.Count > 0) {
                    IChange change = changes.Dequeue();
                    UIManager.Workstation.ImageElement.AddTaskToQueue(change.Apply);
                }
            }
        }
    }

}
