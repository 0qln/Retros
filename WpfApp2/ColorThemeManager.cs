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
using Utillities.Wpf;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace Retros {
    public class ColorThemeManager {
        public List<string> ThemeNames { get; } = new();
        public List<IColorTheme> ColorThemes { get; } = new();

        public IColorTheme Current => currentTheme;
        private IColorTheme currentTheme;

        public ColorThemeManager(IColorTheme colorTheme) {
            currentTheme = colorTheme;
            ThemeChanged += () => UpdateColors();

            // Create lists of the themes
            var ThemeList = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => (
                    typeof(IColorTheme).IsAssignableFrom(type) && 
                    !type.IsInterface && 
                    type.Namespace == "Retros.ColorThemes")
                );

            ColorThemes.Add(colorTheme);
            ThemeList.ToList().ForEach(type => { 
                ThemeNames.Add(type.Name); 
                if (colorTheme.GetType() != type) {
                    ColorThemes.Add(Activator.CreateInstance(type) as IColorTheme); 
                }
            });
        }

        public void SetTheme(IColorTheme colorTheme) {
            currentTheme = colorTheme;
            ThemeChanged?.Invoke();

            if (!ThemeNames.Contains(colorTheme.Name)) {
                ThemeNames.Add(colorTheme.Name);
                ColorThemes.Add(colorTheme);
            }
        }

        public void SaveToFile(string folderPath) {
            if (!Path.Exists(folderPath)) return;

            var json = Json.Serialize(Current);
            string filePath = folderPath + "\\style.json";
            int i = 0;

            while (File.Exists(filePath)) {
                i++;
                filePath = folderPath + "\\style" + i.ToString() + ".json" ;
            }

            try { File.WriteAllText(filePath, json); }
            catch (Exception ex) { 
                ///DebugLibrary.Console.Log(ex);
            }
        }
        public void LoadFromFile(string filePath) {
            filePath = filePath.Replace("\"", "");

            if (!File.Exists(filePath)) {
                return;
            }
            IColorTheme ct = Json.Deserialize(File.ReadAllText(filePath))!;
            if (ct == null) {
                return;
            }
            SetTheme(ct);
        }

        private void UpdateColors() {
            FC1_Changed?.Invoke(Current.FC1);
            FC2_Changed?.Invoke(Current.FC2);
            BG1_Changed?.Invoke(Current.BG1);
            BG2_Changed?.Invoke(Current.BG2);
            BG3_Changed?.Invoke(Current.BG3);
            BG4_Changed?.Invoke(Current.BG4);
            BG5_Changed?.Invoke(Current.BG5);
            BG6_Changed?.Invoke(Current.BG6);
            BGh1_Changed?.Invoke(Current.BGh1);
            BGh2_Changed?.Invoke(Current.BGh2);
            BGh3_Changed?.Invoke(Current.BGh3);
            BC1_Changed?.Invoke(Current.BC1);
            BC2_Changed?.Invoke(Current.BC2);
            BC3_Changed?.Invoke(Current.BC3);
            BCh1_Changed?.Invoke(Current.BCh1);
            AC1_Changed?.Invoke(Current.AC1);

        }

        private void SetHandler(Action<Brush> handler, ref Action<Brush>? colorEvent) {
            colorEvent += handler;
            UpdateColors();
        }

        public delegate Style DStyle();
        public void SetStyle(FrameworkElement element, DStyle style) {
            element.Style = style();
            ThemeChanged += () => { element.Style = style(); };
        }
        public static class Styles {
            public static Style SettingDetailTextbox() {
                throw new NotImplementedException();
            }
        }

        public event Action? ThemeChanged;
        
        private event Action<Brush>? FC1_Changed;
        private event Action<Brush>? FC2_Changed;
        private event Action<Brush>? BG1_Changed;
        private event Action<Brush>? BG2_Changed;
        private event Action<Brush>? BG3_Changed;
        private event Action<Brush>? BG4_Changed;
        private event Action<Brush>? BG5_Changed;
        private event Action<Brush>? BG6_Changed;
        private event Action<Brush>? BGh1_Changed;
        private event Action<Brush>? BGh2_Changed;
        private event Action<Brush>? BGh3_Changed;
        private event Action<Brush>? BC1_Changed;
        private event Action<Brush>? BC2_Changed;
        private event Action<Brush>? BC3_Changed;
        private event Action<Brush>? BCh1_Changed;
        private event Action<Brush>? AC1_Changed;

        public void Set_FC1(Action<Brush> handler) => SetHandler(handler, ref FC1_Changed);
        public void Set_FC2(Action<Brush> handler) => SetHandler(handler, ref FC2_Changed);
        public void Set_BG1(Action<Brush> handler) => SetHandler(handler, ref BG1_Changed);
        public void Set_BG2(Action<Brush> handler) => SetHandler(handler, ref BG2_Changed);
        public void Set_BG3(Action<Brush> handler) => SetHandler(handler, ref BG3_Changed);
        public void Set_BG4(Action<Brush> handler) => SetHandler(handler, ref BG4_Changed);
        public void Set_BG5(Action<Brush> handler) => SetHandler(handler, ref BG5_Changed);
        public void Set_BG6(Action<Brush> handler) => SetHandler(handler, ref BG6_Changed);
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
                    return new Deserializable(JsonSerializer.Deserialize<Serializable>(json)!);
                }
                catch (Exception ex) {
                    //DebugLibrary.Console.Log(ex);
                    return null;
                }
            }

            public class Serializable {
                [JsonInclude] public string Name { get; }
                [JsonInclude] public string FC1 { get; set; }
                [JsonInclude] public string FC2 { get; set; }
                [JsonInclude] public string BG1 { get; set; }
                [JsonInclude] public string BG2 { get; set; }
                [JsonInclude] public string BG3 { get; set; }
                [JsonInclude] public string BG4 { get; set; }
                [JsonInclude] public string BG5 { get; set; }
                [JsonInclude] public string BG6 { get; set; }
                [JsonInclude] public string AC1 { get; set; }
                [JsonInclude] public string BGh1 { get; set; }
                [JsonInclude] public string BGh2 { get; set; }
                [JsonInclude] public string BGh3 { get; set; }
                [JsonInclude] public string BC1 { get; set; }
                [JsonInclude] public string BC2 { get; set; }
                [JsonInclude] public string BC3 { get; set; }
                [JsonInclude] public string BCh1 { get; set; }
                [JsonInclude] public string BCh2 { get; set; }
                [JsonInclude] public string BCh3 { get; set; }

                public Serializable(IColorTheme colorTheme) {
                    Name = colorTheme.Name;
                    FC1 = colorTheme.FC1.ToString();
                    FC2 = colorTheme.FC2.ToString();
                    BG1 = colorTheme.BG1.ToString();
                    BG2 = colorTheme.BG2.ToString();
                    BG3 = colorTheme.BG3.ToString();
                    BG4 = colorTheme.BG4.ToString();
                    BG5 = colorTheme.BG5.ToString();
                    BG6 = colorTheme.BG6.ToString();
                    AC1 = colorTheme.AC1.ToString();
                    BGh1 = colorTheme.BGh1.ToString();
                    BGh2 = colorTheme.BGh2.ToString();
                    BGh3 = colorTheme.BGh3.ToString();
                    BC1 = colorTheme.BC1.ToString();
                    BC2 = colorTheme.BC2.ToString();
                    BC3 = colorTheme.BC3.ToString();
                    BCh1 = colorTheme.BCh1.ToString();
                    BCh2 = colorTheme.BCh2.ToString();
                    BCh3 = colorTheme.BCh3.ToString();
                }

                [JsonConstructor]
                public Serializable(
                    string name,
                    string fC1, string fC2, string bG1, string bG2, string bG3, string bG4, string bG5, string bG6, string aC1, string bGh1,
                    string bGh2, string bGh3, string bC1, string bC2, string bC3, string bCh1, string bCh2, string bCh3) {
                    Name = name;
                    FC1 = fC1;
                    FC2 = fC2;
                    BG1 = bG1;
                    BG2 = bG2;
                    BG3 = bG3;
                    BG4 = bG4;
                    BG5 = bG5;
                    BG6 = bG6;
                    AC1 = aC1;
                    BGh1 = bGh1;
                    BGh2 = bGh2;
                    BGh3 = bGh3;
                    BC1 = bC1;
                    BC2 = bC2;
                    BC3 = bC3;
                    BCh1 = bCh1;
                    BCh2 = bCh2;
                    BCh3 = bCh3;
                }
            }
            public class Deserializable : IColorTheme {
                public string Name { get; }
                public Brush FC1 { get; }
                public Brush FC2 { get; }
                public Brush BG1 { get; }
                public Brush BG2 { get; }
                public Brush BG3 { get; }
                public Brush BG4 { get; }
                public Brush BG5 { get; }
                public Brush BG6 { get; }
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
                    Name = serializable.Name;
                    FC1 = Helper.StringToBrush(serializable.FC1);
                    FC2 = Helper.StringToBrush(serializable.FC2);
                    BG1 = Helper.StringToBrush(serializable.BG1);
                    BG2 = Helper.StringToBrush(serializable.BG2);
                    BG3 = Helper.StringToBrush(serializable.BG3);
                    BG4 = Helper.StringToBrush(serializable.BG4);
                    BG5 = Helper.StringToBrush(serializable.BG5);
                    BG6 = Helper.StringToBrush(serializable.BG6);
                    AC1 = Helper.StringToBrush(serializable.AC1);
                    BGh1 = Helper.StringToBrush(serializable.BGh1);
                    BGh2 = Helper.StringToBrush(serializable.BGh2);
                    BGh3 = Helper.StringToBrush(serializable.BGh3);
                    BC1 = Helper.StringToBrush(serializable.BC1);
                    BC2 = Helper.StringToBrush(serializable.BC2);
                    BC3 = Helper.StringToBrush(serializable.BC3);
                    BCh1 = Helper.StringToBrush(serializable.BCh1);
                    BCh2 = Helper.StringToBrush(serializable.BCh2);
                    BCh3 = Helper.StringToBrush(serializable.BCh3);
                }
            }
        }
    }

}
