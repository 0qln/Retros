using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Retros.WorkstationTableElements {
    public class Slider : IFrameworkElement {
        public System.Windows.Controls.Slider SliderElement { get; }
        public TextBlock TextElement { get; }
        public StackPanel StackPanelElement { get; }
        public Border BorderElement { get; }

        public FrameworkElement FrameworkElement => BorderElement; 


        public Slider(string name) {
            SliderElement = new(); 

            TextElement = new(); 
            TextElement.Text = name;
            TextElement.Foreground = Brushes.White;

            StackPanelElement = new(); 
            StackPanelElement.Orientation = Orientation.Vertical;
            StackPanelElement.Children.Add(TextElement);
            StackPanelElement.Children.Add(SliderElement);

            BorderElement = new Border { Child = StackPanelElement }; 
        }
    }
}
