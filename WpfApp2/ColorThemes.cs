using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Utillities.Wpf;

namespace Retros.ColorThemes {
    public class DefaultDark: IColorTheme {
        public Brush BG1 => Helper.StringToSolidColorBrush("#1f1f1f", 1);
        public Brush BG2 => Helper.StringToSolidColorBrush("#2e2e2e", 1);
        public Brush BG3 => Helper.StringToSolidColorBrush("#000000", 0.45);

        public Brush AC1 => Helper.StringToSolidColorBrush("#bb1f1f", 1);

        public Brush HC1 => Helper.StringToSolidColorBrush("#FFFFFF", 0.1);

        public Brush BC1 => Helper.StringToSolidColorBrush("#FFFFFF", 0);
        public Brush BC2 => Helper.StringToSolidColorBrush("#FFFFFF", 0);
    }

    public class Test: IColorTheme {
        public Brush BG1 => Helper.StringToSolidColorBrush("#FF0000", 1);
        public Brush BG2 => Helper.StringToSolidColorBrush("#00FF00", 1);
        public Brush BG3 => Helper.StringToSolidColorBrush("#000000", 1);

        public Brush AC1 => Helper.StringToSolidColorBrush("#FFFF00", 1);

        public Brush HC1 => Helper.StringToSolidColorBrush("#00FFFF", 1);


        public Brush BC1 => Helper.StringToSolidColorBrush("#FFFFFF", 0);
        public Brush BC2 => Helper.StringToSolidColorBrush("#FFFFFF", 0);
    }
}
