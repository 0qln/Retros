﻿using System;
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
        public Brush BackgroundColor { 
            set {
                MainCanvas.Background = value;
            } 
        }

        public SettingsWindow() {
            InitializeComponent();

            WindowHandle = new(this);
            WindowHandle.SetParentWindow(MainCanvas!);
            WindowHandle.SetBGColor(WindowManager.whBackground);
            WindowHandle.ApplicationButtons.ColorWhenButtonHover = WindowManager.whApplicationButtonHover;
            WindowHandle.SetWindowChromeActiveAll();

            WindowHandleRowDefinition.Height = new GridLength(WindowHandle.Height);

            SettingsList.Background = WindowManager.WorkStationImageGrid_BG;
            SettingDetails.Background = WindowManager.WorkStationGrid_BG;

        }


    }
}
