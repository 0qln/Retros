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
                private List<IChange> changes = new List<IChange>();
                private DispatcherTimer timer = new DispatcherTimer();
                private Type? lastChange = null;
                public Type LastChange => lastChange!;

                public FilterManager() {
                    timer.Interval = UIManager.Framerate;
                    timer.Tick += (s, e) => {
                        ApplyChange();
                    };
                    timer.Start();
                }

                public void Clear() {
                    changes.Clear();
                }

                public void ApplyChange() {
                    if (changes.Count <= 0) return;
                    Debugger.Console.ClearAll();
                    changes.ForEach(c => Debugger.Console.Log(c.GetType()));

                    IChange change = changes[0];
                    UIManager.Workstation.ImageElement.AddTaskToQueue(change.Apply);
                    lastChange = change.GetType();
                    changes.RemoveAt(0);

                }

                public void AddChange(IChange change) {
                    bool replaced = false;
                    for (int i = 0; i < changes.Count; i++) {
                        if (changes[i].GetType() == change.GetType()) {
                            changes[i] = change;
                            replaced = true;
                        }
                    }
                    if (!replaced) {
                        changes.Add(change);
                    }
                }

                public void ApplyAllChanges() {
                    while (changes.Count > 0) {
                        ApplyChange();
                    }
                }
            }
        }
    }

}
