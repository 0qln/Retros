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
using DebugLibrary.Benchmark;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics;
using System.Threading;
using System.Windows.Threading;

namespace Retros {

    // Functionality -> Image Filters Tab Body
    partial class Workstation {
        public partial class WorkstationImage {
            public class FilterManager {
                private Queue<IChange> changes = new();
                private DispatcherTimer timer = new();

                public void ApplyChange() {
                    if (changes.Count <= 0) return;

                    IChange change = changes.Dequeue();
                    UIManager.Workstation.ImageElement.AddTaskToQueue(change.Apply);
                }

                public void AddChange(IChange change) {
                    if (changes.Any(c => c.GetType() == change.GetType())) {
                        IChange existingChange = changes.First(c => c.GetType() == change.GetType());
                        changes = new Queue<IChange>(changes.Where(c => c != existingChange));
                        changes.Enqueue(change);
                    }
                    else {
                        changes.Enqueue(change);
                    }

                    Debugger.Console.ClearAll();
                    foreach (var item in changes) {
                        Debugger.Console.Log(item.GetType().ToString());
                    }
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

}
