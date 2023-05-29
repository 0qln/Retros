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
    public static class Json {
        public static string Serialize(IColorTheme colorTheme) {
            return JsonSerializer.Serialize(new Serializable(colorTheme));
        }
        public static IColorTheme Deserialize(string json) {

            var c = JsonSerializer.Deserialize<Serializable>(json);

            Deserializable ds = new(c);


            return ds;
        }

        public class Serializable {

            [JsonInclude]
            public List<string> colors;

            public Serializable(IColorTheme colorTheme) {
                colors = new() {
                    colorTheme.BG1.ToString(),
                    colorTheme.BG2.ToString(),
                    colorTheme.BG3.ToString(),
                    colorTheme.BG4.ToString(),
                    colorTheme.BG5.ToString(),
                    colorTheme.AC1.ToString(),
                    colorTheme.BGh1.ToString(),
                    colorTheme.BGh2.ToString(),
                    colorTheme.BGh3.ToString(),
                    colorTheme.BC1.ToString(),
                    colorTheme.BC2.ToString(),
                    colorTheme.BC3.ToString(),
                    colorTheme.BCh1.ToString(),
                    colorTheme.BCh2.ToString(),
                    colorTheme.BCh3.ToString(),
                };
            }

            [JsonConstructorAttribute]
            public Serializable(List<string> colors) {
                this.colors = colors;
            }



            /*
            [JsonConstructorAttribute]
            public Serializable(
                string BG1,
                string BG2,
                string BG3,
                string BG4,
                string BG5,
                string AC1,
                string BGh1,
                string BGh2,
                string BGh3,
                string BCh1,
                string BCh2,
                string BCh3) {
                colors = new() {
                    BG1,
                    BG2,
                    BG3,
                    BG4,
                    BG5,
                    AC1,
                    BGh1,
                    BGh2,
                    BGh3,
                    BCh1,
                    BCh2,
                    BCh3
                };
            }
            */
        }

        public class Deserializable : IColorTheme {
            public Brush BG1 { get; }
            public Brush BG2 { get; }
            public Brush BG3 { get; }
            public Brush BG4 { get; }
            public Brush BG5 { get; }
            public Brush AC1 { get; }
            public Brush BGh1 { get; }
            public Brush BGh2 { get; }
            public Brush BGh3 { get; }
            public Brush BC1 { get; }
            public Brush BC2 { get; }
            public Brush BC3 { get; }
            public Brush BCh1 { get; }
            public Brush BCh2 { get; }
            public Brush BCh3 { get; }

            public Deserializable(Serializable serializable) {
                BG1 = StringToBrush(serializable.colors[0]);
                BG2 = StringToBrush(serializable.colors[1]);
                BG3 = StringToBrush(serializable.colors[2]);
                BG4 = StringToBrush(serializable.colors[3]);
                BG5 = StringToBrush(serializable.colors[4]);
                AC1 = StringToBrush(serializable.colors[5]);
                BGh1 = StringToBrush(serializable.colors[6]);
                BGh2 = StringToBrush(serializable.colors[7]);
                BGh3 = StringToBrush(serializable.colors[8]);
                BC1 = StringToBrush(serializable.colors[9]);
                BC2 = StringToBrush(serializable.colors[10]);
                BC3 = StringToBrush(serializable.colors[11]);
                BCh1 = StringToBrush(serializable.colors[12]);
                BCh2 = StringToBrush(serializable.colors[13]);
                BCh3 = StringToBrush(serializable.colors[14]);
            }

            public static Brush StringToBrush(string colorString) {
                colorString = colorString.Substring(1, 8); 

                if (colorString.Length != 8) {
                    throw new ArgumentException("Invalid color string. Expected format: transparency, r, g, b");
                }

                byte transparency = byte.Parse(colorString.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                byte red = byte.Parse(colorString.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                byte green = byte.Parse(colorString.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                byte blue = byte.Parse(colorString.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

                return new SolidColorBrush(Color.FromArgb(transparency, red, green, blue));
            }
        }
    }


    public class DefaultDark: IColorTheme {
        public Brush BG1 { get; } = Helper.StringToSolidColorBrush("#1f1f1f", 1);
        public Brush BG2 { get; } = Helper.StringToSolidColorBrush("#2e2e2e", 1);
        public Brush BG3 { get; } = Helper.StringToSolidColorBrush("#000000", 0.45);
        public Brush BG4 { get; } = Helper.StringToSolidColorBrush("#252525", 1);
        public Brush BG5 { get; } = Helper.StringToSolidColorBrush("#000000", 0.3);

        public Brush AC1 { get; } = Helper.StringToSolidColorBrush("#bb1f1f", 1);

        public Brush BGh1 { get; } = Helper.StringToSolidColorBrush("#FFFFFF", 0.1);
        public Brush BGh2 { get; } = Helper.StringToSolidColorBrush("#000000", 0.4);
        public Brush BGh3 { get; } = Helper.StringToSolidColorBrush("#000000", 0.1);

        public Brush BC1 { get; } = Helper.StringToSolidColorBrush("#3d3d3d", 1);
        public Brush BC2 { get; } = Helper.StringToSolidColorBrush("#FFFFFF", 0);
        public Brush BC3 { get; } = Helper.StringToSolidColorBrush("#000000", 0);

        public Brush BCh1 { get; } = Helper.StringToSolidColorBrush("#000000", 0);
        public Brush BCh2 { get; } = Helper.StringToSolidColorBrush("#000000", 0);
        public Brush BCh3 { get; } = Helper.StringToSolidColorBrush("#000000", 0);
    }

    public class Test: IColorTheme {
        public Brush BG1 { get; } = Helper.StringToSolidColorBrush("#FF0000", 1);
        public Brush BG2 { get; } = Helper.StringToSolidColorBrush("#00FF00", 1);
        public Brush BG3 { get; } = Helper.StringToSolidColorBrush("#000000", 1);
        public Brush BG4 { get; } = Helper.StringToSolidColorBrush("#252525", 1);
        public Brush BG5 { get; } = Helper.StringToSolidColorBrush("#2F2525", 1);

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
