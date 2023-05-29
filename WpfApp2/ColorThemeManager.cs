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

        public void SetTheme(IColorTheme colorTheme) {
            currentTheme = colorTheme;
            UpdateColors();
        }

        private void UpdateColors() {
            BG1_Changed?.Invoke(CurrentTheme.BG1);
            BG2_Changed?.Invoke(CurrentTheme.BG2);
            BG3_Changed?.Invoke(CurrentTheme.BG3);
            BG4_Changed?.Invoke(CurrentTheme.BG4);
            BG5_Changed?.Invoke(CurrentTheme.BG5);
            BGh1_Changed?.Invoke(CurrentTheme.BGh1);
            BGh2_Changed?.Invoke(CurrentTheme.BGh2);
            BGh3_Changed?.Invoke(CurrentTheme.BGh3);
            BC1_Changed?.Invoke(CurrentTheme.BC1);
            BC2_Changed?.Invoke(CurrentTheme.BC2);
            BC3_Changed?.Invoke(CurrentTheme.BC3);
            BCh1_Changed?.Invoke(CurrentTheme.BCh1);
            AC1_Changed?.Invoke(CurrentTheme.AC1);

        }

        private void SetHandler(Action<Brush> handler, ref Action<Brush>? colorEvent) {
            colorEvent += handler;
            UpdateColors();
        }

        private event Action<Brush>? BG1_Changed;
        private event Action<Brush>? BG2_Changed;
        private event Action<Brush>? BG3_Changed;
        private event Action<Brush>? BG4_Changed;
        private event Action<Brush>? BG5_Changed;
        private event Action<Brush>? BGh1_Changed;
        private event Action<Brush>? BGh2_Changed;
        private event Action<Brush>? BGh3_Changed;
        private event Action<Brush>? BC1_Changed;
        private event Action<Brush>? BC2_Changed;
        private event Action<Brush>? BC3_Changed;
        private event Action<Brush>? BCh1_Changed;
        private event Action<Brush>? AC1_Changed;

        public void Set_BG1(Action<Brush> handler) => SetHandler(handler, ref BG1_Changed);
        public void Set_BG2(Action<Brush> handler) => SetHandler(handler, ref BG2_Changed);
        public void Set_BG3(Action<Brush> handler) => SetHandler(handler, ref BG3_Changed);
        public void Set_BG4(Action<Brush> handler) => SetHandler(handler, ref BG4_Changed);
        public void Set_BG5(Action<Brush> handler) => SetHandler(handler, ref BG5_Changed);
        public void Set_BGh1(Action<Brush> handler) => SetHandler(handler, ref BGh1_Changed);
        public void Set_BGh2(Action<Brush> handler) => SetHandler(handler, ref BGh2_Changed);
        public void Set_BGh3(Action<Brush> handler) => SetHandler(handler, ref BGh3_Changed);
        public void Set_BC1(Action<Brush> handler) => SetHandler(handler, ref BC1_Changed);
        public void Set_BC2(Action<Brush> handler) => SetHandler(handler, ref BC2_Changed);
        public void Set_BC3(Action<Brush> handler) => SetHandler(handler, ref BC3_Changed);
        public void Set_BCh1(Action<Brush> handler) => SetHandler(handler, ref BCh1_Changed);
        public void Set_AC1(Action<Brush> handler) => SetHandler(handler, ref AC1_Changed);
    }

}
