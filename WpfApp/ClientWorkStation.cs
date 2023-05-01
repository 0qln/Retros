using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfCustomControls;

namespace Retros {
    internal static class ClientWorkStation {
        private static Grid mainGrid = new();
        private static Border border = new();

        private static RowDefinition tabsRow = new RowDefinition { Height = new GridLength(18) };
        private static RowDefinition tabDefinitionRow = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
        private static StackPanel tabPanel = new();
        private static List<Tab> tabs= new();
        private static int currentSelectedTab = 0;

        public static FrameworkElement FrameworkElement => border;


        public static void Instanciate() {
            border.VerticalAlignment = VerticalAlignment.Stretch;
            border.HorizontalAlignment = HorizontalAlignment.Stretch;
            border.Margin = new Thickness(10);
            border.Child = mainGrid;
            border.BorderBrush = System.Windows.Media.Brushes.Transparent;
            border.Background = System.Windows.Media.Brushes.Transparent;
            border.BorderThickness = new Thickness(1);

            mainGrid.RowDefinitions.Add(tabsRow);
            mainGrid.RowDefinitions.Add(tabDefinitionRow);
            Helper.SetChildInGrid(mainGrid, tabPanel, 0, 0);
            
            tabPanel.Orientation = Orientation.Horizontal;
        }

        public static void AddTab(Tab tab) {
            tabs.Add(tab);
            tab.Index = tabs.Count - 1;
            tabPanel.Children.Add(tab.FrameworkElement);
            Helper.SetChildInGrid(mainGrid, tabs[tab.Index].body.FrameworkElement, 1, 0);
            tabs[tab.Index].body.Hide();

            tab.handle.FrameworkElement.MouseDown += (s, e) => { 
                SelectTab(tab.Index);    
            };
        }


        public static void SelectTab(int index) {
            tabs[currentSelectedTab].body.Hide();
            tabs[index].body.Show();
            currentSelectedTab = index; 
        }


        public static class WorkstationImage {
            private static string Path = "C:\\Users\\User\\OneDrive\\Bilder\\Wallpapers\\mountain-lake-reflection-nature-scenery-hd-wallpaper-uhdpaper.com-385@0@h.jpg";
            public static string GetPath => Path;

            public static Bitmap ?bitmap;
            public static System.Windows.Controls.Image Image = new();

            public static void SetSource(string path) {
                Path = path;
                bitmap = new(path);
                Helper.SetImageSource(Image, path);
            }
        }


        public class Tab  {
            public Handle handle;
            public IBody body;

            private Border border = new();
            public FrameworkElement FrameworkElement => border;

            private int index = -1;
            public int Index { get => index; set => index = value; }


            public Tab(string name, IBody body) {
                handle = new(name);
                border.Child = handle.FrameworkElement;
                this.body = body;
            }
            



            public class Handle {
                private Border border = new();
                private StackPanel stackPanel = new();
                private TextBlock name = new();
                private System.Windows.Controls.Image icon = new();

                public FrameworkElement FrameworkElement => border;

                public Handle(string name) {
                    stackPanel.Orientation = Orientation.Horizontal;
                    stackPanel.Children.Add(this.name);
                    stackPanel.Background = System.Windows.Media.Brushes.Transparent;
                    stackPanel.Margin = new Thickness(3, 0, 5, 0);
                    stackPanel.MouseEnter += (s, e) => { border.BorderBrush = System.Windows.Media.Brushes.Gainsboro; };
                    stackPanel.MouseLeave += (s, e) => { border.BorderBrush = System.Windows.Media.Brushes.Transparent; };

                    border.Child = stackPanel;
                    border.Background = Helper.StringToSolidColorBrush("#252525");
                    border.BorderBrush = System.Windows.Media.Brushes.Transparent;
                    border.BorderThickness = new Thickness(0.5);

                    this.name.Text = name;
                    this.name.VerticalAlignment = VerticalAlignment.Center;
                    this.name.Foreground = System.Windows.Media.Brushes.Gainsboro;
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
