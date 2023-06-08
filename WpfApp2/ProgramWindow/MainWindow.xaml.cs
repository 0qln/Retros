using Retros.ProgramWindow;
using Retros.ProgramWindow.Interactive.Tabs;
using Retros.ProgramWindow.Interactive.Tabs.Handles;
using Retros.ProgramWindow.Interactive.Tabs.Bodies;

using Retros.ProgramWindow.DisplaySystem;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Utillities.Wpf;

namespace Retros
{
    public partial class MainWindow : Window {

        // Window Handle
        public WindowHandle? WindowHandle;

        // UI
        public Workstation SelectedWorkstation;
        public List<Workstation> Workstations = new();
        private const double shadowRectWidth = 5;


        public MainWindow() {
            // Initialize
            InitializeComponent();

            // Init window handle
            WindowHandle = new(this);
            
            // Start WindowManager
            WindowManager.Start(this);


            //UI
            Workstation workstation = new();
            Workstations.Add(workstation);
            SelectedWorkstation = workstation;

            WorkStationGrid.RowDefinitions[0].Height = new(WindowHandle.Height);
            UIManager.ColorThemeManager.Set_BG2(b => WorkStationGrid.Background = b);
            Helper.SetChildInGrid(WorkStationGrid, workstation.FrameworkElement, 1, 0);

            UIManager.ColorThemeManager.Set_BG1(b => WorkStationImageGrid.Background = b);
            Helper.SetChildInGrid(WorkStationImageGrid, workstation.ImageElement.FrameworkElement, 0, 0);

            workstation.TableElement.AddTab(new ImageFilterTab(new ImageFilter(workstation.ImageElement), new DefaultHandle("Filters")));
            workstation.TableElement.AddTab(new TestTab(new Test(workstation.ImageElement), new DefaultHandle("Testing")));
            workstation.TableElement.SelectTab(0);


            Shadow.Width = shadowRectWidth;
            Shadow.Effect = new DropShadowEffect { BlurRadius = 25, ShadowDepth = 10, Color = Colors.Black, Opacity = 0.80, Direction = 180 };
            UIManager.ColorThemeManager.Set_BG2(b => Shadow.Fill = b);
            

            // WindowHandle
            UIManager.ColorThemeManager.Set_BG3(b => WindowHandle.SetBGColor(b));
            UIManager.ColorThemeManager.Set_BGh1(b => WindowHandle.ApplicationButtons.ColorWhenButtonHover = b);

            DropDownMenu fileMenu = new("File", this);
            DropDownMenu editMenu = new("Edit", Application.Current.MainWindow);
            DropDownMenu viewMenu = new("View", Application.Current.MainWindow);
            DropDownMenu settingsMenu = new("Settings", Application.Current.MainWindow);

            WindowHandle.CreateClientButton(fileMenu);
            WindowHandle.CreateClientButton(viewMenu);
            WindowHandle.CreateClientButton(editMenu);
            WindowHandle.CreateClientButton(settingsMenu);

            fileMenu.AddOption("Save").SetKeyboardShortcut("Strg + S").AddCommand(() => UIManager.SaveImage(SelectedWorkstation.ImageElement));
            fileMenu.AddOption("Open").SetKeyboardShortcut("Strg + O").AddCommand(() => UIManager.LoadImage(SelectedWorkstation.ImageElement));
            editMenu.AddOption("Save Filter").SetKeyboardShortcut("Strg + S + F");
            viewMenu.AddOption("Zoom").SetKeyboardShortcut("Bla bla");
            settingsMenu.AddOption("Change Layout").SetKeyboardShortcut("Strg + LShift + L");

            WindowHandle.ActivateAllClientButtons();


            WindowHandle.ApplicationButtons.AddSettingsButton();
            WindowHandle.ApplicationButtons.SettingsButtonImageSource = UIManager.SettingsIconPath;
            WindowHandle.ApplicationButtons.SettingsButtonImagePadding = new Thickness(5);
            WindowHandle.ApplicationButtons.OverrideSettings(WindowManager.ToggleSettings);

            WindowHandle.ApplicationButtons.AddFullcreenButton();
            WindowHandle.ApplicationButtons.FullscreenButtonImageSource = UIManager.SettingsIconPath;
            WindowHandle.ApplicationButtons.FullscreenButtonImagePadding = new Thickness(5);
            //WindowHandle.ApplicationButtons.OverrideFullscreen(WindowManager.ToggleSettings);

            ClientGrid.RowDefinitions[0].Height = new GridLength(WindowHandle.Height);
        }


        // Shadow Behaviour
        private double offsetOnEnter = 0;
        private void Shadow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            (sender as Rectangle)!.CaptureMouse();
            CompositionTarget.Rendering += CompositionTarget_Rendering;

            double shadowPos = Helper.GetAbsolutePosition(Shadow, Application.Current.MainWindow).X;
            double mousePos = Mouse.GetPosition(Application.Current.MainWindow).X; // Mouse Position Relative to the window
            double mousePosDelta = mousePos - shadowPos; // Mouse Position Relative to the AHsoq 
            offsetOnEnter = mousePosDelta;
        }
        private void Shadow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            (sender as Rectangle)!.ReleaseMouseCapture();
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
            offsetOnEnter = 0;
        }
        private void CompositionTarget_Rendering(object? sender, EventArgs e) {
            double mousePos = Mouse.GetPosition(Application.Current.MainWindow).X; // Mouse Position Relative to the window

            double a = mousePos / ClientGrid.ActualWidth;
            double b = 1 - a;

            if (a < 0) a = shadowRectWidth / ClientGrid.ActualWidth;
            if (b < 0) b = 0;
            ClientGrid.ColumnDefinitions[0].Width = new GridLength(a, GridUnitType.Star);
            ClientGrid.ColumnDefinitions[1].Width = new GridLength(b, GridUnitType.Star);
        }

        private void Shadow_MouseEnter(object sender, MouseEventArgs e) {
            Application.Current.MainWindow.Cursor = Cursors.SizeWE;
        }

        private void Shadow_MouseLeave(object sender, MouseEventArgs e) {
            Application.Current.MainWindow.Cursor = Cursors.Arrow;
        }
    }
}
