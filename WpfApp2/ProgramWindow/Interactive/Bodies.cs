﻿using Retros.ProgramWindow.DisplaySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Utillities.Wpf;

// Bodies Manage the Tab body UIElements and functionality
namespace Retros.ProgramWindow.Interactive.Tabs.Bodies {
    public abstract class Body {
        protected readonly WorkstationImage image;

        protected StackPanel stackPanel = new();
        protected Grid mainGrid = new();
        protected Border border = new();
        public FrameworkElement FrameworkElement => border;

        public Body(WorkstationImage image) {
            this.image = image;
            UIManager.ColorThemeManager.Set_BC1(newBrush => border.BorderBrush = newBrush);
            border.Child = mainGrid;
            border.BorderThickness = new Thickness(1);
            Helper.SetChildInGrid(mainGrid, stackPanel, 0, 0);
        }

        public void Hide() => mainGrid.Visibility = Visibility.Collapsed;
        public void Show() => mainGrid.Visibility = Visibility.Visible;
    }


    public class ImageFilter : Body {
        public Button resetButton = new();

        public Slider blueChannelSlider = new("Blue Channel");
        public Slider redChannelSlider = new("Red Channel");
        public Slider greenChannelSlider = new("Green Channel");
        public Slider grayscaleSlider = new("Grayscale");
        public Slider testBlueSlider = new("TestBlue");
        public FilterDisplay FilterDisplay;

        public ImageFilter(WorkstationImage image) : base(image) {
            FilterDisplay = new(image);

            stackPanel.Children.Add(resetButton);
            stackPanel.Children.Add(grayscaleSlider.FrameworkElement);
            stackPanel.Children.Add(blueChannelSlider.FrameworkElement);
            stackPanel.Children.Add(redChannelSlider.FrameworkElement);
            stackPanel.Children.Add(greenChannelSlider.FrameworkElement);
            stackPanel.Children.Add(testBlueSlider.FrameworkElement);
            stackPanel.Children.Add(FilterDisplay.FrameworkElement);

            UIManager.ColorThemeManager.Set_AC1(b => resetButton.Background = b);
            resetButton.Click += ResetButton_Click;
            resetButton.Content = "Back to original";

            grayscaleSlider.SliderElement.ValueChanged += (s, e) => AddFilterChange(new GrayScale(), grayscaleSlider.SliderElement.Value / 10);
            grayscaleSlider.SliderElement.PreviewMouseUp += (s, e) => {
                if (grayscaleSlider.SliderElement.Value != 0)
                    image.GetHistory.AddAndStep(image.GetFilterManager.GetChange(typeof(GrayScale)));
                else {

                }
                    
            };

            blueChannelSlider.SliderElement.ValueChanged += (s, e) => AddFilterChange(new OnlyBlueChannel(), blueChannelSlider.SliderElement.Value / 10);
            blueChannelSlider.SliderElement.PreviewMouseUp += (s, e) => image.GetHistory.AddAndStep(image.GetFilterManager.GetChange(typeof(OnlyBlueChannel))!);

            redChannelSlider.SliderElement.ValueChanged += (s, e) => AddFilterChange(new OnlyRedChannel(), redChannelSlider.SliderElement.Value / 10);
            redChannelSlider.SliderElement.PreviewMouseUp += (s, e) => image.GetHistory.AddAndStep(image.GetFilterManager.GetChange(typeof(OnlyRedChannel))!);

            greenChannelSlider.SliderElement.ValueChanged += (s, e) => AddFilterChange(new OnlyGreenChannel(), greenChannelSlider.SliderElement.Value / 10);
            greenChannelSlider.SliderElement.PreviewMouseUp += (s, e) => image.GetHistory.AddAndStep(image.GetFilterManager.GetChange(typeof(OnlyGreenChannel))!);

            testBlueSlider.SliderElement.ValueChanged += (s, e) => AddFilterChange(new TestBlue(), testBlueSlider.SliderElement.Value / 10);
            testBlueSlider.SliderElement.PreviewMouseUp += (s, e) => image.GetHistory.AddAndStep(image.GetFilterManager.GetChange(typeof(TestBlue))!);

        }

        private void ResetButton_Click(object sender, RoutedEventArgs e) {
            image.GetFilterManager.Clear();
            image.ResetCurrent();
        }

        private void AddFilterChange(IFilterChange filter, double value) {
            if (value == 0) {
                image.GetFilterManager.RemoveChange((IChange)filter);
                FilterDisplay.RemoveItem(filter.GetType().Name);
            }
            else {
                if (image.GetFilterManager.AddChange((IChange)filter)) {
                    FilterDisplay.AddItem(filter.GetType().Name);
                }
                else {
                    image.GetFilterManager.SetFilterIntensity(filter, value);
                }
            }
        }
    }

    public class ImageHistory : Body {
        private ChangeHistory _history;
        private Canvas _canvas = new();
        private Node _root;
        private Node _currentNode;
        private HashSet<Node> _allNodes = new();

        public ImageHistory(WorkstationImage image) : base(image) {
            _history = image.GetHistory;

            _root = new Node(null, _history.RootChange, _history);
            _currentNode = _root;

            _history.AllChildrenRemoved += (parent) => {
                Node parentNode = parent == _currentNode.Source
                    ? _currentNode 
                    : _allNodes.First(node => node.Source == parent);

                foreach (Node node in parentNode.Children) {
                    _allNodes.Remove(node);
                }
                
                parentNode.Cut();
            };
            _history.PositionChanged += (newCurrentNode) => {
                _currentNode.SetHighlight(false);

                // Update _current
                _currentNode = _allNodes.First(node => node.Source == newCurrentNode);
                /*
                var _newCurrentNode = _root.Source == newCurrentNode
                    ? _root
                    : _root.First(child => child.Source == newCurrentNode);

                _currentNode = _newCurrentNode is not null
                    ? _newCurrentNode
                    : _currentNode;
                */

                _currentNode.SetHighlight(true);
                UIManager.ColorThemeManager.UpdateColors();
            };
            _history.MoveBack += (uint steps) => {
                _currentNode.SetHighlight(false);

                int i = 0;
                while (i < steps && _currentNode.Previous is not null) {
                    _currentNode = _currentNode.Previous;
                    i++;
                }

                _currentNode.SetHighlight(true);
                UIManager.ColorThemeManager.UpdateColors();
            };
            _history.MoveForward += (uint[] steps) => {
                _currentNode.SetHighlight(false);

                int i = 0;
                Node? next = _currentNode.Next(steps[i]);
                while (i < steps.Length && next is not null) {
                    _currentNode = next;
                    next = _currentNode.Next(steps[i]);
                    i++;
                }

                _currentNode.SetHighlight(true);
                UIManager.ColorThemeManager.UpdateColors();
            };
            _history.ChangeAdded += (newChange) => {
                var newNode = new Node(_currentNode, newChange, _history);
                _currentNode.Add(newNode);
                _allNodes.Add(newNode);
            };
            _history.ChangeRemoved += (index) => {
                var cutNode = _currentNode.CutNode(index);
                if (cutNode is not null) _allNodes.Remove(cutNode);
            };


            stackPanel.Children.Add(_canvas);
            _canvas.Children.Add(_root.FrameworkElement);
        }

        public static Style NodeStyle() {
            Style style = new Style(typeof(Button));

            style.Setters.Add(new Setter(Button.PaddingProperty, new Thickness(10, 0, 10, 0)));
            style.Setters.Add(new Setter(Button.BackgroundProperty, UIManager.ColorThemeManager.Current.BG6));
            style.Setters.Add(new Setter(Button.ForegroundProperty, UIManager.ColorThemeManager.Current.FC1));
            style.Setters.Add(new Setter(Button.BorderThicknessProperty, new Thickness(0)));
            style.Setters.Add(new Setter(Button.HorizontalAlignmentProperty, HorizontalAlignment.Left));
            style.Setters.Add(new Setter(Button.VerticalAlignmentProperty, VerticalAlignment.Center));
            style.Setters.Add(new Setter(Button.HeightProperty, 20.0));
            style.Setters.Add(new Setter(Button.FontSizeProperty, 8.0));
            style.Setters.Add(new Setter(Button.ContentProperty, new TextBlock { Margin = new Thickness(15, 0, 10, 0) }));


            ControlTemplate buttonTemplate = new ControlTemplate(typeof(Button));
            FrameworkElementFactory borderFactory = new FrameworkElementFactory(typeof(Border));
            borderFactory.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Button.BackgroundProperty));
            borderFactory.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Button.BorderBrushProperty));
            borderFactory.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Button.BorderThicknessProperty));

            FrameworkElementFactory contentPresenterFactory = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenterFactory.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            contentPresenterFactory.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);

            borderFactory.AppendChild(contentPresenterFactory);

            buttonTemplate.VisualTree = borderFactory;

            Trigger mouseOverTrigger = new Trigger { Property = Button.IsMouseOverProperty, Value = true };
            mouseOverTrigger.Setters.Add(new Setter(Button.BackgroundProperty, UIManager.ColorThemeManager.Current.BGh1));
            mouseOverTrigger.Setters.Add(new Setter(Button.BorderBrushProperty, UIManager.ColorThemeManager.Current.BCh2));
            mouseOverTrigger.Setters.Add(new Setter(Button.FontWeightProperty, FontWeights.Bold));

            buttonTemplate.Triggers.Add(mouseOverTrigger);

            style.Setters.Add(new Setter(Button.TemplateProperty, buttonTemplate));

            style.Seal();

            return style;
        }
        private class Node : IFrameworkElement {
            private ChangeHistory.Node _sourceNode;
            private StackPanel _stackPanel;
            private StackPanel _children;
            private List<Node> _nodes = new();
            private Node? _previous;
            private Button _button;

            public FrameworkElement FrameworkElement => _stackPanel;
            public List<Node> Children => _nodes;

            public Node(Node? previous, ChangeHistory.Node source, ChangeHistory history) {
                _sourceNode = source;
                _previous = previous;
                _children = new StackPanel { 
                    Margin=new Thickness(10, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left};

                _button = new Button { Content = source.Value.GetType().Name };
                _button.Click += (s, e) => history.Jump(source);
                UIManager.ColorThemeManager.SetStyle(_button, NodeStyle);

                _stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
                _stackPanel.Children.Add(_button);
                _stackPanel.Children.Add(_children);
            }

            public void SetHighlight(bool activate) {
                if (activate) {
                    UIManager.ColorThemeManager.BG2_Changed -= SetBG;
                    UIManager.ColorThemeManager.AC1_Changed += SetBG;
                }
                else {
                    UIManager.ColorThemeManager.AC1_Changed -= SetBG;
                    UIManager.ColorThemeManager.BG2_Changed += SetBG;
                }
            }
            public void SetBG(Brush b) {
                _button.Background = b;
            }

            public Node? Next(uint index) => (index < _nodes.Count) ? _nodes[(int)index] : null;
            public Node? Previous => _previous;
            public ChangeHistory.Node Source => _sourceNode;

            public Node? First(Func<Node, bool> predicate) {
                foreach (var node in _nodes) {
                    var result = node.First(predicate);
                    if (result is not null) return result;
                }
                return null;
            }

            public void Cut() {
                _nodes.Clear();
                _children.Children.Clear();
            }

            public Node? CutNode(uint index) {
                if (index < _nodes.Count) {
                    Node node = _nodes[(int)index];
                    _children.Children.RemoveAt((int)index);
                    _nodes.RemoveAt((int)index);
                    return node;
                }
                return null;
            }

            public void Add(Node source) {
                _children.Children.Add(source.FrameworkElement);
                _nodes.Add(source);
            }
        }
    }

    public class PixelSorting : Body {

        public PixelSorting(WorkstationImage image) : base(image) {
            TextBlock textBlock = new();
            textBlock.Text = "PixelSorting";
            Helper.SetChildInGrid(mainGrid, textBlock, 0, 0);
        }

    }


    public class Test : Body {
        public Test(WorkstationImage image) : base(image) { }


        public void Run() {
            ///Debugger.Console.ClearAll();
            DebugLibrary.Console.Log("Begin test func");

            Button normal = new Button { Content = "Execute normal" };
            ///normal.Click += (s, e) => DebugLibrary.Benchmark.Measure.Execute(Compute);

            Button gpu = new Button { Content = "Execute on GPU" };
            ///gpu.Click += (s, e) => DebugLibrary.Benchmark.Measure.Execute(PerformVectorAddition());

            stackPanel.Children.Add(normal);
            stackPanel.Children.Add(gpu);

            /*
            using StreamReader sr = new StreamReader(UIManager.SettingsPath_Txt);
            using XmlReader xmlReader = XmlReader.Create(sr);
            Viewbox viewbox = (Viewbox)XamlReader.Load(xmlReader);
            viewbox.RenderTransform = new RotateTransform(90);
            viewbox.Width = 100;
            viewbox.Height = 100;
            Canvas canvas = (Canvas)viewbox.Child; 
            canvas.Width = 2000;
            canvas.Height = 2000;

            foreach(System.Windows.Shapes.Path path1 in canvas.Children) {
                path1.Fill = Brushes.White;
            }
            */


            Viewbox viewbox = Application.Current.FindResource("SettingsIcon3") as Viewbox;
            viewbox.Width = 30;
            viewbox.Height = 30;
            WindowManager.MainWindow.WindowHandle.ApplicationButtons.SettingsButtonContent = viewbox;


            //stackPanel.Children.Add(viewbox);
        }
    }


    public class Export : Body {

        public Export(WorkstationImage image) : base(image) {

        }
    }



    public class Import : Body {

        public Import(WorkstationImage image) : base(image) {

        }
    }


}
