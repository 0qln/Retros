using Retros.ProgramWindow.DisplaySystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup.Localizer;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;
using Utillities.Wpf;

namespace Retros.ProgramWindow.Interactive
{
    public class FilterDisplay : IFrameworkElement
    {
        private HashSet<Type> _itemTypes = new();
        private List<Item> _items = new();
        private readonly WorkstationImage _image;

        public FrameworkElement FrameworkElement => StackPanel;
        public StackPanel StackPanel = new();


        public FilterDisplay(WorkstationImage image) {
            this._image = image;
            StackPanel.VerticalAlignment = VerticalAlignment.Top;
        }


        public bool Contains(Type filter) => _itemTypes.Contains(filter);
        
        public void AddItem(IFilterChange newItem) {
            Item item = new(newItem.GetType().Name, this);
            StackPanel.Children.Add(item.FrameworkElement);
            _items.Add(item);
            _itemTypes.Add(newItem.GetType());

            InvokeItemListChanged();
        }

        public void RemoveItem(IFilterChange item) {
            if (Contains(item.GetType())) return;

            Item removeItem = _items.Find(item => item.Name.Text == item.GetType().Name)!;
            _items.Remove(removeItem);
            StackPanel.Children.Remove(removeItem.FrameworkElement);
            _itemTypes.Add(item.GetType());

            InvokeItemListChanged();
        }


        private Point GetPos(FrameworkElement item) {
            var p = item.TransformToAncestor(WindowManager.MainWindow).Transform(new Point(0, 0));
            return new Point(p.X + item.ActualWidth / 2, p.Y + item.ActualHeight / 2);
        }


        private void InsertItem(Item item, Point newPosition) {
            if (_items.Count == 1) {
                if (item.FrameworkElement.Parent is StackPanel) (item.FrameworkElement.Parent as StackPanel)!.Children.Remove(item.FrameworkElement);
                else (item.FrameworkElement.Parent as Canvas)!.Children.Remove(item.FrameworkElement);
                StackPanel.Children.Add(item.FrameworkElement);
                return;
            }

            int index = StackPanel.Children.Count-1;
            for (int i = 0; i < StackPanel.Children.Count; i++) {
                // if d<0 -> the cursor is above this item
                // if d>0 -> the cursor is below this item
                var pos = GetPos((StackPanel.Children[i] as FrameworkElement)!);
                double d = pos.Y - newPosition.Y;
                
                /*
                Ellipse point  = new Ellipse { Width = 4, Height = 4, Fill = Brushes.Red };
                WindowManager.MainWindow.MainCanvas.Children.Add(point);
                Canvas.SetTop(point, newPosition.Y);
                Canvas.SetLeft(point, newPosition.X);
                */
                if (d >= 0) {
                    index = i; 
                    break;
                }
            }

            _items.Remove(item);
            if (_items.Count == 0) {
                _items.Add(item);
            }
            else {
                _items.Insert(index, item);
            }

            if (item.FrameworkElement.Parent is StackPanel) (item.FrameworkElement.Parent as StackPanel)!.Children.Remove(item.FrameworkElement);
            else (item.FrameworkElement.Parent as Canvas)!.Children.Remove(item.FrameworkElement);
            StackPanel.Children.Insert(index, item.FrameworkElement);
            
            InvokeItemListChanged();
        }

        private void IncreaseHierachy(Item item) {
            if (_items.Count == 1) return;

            int index = _items.IndexOf(item);
            if (index <= 0) return;

            _items.RemoveAt(index);
            _items.Insert(index-1, item);

            StackPanel.Children.RemoveAt(index);
            StackPanel.Children.Insert(index - 1, item.FrameworkElement);

            InvokeItemListChanged();
        }


        private void InvokeItemListChanged() {
            List<string> list = new();
            _items.ForEach(item => list.Add(item.Name.Text));
            _image.GetChangeManager.Order(list);
        }

        private class Item : IFrameworkElement {
            private FilterDisplay _parent;
            private bool _isPinned = false;
            private Button _pinButton = new();

            public FrameworkElement FrameworkElement => MainGrid;
            public Grid MainGrid { get; } = new();
            public Button IncreaseHierachyButton { get; } = new();
            public TextBlock Name { get; } = new();


            public Item(string name, FilterDisplay parent) {
                _parent = parent;

                UIManager.ColorThemeManager.Set_BG6(b => Name.Background = b);
                UIManager.ColorThemeManager.Set_FC1(b => Name.Foreground = b);
                Name.Text = name;
                Name.FontSize = 14;
                Name.PreviewMouseLeftButtonDown += (s, e) => StartDrag();
                Name.PreviewMouseLeftButtonUp += (s, e) => StopDrag();

                UIManager.ColorThemeManager.SetStyle(IncreaseHierachyButton, WindowHandle.ClientButtonStyle);
                IncreaseHierachyButton.MinWidth = 20;
                IncreaseHierachyButton.Click += (s, e) => { parent.IncreaseHierachy(this); };
                IncreaseHierachyButton.Content = "⮭";
                IncreaseHierachyButton.MaxHeight = 20;
                IncreaseHierachyButton.Style = WindowHandle.ClientButtonStyle();
                IncreaseHierachyButton.FontSize = 15;
                IncreaseHierachyButton.Margin = new Thickness(0);

                UIManager.ColorThemeManager.SetStyle(_pinButton, PinButtonStyle);
                _pinButton.Content = "📌";
                _pinButton.MaxWidth = 0;
                _pinButton.MinWidth = 0;
                MainGrid.MouseEnter += (s, e) => {
                    if (_isPinned) return;

                    _pinButton.MaxWidth = 20;
                    _pinButton.MinWidth = 20;
                };
                _pinButton.Click += (s, e) => {
                    if (!_isPinned) {
                        _isPinned = true;
                    }
                    else {
                        _isPinned = false;
                    }
                };
                MainGrid.MouseLeave += (s, e) => {
                    if (_isPinned) return;

                    if (!_isPinned) {
                        _pinButton.MaxWidth = 0;
                        _pinButton.MinWidth = 0;
                    }
                };

                Helper.AddColumn(MainGrid, 1, GridUnitType.Auto);
                Helper.AddColumn(MainGrid, 1, GridUnitType.Auto);
                Helper.AddColumn(MainGrid, 1, GridUnitType.Star);
                Helper.SetChildInGrid(MainGrid, _pinButton, 0, 0);
                Helper.SetChildInGrid(MainGrid, IncreaseHierachyButton, 0, 1);
                Helper.SetChildInGrid(MainGrid, Name, 0, 2);
                MainGrid.MaxHeight = 25;
                UIManager.ColorThemeManager.Set_BG3(b => MainGrid.Background = b);
            }

            private Style PinButtonStyle() {
                Style clientButtonsStyle = new Style(typeof(Button));

                clientButtonsStyle.Setters.Add(new Setter(Button.BackgroundProperty, UIManager.ColorThemeManager.Current.BG6));
                clientButtonsStyle.Setters.Add(new Setter(Button.ForegroundProperty, UIManager.ColorThemeManager.Current.FC1));
                clientButtonsStyle.Setters.Add(new Setter(Button.BorderBrushProperty, UIManager.ColorThemeManager.Current.BC3));
                clientButtonsStyle.Setters.Add(new Setter(Button.BorderThicknessProperty, new Thickness(1)));
                clientButtonsStyle.Setters.Add(new Setter(Button.HorizontalAlignmentProperty, HorizontalAlignment.Left));
                clientButtonsStyle.Setters.Add(new Setter(Button.VerticalAlignmentProperty, VerticalAlignment.Center));

                ControlTemplate userButtonTemplate = new ControlTemplate(typeof(Button));
                FrameworkElementFactory borderFactory = new FrameworkElementFactory(typeof(Border));
                borderFactory.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Button.BackgroundProperty));
                borderFactory.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Button.BorderBrushProperty));
                borderFactory.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Button.BorderThicknessProperty));

                FrameworkElementFactory contentPresenterFactory = new FrameworkElementFactory(typeof(ContentPresenter));
                contentPresenterFactory.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                contentPresenterFactory.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);

                borderFactory.AppendChild(contentPresenterFactory);

                userButtonTemplate.VisualTree = borderFactory;

                Trigger mouseOverTrigger = new Trigger { Property = Button.IsMouseOverProperty, Value = true };
                mouseOverTrigger.Setters.Add(new Setter(Button.BackgroundProperty, UIManager.ColorThemeManager.Current.BGh1));
                mouseOverTrigger.Setters.Add(new Setter(Button.BorderBrushProperty, Brushes.Gray));

                userButtonTemplate.Triggers.Add(mouseOverTrigger);

                clientButtonsStyle.Setters.Add(new Setter(Button.TemplateProperty, userButtonTemplate));

                clientButtonsStyle.Seal();

                return clientButtonsStyle;                
            }


            public override string ToString() {
                return Name.Text;
            }


            public Point GetCenteredPosition() {
                var a = MainGrid.TransformToAncestor(WindowManager.MainWindow!.MainCanvas).Transform(new Point(0, 0));
                var b = MainGrid.ActualWidth;
                var c = MainGrid.ActualHeight;

               return new Point(a.X + b/2, a.Y + c/2);
            }

            private bool IsValid() {
                StackPanel s = _parent.StackPanel;
                Point p = Mouse.GetPosition(WindowManager.MainWindow);
                Point _1 = s.TransformToAncestor(WindowManager.MainWindow).Transform(new Point(0, 0));
                Point _2 = new Point(_1.X + s.ActualWidth, _1.Y + s.ActualHeight);

                if (p.X > _1.X && p.X < _2.X&&
                    p.Y > _1.Y && p.Y < _2.Y)
                    return true;

                return false;
            }

            private Point startPosition;
            private void StartDrag() {
                startPosition = Mouse.GetPosition(WindowManager.MainWindow);
                CompositionTarget.Rendering += Drag;
                _parent.StackPanel.Children.Remove(FrameworkElement);
                WindowManager.MainWindow!.MainCanvas.Children.Add(FrameworkElement);
            }
            private void StopDrag() {
                CompositionTarget.Rendering -= Drag;

                bool isValid = _parent.StackPanel.IsMouseOver;
                foreach ( FrameworkElement child in _parent.StackPanel.Children ) {
                    if (child.IsMouseOver) {
                        isValid = true;
                        break;
                    }
                }

                if (IsValid()) {
                    //DebugLibrary.Console.Log();
                    _parent.InsertItem(this, Mouse.GetPosition(WindowManager.MainWindow));
                }
                else {
                    _parent.InsertItem(this, startPosition);
                }
            }
            private void Drag(object? sender, EventArgs e) {
                Point mousePos = Mouse.GetPosition(WindowManager.MainWindow);
                Canvas.SetTop(FrameworkElement, mousePos.Y);
                Canvas.SetLeft(FrameworkElement, mousePos.X);

                bool isValid = _parent.StackPanel.IsMouseOver;
                foreach (FrameworkElement child in _parent.StackPanel.Children) {
                    if (child.IsMouseOver) {
                        isValid = true;
                        break;
                    }
                }

                if (!WindowManager.MainWindow!.MainCanvas.IsMouseOver
                    || Mouse.LeftButton != MouseButtonState.Pressed) 
                    StopDrag();
            }


        }
    }
}
