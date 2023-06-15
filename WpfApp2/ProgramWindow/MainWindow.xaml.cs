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


        public MainWindow() {
            // Initialize
            InitializeComponent();

            // Init window handle
            WindowHandle = new(this);
            
            // Start WindowManager
            WindowManager.Start(this);


            // UI
            Workstation workstation = new(WindowHandle.Height);
            Workstations.Add(workstation);
            SelectedWorkstation = workstation;
            Helper.SetChildInGrid(ClientGrid, workstation.FrameworkElement, 1, 0);
            

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
            editMenu.AddOption("Undo").SetKeyboardShortcut("Strg + Z");
            InputBindings.Add(new KeyBinding(new ImageChangeManager.UndoCommand(), Key.Z, ModifierKeys.Control));
            InputBindings.Add(new KeyBinding(new ImageChangeManager.RedoCommand(), Key.Y, ModifierKeys.Control));

            editMenu.AddOption("Redo").SetKeyboardShortcut("Strg + Y");

            viewMenu.AddOption("Zoom").SetKeyboardShortcut("Bla bla");

            settingsMenu.AddOption("Change Layout").SetKeyboardShortcut("Strg + LShift + L");

            WindowHandle.ActivateAllClientButtons();

            WindowHandle.ApplicationButtons.ActivateExitButtonSprite();
            WindowHandle.ApplicationButtons.ExitButtonImageSource = UIManager.ExitIconPath;
            WindowHandle.ApplicationButtons.ExitButtonImagePadding = UIManager.IconPadding;

            WindowHandle.ApplicationButtons.ActivateMaximizeButtonSprite();
            WindowHandle.ApplicationButtons.MaximizeButtonImageSource = UIManager.MaximizeIconPath;
            WindowHandle.ApplicationButtons.MaximizeButtonImageSourceWhenMaximized = UIManager.MaximizeIconPath;
            WindowHandle.ApplicationButtons.MaximizeButtonImageSourceWhenWindowed = UIManager.WindowedIconPath;
            WindowHandle.ApplicationButtons.MaximizeButtonImagePadding = UIManager.IconPadding;

            WindowHandle.ApplicationButtons.ActivateMinimizeButtonSprite();
            WindowHandle.ApplicationButtons.MinimizeButtonImageSource = UIManager.MinimizeIconPath;
            WindowHandle.ApplicationButtons.MinimizeButtonImagePadding = UIManager.IconPadding;

            WindowHandle.ApplicationButtons.AddFullcreenButton();
            WindowHandle.ApplicationButtons.FullscreenButtonImageSource = UIManager.FullscreenIconPath;
            WindowHandle.ApplicationButtons.FullscreenButtonImagePadding = UIManager.IconPadding;

            WindowHandle.ApplicationButtons.AddSettingsButton();
            WindowHandle.ApplicationButtons.SettingsButtonImageSource = UIManager.SettingsIconPath;/// Properties.Resources.settings_24;
            WindowHandle.ApplicationButtons.SettingsButtonContentPadding = new Thickness(3);
            WindowHandle.ApplicationButtons.OverrideSettings(WindowManager.ToggleSettings);
        }

    }
}
