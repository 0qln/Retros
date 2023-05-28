using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Utillities.Wpf;

namespace Retros.Settings {

    internal partial class TabDetail {
        public Header _Header;
        public Body _Body;

        public TabDetail(string name, Page page) {
            _Header = new (name);
            _Body = new (page);
        }
    }

    internal partial class TabDetail {
        public class Header : IFrameworkElement {
            public FrameworkElement FrameworkElement => Title;

            public double Height => height;
            public Button Title = new();

            private double height;
            private Brush colorOnHover = Helper.StringToSolidColorBrush("#000000", 0.1);
            private Brush borderColorOnHover = Helper.StringToSolidColorBrush("#000000", 0);

            public Header(string name = "No name specified", double height = 25) {
                Title.Content = name;
                this.height = height;

                UpdateTitleStyle();
            }

            public void UpdateTitleStyle() {
                Title.Style = TitleButtonStyle();
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
                style.Setters.Add(new Setter(Button.HeightProperty, height));
                style.Setters.Add(new Setter(Button.FontWeightProperty, FontWeights.Normal));

                ControlTemplate userButtonTemplate = new ControlTemplate(typeof(Button));
                FrameworkElementFactory borderFactory = new FrameworkElementFactory(typeof(Border));
                borderFactory.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Button.BackgroundProperty));
                borderFactory.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Button.BorderBrushProperty));
                borderFactory.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Button.BorderThicknessProperty));

                FrameworkElementFactory contentPresenterFactory = new FrameworkElementFactory(typeof(ContentPresenter));
                contentPresenterFactory.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Left);
                contentPresenterFactory.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
                contentPresenterFactory.SetValue(ContentPresenter.MarginProperty, new Thickness(40, 0, 0, 0));

                borderFactory.AppendChild(contentPresenterFactory);

                userButtonTemplate.VisualTree = borderFactory;

                Trigger mouseOverTrigger = new Trigger { Property = Button.IsMouseOverProperty, Value = true };
                mouseOverTrigger.Setters.Add(new Setter(Button.BackgroundProperty, colorOnHover));
                mouseOverTrigger.Setters.Add(new Setter(Button.BorderBrushProperty, borderColorOnHover));

                userButtonTemplate.Triggers.Add(mouseOverTrigger);

                style.Setters.Add(new Setter(Button.TemplateProperty, userButtonTemplate));

                style.Seal();

                return style;
            }
        }
    }

    internal partial class TabDetail {
        public class Body : IFrameworkElement {
            public FrameworkElement FrameworkElement => mainGrid;

            private Grid mainGrid = new();
            private Frame pageFrame = new();

            public Body(Page page) {
                mainGrid.Children.Add(pageFrame);
                pageFrame.Content = page;
            }
        }
    }
}
