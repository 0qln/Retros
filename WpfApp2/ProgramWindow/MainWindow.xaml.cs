﻿using Retros.ProgramWindow;
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
        public WindowHandle? windowHandle;

        // UI
        public Workstation Workstation = new();
        private const double shadowRectWidth = 5;


        public MainWindow() {
            // Initialize
            InitializeComponent();

            // Init window handle
            windowHandle = new(this);
            
            // Start WindowManager
            WindowManager.Start(this);


            //UI
            WorkStationGrid.RowDefinitions[0].Height = new(windowHandle.Height);
            WorkStationGrid.Background = UIManager.Background_Colors[1];
            Helper.SetChildInGrid(WorkStationGrid, Workstation.FrameworkElement, 1, 0);
            
            WorkStationImageGrid.Background = UIManager.Background_Colors[0];
            Helper.SetChildInGrid(WorkStationImageGrid, Workstation.ImageElement.Grid, 0, 0);

            Workstation.TableElement.AddTab(new ImageFilterTab(new ImageFilter(), new DefaultHandle("Filters")));
            Workstation.TableElement.SelectTab(0);

            Shadow.Width = shadowRectWidth;
            Shadow.Effect = new DropShadowEffect { BlurRadius = 25, ShadowDepth = 10, Color = Colors.Black, Opacity = 0.80, Direction = 180 };
            Shadow.Fill = UIManager.Background_Colors[1];
            

            // WindowHandle
            windowHandle.SetParentWindow(MainCanvas);
            windowHandle.SetBGColor(UIManager.Background_Colors[2]);
            windowHandle.ApplicationButtons.ColorWhenButtonHover = UIManager.Highlight_Colors[0];


            DropDownMenu fileMenu = new("File", this);
            DropDownMenu editMenu = new("Edit", Application.Current.MainWindow);
            DropDownMenu viewMenu = new("View", Application.Current.MainWindow);
            DropDownMenu settingsMenu = new("Settings", Application.Current.MainWindow);

            windowHandle.CreateClientButton(fileMenu);
            windowHandle.CreateClientButton(viewMenu);
            windowHandle.CreateClientButton(editMenu);
            windowHandle.CreateClientButton(settingsMenu);

            fileMenu.AddOption("Save").SetKeyboardShortcut("Strg + S");
            fileMenu.AddOption("Open").SetKeyboardShortcut("Strg + O").AddCommand(UIManager.LoadImage);
            editMenu.AddOption("Save Filter").SetKeyboardShortcut("Strg + S + F");
            viewMenu.AddOption("Zoom").SetKeyboardShortcut("Bla bla");
            settingsMenu.AddOption("Change Layout").SetKeyboardShortcut("Strg + LShift + L");

            windowHandle.ActivateAllClientButtons();


            windowHandle.ApplicationButtons.AddSettingsButton();
            windowHandle.ApplicationButtons.SettingsButtonImageSource = "pack://application:,,,/settings5.png";
            windowHandle.ApplicationButtons.SettingsButtonImagePadding = new Thickness(5);
            windowHandle.ApplicationButtons.OverrideSettings(WindowManager.ToggleSettings);
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
