using Retros.ProgramWindow.DisplaySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
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
        public FilterDisplay FilterDisplay;

        private List<ChangeAccess> filters;

        public ImageFilter(WorkstationImage image) : base(image) {
            FilterDisplay = new(image);

            ChangeAccess grayscale = ChangeAccess.Instanciate<GrayScale>(FilterDisplay, image);
            ChangeAccess redChannel = ChangeAccess.Instanciate<OnlyRedChannel>(FilterDisplay, image);
            ChangeAccess greenChannel = ChangeAccess.Instanciate<OnlyGreenChannel>(FilterDisplay, image);
            ChangeAccess blueChannel = ChangeAccess.Instanciate<OnlyBlueChannel>(FilterDisplay, image);
            ChangeAccess testBlue = ChangeAccess.Instanciate<TestBlue>(FilterDisplay, image);

            filters = new List<ChangeAccess> { grayscale, redChannel, greenChannel, blueChannel, testBlue };

            stackPanel.Children.Add(resetButton);
            stackPanel.Children.Add(grayscale.FrameworkElement);
            stackPanel.Children.Add(redChannel.FrameworkElement);
            stackPanel.Children.Add(greenChannel.FrameworkElement);
            stackPanel.Children.Add(blueChannel.FrameworkElement);
            stackPanel.Children.Add(testBlue.FrameworkElement);
            stackPanel.Children.Add(FilterDisplay.FrameworkElement);

            UIManager.ColorThemeManager.Set_AC1(b => resetButton.Background = b);
            resetButton.Click += ResetButton_Click;
            resetButton.Content = "Back to original";
        }


        public void AdjustSlisers(IPositiveChange[] values) {
            foreach (ChangeAccess changeAccess in filters) {
                changeAccess.AdjustSlider(
                    (IFilterChange?)values.FirstOrDefault(item => item.GetType() == changeAccess.FilterInstance.GetType()));
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e) {
            image.GetChangeManager.Clear();
            image.GetHistoryManager.Clear();
            image.ResetCurrent();
        }

        // can be static, the client will never be able to activate two different instances of this tab at the same time
        private static RemoveChange? s_removeChangeChache;

        private class ChangeAccess {
            private Type _filterType;
            private IFilterChange _filterInstance;
            private Slider _slider;

            public IFilterChange FilterInstance => _filterInstance;
            public FrameworkElement FrameworkElement => _slider.FrameworkElement;


            private ChangeAccess() { }
            public static ChangeAccess Instanciate<T>(FilterDisplay filterDisplay, WorkstationImage image) where T : IFilterChange, new() {
                ChangeAccess instance = new();
                instance._filterInstance = new T();
                instance._filterType = typeof(T);
                instance._slider = new Slider(instance._filterType.Name);
                instance._slider.SliderElement.ValueChanged += (s, e) => instance.SetToManager(filterDisplay, image);
                instance._slider.SliderElement.PreviewMouseUp += (s, e) => instance.SetChangeHistory(image);
                return instance;
            }


            public void AdjustSlider(IFilterChange? filterChange) {
                if (filterChange is not null) {
                    _slider.SliderElement.Value = filterChange.FilterIntensity;
                }
                else {
                    _slider.SliderElement.Value = 0;
                }
            }

            private void SetChangeHistory(WorkstationImage image) {
                if (_slider.SliderElement.Value != 0) {
                    image.GetHistoryManager.AddAndStep(image.GetChangeManager
                        .GetChange(_filterType)!.Clone());
                }
                else {
                    if (s_removeChangeChache is null) return;

                    image.GetChangeManager.RemoveChange(s_removeChangeChache);
                    image.GetHistoryManager.AddAndStep(s_removeChangeChache);

                    s_removeChangeChache = null;
                }
            }

            private void SetToManager(FilterDisplay filterDisplay, WorkstationImage image) {
                if (_slider.SliderElement.Value == 0) {
                    s_removeChangeChache = new RemoveChange(_filterInstance);
                    filterDisplay.RemoveItem(_filterInstance);
                }
                else {
                    bool success = image.GetChangeManager.AddChange(_filterInstance);
                    
                    if (success) {
                        filterDisplay.AddItem(_filterInstance);
                    }
                    else {
                        image.GetChangeManager.SetFilterIntensity(_filterType, _slider.SliderElement.Value);
                        if (!filterDisplay.Contains(_filterType))
                            filterDisplay.AddItem(_filterInstance);
                    }
                    
                }
            }

        }
    }



    public class ImageHistory : Body {
        private ChangeHistory _history;
        private static Canvas _canvas = new();
        private Node _root;
        private Node _currentNode;
        private HashSet<Node> _allNodes = new();

        public ChangeHistory History => _history;

        public ImageHistory(WorkstationImage image) : base(image) {
            _history = image.GetHistoryManager;

            _root = new Node(null, _history.RootChange, this);
            _currentNode = _root;
            _currentNode.SetHighlight(true);
            _allNodes.Add(_root);

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

                _currentNode = _allNodes.First(node => node.Source == newCurrentNode);

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
                var newNode = new Node(_currentNode, newChange, this);
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

        public void UpdateLines() {
            foreach (var node in _allNodes) {
                CompositionTarget.Rendering += node.UpdateLines;
            }
        }
        public static Style NodeStyle_Highlighed() {
            Style style = new Style(typeof(Button));

            style.Setters.Add(new Setter(Button.PaddingProperty, new Thickness(10, 0, 10, 0)));
            style.Setters.Add(new Setter(Button.BackgroundProperty, UIManager.ColorThemeManager.Current.BG4));
            style.Setters.Add(new Setter(Button.ForegroundProperty, UIManager.ColorThemeManager.Current.FC1));
            style.Setters.Add(new Setter(Button.BorderThicknessProperty, new Thickness(0)));
            style.Setters.Add(new Setter(Button.HorizontalAlignmentProperty, HorizontalAlignment.Left));
            style.Setters.Add(new Setter(Button.VerticalAlignmentProperty, VerticalAlignment.Top));
            style.Setters.Add(new Setter(Button.FontWeightProperty, FontWeights.ExtraBold));
            style.Setters.Add(new Setter(Button.HeightProperty, 20.0));
            style.Setters.Add(new Setter(Button.FontSizeProperty, 11.0));
            style.Setters.Add(new Setter(Button.ContentProperty, new TextBlock { Margin = new Thickness(15, 0, 10, 0) }));


            ControlTemplate buttonTemplate = new ControlTemplate(typeof(Button));
            FrameworkElementFactory borderFactory = new FrameworkElementFactory(typeof(Border));
            borderFactory.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Button.BackgroundProperty));
            borderFactory.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Button.BorderBrushProperty));
            borderFactory.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Button.BorderThicknessProperty));

            FrameworkElementFactory contentPresenterFactory = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenterFactory.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            contentPresenterFactory.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Top);

            borderFactory.AppendChild(contentPresenterFactory);

            buttonTemplate.VisualTree = borderFactory;

            Trigger mouseOverTrigger = new Trigger { Property = Button.IsMouseOverProperty, Value = true };
            //mouseOverTrigger.Setters.Add(new Setter(Button.BackgroundProperty, UIManager.ColorThemeManager.Current.BG4));
            //mouseOverTrigger.Setters.Add(new Setter(Button.BorderBrushProperty, UIManager.ColorThemeManager.Current.BCh2));

            buttonTemplate.Triggers.Add(mouseOverTrigger);

            style.Setters.Add(new Setter(Button.TemplateProperty, buttonTemplate));

            style.Seal();

            return style;
        }
        public static Style NodeStyle() {
            Style style = new Style(typeof(Button));

            style.Setters.Add(new Setter(Button.PaddingProperty, new Thickness(10, 0, 10, 0)));
            style.Setters.Add(new Setter(Button.BackgroundProperty, UIManager.ColorThemeManager.Current.BG4));
            style.Setters.Add(new Setter(Button.ForegroundProperty, UIManager.ColorThemeManager.Current.FC1));
            style.Setters.Add(new Setter(Button.BorderThicknessProperty, new Thickness(0)));
            style.Setters.Add(new Setter(Button.HorizontalAlignmentProperty, HorizontalAlignment.Left));
            style.Setters.Add(new Setter(Button.VerticalAlignmentProperty, VerticalAlignment.Top));
            style.Setters.Add(new Setter(Button.HeightProperty, 20.0));
            style.Setters.Add(new Setter(Button.FontSizeProperty, 11.0));
            style.Setters.Add(new Setter(Button.ContentProperty, new TextBlock { Margin = new Thickness(15, 0, 10, 0) }));


            ControlTemplate buttonTemplate = new ControlTemplate(typeof(Button));
            FrameworkElementFactory borderFactory = new FrameworkElementFactory(typeof(Border));
            borderFactory.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Button.BackgroundProperty));
            borderFactory.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Button.BorderBrushProperty));
            borderFactory.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Button.BorderThicknessProperty));

            FrameworkElementFactory contentPresenterFactory = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenterFactory.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            contentPresenterFactory.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Top);

            borderFactory.AppendChild(contentPresenterFactory);

            buttonTemplate.VisualTree = borderFactory;

            Trigger mouseOverTrigger = new Trigger { Property = Button.IsMouseOverProperty, Value = true };
            //mouseOverTrigger.Setters.Add(new Setter(Button.BackgroundProperty, UIManager.ColorThemeManager.Current.BG4));
            mouseOverTrigger.Setters.Add(new Setter(Button.BorderBrushProperty, UIManager.ColorThemeManager.Current.BCh2));
            mouseOverTrigger.Setters.Add(new Setter(Button.BorderThicknessProperty, new Thickness(0, 0, 0, 0.8)));

            buttonTemplate.Triggers.Add(mouseOverTrigger);

            style.Setters.Add(new Setter(Button.TemplateProperty, buttonTemplate));

            style.Seal();

            return style;
        }
        private class Node : IFrameworkElement {
            private ChangeHistory.Node _sourceNode;
            private StackPanel _mainStackPanel;
            private StackPanel _nameAndArrowsStackPanel;
            private Line _lineVertical;
            private Line _lineHorizontal;
            private StackPanel _children;
            private List<Node> _nodes = new();
            private Node? _previous;
            private Button _button;
            private StackPanel _buttonStackPanel;
            private ImageHistory _history;

            public Button ButtonElement => _button;
            public FrameworkElement FrameworkElement => _mainStackPanel;
            public List<Node> Children => _nodes;

            public Node(Node? previous, ChangeHistory.Node source, ImageHistory history) {
                _history = history;
                _sourceNode = source;
                _previous = previous;
                _children = new StackPanel { 
                    Margin = new Thickness(0, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left};

                _buttonStackPanel = new StackPanel { Orientation  = Orientation.Horizontal };

                string name = source.Value.GetType().Name;
                name += source.Value.GetType() == typeof(RemoveChange)
                    ? ": " + ((RemoveChange)source.Value).ValueType.Name
                    : "";
                _button = new Button { Content = name };
                _button.Click += (s, e) => history.History.Jump(source);
                
                UIManager.ColorThemeManager.SetStyle(_button, NodeStyle);
                _buttonStackPanel.Children.Add(_button);

                _lineVertical = new Line {
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment= HorizontalAlignment.Right,
                    X1 = 0, Y1 = 0, X2 = 0,
                };
                UIManager.ColorThemeManager.Set_BC2(b => _lineVertical.Stroke = b);

                _nameAndArrowsStackPanel = new();
                _nameAndArrowsStackPanel.Children.Add(_buttonStackPanel);
                _nameAndArrowsStackPanel.Children.Add(_lineVertical);

                _mainStackPanel = new StackPanel { Orientation = Orientation.Horizontal };
                _mainStackPanel.Children.Add(_nameAndArrowsStackPanel);
                _mainStackPanel.Children.Add(_children);

                _lineHorizontal = new Line {
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Center,
                    X1 = 0, Y1 = 0, X2 = 20, Y2 = 0,
                };
                _buttonStackPanel.Children.Insert(0, _lineHorizontal);
                if (previous is not null) {
                    UIManager.ColorThemeManager.Set_BC2(b => _lineHorizontal.Stroke = b);
                }
                else {
                    UIManager.ColorThemeManager.Set_BC3(b => _lineHorizontal.Stroke = b);
                }
            }

            public void UpdateLines(object? sender, EventArgs e) {
                if (_nodes.Count < 2) return;

                double newHeight = 0;
                for (int i = 1; i < _nodes.Count; i++) {
                    var pos = _PositionOf(_nodes[i - 1].ButtonElement);
                    var pos1 = _PositionOf(_nodes[i].ButtonElement);
                    double d = Math.Abs(pos1.Y - pos.Y);
                    newHeight += d;
                }
                newHeight -= _nodes[_nodes.Count - 1].ButtonElement.ActualHeight / 2;
                _lineVertical.Y2 = newHeight;

                CompositionTarget.Rendering -= UpdateLines;
            }

            private Point _PositionOf(FrameworkElement element) => element.TranslatePoint(new Point(), _canvas);

            public void SetHighlight(bool activate) {
                if (activate) {
                    UIManager.ColorThemeManager.SetStyle(_button, NodeStyle_Highlighed);
                    UIManager.ColorThemeManager.RemoveStyle(_button, NodeStyle);
                }
                else {
                    UIManager.ColorThemeManager.RemoveStyle(_button, NodeStyle_Highlighed);
                    UIManager.ColorThemeManager.SetStyle(_button, NodeStyle);
                }
            }

            public Node? Next(uint index) => (index < _nodes.Count) ? _nodes[(int)index] : null;
            public Node? Previous => _previous;
            public ChangeHistory.Node Source => _sourceNode;

            //Todo
            //--
            public Node? First(Func<Node, bool> predicate) {
                foreach (var node in _nodes) {
                    var result = node.First(predicate);
                    if (result is not null) return result;
                }
                return null;
            }
            //--

            public void Cut() {
                _nodes.Clear();
                _children.Children.Clear();

                _history.UpdateLines();
            }

            public Node? CutNode(uint index) {
                if (index < _nodes.Count) {
                    Node node = _nodes[(int)index];
                    _children.Children.RemoveAt((int)index);
                    _nodes.RemoveAt((int)index);
                    return node;
                }

                _history.UpdateLines();

                return null;
            }

            public void Add(Node source) {
                _children.Children.Add(source.FrameworkElement);
                _nodes.Add(source);

                _history.UpdateLines();
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
