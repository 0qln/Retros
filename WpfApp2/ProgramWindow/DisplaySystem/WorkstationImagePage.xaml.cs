using Retros.Settings;
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

namespace Retros.ProgramWindow.DisplaySystem {
    public partial class WorkstationImagePage : Page {

        public DropShadowEffect ImageEffect;

        
        public WorkstationImagePage() {
            InitializeComponent();

            ImageEffect = new DropShadowEffect { BlurRadius = 30, ShadowDepth = 15, Color = Colors.Black, Opacity = 0.8, Direction = 270 };
            SettingsManager.WorkstationImageShadow.BlurRadius += (value) => ImageEffect.BlurRadius = value;
            SettingsManager.WorkstationImageShadow.ShadowDepth += (value) => ImageEffect.ShadowDepth = value;
            SettingsManager.WorkstationImageShadow.Opacity += (value) => ImageEffect.Opacity = value;
            SettingsManager.WorkstationImageShadow.Direction += (value) => ImageEffect.Direction = value;
            SettingsManager.WorkstationImageShadow.Enabled += (enabled) => {
                if (enabled) Image.Effect = ImageEffect;
                else Image.Effect = null;
            };
        }

        public void SetImage(Image image) {
            Image = image;
        }


    }
}
