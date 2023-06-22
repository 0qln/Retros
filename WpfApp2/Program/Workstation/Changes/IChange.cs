using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

// This is primarily used for the History
namespace Retros.Program.Workstation.Changes {

    public interface ICloneable {
        public IChange Clone();
    }


    // represents a general change to the images change history 
    public interface IChange : ICloneable {
        public string UITypeName { get; }
    }
    


    // add a layer of abstraction
    public interface IFilter : IChange {
        public void Generate(WriteableBitmap bitmap);
        public bool Applied { get; }
        public double FilterIntensity { get; set; }
    }

    // create a wrapper for the IFilter interface
    // catch/implement/resolve all the contracts 
    public abstract class FilterBase<T> where T : FilterBase<T>, IFilter, new() {
        protected double _filterIntensity;
        protected bool _applied = false;

        public string UITypeName {
            get {
                string output = $"Filter <{typeof(T).Name}>";
                if (Settings.SettingsManager.ImageHistory.ShowFilterIntensityInNameValue) {
                    output += $" {Math.Round(_filterIntensity, 3)}";
                }
                return output;
            }
        }
        public double FilterIntensity {
            get {
                return _filterIntensity;
            }
            set {
                _filterIntensity = value;
                _applied = false;
            }
        }
        public bool Applied => _applied;


        public IChange Clone() {
            var clone = new T();
            clone._applied = _applied;
            clone._filterIntensity = _filterIntensity;
            return clone;
        }

        public override string ToString() {
            return typeof(T).Name.ToString() + " " + _filterIntensity;
        }

        public abstract void Generate(WriteableBitmap writeableBitmap);
    }



    public interface IRemoveFilter : IChange {

    }

    public abstract class RemoveFilterBase : IRemoveFilter {
        public IFilter RemovedFilter { get; }
        public string UITypeName => $"Remove {RemovedFilter.GetType()}"; 


        public RemoveFilterBase(IFilter filter) {
            RemovedFilter = filter;
        }

        public abstract IChange Clone();
    }











    /*
    // old IChange, represents a positive change (e.g. smth that adds to the image)
    public interface IPositiveChange : IChange {
        public void Generate(WriteableBitmap bitmap);
        public bool Applied { get; }
    }

    // represents the removal of a positive change
    public interface INegativeChange : IChange {
        public IPositiveChange? NegativeValue { get; } // the change that was removed
    }

    // keeps track of the changes to the filter hierachy
    public interface IFilterHierachyChange : IChange {
        public IPositiveChange[] FilterHierachy { get; }
    }
    */
    /*
    // represents filters
    public interface IFilterChange : IChange {
        public double FilterIntensity { get; set; }
        public void Generate(WriteableBitmap bitmap);
        public bool Applied { get; }
    }

    // represents a resize 
    public interface IResizeChange : IChange {
        public void Generate(WriteableBitmap bitmap);
        public bool Applied { get; }
    }
    */
}
