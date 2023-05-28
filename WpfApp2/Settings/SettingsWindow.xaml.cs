﻿using Retros.Settings;
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
using Utillities.Wpf;
using Retros.Settings.Pages;

namespace Retros {
    public partial class SettingsWindow : Window {
        public static WindowHandle? WindowHandle;

        public SettingsWindow() {

            InitializeComponent();

            WindowHandle = new(this);
            WindowHandle.SetParentWindow(MainCanvas!);
            WindowHandle.SetBGColor(UIManager.Background_Colors[2]);
            WindowHandle.ApplicationButtons.ColorWhenButtonHover = UIManager.Highlight_Colors[0];
            WindowHandle.SetWindowChromeActiveAll();

            SettingsList_Padding.MinHeight = WindowHandle.Height;
            SettingDetailDisplay_Padding.MinHeight = WindowHandle.Height;

            SettingsList.Background = UIManager.Background_Colors[0];
            SettingDetailDisplaySP.Background = UIManager.Background_Colors[1];


            Loaded += (s, e) => {
                Tab appearance = new Tab("Appearance");
                appearance.AddDetail(new TabDetail("Color Theme", new ColorTheme()));

                Tab export = new Tab("Export");
                export.AddDetail(new TabDetail("Default Export Path", new DefaultExportPath()));
            };
            
        }

    }
}
