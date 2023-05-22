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
using Retros.WorkstationTableElements.Tabs;

namespace Retros {

    // Functionality -> Image Filters Tab Body
    partial class Workstation {
        public partial class WorkstationImage {
            public class FilterManager {
                private HashSet<Type> changeTypes = new(); // (change, index)
                private List<IChange> changes = new();
                private DispatcherTimer timer = new();

                public FilterManager() {
                    timer.Interval = TimeSpan.FromMilliseconds(250);
                    timer.Tick += (s, e) => ApplyChanges();
                    timer.Start();
                }

                public void Clear() {
                    changes.Clear();
                }

                public void ApplyChanges() {
                    if (changes.Count > 0) {
                        changes.ForEach(change => {
                            if (!change.Applied()) change.Apply();
                        });
                    }
                }

                public bool AddChange(IChange change) {
                    if (HasChange(change)) {
                        changes.Remove(GetChange(change));
                    }
                    else {
                        changeTypes.Add(change.GetType());
                    }

                    changes.Add(change);
                    return true;
                }

                public IChange GetChange(IChange change) {
                    if (changes.Count > 0)
                        return changes.First(c => c.GetType() == change.GetType());
                    return null!;
                }
                public bool HasChange(IChange change) {
                    return changeTypes.Contains(change.GetType());
                }
                public bool HasChange(Type type) {
                    return changeTypes.Contains(type);
                }
            }
        }
    }

}
