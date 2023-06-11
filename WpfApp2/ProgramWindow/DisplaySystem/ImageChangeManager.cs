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
using System.Windows.Input;

namespace Retros.ProgramWindow.DisplaySystem {
    // Functionality -> Image Filters Tab Body
    public class ImageChangeManager {
        private HashSet<Type> changeTypes = new(); 
        private List<IPositiveChange> changes = new();
        private DispatcherTimer timer = new();
        private WorkstationImage image;
        private TimeSpan Interval = TimeSpan.FromMilliseconds(16.66f);
        private ChangeHistory? history;


        public ImageChangeManager(WorkstationImage image) {
            timer.Interval = Interval;
            timer.Tick += (s, e) => ApplyChanges();
            timer.Start();

            history = image.GetHistory;
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

        public bool AddChange(IPositiveChange change) {
            if (ContainsChange(change)) {
                return false;
            }

            changeTypes.Add(change.GetType());
            changes.Add(change);
            changed = true;

            return true;
        }
        public void RemoveChange(IPositiveChange change) {
            if (!ContainsChange(change)) return;

            changes.Remove(GetChange(change));
            changeTypes.Remove(change.GetType());
            changed = true;
            justRemoved = true;

            history?.Undo();
        }

        public void SetFilterIntensity(IFilterChange filter, double value) {
            GetFilter(filter).FilterIntensity = value;
            changed = true;
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
                WindowManager.MainWindow.SelectedWorkstation.ImageElement.GetHistory.Undo();
            }
        }

        public class RedoCommand : ICommand {
            public event EventHandler? CanExecuteChanged;

            public bool CanExecute(object? parameter) {
                return true;
            }

            public void Execute(object? parameter) {
                WindowManager.MainWindow.SelectedWorkstation.ImageElement.GetHistory.Redo();
            }
        }
    }
}