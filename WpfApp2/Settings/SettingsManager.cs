using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retros.Settings {
    public static class SettingsManager {
        public delegate void BooleanHandler(bool enabled);
        public delegate void DoubleHandler(double value);

        public static class WorkstationImageShadow {
            public static event BooleanHandler? Enabled;
            public static void InvokeEnabled(bool enabled) => Enabled?.Invoke(enabled);

            public static event DoubleHandler? BlurRadius;
            public static void InvokeBlurRadius(double value) => BlurRadius?.Invoke(value);

            public static event DoubleHandler? ShadowDepth;
            public static void InvokeShadowDepth(double value) => ShadowDepth?.Invoke(value);

            public static event DoubleHandler? Opacity;
            public static void InvokeOpacity(double value) => Opacity?.Invoke(value);

            public static event DoubleHandler? Direction;
            public static void InvokeDirection(double value) => Direction?.Invoke(value);
        }

        public static class ImageHistory {
            public static bool ShowFilterIntensityInNameValue = true;
            public static event BooleanHandler? ShowFilterIntensityInName;
            public static void InvokeShowFilterIntensityInName(bool value) {
                ShowFilterIntensityInNameValue = value;
                ShowFilterIntensityInName?.Invoke(value);
            }
        }
    }
}
