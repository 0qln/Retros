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
using System.Windows.Input;
using System.Security.Policy;
using Retros.ProgramWindow.Interactive.Tabs;
using Retros.ProgramWindow.Interactive.Tabs.Bodies;

namespace Retros.ProgramWindow.DisplaySystem {
    // Functionality -> Image Filters Tab Body
    public class ImageChangeManager {
        private HashSet<Type> changeTypes = new(); 
        private List<IPositiveChange> changes = new();
        private DispatcherTimer timer = new();
        private WorkstationImage image;
        private TimeSpan Interval = TimeSpan.FromMilliseconds(16.66f);
        private ChangeHistory? history;


        public IPositiveChange[] CurrentChanges {
            set {
                changes = value.ToList();

                // update sliders
                Tab? tab = WindowManager.MainWindow!.SelectedWorkstation.TableElement.
                    GetTab(typeof(ImageFilterTab));

                if (tab is null) 
                    return;

                ((ImageFilter)tab.Body).AdjustSlisers(value);


                // Update filter hierachy tab

            }
            get {
                IPositiveChange[] arr = new IPositiveChange[changes.Count];
                for (int i = 0; i < changes.Count; i++) {
                    arr[i] = (IPositiveChange)changes[i].Clone();
                }
                return arr;
            }
        }
        public void PrintChanges() {
            DebugLibrary.Console.Log("Changes:");
            foreach (var change in changes) {
                DebugLibrary.Console.Log(change.GetType().Name.ToString());
            }
        }


        public ImageChangeManager(WorkstationImage image) {
            timer.Interval = Interval;
            timer.Tick += (s, e) => ApplyChanges();
            timer.Start();

            history = image.GetHistoryManager;
            
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

        public bool AddChange(IPositiveChange change) {
            if (ContainsChange(change)) {
                return false;
            }

            changeTypes.Add(change.GetType());
            changes.Add(change);
            changed = true;

            return true;
        }
        public void RemoveChange(INegativeChange change) {
            if (!ContainsChangeContent(change)) {
                return;
            }

            changeTypes.Remove(change.ValueType);
            changes.Remove(GetChange(change.Value!)!);
            changed = true;
            justRemoved = true;
        }

        public void SetFilterIntensity(IFilterChange filter, double value) {
            if (ContainsFilter(filter)) {
                GetFilter(filter)!.FilterIntensity = value;
                changed = true;
            }
        }
        public void SetFilterIntensity(Type filter, double value) {
            if (ContainsChange(filter)) {
                ((IFilterChange)GetChange(filter)).FilterIntensity = value;
                changed = true;
            }
        }


        public IFilterChange? GetFilter(IFilterChange filter) {
            return (changes.Count > 0) 
                ? (IFilterChange)changes.First(c => c.GetType() == filter.GetType())
                : null;
        }
        public IPositiveChange? GetChange(IPositiveChange change) {
            return changes.Count > 0 
                ? changes.First(c => c.GetType() == change.GetType()) 
                : null;
        }
        public IPositiveChange? GetChange(Type changeType) {
            return changes.Count > 0 
                ? changes.First(c => c.GetType() == changeType) 
                : null;
        }
        public bool ContainsChange(IPositiveChange change) {
            return changeTypes.Contains(change.GetType());
        }
        public bool ContainsChangeContent(INegativeChange change) {
            if (ContainsChange(change.ValueType)) {
                return true;
            }
            if (change.Value is not null
                && ContainsChange(change.Value)) {
                return true;
            }
            return false;
        }
        public bool ContainsFilter(IFilterChange filter) {
            return changeTypes.Contains(((IPositiveChange)filter).GetType());
        }
        public bool ContainsChange(Type type) {
            return changeTypes.Contains(type);
        }


        public class UndoCommand : ICommand {
            public event EventHandler? CanExecuteChanged;

            public bool CanExecute(object? parameter) {
                return true;
            }

            public void Execute(object? parameter) {
                WindowManager.MainWindow?.SelectedWorkstation.ImageElement.GetHistoryManager.Undo();
            }
        }

        public class RedoCommand : ICommand {
            public event EventHandler? CanExecuteChanged;

            public bool CanExecute(object? parameter) {
                return true;
            }

            public void Execute(object? parameter) {
                WindowManager.MainWindow?.SelectedWorkstation.ImageElement.GetHistoryManager.Redo();
            }
        }
    }
}