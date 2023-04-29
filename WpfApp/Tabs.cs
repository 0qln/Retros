using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Retros.ClientWorkStation.Tab;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using WpfCustomControls;

namespace Retros {
    //TODO
    internal class TabBodies {
        public class ImageEditing {
            internal class Filters : IBody {
                private Grid mainGrid = new();
                private Border border = new();
                public FrameworkElement FrameworkElement => border;


                public Filters() {
                    border.BorderBrush = Helper.StringToSolidColorBrush("#3d3d3d");
                    border.Child = mainGrid;
                    border.BorderThickness = new Thickness(1);

                }


                public void Hide() {

                }
                public void Show() {

                }
            }

            internal class PixelSorting : IBody {
                private Grid mainGrid = new();
                private Border border = new();
                public FrameworkElement FrameworkElement => border;


                public PixelSorting() {
                    border.BorderBrush = Helper.StringToSolidColorBrush("#3d3d3d");
                    border.Child = mainGrid;
                    border.BorderThickness = new Thickness(1);


                }


                public void Hide() {

                }
                public void Show() {

                }
            }

        }


    }

}
