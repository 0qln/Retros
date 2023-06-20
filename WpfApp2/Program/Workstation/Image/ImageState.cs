using Retros.Program.Workstation.Changes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retros.Program.Workstation.Image {
    public class ImageState {

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

        public static string IdentifyDifference(ImageState? prev, ImageState next) {
            if (prev is null) {
                //root node 
            }
            //...



            return "";
        }
    }
}
