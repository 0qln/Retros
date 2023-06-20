using Retros.Program.Workstation.Changes;
using Retros.Program.Workstation.Image;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Xml.Linq;
using Utillities.Wpf;

namespace Retros.Program.Workstation.TabUI
{
    public class FilterDisplay : IFrameworkElement
    {
        private HashSet<Type> _itemTypes = new();
        private List<Item> _items = new();
        private readonly WorkstationImage _image;
        private bool _isDragging;

        public FrameworkElement FrameworkElement => StackPanel;
        public StackPanel StackPanel = new();


        public delegate void HierachyChangedHandler(IFilter[] hierachy);
        public event HierachyChangedHandler HierachyChanged;


        public FilterDisplay(WorkstationImage image)
        {
            _image = image;
            StackPanel.VerticalAlignment = VerticalAlignment.Top;

            UIManager.ColorThemeManager.BG6_Changed += SetBG;
            StackPanel.MouseEnter += (s, e) =>
            {
                if (_isDragging)
                {
                    UIManager.ColorThemeManager.BG5_Changed += SetBG;
                    SetBG(UIManager.ColorThemeManager.Current.BG5);

                    UIManager.ColorThemeManager.BG6_Changed -= SetBG;
                }
            };
            StackPanel.MouseLeave += (s, e) =>
            {
                UIManager.ColorThemeManager.BG6_Changed += SetBG;
                SetBG(UIManager.ColorThemeManager.Current.BG6);

                UIManager.ColorThemeManager.BG5_Changed -= SetBG;
            };
        }
        private void SetBG(Brush b)
        {
            StackPanel.Background = b;
        }


        public bool Contains(Type type)
        {
            return _itemTypes.Contains(type);
        }

        public void AddFilter(IFilter newItem)
        {
            Item item = new(newItem, this, 22);
            StackPanel.Children.Add(item.FrameworkElement);
            _items.Add(item);
            _itemTypes.Add(newItem.GetType());
        }

        public void RemoveItem(IFilter item)
        {
            if (!Contains(item.GetType())) return;

            Item? removeItem = _items.Find(item_ => item_.Value.GetType() == item.GetType());
            if (removeItem is null) return;

            _items.Remove(removeItem);
            StackPanel.Children.Remove(removeItem.FrameworkElement);
            _itemTypes.Add(item.GetType());

            InvokeItemListOrderChanged();
        }

        private void Print()
        {
            DebugLibrary.Console.Log("--------Items: ");
            _items.ForEach(DebugLibrary.Console.Log);
        }
        private Point GetPos(FrameworkElement item)
        {
            var p = item.TransformToAncestor(WindowManager.MainWindow).Transform(new Point(0, 0));
            return new Point(p.X + item.ActualWidth / 2, p.Y + item.ActualHeight / 2);
        }


        private void DropItem(Item item, Point newPosition)
        {
            (item.FrameworkElement.Parent as Panel)!.Children.Remove(item.FrameworkElement);
            StackPanel.Children.Add(item.FrameworkElement);

            if (_items.Count <= 2) return;


            // Get the index of the new Position
            int index = StackPanel.Children.Count - 1;
            for (int i = 0; i < StackPanel.Children.Count; i++)
            {
                // if d<0 -> the cursor is above this item
                // if d>0 -> the cursor is below this item
                var pos = GetPos((StackPanel.Children[i] as FrameworkElement)!);
                double d = pos.Y - newPosition.Y;

                if (d >= 0)
                {
                    index = i;
                    break;
                }
            }


            SwapItems(item, index);

            InvokeItemListOrderChanged();
        }

        private void IncreaseHierachy(Item item)
        {
            if (!_items.Contains(item)
                || _items.IndexOf(item) == 0
                || _items.Count <= 1)
            {
                return;
            }

            SwapItems(item, _items.IndexOf(item) - 1);

            InvokeItemListOrderChanged();
        }


        // unsafe Methods that respect the pin of the items they handle
        private void MoveItem(Item item, int index)
        {
            if (item.IsPinned) return;

            RemoveItem(item);
            InsertItem(item, index);
        }
        private void InsertItem(Item item, int index)
        {
            _items.Insert(index, item);
            StackPanel.Children.Insert(index, item.FrameworkElement);
        }
        private void RemoveItem(Item item)
        {
            RemoveItemAt(_items.IndexOf(item));
        }
        private void RemoveItemAt(int index)
        {
            ((Panel)_items[index].FrameworkElement.Parent)!.Children.RemoveAt(index);
            _items.RemoveAt(index);
        }
        private void SwapItems(Item item1, Item item2)
        {
            SwapItems(_items.IndexOf(item1), _items.IndexOf(item2));
        }
        private void SwapItems(Item item1, int indexItem2)
        {
            SwapItems(_items.IndexOf(item1), indexItem2);
        }
        private void SwapItems(int index1, int index2)
        {
            if (_items[index1].IsPinned ||
                _items[index2].IsPinned) return;

            if (index1 == index2) return;

            // Swap StackPanel
            UIElement element1 = StackPanel.Children[index1];
            UIElement element2 = StackPanel.Children[index2];

            ((Panel)_items[index1].FrameworkElement.Parent).Children.Remove(element1);
            ((Panel)_items[index2].FrameworkElement.Parent).Children.Remove(element2);

            if (index1 < index2)
            {
                StackPanel.Children.Insert(index1, element2);
                StackPanel.Children.Insert(index2, element1);
            }
            else
            {
                StackPanel.Children.Insert(index2, element1);
                StackPanel.Children.Insert(index1, element2);
            }

            // Swap List
            var temp = _items[index1];
            _items[index1] = _items[index2];
            _items[index2] = temp;
        }

        private void InvokeItemListOrderChanged()
        {
            List<string> list = new();
            _items.ForEach(item => list.Add(item.Name.Text));
            _image.GetFilterManager.Order(list);
            _image.GetFilterManager.ApplyFilters();
        }


        private class Item : IFrameworkElement
        {
            private FilterDisplay _parent;
            private bool _isPinned = false;
            private Button _pinButton = new();
            private IFilter _value;

            public FrameworkElement FrameworkElement => MainGrid;
            public Grid MainGrid { get; } = new();
            public Button IncreaseHierachyButton { get; } = new();
            public TextBlock Name { get; } = new();
            public bool IsPinned => _isPinned;
            public IFilter Value => _value;


            public Item(IFilter value, FilterDisplay parent, double height)
            {
                _parent = parent;
                _value = value;

                UIManager.ColorThemeManager.Set_BG6(b => Name.Background = b);
                UIManager.ColorThemeManager.Set_FC1(b => Name.Foreground = b);
                Name.Text = value.GetType().Name;
                Name.VerticalAlignment = VerticalAlignment.Center;
                Name.FontSize = 14;
                Name.PreviewMouseLeftButtonDown += (s, e) => StartDrag();
                Name.PreviewMouseLeftButtonUp += (s, e) => StopDrag();

                UIManager.ColorThemeManager.SetStyle(IncreaseHierachyButton, WindowHandle.ClientButtonStyle);
                IncreaseHierachyButton.MinWidth = height;
                IncreaseHierachyButton.Click += (s, e) => { parent.IncreaseHierachy(this); };
                IncreaseHierachyButton.Content = "⮭";
                IncreaseHierachyButton.MaxHeight = height;
                IncreaseHierachyButton.MinHeight = height;

                IncreaseHierachyButton.Style = WindowHandle.ClientButtonStyle();
                IncreaseHierachyButton.FontSize = 15;
                IncreaseHierachyButton.Margin = new Thickness(0);

                UIManager.ColorThemeManager.SetStyle(_pinButton, PinButtonStyle);
                _pinButton.Content = "📌";
                _pinButton.MaxWidth = height;
                _pinButton.MinWidth = height;
                _pinButton.MinHeight = height;
                MainGrid.MouseEnter += (s, e) =>
                {
                    if (_isPinned) return;
                    if (_parent._isDragging) return;

                    Activate_pinButton(height);
                };
                _pinButton.Click += (s, e) =>
                {
                    if (!_isPinned)
                    {
                        _isPinned = true;
                    }
                    else
                    {
                        _isPinned = false;
                    }
                };
                MainGrid.MouseLeave += (s, e) =>
                {
                    if (_isPinned) return;

                    Deactivate_pinButton();
                };

                Helper.AddColumn(MainGrid, 1, GridUnitType.Auto);
                Helper.AddColumn(MainGrid, 1, GridUnitType.Auto);
                Helper.AddColumn(MainGrid, 1, GridUnitType.Star);
                Helper.SetChildInGrid(MainGrid, _pinButton, 0, 0);
                Helper.SetChildInGrid(MainGrid, IncreaseHierachyButton, 0, 1);
                Helper.SetChildInGrid(MainGrid, Name, 0, 2);
                MainGrid.MaxHeight = height;
                MainGrid.MinHeight = height;
                UIManager.ColorThemeManager.Set_BG3(b => MainGrid.Background = b);
            }

            private void Activate_pinButton(double height)
            {
                _pinButton.MaxWidth = height;
                _pinButton.MinWidth = height;
            }
            private void Deactivate_pinButton()
            {
                _pinButton.MaxWidth = 0;
                _pinButton.MinWidth = 0;
            }

            private Style PinButtonStyle()
            {
                Style clientButtonsStyle = new Style(typeof(Button));

                clientButtonsStyle.Setters.Add(new Setter(Control.BackgroundProperty, UIManager.ColorThemeManager.Current.BG6));
                clientButtonsStyle.Setters.Add(new Setter(Control.ForegroundProperty, UIManager.ColorThemeManager.Current.FC1));
                clientButtonsStyle.Setters.Add(new Setter(Control.BorderBrushProperty, UIManager.ColorThemeManager.Current.BC3));
                clientButtonsStyle.Setters.Add(new Setter(Control.BorderThicknessProperty, new Thickness(1)));
                clientButtonsStyle.Setters.Add(new Setter(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left));
                clientButtonsStyle.Setters.Add(new Setter(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center));

                ControlTemplate userButtonTemplate = new ControlTemplate(typeof(Button));
                FrameworkElementFactory borderFactory = new FrameworkElementFactory(typeof(Border));
                borderFactory.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Control.BackgroundProperty));
                borderFactory.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Control.BorderBrushProperty));
                borderFactory.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Control.BorderThicknessProperty));

                FrameworkElementFactory contentPresenterFactory = new FrameworkElementFactory(typeof(ContentPresenter));
                contentPresenterFactory.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                contentPresenterFactory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);

                borderFactory.AppendChild(contentPresenterFactory);

                userButtonTemplate.VisualTree = borderFactory;

                Trigger mouseOverTrigger = new Trigger { Property = UIElement.IsMouseOverProperty, Value = true };
                mouseOverTrigger.Setters.Add(new Setter(Control.BackgroundProperty, UIManager.ColorThemeManager.Current.BGh1));
                mouseOverTrigger.Setters.Add(new Setter(Control.BorderBrushProperty, Brushes.Gray));

                userButtonTemplate.Triggers.Add(mouseOverTrigger);

                clientButtonsStyle.Setters.Add(new Setter(Control.TemplateProperty, userButtonTemplate));

                clientButtonsStyle.Seal();

                return clientButtonsStyle;
            }


            public override string ToString()
            {
                return Name.Text;
            }


            public Point GetCenteredPosition()
            {
                var a = MainGrid.TransformToAncestor(WindowManager.MainWindow!.MainCanvas).Transform(new Point(0, 0));
                var b = MainGrid.ActualWidth;
                var c = MainGrid.ActualHeight;

                return new Point(a.X + b / 2, a.Y + c / 2);
            }

            private bool IsValid()
            {
                StackPanel s = _parent.StackPanel;
                Point p = Mouse.GetPosition(WindowManager.MainWindow);
                Point _1 = s.TransformToAncestor(WindowManager.MainWindow).Transform(new Point(0, 0));
                Point _2 = new Point(_1.X + s.ActualWidth, _1.Y + s.ActualHeight);

                if (p.X > _1.X && p.X < _2.X &&
                    p.Y > _1.Y && p.Y < _2.Y)
                    return true;

                return false;
            }

            private Point startPosition;
            private void StartDrag()
            {
                if (_isPinned)
                {
                    return;
                }
                _parent._isDragging = true;
                startPosition = Mouse.GetPosition(WindowManager.MainWindow);
                CompositionTarget.Rendering += Drag;
                _parent.StackPanel.Children.Remove(FrameworkElement);
                WindowManager.MainWindow!.MainCanvas.Children.Add(FrameworkElement);
                Deactivate_pinButton();
            }
            private void StopDrag()
            {
                _parent._isDragging = false;
                CompositionTarget.Rendering -= Drag;

                bool isValid = _parent.StackPanel.IsMouseOver;
                foreach (FrameworkElement child in _parent.StackPanel.Children)
                {
                    if (child.IsMouseOver)
                    {
                        isValid = true;
                        break;
                    }
                }

                if (IsValid())
                {
                    //DebugLibrary.Console.Log();
                    _parent.DropItem(this, Mouse.GetPosition(WindowManager.MainWindow));
                }
                else
                {
                    _parent.DropItem(this, new Point(startPosition.Y, _parent.StackPanel.Children.Count - 1));
                }
            }
            private void Drag(object? sender, EventArgs e)
            {
                Point mousePos = Mouse.GetPosition(WindowManager.MainWindow);
                Canvas.SetTop(FrameworkElement, mousePos.Y);
                Canvas.SetLeft(FrameworkElement, mousePos.X);

                bool isValid = _parent.StackPanel.IsMouseOver;
                foreach (FrameworkElement child in _parent.StackPanel.Children)
                {
                    if (child.IsMouseOver)
                    {
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
