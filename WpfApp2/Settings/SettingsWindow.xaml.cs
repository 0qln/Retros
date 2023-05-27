using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Shell;
using WpfUtillities;

namespace Retros {
    public partial class SettingsWindow : Window {
        public static WindowHandle? WindowHandle;

        public SettingsWindow() {

            InitializeComponent();

            WindowHandle = new(this);
            WindowHandle.SetParentWindow(MainCanvas!);
            WindowHandle.SetBGColor(WindowManager.whBackground);
            WindowHandle.ApplicationButtons.ColorWhenButtonHover = WindowManager.whApplicationButtonHover;
            WindowHandle.SetWindowChromeActiveAll();

            SettingsList_Padding.MinHeight = WindowHandle.Height;

            SettingsList.Background = WindowManager.WorkStationImageGrid_BG;
            SettingDetailDisplay.Background = WindowManager.WorkStationGrid_BG;

            
            SettingHeaderButtonHeight = 30;
            SettingHeaderButtonColorWhenHover = Helper.StringToSolidColorBrush("#000000", 0.3);
            SettingHeaderButtonBorderColorWhenHover = Helper.StringToSolidColorBrush("#000000", 0);
            SettingHeaderArrowButtonBorderColorWhenHover = Helper.StringToSolidColorBrush("#FFFFFF", 0.1);

            AppearanceToggle.Style = SettingHeaderArrowButtonStyle();
            AppearanceButton.Style = SettingHeaderButtonStyle();
            ThemesButton.Style = SettingHeaderFootButtonStyle();
            TabsButton.Style = SettingHeaderFootButtonStyle();

            ExportToggle.Style = SettingHeaderArrowButtonStyle();
            ExportButton.Style = SettingHeaderButtonStyle();

            ImportToggle.Style = SettingHeaderArrowButtonStyle();
            ImportButton.Style = SettingHeaderButtonStyle();
        }


        public static double SettingHeaderButtonHeight { get; set; }
        public static Brush SettingHeaderButtonColorWhenHover { get; set; }
        public static Brush SettingHeaderButtonBorderColorWhenHover { get; set; }
        public static Brush SettingHeaderArrowButtonBorderColorWhenHover { get; set; }
        public static Style SettingHeaderButtonStyle() {
            Style style = new();

            style.Setters.Add(new Setter(Button.MarginProperty, new Thickness(0, 0, 0, 0)));
            style.Setters.Add(new Setter(Button.BackgroundProperty, Brushes.Transparent));
            style.Setters.Add(new Setter(Button.ForegroundProperty, Brushes.White));
            style.Setters.Add(new Setter(Button.BorderBrushProperty, Brushes.Transparent));
            style.Setters.Add(new Setter(Button.BorderThicknessProperty, new Thickness(1)));
            style.Setters.Add(new Setter(Button.HorizontalAlignmentProperty, HorizontalAlignment.Stretch));
            style.Setters.Add(new Setter(Button.VerticalAlignmentProperty, VerticalAlignment.Center));
            style.Setters.Add(new Setter(Button.HeightProperty, SettingHeaderButtonHeight));
            style.Setters.Add(new Setter(Button.FontWeightProperty, FontWeights.Bold));

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
            mouseOverTrigger.Setters.Add(new Setter(Button.BackgroundProperty, SettingHeaderArrowButtonBorderColorWhenHover));
            mouseOverTrigger.Setters.Add(new Setter(Button.BorderBrushProperty, SettingHeaderButtonBorderColorWhenHover));

            userButtonTemplate.Triggers.Add(mouseOverTrigger);

            style.Setters.Add(new Setter(Button.TemplateProperty, userButtonTemplate));

            style.Seal();

            return style;
        }
        public static Style SettingHeaderArrowButtonStyle() {
            Style style = new();

            style.Setters.Add(new Setter(Button.MarginProperty, new Thickness(0, 0, 0, 0)));
            style.Setters.Add(new Setter(Button.BackgroundProperty, Brushes.Transparent));
            style.Setters.Add(new Setter(Button.ForegroundProperty, Brushes.White));
            style.Setters.Add(new Setter(Button.BorderBrushProperty, Brushes.Transparent));
            style.Setters.Add(new Setter(Button.BorderThicknessProperty, new Thickness(1)));
            style.Setters.Add(new Setter(Button.HorizontalAlignmentProperty, HorizontalAlignment.Center));
            style.Setters.Add(new Setter(Button.VerticalAlignmentProperty, VerticalAlignment.Center));
            style.Setters.Add(new Setter(Button.HeightProperty, SettingHeaderButtonHeight));
            style.Setters.Add(new Setter(Button.FontWeightProperty, FontWeights.Bold));


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
            mouseOverTrigger.Setters.Add(new Setter(Button.BackgroundProperty, SettingHeaderArrowButtonBorderColorWhenHover));
            mouseOverTrigger.Setters.Add(new Setter(Button.BorderBrushProperty, SettingHeaderButtonBorderColorWhenHover));

            userButtonTemplate.Triggers.Add(mouseOverTrigger);

            style.Setters.Add(new Setter(Button.TemplateProperty, userButtonTemplate));

            style.Seal();

            return style;
        }
        public static Style SettingHeaderFootButtonStyle() {
            Style style = new();

            style.Setters.Add(new Setter(Button.MarginProperty, new Thickness(0, 0, 0, 0)));
            style.Setters.Add(new Setter(Button.BackgroundProperty, Brushes.Transparent));
            style.Setters.Add(new Setter(Button.ForegroundProperty, Brushes.White));
            style.Setters.Add(new Setter(Button.BorderBrushProperty, Brushes.Transparent));
            style.Setters.Add(new Setter(Button.BorderThicknessProperty, new Thickness(1)));
            style.Setters.Add(new Setter(Button.HorizontalAlignmentProperty, HorizontalAlignment.Stretch));
            style.Setters.Add(new Setter(Button.VerticalAlignmentProperty, VerticalAlignment.Center));
            style.Setters.Add(new Setter(Button.HeightProperty, SettingHeaderButtonHeight));
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
            mouseOverTrigger.Setters.Add(new Setter(Button.BackgroundProperty, SettingHeaderButtonColorWhenHover));
            mouseOverTrigger.Setters.Add(new Setter(Button.BorderBrushProperty, SettingHeaderButtonBorderColorWhenHover));

            userButtonTemplate.Triggers.Add(mouseOverTrigger);

            style.Setters.Add(new Setter(Button.TemplateProperty, userButtonTemplate));

            style.Seal();

            return style;
        }
        

        
        private void AppearanceButton_Click(object sender, RoutedEventArgs e) {

        }

        private void ExportButton_Click(object sender, RoutedEventArgs e) {

        }

        private void ImportButton_Click(object sender, RoutedEventArgs e) {

        }

        private void ThemesButton_Click(object sender, RoutedEventArgs e) {

        }

        private void TabsButton_Click(object sender, RoutedEventArgs e) {

        }


        private void ToggleArrow(Button toggle, StackPanel children) {
            if (children.Visibility == Visibility.Visible) {
                children.Visibility = Visibility.Collapsed;
                toggle.Content = "v";
            }
            else {
                children.Visibility = Visibility.Visible;
                toggle.Content = ">";
            }
        }

        private void AppearanceToggle_Click(object sender, RoutedEventArgs e) => ToggleArrow(AppearanceToggle, AppearanceChildren);

        private void ExportToggle_Click(object sender, RoutedEventArgs e) => ToggleArrow(ExportToggle, ExportChildren);

        private void ImportToggle_Click(object sender, RoutedEventArgs e) => ToggleArrow(ImportToggle, ImportChildren);

    }
}
