using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Retros {
    public class ColorThemeManager {
        public IColorTheme CurrentTheme => currentTheme;
        private IColorTheme currentTheme;

        public ColorThemeManager(IColorTheme colorTheme) {
            currentTheme = colorTheme;
        }

        public void Set(IColorTheme colorTheme) {
            currentTheme = colorTheme;
            UpdateColors();
        }

        private void UpdateColors() {
            BG1_Changed?.Invoke(CurrentTheme.BG1);
            BG2_Changed?.Invoke(CurrentTheme.BG2);
            BG3_Changed?.Invoke(CurrentTheme.BG3);
            BG4_Changed?.Invoke(CurrentTheme.BG4);
            AC1_Changed?.Invoke(CurrentTheme.AC1);
            HC1_Changed?.Invoke(CurrentTheme.HC1);
            HC2_Changed?.Invoke(CurrentTheme.HC2);
            BC1_Changed?.Invoke(CurrentTheme.BC1);
            BC2_Changed?.Invoke(CurrentTheme.BC2);
        }

        private event Action<Brush>? BG1_Changed;
        private event Action<Brush>? BG2_Changed;
        private event Action<Brush>? BG3_Changed;
        private event Action<Brush>? BG4_Changed;
        private event Action<Brush>? AC1_Changed;
        private event Action<Brush>? HC1_Changed;
        private event Action<Brush>? HC2_Changed;
        private event Action<Brush>? BC1_Changed;
        private event Action<Brush>? BC2_Changed;



        public void Set_BG1(Action<Brush> handler) {
            BG1_Changed += handler;
            UpdateColors();
        }
        public void Set_BG2(Action<Brush> handler) {
            BG2_Changed += handler;
            UpdateColors();
        }
        public void Set_BG3(Action<Brush> handler) {
            BG3_Changed += handler;
            UpdateColors();
        }
        public void Set_BG4(Action<Brush> handler) {
            BG4_Changed += handler;
            UpdateColors();
        }
        public void Set_AC1(Action<Brush> handler) {
            AC1_Changed += handler;
            UpdateColors();
        }
        public void Set_HC1(Action<Brush> handler) {
            HC1_Changed += handler;
            UpdateColors();
        }
        public void Set_HC2(Action<Brush> handler) {
            HC2_Changed += handler;
            UpdateColors();
        }
        public void Set_BC1(Action<Brush> handler) {
            BC1_Changed += handler;
            UpdateColors();
        }
        public void Set_BC2(Action<Brush> handler) {
            BC2_Changed += handler;
            UpdateColors();
        }
    }

}
