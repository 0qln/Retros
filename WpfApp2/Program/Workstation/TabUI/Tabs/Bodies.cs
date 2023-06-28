using CustomControlLibrary;
using Retros.CustomUI;
using Retros.Program.Workstation.Image;
using Retros.Program.DisplaySystem;
using Retros.Settings;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Utillities.Wpf;
using Retros.Program.Workstation.Changes;
using Slider = Retros.CustomUI.Slider;
using System.Windows.Threading;

// Bodies Manage the Tab body UIElements and functionality
namespace Retros.Program.Workstation.TabUI.Tabs
{
    public abstract class Body {

        protected readonly WorkstationImage _image;
        protected readonly StackPanel _stackPanel = new();
        protected readonly Grid _mainGrid = new();
        protected readonly Border _border = new();

        public FrameworkElement FrameworkElement => _border;


        public Body(WorkstationImage image) {
            _image = image;
            UIManager.ColorThemeManager.Set_BC1(newBrush => _border.BorderBrush = newBrush);
            _border.Child = _mainGrid;
            _border.BorderThickness = new Thickness(1);
            Helper.SetChildInGrid(_mainGrid, _stackPanel, 0, 0);
        }

        public void Hide() => _mainGrid.Visibility = Visibility.Collapsed;
        public void Show() => _mainGrid.Visibility = Visibility.Visible;


        public abstract void Reset();
    }



    public class PixelSortingBody : Body
    {
        public readonly Button GenerateButton = new();
        
        public readonly StackPanel OrientationPanel = new();
        public readonly TextBlock OrientationText = new();
        public readonly SelectionBox OrientationSelection = new();

        public readonly StackPanel DirectionPanel = new();
        public readonly TextBlock DirectionText = new();
        public readonly SelectionBox DirectionSelection = new();

        public readonly StackPanel SortByPanel = new();
        public readonly TextBlock SortByText = new();
        public readonly SelectionBox SortBySelection = new();

        private readonly PixelSorter _sorter = new();

        public PixelSortingBody(WorkstationImage image) : base(image) { 
            OrientationText.Text = "Filter Orientation";
            UIManager.ColorThemeManager.Set_FC1(b => OrientationText.Foreground = b);

            OrientationSelection.AddOptions<Orientation>();
            OrientationSelection.SelectOption(0);
            OrientationSelection.SelectedChanged += OrientationSelection_SelectedChanged;
            UIManager.ColorThemeManager.SetStyle(OrientationSelection, TabDetail.Body.SelectionBoxStyle);

            OrientationPanel.Children.Add(OrientationText);
            OrientationPanel.Children.Add(OrientationSelection);
            OrientationPanel.Orientation = Orientation.Horizontal;


            DirectionText.Text = "Filter Direction";
            UIManager.ColorThemeManager.Set_FC1(b => DirectionText.Foreground = b);
            
            DirectionSelection.AddOptions<SortDirection>();
            DirectionSelection.SelectOption(0);
            DirectionSelection.SelectedChanged += DirectionSelection_SelectedChanged;
            UIManager.ColorThemeManager.SetStyle(DirectionSelection, TabDetail.Body.SelectionBoxStyle);
            
            DirectionPanel.Children.Add(DirectionText);
            DirectionPanel.Children.Add(DirectionSelection);
            DirectionPanel.Orientation = Orientation.Horizontal;


            SortByText.Text = "Sort By";
            UIManager.ColorThemeManager.Set_FC1(b => SortByText.Foreground = b);
            
            SortBySelection.AddOptions<SortBy>();
            SortBySelection.SelectOption(0);
            SortBySelection.SelectedChanged += SortBySelection_SelectedChanged; ;
            UIManager.ColorThemeManager.SetStyle(SortBySelection, TabDetail.Body.SelectionBoxStyle);

            SortByPanel.Children.Add(SortByText);
            SortByPanel.Children.Add(SortBySelection);
            SortByPanel.Orientation = Orientation.Horizontal;            


            GenerateButton.Content = "Generate";
            GenerateButton.Click += GenerateButton_Click;

            _stackPanel.Children.Add(OrientationPanel);
            _stackPanel.Children.Add(DirectionPanel);
            _stackPanel.Children.Add(SortByPanel);
            _stackPanel.Children.Add(GenerateButton);
        
        }

        private void SortBySelection_SelectedChanged(string obj)
        {
            _sorter.SortBy = (SortBy)Enum.Parse(typeof(SortBy), obj);
        }

        private void OrientationSelection_SelectedChanged(string obj)
        {
            _sorter.Orientation = (Orientation)Enum.Parse(typeof(Orientation), obj);
        }

        private void DirectionSelection_SelectedChanged(string obj)
        {
            _sorter.Direction = (SortDirection)Enum.Parse(typeof(SortDirection), obj);
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_image.GetFilterManager.AddFilter(_sorter))
            {
                _image.GetFilterManager.RemoveFilter(_sorter);
                _image.GetFilterManager.AddFilter(_sorter);
            }
        }

        public override void Reset()
        {
            throw new NotImplementedException();
        }
    }


    public class ImageFilterBody : Body {

        public Button resetButton = new();
        public FilterDisplay FilterDisplay;

        private List<ChangeAccess> filters;

        public ImageFilterBody(WorkstationImage image) : base(image) {
            FilterDisplay = new(image);

            ChangeAccess grayscale = ChangeAccess.Instanciate<GrayScale>(FilterDisplay, image);
            ChangeAccess redChannel = ChangeAccess.Instanciate<OnlyRedChannel>(FilterDisplay, image);
            ChangeAccess greenChannel = ChangeAccess.Instanciate<OnlyGreenChannel>(FilterDisplay, image);
            ChangeAccess blueChannel = ChangeAccess.Instanciate<OnlyBlueChannel>(FilterDisplay, image);
            ChangeAccess testBlue = ChangeAccess.Instanciate<TestBlue>(FilterDisplay, image);

            filters = new List<ChangeAccess> { grayscale, redChannel, greenChannel, blueChannel, testBlue, };

            _stackPanel.Children.Add(resetButton);
            _stackPanel.Children.Add(grayscale.FrameworkElement);
            _stackPanel.Children.Add(redChannel.FrameworkElement);
            _stackPanel.Children.Add(greenChannel.FrameworkElement);
            _stackPanel.Children.Add(blueChannel.FrameworkElement);
            _stackPanel.Children.Add(testBlue.FrameworkElement);
            _stackPanel.Children.Add(FilterDisplay.FrameworkElement);

            UIManager.ColorThemeManager.Set_AC1(b => resetButton.Background = b);
            resetButton.Click += (_,_) => _image.Reset();
            resetButton.Content = "Back to original";


        }


        public void AdjustSlisers(IFilter[] values) {
            foreach (ChangeAccess changeAccess in filters) {
                changeAccess.AdjustSlider(values.FirstOrDefault(item => item.GetType() == changeAccess.FilterInstance.GetType()));
            }
        }

        public override void Reset() {
            foreach (ChangeAccess changeAccess in filters) {
                changeAccess.Reset();
            }
        }

        private class ChangeAccess {
            private Type _filterType;
            private IFilter _filterInstance;
            private Slider _slider;

            public IFilter FilterInstance => _filterInstance;
            public FrameworkElement FrameworkElement => _slider.FrameworkElement;


            private ChangeAccess() { }
            public static ChangeAccess Instanciate<T>(FilterDisplay filterDisplay, WorkstationImage image) where T : IFilter, new() {
                ChangeAccess instance = new();
                instance._filterInstance = new T();
                instance._filterType = typeof(T);
                instance._slider = new Slider(instance._filterType.Name);
                instance._slider.SliderElement.ValueChanged += (s, e) => instance.SetToManager(filterDisplay, image);
                instance._slider.SliderElement.PreviewMouseUp += (s, e) => instance.SetChangeHistory(image);
                return instance;
            }


            public void AdjustSlider(IFilter? filterChange) {
                if (filterChange is not null) {
                    _slider.SliderElement.Value = filterChange.FilterIntensity;
                }
                else {
                    _slider.SliderElement.Value = 0;
                }
            }

            private void SetChangeHistory(WorkstationImage image) {
                image.GetHistoryManager.AddAndStep(ImageState.FromImage(image));
            }

            public void Reset() {
                _slider.SliderElement.Value = 0;
                _filterInstance.FilterIntensity = 0;
            }

            private void SetToManager(FilterDisplay filterDisplay, WorkstationImage image) {
                if (_slider.SliderElement.Value == 0) {
                    image.GetFilterManager.RemoveFilter(_filterInstance);
                    filterDisplay.RemoveItem(_filterInstance);
                }
                else {
                    bool success = image.GetFilterManager.AddFilter(_filterInstance);

                    if (!success) {
                        image.GetFilterManager.SetFilterIntensity(FilterInstance, _slider.SliderElement.Value);
                    }

                    if (!filterDisplay.Contains(_filterType)) {
                        filterDisplay.AddFilter(_filterInstance);
                    }                    
                }
            }

        }
    }



    public class ImageHistoryBody : Body {
        private ChangeHistory _history;
        private static Canvas _canvas = new();
        private Node _root;
        private Node _currentNode;
        private HashSet<Node> _allNodes = new();

        public ChangeHistory History => _history;


        public ImageHistoryBody(WorkstationImage image) : base(image) {
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
            _history.MoveBack += (steps) => {
                _currentNode.SetHighlight(false);

                int i = 0;
                while (i < steps && _currentNode.Previous is not null) {
                    _currentNode = _currentNode.Previous;
                    i++;
                }

                _currentNode.SetHighlight(true);
                UIManager.ColorThemeManager.UpdateColors();
            };
            _history.MoveForward += (steps) => {
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


            _stackPanel.Children.Add(_canvas);
            _canvas.Children.Add(_root.FrameworkElement);
        }


        public override void Reset() {
            _currentNode = _root;
            _root.Cut();
            _allNodes.Clear();
            _allNodes.Add(_root);

            _history.Reset();
        }

        public void UpdateLines() {
            foreach (var node in _allNodes) {
                CompositionTarget.Rendering += node.UpdateLines;
            }
        }


        public static Style NodeStyle_Highlighed() {
            Style style = new Style(typeof(Button));

            style.Setters.Add(new Setter(Control.PaddingProperty, new Thickness(10, 0, 10, 0)));
            style.Setters.Add(new Setter(Control.BackgroundProperty, UIManager.ColorThemeManager.Current.BG4));
            style.Setters.Add(new Setter(Control.ForegroundProperty, UIManager.ColorThemeManager.Current.FC1));
            style.Setters.Add(new Setter(Control.BorderThicknessProperty, new Thickness(0)));
            style.Setters.Add(new Setter(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left));
            style.Setters.Add(new Setter(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Top));
            style.Setters.Add(new Setter(Control.FontWeightProperty, FontWeights.ExtraBold));
            style.Setters.Add(new Setter(FrameworkElement.HeightProperty, 20.0));
            style.Setters.Add(new Setter(FrameworkElement.MinWidthProperty, 20.0));
            style.Setters.Add(new Setter(Control.FontSizeProperty, 11.0));
            style.Setters.Add(new Setter(ContentControl.ContentProperty, new TextBlock { Margin = new Thickness(15, 0, 10, 0) }));


            ControlTemplate buttonTemplate = new ControlTemplate(typeof(Button));
            FrameworkElementFactory borderFactory = new FrameworkElementFactory(typeof(Border));
            borderFactory.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Control.BackgroundProperty));
            borderFactory.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Control.BorderBrushProperty));
            borderFactory.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Control.BorderThicknessProperty));

            FrameworkElementFactory contentPresenterFactory = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenterFactory.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            contentPresenterFactory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Top);

            borderFactory.AppendChild(contentPresenterFactory);

            buttonTemplate.VisualTree = borderFactory;

            Trigger mouseOverTrigger = new Trigger { Property = UIElement.IsMouseOverProperty, Value = true };
            //mouseOverTrigger.Setters.Add(new Setter(Button.BackgroundProperty, UIManager.ColorThemeManager.Current.BG4));
            //mouseOverTrigger.Setters.Add(new Setter(Button.BorderBrushProperty, UIManager.ColorThemeManager.Current.BCh2));

            buttonTemplate.Triggers.Add(mouseOverTrigger);

            style.Setters.Add(new Setter(Control.TemplateProperty, buttonTemplate));

            style.Seal();

            return style;
        }
        public static Style NodeStyle() {
            Style style = new Style(typeof(Button));

            style.Setters.Add(new Setter(Control.PaddingProperty, new Thickness(10, 0, 10, 0)));
            style.Setters.Add(new Setter(Control.BackgroundProperty, UIManager.ColorThemeManager.Current.BG4));
            style.Setters.Add(new Setter(Control.ForegroundProperty, UIManager.ColorThemeManager.Current.FC1));
            style.Setters.Add(new Setter(Control.BorderThicknessProperty, new Thickness(0)));
            style.Setters.Add(new Setter(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left));
            style.Setters.Add(new Setter(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Top));
            style.Setters.Add(new Setter(FrameworkElement.HeightProperty, 20.0));
            style.Setters.Add(new Setter(FrameworkElement.MinWidthProperty , 20.0));
            style.Setters.Add(new Setter(Control.FontSizeProperty, 11.0));
            style.Setters.Add(new Setter(ContentControl.ContentProperty, new TextBlock { Margin = new Thickness(15, 0, 10, 0) }));


            ControlTemplate buttonTemplate = new ControlTemplate(typeof(Button));
            FrameworkElementFactory borderFactory = new FrameworkElementFactory(typeof(Border));
            borderFactory.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Control.BackgroundProperty));
            borderFactory.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Control.BorderBrushProperty));
            borderFactory.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Control.BorderThicknessProperty));

            FrameworkElementFactory contentPresenterFactory = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenterFactory.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            contentPresenterFactory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Top);

            borderFactory.AppendChild(contentPresenterFactory);

            buttonTemplate.VisualTree = borderFactory;

            Trigger mouseOverTrigger = new Trigger { Property = UIElement.IsMouseOverProperty, Value = true };
            mouseOverTrigger.Setters.Add(new Setter(Button.BackgroundProperty, UIManager.ColorThemeManager.Current.BG4));
            mouseOverTrigger.Setters.Add(new Setter(Control.BorderBrushProperty, UIManager.ColorThemeManager.Current.BCh2));
            mouseOverTrigger.Setters.Add(new Setter(Control.BorderThicknessProperty, new Thickness(0, 0, 0, 0.8)));
            buttonTemplate.Triggers.Add(mouseOverTrigger);

            Trigger mouseNotOverTrigger = new Trigger { Property = UIElement.IsMouseOverProperty, Value = false };
            if (SettingsManager.ImageHistory.CompactNodeLayout.Value) mouseNotOverTrigger.Setters.Add(new Setter(Button.MaxWidthProperty, SettingsManager.ImageHistory.CompactNodeMaxWidth.Value));
            buttonTemplate.Triggers.Add(mouseNotOverTrigger);

            style.Setters.Add(new Setter(Control.TemplateProperty, buttonTemplate));

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
            private ImageHistoryBody _history;

            public Button ButtonElement => _button;
            public FrameworkElement FrameworkElement => _mainStackPanel;
            public List<Node> Children => _nodes;

            public Node(Node? previous, ChangeHistory.Node source, ImageHistoryBody history) {
                _history = history;
                _sourceNode = source;
                _previous = previous;
                _children = new StackPanel {
                    Margin = new Thickness(0, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                _buttonStackPanel = new StackPanel { Orientation = Orientation.Horizontal };

                string name = source.Name;
                _button = new Button { Content = name };
                _button.Click += (s, e) => history.History.Jump(source);
                _buttonStackPanel.Children.Add(_button);

                UIManager.ColorThemeManager.SetStyle(_button, NodeStyle);
                SettingsManager.ImageHistory.CompactNodeLayout.ValueChanged += (value) => UIManager.ColorThemeManager.SetStyle(_button, NodeStyle);
                SettingsManager.ImageHistory.CompactNodeMaxWidth.ValueChanged += (value) => UIManager.ColorThemeManager.SetStyle(_button, NodeStyle);


                _lineVertical = new Line {
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    X1 = 0,
                    Y1 = 0,
                    X2 = 0,
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
                    X1 = 0,
                    Y1 = 0,
                    X2 = 20,
                    Y2 = 0,
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

            public Node? Next(uint index) => index < _nodes.Count ? _nodes[(int)index] : null;
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





    public class Test : Body {
        public Test(WorkstationImage image) : base(image) { }


        public void Run() {
            ///Debugger.Console.ClearAll();
            //DebugLibrary.Console.Log("Begin test func");

            Button normal = new Button { Content = "Execute normal" };
            ///normal.Click += (s, e) => DebugLibrary.Benchmark.Measure.Execute(Compute);

            Button gpu = new Button { Content = "Execute on GPU" };
            ///gpu.Click += (s, e) => DebugLibrary.Benchmark.Measure.Execute(PerformVectorAddition());

            _stackPanel.Children.Add(normal);
            _stackPanel.Children.Add(gpu);

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

        public override void Reset() {

        }
    }



    public class Export : Body {

        public Export(WorkstationImage image) : base(image) {

        }

        public override void Reset() {

        }
    }



    public class Import : Body {

        public Import(WorkstationImage image) : base(image) {

        }

        public override void Reset() {

        }
    }


}
