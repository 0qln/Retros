using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Utillities.Wpf;

namespace Retros.Settings {
    internal partial class Tab {
        public Header _Header;
        public Body _Body;

        public Tab(string name) {
            _Body = new();
            _Header = new(_Body, name);
        }

        public Tab AddDetail(TabDetail tabDetail) {
            _Header.AddDetail(tabDetail);
            _Body.AddDetail(tabDetail);
            return this;
        }
    }


    internal partial class Tab{
        public class Header : IFrameworkElement {
            public FrameworkElement FrameworkElement => mainGrid;

            // all of these should have a set; which calls an update function when called
            public double TitleHeight => titleHeight;
            public double ArrowWidth => arrowWidth;

            private double titleHeight;
            private double arrowWidth;
            private Brush titleColorOnHover = UIManager.ColorThemeManager.Current.BGh2;
            private Brush titleBorderOnHover = UIManager.ColorThemeManager.Current.BCh1;
            private Brush arrowColorOnHover = UIManager.ColorThemeManager.Current.BGh1;
            private Brush arrowBorderOnHover = UIManager.ColorThemeManager.Current.BCh1;
            private Grid mainGrid = new(); // contains all
            private Grid headerGrid = new();
            private Button arrow = new(); // show/ hide children
            private Button title = new(); // open the tab
            private StackPanel detailTitles = new(); // contains the FrameworkElements of the headers of the detail tabs


            public Header(Body body, string name = "No name specified", double height = 30, double minArrowButtonWidth = 25) {
                titleHeight = height;
                arrowWidth = minArrowButtonWidth;
                title.Content = name;
                detailTitles.Visibility = Visibility.Collapsed;

                WindowManager.SettingsWindow!.SettingsList.Children.Add(FrameworkElement);
                Helper.AddRow(mainGrid, height, GridUnitType.Pixel);
                Helper.AddRow(mainGrid, 1, GridUnitType.Auto);
                Helper.SetChildInGrid(mainGrid, headerGrid, 0, 0);
                Helper.SetChildInGrid(mainGrid, detailTitles, 1, 0);

                Helper.AddColumn(headerGrid, 1, GridUnitType.Auto);
                Helper.AddColumn(headerGrid, 1, GridUnitType.Star);
                Helper.SetChildInGrid(headerGrid, arrow, 0, 0);
                Helper.SetChildInGrid(headerGrid, title, 0, 1);

                UIManager.ColorThemeManager.Set_BGh2(b => { titleColorOnHover = b; UpdateTitle(); } );
                UIManager.ColorThemeManager.Set_BCh1(b => { titleBorderOnHover = b; UpdateTitle(); } );
                UIManager.ColorThemeManager.Set_BGh1(b => { arrowColorOnHover = b; UpdateArrow(); });
                UIManager.ColorThemeManager.Set_BCh1(b => { arrowColorOnHover = b; UpdateArrow(); });

                UpdateArrow();
                UpdateTitle();

                arrow.Click += (s, e) => {
                    if (detailTitles.Visibility == Visibility.Visible) {
                        detailTitles.Visibility = Visibility.Collapsed;
                        arrow.Content = "🢂";
                    }
                    else {
                        detailTitles.Visibility = Visibility.Visible;
                        arrow.Content = "🢆";
                    }
                };
                title.Click += (s, e) => body.Show();

            }
            public void AddDetail(TabDetail tabDetail) {
                detailTitles.Children.Add(tabDetail._Header.FrameworkElement);
            }

            public void UpdateStyles() {
                UpdateArrow();
                UpdateTitle();
            }

            public void UpdateArrow() {
                arrow.Style = ArrowButtonStyle();
            }
            public void UpdateTitle() {
                title.Style = TitleButtonStyle();
            }


            public Style ArrowButtonStyle() {
                Style style = new();

                style.Setters.Add(new Setter(Button.MarginProperty, new Thickness(0, 0, 0, 0)));
                style.Setters.Add(new Setter(Button.BackgroundProperty, Brushes.Transparent));
                style.Setters.Add(new Setter(Button.ForegroundProperty, Brushes.White));
                style.Setters.Add(new Setter(Button.BorderBrushProperty, Brushes.Transparent));
                style.Setters.Add(new Setter(Button.BorderThicknessProperty, new Thickness(1)));
                style.Setters.Add(new Setter(Button.HorizontalAlignmentProperty, HorizontalAlignment.Center));
                style.Setters.Add(new Setter(Button.VerticalAlignmentProperty, VerticalAlignment.Center));
                style.Setters.Add(new Setter(Button.HeightProperty, titleHeight));
                style.Setters.Add(new Setter(Button.FontWeightProperty, FontWeights.Bold));
                style.Setters.Add(new Setter(Button.MinWidthProperty, arrowWidth));
                style.Setters.Add(new Setter(Button.ContentProperty, "🢂"));


                ControlTemplate userButtonTemplate = new ControlTemplate(typeof(Button));
                FrameworkElementFactory borderFactory = new FrameworkElementFactory(typeof(Border));
                borderFactory.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Button.BackgroundProperty));
                borderFactory.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Button.BorderBrushProperty));
                borderFactory.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Button.BorderThicknessProperty));

                FrameworkElementFactory contentPresenterFactory = new FrameworkElementFactory(typeof(ContentPresenter));
                contentPresenterFactory.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                contentPresenterFactory.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
                contentPresenterFactory.SetValue(ContentPresenter.MarginProperty, new Thickness(0, 0, 0, 0));

                borderFactory.AppendChild(contentPresenterFactory);

                userButtonTemplate.VisualTree = borderFactory;

                Trigger mouseOverTrigger = new Trigger { Property = Button.IsMouseOverProperty, Value = true };
                mouseOverTrigger.Setters.Add(new Setter(Button.BackgroundProperty, arrowColorOnHover));
                mouseOverTrigger.Setters.Add(new Setter(Button.BorderBrushProperty, arrowBorderOnHover));

                userButtonTemplate.Triggers.Add(mouseOverTrigger);

                style.Setters.Add(new Setter(Button.TemplateProperty, userButtonTemplate));

                style.Seal();

                return style;
            }
            public Style TitleButtonStyle() {
                Style style = new();

                style.Setters.Add(new Setter(Button.MarginProperty, new Thickness(0, 0, 0, 0)));
                style.Setters.Add(new Setter(Button.BackgroundProperty, Brushes.Transparent));
                style.Setters.Add(new Setter(Button.ForegroundProperty, Brushes.White));
                style.Setters.Add(new Setter(Button.BorderBrushProperty, Brushes.Transparent));
                style.Setters.Add(new Setter(Button.BorderThicknessProperty, new Thickness(1)));
                style.Setters.Add(new Setter(Button.HorizontalAlignmentProperty, HorizontalAlignment.Stretch));
                style.Setters.Add(new Setter(Button.VerticalAlignmentProperty, VerticalAlignment.Center));
                style.Setters.Add(new Setter(Button.HeightProperty, titleHeight));
                style.Setters.Add(new Setter(Button.FontWeightProperty, FontWeights.SemiBold));
                style.Setters.Add(new Setter(Button.FontSizeProperty, 14.0));

                ControlTemplate userButtonTemplate = new ControlTemplate(typeof(Button));
                FrameworkElementFactory borderFactory = new FrameworkElementFactory(typeof(Border));
                borderFactory.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Button.BackgroundProperty));
                borderFactory.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Button.BorderBrushProperty));
                borderFactory.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Button.BorderThicknessProperty));

                FrameworkElementFactory contentPresenterFactory = new FrameworkElementFactory(typeof(ContentPresenter));
                contentPresenterFactory.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Left);
                contentPresenterFactory.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
                contentPresenterFactory.SetValue(ContentPresenter.MarginProperty, new Thickness(10, 0, 0, 0));

                borderFactory.AppendChild(contentPresenterFactory);

                userButtonTemplate.VisualTree = borderFactory;

                Trigger mouseOverTrigger = new Trigger { Property = Button.IsMouseOverProperty, Value = true };
                mouseOverTrigger.Setters.Add(new Setter(Button.BackgroundProperty, titleColorOnHover));
                mouseOverTrigger.Setters.Add(new Setter(Button.BorderBrushProperty, titleBorderOnHover));

                userButtonTemplate.Triggers.Add(mouseOverTrigger);

                style.Setters.Add(new Setter(Button.TemplateProperty, userButtonTemplate));

                style.Seal();

                return style;
            }
        }
    }

    internal partial class Tab {
        public class Body : IFrameworkElement {
            public FrameworkElement FrameworkElement => details;

            private StackPanel details = new();

            public Body() {
                Helper.SetChildInGrid(WindowManager.SettingsWindow.SettingDetailDisplay, details, 0, 0);
                details.Visibility = Visibility.Collapsed;
            }

            public void Show() {
                foreach (FrameworkElement child in WindowManager.SettingsWindow!.SettingDetailDisplay.Children) {
                    child.Visibility = Visibility.Collapsed;
                }
                FrameworkElement.Visibility = Visibility.Visible;
            }

            public void AddDetail(TabDetail tabDetail) {
                details.Children.Add(tabDetail._Body.FrameworkElement);
            }
        }
    }


}
