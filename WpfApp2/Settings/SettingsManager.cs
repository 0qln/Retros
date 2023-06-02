using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retros.Settings {
    public static class SettingsManager {
        public delegate void hWorkstationImageShadow(bool enabled);
        public static event hWorkstationImageShadow? WorkstationImageShadow;
        public static void InvokeorkstationImageShadow(bool enabled) => WorkstationImageShadow?.Invoke(enabled);
    }
}
