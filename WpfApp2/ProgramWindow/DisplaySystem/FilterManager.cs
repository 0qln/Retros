using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.Windows;
using System.IO;
using System.Diagnostics;
using System.Drawing.Imaging;
using DebugLibrary.Benchmark;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics;
using System.Threading;
using System.Windows.Threading;
using Retros;
using System.Windows.Navigation;
using DebugLibrary;
using static System.Net.WebRequestMethods;

namespace Retros.ProgramWindow.DisplaySystem {
    // Functionality -> Image Filters Tab Body
    public class ImageChangeManager {
        private HashSet<Type> changeTypes = new(); 
        private List<IChange> changes = new();
        private DispatcherTimer timer = new();
        private WorkstationImage image;
        private TimeSpan Interval = TimeSpan.FromMilliseconds(100);

        public ImageChangeManager(WorkstationImage image) {
            timer.Interval = Interval;
            timer.Tick += (s, e) => ApplyChanges();
            timer.Start();

            this.image = image;
        }

        public void Order(List<string> filterStrings) {
            if (filterStrings.Count != changes.Count) return;

            int i = 0; // types
            int j = 0; // strings

            while (i < changes.Count) {

                if (changes[i].GetType().Name != filterStrings[j]) {
                    int tempJ = j;
                    j++;
                    while (changes[i].GetType().Name != filterStrings[j]) {
                        j++;
                    }
                    //insert
                    var tempI = changes[i];
                    changes.RemoveAt(i);
                    changes.Insert(j, tempI);

                    j = tempJ;
                }
                else {
                    i++;
                    j++;
                }
            }

            changed = true;
            ApplyChanges();
        }

        public void Clear() {
            changes.Clear();
            changeTypes.Clear();
        }

        private bool justRemoved = false;
        private bool changed = false;
        public void ApplyChanges() {
            if (!changed) {
                return;
            }

            if (changes.Count == 0) {
                if (justRemoved) {
                    image.ChangeCurentImage(image.ResizedSourceBitmap);
                    justRemoved = false;
                }
                return;
            }
            
            image.ChangeCurentImage (ApplyChanges (new WriteableBitmap (image.ResizedSourceBitmap)));

            changed = false;
        }
        public WriteableBitmap ApplyChanges(WriteableBitmap bitmap) {
            for (int i = changes.Count - 1; i >= 0; i--) {
                changes[i].Generate(bitmap);
            }
            return bitmap;
        }

        // Could cause bugs, better use the other option 
        public bool AddChange(Type changeType) {
            if (ContainsChange(changeType)) {
                return false;
            }

            changeTypes.Add(changeType);
            changes.Add((IChange) Activator.CreateInstance(changeType, image)!);
            changed = true;

            return true;
        }
        public bool AddChange(IChange change) {
            if (ContainsChange(change)) {
                return false;
            }

            changeTypes.Add(change.GetType());
            changes.Add(change);
            changed = true;

            return true;
        }
        public bool AddFilter(IFilterChange filterChange) {
            if (ContainsFilter(filterChange)) {
                return false;
            }

            changeTypes.Add(filterChange.GetType());
            changes.Add((IChange)filterChange);
            changed = true;

            return true;
        }

        public void SetFilterIntensity(IFilterChange filter, double value) {
            GetFilter(filter).FilterIntensity = value;
            changed = true;
        }

        public void RemoveChange(Type changeType) {
            if (!ContainsChange(changeType)) return;

            changes.Remove(changes.First(c=> changeType == c.GetType()));
            changeTypes.Remove(changeType);
            changed = true;
            justRemoved = true;
        }
        public void RemoveChange(IChange change) {
            if (!ContainsChange(change)) return;

            changes.Remove(GetChange(change));
            changeTypes.Remove(change.GetType());
            changed = true;
            justRemoved = true;
        }

        public IFilterChange GetFilter(IFilterChange filter) {
            if (changes.Count > 0) return (IFilterChange)changes.First(c => c.GetType() == filter.GetType());
            return null!;
        }
        public IChange GetChange(IChange change) {
            if (changes.Count > 0) return changes.First(c => c.GetType() == change.GetType());
            return null!;
        }
        public IChange GetChange(Type changeType) {
            if (changes.Count > 0) return changes.First(c => c.GetType() == changeType);
            return null!;
                    
        }
        public bool ContainsChange(IChange change) {
            return changeTypes.Contains(change.GetType());
        }
        public bool ContainsFilter(IFilterChange filter) {
            return changeTypes.Contains(((IChange)filter).GetType());
        }
        public bool ContainsChange(Type type) {
            return changeTypes.Contains(type);
        }
    }
}