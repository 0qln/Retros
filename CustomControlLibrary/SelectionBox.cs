using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Utillities.Wpf;

namespace CustomControlLibrary {
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:CustomControlLibrary"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:CustomControlLibrary;assembly=CustomControlLibrary"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class SelectionBox : Control {
        private Popup? _popup;
        private Button? _toggleButton;
        private TextBlock? _textText;
        private TextBlock? _arrowText;
        private bool _isCollapsed = true;
        private HashSet<string> _optionSet = new();
        private StackPanel? _options;

        public bool IsCollapsed => _isCollapsed;


        static SelectionBox() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SelectionBox), new FrameworkPropertyMetadata(typeof(SelectionBox)));
        }

        public override void OnApplyTemplate() {
            base.OnApplyTemplate();


            _toggleButton = GetTemplateChild("_toggleButton") as Button;
            if (_toggleButton != null) {
                _toggleButton.Click += Button_Click;
            }

            _popup = GetTemplateChild("_popup") as Popup;

            _textText = GetTemplateChild("_textText") as TextBlock;

            _arrowText = GetTemplateChild("_arrowText") as TextBlock;

            _options = GetTemplateChild("_options") as StackPanel;


            Loaded += delegate {
                if (_toggleButton is not null && _popup is not null) {

                    var adornerLayer = AdornerLayer.GetAdornerLayer(_toggleButton);
                    adornerLayer.Add(new PoppupAdorner(_toggleButton));

                    Window.GetWindow(_toggleButton).LocationChanged += (s,e) => UpadteOptionsPosition();
                }
            };
        }


        public void UpadteOptionsPosition() {
            if (_popup is null) {
                return;
            }
            _popup.HorizontalOffset = 1;
            _popup.VerticalOffset = 1;
            _popup.HorizontalOffset = 0;
            _popup.VerticalOffset = 0;
        }


        private void Button_Click(object sender, RoutedEventArgs e) {
            if (_arrowText is null
                || _popup is null) return;


            if (_isCollapsed) {
                _isCollapsed = false;
                _arrowText.Text = "⮟";
                _popup.IsOpen = true;
            }
            else {
                _isCollapsed = true;
                _arrowText.Text = "⮝";
                _popup.IsOpen = false;
            }

            DebugLibrary.Console.Log($"Collapsed: {IsCollapsed}");
        }

        public void AddOption(string newOption) {
            if (_optionSet.Contains(newOption)
                || _options == null) return;

            _optionSet.Add(newOption);
            _options.Children.Add(new Button {
                Content = newOption
            });
        }
    }

    // Adorners must subclass the abstract base class Adorner.
    internal class PoppupAdorner : Adorner {
        // Be sure to call the base class constructor.
        public PoppupAdorner(UIElement adornedElement) : base(adornedElement) { }

        // A common way to implement an adorner's rendering behavior is to override the OnRender
        // method, which is called by the layout system as part of a rendering pass.
        protected override void OnRender(DrawingContext drawingContext) {
            Rect adornedElementRect = new Rect(this.AdornedElement.DesiredSize);

            // Some arbitrary drawing implements.
            SolidColorBrush renderBrush = new SolidColorBrush(Colors.Green);
            renderBrush.Opacity = 0.2;
            Pen renderPen = new Pen(new SolidColorBrush(Colors.Navy), 1.5);
            double renderRadius = 5.0;

            // Draw a circle at each corner.
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopLeft, renderRadius, renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopRight, renderRadius, renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomLeft, renderRadius, renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomRight, renderRadius, renderRadius);
        }
    }

}
