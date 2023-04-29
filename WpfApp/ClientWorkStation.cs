using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfCustomControls;

namespace Retros {
    internal class ClientWorkStation {
        private Grid mainGrid = new();
        private Border border = new();

        private RowDefinition tabsRow = new RowDefinition { Height = new GridLength(18) };
        private RowDefinition tabDefinitionRow = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
        private StackPanel tabPanel = new();
        private List<Tab> tabs= new();
        private int currentSelectedTab = 0;

        public FrameworkElement FrameworkElement => border;


        public ClientWorkStation() {
            border.VerticalAlignment = VerticalAlignment.Stretch;
            border.HorizontalAlignment = HorizontalAlignment.Stretch;
            border.Margin = new Thickness(10);
            border.Child = mainGrid;
            border.BorderBrush = Brushes.Transparent;
            border.Background = Brushes.Transparent;
            border.BorderThickness = new Thickness(1);

            mainGrid.RowDefinitions.Add(tabsRow);
            mainGrid.RowDefinitions.Add(tabDefinitionRow);
            Helper.SetChildInGrid(mainGrid, tabPanel, 0, 0);
            
            tabPanel.Orientation = Orientation.Horizontal;
        }

        public void AddTab(Tab tab) {
            tabs.Add(tab);
            tabPanel.Children.Add(tab.FrameworkElement);
            Helper.SetChildInGrid(mainGrid, tabs[tabs.Count-1].body.FrameworkElement, 1, 0);
            tabs[tabs.Count-1].body.Hide();
        }


        public void SelectTab(int index) {
            tabs[currentSelectedTab].body.Hide();
            tabs[index].body.Show();
            currentSelectedTab = index;

        }




        public void SetBG(Brush color) {
            border.Background = color;
        }


        public class Tab  {
            public Handle handle;
            public IBody body;

            private Border border = new();
            public FrameworkElement FrameworkElement => border;


            public Tab(string name, IBody body) {
                handle = new(name);
                border.Child = handle.FrameworkElement;
                this.body = body;
            }
            



            public class Handle {
                private Border border = new();
                private StackPanel stackPanel = new();
                private TextBlock name = new();
                private Image icon = new();

                public FrameworkElement FrameworkElement => border;

                public Handle(string name) {
                    stackPanel.Orientation = Orientation.Horizontal;
                    stackPanel.Children.Add(this.name);
                    stackPanel.Background = Brushes.Transparent;
                    stackPanel.Margin = new Thickness(3, 0, 5, 0);
                    stackPanel.MouseEnter += (s, e) => { border.BorderBrush = Brushes.Gainsboro; };
                    stackPanel.MouseLeave += (s, e) => { border.BorderBrush = Brushes.Transparent; };

                    border.Child = stackPanel;
                    border.Background = Helper.StringToSolidColorBrush("#252525");
                    border.BorderBrush = Brushes.Transparent;
                    border.BorderThickness = new Thickness(0.5);

                    this.name.Text = name;
                    this.name.VerticalAlignment = VerticalAlignment.Center;
                    this.name.Foreground = Brushes.Gainsboro;
                    this.name.TextDecorations = TextDecorations.Underline;

                }

                public void SetIcon(string path) {
                    Helper.SetImageSource(icon, path);
                    stackPanel.Children.Insert(0, icon);
                }
            }

            public interface IBody {
                public FrameworkElement FrameworkElement { get; }
                public void Hide();
                public void Show();
            }
        }
    }

}
