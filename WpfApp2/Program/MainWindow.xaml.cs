using Retros.Program.Workstation.TabUI.Tabs;
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
using Retros.Program.Workstation;
using Retros.Program.Workstation.Changes;
using Retros;

namespace Retros.Program
{
    public partial class MainWindow : Window {

        // Window Handle
        private WindowHandle? _visualHandle;

        // UI
        public Workstation.Workstation SelectedWorkstation;
        public List<Workstation.Workstation> Workstations = new();


        public MainWindow() {
            // Initialize
            InitializeComponent();

            // Init window handle
            _visualHandle = new(this);
            
            // Start WindowManager
            WindowManager.Start(this);


            // UI
            Workstation.Workstation workstation = new(_visualHandle.Height);
            Workstations.Add(workstation);
            SelectedWorkstation = workstation;
            Helper.SetChildInGrid(ClientGrid, workstation.FrameworkElement, 1, 0);


            // Window handle
            _visualHandle.ClientButtons.Add(new ClientButton(
                "File",
                new PopupMenu(
                    new PopupMenuOption("Save").SetAction(() => UIManager.SaveImage(SelectedWorkstation.ImageElement)),
                    new PopupMenuOption("Open").SetAction(() => UIManager.LoadImage(SelectedWorkstation.ImageElement)))
                ));

            _visualHandle.ClientButtons.Add(new ClientButton(
                "View",
                new PopupMenu(
                    new PopupMenuOption("Zoom"))
                ));

            _visualHandle.ClientButtons.Add(new ClientButton(
                "Edit",
                new PopupMenu(
                    new PopupMenuOption("Save FIlter"),
                    new PopupMenuOption("Undo"),
                    new PopupMenuOption("Redo"))));


            _visualHandle.ClientButtons.SetWindowChromActiveAll(true);

            UIManager.ColorThemeManager.Set_BG3(b => _visualHandle.BackgroundColor = b);
            UIManager.ColorThemeManager.Set_BGh1(b => _visualHandle.ApplicationButtons.ColorWhenButtonHover = b);


            _visualHandle.ApplicationButtons.ActivateExitButtonSprite();
            _visualHandle.ApplicationButtons.ExitButtonImageSource = UIManager.ExitIconPath;
            _visualHandle.ApplicationButtons.ExitButtonImagePadding = UIManager.IconPadding;

            _visualHandle.ApplicationButtons.ActivateMaximizeButtonSprite();
            _visualHandle.ApplicationButtons.MaximizeButtonImageSource = UIManager.MaximizeIconPath;
            _visualHandle.ApplicationButtons.MaximizeButtonImageSourceWhenMaximized = UIManager.MaximizeIconPath;
            _visualHandle.ApplicationButtons.MaximizeButtonImageSourceWhenWindowed = UIManager.WindowedIconPath;
            _visualHandle.ApplicationButtons.MaximizeButtonImagePadding = UIManager.IconPadding;

            _visualHandle.ApplicationButtons.ActivateMinimizeButtonSprite();
            _visualHandle.ApplicationButtons.MinimizeButtonImageSource = UIManager.MinimizeIconPath;
            _visualHandle.ApplicationButtons.MinimizeButtonImagePadding = UIManager.IconPadding;

            _visualHandle.ApplicationButtons.AddFullcreenButton();
            _visualHandle.ApplicationButtons.FullscreenButtonImageSource = UIManager.FullscreenIconPath;
            _visualHandle.ApplicationButtons.FullscreenButtonImagePadding = UIManager.IconPadding;

            _visualHandle.ApplicationButtons.AddSettingsButton();
            _visualHandle.ApplicationButtons.SettingsButtonImageSource = UIManager.SettingsIconPath;/// Properties.Resources.settings_24;
            _visualHandle.ApplicationButtons.SettingsButtonContentPadding = new Thickness(3);
            _visualHandle.ApplicationButtons.OverrideSettings(WindowManager.ToggleSettings);
        }

    }
}
