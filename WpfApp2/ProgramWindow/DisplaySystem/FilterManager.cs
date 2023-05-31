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

namespace Retros.ProgramWindow.DisplaySystem {
    // Functionality -> Image Filters Tab Body
    public class FilterManager {
        private HashSet<Type> filterTypes = new(); 
        private List<IChange> filters = new();
        private DispatcherTimer timer = new();
        private WorkstationImage image; 

        public FilterManager(WorkstationImage image) {
            timer.Interval = TimeSpan.FromMilliseconds(250);
            timer.Tick += (s, e) => ApplyChanges();
            timer.Start();

            this.image = image;
        }

        public void Order(List<string> filterStrings) {
            if (filterStrings.Count != filters.Count) return;

            int i = 0; // types
            int j = 0; // strings

            while (i < filters.Count) {

                if (filters[i].GetType().Name != filterStrings[j]) {
                    int tempJ = j;
                    j++;
                    while (filters[i].GetType().Name != filterStrings[j]) {
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
            if (filters.Count == 0) {
                return;
            }

            DebugLibrary.Console.Log(Measure.Execute(() => { 
                image.DummyImage = new WriteableBitmap(image.ResizedSourceBitmap);

            }).ElapsedMilliseconds);
            filters.ForEach(filter => {
                ///filter.Generate();
                DebugLibrary.Console.Log(Measure.Execute(filter.Generate).ElapsedMilliseconds);
            });
            image.ChangeCurentImage(image.DummyImage);
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