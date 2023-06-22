using Retros.Program.Workstation.Changes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retros.Program.Workstation.Image {
    public class ImageState {
        private IChange _changeFromPrev;

        public IFilter[] Filters { get; }


        public ImageState() {
            Filters = new IFilter[0];    
        }
        public ImageState(WorkstationImage image) {
            Filters = image.GetFilterManager.CurrentFilters;
        }


        public static ImageState FromImage(WorkstationImage image) {
            return new ImageState(image);
        }
        public static ImageState FromEmpty() {
            return new ImageState();
        }
        public ImageState SetChangeFromPrev(IChange changeFromPrev) {
            if (_changeFromPrev is not null) {
                _changeFromPrev = changeFromPrev;
            }
            return this;
        }

        public static string IdentifyDifference(ImageState? prev, ImageState next) {
            if (prev is null) {
                //root node 
                return "Loaded Image into workspace";
            }

            //...compare filters
            bool equalFilterLength = prev.Filters.Length == next.Filters.Length;
            int greatestFilterLength = 
                equalFilterLength 
                ? prev.Filters.Length 
                : (prev.Filters.Length > next.Filters.Length ? prev.Filters.Length : next.Filters.Length);

            if (equalFilterLength) {
                for (int i = 0; i < prev.Filters.Length; i++) {
                    DebugLibrary.Console.Log(prev.GetHashCode() + ", " + next.GetHashCode());
                    DebugLibrary.Console.Log(prev.Filters.GetHashCode() + ", " + next.Filters.GetHashCode());
                    DebugLibrary.Console.Log(prev.Filters[i].ToString()! + ", " + next.Filters[i].ToString()!);
                    DebugLibrary.Console.Log(prev.Filters[i].GetHashCode().ToString()! + ", " + next.Filters[i].GetHashCode().ToString()!);

                    if (prev.Filters[i].GetType() != next.Filters[i].GetType()) {
                        return "Filter order changed";
                    }
                    DebugLibrary.Console.Log(next.Filters[i].ToString()!);
                    DebugLibrary.Console.Log(prev.Filters[i].ToString()!);
                    if (prev.Filters[i].FilterIntensity != next.Filters[i].FilterIntensity) {
                        return "Filter intensity changed";
                    }
                }
            }
            else {
                return "Added/removed filter";
            }

            return "";
        }
    }
}
