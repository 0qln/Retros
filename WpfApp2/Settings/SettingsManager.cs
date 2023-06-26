using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Retros.Settings {
    public static class SettingsManager {

        public static class WorkstationImageShadow {
            public static Setting<bool> Enabled = new (true);
            public static Setting<double> BlurRadius = new Setting<double>(50);
            public static Setting<double> ShadowDepth;
            public static Setting<double> Opacity;
            public static Setting<double> Direction;
        }

        public static class ImageHistory {
            public static Setting<bool> ShowFilterIntensityInName = new Setting<bool>(true);
            public static Setting<bool> CompactNodeLayout = new Setting<bool>(true);
            public static Setting<double> CompactNodeMaxWidth = new Setting<double>(30);
        }

        public static class SettingsPages {
            public static Setting<double> TextBlockHeight_Normal = new Setting<double>(18);
            public static Setting<double> TextBlockFontSize_Normal = new(14);
            public static Setting<double> LineMarginLeft = new(5);
            public static Setting<double> LineMarginTop = new(5);
        }


        public class Setting<T>
        {
            public event Action<T>? ValueChanged;
            public T Value
            {
                set
                {
                    _value = value;
                    ValueChanged?.Invoke(value);
                }
                get
                {
                    return _value;
                }
            }

            private T _value;


            public Setting(T startValue)
            {
                _value = startValue;
            }
        }
    }
}
