using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Utillities.Wpf;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.CodeDom;

namespace Retros.ColorThemes {
    public class DefaultDark: IColorTheme {
        public string Name { get; } = "DefaultDark";

        public Brush FC1 { get; } = Helper.StringToSolidColorBrush("#FFFFFF", 1);
        public Brush FC2 { get; } = Helper.StringToSolidColorBrush("#0078d4", 1);

        public Brush BG1 { get; } = Helper.StringToSolidColorBrush("#1f1f1f", 1);
        public Brush BG2 { get; } = Helper.StringToSolidColorBrush("#2e2e2e", 1);
        public Brush BG3 { get; } = Helper.StringToSolidColorBrush("#000000", 0.45);
        public Brush BG4 { get; } = Helper.StringToSolidColorBrush("#252525", 1);
        public Brush BG5 { get; } = Helper.StringToSolidColorBrush("#000000", 0.3);
        public Brush BG6 { get; } = Helper.StringToSolidColorBrush("#000000", 0);

        public Brush AC1 { get; } = Helper.StringToSolidColorBrush("#bb1f1f", 1);

        public Brush BGh1 { get; } = Helper.StringToSolidColorBrush("#FFFFFF", 0.1);
        public Brush BGh2 { get; } = Helper.StringToSolidColorBrush("#000000", 0.4);
        public Brush BGh3 { get; } = Helper.StringToSolidColorBrush("#000000", 0.1);

        public Brush BC1 { get; } = Helper.StringToSolidColorBrush("#3d3d3d", 1);
        public Brush BC2 { get; } = Helper.StringToSolidColorBrush("#999999", 1);
        public Brush BC3 { get; } = Helper.StringToSolidColorBrush("#000000", 0);

        public Brush BCh1 { get; } = Helper.StringToSolidColorBrush("#000000", 0);
        public Brush BCh2 { get; } = Helper.StringToSolidColorBrush("#000000", 0);
        public Brush BCh3 { get; } = Helper.StringToSolidColorBrush("#000000", 0);
    }

    public class Test: IColorTheme {
        public string Name { get; } = "Test";

        public Brush FC1 { get; } = Helper.StringToSolidColorBrush("#FFFFFF", 1);
        public Brush FC2 { get; } = Helper.StringToSolidColorBrush("#0078d4", 1);

        public Brush BG1 { get; } = Helper.StringToSolidColorBrush("#FF0000", 1);
        public Brush BG2 { get; } = Helper.StringToSolidColorBrush("#00FF00", 1);
        public Brush BG3 { get; } = Helper.StringToSolidColorBrush("#000000", 1);
        public Brush BG4 { get; } = Helper.StringToSolidColorBrush("#252525", 1);
        public Brush BG5 { get; } = Helper.StringToSolidColorBrush("#2F2525", 1);
        public Brush BG6 { get; } = Helper.StringToSolidColorBrush("#000000", 0);

        public Brush AC1 { get; } = Helper.StringToSolidColorBrush("#FFFF00", 1);

        public Brush BGh1 { get; } = Helper.StringToSolidColorBrush("#00FFFF", 1);
        public Brush BGh2 { get; } = Helper.StringToSolidColorBrush("#FFFFFF", 0.1);
        public Brush BGh3 { get; } = Helper.StringToSolidColorBrush("#000000", 0.1);

        public Brush BC1 { get; } = Helper.StringToSolidColorBrush("#FFFFFF", 1);
        public Brush BC2 { get; } = Helper.StringToSolidColorBrush("#FFFFFF", 1);
        public Brush BC3 { get; } = Helper.StringToSolidColorBrush("#FFFFFF", 1);

        public Brush BCh1 { get; } = Helper.StringToSolidColorBrush("#F0F0F0", 1);
        public Brush BCh2 { get; } = Helper.StringToSolidColorBrush("#000000", 0);
        public Brush BCh3 { get; } = Helper.StringToSolidColorBrush("#000000", 0);
    }
}
