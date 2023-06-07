using CustomControlLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;
using Utillities.Wpf;

namespace Retros.Settings {

    internal partial class TabDetail {
        public Header _Header;
        public Body _Body;

        public TabDetail(Page page) {
            _Body = new (page);
            _Header = new (page.Title);
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
                mainGrid.Margin = new Thickness(20);
                pageFrame.Content = page;
            }

            public static Style ButtonStyle() {
                Style style = new Style(typeof(Button));

                style.Setters.Add(new Setter(Button.PaddingProperty, new Thickness(10, 0, 10, 0)));
                style.Setters.Add(new Setter(Button.BackgroundProperty, UIManager.ColorThemeManager.Current.BG6));
                style.Setters.Add(new Setter(Button.ForegroundProperty, UIManager.ColorThemeManager.Current.FC1));
                style.Setters.Add(new Setter(Button.BorderBrushProperty, UIManager.ColorThemeManager.Current.BC2));
                style.Setters.Add(new Setter(Button.BorderThicknessProperty, new Thickness(1)));
                style.Setters.Add(new Setter(Button.HorizontalAlignmentProperty, HorizontalAlignment.Left));
                style.Setters.Add(new Setter(Button.VerticalAlignmentProperty, VerticalAlignment.Center));
                style.Setters.Add(new Setter(Button.HeightProperty, 20.0));
                style.Setters.Add(new Setter(Button.ContentProperty, new TextBlock { Margin = new Thickness(10, 0, 10, 0) } ));


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

                buttonTemplate.Triggers.Add(mouseOverTrigger);

                style.Setters.Add(new Setter(Button.TemplateProperty, buttonTemplate));

                style.Seal();

                return style;
            }

            public static Style HeadlineStyle() {
                Style style = new Style(typeof(TextBlock));
                style.Setters.Add(new Setter(TextBlock.ForegroundProperty, UIManager.ColorThemeManager.Current.FC1));
                style.Setters.Add(new Setter(TextBlock.BackgroundProperty, UIManager.ColorThemeManager.Current.BG6));
                style.Setters.Add(new Setter(TextBlock.FontWeightProperty, FontWeights.SemiBold));
                style.Setters.Add(new Setter(TextBlock.FontSizeProperty, 30.0));

                return style;
            }

            public static Style SelectionBoxStyle() {
                Style style = new Style(typeof(SelectionBox));
                style.Setters.Add(new Setter(SelectionBox.OptionsTextBrushProperty, UIManager.ColorThemeManager.Current.FC1));
                style.Setters.Add(new Setter(SelectionBox.OptionsBackgroundProperty, UIManager.ColorThemeManager.Current.BG1));
                style.Setters.Add(new Setter(SelectionBox.OptionsBorderThicknessProperty, new Thickness(1.0)));
                style.Setters.Add(new Setter(SelectionBox.OptionsBorderBrushProperty, UIManager.ColorThemeManager.Current.BC1));
                style.Setters.Add(new Setter(SelectionBox.OptionsBackgroundOnHoverProperty, UIManager.ColorThemeManager.Current.BG2));
                style.Setters.Add(new Setter(SelectionBox.OptionBoxBorderBrushProperty, UIManager.ColorThemeManager.Current.BC2));
                style.Setters.Add(new Setter(SelectionBox.BackgroundProperty, UIManager.ColorThemeManager.Current.BG1));
                style.Setters.Add(new Setter(SelectionBox.BorderBrushProperty, UIManager.ColorThemeManager.Current.BC2));
                style.Setters.Add(new Setter(SelectionBox.BorderBrushOnHoverProperty, UIManager.ColorThemeManager.Current.BCh2));
                style.Setters.Add(new Setter(SelectionBox.BorderThicknessProperty, new Thickness(1)));
                style.Setters.Add(new Setter(SelectionBox.ForegroundProperty, UIManager.ColorThemeManager.Current.FC1));
                style.Setters.Add(new Setter(SelectionBox.HeightProperty, 20.0));



                return style;
            }

            public static Style CheckBoxStyle() {
                Style style = new Style(typeof (CheckBox));

                return style;
            }

            public static Style TextblockStyle() {
                Style style = new Style(typeof(TextBlock));
                style.Setters.Add(new Setter(TextBlock.MarginProperty, new Thickness(0, 5, 0, 0)));
                style.Setters.Add(new Setter(TextBlock.BackgroundProperty, UIManager.ColorThemeManager.Current.BG2));
                style.Setters.Add(new Setter(TextBlock.ForegroundProperty, UIManager.ColorThemeManager.Current.FC1));
                style.Setters.Add(new Setter(TextBlock.FontSizeProperty, 13.0));
                style.Setters.Add(new Setter(TextBlock.FontStyleProperty, FontStyles.Italic));
                style.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));

                return style;
            }

            public static Style TextboxStyle() {
                Style style = new Style(typeof(TextBox));
                style.Setters.Add(new Setter(TextBox.MinWidthProperty, 100.0));
                style.Setters.Add(new Setter(TextBox.BackgroundProperty, UIManager.ColorThemeManager.Current.BG1));
                style.Setters.Add(new Setter(TextBox.ForegroundProperty, UIManager.ColorThemeManager.Current.FC2));
                style.Setters.Add(new Setter(TextBox.MaxLinesProperty, 1));
                style.Setters.Add(new Setter(TextBox.TextWrappingProperty, TextWrapping.Wrap));

                return style;
            }
        }
    }
}
