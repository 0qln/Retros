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
        private HashSet<Type> filterTypes = new(); 
        private List<IChange> filters = new();
        private DispatcherTimer timer = new();
        private WorkstationImage image; 

        public FilterManager(WorkstationImage image) {
            timer.Interval = TimeSpan.FromMilliseconds(400);
            timer.Tick += (s, e) => ApplyChanges();
            timer.Start();

            this.image = image;
        }

        public void Order(List<string> filterStrings) {
            if (filterStrings.Count != filters.Count) return;

            int i = 0; // types
            int j = 0; // strings

            while (i < filters.Count) {
                Debugger.Console.Log(i + ", " + j);

                if (filters[i].GetType().Name != filterStrings[j]) {
                    int tempJ = j;
                    j++;
                    while (filters[i].GetType().Name != filterStrings[j]) {
                        Debugger.Console.Log(j);
                        j++;
                    }
                    //insert
                    var tempI = filters[i];
                    filters.RemoveAt(i);
                    filters.Insert(j, tempI);

                    j = tempJ;
                }
                else {
                    i++;
                    j++;
                }
            }
        }

        public void Clear() {
            filters.Clear();
            filterTypes.Clear();
        }

        public void ApplyChanges() {
            return;
            if (filters.Count > 0) {
                bool changed = false;
                Measure.Execute(() => {
                    filters.ForEach(filter => {
                        if (!filter.Applied && changed == false) {
                            changed = true;
                            Debugger.Console.Log("Changed is true, DummyImage = new()");
                        }
                            image.DummyImage = new WriteableBitmap((BitmapSource)image.SourceImage.Source);
                        //if (changed) {
                            Debugger.Console.Log(filter.GetType().Name);
                            Measure.Execute(filter.Generate);
                        //}
                    });
                    Debugger.Console.Log("Total");
                });
                Debugger.Console.Log("filters: " + filters.Count);
                if (changed) {
                }
                    image.ChangeCurentImage(image.DummyImage);
            }
        }

        // Could cause bugs, better use the other option 
        public bool AddChange(Type changeType) {
            if (ContainsChange(changeType)) {
                return false;
            }

            filterTypes.Add(changeType);
            filters.Add((IChange) Activator.CreateInstance(changeType, image)!);
            return true;
        }
        public bool AddChange(IChange change) {
            if (ContainsChange(change)) {
                return false;
            }

            filterTypes.Add(change.GetType());
            filters.Add(change);
            return true;
        }
        public bool AddChange(IFilterChange filterChange) {
            if (ContainsFilter(filterChange)) {
                return false;
            }

            filterTypes.Add(filterChange.GetType());
            filters.Add((IChange)filterChange);
            return true;
        }

        public void SetFilterIntensity(IFilterChange filter, double value) {
            GetFilter(filter).FilterIntensity = value;
        }

        public void RemoveChange(Type changeType) {
            if (!ContainsChange(changeType)) return;

            filters.Remove(filters.First(c=> changeType == c.GetType()));
            filterTypes.Remove(changeType);
        }
        public void RemoveChange(IChange change) {
            if (!ContainsChange(change)) return;

            filters.Remove(GetChange(change));
            filterTypes.Remove(change.GetType());
        }

        public IFilterChange GetFilter(IFilterChange filter) {
            if (filters.Count > 0) return (IFilterChange)filters.First(c => c.GetType() == filter.GetType());
            return null!;
        }
        public IChange GetChange(IChange change) {
            if (filters.Count > 0) return filters.First(c => c.GetType() == change.GetType());
            return null!;
        }
        public IChange GetChange(Type changeType) {
            if (filters.Count > 0) return filters.First(c => c.GetType() == changeType);
            return null!;
                    
        }
        public bool ContainsChange(IChange change) {
            return filterTypes.Contains(change.GetType());
        }
        public bool ContainsFilter(IFilterChange filter) {
            return filterTypes.Contains(((IChange)filter).GetType());
        }
        public bool ContainsChange(Type type) {
            return filterTypes.Contains(type);
        }
    }
}