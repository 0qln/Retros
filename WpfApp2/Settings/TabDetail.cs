using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
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
            private Brush colorOnHover = UIManager.ColorThemeManager.Current.BGh3;
            private Brush borderColorOnHover = UIManager.ColorThemeManager.Current.BCh1;

            public Header(string name = "No name specified", double height = 25) {
                Title.Content = name;
                this.height = height;

                UIManager.ColorThemeManager.Set_BGh3(b => { colorOnHover = b; UpdateTitleStyle(); });
                UIManager.ColorThemeManager.Set_BCh1(b => { borderColorOnHover = b; UpdateTitleStyle(); });

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

            public static Style ButtonStyle(string name) {
                Style style = new Style(typeof(Button));

                style.Setters.Add(new Setter(Button.PaddingProperty, new Thickness(10, 0, 10, 0)));
                style.Setters.Add(new Setter(Button.BackgroundProperty, UIManager.ColorThemeManager.Current.BG6));
                style.Setters.Add(new Setter(Button.ForegroundProperty, UIManager.ColorThemeManager.Current.FC1));
                style.Setters.Add(new Setter(Button.BorderBrushProperty, UIManager.ColorThemeManager.Current.BC2));
                style.Setters.Add(new Setter(Button.BorderThicknessProperty, new Thickness(1)));
                style.Setters.Add(new Setter(Button.HorizontalAlignmentProperty, HorizontalAlignment.Left));
                style.Setters.Add(new Setter(Button.VerticalAlignmentProperty, VerticalAlignment.Center));
                style.Setters.Add(new Setter(Button.HeightProperty, 25.0));
                style.Setters.Add(new Setter(Button.ContentProperty, new TextBlock { Text = name, Margin = new Thickness(10, 0, 10, 0) } ));


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

                userButtonTemplate.Triggers.Add(mouseOverTrigger);

                style.Setters.Add(new Setter(Button.TemplateProperty, userButtonTemplate));

                style.Seal();

                return style;
            }

            public static Style HeadlineStyle(string text) {
                Style style = new Style(typeof(TextBlock));
                style.Setters.Add(new Setter(TextBlock.ForegroundProperty, UIManager.ColorThemeManager.Current.FC1));
                style.Setters.Add(new Setter(TextBlock.BackgroundProperty, UIManager.ColorThemeManager.Current.BG6));
                style.Setters.Add(new Setter(TextBlock.FontWeightProperty, FontWeights.SemiBold));
                style.Setters.Add(new Setter(TextBlock.TextProperty, text));
                style.Setters.Add(new Setter(TextBlock.FontSizeProperty, 30.0));

                return style;
            }

            public static Style ComboboxStyle() {
                Style style = new Style(typeof(ComboBox));
                style.Setters.Add(new Setter(ComboBox.BackgroundProperty, UIManager.ColorThemeManager.Current.BG2));
                return style;
            }

            public static Style TextblockStyle(string content) {
                Style style = new Style(typeof(TextBlock));
                style.Setters.Add(new Setter(TextBlock.MarginProperty, new Thickness(0, 5, 0, 0)));
                style.Setters.Add(new Setter(TextBlock.BackgroundProperty, UIManager.ColorThemeManager.Current.BG2));
                style.Setters.Add(new Setter(TextBlock.ForegroundProperty, UIManager.ColorThemeManager.Current.FC1));
                style.Setters.Add(new Setter(TextBlock.TextProperty, content));
                style.Setters.Add(new Setter(TextBlock.FontSizeProperty, 13.0));
                style.Setters.Add(new Setter(TextBlock.FontStyleProperty, FontStyles.Italic));
                style.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));

                return style;
            }

            public static Style TextboxStyle(string content = "") {
                Style style = new Style(typeof(TextBox));
                style.Setters.Add(new Setter(TextBox.MinWidthProperty, 100.0));
                style.Setters.Add(new Setter(TextBox.MarginProperty, new Thickness(10, 0, 0, 0)));
                style.Setters.Add(new Setter(TextBox.BackgroundProperty, UIManager.ColorThemeManager.Current.BG1));
                style.Setters.Add(new Setter(TextBox.ForegroundProperty, UIManager.ColorThemeManager.Current.FC2));
                style.Setters.Add(new Setter(TextBox.MaxLinesProperty, 1));
                style.Setters.Add(new Setter(TextBox.TextProperty, content));
                style.Setters.Add(new Setter(TextBox.TextWrappingProperty, TextWrapping.Wrap));

                return style;
            }
        }
    }
}
