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

namespace Retros.ProgramWindow.DisplaySystem {
    // Functionality -> Image Filters Tab Body
    public class FilterManager {
        private HashSet<Type> changeTypes = new(); 
        private List<IChange> changes = new();
        private DispatcherTimer timer = new();
        private WorkstationImage image; 

        public FilterManager(WorkstationImage image) {
            timer.Interval = WindowManager.Framerate;
            timer.Tick += (s, e) => ApplyChanges();
            timer.Start();

            this.image = image;
        }

        public void Clear() {
            changes.Clear();
            changeTypes.Clear();
        }

        public void ApplyChanges() {
            if (changes.Count > 0) {
                image.DummyImage = new WriteableBitmap((BitmapSource)image.Original.Source);
                changes.ForEach(change => {
                    change.Apply();
                });
                image.CurrentImage.Source = image.DummyImage;
            }
        }

        // Could cause bugs, better use the other option 
        public bool AddChange(Type changeType) {
            if (ContainsChange(changeType)) {
                return false;
            }

            changeTypes.Add(changeType);
            changes.Add((IChange) Activator.CreateInstance(changeType, image)!);
            return true;
        }
        public bool AddChange(IChange change) {
            if (ContainsChange(change)) {
                return false;
            }

            changeTypes.Add(change.GetType());
            changes.Add(change);
            return true;
        }
        public bool AddChange(IFilterChange filterChange) {
            if (ContainsFilter(filterChange)) {
                return false;
            }

            changeTypes.Add(filterChange.GetType());
            changes.Add((IChange)filterChange);
            return true;
        }

        public void SetFilterIntensity(IFilterChange filter, double value) {
            GetFilter(filter).FilterIntensity = value;
        }

        public void RemoveChange(Type changeType) {
            if (!ContainsChange(changeType)) return;

            changes.Remove(changes.First(c=> changeType == c.GetType()));
            changeTypes.Remove(changeType);
        }
        public void RemoveChange(IChange change) {
            if (!ContainsChange(change)) return;

            changes.Remove(GetChange(change));
            changeTypes.Remove(change.GetType());
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