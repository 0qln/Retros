using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Retros.Settings {
    public static class SettingsManager {

        public static class WorkstationImageShadow {
            public static event Action<bool>? Enabled;
            public static void InvokeEnabled(bool enabled) => Enabled?.Invoke(enabled);

            public static event Action<double>? BlurRadius;
            public static void InvokeBlurRadius(double value) => BlurRadius?.Invoke(value);

            public static event Action<double>? ShadowDepth;
            public static void InvokeShadowDepth(double value) => ShadowDepth?.Invoke(value);

            public static event Action<double>? Opacity;
            public static void InvokeOpacity(double value) => Opacity?.Invoke(value);

            public static event Action<double>? Direction;
            public static void InvokeDirection(double value) => Direction?.Invoke(value);
        }

        public static class ImageHistory {
            public static bool ShowFilterIntensityInNameValue = true;
            public static event Action<bool>? ShowFilterIntensityInName;
            public static void InvokeShowFilterIntensityInName(bool value) {
                ShowFilterIntensityInNameValue = value;
                ShowFilterIntensityInName?.Invoke(value);
            }

            public static bool CompactNodeLayoutValue = true;
            public static event Action<bool>? CompactNodeLayout;
            public static void InvokeCompactNodeLayout(bool value) {
                CompactNodeLayoutValue = value;
                CompactNodeLayout?.Invoke(value);
            }

            public static double CompactNodeMaxWidthValue = 30;
            public static event Action<double>? CompactNodeMaxWidth;
            public static void InvokeCompactNodeMaxWidth(double value) {
                CompactNodeMaxWidthValue = value;
                CompactNodeMaxWidth?.Invoke(value);
            }
        }

        public static class SettingsPages {

            public static double TextBlockHeight_Normal = 18;
            public static double TextBlockFontSize_Normal = 14;
            public static double LineMarginLeft = 5;

            public static double LineMarginTopValue = 5;
            public static event Action? LineMarginTop;
            public static void InvokeLineMarginTop(double value) {
                LineMarginTopValue = value;
                LineMarginTop?.Invoke();
            }
        }

    }
}
