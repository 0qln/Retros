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
using System.Windows.Navigation;
using DebugLibrary;
using System.Windows.Input;
using System.Security.Policy;
using Retros.Program.Workstation.TabUI.Tabs;
using Retros.Program.Workstation.Image;


namespace Retros.Program.Workstation.Changes
{
    // Functionality -> Image Filters Tab Body
    public class FilterManager
    {
        private HashSet<Type> _filterTypes = new();
        private List<IFilter> _filters = new();
        private DispatcherTimer _timer = new();
        private WorkstationImage _image;
        private TimeSpan _interval = TimeSpan.FromMilliseconds(16.66f);

        public event Action? HierachyOrderChanged;


        public IFilter[] CurrentFilters
        {
            set
            {
                _filters = new List<IFilter>();
                for (int i = 0; i < value.Length; i++)
                {
                    _filters.Add((IFilter)value[i].Clone());
                }

                // Update sliders
                Tab? tab = WindowManager.MainWindow!.SelectedWorkstation.TabElement.
                    GetTab(typeof(ImageFilterTab));

                if (tab is null)
                    return;

                ((ImageFilterBody)tab.Body).AdjustSlisers(value);
            }
            get
            {
                IFilter[] arr = new IFilter[_filters.Count];
                for (int i = 0; i < _filters.Count; i++)
                {
                    arr[i] = (IFilter)_filters[i].Clone();
                }
                return arr;
            }
        }
        public void PrintChanges()
        {
            //DebugLibrary.Console.Log("filter list:");
            foreach (var filter in _filters)
            {
                //DebugLibrary.Console.Log(filter.GetType().Name.ToString());
            }
            //DebugLibrary.Console.Log("filter set:");
            foreach (var filter in _filterTypes)
            {
                //DebugLibrary.Console.Log(filter.Name.ToString());
            }
        }


        public FilterManager(WorkstationImage image)
        {
            _timer.Interval = _interval;
            _timer.Tick += (s, e) => ApplyFilters();
            _timer.Start();

            HierachyOrderChanged += SendHierachyOrderChangeToHistory;

            _image = image;
        }


        public void Order(List<string> filterStrings)
        {
            if (_filters.Count == 0) return;
            if (filterStrings.Count != _filters.Count) return;

            int i = 0; // types
            int j = 0; // strings

            while (i < _filters.Count)
            {

                if (_filters[i].GetType().Name != filterStrings[j])
                {
                    int tempJ = j;
                    j++;
                    while (_filters[i].GetType().Name != filterStrings[j])
                    {
                        j++;
                    }
                    //insert
                    var tempI = _filters[i];
                    _filters.RemoveAt(i);
                    _filters.Insert(j, tempI);

                    j = tempJ;
                }
                else
                {
                    i++;
                    j++;
                }
            }
            changed = true;

            HierachyOrderChanged?.Invoke();
        }

        public void SendHierachyOrderChangeToHistory()
        {
            _image.GetHistoryManager.AddAndStep(ImageState.FromImage(_image));
        }

        public void Clear()
        {
            _filters.Clear();
            _filterTypes.Clear();
        }

        private bool justRemoved = false;
        private bool changed = false;
        public unsafe void ApplyFilters()
        {
            if (!changed)
            {
                return;
            }

            if (_filters.Count == 0)
            {
                if (justRemoved)
                {
                    _image.ChangeCurrentImages(_image.ResizedSourceBitmap);
                    justRemoved = false;
                }
                return;
            }

            Task.Run(() =>
            {
                WriteableBitmap bitmap = _image.GetDownscaledSourceImage();
                ApplyChanges(bitmap);
                bitmap.Freeze();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _image.ChangeCurrentImages(bitmap);
                });
                MessageBox.Show("1");
            });
            MessageBox.Show("2");

            changed = false;
        }
        public void ApplyChanges(WriteableBitmap bitmap)
        {
            for (int i = _filters.Count - 1; i >= 0; i--)
            {
                //MessageBox.Show(Measure.Execute(() => _filters[i].Generate(bitmap)).ElapsedMilliseconds.ToString());
                _filters[i].Generate(bitmap);
            }
        }

        public bool AddFilter(IFilter filter)
        {
            if (ContainsFilter(filter)) return false;

            _filters.Add(filter);
            _filterTypes.Add(filter.GetType());

            changed = true;

            return true;
        }
        public void RemoveFilter<T>() where T : IFilter
        {
            if (!ContainsFilter<T>()) return;

            _filterTypes.Remove(typeof(T));
            _filters.Remove(GetFilter<T>()!);
            changed = true;
            justRemoved = true;
        }
        public void RemoveFilter(IFilter filter)
        {
            if (!ContainsFilter(filter)) return;

            _filterTypes.Remove(filter.GetType());
            _filters.Remove(GetFilter(filter)!);
            changed = true;
            justRemoved = true;
        }

        public void SetFilterIntensity(IFilter filter, double value)
        {
            if (ContainsFilter(filter))
            {
                GetFilter(filter)!.FilterIntensity = value;
                changed = true;
            }
        }
        public void SetFilterIntensity<T>(double value) where T : IFilter
        {
            if (ContainsFilter<T>())
            {
                GetFilter<T>()!.FilterIntensity = value;
                changed = true;
            }
        }


        public IFilter? GetFilter(IFilter pFilter)
        {
            return _filters.Count > 0
                ? _filters.FirstOrDefault(filter => filter.GetType() == pFilter.GetType())
                : null;
        }
        public IFilter? GetFilter<T>() where T : IFilter
        {
            return _filters.Count > 0
                ? _filters.FirstOrDefault(filter => filter.GetType() == typeof(T))
                : null;
        }
        public bool ContainsFilter<T>() where T : IFilter
        {
            return _filterTypes.Contains(typeof(T));
        }
        public bool ContainsFilter(IFilter filter)
        {
            return _filterTypes.Contains((filter).GetType());
        }

    }
}