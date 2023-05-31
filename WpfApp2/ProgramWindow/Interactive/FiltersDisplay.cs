﻿using System;
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
using Utillities.Wpf;

namespace Retros.ProgramWindow.Interactive
{
    public class FilterDisplay : IFrameworkElement
    {
        public FrameworkElement FrameworkElement => StackPanel;
        public StackPanel StackPanel = new();

        private List<Item> items = new();

        public FilterDisplay() {
            ///for (int i = 0; i < 10; i++) AddItem("Item" + i.ToString());

        }

        public void AddItem(string name) {
            Item item = new(name, this);
            StackPanel.Children.Add(item.FrameworkElement);
            items.Add(item);

            InvokeItemListChanged();
        }

        public void RemoveItem(string name) {
            Item item = items.Find(item => item.TextBlock.Text == name)!;
            if (item == null) return;
            items.Remove(item);
            StackPanel.Children.Remove(item.FrameworkElement);

            InvokeItemListChanged();
        }


        private Point GetPos(FrameworkElement item) {
            var p = item.TransformToAncestor(WindowManager.MainWindow).Transform(new Point(0, 0));
            return new Point(p.X + item.ActualWidth / 2, p.Y + item.ActualHeight / 2);
        }


        private void InsertItem(Item item, Point newPosition) {
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

            items.Remove(item);
            items.Insert(index, item);

            WindowManager.MainWindow!.MainCanvas.Children.Remove(item.FrameworkElement);
            StackPanel.Children.Insert(index, item.FrameworkElement);

            InvokeItemListChanged();
        }


        private void InvokeItemListChanged() {
            List<string> list = new();
            items.ForEach(item => list.Add(item.TextBlock.Text));
            WindowManager.MainWindow!.Workstation.ImageElement.GetFilterManager.Order(list);
        }



        private class Item : IFrameworkElement {
            public FrameworkElement FrameworkElement => MainGrid;
            public Grid MainGrid = new();
            public TextBlock TextBlock = new();
            private FilterDisplay parent;

            public Item(string name, FilterDisplay parent) {
                this.parent = parent;

                TextBlock.Text = name;
                TextBlock.FontSize = 14;
                UIManager.ColorThemeManager.Set_BG3(b => TextBlock.Background = b);
                UIManager.ColorThemeManager.Set_FC1(b => TextBlock.Foreground = b);
                MainGrid.Children.Add(TextBlock);

                FrameworkElement.PreviewMouseLeftButtonDown += (s, e) => StartDrag();
                FrameworkElement.PreviewMouseLeftButtonUp += (s, e) => StopDrag();
            }


            public Point GetCenteredPosition() {
                var a = MainGrid.TransformToAncestor(WindowManager.MainWindow.MainCanvas).Transform(new Point(0, 0));
                var b = MainGrid.ActualWidth;
                var c = MainGrid.ActualHeight;

               return new Point(a.X + b/2, a.Y + c/2);
            }

            private bool IsValid() {
                StackPanel s = parent.StackPanel;
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
                parent.StackPanel.Children.Remove(FrameworkElement);
                WindowManager.MainWindow!.MainCanvas.Children.Add(FrameworkElement);
            }
            private void StopDrag() {
                CompositionTarget.Rendering -= Drag;

                bool isValid = parent.StackPanel.IsMouseOver;
                foreach ( FrameworkElement child in parent.StackPanel.Children ) {
                    if (child.IsMouseOver) {
                        isValid = true;
                        break;
                    }
                }

                if (IsValid()) {
                    parent.InsertItem(this, Mouse.GetPosition(WindowManager.MainWindow));
                }
                else {
                    parent.InsertItem(this, startPosition);
                }
            }
            private void Drag(object? sender, EventArgs e) {
                Point mousePos = Mouse.GetPosition(WindowManager.MainWindow);
                Canvas.SetTop(FrameworkElement, mousePos.Y);
                Canvas.SetLeft(FrameworkElement, mousePos.X);

                bool isValid = parent.StackPanel.IsMouseOver;
                foreach (FrameworkElement child in parent.StackPanel.Children) {
                    if (child.IsMouseOver) {
                        isValid = true;
                        break;
                    }
                }

                if (!WindowManager.MainWindow.MainCanvas.IsMouseOver
                    || Mouse.LeftButton != MouseButtonState.Pressed) 
                    StopDrag();
            }


        }
    }
}