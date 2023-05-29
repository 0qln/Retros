using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Retros {
    // 1 indexed
    public interface IColorTheme {
        // Background color
        public Brush BG1 { get; }
        public Brush BG2 { get; }
        public Brush BG3 { get; }
        public Brush BG4 { get; }
        public Brush BG5 { get; }

        // Accent color
        public Brush AC1 { get; }

        // Highlight color
        public Brush HC1 { get; } 
        public Brush HC2 { get; }

        // Border color
        public Brush BC1 { get; } 
        public Brush BC2 { get; }

    }
}
