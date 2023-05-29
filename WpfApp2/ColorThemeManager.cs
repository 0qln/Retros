using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;

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

        public void SaveToFile(string path) {
            var json = Json.Serialize(UIManager.ColorThemeManager.CurrentTheme);
            if (!Path.Exists(path)) {
                return;
            }

            string filePath = path + "\\style.json";
            int i = 0;
            while (File.Exists(filePath)) {
                i++;
                filePath = path + "\\style" + i.ToString() + ".json" ;
            }
            try { File.WriteAllText(filePath, json); }
            catch (Exception ex) { Debugger.Console.Log(ex);}
        }
        public void LoadFromFile(string path) {
            if (!File.Exists(path)) {
                return;
            }
            IColorTheme ct = Json.Deserialize(File.ReadAllText(path))!;
            if (ct == null) {
                return;
            }
            SetTheme(ct);
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


        public static class Json {
            public static string Serialize(IColorTheme colorTheme) => JsonSerializer.Serialize(new Serializable(colorTheme));
            public static IColorTheme? Deserialize(string json) {
                try {
                    return new Deserializable(JsonSerializer.Deserialize<Serializable>(json));
                }
                catch (Exception ex) {
                    Debugger.Console.Log(ex);
                    return null;
                }
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
    }

}
