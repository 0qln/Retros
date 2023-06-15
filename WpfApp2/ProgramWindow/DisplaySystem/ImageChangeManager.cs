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
using static System.Net.Mime.MediaTypeNames;

namespace Retros.ProgramWindow.DisplaySystem {
    // Functionality -> Image Filters Tab Body
    public class ImageChangeManager {
        private HashSet<Type> _changeTypes = new(); 
        private List<IPositiveChange> _changes = new();
        private DispatcherTimer _timer = new();
        private WorkstationImage _image;
        private TimeSpan _interval = TimeSpan.FromMilliseconds(16.66f);
        private ChangeHistory? _history;

        public event Action? HierachyOrderChanged;


        public IPositiveChange[] CurrentChanges {
            set {
                _changes = value.ToList();

                // Update sliders
                Tab? tab = WindowManager.MainWindow!.SelectedWorkstation.TableElement.
                    GetTab(typeof(ImageFilterTab));

                if (tab is null) 
                    return;

                ((ImageFilter)tab.Body).AdjustSlisers(value);
            }
            get {
                IPositiveChange[] arr = new IPositiveChange[_changes.Count];
                for (int i = 0; i < _changes.Count; i++) {
                    arr[i] = (IPositiveChange)_changes[i].Clone();
                }
                return arr;
            }
        }
        public void PrintChanges() {
            DebugLibrary.Console.Log("Changes:");
            foreach (var change in _changes) {
                DebugLibrary.Console.Log(change.GetType().Name.ToString());
            }
        }


        public ImageChangeManager(WorkstationImage image) {
            _timer.Interval = _interval;
            _timer.Tick += (s, e) => ApplyChanges();
            _timer.Start();

            _history = image.GetHistoryManager;

            HierachyOrderChanged += SendHierachyOrderUpdateToHistory;

            _image = image;
        }


        public void Order(List<string> filterStrings) {
            if (filterStrings.Count != _changes.Count) return;

            int i = 0; // types
            int j = 0; // strings

            while (i < _changes.Count) {

                if (_changes[i].GetType().Name != filterStrings[j]) {
                    int tempJ = j;
                    j++;
                    while (_changes[i].GetType().Name != filterStrings[j]) {
                        j++;
                    }
                    //insert
                    var tempI = _changes[i];
                    _changes.RemoveAt(i);
                    _changes.Insert(j, tempI);

                    j = tempJ;
                }
                else {
                    i++;
                    j++;
                }
            }
            changed = true;

            HierachyOrderChanged?.Invoke();
        }

        public void SendHierachyOrderUpdateToHistory() {
            _history?.Add(new FilterHierachyChange(CurrentChanges));
            PrintChanges();
        }

        public void Clear() {
            _changes.Clear();
            _changeTypes.Clear();
        }

        private bool justRemoved = false;
        private bool changed = false;
        public void ApplyChanges() {
            if (!changed) {
                return;
            }

            if (_changes.Count == 0) {
                if (justRemoved) {
                    _image.ChangeCurentImage(_image.ResizedSourceBitmap);
                    justRemoved = false;
                }
                return;
            }
            
            
            _image.ChangeCurentImage (ApplyChanges (new WriteableBitmap (_image.ResizedSourceBitmap)));


            changed = false;
        }
        public WriteableBitmap ApplyChanges(WriteableBitmap bitmap) {
            for (int i = _changes.Count - 1; i >= 0; i--) {
                _changes[i].Generate(bitmap);
            }
            return bitmap;
        }

        public bool AddChange(IPositiveChange change) {
            if (ContainsChange(change)) {
                return false;
            }

            _changeTypes.Add(change.GetType());
            _changes.Add(change);
            changed = true;

            return true;
        }
        public void RemoveChange(INegativeChange change) {
            if (!ContainsChangeContent(change)) {
                return;
            }

            _changeTypes.Remove(change.ValueType);
            _changes.Remove(GetChange(change.Value!)!);
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
            return (_changes.Count > 0) 
                ? (IFilterChange)_changes.First(c => c.GetType() == filter.GetType())
                : null;
        }
        public IPositiveChange? GetChange(IPositiveChange change) {
            return _changes.Count > 0 
                ? _changes.First(c => c.GetType() == change.GetType()) 
                : null;
        }
        public IPositiveChange? GetChange(Type changeType) {
            return _changes.Count > 0 
                ? _changes.First(c => c.GetType() == changeType) 
                : null;
        }
        public bool ContainsChange(IPositiveChange change) {
            return _changeTypes.Contains(change.GetType());
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
            return _changeTypes.Contains(((IPositiveChange)filter).GetType());
        }
        public bool ContainsChange(Type type) {
            return _changeTypes.Contains(type);
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